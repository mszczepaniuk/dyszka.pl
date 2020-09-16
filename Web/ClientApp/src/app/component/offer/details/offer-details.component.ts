import { Component, OnInit } from '@angular/core';
import { BaseComponent } from '../../BaseComponent';
import { OfferService } from '../../../service/offer.service';
import { ActivatedRoute } from '@angular/router';
import { Offer } from '../../../model/offer.model';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';

@Component({
  selector: 'app-offer-details',
  templateUrl: './offer-details.component.html',
  styleUrls: ['./offer-details.component.css']
})
export class OfferDetailsComponent extends BaseComponent implements OnInit {
  private offer: Offer;
  private loading: boolean;
  private form: FormGroup;

  constructor(private offerService: OfferService,
    private route: ActivatedRoute,
    private formBuilder: FormBuilder) {
    super();
  }

  ngOnInit(): void {
    this.form = this.createForm();
    this.form.disable();
    this.loading = true;
    this.safeSub(
      this.route.paramMap.subscribe(params => {
        this.offerService.getById(params.get('id')).subscribe((result) => {
          this.offer = new Offer(result);
          this.form.patchValue(this.offer);
          this.loading = false;
        });
      }));
  }

  private createForm() {
    return this.formBuilder.group({
      title: [''],
      price: [''],
      tags: [''],
      description: ['']
    });
  }
}
