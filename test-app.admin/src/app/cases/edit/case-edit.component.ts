import { Component, Input, ViewChild, TemplateRef, EventEmitter, Output, ElementRef } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import {NgbModal, ModalDismissReasons} from '@ng-bootstrap/ng-bootstrap';

import { AuthenticationService } from '../../_services/authentication.service';

import { animate, AnimationBuilder, style } from '@angular/animations';

import { SkinsService } from '../../_services/data/skins.service';
import { CasesService } from '../../_services/data/cases.service';

import { ToastrService } from 'ngx-toastr';

import _ from 'underscore';

@Component({
  selector: 'case-edit',
  templateUrl: './case-edit.component.html',
  styleUrls: ['./case-edit.component.scss'],
})
export class CasesEditComponent {
  private sub: any;
  id: number;

  skins;
  currentCase;
  selectedSkin;

  constructor(private router: Router,
              private route: ActivatedRoute,
              private _skinsService: SkinsService,
              private _caseService: CasesService) {
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
        console.info(data);
      },
      (err) => {
        console.error(err);
      }
    );
  }

  onCaclulateChances() {
    this._caseService.caclulateChances(this.currentCase.id).subscribe(
      (data) => {
        let i = 0;
        var s = _.map(this.currentCase.skins, (skin) => {
          skin.chance = data[i].chance;
          i++;
          return skin;
        });
        console.info(s);
      },
      (err) => {
        console.error(err);
      }
    );
  }
}
