import { Component } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { AuthenticationService } from '../../_services/authentication.service';
import { AuthenticationManagerService } from '../../_services/authentication.manager.service';

@Component({
  selector: 'main',
  templateUrl: './main.component.html',
  styleUrls: ['./main.component.scss'],
})
export class MainComponent {

  currentUser;

  constructor(private _authService: AuthenticationService,
              private _authenticationManagerService: AuthenticationManagerService) {

    this._authService.currentUser$.subscribe(x => {
      this.currentUser = x;
    });
  }
}
