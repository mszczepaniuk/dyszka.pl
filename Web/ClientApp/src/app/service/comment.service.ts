import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { MatSnackBar } from '@angular/material';
import { map, catchError } from 'rxjs/operators';

@Injectable()
export class CommentService {

  private commentsUrl = 'api/comments/';

  constructor(private httpClient: HttpClient,
    private snackBar: MatSnackBar) {

  }

  public getPagedAndFiltered(page: number, profileUserName?: string, offerId?: string) {
    var query = `${this.commentsUrl}?page=${page}`;
    if (profileUserName) {
      query = query.concat(`&profileUsername=${profileUserName}`);
    }
    if (offerId) {
      query = query.concat(`&offerId=${offerId}`);
    }
    return this.httpClient.get(query);
  }

  public addComment(comment) {
    return this.httpClient.post(this.commentsUrl, comment).pipe(
      map(result => {
        this.snackBar.open('Dodano komentarz', '', { duration: 2000 });
        return result;
      }),
      catchError(error => {
        this.snackBar.open('Błąd przy dodawaniu komentarza', '', { duration: 2000 });
        return error;
      }));
  }

  public deleteComment(id: string) {
    return this.httpClient.delete(this.commentsUrl + id).pipe(
      map(result => {
        this.snackBar.open('Usunięto komentarz', '', { duration: 2000 });
        return result;
      }),
      catchError(error => {
        this.snackBar.open('Błąd przy usuwaniu komentarza', '', { duration: 2000 });
        return error;
      }));
  }

  public setToPositive(id: string) {
    return this.httpClient.put(this.commentsUrl + id + '/toPositive', {}).pipe(
      map(result => {
        this.snackBar.open('Zmienio na pozytywny', '', { duration: 2000 });
        return result;
      }),
      catchError(error => {
        this.snackBar.open('Błąd przy zmianie na pozytywny', '', { duration: 2000 });
        return error;
      }));
  }
}
