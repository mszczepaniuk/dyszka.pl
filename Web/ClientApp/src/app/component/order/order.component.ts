import { Component, OnInit } from '@angular/core';
import { BaseComponent } from '../BaseComponent';
import { Order } from '../../model/order.model';
import { BehaviorSubject } from 'rxjs';
import { OrderService } from '../../service/order.service';
import { PagedResult } from '../../model/paged-result.model';
import { faOutdent, faEnvelope, faForward, faBackward, faCheck } from '@fortawesome/free-solid-svg-icons';
import { MatDialog } from '@angular/material';
import { DialogComponent } from '../dialog/dialog.component';
import { DialogResult } from '../../enum/dialog-result.enum';

@Component({
  selector: 'app-order',
  templateUrl: './order.component.html',
  styleUrls: ['./order.component.css']
})
export class OrderComponent extends BaseComponent implements OnInit {
  faOutdent = faOutdent;
  faEnvelope = faEnvelope;
  faForward = faForward;
  faBackward = faBackward;
  faCheck = faCheck;

  private selectedTab = 'orderedByUser';
  private orders$: BehaviorSubject<Order[]>;
  private orders: Order[];
  private pagesCount: number;
  private currentPage: number = 1;
  private loading: boolean;

  constructor(private orderService: OrderService,
    private dialog: MatDialog) {
    super();
  }

  ngOnInit(): void {
    this.orders$ = new BehaviorSubject<Order[]>([]);
    this.safeSub(
      this.orders$.subscribe(orders => this.orders = orders),
      this.getPage(this.currentPage));
  }

  private getPage(page: number) {
    this.loading = true;
    return this.getObservable(page).subscribe(
      (result: PagedResult<Order>) => {
        let orders = [];
        result.items.forEach(order => {
          orders.push(new Order(order));
        });
        this.orders$.next(orders);
        this.currentPage = result.currentPage;
        this.pagesCount = result.pagesCount;
        this.loading = false;
      },
      () => {
        this.loading = false;
      });
  }

  private getObservable(page: number) {
    switch (this.selectedTab) {
      case 'orderedByUser':
        return this.orderService.getCreatedByCurrentUser(page);
      case 'completed':
        return this.orderService.getOrdersForCurrentUserOffers(page, true);
      case 'inProgress':
        return this.orderService.getOrdersForCurrentUserOffers(page, false);
    }
  }

  private markAsDone(id: string) {
    this.dialog.open(DialogComponent,
      {
        width: '450px',
        data: {
          message: 'Czy na pewno chcesz zmienić status zamówienia na zrealizowane?',
        }
      }).afterClosed().subscribe((result: DialogResult) => {
      if (result === DialogResult.Yes) {
        this.orderService.markAsDone(id).subscribe(() => {
          this.getPage(this.currentPage);
        });
      }
    });
  }

  private setToOrderedByUser() {
    this.selectedTab = 'orderedByUser';
    this.getPage(1);
  }

  private setToCompleted() {
    this.selectedTab = 'completed';
    this.getPage(1);
  }

  private setToInProgress() {
    this.selectedTab = 'inProgress';
    this.currentPage = 1;
    this.getPage(1);
  }
}
