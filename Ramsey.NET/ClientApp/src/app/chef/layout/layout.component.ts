import { Component, OnInit } from '@angular/core';
import { AuthService } from '../../services/auth.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-layout',
  template: `
    <h1>Dashboard Layout</h1>
    <p>
      <a routerLink="test" >home</a> |
      <a routerLink="config"> config </a>
      <button (click)="logout()">sign out</button>
    </p>

    <router-outlet></router-outlet>
  `,
  styles: []
})
export class LayoutComponent implements OnInit {

  constructor(private auth: AuthService, private router: Router) { }

  ngOnInit() {
  }

  logout() {
    this.auth.logout();
    this.router.navigate(['/login']);
  }

}
