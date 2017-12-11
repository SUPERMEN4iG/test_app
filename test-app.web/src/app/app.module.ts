import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';

import { HttpClientModule } from '@angular/common/http';

import { AppRoutingModule } from './app.routing';

// Config
import { APP_CONFIG, AppConfig } from './app.config';

// Auth modules
import { AuthenticationService } from './_services/authentication.service';

// Http Interceptors
import { HTTP_INTERCEPTORS, JsonpInterceptor } from '@angular/common/http';
import { TokenInterceptor } from './_services/TokenInterceptor';
import { JwtInterceptor } from './_services/JwtInterceptor';

// Components
import { AppComponent } from './app.component';
import { FullLayoutComponent } from './layout/full-layout.component';

@NgModule({
  declarations: [
    AppComponent,
    FullLayoutComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule
  ],
  providers: [
    { provide: HTTP_INTERCEPTORS, useClass: TokenInterceptor, multi: true },
    AuthenticationService,
    {
      provide: APP_CONFIG,
      useValue: AppConfig
    },
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
