import { Component } from '@angular/core';
import {ItemService} from "./services/item.service";
import {UserService} from "./services/user.service";

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent {
  constructor(public userService: UserService) {
  }
  title = 'ToDo Application';
}
