import { NgModule } from '@angular/core';
import { CommonModule, CurrencyPipe } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';

// pipes
import { StatisticComponent } from './statistic.component';
import { StatisticRoutingModule } from './statistic.routing';

import { DataSharedModule } from '../app.shared.module';

@NgModule({
  imports: [
    StatisticRoutingModule,
    CommonModule,
    FormsModule,
    DataSharedModule
  ],
  declarations: [
    StatisticComponent
  ],
  providers: [
    CurrencyPipe
  ],
})
export class StatisticModule { }
