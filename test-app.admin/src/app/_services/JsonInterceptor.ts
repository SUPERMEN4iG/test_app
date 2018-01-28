import { Injectable, Inject } from '@angular/core';
import { Observable } from 'rxjs/Observable';
import { HttpInterceptor, HttpRequest, HttpHandler, HttpEvent, HttpHeaders } from '@angular/common/http';

@Injectable()
export class JsonInterceptor implements HttpInterceptor {

  private headers = new HttpHeaders({ "Content-Type": "application/json" });

  constructor() {

  }

  intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {

    // Возможен кастомный header
    if (request.headers.keys().length <= 1) {
      request = request.clone({
          setHeaders: {
            'Content-Type': 'application/json'
          }
      });
    }

    console.info('handle()');

    return next.handle(request);
  }
}
