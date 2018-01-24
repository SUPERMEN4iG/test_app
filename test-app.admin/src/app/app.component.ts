import { Component } from '@angular/core';
import { Router, NavigationEnd  } from '@angular/router';

import { AuthenticationService } from './_services/authentication.service';

@Component({
  selector: 'body',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent {
  title = 'app';

  constructor(private _authService: AuthenticationService,
              private router: Router) {
  }

  ngOnInit() {
    this._authService.init().subscribe((data) => {}, (err) => {});

    this.router.events.subscribe((evt) => {
        if (!(evt instanceof NavigationEnd)) {
            return;
        }
        window.scrollTo(0, 0)
    });
  }
}
