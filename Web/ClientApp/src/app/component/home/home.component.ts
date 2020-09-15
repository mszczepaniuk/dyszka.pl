import { Component } from '@angular/core';
import { IdentityService } from '../../service/identity.service';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
})
export class HomeComponent {

  constructor(private identityService: IdentityService) {

  }
}
