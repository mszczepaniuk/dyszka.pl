import { Injectable } from '@angular/core';
import { IdentityService } from './identity.service';
import { HttpClient, HttpParams, HttpHeaders } from '@angular/common/http';
import { map, catchError } from 'rxjs/operators';
import { UserBuilder } from '../model/builder/user.builder';
import { MatSnackBar } from '@angular/material';
import { Router } from '@angular/router';
import { BillingData } from '../model/billing-data.model';

@Injectable()
export class UserService {
  private url = '/api/users/';

  constructor(private identityService: IdentityService,
    private httpClient: HttpClient,
    private snackBar: MatSnackBar,
    private router: Router) {

  }

  public getUserByUserName(username: string) {
    return this.httpClient.get(`${this.url}${username}`).pipe(
      map(result => {
        if (result
          && result['userName']
          && this.identityService.user$.value && result['userName'] === this.identityService.user$.value.userName) {
          this.identityService.user$.next(new UserBuilder(this.identityService.user$.value).addApplicationData(result).build());
        }
        return result;
      }));
  }

  public getUserIdentityData(username: string) {
    return this.httpClient.get(`${this.url}identity/${username}`);
  }

  public editCurrentUser(data) {
    return this.httpClient.post(`${this.url}${this.identityService.user$.value.applicationId}`, data).pipe(
      map(result => {
        this.snackBar.open('Poprawnie edytowano użytkownika', '', { duration: 2000 });
        return result;
      }), catchError(error => {
        this.snackBar.open('Nie udało się edytować użytkownika', '', { duration: 2000 });
        return error;
      }));
  }

  public banUser(username: string) {
    return this.httpClient.put(`${this.url}ban/${username}`, {}).pipe(
      map(result => {
        this.snackBar.open('Zbanowano użytkownika', '', { duration: 2000 });
        return result;
      }),
      catchError(error => {
        this.snackBar.open('Błąd podczas banowania użytkownika', '', { duration: 2000 });
        return error;
      }));
  }

  public unbanUser(username: string) {
    return this.httpClient.put(`${this.url}unban/${username}`, {}).pipe(
      map(result => {
        this.snackBar.open('Odbanowano użytkownika', '', { duration: 2000 });
        return result;
      }),
      catchError(error => {
        this.snackBar.open('Błąd podczas odbanowania użytkownika', '', { duration: 2000 });
        return error;
      }));
  }

  public removeUser(id: string) {
    return this.httpClient.delete(`${this.url}${id}`).pipe(
      map(result => {
        this.snackBar.open('Usunięto użytkownika', '', { duration: 2000 });
        this.router.navigateByUrl('/');
        if (this.identityService.user$.value.applicationId === id) {
          this.identityService.logout();
        }
        return result;
      }),
      catchError(error => {
        this.snackBar.open('Bład podczas usuwania użytkownika', '', { duration: 2000 });
        return error;
      }));
  }

  public getBillingData(username: string) {
    return this.httpClient.get(`${this.url}${username}/billing-data`);
  }

  public postBillingData(billingData: BillingData) {
    return this.httpClient.post(`${this.url}billing-data`, billingData);
  }
}
