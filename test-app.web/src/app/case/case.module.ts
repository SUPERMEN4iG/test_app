import { NgModule } from '@angular/core';
import { CommonModule, CurrencyPipe } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';

import { CaseRoutingModule } from './case.routing';

import { CaseComponent } from './case.component';
import { RouletteComponent } from './roulette/roulette.component';

import { DataSharedModule } from '../app.shared.module';

@NgModule({
  imports: [
    CaseRoutingModule,
    CommonModule,
    FormsModule,
    DataSharedModule
  ],
  declarations: [
    CaseComponent,
    RouletteComponent
  ],
  providers: [
    CurrencyPipe
  ],
})
export class CaseModule { }
