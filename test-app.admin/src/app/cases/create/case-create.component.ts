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
import { SkinsAvailablePipe } from '../edit/case-edit-skins.pipe';

@Component({
  selector: 'case-create',
  templateUrl: './case-create.component.html',
  styleUrls: ['./case-create.component.scss'],
})
export class CaseCreateComponent {
  private sub: any;
  id: number;
  currentCase;
  currentStatistic;
  categories = [];
  skins = [];
  avalibleSkins = [];

  isDropProgress = false;

  constructor(private router: Router,
              private route: ActivatedRoute,
              private _skinsService: SkinsService,
              private _caseService: CasesService,
              private modalService: BsModalService,
              private _notificationService: ToastrService,
              private _currencyPipe: CurrencyPipe,
              private _skinsPipe: SkinsAvailablePipe) {
    this.buildCase();

    this._caseService.getCategories().subscribe(
      (categories) => {
        this.categories = categories;
      },
      (err) => {
        console.error(err);
      }
    );

    this._skinsService.data$.subscribe((sk) => {
      if (sk.length > 0) {
        this.skins = sk;
        this.avalibleSkins = sk;
      }
    }, (err) => { console.error(err); });
  }

  buildCase() {
    this.currentCase = {
      fullName: 'Simple Name',
      image: null,
      price: 0,
      skins: [],
      category: null,
      isNeedRecalc: false,
      isCaseCreator: true,
    };
  }

  onCategoryChange(category) {
    if (category == -1) {
      console.info('ADD MORE!');
    }
  }

  onFileChange($event): void {
    var file:File = $event.target.files[0];
    var reader:FileReader = new FileReader();

    reader.onloadend = (e) => {
      this.currentCase.image = reader.result;
    }
    reader.readAsDataURL(file);
  }

  onSkinAdd($event): void {
    this.currentCase.skins.push($event.dragData);
  }

  onDragEnter($event): void {
    console.info($event);
  }

  onDragStart($event): void {
    console.info('onDragStart()');
    this.isDropProgress = true;
  }

  onDragEnd($event): void {
    console.info('onDragEnd()');
    this.isDropProgress = false;
    this.onSkinChange();
  }

  onSkinChange() {
    this.skins = this._skinsPipe.transform(this.avalibleSkins, this.currentCase.skins);
  }

  onClickSave() {
    this._caseService.createCase(this.currentCase).subscribe(
      (data) => {
        console.info(data);
        this._notificationService.success("Created", `Case ${this.currentCase.fullName} (${this._currencyPipe.transform(this.currentCase.price)})`);
      },
      (err) => {
        console.info(err);
        this._notificationService.error(err.message, `Case ${this.currentCase.fullName} (${this._currencyPipe.transform(this.currentCase.price)})`);
      }
    );
  }

  ngOnInit() {
  }
}
