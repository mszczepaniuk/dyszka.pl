import { Injectable } from '@angular/core';
import { Config } from '../config';
import { HttpClient, HttpParams, HttpHeaders } from '@angular/common/http';
import { BehaviorSubject } from 'rxjs';
import { map, catchError, skip } from 'rxjs/operators';
import * as decodeJwt from 'jwt-decode';
import { User } from '../model/user.model';
import { UserBuilder } from '../model/builder/user.builder';
import { Router } from '@angular/router';
import { MatSnackBar } from '@angular/material';

@Injectable()
export class IdentityService {
  public user$ = new BehaviorSubject<User>(null);
  private accessToken$ = new BehaviorSubject<string>(localStorage.getItem(Config.localStorageAccessTokenKey) || '');
  private accessTokenExpirationTimestamp: number;
  private refreshToken$ = new BehaviorSubject<string>(localStorage.getItem(Config.localStorageRefreshTokenKey) || '');
  private refreshingToken = false;

  constructor(
    private httpClient: HttpClient,
    private router: Router,
    private snackBar: MatSnackBar
  ) {
    this.user$.subscribe(user => {
      if (user && user.isBanned) {
        this.snackBar.open('ZALOGOWANY UŻYTKOWNIK JEST ZBANOWANY');
      }
    })
    this.accessToken$.subscribe(token => {
      localStorage.setItem(Config.localStorageAccessTokenKey, token);
      if (token) {
        const decodedToken = decodeJwt(token);
        this.user$.next(new UserBuilder(this.user$.value).addIdentityData(decodedToken).build());
        const lastUpdate = parseInt(localStorage.getItem(Config.localStorageLastUpdateKey)) || Date.now();
        this.accessTokenExpirationTimestamp = lastUpdate + (parseInt(decodedToken['exp']) - parseInt(decodedToken['iat'])) * 1000;
      } else {
        this.accessTokenExpirationTimestamp = null;
        this.user$.next(null);
        localStorage.setItem(Config.localStorageLastUpdateKey, '');
      }
    });
    this.accessToken$.pipe(skip(1)).subscribe(() => {
      localStorage.setItem(Config.localStorageLastUpdateKey, Date.now().toString());
    });
    this.refreshToken$.subscribe(token => {
      localStorage.setItem(Config.localStorageRefreshTokenKey, token);
    });
  }

  public logIn(username: string, password: string) {
    const body = new HttpParams()
      .set('username', username)
      .set('password', password)
      .set('client_id', Config.clientId)
      .set('client_secret', Config.clientSecret)
      .set('scope', Config.clientScopes)
      .set('grant_type', 'password');

    this.httpClient.post(Config.identityServerUrl + 'connect/token',
      body.toString(), {
      headers: new HttpHeaders().set('Content-Type', 'application/x-www-form-urlencoded')
    }).subscribe(response => {
      this.accessToken$.next(response['access_token']);
      this.refreshToken$.next(response['refresh_token']);
      this.router.navigateByUrl(`/profile/${username}`);
      this.snackBar.open('Zalogowano', '', { duration: 2000 });
    }, error => {
      this.accessToken$.next('');
      this.snackBar.open('Nie udało się zalogować', '', { duration: 2000 });
    });
  }

  public register(username: string, password: string, confirmPassword: string) {
    this.httpClient.post(Config.identityServerUrl + 'api/identity/register',
      {
        UserName: username,
        Password: password,
        ConfirmPassword: confirmPassword
      }).subscribe(response => {
        this.logIn(username, password);
      }, () => {
        this.snackBar.open('Doszło do błędu podczas rejestracji', '', { duration: 2000 });
      });
  }

  public getAccessToken() {
    return this.accessToken$.value;
  }

  public isTokenAvailable() {
    return !this.refreshingToken && this.accessToken$.value;
  }

  public isAccessTokenValid() {
    return this.accessTokenExpirationTimestamp && Date.now() < this.accessTokenExpirationTimestamp;
  }

  public refreshAccessToken() {
    this.refreshingToken = true;
    const body = new HttpParams()
      .set('client_id', Config.clientId)
      .set('client_secret', Config.clientSecret)
      .set('grant_type', 'refresh_token')
      .set('refresh_token', this.refreshToken$.value)
    return this.httpClient.post(Config.identityServerUrl + 'connect/token',
      body.toString(), {
      headers: new HttpHeaders().set('Content-Type', 'application/x-www-form-urlencoded')
    }).pipe(
      map(response => {
        this.accessToken$.next(response['access_token']);
        this.refreshToken$.next(response['refresh_token']);
        this.refreshingToken = false;
        return response['access_token'];
      }),
      catchError(error => {
        console.log("BLAD PODCZAS REFRESH");
        console.log(this.refreshToken$.value);
        console.log(this.accessToken$.value);
        this.accessToken$.next('');
        this.refreshToken$.next('');
        this.router.navigateByUrl('/');
        this.refreshingToken = false;
        return null;
      }));
  }

  public isLoggedIn() {
    return this.user$.value !== null;
  }

  public logout() {
    this.accessToken$.next('');
    this.user$.next(null);
    this.refreshToken$.next('');
    this.router.navigateByUrl('/');
    this.snackBar.open('Wylogowano', '', { duration: 2000 });
  }

  public isInRole(role: string) {
    return this.user$.value && this.user$.value.roles.some(r => r === role);
  }

  public isBanned() {
    return this.user$.value && this.user$.value.isBanned;
  }
}
