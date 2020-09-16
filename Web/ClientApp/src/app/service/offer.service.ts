import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { MatSnackBar } from '@angular/material';
import { Router } from '@angular/router';

@Injectable()
export class OfferService {
  private offerUrl = `/api/offers/`;

  constructor(private httpClient: HttpClient,
    private snackBar: MatSnackBar,
    private router: Router) {

  }

  public getById(id: string) {
    return this.httpClient.get(`${this.offerUrl}${id}`);
  }

  public addOffer(offer) {
    this.httpClient.post(this.offerUrl, offer).subscribe(
      result => {
        this.snackBar.open('Utworzono ofertę');
        this.router.navigateByUrl(`/offer/${result}`);
      },
      () => {
        this.snackBar.open('Doszło do błędu przy tworzeniu oferty');
      });
  }
}
