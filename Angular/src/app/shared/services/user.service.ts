import { Injectable } from '@angular/core';
import { User } from '../classes/user';
import { Observable, of } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class UserService {
  isLoggedIn = false;
  username?: string;
  constructor() {
    this.isLoggedIn = this.checkToken();
    if (this.isLoggedIn) {
      this.username = localStorage.getItem('TodoCurrentUser')!;
    }
  }

  checkToken() {
    return (
      localStorage.getItem('TodoCurrentUser') != null &&
      localStorage.getItem('TodoCurrentUser') != ''
    );
  }

  login(user: User): Observable<User | undefined> {
    if (user.login != 'admin' || user.password != 'P@55w0rd') {
      return of(undefined);
    }
    localStorage.setItem('TodoCurrentUser', user.login);
    this.username = user.login;
    this.isLoggedIn = true;
    return of(user);
  }

  register(user: User) {
    if (user.login != 'admin' || user.password != 'P@55w0rd') {
      return of(undefined);
    }
    return of(user);
  }

  logout() {
    this.isLoggedIn = false;
    localStorage.setItem('TodoCurrentUser', '');
    this.username = undefined;
  }

  private refreshToken() {}
}
