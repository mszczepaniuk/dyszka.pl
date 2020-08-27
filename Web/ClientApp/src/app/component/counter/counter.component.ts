import { Component } from '@angular/core';
import { IdentityService } from '../../service/identity.service';
import { Config } from '../../config';

@Component({
  selector: 'app-counter-component',
  templateUrl: './counter.component.html'
})
export class CounterComponent {
  public currentCount = 0;

  constructor(private identityService: IdentityService) { }

  public incrementCounter() {
    this.currentCount++;
    this.identityService.logIn("administrator", "wojtek123");
  }
}
