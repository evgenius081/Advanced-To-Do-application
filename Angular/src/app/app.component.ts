import { Component } from '@angular/core';
import { UserService } from './shared/services/user.service';
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss'],
})
export class AppComponent {
  private subscription: Subscription;

  constructor(private userService: UserService) {
    this.isLoggedIn = this.userService.isLoggedIn;
    this.subscription = this.userService.isLoggedIn$.subscribe((i) => this.isLoggedIn = i);
  }
  title = 'ToDo Application';
  isLoggedIn: boolean = false;

  ngOnDestroy() {
     this.subscription.unsubscribe();
   }
}
