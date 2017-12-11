import { Injectable, Inject, Injector } from '@angular/core';
import { Observable } from 'rxjs/Observable';
import 'rxjs/add/operator/do';
import { HttpInterceptor, HttpRequest, HttpHandler, HttpEvent, HttpHeaders, HttpResponse, HttpErrorResponse } from '@angular/common/http';
import { AuthenticationService } from './authentication.service';
import { Router, ActivatedRoute } from '@angular/router';

@Injectable()
export class JwtInterceptor implements HttpInterceptor {

  private router: Router;

    constructor(
      private injector: Injector
    ) { }

  intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    this.router = this.injector.get(Router); // get it here within intercept

    return next.handle(request).do((event: HttpEvent<any>) => {
      if (event instanceof HttpResponse) {
        // do stuff with response if you want
      }
    }, (err: any) => {
      if (err instanceof HttpErrorResponse) {
        if (err.status === 401) {
          console.info('TOKEN HAS BEEN DIED RELOGIN');
          this.router.navigate(['./logout']);
        }
      }
    });
  }
}
