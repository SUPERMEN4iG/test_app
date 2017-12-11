import { Component, OnInit, AfterViewInit, ViewChild } from '@angular/core';
import { Route, Router } from '@angular/router';

import { HttpClient } from '@angular/common/http';

@Component({
  templateUrl: './full-layout.component.html',
  styleUrls: ['./full-layout.component.scss'],
})
export class FullLayoutComponent implements OnInit, AfterViewInit {

  constructor(private router: Router,
              private http: HttpClient) {

    document.addEventListener('onlogin', function() {
      console.log('keys pressed');
    });
  }

  loginSteam() {
    var externalProviderUrl = "https://localhost:44362/Account/SignInWithSteam";
    var oauthWindow = window.open(externalProviderUrl, "Authenticate Account", "location=0,status=0,width=600,height=750");
    window.addEventListener('message', function(e) {
      console.info(e);
    } , false);

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
