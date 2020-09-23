import { OnDestroy } from '@angular/core';
import { Subscription } from 'rxjs';

export class BaseComponent implements OnDestroy {
  subscriptionsArray: Subscription[] = [];

  safeSub(...sub: Subscription[]) {
    this.subscriptionsArray = this.subscriptionsArray.concat(sub);
  }

  ngOnDestroy(): boolean {
    for (let sub of this.subscriptionsArray) {
      if (sub) {
        sub.unsubscribe();
      }
    }
    return true;
  }
}

