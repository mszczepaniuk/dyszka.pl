import { MatSnackBar, MatDialog } from '@angular/material';
import { BehaviorSubject } from 'rxjs';
import { Component, OnInit } from '@angular/core';
import { BaseComponent } from '../BaseComponent';
import { UserService } from '../../service/user.service';
import { IdentityService } from '../../service/identity.service';
import { ActivatedRoute, Router } from '@angular/router';
import { BillingData } from '../../model/billing-data.model';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { faEdit, faSave, faUndo } from '@fortawesome/free-solid-svg-icons';
import { DialogComponent } from '../dialog/dialog.component';
import { DialogResult } from '../../enum/dialog-result.enum';
import { Title } from '@angular/platform-browser';

@Component({
  selector: 'app-billing-data',
  templateUrl: './billing-data.component.html',
  styleUrls: ['./billing-data.component.css']
})
export class BillingDataComponent extends BaseComponent implements OnInit {
  faEdit = faEdit;
  faSave = faSave;
  faUndo = faUndo;

  private submitted: boolean;
  private errors: string[];
  private billingData$: BehaviorSubject<BillingData>;
  private billingData: BillingData;
  private loading: boolean;
  private username: string;
  private form: FormGroup;

  constructor(private route: ActivatedRoute,
    private userService: UserService,
    private identityService: IdentityService,
    private router: Router,
    private formBuilder: FormBuilder,
    private dialog: MatDialog,
    private snackBar: MatSnackBar,
    private titleService: Title) {
    super();
  }

  ngOnInit(): void {
    this.form = this.createForm();
    this.form.disable();
    this.billingData$ = new BehaviorSubject<BillingData>(null);
    this.safeSub(
      this.route.paramMap.subscribe(params => {
        this.username = params.get('username');
        if (this.identityService.user$.value.userName === this.username ||
          this.identityService.isInRole('admin')) {
          this.titleService.setTitle(`Dane do przelewu użytkownika ${this.username}`);
          this.getBillingData();
        } else {
          this.router.navigateByUrl('/forbidden');
        }
      }),
      this.billingData$.subscribe(billingData => {
        this.billingData = billingData;
        if (billingData) {
          this.form.patchValue(this.billingData);
        }
      })
    );
  }

  private getBillingData() {
    this.loading = true;
    return this.safeSub(this.userService.getBillingData(this.username)
      .subscribe(
        (billingData: BillingData) => {
          this.billingData$.next(new BillingData(billingData));
          this.loading = false;
        },
        () => {
          this.loading = false;
        }));
  }

  private editBillingData() {
    this.submitted = true;
    if (this.form.valid) {
      this.errors = [];
      this.dialog.open(DialogComponent,
        {
          width: '450px',
          data: {
            message: 'Czy na pewno chcesz zapisać zmiany?'
          }
        }).afterClosed().subscribe((result: DialogResult) => {
        if (result === DialogResult.Yes) {
          this.userService.postBillingData(this.form.getRawValue()).subscribe(
            () => {
              this.getBillingData();
              this.form.disable();
              this.snackBar.open('Dokonano edycji danych do przelewu', '', { duration: 2000 });
            },
            () => {
              this.snackBar.open('Edycja danych do przelewu sie nie powiodła', '', { duration: 2000 });
            });
        }
      });
    } else {
      this.setErrors();
    }
  }

  private editClick() {
    this.form.enable();
  }

  private undoClick() {
    this.billingData$.next(this.billingData);
    this.form.disable();
  }

  private createForm() {
    return this.formBuilder.group({
      name: ['', [Validators.required]],
      bankAccountNumber: ['', [Validators.required, Validators.pattern(/^\d{26}$/)]],
      street: [''],
      city: [''],
      postalCode: ['']
    });
  }

  private setErrors() {
    this.errors = [];
    this.errors.push('Nazwa użytkownika i numer konta muszą być podane');
    this.errors.push('Numer konta powinien zawierać 26 cyfr');
  }
}
