import { Component, ViewChild } from '@angular/core';
import { AdministrationService } from '../../service/administration.service';
import { BaseComponent } from '../BaseComponent';
import { FormsModule, FormGroup, FormControl, Validators } from '@angular/forms';
import { MatDialog, MatDialogTitle } from '@angular/material';
import { DialogComponent } from '../dialog/dialog.component';
import { DialogResult } from '../../enum/dialog-result.enum';
import { BehaviorSubject } from 'rxjs';
import { AuditLog } from '../../model/audit-log.model';
import { PagedResult } from '../../model/paged-result.model';
import { PaymentService } from '../../service/payment.service';
import { faPlus, faTrashAlt, faOutdent, faCheck, faMoneyBill } from '@fortawesome/free-solid-svg-icons';
import { Payment } from '../../model/payment.model';
import { Title } from '@angular/platform-browser';

@Component({
  selector: 'app-forbidden',
  templateUrl: './administration.component.html',
  styleUrls: ['./administration.component.css']
})
export class AdministrationComponent extends BaseComponent {
  faPlus = faPlus;
  faTrashAlt = faTrashAlt;
  faOutdent = faOutdent;
  faCheck = faCheck;
  faMoneyBill = faMoneyBill;

  addingAdminForm = new FormGroup({
    adminName: new FormControl('', [Validators.required])
  });

  addingModForm = new FormGroup({
    modName: new FormControl('', [Validators.required])
  });

  public logsArray$ = new BehaviorSubject<AuditLog[]>([]);
  adminsArray = [];
  modsArray = [];
  currentTab = 'admins';
  adminName: string;
  currentLogsPage: number;
  maxLogPage: number;
  currentPaymentsPage: number = 1;
  paymentsPageCount: number;
  private payments$ = new BehaviorSubject<Payment[]>([]);
  private payments: Payment[];

  constructor(
    private administrationService: AdministrationService,
    private formsModule: FormsModule,
    private dialog: MatDialog,
    private paymentService: PaymentService,
    private titleService: Title) {
    super();
    this.titleService.setTitle('Panel administratorski');
    this.safeSub(
      this.administrationService.admins$.subscribe(admins => {
        this.adminsArray = admins;
      }),
      this.administrationService.moderators$.subscribe(mods => {
        this.modsArray = mods;
      }),
      this.payments$.subscribe(payments => this.payments = payments),
      this.getLogsPage(1),
      this.getPaymentsPage(1)
    );
  }

  showAdmins() {
    this.currentTab = 'admins';
  }

  showMods() {
    this.currentTab = 'mods';
  }

  showLogs() {
    this.currentTab = 'logs';
  }

  showPayments() {
    this.currentTab = 'payments';
  }

  deleteAdmin(admin) {
    this.dialog.open(DialogComponent,
      {
        width: "450px",
        data: {
          message: "Czy na pewno usunąć rolę temu użytkownikowi?",
          dialogTitle: "Usuwanie roli administratora"
        }
      }).afterClosed().subscribe((dialogResult: DialogResult) => {
      if (dialogResult === DialogResult.Yes) {
        this.administrationService.deleteAdminRole(admin.userName);
      }
    });
  }

  addAdminRole() {
    this.dialog.open(DialogComponent,
      {
        width: "450px",
        data: {
          message: "Czy na pewno nadać rolę temu użytkownikowi?",
          dialogTitle: "Dodanie roli administratora"
        }
      }).afterClosed().subscribe((dialogResult: DialogResult) => {
      if (dialogResult === DialogResult.Yes) {
        this.administrationService.setUserToAdmin(this.addingAdminForm.controls['adminName'].value);
      }
    });
  }

  deleteMod(mod) {
    this.dialog.open(DialogComponent,
      {
        width: "450px",
        data: {
          message: "Czy na pewno usunąć rolę temu użytkownikowi?",
          dialogTitle: "Usuwanie roli moderatora"
        }
      }).afterClosed().subscribe((dialogResult: DialogResult) => {
      if (dialogResult === DialogResult.Yes) {
        this.administrationService.deleteModRole(mod.userName);
      }
    });
  }

  addModRole() {
    this.dialog.open(DialogComponent,
      {
        width: "450xpx",
        data: {
          message: "Czy na pewno nadać rolę temu użytkownikowi",
          dialogTitle: "Dodanie roli moderatora"
        }
      }).afterClosed().subscribe((dialogResult: DialogResult) => {
      if (dialogResult === DialogResult.Yes) {
        this.administrationService.setUserToMod(this.addingModForm.controls['modName'].value);
      }
    });
  }

  getLogsPage(page: number) {
    return this.administrationService.getAuditLogs(page).subscribe((logs: PagedResult<AuditLog>) => {
      let logsTempArray = [];
      logs.items.forEach(log => {
        logsTempArray.push(new AuditLog(log));
      });
      this.logsArray$.next(logsTempArray);
      this.currentLogsPage = logs.currentPage;
      this.maxLogPage = logs.pagesCount;
    });
  }

  getPaymentsPage(page: number) {
    return this.paymentService.getPaged(page).subscribe((result: PagedResult<Payment>) => {
      let payments = [];
      result.items.forEach(payment => {
        payments.push(new Payment(payment));
      });
      this.payments$.next(payments);
      this.currentPaymentsPage = result.currentPage;
      this.paymentsPageCount = result.pagesCount;
    });
  }

  markAsDone(id: string) {
    this.dialog.open(DialogComponent,
      {
        width: '450px',
        data: {
          message: 'Czy na pewno płatność została zrealizowana?'
        }
      }).afterClosed().subscribe((result: DialogResult) => {
      if (result === DialogResult.Yes) {
        this.paymentService.markAsDone(id).subscribe(() => this.getPaymentsPage(this.currentPaymentsPage));
      }
    });
  }
}
