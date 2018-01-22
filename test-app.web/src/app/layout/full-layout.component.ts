import { Component, OnInit, AfterViewInit, ViewChild, Inject } from '@angular/core';
import { IAppConfig  } from '../_interfaces/IAppConfig';
import { APP_CONFIG } from '../app.config';
import { Route, Router } from '@angular/router';

import { Observable } from 'rxjs/Observable';
import 'rxjs/add/operator/do';
import 'rxjs/add/operator/finally';

import { HttpClient } from '@angular/common/http';

import { AuthenticationService } from '../_services/authentication.service';
import { MainService } from '../_services/data/main.service';
import { AuthenticationManagerService } from '../_services/authentication.manager.service';

@Component({
  templateUrl: './full-layout.component.html',
  styleUrls: ['./full-layout.component.scss'],
})
export class FullLayoutComponent implements OnInit, AfterViewInit {

  public currentUser: any = {};
  public isLoadingLogin: Boolean = false;

  mainData = {
    opennedCases: 0,
    usersRegistered: 0,
    online: 0,
    serverTime: null
  };

  constructor(@Inject(APP_CONFIG)private _appConfig: IAppConfig,
              private router: Router,
              private _authService: AuthenticationService,
              private _authenticationManagerService: AuthenticationManagerService,
              private _mainService: MainService,
              private http: HttpClient) {

    // listen currentUser
    this._authService.currentUser$.subscribe(x => {
      this.currentUser = x;
    });

    this._mainService.data$.subscribe(
      (data) => {
        this.mainData = data;
      },
      (err) => { console.error(err); }
    );

    this._mainService.refresh(false);

    this._authenticationManagerService.isLoadingLogin$.subscribe((b) => {
      this.isLoadingLogin = b;
    });
  }

  loginSteam() {
    this._authenticationManagerService.loginSteam();
  }

  logout() {
    this._authService.logout();
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
