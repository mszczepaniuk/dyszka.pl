import { Component, OnInit, ViewChild, ElementRef } from '@angular/core';
import { BaseComponent } from '../../BaseComponent';
import { OfferService } from '../../../service/offer.service';
import { ActivatedRoute, Router } from '@angular/router';
import { Offer } from '../../../model/offer.model';
import { FormBuilder, FormGroup, FormControl } from '@angular/forms';
import { IdentityService } from '../../../service/identity.service';
import { faBan } from '@fortawesome/free-solid-svg-icons';
import { MatDialog } from '@angular/material';
import { DialogComponent } from '../../dialog/dialog.component';
import { OfferPromotionService } from '../../../service/offer-promotion.service';

declare var paypal;

@Component({
  selector: 'app-offer-details',
  templateUrl: './offer-details.component.html',
  styleUrls: ['./offer-details.component.css']
})
export class OfferDetailsComponent extends BaseComponent implements OnInit {

  @ViewChild('paypal', { static: false }) paypalElement: ElementRef;

  faBan = faBan;
  private offer: Offer;
  private loading: boolean;
  private form: FormGroup;
  private canPromote = false;
  private offerPromotionForm: FormGroup;
  private promoTags = [];
  private defaultPromo;

  constructor(private offerService: OfferService,
    private route: ActivatedRoute,
    private formBuilder: FormBuilder,
    private identityService: IdentityService,
    private dialog: MatDialog,
    private offerPromotionService: OfferPromotionService,
    private router: Router) {
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
          if (this.offer.authorUserName === this.identityService.user$.value.userName) {
            this.offerPromotionService.getTagsAvailableForPromotion(this.offer.id, this.offer.tags).subscribe(
              result => {
                if (Object.keys(result).length !== 0) {
                  this.renderPayPalButton();
                  Object.keys(result).forEach(tag => {
                    this.promoTags.push({ tag: tag, value: result[tag] });
                  });
                  this.defaultPromo = this.promoTags[0];
                  this.canPromote = true;
                  this.offerPromotionForm = this.createOfferPromotionForm();
                }
                this.loading = false;
              },
              error => {
                this.loading = false;
              });
          } else {
            this.renderPayPalButton();
            this.loading = false;
          }
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
                  value: this.offerPromotionForm.controls.promoTag.value.value.toFixed(2)
                }
              }
            ]
          });
        }
      },
      onApprove: (data, actions) => {
        if (this.identityService.user$.value.userName !== this.offer.authorUserName) {
          this.offerService.orderOffer(this.offer.id);
        } else if (this.canPromote) {
          this.offerPromotionService.promoteOffer(this.offer.id, this.offerPromotionForm.controls.promoTag.value.tag)
            .subscribe(
              result => {
                this.router.navigateByUrl("/");
              });
        }
      }
    }).render(this.paypalElement.nativeElement);
  }

  private createOfferPromotionForm() {
    return this.formBuilder.group({
      promoTag: new FormControl(this.promoTags[0]),
    });
  }

  private compareObjects(o1: any, o2: any): boolean {
    return o1.tag === o2.tag && o1.value === o2.value;
  }
}
