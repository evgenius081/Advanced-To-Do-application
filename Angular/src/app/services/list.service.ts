import { Injectable } from '@angular/core';
import {TodoList} from "../classes/todo-list";
import {Observable, of} from "rxjs";
import {TodoListWithStatistics} from "../classes/todo-list-with-statistics";
import { ItemService } from "./item.service";
import { TodoItem } from "../classes/todo-item";
import { TodoListCreate } from "../classes/todo-list-create";

@Injectable({
  providedIn: 'root'
})
export class ListService {
  listsWithStat: TodoListWithStatistics[] = [
    {id: 1, title: "House things aaaaaaaa", isArchived: false, userID: 1, itemsNotStarted: 3, itemsInProcess: 2, itemsCompleted: 1},
    {id: 2, title: "Work things", isArchived: false, userID: 1, itemsNotStarted: 2, itemsInProcess: 1, itemsCompleted: 3},
    {id: 3, title: "Being adult", isArchived: true, userID: 1, itemsNotStarted: 3, itemsInProcess: 1, itemsCompleted: 2}
  ]

  lists: TodoList[] = [
    {id: 1, title: "House things aaaaaaaa", isArchived: false, userID: 1},
    {id: 2, title: "Work things", isArchived: false, userID: 1},
    {id: 3, title: "Being adult", isArchived: true, userID: 1}
  ]


  constructor(private itemService: ItemService) { }

  getList(id: number): Observable<TodoList>{
    return of(this.lists.find(l => l.id == id)!)
  }

  getLists(): Observable<TodoListWithStatistics[]>{
    return of(this.listsWithStat);
  }

  createList(createdList: TodoListCreate){
    let id = this.lists.length == 0 ? 1 : this.lists.slice(-1)[0].id + 1;
    let newList = {
      id: id,
      title: createdList.title,
      isArchived: createdList.isArchived,
      userID: createdList.userID
    };
    this.lists.push(newList)
    this.listsWithStat.push({
      id: id,
      title: createdList.title,
      isArchived: createdList.isArchived,
      userID: createdList.userID,
      itemsCompleted: 0,
      itemsNotStarted: 0,
      itemsInProcess: 0
    })

    return of(newList);
  }

  updateList(listToUpdate: TodoList){
    let list = this.lists.find(list => list.id == listToUpdate.id)
    if (list != undefined){
      this.lists[this.lists.indexOf(list)] = listToUpdate
    }
    let listStat = this.listsWithStat.find(list => list.id == listToUpdate.id);
    this.listsWithStat[this.listsWithStat.indexOf(listStat!)] =
      {id: listToUpdate.id, title: listToUpdate.title, isArchived: listToUpdate.isArchived,
      userID: listToUpdate.userID, itemsCompleted: listStat!.itemsCompleted, itemsInProcess: listStat!.itemsInProcess,
      itemsNotStarted: listStat!.itemsNotStarted}
  }

  deleteList(id: number){
    this.lists.splice(this.lists.indexOf(this.lists.find(l => l.id == id)!) , 1);
    this.listsWithStat.splice(this.listsWithStat.indexOf(this.listsWithStat.find(l => l.id == id)!) , 1);

    let items: TodoItem[] = [];
    this.itemService.getItemsByListID(id).subscribe(is => items = is);
    items.map(item => this.itemService.deleteItem(item.id))


    return new Observable<void>();
  }
}
