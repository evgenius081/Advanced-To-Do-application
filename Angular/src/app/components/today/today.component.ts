import { Component, effect } from '@angular/core';
import { TodoItem } from '../../shared/classes/item/todo-item';
import { ItemService } from '../../shared/services/item.service';
import { faEye, faEyeSlash } from '@fortawesome/free-solid-svg-icons';
import { UserService } from '../../shared/services/user.service';
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-today',
  templateUrl: './today.component.html',
  styleUrls: ['./today.component.scss'],
})
export class TodayComponent {
  items: TodoItem[] = [];
  itemsToShow: TodoItem[] = [];
  showCompleted = false;
  faEye = faEye;
  faEyeSlash = faEyeSlash;
  username: string | undefined = undefined;

  constructor(
    private itemService: ItemService,
    public userService: UserService
  ) {
    this.username = this.userService.usernameSignal$();
    effect(() => this.username = this.userService.usernameSignal$());
  }

  ngOnInit(): void {
    this.getItems();
  }

  handleShowCompleted() {
    this.showCompleted = !this.showCompleted;
    this.updateItemsToShow();
  }

  updateItemsToShow() {
    if (!this.showCompleted) {
      this.itemsToShow = this.items.filter((item) => item.status != 2);
    } else {
      this.itemsToShow = this.items;
    }
  }

  deleteItem(id: number) {
    this.items.splice(
      this.items.indexOf(this.items.find((item) => item.id == id)!),
      1
    );
    this.updateItemsToShow();
  }

  getItems() {
    this.itemService.getTodayItems().subscribe((i) => {
      this.items = i.sort((a, b) => this.itemService.compareItems(a, b));
      if (!this.showCompleted) {
        this.itemsToShow = this.items.filter((item) => item.status != 2);
      }
    });
  }

  changeItem(value: TodoItem) {
    let item = this.items.find((i) => i.id == value.id);
    if (item != undefined) {
      this.items[this.items.indexOf(item)] = value;
    }
    this.updateItemsToShow();
  }
}
