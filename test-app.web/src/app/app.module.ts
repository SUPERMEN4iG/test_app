import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';

import { HttpClientModule } from '@angular/common/http';

import { AppRoutingModule } from './app.routing';

// Config
import { APP_CONFIG, AppConfig } from './app.config';

// core modules
import { DataSharedModule } from './app.shared.module';

// Auth modules
import { AuthenticationService } from './_services/authentication.service';

// Http Interceptors
import { HTTP_INTERCEPTORS, JsonpInterceptor } from '@angular/common/http';
import { TokenInterceptor } from './_services/TokenInterceptor';
import { JwtInterceptor } from './_services/JwtInterceptor';

// Components
import { AppComponent } from './app.component';
import { FullLayoutComponent } from './layout/full-layout.component';
import { MainComponent } from './pages/main/main.component';
import { CasesComponent } from './_components/cases/cases.component';

import { LastwinnersComponent } from './layout/lastwinners/lastwinners.component';

// Data services
import { CasesService } from './_services/data/cases.service';
import { UsersService } from './_services/data/users.service';

// Interface
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';


@NgModule({
  declarations: [
    AppComponent,
    FullLayoutComponent,
    LastwinnersComponent,
    CasesComponent,
    MainComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    DataSharedModule.forRoot(),
    NgbModule.forRoot()
  ],
  providers: [
    { provide: HTTP_INTERCEPTORS, useClass: TokenInterceptor, multi: true },
    AuthenticationService,
    CasesService,
    UsersService,
    {
      provide: APP_CONFIG,
      useValue: AppConfig
    },
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
