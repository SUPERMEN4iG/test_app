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
  selector: 'case-edit',
  templateUrl: './case-edit.component.html',
  styleUrls: ['./case-edit.component.scss'],
})
export class CasesEditComponent {
  private sub: any;
  id: number;

  margine: number;

  skins;
  currentCase;
  selectedSkin;
  currentCaseStatistic;

  modalRef: BsModalRef;

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

            this._skinsService.data$.subscribe((sk) => {
              if (sk.length > 0) {
                this.skins = sk;
                this.selectedSkin = this.skins[0].id;
              }
            }, (err) => { console.error(err); });
          }
        },
        (err) => {
          console.error(err);
        }
      );
    });
  }

  setDecimal($event) {
    $event.target.value = parseFloat($event.target.value);
  }

  addSkin() {
    this.currentCase.skins.push(_.findWhere(this.skins, { id: this.selectedSkin }));
    this.selectedSkin = null;
  }

  onRemoveSkin(skin) {
    this.currentCase.skins = _.without(this.currentCase.skins, skin);
  };

  onSave() {
    console.info(this.currentCase);

    this._caseService.saveCase(this.currentCase).subscribe(
      (data) => {
        this._notificationService.success("Saved", `Case ${this.currentCase.fullName} (${this._currencyPipe.transform(this.currentCase.price)})`);
      },
      (err) => {
        this._notificationService.error(err.message, `Case ${this.currentCase.fullName} (${this._currencyPipe.transform(this.currentCase.price)})`);
      }
    );
  }
  //TODO: надо здесь сделать выбор маржи под которую хотим пересчитать! ))
  onCaclulateChances() {
    this._caseService.caclulateChances(this.currentCase.id, this.margine).subscribe(
      (data) => {
        let i = 0;
        var s = _.map(this.currentCase.skins, (skin) => {
          skin.chance = data[i].chance;
          i++;
          return skin;
        });
        this._notificationService.success("Recalculation was successful, please save", `Case ${this.currentCase.fullName} (${this._currencyPipe.transform(this.currentCase.price)})`);
      },
      (err) => {
        console.error(err);
      }
    );
  }

  openModal(template: TemplateRef<any>) {
    this.modalRef = this.modalService.show(template);
  }

  onTestCaseOpen(casea, count, template: TemplateRef<any>) {
    this.currentCaseStatistic = {
      title: `Test ${count} open ${casea.fullName}`,
      totals: {},
      data: []
    };
    this._caseService.testOpenCase(casea.id, count).subscribe(
      (data) => {
        this.openModal(template);
        this.currentCaseStatistic.data = data.result;
        this.currentCaseStatistic.totals = data.totals;
        this.currentCaseStatistic.totals.totalIncome = data.totals.totalCasePrice - data.totals.totalSkinPrice;
        console.info(data);
      },
      (err) => {
        console.error(err);
      }
    );
  }
}
