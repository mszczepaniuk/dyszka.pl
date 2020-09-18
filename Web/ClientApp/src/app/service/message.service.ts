import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { MatSnackBar } from '@angular/material';
import { map, catchError } from 'rxjs/operators';
import { IdentityService } from './identity.service';

@Injectable()
export class MessageService {

  private messagesUrl = 'api/messages/';

  constructor(private httpClient: HttpClient,
    private snackBar: MatSnackBar,
    private identityService: IdentityService) {

  }

  public getMessages(page: number, username: string) {
    return this.httpClient.get(`${this.messagesUrl}${page}/${this.identityService.user$.value.userName}/${username}`);
  }

  public addMessage(message) {
    return this.httpClient.post(this.messagesUrl, message).pipe(map(result => result),
      catchError(error => {
        this.snackBar.open('Blad podczas wysylania wiadomosci', '', { duration: 2000 });
        return error;
      }));
  }
}
