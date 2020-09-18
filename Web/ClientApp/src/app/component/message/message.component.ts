import { Component, OnInit } from '@angular/core';
import { BaseComponent } from '../BaseComponent';
import { MessageService } from '../../service/message.service';
import { Message } from '../../model/message.model';
import { BehaviorSubject } from 'rxjs';
import { ActivatedRoute } from '@angular/router';
import { PagedResult } from '../../model/paged-result.model';

@Component({
  selector: 'app-message',
  templateUrl: './message.component.html',
  styleUrls: ['/message.component.css']
})
export class MessageComponent extends BaseComponent implements OnInit {

  public messages$: BehaviorSubject<Message[]>;
  private currentPage: number = 1;
  private loading: boolean;
  private pagesCount: number;
  private messages: Message[] = [];
  private receiverUsername: string;

  constructor(private messageService: MessageService,
    private route: ActivatedRoute) {
    super();
  }

  ngOnInit(): void {
    this.safeSub(
      this.messages$.subscribe(messages => this.messages = messages),
      this.route.paramMap.subscribe(params => {
        this.receiverUsername = params['username'];
        this.safeSub(
          this.getPage(this.currentPage)
        );
      }));
  }

  private getPage(page: number) {
    this.loading = true;
    return this.messageService.getMessages(page, this.receiverUsername)
      .subscribe((result: PagedResult<Message>) => {
        let offers = [];
        result.items.forEach(offer => {
          offers.push(new Message(offer));
        });
        this.messages$.next(offers);
        this.currentPage = result.currentPage;
        this.pagesCount = result.pagesCount;
        this.loading = false;
      }, () => {
        this.loading = false;
      });
  }

  private addComment(text: string) {
    this.messageService.addMessage({ text: text, receiverUserName: this.receiverUsername }).subscribe(() => {
      this.getPage(1);
    });
  }
}
