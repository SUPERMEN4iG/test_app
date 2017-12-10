import { Component, OnInit, AfterViewInit, ViewChild } from '@angular/core';
import { Route, Router } from '@angular/router';

@Component({
  templateUrl: './full-layout.component.html',
  styleUrls: ['./full-layout.component.scss'],
})
export class FullLayoutComponent implements OnInit, AfterViewInit {

  constructor(private router: Router) {

  }

  ngOnInit() {

  }

  ngAfterViewInit() {
    console.info('Layout.Init()');
  }
}
