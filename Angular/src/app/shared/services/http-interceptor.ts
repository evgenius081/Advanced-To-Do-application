import { Injectable } from '@angular/core';
import {
  HttpEvent,
  HttpInterceptor,
  HttpHandler,
  HttpRequest,
  HttpHeaders,
} from '@angular/common/http';
import { catchError, Observable, switchMap, tap, throwError } from 'rxjs';
import { Token } from '../classes/token';
import { HttpService } from './http.service';
import { TokenService } from './token.service';
import { TokenType } from '../enums/token-type';

@Injectable({
  providedIn: 'root',
})
export class ApiInterceptor implements HttpInterceptor {
  constructor(
    private httpService: HttpService,
    private tokenService: TokenService
  ) {}

  intercept(
    req: HttpRequest<any>,
    next: HttpHandler
  ): Observable<HttpEvent<any>> {
    const apiReq = req.clone({
      url: `${process.env['NG_APP_BACKEND_BASE_URL']}/${req.url}`,
    });
    return next.handle(apiReq).pipe(
      catchError((error) => {
        if (error.status === 401) {
          return this.reAuthenticate().pipe(
            tap((token) => {
              this.tokenService.writeToken(token, TokenType.ACCESS);
            }),
            switchMap(() =>
              next.handle(
                apiReq.clone({
                  headers: new HttpHeaders({
                    Authorization: `Bearer ${
                      this.tokenService.getToken(TokenType.ACCESS) ?? ""
                    }`,
                  }),
                })
              )
            )
          );
        }
        return throwError(error);
      })
    );
  }

  reAuthenticate(): Observable<any> {
    const token: Token = {
      accessToken: this.tokenService.getToken(TokenType.ACCESS) ?? "",
      refreshToken: this.tokenService.getToken(TokenType.REFRESH) ?? "",
    };
    return this.httpService.postWithoutToken<Token>('tokens/refresh', token);
  }
}
