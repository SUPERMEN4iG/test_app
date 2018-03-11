import { NgModule } from '@angular/core';
import { Http } from '@angular/http';
import { CommonModule } from '@angular/common';

import { CasesService } from './_services/data/cases.service';
import { UsersService } from './_services/data/users.service';
import { PaymentService } from './_services/data/payment.service';
import { LastWinnersService } from './_services/data/lastwinners.service';

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
        UsersService,
        PaymentService,
        LastWinnersService
      ]
    }
  }
}
