import { NgModule } from '@angular/core';
import { CommonModule, CurrencyPipe } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';

import { TermsComponent } from './terms.component';
import { TermsRoutingModule } from './terms.routing';

import { DataSharedModule } from '../../app.shared.module';

@NgModule({
  imports: [
    TermsRoutingModule,
    CommonModule,
    FormsModule,
    DataSharedModule
  ],
  declarations: [
    TermsComponent,
  ],
  providers: [
    CurrencyPipe
  ],
})
export class TermsModule { }
