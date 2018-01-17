import { Component, Input, ViewChild, TemplateRef, EventEmitter, Output } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import {NgbModal, ModalDismissReasons} from '@ng-bootstrap/ng-bootstrap';

import { AuthenticationService } from '../_services/authentication.service';
import { UsersService } from '../_services/data/users.service';

import { CasesService } from '../_services/data/cases.service';

import { RouletteComponent } from './roulette/roulette.component';

@Component({
  selector: 'case',
  templateUrl: './case.component.html',
  styleUrls: ['./case.component.scss'],
})
export class CaseComponent {
  private sub: any;
  caseName: string;
  currentCase: any;
  winSkin: any;

  isSpinning = false;
  isOpenCaseClick = false;

  @ViewChild("modalWinContent") modalWinContent : TemplateRef<any>;

  @ViewChild('roulette') roulette: RouletteComponent;

  constructor(private router: Router,
              private route: ActivatedRoute,
              private caseService: CasesService,
              private modalService: NgbModal,
              private _authService: AuthenticationService,
              private _userService: UsersService) {
  }

  ngOnInit() {
    this.sub = this.route.params.subscribe(params => {
      this.caseName = params['name'];

      this.caseService.data$.subscribe(
        (data) => {
          if (data.length == 0) return;

          this.currentCase = this.caseService.getByCaseName(this.caseName);
      });

    });
  }

  openWinModal() {
    this.modalService.open(this.modalWinContent).result.then((result) => {
    }, (reason) => {
    });
  }

  openCase() {
    this.isOpenCaseClick = true;
    this.roulette.spin();
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
