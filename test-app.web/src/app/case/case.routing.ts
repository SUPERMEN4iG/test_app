import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { CaseComponent } from './case.component';

const routes: Routes = [
  {
    path: '',
    data: {
      title: 'Apps'
    },
    children: [
      {
        path: '',
        pathMatch: 'full'
      },
      {
        path: ':name',
        component: CaseComponent,
        data: {
          title: ''
        }
      }
    ],
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class CaseRoutingModule {}
