import { Injectable } from '@angular/core';

import {
  CanActivate,
  Router,
  ActivatedRouteSnapshot,
  RouterStateSnapshot
} from '@angular/router';

import { IdentityService } from '../service/identity.service';
import { take, map } from 'rxjs/operators';
import { Observable } from 'rxjs';
import { User } from '../model/user.model';

@Injectable()
export class AuthGuard implements CanActivate {

  private readonly forbiddenUrl: string = '/forbidden';

  constructor(
    private identityService: IdentityService,
    private router: Router
  ) { }

  public canActivate(
    route: ActivatedRouteSnapshot,
    state: RouterStateSnapshot
  ): Observable<boolean> | boolean {
    return this.identityService.user$.pipe(
      map(
        user => this.checkIfLoggedIn(user) && this.checkUserIsBanned(user) && this.checkUserIsInRole(route)
      ),
      take(1)
    );
  }

  checkIfLoggedIn(user: User) {
    let result = true;
    if (!user) {
      this.router.navigate([this.forbiddenUrl]);
      result = false;
    }
    return result;
  }

  checkUserIsBanned(user: User): boolean {
    if (user.isBanned) {
      this.router.navigate([this.forbiddenUrl]);
    }
    return !user.isBanned;
  }

  checkUserIsInRole(route: ActivatedRouteSnapshot): boolean {
    if (!route.data.role) {
      return true;
    }
    const hasPermission = this.identityService.isInRole(route.data.role);
    if (!hasPermission) {
      this.router.navigate([this.forbiddenUrl]);
    }
    return hasPermission;
  }
}
