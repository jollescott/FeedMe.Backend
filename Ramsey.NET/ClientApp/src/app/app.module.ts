import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { RouterModule, Routes } from '@angular/router';

import { AppComponent } from './app.component';
import { FetchDataComponent } from './fetch-data/fetch-data.component';
import { TestComponent } from './test/test.component'

import { RamseyService } from './services/ramsey.service';
import { AuthService } from './services/auth.service';
import { LoginComponent } from './login/login.component';
import { PageNotFoundComponent } from './page-not-found/page-not-found.component';

import { ChefModule } from './chef/chef.module';

export const APP_ROUTES: Routes = [
  { path: 'login', component: LoginComponent },
  { path: '', redirectTo: 'chef', pathMatch: 'full' },
  { path: '**', component: PageNotFoundComponent }
];

@NgModule({
  declarations: [
    AppComponent,
    FetchDataComponent,
    TestComponent,
    LoginComponent,
    PageNotFoundComponent
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    HttpClientModule,
    FormsModule,
    RouterModule.forRoot(APP_ROUTES),
    ChefModule
  ],
  providers: [RamseyService, AuthService],
  bootstrap: [AppComponent]
})
export class AppModule { }
