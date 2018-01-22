import { Injectable, Inject } from '@angular/core';
import { HttpClient, HttpResponse, HttpErrorResponse } from '@angular/common/http';
import { BehaviorSubject } from 'rxjs/BehaviorSubject';
import { Observable } from 'rxjs/Observable';
import { ErrorResponse } from './Responses';
import 'rxjs/add/operator/map';

import { AccessData } from '../_models/access-data';

import { AuthenticationService } from '../_services/authentication.service';

import { APP_CONFIG } from '../app.config';
import { IAppConfig } from '../_interfaces/IAppConfig';

@Injectable()
export class AuthenticationManagerService {

    isLoadingLogin$: BehaviorSubject<Boolean> = <BehaviorSubject<Boolean>> new BehaviorSubject(false);

    constructor(private _authService: AuthenticationService,
                @Inject(APP_CONFIG)private _appConfig: IAppConfig) {

        // listen steam auth window for auth data
        if (window.addEventListener) {
            window.addEventListener("message", this.receiveWindowMessage.bind(this), false);
        } else {
            (<any>window).attachEvent("onmessage", this.receiveWindowMessage.bind(this));
        }
    }

    loginSteam() {
        this.isLoadingLogin$.next(true);
        var externalProviderUrl = `${this._appConfig.endPoint}/Account/SignInWithSteam`;
        this.popupCenter(externalProviderUrl, "Authenticate Account", 600, 750);
      }
    
    closedInterval = null;

    popupCenter(url, title, w, h) {
        this.closedInterval = null;
        var left = (screen.width/2)-(w/2);
        var top = (screen.height/2)-(h/2);
        var newWindow = window.open(url, title, 'toolbar=no, location=no, directories=no, status=no, menubar=no, scrollbars=no, resizable=no, copyhistory=no, width='+w+', height='+h+', top='+top+', left='+left);

        // Вдруг закроют окошко нечаянно колхозим) onbeforeunload не работает!
        this.closedInterval = setInterval(() => {
            if (newWindow.closed) {
            this.isLoadingLogin$.next(false);
            clearInterval(this.closedInterval);
            }
        }, Math.pow(2, 9));

        if (window.focus) {
            newWindow.focus();
        }
    }

    receiveWindowMessage: any = (event: any) =>  {
        if (event.origin == `${this._appConfig.endPoint}`) {
          this.onLogin(event.data);
        }
    }

    onLogin(data) {
        const access_data: AccessData = JSON.parse(data);
        console.info(access_data);
        this._authService.login(access_data.token)
          .finally(() => { this.isLoadingLogin$.next(false); clearInterval(this.closedInterval); })
          .subscribe(
            (data) => {
              console.info(data);
            },
            (err) => {
              console.error(err);
            }
        );
    }
}
