import { NgModule } from '@angular/core';
import { CommonModule, CurrencyPipe } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';

import { CasesRoutingModule } from './cases.routing';

import { CasesComponent } from './cases.component';

import { DataSharedModule } from '../app.shared.module';

@NgModule({
  imports: [
    CasesRoutingModule,
    CommonModule,
    FormsModule,
    DataSharedModule
  ],
  declarations: [
    CasesComponent
  ],
  providers: [
    CurrencyPipe
  ],
})
export class CasesModule { }
