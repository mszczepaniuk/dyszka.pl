import { Component, OnInit, Input } from '@angular/core';
import { BaseComponent } from '../BaseComponent';
import { CommentService } from '../../service/comment.service';
import { PagedResult } from '../../model/paged-result.model';
import { Comment } from '../../model/comment.model';
import { BehaviorSubject } from 'rxjs';
import { faTrashAlt, faCheck, faPlus, faForward, faBackward } from '@fortawesome/free-solid-svg-icons';
import { IdentityService } from '../../service/identity.service';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatDialog } from '@angular/material';
import { DialogComponent } from '../dialog/dialog.component';
import { DialogResult } from '../../enum/dialog-result.enum';

@Component({
  selector: 'app-comment',
  templateUrl: './comment.component.html',
  styleUrls: ['./comment.component.css']
})
export class CommentComponent extends BaseComponent implements OnInit {
  faTrashAlt = faTrashAlt;
  faCheck = faCheck;
  faPlus = faPlus;
  faForward = faForward;
  faBackward = faBackward;

  @Input() public username: string;
  @Input() public offerId: string;

  public comments$: BehaviorSubject<Comment[]>;

  private pagesCount: number;
  private currentPage: number = 1;
  private comments: Comment[] = [];
  private loading: boolean;
  private form: FormGroup;
  private submitted = false;
  private errors: string[];

  constructor(private commentService: CommentService,
    private identityService: IdentityService,
    private formBuilder: FormBuilder,
    private dialog: MatDialog) {
    super();
  }

  ngOnInit(): void {
    this.comments$ = new BehaviorSubject<Comment[]>([]);
    this.form = this.createForm();
    this.safeSub(
      this.comments$.subscribe(comments => this.comments = comments),
      this.getPage(this.currentPage));
  }

  private getPage(page: number) {
    this.loading = true;
    return this.commentService.getPagedAndFiltered(page, this.username, this.offerId).subscribe(
      (result: PagedResult<Comment>) => {
        let offers = [];
        result.items.forEach(comment => {
          offers.push(new Comment(comment));
        });
        this.comments$.next(offers);
        this.currentPage = result.currentPage;
        this.pagesCount = result.pagesCount;
        this.loading = false;
      },
      () => {
        this.loading = false;
      });
  }

  private addComment() {
    this.submitted = true;
    if (this.form.valid) {
      let comment = this.form.getRawValue();
      comment['offerId'] = this.offerId;
      this.commentService.addComment(comment).subscribe(() => {
        this.form = this.createForm();
        this.getPage(1);
      });
    } else {
      this.setErrors();
    }
  }

  private deleteComment(id: string) {
    this.dialog.open(DialogComponent,
      {
        width: '450px',
        data: {
          message: 'Czy na pewno chcesz usunąć komentarz?',
        }
      }).afterClosed().subscribe(result => {
        if (result === DialogResult.Yes) {
          this.commentService.deleteComment(id).subscribe(() => {
            this.getPage(this.currentPage);
          });
        }
      });
  }

  private setToPositive(id: string) {
    this.dialog.open(DialogComponent,
      {
        width: '450px',
        data: {
          message: 'Czy na pewno chcesz zmienic komentarz na pozytywny? Nie można tego cofnąć.',
        }
      }).afterClosed().subscribe(result => {
        if (result === DialogResult.Yes) {
          this.commentService.setToPositive(id).subscribe(() => {
            this.comments$.next(this.comments.map(comment => {
              if (comment.id === id) {
                comment.isPositive = true;
              }
              return comment;
            }));
          });
        }
      });
  }

  private createForm() {
    return this.formBuilder.group({
      text: ['', [Validators.required, Validators.maxLength(160)]],
      isPositive: [false]
    });
  }

  private setErrors() {
    this.errors = [];
    if (this.form.controls.text.errors && this.form.controls.text.errors.required) {
      this.errors.push('Podaj treść komentarza');
    }
    if (this.form.controls.text.errors && this.form.controls.text.errors.maxlength) {
      this.errors.push('Maksymalna długość komentarza to 160 znaków');
    }
  }
}
