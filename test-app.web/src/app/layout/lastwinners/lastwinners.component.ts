import { Component } from '@angular/core';
import { BehaviorSubject } from 'rxjs/BehaviorSubject';
import { Observable } from 'rxjs/Observable';
import 'rxjs/add/operator/filter';

import { trigger,style,transition,animate,keyframes,query,stagger, sequence } from '@angular/animations';

import { MainService } from '../../_services/data/main.service';
import { LastWinnersService } from '../../_services/data/lastwinners.service';
import { AuthenticationService } from '../../_services/authentication.service';

import { LastWinner } from './lastwinners.model';

@Component({
  selector: 'app-lastwinners',
  templateUrl: './lastwinners.component.html',
  styleUrls: ['./lastwinners.component.scss'],
  animations: [

    trigger('listAnimation', [
      transition('* => void', [
        style({ opacity: '1', transform: 'translateX(0)', 'box-shadow': '0 1px 4px 0 rgba(0, 0, 0, 0.3)'}),
        sequence([
          animate(".50s ease", style({ opacity: '.2', transform: 'translateX(20px)', 'box-shadow': 'none'  })),
          animate(".2s ease", style({ opacity: 0, transform: 'translateX(20px)', 'box-shadow': 'none'  }))
        ])
      ]),
      transition('void => *', [
        style({ opacity: '0', transform: 'translateX(20px)', 'box-shadow': 'none' }),
        sequence([
          animate(".2s ease", style({ opacity: '.2', transform: 'translateX(-20px)', 'box-shadow': 'none'  })),
          animate(".70s ease", style({ opacity: 1, transform: 'translateX(0)', 'box-shadow': '0 1px 4px 0 rgba(0, 0, 0, 0.3)'  }))
        ])
      ])
    ])

  ]
})
export class LastwinnersComponent {
  winners: LastWinner[] = [];

  constructor(private _mainService: MainService,
              private _lastWinnersService: LastWinnersService,
              private _authService: AuthenticationService) {
    this.winners.push(null);

    this._lastWinnersService.messages.subscribe((message) => {
      if (this._authService.currentUser == null || this._authService.currentUser.id == message.user_id) {
        setTimeout(() => {
          this.winners.pop();
          this.winners.unshift(message);
        }, 10000);
      } else {
        this.winners.pop();
        this.winners.unshift(message);
      }
    });
    // this.winners.push({
    //   user_id: "1",
    //   skin_image: "https://steamcommunity-a.akamaihd.net/economy/image/class/578080/2451624329/82fx82f",
    //   skin_name: "School Shirt",
    //   skin_rarity: "uncommon",
    //   user_name: "TestUser1",
    //   case_name: "Case name #1"
    // });

    // this.winners.push({
    //   user_id: "1",
    //   skin_image: "https://steamcommunity-a.akamaihd.net/economy/image/class/578080/2451623345/82fx82f",
    //   skin_name: "Vintage Baseball Hat (Black)",
    //   skin_rarity: "common",
    //   user_name: "TestUser2",
    //   case_name: "Case name #1"
    // });

    // this.winners.push({
    //   user_id: "1",
    //   skin_image: "https://steamcommunity-a.akamaihd.net/economy/image/class/578080/2451623345/82fx82f",
    //   skin_name: "Vintage Baseball Hat (Black)",
    //   skin_rarity: "common",
    //   user_name: "TestUser2",
    //   case_name: "Case name #1"
    // });

    // this.winners.push({
    //   user_id: "1",
    //   skin_image: "https://steamcommunity-a.akamaihd.net/economy/image/class/578080/2451623345/82fx82f",
    //   skin_name: "Vintage Baseball Hat (Black)",
    //   skin_rarity: "common",
    //   user_name: "TestUser2",
    //   case_name: "Case name #1"
    // });

    // this.winners.push({
    //   user_id: "1",
    //   skin_image: "https://steamcommunity-a.akamaihd.net/economy/image/class/578080/2451623345/82fx82f",
    //   skin_name: "Vintage Baseball Hat (Black)",
    //   skin_rarity: "common",
    //   user_name: "TestUser2",
    //   case_name: "Case name #1"
    // });

    // this.winners.push({
    //   user_id: "1",
    //   skin_image: "https://steamcommunity-a.akamaihd.net/economy/image/class/578080/2451624329/82fx82f",
    //   skin_name: "School Shirt",
    //   skin_rarity: "uncommon",
    //   user_name: "TestUser1",
    //   case_name: "Case name #1"
    // });

    // this.winners.push({
    //   user_id: "1",
    //   skin_image: "https://steamcommunity-a.akamaihd.net/economy/image/class/578080/2451623345/82fx82f",
    //   skin_name: "Vintage Baseball Hat (Black)",
    //   skin_rarity: "common",
    //   user_name: "TestUser2",
    //   case_name: "Case name #1"
    // });

    // this.winners.push({
    //   user_id: "1",
    //   skin_image: "https://steamcommunity-a.akamaihd.net/economy/image/class/578080/2451623345/82fx82f",
    //   skin_name: "Vintage Baseball Hat (Black)",
    //   skin_rarity: "common",
    //   user_name: "TestUser2",
    //   case_name: "Case name #1"
    // });
  }

  removeItem() {
    this.winners.pop();
  }

  pushItem() {
    this.winners.unshift({
      user_id: "1",
      skin_image: "https://steamcommunity-a.akamaihd.net/economy/image/class/578080/2451623345/82fx82f",
      skin_name: "Vintage Baseball Hat (Black)",
      skin_rarity: 1,
      user_name: "TestUser2",
      case_name: "Case name #1",
      case_static_name: "test-rare-2",
    });
  }

  ngOnInit() {
    this._mainService.data$.subscribe((data) => {
      this.winners = data.lastWinners;
    });

    // setInterval(() => {
    //   this.removeItem();
    //   this.pushItem();
    // }, 3000);
  }
}
