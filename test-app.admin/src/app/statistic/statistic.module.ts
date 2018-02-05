import { NgModule } from '@angular/core';
import { CommonModule, CurrencyPipe } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';

// pipes
import { StatisticComponent } from './statistic.component';
import { StatisticRoutingModule } from './statistic.routing';

import { DataSharedModule } from '../app.shared.module';

import { SortPipeModule } from '../_pipes/sort/sort.module';

// Modal Component
import { ModalModule } from 'ngx-bootstrap/modal';

@NgModule({
  imports: [
    StatisticRoutingModule,
    CommonModule,
    FormsModule,
    DataSharedModule,
    ModalModule.forRoot(),
    SortPipeModule
  ],
  declarations: [
    StatisticComponent,
  ],
  providers: [
    CurrencyPipe
  ],
})
export class StatisticModule { }
