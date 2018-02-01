import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

// Components
import { FullLayoutComponent } from './layout/full-layout.component';

import { MainComponent } from './pages/main/main.component';

const routes: Routes = [
  {
    path: '',
    component: FullLayoutComponent,
    children: [
      {
        path: '',
        component: MainComponent,
      },
      {
        path: 'cases',
        loadChildren: './cases/cases.module#CasesModule'
      },
      {
        path: 'statistic',
        loadChildren: './statistic/statistic.module#StatisticModule'
      },
    ]
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
