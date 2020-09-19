import { Component, OnInit, ViewChild, ElementRef, AfterViewInit } from '@angular/core';
import { BaseComponent } from '../../BaseComponent';
import { OfferService } from '../../../service/offer.service';
import { ActivatedRoute } from '@angular/router';
import { Offer } from '../../../model/offer.model';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { IdentityService } from '../../../service/identity.service';
import { faBan } from '@fortawesome/free-solid-svg-icons';
import { MatDialog } from '@angular/material';
import { DialogComponent } from '../../dialog/dialog.component';

declare var paypal;

@Component({
  selector: 'app-offer-details',
  templateUrl: './offer-details.component.html',
  styleUrls: ['./offer-details.component.css']
})
export class OfferDetailsComponent extends BaseComponent implements OnInit, AfterViewInit {

  @ViewChild('paypal', { static: false }) paypalElement: ElementRef;

  faBan = faBan;
  private offer: Offer;
  private loading: boolean;
  private form: FormGroup;
  // CHANGE TO FALSE
  private canPromote = true;

  constructor(private offerService: OfferService,
    private route: ActivatedRoute,
    private formBuilder: FormBuilder,
    private identityService: IdentityService,
    private dialog: MatDialog) {
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

  ngAfterViewInit(): void {
    this.renderPayPalButton();
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

  private renderPayPalButton() {
    paypal.Buttons({
      createOrder: (data, actions) => {
        if (!this.offer || !this.identityService.isLoggedIn() || this.identityService.isBanned()) {
          return null;
        }
        if (this.identityService.user$.value.userName !== this.offer.authorUserName) {
          return actions.order.create({
            purchase_units: [
              {
                description: this.offer.title,
                amount: {
                  currency_code: 'PLN',
                  value: this.offer.price
                }
              }
            ]
          });
        } else if (this.canPromote) {
          return actions.order.create({
              purchase_units: [
                {
                  description: `Promocja oferty ${this.offer.title}`,
                  amount: {
                    currency_code: 'PLN',
                    value: this.offer.price
                  }
                }
              ]
            });
        }
      },
      onApprove: async (data, actions) => {
        if (this.identityService.user$.value.userName !== this.offer.authorUserName) {
          this.offerService.orderOffer(this.offer.id);
        } else if (this.canPromote) {

        }
      }
    }).render(this.paypalElement.nativeElement);
  }
}
