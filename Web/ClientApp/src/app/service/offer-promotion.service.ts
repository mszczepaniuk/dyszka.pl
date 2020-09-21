import { Injectable } from '@angular/core';
import { MatSnackBar } from '@angular/material';
import { HttpClient } from '@angular/common/http';
import { map, catchError } from 'rxjs/operators';

@Injectable()
export class OfferPromotionService {

  private offerPromotionsUrl = 'api/offer-promotions/';

  constructor(private httpClient: HttpClient,
    private snackBar: MatSnackBar) {

  }

  public getTagsAvailableForPromotion(id: string, tags: string[]) {
    var url = `${this.offerPromotionsUrl}${id}?tags=${tags[0]}`;
    if (tags.length > 1) {
      for (let i = 0; i < tags.length; i++) {
        url = url.concat('&tags=' + tags[i]);
      }
    }
    return this.httpClient.get(url);
  }

  public promoteOffer(id: string, tag: string) {
    return this.httpClient.post(`${this.offerPromotionsUrl}${id}/${tag}`, {}).pipe(
      map((result) => {
        this.snackBar.open('Oferta zaczeła być promowana', '', { duration: 2000 });
        return result;
      }),
      catchError((error) => {
        this.snackBar.open('Blad podczas promowania oferty', '', { duration: 2000 });
        return error;
      }));
  }
}
