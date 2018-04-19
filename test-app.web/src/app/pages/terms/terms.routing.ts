import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { TermsComponent } from './terms.component';

const routes: Routes = [
  {
    path: '',
    data: {
      title: 'Terms'
    },
    children: [
      {
        path: '',
        component: TermsComponent,
        pathMatch: 'full'
      },
    ],
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class TermsRoutingModule {}
