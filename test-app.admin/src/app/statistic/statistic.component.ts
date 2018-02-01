import { Component, Input, ViewChild, TemplateRef, EventEmitter, Output, ElementRef } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import {NgbModal, ModalDismissReasons} from '@ng-bootstrap/ng-bootstrap';

import { AuthenticationService } from '../_services/authentication.service';

import { animate, AnimationBuilder, style } from '@angular/animations';

import { ToastrService } from 'ngx-toastr';
import { AuthenticationManagerService } from '../_services/authentication.manager.service';

@Component({
  selector: 'statistic',
  templateUrl: './statistic.component.html',
  styleUrls: ['./statistic.component.scss'],
})
export class StatisticComponent {
  private sub: any;

  constructor(private router: Router,
              private route: ActivatedRoute) {
  }

  ngOnInit() {
    this.sub = this.route.params.subscribe(params => {

    });
  }
}
