import { Injectable, Inject } from '@angular/core';
import { BehaviorSubject } from 'rxjs/BehaviorSubject';
import { Subject } from 'rxjs/Subject';
import { Observable } from 'rxjs/Observable';
import 'rxjs/add/operator/map';
import 'rxjs/add/operator/catch';
import { HttpClient, HttpResponse } from '@angular/common/http';
import { ErrorResponse } from '../Responses';

import { APP_CONFIG } from '../../app.config';
import { IAppConfig } from '../../_interfaces/IAppConfig';
import { AuthenticationService } from '../../_services/authentication.service';

import * as _ from 'underscore';
import { environment } from '../../../environments/environment';

@Injectable()
export class PaymentService {
  serviceEndPoint: String = 'payment/';
  apiEndPoint: String;

  constructor(private _http: HttpClient,
              @Inject(APP_CONFIG)private _appConfig: IAppConfig) {
    this.apiEndPoint = `${environment.apiEndPoint + this.serviceEndPoint}`;
  }

  private handleError(error: HttpResponse<ErrorResponse>) {
    console.error(error);
    return Observable.throw((<any>error).error || 'Server error');
  }

  getG2ARefillData(amount) {
    return this._http.get(`${this.apiEndPoint}getg2arefilldata?amount=${amount}`)
      .map((x: any) => {
        try {
          const response = JSON.parse(x.data);

          return response;
        } catch (error) {
          throw new Error(error);
        }
      })
      .catch(this.handleError);
  }
}
