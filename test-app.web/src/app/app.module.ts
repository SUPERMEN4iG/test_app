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
import { MainService } from './_services/data/main.service';

// Interface
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';

import { BrowserAnimationsModule } from '@angular/platform-browser/animations';

import { ToastrModule } from 'ngx-toastr';
import { AuthenticationManagerService } from './_services/authentication.manager.service';
import { WinnerService } from './_services/data/winner.service';

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
    NgbModule.forRoot(),
    BrowserAnimationsModule,
    ToastrModule.forRoot({
      timeOut: 5000,
      positionClass: 'toast-bottom-right',
      preventDuplicates: true,
    })
  ],
  providers: [
    { provide: HTTP_INTERCEPTORS, useClass: TokenInterceptor, multi: true },
    { provide: HTTP_INTERCEPTORS, useClass: JwtInterceptor, multi: true },
    AuthenticationService,
    AuthenticationManagerService,
    CasesService,
    WinnerService,
    UsersService,
    MainService,
    {
      provide: APP_CONFIG,
      useValue: AppConfig
    },
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
