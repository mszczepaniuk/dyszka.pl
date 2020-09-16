import { Component, OnInit } from '@angular/core';
import { BaseComponent } from '../../BaseComponent';
import { OfferService } from '../../../service/offer.service';
import { ActivatedRoute } from '@angular/router';
import { Offer } from '../../../model/offer.model';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { IdentityService } from '../../../service/identity.service';
import { faBan } from '@fortawesome/free-solid-svg-icons';

@Component({
  selector: 'app-offer-details',
  templateUrl: './offer-details.component.html',
  styleUrls: ['./offer-details.component.css']
})
export class OfferDetailsComponent extends BaseComponent implements OnInit {
  faBan = faBan;
  private offer: Offer;
  private loading: boolean;
  private form: FormGroup;

  constructor(private offerService: OfferService,
    private route: ActivatedRoute,
    private formBuilder: FormBuilder,
    private identityService: IdentityService) {
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

  private isOwner() {
    return this.identityService.isLoggedIn() && this.identityService.user$.value.userName === this.offer.authorUserName;
  }

  private createForm() {
    return this.formBuilder.group({
      authorUserName: [''],
      title: [''],
      price: [''],
      tags: [''],
      description: ['']
    });
  }

  private showOffer() {
    this.offerService.showOffer(this.offer.id).subscribe(() => {
      this.offer.isHidden = false;
    });
  }

  private hideOffer() {
    this.offerService.hideOffer(this.offer.id).subscribe(() => {
      this.offer.isHidden = true;
    });;
  }
}
