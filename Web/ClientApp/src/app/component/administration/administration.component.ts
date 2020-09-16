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

@Component({
  selector: 'app-forbidden',
  templateUrl: './administration.component.html',
  styleUrls: ['./administration.component.css']
})
export class AdministrationComponent extends BaseComponent {

  addingAdminForm = new FormGroup({
    adminName: new FormControl('', [Validators.required])
  })

  addingModForm = new FormGroup({
    modName: new FormControl('', [Validators.required])
  })

  public logsArray$ = new BehaviorSubject<AuditLog[]>([]);
  adminsArray = [];
  modsArray = [];
  adminsShowBool: boolean;
  modsShowBool: boolean;
  logsShowBool: boolean;
  adminName: string;
  currentLogsPage: number;
  maxLogPage: number;
 
  //addingAdminForm = new FormGroup;

  constructor(
    private administrationService: AdministrationService,
    private formsModule: FormsModule,
    private dialog: MatDialog) {
    super();
    //// TESTOWE LOGI
    this.safeSub(
      this.administrationService.admins$.subscribe(admins => {
        this.adminsArray = admins;
      }),
      this.administrationService.moderators$.subscribe(mods => {
        this.modsArray = mods;
      }),
    );
    this.getLogsPage(1);
    this.adminsShowBool = true;
  }

  showAdmins() {
    this.modsShowBool = false;
    this.logsShowBool = false;
    this.adminsShowBool = true;
  }

  showMods() {
    this.logsShowBool = false;
    this.adminsShowBool = false;
    this.modsShowBool = true;
  }

  showLogs() {
    this.modsShowBool = false;
    this.adminsShowBool = false;
    this.logsShowBool = true;
  }

  deleteAdmin(admin) {
    this.dialog.open(DialogComponent, {
      width: "450px",
      data: {
        message: "Czy na pewno usunąć rolę temu użytkownikowi?",
        dialogTitle: "Usuwanie roli administratora"
      }
    }).afterClosed().subscribe((dialogResult: DialogResult) => {
      if (dialogResult === DialogResult.Yes) {
        this.administrationService.deleteAdminRole(admin.userName);
      }
    })
  }

  addAdminRole() {
    this.dialog.open(DialogComponent, {
      width: "450px",
      data: {
        message: "Czy na pewno nadać rolę temu użytkownikowi?",
        dialogTitle: "Dodanie roli administratora"
      }
    }).afterClosed().subscribe((dialogResult: DialogResult) => {
      if (dialogResult === DialogResult.Yes) {
        this.administrationService.setUserToAdmin(this.addingAdminForm.controls['adminName'].value);
      } 
    })
  }

  deleteMod(mod) {
    this.dialog.open(DialogComponent, {
      width: "450px",
      data: {
        message: "Czy na pewno usunąć rolę temu użytkownikowi?",
        dialogTitle: "Usuwanie roli moderatora"
      }
    }).afterClosed().subscribe((dialogResult: DialogResult) => {
      if (dialogResult === DialogResult.Yes) {
        this.administrationService.deleteModRole(mod.userName);
      }
    })
  }

  addModRole() {
    this.dialog.open(DialogComponent, {
      width: "450xpx",
      data: {
        message: "Czy na pewno nadać rolę temu użytkownikowi",
        dialogTitle: "Dodanie roli moderatora"
      }
    }).afterClosed().subscribe((dialogResult: DialogResult) => {
      if (dialogResult === DialogResult.Yes) {
        this.administrationService.setUserToMod(this.addingModForm.controls['modName'].value);
      }
    })
  }

  getLogsPage(page: number) {
    this.administrationService.getAuditLogs(page).subscribe((logs: PagedResult<AuditLog>) => {
      let logsTempArray = [];
      logs.items.forEach(log => {
        logsTempArray.push(new AuditLog(log));
      });
      this.logsArray$.next(logsTempArray);
      console.log(logsTempArray);
      console.log(this.logsArray$.value);
      this.currentLogsPage = logs.currentPage;
      this.maxLogPage = logs.pagesCount
    })
  }

}
