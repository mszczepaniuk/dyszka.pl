import { Injectable } from '@angular/core';
import { HttpInterceptor, HttpRequest, HttpHandler, HttpEvent } from '@angular/common/http';
import { IdentityService } from '../service/identity.service';
import { Observable } from 'rxjs/internal/Observable';
import { mergeMap } from 'rxjs/operators';

@Injectable()
export class TokenInterceptor implements HttpInterceptor {

  constructor(private identityService: IdentityService) { }

  intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    if (this.identityService.isTokenAvailable()) {
      return this.withToken(request, next);
    }
    return next.handle(request);
  }

  private withToken(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    if (this.identityService.isAccessTokenValid()) {
      request = request.clone({
        setHeaders: {
          Authorization: `Bearer ${this.identityService.getAccessToken()}`
        }
      });
      return next.handle(request);
    }
    return this.identityService.refreshAccessToken()
      .pipe(mergeMap(response => {
        request = request.clone({
          setHeaders: {
            Authorization: `Bearer ${response || ''}`
          }
        });
        return next.handle(request);
      }));
  }
}
