import { NgModule } from '@angular/core';
import { CommonModule, CurrencyPipe } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';

// pipes
import { SkinsAvailablePipe } from './edit/case-edit-skins.pipe';

import { CasesRoutingModule } from './cases.routing';

import { CasesComponent } from './cases.component';
import { CasesListComponent } from './list/cases-list.component';
import { CasesEditComponent } from './edit/case-edit.component';

import { DataSharedModule } from '../app.shared.module';

@NgModule({
  imports: [
    CasesRoutingModule,
    CommonModule,
    FormsModule,
    DataSharedModule
  ],
  declarations: [
    CasesComponent,
    CasesListComponent,
    CasesEditComponent,
    SkinsAvailablePipe
  ],
  providers: [
    CurrencyPipe
  ],
})
export class CasesModule { }
