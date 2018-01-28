import { NgModule } from '@angular/core';
import { Http } from '@angular/http';
import { CommonModule } from '@angular/common';

import { CasesService } from './_services/data/cases.service';
import { SkinsService } from './_services/data/skins.service';

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
        CasesService,
        SkinsService
      ]
    }
  }
}
