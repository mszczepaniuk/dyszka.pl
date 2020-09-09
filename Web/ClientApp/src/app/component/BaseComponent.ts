import { OnDestroy } from '@angular/core';
import { Subscription } from 'rxjs';

export class BaseComponent implements OnDestroy {
  subscriptionsArray: Subscription[] = [];
  timersArray: number[] = [];

  safeSub(...sub: Subscription[]) {
    this.subscriptionsArray = this.subscriptionsArray.concat(sub);
  }

  safeInterval(fun: () => any, time: number) {
    const intervalID = window.setInterval(fun, time);
    this.timersArray.push(
      intervalID
    );
    return intervalID;
  }

  ngOnDestroy(): boolean {
    for (let sub of this.subscriptionsArray) {
      if (sub) {
        sub.unsubscribe();
      }
    }
    for (let timer of this.timersArray) {
      if (timer) {
        clearInterval(timer);
      }
    }
    return true;
  }
}

