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

@Injectable()
export class CasesService {
  serviceEndPoint: String = 'cases/';
  apiEndPoint: String;
  data: Array<any> = new Array<object>();
  data$: BehaviorSubject<any[]>;

  constructor(private _http: HttpClient,
              @Inject(APP_CONFIG)private _appConfig: IAppConfig) {
    this.apiEndPoint = `${_appConfig.apiEndPoint + this.serviceEndPoint}`;
    this.data$ = <BehaviorSubject<Object[]>> new BehaviorSubject(new Array<Object>());
    console.info(this.data);

    if (this.data.length === 0) {
      this.refresh(false);
    }
  }

  private handleError(error: HttpResponse<ErrorResponse>) {
    console.error(error);
    return Observable.throw(error || 'Server error');
  }

  public refresh(isFromCache: boolean) {
    this.data$.next([]);

    if (isFromCache === undefined || isFromCache === null || isFromCache === false) {
      this.getCases().subscribe(x => {
        this.data = x;
        this.data$.next(x);
      });
    } else {
      this.data$.next(this.data);
    }
  }

  getCases() {
    return this._http.get(`${this.apiEndPoint}getcases`)
      .map((x: any) => {
        try {
          const response = x;

          if (!_.isObject(response)) {
            throw new Error(response);
          }

          return response;
        } catch (error) {
          throw new Error(error);
        }
      })
      .catch(this.handleError);
  }

  openCase(id: number) {
    return this._http.get(`${this.apiEndPoint}opencase?id=${id}`)
    .map((x: any) => {
      try {
        const response = x;

        if (!_.isObject(response)) {
          throw new Error(response);
        }

        return response;
      } catch (error) {
        throw new Error(error);
      }
    })
    .catch(this.handleError);
  }

  getByCaseName(name) {
    return _.filter(this.data, function(obj) {
      return _.some(obj.cases, { staticName: name });
    })[0].cases[0];
  }

}
