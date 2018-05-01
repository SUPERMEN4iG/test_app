import { NgModule } from '@angular/core';
import { CommonModule, CurrencyPipe } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';

// pipes
import { SkinsAvailablePipe } from './edit/case-edit-skins.pipe';

import { CasesRoutingModule } from './cases.routing';

import { CasesComponent } from './cases.component';
import { CasesListComponent } from './list/cases-list.component';
import { CasesEditComponent } from './edit/case-edit.component';

import { SortPipeModule } from '../_pipes/sort/sort.module';

import { DataSharedModule } from '../app.shared.module';

// Modal Component
import { ModalModule } from 'ngx-bootstrap/modal';
import { CasesStatisticComponent } from './statistic/case-statistic.component';
import { CaseCreateComponent } from './create/case-create.component';
import { DndModule } from 'ng2-dnd';

@NgModule({
  imports: [
    CasesRoutingModule,
    CommonModule,
    FormsModule,
    DataSharedModule,
    SortPipeModule,
    ModalModule.forRoot(),
    DndModule,
  ],
  declarations: [
    CasesComponent,
    CasesListComponent,
    CasesEditComponent,
    CasesStatisticComponent,
    CaseCreateComponent,
    SkinsAvailablePipe
  ],
  providers: [
    CurrencyPipe,
    SkinsAvailablePipe
  ],
})
export class CasesModule { }
