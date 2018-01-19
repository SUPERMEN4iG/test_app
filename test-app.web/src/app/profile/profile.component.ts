import { Component } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { BehaviorSubject } from 'rxjs/BehaviorSubject';
import { Observable } from 'rxjs/Observable';
import 'rxjs/add/operator/map';
import 'rxjs/add/observable/of';
import 'rxjs/add/operator/catch';
import 'rxjs/add/observable/throw';

import { ToastrService } from 'ngx-toastr';

import { UsersService } from '../_services/data/users.service';
import { AuthenticationService } from '../_services/authentication.service';

@Component({
  selector: 'profile',
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.scss'],
})
export class ProfileComponent {
  private sub: any;
  profileId: string;
  user: any;
  currentUser: any;

  isDisableForEdit = true;

  constructor(private router: Router,
              private route: ActivatedRoute,
              private _usersService: UsersService,
              private _authService: AuthenticationService,
              private _notification: ToastrService) {
  }

  ngOnInit() {
    this.sub = this.route.params.subscribe(params => {
      this.profileId = params['id'];

      this._usersService.getUser(this.profileId).subscribe(
        (data) => { 
          this.user = data; console.info(this.user); 

          this._authService.currentUser$.subscribe((cu: any) => {
            if (cu && cu.id == this.user.id) {
              this.isDisableForEdit = false;
            }
          });
        },
        (err) => console.error(err)
      );

    });
  }

  onClickSaveTradeUrl() {
    console.info('onClickSaveTradeUrl()');
    console.info(this.user.tradeofferUrl);

    this._usersService.updateTradeOffer(this.user.tradeofferUrl).subscribe(
      (data) => {
        this._notification.success('Updated', 'Trade offer url');
      },
      (err) => {
        this._notification.error(err.message, 'Trade offer url');
      }
    );
  }
}
