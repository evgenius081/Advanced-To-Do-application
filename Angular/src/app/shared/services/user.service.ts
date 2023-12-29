import { Injectable, WritableSignal, signal } from '@angular/core';
import { UserLogin } from '../classes/user/user-login';
import { Observable, of, catchError, Subject } from 'rxjs';
import { HttpService } from './http.service';
import { TokenService } from './token.service';
import { Router } from '@angular/router';
import { TokenType } from '../enums/token-type';
import * as moment from 'moment';

@Injectable({
  providedIn: 'root',
})
export class UserService {
  isLoggedInSignal$: WritableSignal<boolean> = signal<boolean>(false);
  usernameSignal$: WritableSignal<string | undefined> = signal<string | undefined>(undefined);

  constructor(
    private tokenService: TokenService,
    private httpService: HttpService,
    private router: Router
  ) {
    if (
      this.tokenService.getToken(TokenType.ACCESS) &&
      this.checkIfTokenExpired()
    ) {
      this.usernameSignal$.set(this.tokenService.getUserNameFromToken());
      this.isLoggedInSignal$.set(true);
    } else {
      this.tokenService.removeToken(TokenType.ACCESS);
      this.tokenService.removeToken(TokenType.REFRESH);
    }
  }

  login(user: UserLogin): Observable<object> {
    return this.httpService
      .postWithoutToken('users/login', user)
      .pipe(catchError(this.handleError<any>()));
  }

  register(user: UserLogin): Observable<object> {
    return this.httpService
      .postWithoutToken('users/register', user)
      .pipe(catchError(this.handleError<object>()));
  }

  logout(): void {
    this.tokenService.removeToken(TokenType.ACCESS);
    this.tokenService.removeToken(TokenType.REFRESH);
    this.usernameSignal$.set(undefined);
    this.isLoggedInSignal$.set(false);
  console.log(this.isLoggedInSignal$())
  }

  checkIfTokenExpired(): boolean {
    const expiresAt: moment.Moment | undefined =
      this.tokenService.getExpirationDateFromToken();
    return (
      expiresAt !== undefined &&
      expiresAt.isBefore(moment())
    );
  }

  handleError<T>(
    operation = 'operation',
    result?: T
  ): (error: any) => Observable<T> {
    return (error: any): Observable<T> => {
      this.logout();
      this.router.navigate(['/login']);
      console.error(error);
      return of(result as T);
    };
  }
}
