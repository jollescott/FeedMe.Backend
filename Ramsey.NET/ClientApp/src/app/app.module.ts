import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { RouterModule, Routes } from '@angular/router';
import { JwtInterceptor } from './helpers';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { NgJsonEditorModule } from 'ang-jsoneditor';

import { AppComponent } from './app.component';

import { RamseyService } from './services/ramsey.service';
import { AuthService } from './services/auth.service';
import { LoginComponent } from './login/login.component';
import { PageNotFoundComponent } from './page-not-found/page-not-found.component';

import { ChefModule } from './chef/chef.module';
import { PrivacyComponent } from './privacy/privacy.component';

export const APP_ROUTES: Routes = [
  { path: 'login', component: LoginComponent },
  { path: 'privacy', component: PrivacyComponent},
  { path: '**', component: PageNotFoundComponent },
];

@NgModule({
  declarations: [
    AppComponent,
    LoginComponent,
    PageNotFoundComponent,
    PrivacyComponent
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    HttpClientModule,
    FormsModule,
    RouterModule.forRoot(APP_ROUTES),
    NgJsonEditorModule,
    ChefModule
  ],
  providers: [
    RamseyService,
    AuthService,
    { provide: HTTP_INTERCEPTORS, useClass: JwtInterceptor, multi: true },
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
