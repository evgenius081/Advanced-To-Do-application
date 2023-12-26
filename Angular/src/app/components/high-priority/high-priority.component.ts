import { Component } from '@angular/core';
import {TodoItem} from "../classes/todo-item";
import {ListService} from "../services/list.service";
import {ItemService} from "../services/item.service";
import {faEye, faEyeSlash} from "@fortawesome/free-solid-svg-icons";

@Component({
  selector: 'app-high-priority',
  templateUrl: './high-priority.component.html',
  styleUrls: ['./high-priority.component.scss']
})
export class HighPriorityComponent {
  items: TodoItem[] = [];
  itemsToShow: TodoItem[] = [];
  username: string = "admin";
  showCompleted = false
  faEye = faEye
  faEyeSlash = faEyeSlash

  constructor(private listService: ListService,
              private itemService: ItemService) {
  }

  ngOnInit(): void {
    this.getItems();
  }

  handleShowCompleted(){
    this.showCompleted = !this.showCompleted;
    this.updateItemsToShow();
  }

  updateItemsToShow(){
    if (!this.showCompleted){
      this.itemsToShow = this.items.filter(item => item.status != 2);
    }
    else{
      this.itemsToShow = this.items;
    }
  }

  deleteItem(id: number){
    this.items.splice(this.items.indexOf(this.items.find(item => item.id == id)!), 1);
    this.updateItemsToShow();
  }

  compareItems(a: TodoItem, b: TodoItem){
    if (a.priority < b.priority){
      return 1;
    }else if (a.priority > b.priority){
      return -1;
    }
    else{
      if (new Date(a.deadline) > new Date(b.deadline)){
        return 1;
      }
      else if(new Date(a.deadline) < new Date(b.deadline)){
        return -1;
      }
      else{
        if (a.status < b.status){
          return 1;
        }
        else if(a.status > b.status){
          return -1;
        }
        return 0;
      }
    }
  }

  getItems(){
    let items: TodoItem[];
    this.itemService.getPrimaryItems()
      .subscribe(i => items = i);
    this.items = items!.sort((a, b) => this.compareItems(a, b));
    if (!this.showCompleted){
      this.itemsToShow = this.items.filter(item => item.status != 2);
    }
  }

  changeItem(value: TodoItem){
    let item = this.items.find(i => i.id == value.id);
    if (item != undefined){
      if (value.priority != 2){
        this.items.splice(this.items.indexOf(item), 1);
        this.updateItemsToShow();
        return;
      }
      this.items[this.items.indexOf(item)] = value;
    }
    this.updateItemsToShow();
  }
}
