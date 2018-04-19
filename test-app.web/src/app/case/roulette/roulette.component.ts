import { Component, Input, ViewChild, TemplateRef, ElementRef, EventEmitter, Output } from '@angular/core';
import { CurrencyPipe } from '@angular/common';
import { Router, ActivatedRoute } from '@angular/router';
import { NgbModal, ModalDismissReasons } from '@ng-bootstrap/ng-bootstrap';

import { AuthenticationService } from '../../_services/authentication.service';

import { UsersService } from '../../_services/data/users.service';
import { CasesService } from '../../_services/data/cases.service';
import { MainService } from '../../_services/data/main.service';

import { ToastrService } from 'ngx-toastr';

import { animate, AnimationBuilder, style } from '@angular/animations';

import { Skin } from './skin';
import { WinnerService } from '../../_services/data/winner.service';

@Component({
  selector: 'roulette',
  templateUrl: './roulette.component.html',
  styleUrls: ['./roulette.component.scss'],
})
export class RouletteComponent {

  SKIN_WIDTH = 140;
  SKINS_COUNT = 60;
  SKIN_PRIZE_ID = this.SKINS_COUNT - 10;
  SPIN_SECS = 10;

  @Input('skins')
  skins = [];
  skinsRoulette = [];

  countSpin = 0;

  @Input() isSpinning: boolean = false;
  @Output() isSpinningChange: EventEmitter<boolean> = new EventEmitter<boolean>();

  @Output() isStartAgain: EventEmitter<boolean> = new EventEmitter<boolean>();

  @Input() currentCase: any;

  isWinned: boolean = false;
  caseMessage: string = '';

  prize = null;
  microposX = 0;

  spingAudio = null;
  winAudio = null;
  itemActionAudio = null;
  startAudio = null;

  currentWinner = null;

  @ViewChild('casesCarusel') casesCarusel: ElementRef;

  constructor(private router: Router,
              private route: ActivatedRoute,
              private animBuilder: AnimationBuilder,
              private caseService: CasesService,
              private modalService: NgbModal,
              private _authService: AuthenticationService,
              private _userService: UsersService,
              private _mainService: MainService,
              private _notification: ToastrService,
              private _winnerService: WinnerService,
              private currencyPipe: CurrencyPipe) {
  }

  ngOnInit() {
    this.init();
  }

  init() {
    var skins = [];

    if (this.countSpin == 0) {
      this.moveRouletteToX(0, 1);
      this.skinsRoulette.splice(0, this.SKINS_COUNT);
      skins = this.fillSkins(0, this.SKIN_PRIZE_ID - 1);
      this.skinsRoulette = skins;
    } else {
      skins = this.skinsRoulette;
      skins.splice(0, this.SKIN_PRIZE_ID - 4);
      skins.splice(4 + 4 + 1, skins.length);
      skins = skins.concat(this.fillSkins(0, this.SKIN_PRIZE_ID - 1 - skins.length));

      this.skinsRoulette = skins;

      this.moveRouletteToX(-this.microposX, 0);
    }
  }

  shuffleArray(array) {
    var j, x, i;
    for (i = array.length - 1; i > 0; i--) {
        j = Math.floor(Math.random() * (i + 1));
        x = array[i];
        array[i] = array[j];
        array[j] = x;
    }

    return array;
  }

  fillSkins(from_i, to_i) {
    var i;
    var j = 0;
    var skins = [];
    for (i = from_i; i <= to_i; i += 1) {
        skins[i] = new Skin(
            i,
            this.skins[j]
        );
        j = (j === this.skins.length - 1) ? 0 : j + 1;
    }

    return this.shuffleArray(skins);
  }

  animationPlayer;
  moveRouletteToX(x, secs, onDone = null): void {
    const moveBallAnimation = this.animBuilder.build([
      animate(secs +'s ease', style({
          'transform': 'translateX('+x+'px)'
      }))
    ]);
    this.animationPlayer = moveBallAnimation.create(this.casesCarusel.nativeElement);
    this.animationPlayer.onDone((evt) => { if (onDone) { onDone(); } });
    this.animationPlayer.play();
  }

  floorN(x, n)
  {
    var mult = Math.pow(10, n);
    return Math.floor(x*mult)/mult;
  }

  sellItem() {
    let lastState = this.currentWinner.state;
    this.currentWinner.state = 1337;
    this._winnerService.sell(this.currentWinner.id).subscribe(
      (data) => {
        this.currentWinner.state = 1;
        this.currentWinner.price = data.data;
        this._userService.appnedWin(this._authService.currentUser.id, this.currentWinner);
        this._authService.updateUser('balance', (this._authService.currentUser.balance + this.currentWinner.price));
        this._notification.success(`Sold for ${this.currencyPipe.transform(this.currentWinner.price)}`, 'Sell item');

        this.itemActionAudio = new Audio();
        this.spingAudio.src = "../../../assets/sounds/item_action.mp3";
        this.spingAudio.load();
        this.spingAudio.play();
      },
      (err) => {
        this._notification.error(err.message, 'Sell item');
        this.currentWinner.state = lastState;
      }
    );
  }

  spin() {
    this.currentWinner = null;
    const templateCaseMessage = 'OPENNING CASE';
    this.caseMessage = templateCaseMessage;

    let caseMessageInterval = setInterval(() => {
      // console.info(this.caseMessage.charAt(this.caseMessage.length - 3));
      if (this.caseMessage.charAt(this.caseMessage.length - 3) == '.') {
        this.caseMessage = templateCaseMessage;
      } else {
        this.caseMessage += '.';
      }

    }, 300);

    this.startAudio = new Audio();
    this.startAudio.src = "../../../assets/sounds/start.mp3";
    this.startAudio.load();
    this.startAudio.play();

    this.caseService.openCase(this.currentCase.id).subscribe(
      (data) => {

        let winSkin = data.winner;
        this._authService.updateUser('balance', this.floorN(this._authService.currentUser.balance - this.currentCase.price, 2));
        this._userService.appnedWin(this._authService.currentUser.id, winSkin);

        this.isSpinning = true;
        this.isSpinningChange.emit(true);
        this.isWinned = false;

        if (this.countSpin >= 1) {
          this.init();
        }

        this.prize = new Skin(this.SKIN_PRIZE_ID, winSkin);

        let skins = this.skinsRoulette;
        skins[this.SKIN_PRIZE_ID] = this.prize;

        let afterSkins = this.fillSkins(0, 8);
        afterSkins.forEach(function(element, i) {
            skins.push(element);
        }, this);

        this.skinsRoulette = skins;
        this.microposX = Math.random() * (this.SKIN_WIDTH * 0.9 - this.SKIN_WIDTH * 0.1) + this.SKIN_WIDTH * 0.1;
        let prizePosition_x = (this.SKIN_WIDTH * this.SKIN_PRIZE_ID) -
                    ((this.SKIN_WIDTH) * 4) +
                    this.microposX;

        var currentTiming = 250;
        var currentTimeout = null;

        var startDate = new Date().getTime();

        var cElement = 1;
        var cElementWidth = 0;

        var testLooping = setInterval(() => {
          // var dateDiff = (1 + 10) / ((new Date().getTime() - startDate));
          // console.info(dateDiff);
          var rayX = -1 * (this.casesCarusel.nativeElement.getBoundingClientRect().x);
          cElementWidth = this.SKIN_WIDTH * cElement;
          // console.info(rayX);

          if (cElementWidth - this.SKIN_WIDTH / 2 <= rayX && cElementWidth + this.SKIN_WIDTH / 2 >= rayX) {
            this.spingAudio = new Audio();
            this.spingAudio.src = "../../../assets/sounds/scroll.wav";
            this.spingAudio.load();
            this.spingAudio.play();
            cElement++;
          }
        }, 1);

        var loopSpinning = () => {
          this.spingAudio = new Audio();
          this.spingAudio.src = "../../../assets/sounds/scroll.wav";
          this.spingAudio.load();
          this.spingAudio.play();

          if (currentTiming <= 340) {
             currentTiming += 5;
          } else {
            currentTiming += 100;
          }

          // console.info(currentTiming);

          if (currentTiming < 945) {
            currentTimeout = window.setTimeout(loopSpinning, currentTiming);
          } else {
            clearTimeout(currentTimeout);
          }
        };
        // loopSpinning();

        this.moveRouletteToX(-1 * prizePosition_x, this.SPIN_SECS, () => {
          this.countSpin++;
          this.isSpinning = false;
          this.isSpinningChange.emit(false);
          this.isStartAgain.emit(false);
          this.isWinned = true;
          clearTimeout(currentTimeout);
          clearInterval(caseMessageInterval);
          this._mainService.increseOpennedCases();
          this.currentWinner = winSkin;

          clearInterval(testLooping);

          setTimeout(() => {
            this.winAudio = new Audio();
            this.winAudio.src = "../../../assets/sounds/win.mp3";
            this.winAudio.load();
            this.winAudio.play();
          }, 200);
        });
      },
      (err) => {
        this._notification.error(err.message, 'Open case');
        this.caseMessage = err.message;
        clearInterval(caseMessageInterval);
        this.isStartAgain.emit(true);
      }
    );
  }
}
