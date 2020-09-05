import { Injectable } from '@angular/core';
import { IdentityService } from './identity.service';
import { HttpClient, HttpParams, HttpHeaders } from '@angular/common/http';
import { map, catchError } from 'rxjs/operators';
import { UserBuilder } from '../model/builder/user.builder';

@Injectable()
export class UserService {
  private url = '/api/users/';

  constructor(private identityService: IdentityService,
    private httpClient: HttpClient) {

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
}
