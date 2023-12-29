import { Component, Input } from '@angular/core';
import { UserService } from '../../../shared/services/user.service';
import { Router } from '@angular/router';
import { UserLogin } from '../../../shared/classes/user/user-login';
import { TokenService } from '../../../shared/services/token.service';
import { Token } from '../../../shared/classes/token';
import { TokenType } from '../../../shared/enums/token-type';
import { of } from 'rxjs';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss'],
})
export class LoginComponent {
  @Input() login?: string = 'admin';
  @Input() password?: string = 'P@55w0rd';

  constructor(private tokenService: TokenService, private userService: UserService, private router: Router) {}

  handleSubmit(event: Event) {
    event.preventDefault();
    if (this.login && this.password) {
      const userLogin: UserLogin = {
        login: this.login,
        password: this.password,
      };
      this.userService.login(userLogin).subscribe((t) => {
        const token: Token = t as Token;
        console.log(token)
        this.tokenService.writeToken(token.accessToken, TokenType.ACCESS);
        this.tokenService.writeToken(token.refreshToken, TokenType.REFRESH);
        this.userService.isLoggedInSignal$.set(true);
        this.userService.usernameSignal$.set(this.tokenService.getUserNameFromToken());
        this.router.navigate(['']);
      });
    }
  }
}
