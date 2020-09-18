import { Component, OnInit } from '@angular/core';
import { BaseComponent } from '../../BaseComponent';
import { BehaviorSubject } from 'rxjs';
import { MessageService } from '../../../service/message.service';
import { Message } from '../../../model/message.model';
import { PagedResult } from '../../../model/paged-result.model';
import { faEnvelope } from '@fortawesome/free-solid-svg-icons';

@Component({
  selector: 'app-message-inbox',
  templateUrl: './message-inbox.component.html',
  styleUrls: ['/message-inbox.component.css']
})
export class MessageInboxComponent extends BaseComponent implements OnInit {
  faEnvelope = faEnvelope;

  private messages$: BehaviorSubject<Message[]>;
  private currentPage: number = 1;
  private loading: boolean;
  private pagesCount: number;
  private messages: Message[] = [];

  constructor(private messageService: MessageService) {
    super();
  }

  ngOnInit() {
    this.messages$ = new BehaviorSubject<Message[]>([]);
    this.safeSub(
      this.getPage(this.currentPage),
      this.messages$.subscribe(messages => this.messages = messages)
    );
  }

  private getPage(page: number) {
    this.loading = true;
    return this.messageService.getInbox(page).subscribe((result: PagedResult<Message>) => {
        let offers = [];
        for (let i = result.items.length - 1; i >= 0; i--) {
          offers.push(new Message(result.items[i]));
        }
        this.messages$.next(offers);
        this.currentPage = result.currentPage;
        this.pagesCount = result.pagesCount;
        this.loading = false;
      },
      () => {
        this.loading = false;
      });
  }
}
