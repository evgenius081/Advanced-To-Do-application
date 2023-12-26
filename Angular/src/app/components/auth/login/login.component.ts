import { Component, Input } from '@angular/core';
import { UserService } from '../../../shared/services/user.service';
import { User } from '../../../shared/classes/user';
import { Router } from '@angular/router';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss'],
})
export class LoginComponent {
  @Input() login?: string = 'admin';
  @Input() password?: string = 'P@55w0rd';

  constructor(private userService: UserService, private router: Router) {}

  handleSubmit(event: Event) {
    event.preventDefault();
    if (this.login != undefined && this.password != undefined) {
      let user: User | undefined;
      this.userService
        .login({ login: this.login, password: this.password })
        .subscribe((u) => (user = u));
      if (user) {
        console.log('faesef');
        this.router.navigate(['']);
      }
    }
  }
}
