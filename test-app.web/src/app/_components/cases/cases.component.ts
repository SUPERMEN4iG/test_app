import { Component, OnInit, AfterViewInit, ViewChild, Inject } from '@angular/core';
import { Route, Router } from '@angular/router';

import { Observable } from 'rxjs/Observable';
import 'rxjs/add/operator/do';
import 'rxjs/add/operator/finally';

import { HttpClient } from '@angular/common/http';

import { CasesService } from '../../_services/data/cases.service';

@Component({
  selector: 'app-cases',
  templateUrl: './cases.component.html',
  styleUrls: ['./cases.component.scss'],
})
export class CasesComponent implements OnInit, AfterViewInit {

  cases = [];
  isLoading = false;

  constructor(private _casesService: CasesService) {
  }

  ngOnInit() {
    this.isLoading = true;
    this._casesService.data$.subscribe(
      (data) => {
        this.cases = data;
        this.isLoading = false;
      },
      (err) => console.error(err)
    );
  }

  ngAfterViewInit() {
    console.info('Cases.Init()');
  }
}
