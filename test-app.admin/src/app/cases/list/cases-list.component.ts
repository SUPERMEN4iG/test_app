import { Component, Input, ViewChild, TemplateRef, EventEmitter, Output, ElementRef } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import {NgbModal, ModalDismissReasons} from '@ng-bootstrap/ng-bootstrap';

import { AuthenticationService } from '../../_services/authentication.service';

import { animate, AnimationBuilder, style } from '@angular/animations';

import { CasesService } from '../../_services/data/cases.service';

import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'case-list',
  templateUrl: './cases-list.component.html',
  styleUrls: ['./cases-list.component.scss'],
})
export class CasesListComponent {
  cases = null;

  constructor(private router: Router,
              private route: ActivatedRoute,
              private _casesService: CasesService) {
    this._casesService.data$.subscribe(
      (data) => {
        this.cases = data;
      }
    );
  }

  ngOnInit() {

  }
}
