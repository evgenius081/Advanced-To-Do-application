import { Component, effect } from '@angular/core';
import { UserService } from './shared/services/user.service';
import { faBell } from '@fortawesome/free-solid-svg-icons';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss'],
})
export class AppComponent {
  constructor(private userService: UserService) {
    effect(() => this.isLoggedIn = this.userService.isLoggedInSignal$());
  }

  title = 'ToDo Application';
  isLoggedIn: boolean = this.userService.isLoggedInSignal$();
  faBell = faBell;
  notificationsNumber: number = 5;
  showNotifications: boolean = false;

  notificationsClick(): void {
    this.showNotifications = !this.showNotifications;
  }
}
