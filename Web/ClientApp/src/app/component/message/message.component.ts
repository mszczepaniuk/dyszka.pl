import { Component, OnInit } from '@angular/core';
import { BaseComponent } from '../BaseComponent';
import { MessageService } from '../../service/message.service';
import { Message } from '../../model/message.model';
import { BehaviorSubject } from 'rxjs';
import { ActivatedRoute } from '@angular/router';
import { PagedResult } from '../../model/paged-result.model';
import { IdentityService } from '../../service/identity.service';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatSnackBar } from '@angular/material';

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
  private form: FormGroup;

  constructor(private messageService: MessageService,
    private route: ActivatedRoute,
    private identityService: IdentityService,
    private formBuilder: FormBuilder,
    private snackBar: MatSnackBar) {
    super();
  }

  ngOnInit(): void {
    this.form = this.formBuilder.group({
      text: ['', [Validators.required]]
    });
    this.messages$ = new BehaviorSubject<Message[]>([]);
    this.safeSub(
      this.messages$.subscribe(messages => this.messages = messages),
      this.route.paramMap.subscribe(params => {
        this.receiverUsername = params.get('username');
        this.safeSub(
          this.getPage(this.currentPage)
        );
      }));
  }

  private onSubmit() {
    if (this.form.valid) {
      this.messageService.addMessage({ text: this.form.controls.text.value, receiverUserName: this.receiverUsername })
        .subscribe(() => this.getPage(1));
    } else {
      this.snackBar.open('Wiadomość nie może być pusta');
    }
  }

  private getPage(page: number) {
    this.loading = true;
    return this.messageService.getMessages(page, this.receiverUsername)
      .subscribe((result: PagedResult<Message>) => {
        let offers = [];
        for (let i = result.items.length - 1; i >= 0; i--) {
          offers.push(new Message(result.items[i]));
        }
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
