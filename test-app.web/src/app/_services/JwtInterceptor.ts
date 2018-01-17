import { Injectable, Inject, Injector } from '@angular/core';
import { Observable } from 'rxjs/Observable';
import 'rxjs/add/operator/do';
import { HttpInterceptor, HttpRequest, HttpHandler, HttpEvent, HttpHeaders, HttpResponse, HttpErrorResponse } from '@angular/common/http';
import { AuthenticationService } from './authentication.service';
import { Router, ActivatedRoute } from '@angular/router';

@Injectable()
export class JwtInterceptor implements HttpInterceptor {

  private authService: AuthenticationService;

    constructor(
      private injector: Injector
    ) { }

  intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    this.authService = this.injector.get(AuthenticationService);

    return next.handle(request).do((event: HttpEvent<any>) => {
      if (event instanceof HttpResponse) {
        // do stuff with response if you want
      }
    }, (err: any) => {
      if (err instanceof HttpErrorResponse) {
        if (err.status === 401) {
          this.authService.logout();
          console.info('TOKEN HAS BEEN DIED RELOGIN');
        }
      }
    });
  }
}
