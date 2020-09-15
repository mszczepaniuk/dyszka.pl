import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';

@Injectable()
export class OfferService {
  private offerUrl = `/api/offers/`;

  constructor(private httpClient: HttpClient) {

  }

  public addOffer(offer) {
    this.httpClient.post(this.offerUrl, offer).subscribe(
      result => {
        console.log('wow');
      },
      () => {
        console.log('le');
      });
  }
}
