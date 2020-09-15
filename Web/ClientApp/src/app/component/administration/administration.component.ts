import { Component } from '@angular/core';
import { AdministrationService } from '../../service/administration.service';
import { BaseComponent } from '../BaseComponent';
import { FormsModule, FormGroup, FormControl, Validators } from '@angular/forms';



@Component({
  selector: 'app-forbidden',
  templateUrl: './administration.component.html',
  styleUrls: ['./administration.component.css']
})
export class AdministrationComponent extends BaseComponent {

  addingAdminForm = new FormGroup({
    adminName: new FormControl('', [Validators.required])
  })

  adminsArray = [];
  modsArray = [];
  adminsShowBool: boolean;
  modsShowBool: boolean;
  logsShowBool: boolean;
  adminName: string;
  //addingAdminForm = new FormGroup;

  constructor(
    private administrationService: AdministrationService,
    private formsModule: FormsModule) {
    super();
    // TESTOWE LOGI
    this.safeSub(
      this.administrationService.admins$.subscribe(admins => {
        console.log(admins);
      }),
      this.administrationService.moderators$.subscribe(mods => {
        console.log(mods);
      }),
      this.administrationService.getAuditLogs(1).subscribe(logs => console.log(logs)),
      this.administrationService.getAuditLogs(2).subscribe(logs => console.log(logs)),
      this.administrationService.getAuditLogs(3).subscribe(logs => console.log(logs)),
      this.administrationService.getAuditLogs(4).subscribe(logs => console.log(logs)),

    );

    this.getAdmins();
    this.adminsShowBool = true;
  }

  getAdmins() {
    this.administrationService.admins$.subscribe(admins => {
      this.adminsArray = admins;
    });
  }

  getMods() {
    this.administrationService.moderators$.subscribe(mods => {
      this.modsArray = mods;
    });
  }

  showAdmins() {
    this.getAdmins();
    this.modsShowBool = false;
    this.logsShowBool = false;
    this.adminsShowBool = true;
  }

  showMods() {
    this.getMods();
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
    console.log(admin.userName);
  }

  addAdminRole() {
    this.administrationService.setUserToAdmin(this.addingAdminForm.controls['adminName'].value);
  }

}
