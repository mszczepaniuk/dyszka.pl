import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { map, catchError } from 'rxjs/operators';
import { MatSnackBar } from '@angular/material';

@Injectable()
export class PaymentService
{
  private paymentsUrl = 'api/payments/';

  constructor(private httpClient: HttpClient,
    private snackBar: MatSnackBar) {

  }

  public getPaged(page: number) {
    return this.httpClient.get(`${this.paymentsUrl}${page}`);
  }

  public markAsDone(id: string) {
    return this.httpClient.put(`${this.paymentsUrl}${id}`, {}).pipe(map((result) => {
      this.snackBar.open('Zmieniono staus na wykonany', '', { duration: 2000 });
      return result;
    }), catchError((error) => {
      this.snackBar.open('Bład przy zmianie statusu', '', { duration: 2000 });
      return error;
    }));
  }
}
