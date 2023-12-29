import { Component, effect } from '@angular/core';
import { TodoItem } from '../../shared/classes/item/todo-item';
import { ItemService } from '../../shared/services/item.service';
import { faEye, faEyeSlash } from '@fortawesome/free-solid-svg-icons';
import { UserService } from 'src/app/shared/services/user.service';
import { ItemPriority } from 'src/app/shared/enums/item-priority';

@Component({
  selector: 'app-high-priority',
  templateUrl: './high-priority.component.html',
  styleUrls: ['./high-priority.component.scss'],
})
export class HighPriorityComponent {
  items: TodoItem[] = [];
  itemsToShow: TodoItem[] = [];
  username: string | undefined;
  showCompleted = false;
  faEye = faEye;
  faEyeSlash = faEyeSlash;

  constructor(private itemService: ItemService, private userService: UserService) {
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
    this.itemService.getPrimaryItems().subscribe((i) => {
      this.items = i!.sort((a, b) => this.itemService.compareItems(a, b));
      if (!this.showCompleted) {
        this.itemsToShow = this.items.filter((item) => item.status != 2);
      }
    });
  }

  changeItem(value: TodoItem) {
    let item = this.items.find((i) => i.id == value.id);
    if (item != undefined) {
      if (value.priority != ItemPriority.HIGH) {
        this.items.splice(this.items.indexOf(item), 1);
        this.updateItemsToShow();
        return;
      }
      this.items[this.items.indexOf(item)] = value;
    }
    this.updateItemsToShow();
  }
}
