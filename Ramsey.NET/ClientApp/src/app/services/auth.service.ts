import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import decode from 'jwt-decode';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { retry } from 'rxjs/operators';

@Injectable()
export class AuthService {

  constructor(private http: HttpClient) { }

  /**
   * this is used to clear anything that needs to be removed
   */
  clear(): void {
    localStorage.clear();
  }

  /**
   * check for expiration and if token is still existing or not
   * @return {boolean}
   */
  isAuthenticated(): boolean {
    return localStorage.getItem('token') != null && !this.isTokenExpired();
  }

  // simulate jwt token is valid
  // https://github.com/theo4u/angular4-auth/blob/master/src/app/helpers/jwt-helper.ts
  isTokenExpired(): boolean {
    return false;
  }

  public async login(email: string, password: string): Promise<boolean> {
    const url = window.location.origin + '/admin/authenticate';

    console.log(url);

    const login = {
      Id: 0,
      Firstname: "",
      Lastname: "",
      Username: email,
      Password: password
    } as LoginDto;

    var admin = await this.http.post<Admin>(url, login).toPromise();

    console.log(admin);

    if (admin.token !== undefined) {
      localStorage.setItem('token', admin.token);
      return true;
    }
    else {
      return false;
    }
  }

  /**
   * this is used to clear local storage and also the route to login
   */
  logout(): void {
    this.clear();
  }

  decode() {
    return decode(localStorage.getItem('token'));
  }
}

export interface LoginDto {
  Id: number;
  Firstname: string;
  Lastname: string;
  Username: string;
  Password: string;
}

export interface Admin {
  id: number;
  username: string;
  firstname: string;
  lastname: string;
  token: string;
}
