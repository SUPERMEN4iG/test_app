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
export class StatisticService {
  serviceEndPoint: String = 'admin/';
  apiEndPoint: String;
  data: Array<any> = new Array<object>();
  data$: BehaviorSubject<any[]>;

  constructor(private _http: HttpClient,
              @Inject(APP_CONFIG)private _appConfig: IAppConfig) {
    this.apiEndPoint = `${environment.apiEndPoint + this.serviceEndPoint}`;
    this.data$ = <BehaviorSubject<Object[]>> new BehaviorSubject(new Array<Object>());
    console.info(this.data);

    if (this.data.length === 0) {
      this.refresh(false);
    }
  }

  private handleError(error: HttpResponse<ErrorResponse>) {
    console.error(error);
    return Observable.throw((<any>error).error || 'Server error');
  }

  public refresh(isFromCache: boolean) {
    this.data$.next([]);

    if (isFromCache === undefined || isFromCache === null || isFromCache === false) {
      this.getStatistic().subscribe(x => {
        this.data = x;
        this.data$.next(x);
      });
    } else {
      this.data$.next(this.data);
    }
  }

  getStatistic() {
    return this._http.get(`${this.apiEndPoint}getstatistic`)
      .map((x: any) => {
        try {
          const response = x;

          return response;
        } catch (error) {
          throw new Error(error);
        }
      })
      .catch(this.handleError);
  }

}
