import { Component } from '@angular/core';
import { TodoItem } from '../../shared/classes/todo-item';
import { ItemService } from '../../shared/services/item.service';
import { faEye, faEyeSlash } from '@fortawesome/free-solid-svg-icons';
import { UserService } from '../../shared/services/user.service';

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

  constructor(
    private itemService: ItemService,
    public userService: UserService
  ) {}

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

  compareItems(a: TodoItem, b: TodoItem) {
    if (a.priority < b.priority) {
      return 1;
    } else if (a.priority > b.priority) {
      return -1;
    } else {
      if (new Date(a.deadline) > new Date(b.deadline)) {
        return 1;
      } else if (new Date(a.deadline) < new Date(b.deadline)) {
        return -1;
      } else {
        if (a.status < b.status) {
          return 1;
        } else if (a.status > b.status) {
          return -1;
        }
        return 0;
      }
    }
  }

  getItems() {
    let items: TodoItem[];
    this.itemService.getTodayItems().subscribe((i) => (items = i));
    this.items = items!.sort((a, b) => this.compareItems(a, b));
    if (!this.showCompleted) {
      this.itemsToShow = this.items.filter((item) => item.status != 2);
    }
  }

  changeItem(value: TodoItem) {
    let item = this.items.find((i) => i.id == value.id);
    if (item != undefined) {
      this.items[this.items.indexOf(item)] = value;
    }
    this.updateItemsToShow();
  }
}
