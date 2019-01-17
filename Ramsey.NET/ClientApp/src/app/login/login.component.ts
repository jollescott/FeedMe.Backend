import { Component, OnInit } from '@angular/core';
import { AuthService } from '../services/auth.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styles: []
})
export class LoginComponent implements OnInit {

  constructor(private auth: AuthService, private router: Router) { }

  email: string;
  password: string;

  ngOnInit() {
  }

  login() {
    this.auth.login(this.email, this.password).then(x => {
      if (x) {
        this.router.navigate(['/chef']);
      }
      else {
        alert("Wrong credentials");
      }
    });
  }

}

interface ILoginFormData {
  email: string;
  password: string;
}
