import { Component, OnInit, AfterViewInit, ViewChild, Inject } from '@angular/core';
import { IAppConfig  } from '../_interfaces/IAppConfig';
import { APP_CONFIG } from '../app.config';
import { Route, Router } from '@angular/router';

import { Observable } from 'rxjs/Observable';
import 'rxjs/add/operator/do';
import 'rxjs/add/operator/finally';

import { AccessData } from '../_models/access-data';

import { HttpClient } from '@angular/common/http';

import { AuthenticationService } from '../_services/authentication.service';

@Component({
  templateUrl: './full-layout.component.html',
  styleUrls: ['./full-layout.component.scss'],
})
export class FullLayoutComponent implements OnInit, AfterViewInit {

  public currentUser: any = {};
  public isLoadingLogin = false;

  constructor(@Inject(APP_CONFIG)private _appConfig: IAppConfig,
              private router: Router,
              private _authService: AuthenticationService,
              private http: HttpClient) {

    // listen steam auth window for auth data
    if (window.addEventListener) {
      window.addEventListener("message", this.receiveWindowMessage.bind(this), false);
    } else {
        (<any>window).attachEvent("onmessage", this.receiveWindowMessage.bind(this));
    }

    // listen currentUser
    this._authService.currentUser$.subscribe(x => {
      this.currentUser = x;
    });
  }

  loginSteam() {
    this.isLoadingLogin = true;
    var externalProviderUrl = "https://localhost:44362/Account/SignInWithSteam";
    this.popupCenter(externalProviderUrl, "Authenticate Account", 600, 750);
  }

  popupCenter(url, title, w, h) {
    var left = (screen.width/2)-(w/2);
    var top = (screen.height/2)-(h/2);
    var newWindow = window.open(url, title, 'toolbar=no, location=no, directories=no, status=no, menubar=no, scrollbars=no, resizable=no, copyhistory=no, width='+w+', height='+h+', top='+top+', left='+left);

    if (window.focus) {
      newWindow.focus();
    }
  }

  logout() {
    this._authService.logout();
  }

  receiveWindowMessage: any = (event: any) =>  {
    if (event.origin == this._appConfig.endPoint) {
      this.onLogin(event.data);
    }
  }

  onLogin(data) {
    const access_data: AccessData = JSON.parse(data);
    console.info(access_data);
    this._authService.login(access_data.token)
      .finally(() => { this.isLoadingLogin = false; })
      .subscribe(
        (data) => {
          console.info(data);
        },
        (err) => {
          console.error(err);
        }
      );
  }

  ngOnInit() {
    // setInterval(() => {
    //   this.http.get("https://localhost:44362/Account/GetAccountData").subscribe(
    //     (data) => { console.info(data); },
    //     (err) => { console.error(err); }
    //   );
    // }, 3000);
  }

  ngAfterViewInit() {
    console.info('Layout.Init()');
  }
}
