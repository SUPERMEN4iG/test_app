import { Component, Input, ViewChild, TemplateRef, EventEmitter, Output, ElementRef } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import {NgbModal, ModalDismissReasons} from '@ng-bootstrap/ng-bootstrap';

import { AuthenticationService } from '../_services/authentication.service';
import { UsersService } from '../_services/data/users.service';

import { CasesService } from '../_services/data/cases.service';

import { RouletteComponent } from './roulette/roulette.component';

import { animate, AnimationBuilder, style } from '@angular/animations';

import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'case',
  templateUrl: './case.component.html',
  styleUrls: ['./case.component.scss'],
})
export class CaseComponent {
  private sub: any;
  caseName: string;
  currentCase: any;
  currentUser: any;
  winSkin: any;

  isSpinning = false;
  isOpenCaseClick = false;

  @ViewChild("modalWinContent") modalWinContent : TemplateRef<any>;

  @ViewChild('roulette') roulette: RouletteComponent;

  @ViewChild('caseContainer') caseContainer: ElementRef;
  @ViewChild('rouletteContainer') rouletteContainer: ElementRef;

  constructor(private router: Router,
              private route: ActivatedRoute,
              private caseService: CasesService,
              private modalService: NgbModal,
              private _authService: AuthenticationService,
              private _userService: UsersService,
              private animBuilder: AnimationBuilder,
              private _notification: ToastrService) {
  }

  ngOnInit() {
    this.sub = this.route.params.subscribe(params => {
      this.caseName = params['name'];

      this._authService.currentUser$.subscribe((u) => { this.currentUser = u; });

      this.caseService.data$.subscribe(
        (data) => {
          if (data.length == 0) return;

          this.currentCase = this.caseService.getByCaseName(this.caseName);
          console.info(this.currentCase);
      });

    });
  }

  openWinModal() {
    this.modalService.open(this.modalWinContent).result.then((result) => {
    }, (reason) => {
    });
  }

  animationPlayer;
  setOpacity(container, value, secs, onDone = null): void {
    const moveBallAnimation = this.animBuilder.build([
      animate(`${secs}s ease`, style({
          'opacity': `${value}`,
          'display': `${ value == 0 ? 'none' : 'block' }`
      }))
    ]);
    this.animationPlayer = moveBallAnimation.create(container);
    this.animationPlayer.onDone((evt) => { if (onDone) { onDone(); } });
    this.animationPlayer.play();
  }

  openCase() {

    this.setOpacity(this.caseContainer.nativeElement, 0, 0.5, () => {
      this.isOpenCaseClick = true;

      this.setOpacity(this.rouletteContainer.nativeElement, 1, 0.5, () => {
        this.roulette.spin();
      });
    });

    // this.caseService.openCase(this.currentCase.id).subscribe(
    //   (data) => {
    //     this.winSkin = data.winner;
    //     this.openWinModal();
    //     this._authService.updateUser('balance', (this._authService.currentUser.balance - this.currentCase.price));
    //     this._userService.appnedWin(this._authService.currentUser.id, this.winSkin);
    //   },
    //   (err) => {
    //     console.error(err);
    //   }
    // );
  }
}
