import { Injectable } from "@angular/core";
import { HttpInterceptor, HttpRequest, HttpHandler, HttpEvent } from "@angular/common/http";
import { IdentityService } from "../service/identity.service";
import { Observable } from "rxjs/internal/Observable";
import { mergeMap } from 'rxjs/operators';

@Injectable()
export class TokenInterceptor implements HttpInterceptor {

  constructor(private identityService: IdentityService) { }

  intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    //TODO: Refresh token logic.
    if (this.identityService.isAccessTokenValid()) {
      request = request.clone({
        setHeaders: {
          Authorization: `Bearer ${this.identityService.getAccessToken()}`
        }
      });
    }
    return next.handle(request);
  }
}
