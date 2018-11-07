import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { RouterModule } from '@angular/router';

import { AppComponent } from './app.component';
import { FetchDataComponent } from './fetch-data/fetch-data.component';
import { TestComponent } from './test/test.component'

import { GusteauService } from './gusteau.service'

@NgModule({
  declarations: [
    AppComponent,
    FetchDataComponent,
    TestComponent
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    HttpClientModule,
    FormsModule,
    RouterModule.forRoot([
      { path: '', component: TestComponent, pathMatch: 'full' },
    ])
  ],
  providers: [GusteauService],
  bootstrap: [AppComponent]
})
export class AppModule { }
