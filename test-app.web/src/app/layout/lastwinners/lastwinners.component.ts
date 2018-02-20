import { Component } from '@angular/core';
import { BehaviorSubject } from 'rxjs/BehaviorSubject';
import { Observable } from 'rxjs/Observable';
import 'rxjs/add/operator/filter';

import { LastWinner } from './lastwinners.model';

@Component({
  selector: 'app-lastwinners',
  templateUrl: './lastwinners.component.html',
  styleUrls: ['./lastwinners.component.scss']
})
export class LastwinnersComponent {
  winners: LastWinner[] = [];

  constructor() {
    this.winners.push({
      user_id: "1",
      skin_image: "https://steamcommunity-a.akamaihd.net/economy/image/class/578080/2451624329/82fx82f",
      skin_name: "School Shirt",
      skin_rarity: "uncommon",
      user_name: "TestUser1",
      case_name: "Case name #1"
    });

    this.winners.push({
      user_id: "1",
      skin_image: "https://steamcommunity-a.akamaihd.net/economy/image/class/578080/2451623345/82fx82f",
      skin_name: "Vintage Baseball Hat (Black)",
      skin_rarity: "common",
      user_name: "TestUser2",
      case_name: "Case name #1"
    });

    this.winners.push({
      user_id: "1",
      skin_image: "https://steamcommunity-a.akamaihd.net/economy/image/class/578080/2451623345/82fx82f",
      skin_name: "Vintage Baseball Hat (Black)",
      skin_rarity: "common",
      user_name: "TestUser2",
      case_name: "Case name #1"
    });

    this.winners.push({
      user_id: "1",
      skin_image: "https://steamcommunity-a.akamaihd.net/economy/image/class/578080/2451623345/82fx82f",
      skin_name: "Vintage Baseball Hat (Black)",
      skin_rarity: "common",
      user_name: "TestUser2",
      case_name: "Case name #1"
    });

    this.winners.push({
      user_id: "1",
      skin_image: "https://steamcommunity-a.akamaihd.net/economy/image/class/578080/2451623345/82fx82f",
      skin_name: "Vintage Baseball Hat (Black)",
      skin_rarity: "common",
      user_name: "TestUser2",
      case_name: "Case name #1"
    });

    this.winners.push({
      user_id: "1",
      skin_image: "https://steamcommunity-a.akamaihd.net/economy/image/class/578080/2451624329/82fx82f",
      skin_name: "School Shirt",
      skin_rarity: "uncommon",
      user_name: "TestUser1",
      case_name: "Case name #1"
    });

    this.winners.push({
      user_id: "1",
      skin_image: "https://steamcommunity-a.akamaihd.net/economy/image/class/578080/2451623345/82fx82f",
      skin_name: "Vintage Baseball Hat (Black)",
      skin_rarity: "common",
      user_name: "TestUser2",
      case_name: "Case name #1"
    });

    this.winners.push({
      user_id: "1",
      skin_image: "https://steamcommunity-a.akamaihd.net/economy/image/class/578080/2451623345/82fx82f",
      skin_name: "Vintage Baseball Hat (Black)",
      skin_rarity: "common",
      user_name: "TestUser2",
      case_name: "Case name #1"
    });
  }
}
