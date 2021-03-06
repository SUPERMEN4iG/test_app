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
import { WinnerService } from '../_services/data/winner.service';
import { PaymentService } from '../_services/data/payment.service';

@Component({
  selector: 'profile',
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.scss'],
})
export class ProfileComponent {
  private sub: any;
  profileId: string;
  user: any;
  isLoading: boolean = false;
  currentUser: any;

  amount: any;

  isDisableForEdit = true;

  constructor(private router: Router,
              private route: ActivatedRoute,
              private _usersService: UsersService,
              private _authService: AuthenticationService,
              private _notification: ToastrService,
              private _winnerService: WinnerService,
              private _paymentService: PaymentService) {
  }

  ngOnInit() {
    this.isLoading = true;
    this.sub = this.route.params.subscribe(params => {
      this.profileId = params['id'];

      this._usersService.getUser(this.profileId).finally(() => { this.isLoading = false; }).subscribe(
        (data) => {
          this.user = data; console.info(this.user);

          this._authService.currentUser$.subscribe((cu: any) => {
            if (cu && cu.id == this.user.id) {
              this.isDisableForEdit = false;
            } else {
              this.isDisableForEdit = true;
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

  onClickSell(item) {
    item.state = 1337;
    this._winnerService.sell(item.id).subscribe(
      (data) => {
        item.price = data.data;
        item.state = 1;
        this._authService.updateUser('balance', (this._authService.currentUser.balance + item.price));
      },
      (err) => {
        item.state = 0;
        this._notification.error(err.message, 'Sell item');
      }
    );
  }

  onClickRefillBalance() {
    this._paymentService.getG2ARefillData(this.amount).subscribe(
      (data) => {
        window.location.href = `https://checkout.test.pay.g2a.com/index/gateway?token=${data.token}`;
        console.info(data);
      },
      (err) => {
        this._notification.error(err.message, 'Refill balance');
      }
    );
  }
}
