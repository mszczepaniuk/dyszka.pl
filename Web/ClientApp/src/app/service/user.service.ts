import { Injectable } from '@angular/core';
import { IdentityService } from './identity.service';
import { HttpClient, HttpParams, HttpHeaders } from '@angular/common/http';
import { map, catchError } from 'rxjs/operators';
import { UserBuilder } from '../model/builder/user.builder';
import { MatSnackBar } from '@angular/material';

@Injectable()
export class UserService {
  private url = '/api/users/';

  constructor(private identityService: IdentityService,
    private httpClient: HttpClient,
    private snackBar: MatSnackBar) {

  }

  public getUserByUserName(username: string) {
    return this.httpClient.get(`${this.url}${username}`).pipe(map(result => {
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
    return this.httpClient.post(`${this.url}${this.identityService.user$.value.applicationId}`, data).pipe(map(result => {
      this.snackBar.open('Poprawnie edytowano użytkownika', '', { duration: 2000 });
      return result;
    }), catchError(error => {
      this.snackBar.open('Nie udało się edytować użytkownika', '', { duration: 2000 });
      return error;
    }));
  }
}
