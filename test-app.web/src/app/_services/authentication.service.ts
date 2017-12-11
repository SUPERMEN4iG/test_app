import { Injectable, Inject } from '@angular/core';
import { HttpClient, HttpResponse, HttpErrorResponse } from '@angular/common/http';
import { BehaviorSubject } from 'rxjs/BehaviorSubject';
import { Observable } from 'rxjs/Observable';
import { ErrorResponse } from './Responses';
import 'rxjs/add/operator/map';

import { APP_CONFIG } from '../app.config';
import { IAppConfig } from '../_interfaces/IAppConfig';

@Injectable()
export class AuthenticationService {
  serviceEndPoint: string = 'user/';
  apiEndPoint: string;
  token: string;
  currentUser = null;
  currentUser$: BehaviorSubject<Object>;

    constructor(private http: HttpClient,
                @Inject(APP_CONFIG)private _appConfig: IAppConfig) {
      this.currentUser$ = <BehaviorSubject<Object>> new BehaviorSubject(new Object());
      const currentUser = JSON.parse(localStorage.getItem('currentUser'));
      this.currentUser = currentUser;
      this.currentUser$.next(this.currentUser);
      this.token = currentUser && currentUser.token;
      this.apiEndPoint = `${_appConfig.apiEndPoint + this.serviceEndPoint}`;
    }

    getToken() {
      return this.token;
    }

    login(access_token) {
      this.token = access_token;
      return this.http.get(`${this.apiEndPoint}getuserdata`)
        .map((response: any) => {
          const authData = response;

          if (authData && authData.id) {
            this.token = access_token;
            localStorage.setItem('currentUser', JSON.stringify(authData));
            localStorage.setItem('access_token', this.token);
            this.currentUser = authData;
            this.currentUser$.next(this.currentUser);
          }
          return authData;
        });
        // return this.http.post('/api/authenticate', JSON.stringify({ username, password }))
        //     .map((response: Response) => {
        //         const user = response.json();
        //         if (user && user.token) {
        //             this.token = user.token;
        //             localStorage.setItem('currentUser', JSON.stringify(user));
        //         }
        //         return user;
        //     });
    }

    private handleError(error: HttpErrorResponse) {
      console.error(error);
      return Observable.throw((<ErrorResponse>error.error).message || 'Server error');
    }

    isLogIn() {
      return this.currentUser != null;
    }

    logout() {
        localStorage.removeItem('currentUser');
        localStorage.removeItem('access_token');
        this.currentUser = null;
        this.currentUser$.next(null);
    }
}
