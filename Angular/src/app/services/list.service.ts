import { Injectable } from '@angular/core';
import {TodoList} from "../classes/todo-list";
import {Observable, of} from "rxjs";
import {TodoListWithStatistics} from "../classes/todo-list-with-statistics";

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


  constructor() { }

  getList(id: number): Observable<TodoList>{
    return of(this.lists.find(l => l.id == id)!)
  }

  getLists(): Observable<TodoListWithStatistics[]>{
    return of(this.listsWithStat)
  }
}
