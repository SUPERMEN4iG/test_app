import { Component } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { BehaviorSubject } from 'rxjs/BehaviorSubject';
import { Observable } from 'rxjs/Observable';
import 'rxjs/add/operator/map';
import 'rxjs/add/observable/of';
import 'rxjs/add/operator/catch';
import 'rxjs/add/observable/throw';

import { UsersService } from '../_services/data/users.service';

@Component({
  selector: 'profile',
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.scss'],
})
export class ProfileComponent {
  private sub: any;
  profileId: string;
  user: any;

  constructor(private router: Router,
              private route: ActivatedRoute,
              private _usersService: UsersService) {
  }

  ngOnInit() {
    this.sub = this.route.params.subscribe(params => {
      this.profileId = params['id'];

      this._usersService.getUser(this.profileId).subscribe(
        (data) => { this.user = data; console.info(this.user); },
        (err) => console.error(err)
      );

    });
  }
}
