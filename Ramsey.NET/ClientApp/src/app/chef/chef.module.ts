import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { LayoutComponent } from './layout/layout.component';
import { TestComponent } from './test/test.component';
import { ConfigComponent } from './config/config.component';
import { Routes, RouterModule } from '@angular/router';
import { AuthGuard } from '../guards/auth-guard.service';
import { NgJsonEditorModule } from 'ang-jsoneditor';

export const dashboardRoutes: Routes = [
  {
    path: 'chef',
    component: LayoutComponent,
    canActivate: [AuthGuard],
    children: [
      { path: '', redirectTo: 'test', pathMatch: 'full' },
      { path: 'test', component: TestComponent },
      { path: 'config', component: ConfigComponent }
    ]
  }
];

@NgModule({
  imports: [
    CommonModule,
    RouterModule.forChild(dashboardRoutes),
    NgJsonEditorModule
  ],
  declarations: [LayoutComponent, TestComponent, ConfigComponent],
  providers: [AuthGuard]
})
export class ChefModule { }
