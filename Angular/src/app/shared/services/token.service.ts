import { Injectable } from '@angular/core';
import { jwtDecode as decode } from 'jwt-decode';
import { TokenType } from '../enums/token-type';
import * as moment from "moment";

@Injectable({
  providedIn: 'root',
})
export class TokenService {
  private access_token_id: string = 'TodoToken_access';
  private refresh_token_id: string = "TodoToken_refresh";

  constructor() {}

  writeToken(token: string, tokenType: TokenType): void {
    localStorage.setItem(tokenType === TokenType.ACCESS ? this.access_token_id : this.refresh_token_id, token);
  }

  removeToken(tokenType: TokenType): void {
    localStorage.removeItem(tokenType === TokenType.ACCESS ? this.access_token_id : this.refresh_token_id);
  }

  getToken(tokenType: TokenType): string | null {
    return localStorage.getItem(tokenType === TokenType.ACCESS ? this.access_token_id : this.refresh_token_id);
  }

  getUserNameFromToken(): string | undefined {
    const token: string | null = this.getToken(TokenType.ACCESS);
    return token ? (decode(token) as any)[
        'http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name'
      ] as string : undefined;
  }

  getUserIdFromToken(): number | undefined {
    const token: string | null = this.getToken(TokenType.ACCESS);
    return token ? (decode(token) as any)[
        'http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier'
      ] as number : undefined;
  }

  getExpirationDateFromToken(): moment.Moment | undefined {
    const token: string | null = this.getToken(TokenType.ACCESS);
    return token ? moment(decode(token).exp) : undefined;
  }
}
