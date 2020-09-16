import { Component, OnInit } from '@angular/core';
import { IdentityService } from '../../service/identity.service';
import { OfferService } from '../../service/offer.service';
import { BaseComponent } from '../BaseComponent';
import { ActivatedRoute, Router, ParamMap } from '@angular/router';
import { Offer } from '../../model/offer.model';
import { BehaviorSubject } from 'rxjs';
import { faBackward, faForward} from '@fortawesome/free-solid-svg-icons';
import { PagedResult } from '../../model/paged-result.model';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent extends BaseComponent implements OnInit {
  faBackward = faBackward;
  faForward = faForward;

  public offers$: BehaviorSubject<Offer[]>;
  private currentPage: number = 1;
  private currentUsername: string;
  private currentTags: string[];
  private loading: boolean;
  private pagesCount: number;


  constructor(private identityService: IdentityService,
    private offerService: OfferService,
    private activatedroute: ActivatedRoute) {
    super();
  }

  ngOnInit(): void {
    this.offers$ = new BehaviorSubject<Offer[]>([]);
    this.safeSub(
      this.activatedroute.queryParams.subscribe(params => {
        this.currentUsername = params['username'];
        this.currentTags = params['tags'];
        if (this.currentTags && !Array.isArray(this.currentTags)) {
          this.currentTags = [this.currentTags as any];
        }
        this.safeSub(this.getPage(this.currentPage));
      }));
    this.offers$.subscribe(offers => console.log(offers));
  }

  private getPage(page: number) {
    this.loading = true;
    return this.offerService.getPaged(page, this.currentUsername, this.currentTags)
      .subscribe((result: PagedResult<Offer>) => {
        let offers = [];
        result.items.forEach(offer => {
          offers.push(new Offer(offer));
        });
        this.offers$.next(offers);
        this.currentPage = result.currentPage;
        this.pagesCount = result.pagesCount;
        this.loading = false;
      }, () => {
        this.loading = false;
      });
  }
}
