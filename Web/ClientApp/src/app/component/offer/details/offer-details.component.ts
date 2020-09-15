import { Component, OnInit } from '@angular/core';
import { BaseComponent } from '../../BaseComponent';

@Component({
  selector: 'app-offer-details',
  templateUrl: './offer-details.component.html',
  styleUrls: ['./offer-details.component.css']
})
export class OfferDetailsComponent extends BaseComponent implements OnInit {

  constructor() {
    super();
  }

  ngOnInit(): void {
    throw new Error("Not implemented");
  }
}
