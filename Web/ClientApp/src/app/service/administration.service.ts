import { Injectable } from '@angular/core';
import { IdentityService } from './identity.service';
import { HttpClient, HttpParams, HttpHeaders } from '@angular/common/http';
import { map, catchError } from 'rxjs/operators';
import { UserBuilder } from '../model/builder/user.builder';
import { MatSnackBar } from '@angular/material';
import { Router } from '@angular/router';
import { User } from '../model/user.model';
import { BehaviorSubject } from 'rxjs';

@Injectable()
export class AdministrationService {
  public admins$ = new BehaviorSubject<User[]>([]);
  public moderators$ = new BehaviorSubject<User[]>([]);

  private usersUrl = '/api/users/';
  private auditLogsUrl = 'api/audit-logs/';

  constructor(private identityService: IdentityService,
    private httpClient: HttpClient,
    private snackBar: MatSnackBar,
    private router: Router) {
    this.refreshAdmins();
    this.refreshModerators();
  }

  public refreshAdmins() {
    this.httpClient.get(`${this.usersUrl}admins/all`).subscribe(
      response => {
        this.admins$.next(this.responseToUserList(response));
      });
  }

  public refreshModerators() {
    this.httpClient.get(`${this.usersUrl}moderators/all`).subscribe(
      response => {
        this.moderators$.next(this.responseToUserList(response));
      });
  }

  public getAuditLogs(page: number) {
    return this.httpClient.get(`${this.auditLogsUrl}${page}`);
  }

  private responseToUserList(response) {
    if (!response) {
      return [];
    }
    var users = [];
    response.forEach(user => {
      users.push(new UserBuilder().addApplicationData(user).build());
    });
    return users;
  }
}