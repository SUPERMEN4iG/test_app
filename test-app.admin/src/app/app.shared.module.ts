import { NgModule } from '@angular/core';
import { Http } from '@angular/http';
import { CommonModule } from '@angular/common';

@NgModule({
  imports:      [ CommonModule ],
  declarations: [],
  exports:      [ CommonModule ]
})
export class DataSharedModule {
  static forRoot() {
    return {
      ngModule: DataSharedModule,
      providers: [
      ]
    }
  }
}
