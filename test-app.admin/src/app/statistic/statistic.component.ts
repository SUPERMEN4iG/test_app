import { Component, Input, ViewChild, TemplateRef, EventEmitter, Output, ElementRef } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import {NgbModal, ModalDismissReasons} from '@ng-bootstrap/ng-bootstrap';

import { AuthenticationService } from '../_services/authentication.service';

import { animate, AnimationBuilder, style } from '@angular/animations';

import { ToastrService } from 'ngx-toastr';
import { AuthenticationManagerService } from '../_services/authentication.manager.service';
import { StatisticService } from '../_services/data/statistic.service';

import { BsModalService } from 'ngx-bootstrap/modal';
import { BsModalRef } from 'ngx-bootstrap/modal/bs-modal-ref.service';

@Component({
  selector: 'statistic',
  templateUrl: './statistic.component.html',
  styleUrls: ['./statistic.component.scss'],
})
export class StatisticComponent {
  private sub: any;

  statistic;

  modalRef: BsModalRef;

  currentCase;
  currentCaseStatistic

  constructor(private router: Router,
              private route: ActivatedRoute,
              private _statisticService: StatisticService,
              private modalService: BsModalService) {
  }

  openModal(template: TemplateRef<any>) {
    this.modalRef = this.modalService.show(template);
  }

  ngOnInit() {
    this._statisticService.data$.subscribe(
      (data) => {
        this.statistic = data;
        console.info(this.statistic);
      },
      (err) => { console.error(err); }
    );

    this.sub = this.route.params.subscribe(params => {

    });
  }

  onClickDropRate(casea, template: TemplateRef<any>) {
    this.currentCase = casea;
    this.currentCaseStatistic = [];
    this._statisticService.getDropRate(casea.id).subscribe(
      (data) => {
        this.openModal(template);
        this.currentCaseStatistic = data;
        console.info(data);
      },
      (err) => {
        console.error(err);
      }
    );
  }
}
