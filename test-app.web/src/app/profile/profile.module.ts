import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';

import { ProfileRoutingModule } from './profile.routing';

import { ProfileComponent } from './profile.component';

import { DataSharedModule } from '../app.shared.module';

@NgModule({
  imports: [
    ProfileRoutingModule,
    CommonModule,
    FormsModule,
    DataSharedModule
  ],
  declarations: [
    ProfileComponent,
  ],
  providers: [
  ],
})
export class ProfileModule { }
