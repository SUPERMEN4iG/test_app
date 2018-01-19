import { Injectable, Inject } from '@angular/core';
import { HttpClient, HttpResponse, HttpErrorResponse } from '@angular/common/http';
import { ErrorResponse } from '../Responses';
import { BehaviorSubject } from 'rxjs/BehaviorSubject';
import { Observable } from 'rxjs/Observable';
import 'rxjs/add/operator/map';
import 'rxjs/add/observable/of';
import 'rxjs/add/operator/catch';
import 'rxjs/add/observable/throw';

import { APP_CONFIG } from '../../app.config';
import { IAppConfig } from '../../_interfaces/IAppConfig';

import * as _ from 'underscore';

@Injectable()
export class UsersService {
  serviceEndPoint: String = 'user/';
  apiEndPoint: String;
  data: Array<any> = new Array<object>();
  data$: BehaviorSubject<Object[]>;

  constructor(private _http: HttpClient,
              @Inject(APP_CONFIG)private _appConfig: IAppConfig) {
    this.apiEndPoint = `${_appConfig.apiEndPoint + this.serviceEndPoint}`;
    this.data$ = <BehaviorSubject<Object[]>> new BehaviorSubject(new Array<Object>());
  }

  private handleError(error: HttpResponse<ErrorResponse>) {
    console.error(error);
    return Observable.throw((<any>error).error || 'Server error');
  }

  public getUser(id: string): Observable<any> {
    const founded = this.data.find((o) => { return o.id == id; });
    if (founded !== undefined && founded !== null) {
      return Observable.of(founded);
    } else {
      return this._http.get(`${this.apiEndPoint}getuser/?id=${id}`)
        .map(x => {
          this.data.push(x);
          this.data$.next(this.data);
          return x;
        });
    }
  }

  public refreshUser(id: string): Observable<any> {
    const founded = this.data.find((o) => { return o.id == id; });
    const foundedIndex = this.data.indexOf(founded);

    if (foundedIndex > -1) {
      delete this.data[foundedIndex]; this.data.length--;
    }

    return this.getUser(id);
  }

  public appnedWin(id: string, win: any): any {
    const founded = this.data.find((o) => { return o.id == id; });
    const foundedIndex = this.data.indexOf(founded);

    if (foundedIndex > -1) {
      founded.wonItems.push(win);
      delete this.data[foundedIndex]; this.data.length--;
      this.data.push(founded);
      this.data$.next(founded);
    }
  }

  public updateTradeOffer(tradeofferurl) {
    return this._http.post(`${this.apiEndPoint}updatetradeoffer`, {'tradeofferurl': tradeofferurl})
        .map(x => {
          return x;
        }).catch(this.handleError);
  }

}
