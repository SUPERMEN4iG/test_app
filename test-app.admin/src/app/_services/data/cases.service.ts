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
export class CasesService {
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
      this.getCases().subscribe(x => {
        this.data = x;
        this.data$.next(x);
      });
    } else {
      this.data$.next(this.data);
    }
  }

  getCases() {
    return this._http.get(`${this.apiEndPoint}getcasesdata`)
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

  getByCaseId(id) {

    var results = _.map(this.data, function(row) {
      var founded = _.filter(row.cases, function(c) {
        return c.id == id;
      });

      if (founded.length > 0) {
        return founded;
      }
    });

    return _.find(results, (s) => { return s != undefined })[0];
  }

  caclulateChances(caseId, margine) {
    return this._http.get(`${this.apiEndPoint}caclulatechances?caseId=${caseId}&margine=${margine}`)
      .map((x: any) => {
        try {
          const response = x;

          return response;
        } catch (error) {
          throw new Error(error);
        }
      })
      .catch(this.handleError);
  };

  testOpenCase(caseId, count) {

    return this._http.get(`${this.apiEndPoint}testopencase?caseId=${caseId}&count=${count}`)
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

  saveCase(dataCase) {
    return this._http.post(`${this.apiEndPoint}savecase`, JSON.stringify(dataCase))
      .map((x: any) => {
        try {
          const response = x;

          return response;
        } catch (error) {
          throw new Error(error);
        }
      })
      .catch(this.handleError);
  };

}
