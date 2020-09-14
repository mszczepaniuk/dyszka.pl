import { Component } from '@angular/core';
import { AdministrationService } from '../../service/administration.service';
import { BaseComponent } from '../BaseComponent';

@Component({
  selector: 'app-forbidden',
  templateUrl: './administration.component.html',
  styleUrls: ['./administration.component.css']
})
export class AdministrationComponent extends BaseComponent {

  constructor(private administrationService: AdministrationService) {
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
      this.administrationService.getAuditLogs(4).subscribe(logs => console.log(logs))
      );
  }
}
