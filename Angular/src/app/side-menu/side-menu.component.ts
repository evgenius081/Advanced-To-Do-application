import { Component } from '@angular/core';
import {TodoList} from "../classes/todo-list";
import { faStar, faClock, faBoxArchive, faPlus } from "@fortawesome/free-solid-svg-icons"
import {TodoListWithStatistics} from "../classes/todo-list-with-statistics";
import {ListService} from "../services/list.service";

@Component({
  selector: 'app-side-menu',
  templateUrl: './side-menu.component.html',
  styleUrls: ['./side-menu.component.scss']
})
export class SideMenuComponent {
  faStar = faStar
  faClock = faClock
  faBoxArchive = faBoxArchive
  faPlus = faPlus
  icon = false
  lists: TodoListWithStatistics[] = [];

  constructor (private listService: ListService){

  }

  click(){
    this.icon = !this.icon;
  }

  ngOnInit(){
    this.getLists()
  }

  getLists(){
    this.listService.getLists().subscribe(lists => this.lists = lists)
  }

  findNotArchived(list: TodoListWithStatistics): boolean{
    return !list.isArchived
  }

  findArchived(list: TodoListWithStatistics): boolean{
    return list.isArchived
  }
}
