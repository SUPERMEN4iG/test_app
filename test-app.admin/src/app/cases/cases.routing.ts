import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { CasesComponent } from './cases.component';

import { CasesListComponent  } from './list/cases-list.component';
import { CasesEditComponent } from './edit/case-edit.component';

const routes: Routes = [
  {
    path: '',
    data: {
      title: 'Apps'
    },
    children: [
      {
        path: '',
        pathMatch: 'full',
        redirectTo: 'list'
      },
      {
        path: 'list',
        component: CasesListComponent,
      },
      {
        path: ':id',
        data: {
          title: ' '
        },
        children: [
          {
            path: '',
            pathMatch: 'full',
            redirectTo: 'edit'
          },
          {
            path: 'edit',
            component: CasesEditComponent,
          },
        ]
      }
    ],
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class CasesRoutingModule {}
