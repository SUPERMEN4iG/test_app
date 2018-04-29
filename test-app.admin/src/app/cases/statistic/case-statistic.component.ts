import { Component, Input, ViewChild, TemplateRef, EventEmitter, Output, ElementRef } from '@angular/core';
import { CurrencyPipe } from '@angular/common';
import { Router, ActivatedRoute } from '@angular/router';
import {NgbModal, ModalDismissReasons} from '@ng-bootstrap/ng-bootstrap';

import { AuthenticationService } from '../../_services/authentication.service';

import { animate, AnimationBuilder, style } from '@angular/animations';

import { SkinsService } from '../../_services/data/skins.service';
import { CasesService } from '../../_services/data/cases.service';

import { ToastrService } from 'ngx-toastr';
import { BsModalService } from 'ngx-bootstrap/modal';
import { BsModalRef } from 'ngx-bootstrap/modal/bs-modal-ref.service';

import _ from 'underscore';

@Component({
  selector: 'case-statistic',
  templateUrl: './case-statistic.component.html',
  styleUrls: ['./case-statistic.component.scss'],
})
export class CasesStatisticComponent {
  private sub: any;
  id: number;
  currentCase;
  currentStatistic;

  constructor(private router: Router,
              private route: ActivatedRoute,
              private _skinsService: SkinsService,
              private _caseService: CasesService,
              private modalService: BsModalService,
              private _notificationService: ToastrService,
              private _currencyPipe: CurrencyPipe) {
  }

  ngOnInit() {
    this.sub = this.route.params.subscribe(params => {
      this.id = Number.parseInt(params['id']);

      this._caseService.data$.subscribe(
        (data) => {
          if (data.length > 0) {
            this.currentCase = this._caseService.getByCaseId(this.id);

            if (this.currentCase != null) {
              this._caseService.getStatistic(this.id).subscribe(
                (data) => {
                  console.info(data);
                  this.currentStatistic = data;
                },
                (err) => {
                  console.error(err);
                }
              );
            }
          }
        },
        (err) => {
          console.error(err);
        }
      );
    });
  }
}
