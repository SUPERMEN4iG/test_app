import { Component } from '@angular/core';

import { AuthenticationService } from './_services/authentication.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent {
  title = 'app';

  constructor(private _authService: AuthenticationService) {
  }

  ngOnInit() {
    this._authService.init().subscribe((data) => {}, (err) => {});
  }
}
