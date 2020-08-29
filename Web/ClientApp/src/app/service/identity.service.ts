import { Injectable } from "@angular/core";
import { Config } from "../config";
import { HttpClient, HttpParams, HttpHeaders } from "@angular/common/http";
import { BehaviorSubject } from "rxjs";
import { map, catchError } from "rxjs/operators";
import * as decodeJwt from "jwt-decode";
import { User } from "../model/user.model";
import { UserBuilder } from "../model/builder/user.builder";

@Injectable()
export class IdentityService {
  public user$ = new BehaviorSubject<User>(null);
  private accessToken$ = new BehaviorSubject<string>(localStorage.getItem(Config.localStorageAccessTokenKey));
  private accessTokenExpirationTimestamp: number;
  private refreshToken$ = new BehaviorSubject<string>(localStorage.getItem(Config.localStorageRefreshTokenKey));
  private refreshingToken = false;

  constructor(
    private httpClient: HttpClient
  ) {
    this.accessToken$.subscribe(token => {
      localStorage.setItem(Config.localStorageAccessTokenKey, token);
      if (token) {
        const decodedToken = decodeJwt(token);
        this.user$.next(new UserBuilder(this.user$.value).addDataFromToken(decodedToken).build());
        this.accessTokenExpirationTimestamp = Date.now() + (parseInt(decodedToken["exp"]) - parseInt(decodedToken["iat"])) * 1000;
      } else {
        this.accessTokenExpirationTimestamp = null;
      }
    });
    this.refreshToken$.subscribe(token => {
      localStorage.setItem(Config.localStorageRefreshTokenKey, token);
    });
  }

  public logIn(username: string, password: string) {
    const body = new HttpParams()
      .set("username", username)
      .set("password", password)
      .set("client_id", Config.clientId)
      .set("client_secret", Config.clientSecret)
      .set("scope", Config.clientScopes)
      .set("grant_type", "password");

    //TODO: Error handling.
    this.httpClient.post(Config.identityServerUrl + "connect/token",
      body.toString(), {
      headers: new HttpHeaders().set('Content-Type', 'application/x-www-form-urlencoded')
    }).subscribe(response => {
      this.accessToken$.next(response["access_token"]);
      this.refreshToken$.next(response["refresh_token"]);
    }, () => {
      this.accessToken$.next(null);
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
      .set("client_id", Config.clientId)
      .set("client_secret", Config.clientSecret)
      .set("grant_type", "refresh_token")
      .set("refresh_token", this.refreshToken$.value || "")
    return this.httpClient.post(Config.identityServerUrl + "connect/token",
      body.toString(), {
      headers: new HttpHeaders().set('Content-Type', 'application/x-www-form-urlencoded')
    }).pipe(
      map(response => {
        this.accessToken$.next(response["access_token"]);
        this.refreshToken$.next(response["refresh_token"]);
        this.refreshingToken = false;
        return response["access_token"];
      }),
      catchError(error => {
        this.accessToken$.next(null);
        this.refreshToken$.next(null);
        this.refreshingToken = false;
        return null;
      }));
  }
}
