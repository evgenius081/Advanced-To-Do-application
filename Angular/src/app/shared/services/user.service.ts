import { Injectable } from '@angular/core';
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
  isLoggedIn$: Subject<boolean> = new Subject<boolean>();
  username$: Subject<string | undefined> = new Subject<string | undefined>();

  isLoggedIn: boolean = false;
  username: string | undefined = undefined;

  constructor(
    private tokenService: TokenService,
    private httpService: HttpService,
    private router: Router
  ) {
    if (
      this.tokenService.getToken(TokenType.ACCESS) &&
      this.checkIfTokenExpired()
    ) {
      this.username = this.tokenService.getUserNameFromToken();
      this.isLoggedIn = true;
      this.username$.next(this.tokenService.getUserNameFromToken());
      this.isLoggedIn$.next(true);
    } else {
      this.username = undefined;
      this.isLoggedIn = false;
      this.username$.next(undefined);
      this.isLoggedIn$.next(false);
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
    this.username$.next(undefined);
    this.isLoggedIn$.next(false);
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
