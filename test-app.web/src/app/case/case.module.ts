import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';

import { CaseRoutingModule } from './case.routing';

import { CaseComponent } from './case.component';

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
  ],
  providers: [
  ],
})
export class CaseModule { }
