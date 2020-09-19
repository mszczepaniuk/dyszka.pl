import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { MatSnackBar } from '@angular/material';
import { Router } from '@angular/router';
import { map, catchError } from 'rxjs/operators';

@Injectable()
export class OrderService {

  private ordersUrl = 'api/orders/';

  constructor(private httpClient: HttpClient,
    private snackBar: MatSnackBar) {

  }

  public getCreatedByCurrentUser(page: number) {
    return this.httpClient.get(`${this.ordersUrl}user-orders/${page}`);
  }

  public getOrdersForCurrentUserOffers(page: number, done: boolean) {
    return this.httpClient.get(`${this.ordersUrl}user-offers/${page}/${done}`);
  }

  public markAsDone(id: string) {
    return this.httpClient.post(`${this.ordersUrl}${id}/done`, {}).pipe(map((result) => {
        this.snackBar.open('Zmieniono status na zrealizowany', '', { duration: 2000 });
        return result;
      }),
      catchError((error) => {
        this.snackBar.open('Blad podczas zmiany statusu', '', { duration: 2000 });
        return error;
      }));
  }
}
