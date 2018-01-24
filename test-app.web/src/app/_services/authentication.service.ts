import { Injectable, Inject } from '@angular/core';
import { HttpClient, HttpResponse, HttpErrorResponse } from '@angular/common/http';
import { BehaviorSubject } from 'rxjs/BehaviorSubject';
import { Observable } from 'rxjs/Observable';
import { ErrorResponse } from './Responses';
import 'rxjs/add/operator/map';

import { APP_CONFIG } from '../app.config';
import { IAppConfig } from '../_interfaces/IAppConfig';
import { environment } from '../../environments/environment';

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
      this.apiEndPoint = `${environment.apiEndPoint + this.serviceEndPoint}`;
    }

    getToken() {
      return localStorage.getItem('access_token');
    }

    init() {
      if (this.getToken() != null) {
        return this.http.get(`${this.apiEndPoint}getuserdata`)
        .map((response: any) => {
          const authData = response;

          if (authData && authData.id) {
            localStorage.setItem('currentUser', JSON.stringify(authData));
            this.currentUser = authData;
            this.currentUser$.next(this.currentUser);
          }
          return authData;
        });
      } else {
        return Observable.of(null);
      }
    }

    login(access_token) {
      localStorage.setItem('access_token', access_token);

      return this.init();
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

    updateUser(property: string, value: any) {
      this.currentUser[property] = value;
      this.currentUser$.next(this.currentUser);
    }

    logout() {
        localStorage.removeItem('currentUser');
        localStorage.removeItem('access_token');
        this.currentUser = null;
        this.currentUser$.next(null);
    }
}
