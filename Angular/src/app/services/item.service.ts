import { Injectable } from '@angular/core';
import {TodoItem} from "../classes/todo-item";
import { Observable, of } from "rxjs";
import { ListService } from "./list.service";
import { TodoItemCreate } from "../classes/todo-item-create";

@Injectable({
  providedIn: 'root'
})
export class ItemService {
items: TodoItem[] = [
  { id: 1,
    title: "Buy groceries",
    description: "Milk, bread, eggs, cheese",
    createdAt: new Date(new Date().getTime() - (1000 * 60 * 60 * 24)).toISOString(),
    deadline: new Date(new Date().getTime() + (1000 * 60 * 60)*2).toISOString(),
    toDoListID: 1,
    status: 0,
    remind: true,
    priority: 2 },
  { id: 2,
    title: "Clean the house",
    description: "Vacuum the floors and dust the surfaces",
    createdAt: new Date(new Date().getTime() - (1000 * 60 * 60 * 24) * 4).toISOString(),
    deadline: new Date(new Date().getTime() + (1000 * 60)*40).toISOString(),
    toDoListID: 1,
    status: 0,
    remind: false,
    priority: 2},
  { id: 3,
    title: "Finish coding challenge",
    description: "Complete the coding challenge for the job application",
    createdAt: new Date(new Date().getTime() - (1000 * 60 * 60 * 24)*7).toISOString(),
    deadline: new Date(new Date().getTime() + (1000 * 60 * 60 * 24)*3).toISOString(),
    toDoListID: 2,
    status: 1,
    remind: false,
    priority: 1 },
  { id: 4,
    title: "Finish project report",
    description: "Write up the findings from the experiments",
    createdAt: new Date(new Date().getTime() - (1000 * 60 * 60 * 24)).toISOString(),
    deadline: new Date(new Date().getTime() + (1000 * 60)*2).toISOString(),
    toDoListID: 2,
    status: 1,
    remind: false,
    priority: 1 },
  { id: 5,
    title: "Submit expense report",
    description: "Submit the expense report for reimbursement",
    createdAt: new Date(new Date().getTime() - (1000 * 60 * 60 * 24)*3).toISOString(),
    deadline: new Date(new Date().getTime() - (1000 * 60)*6).toISOString(),
    toDoListID: 2,
    status: 1,
    remind: false,
    priority: 2 },
  { id: 6,
    title: "Reply to email",
    description: "Respond to the email from your coworker",
    createdAt: new Date(new Date().getTime() - (1000 * 60 * 60 * 24)).toISOString(),
    deadline: new Date(new Date().getTime() + (1000 * 60 * 60  * 24)*2).toISOString(),
    toDoListID: 2,
    status: 0,
    remind: false,
    priority: 1 },
  { id: 7,
    title: "Prepare for meeting",
    description: "Review the agenda and prepare notes for the meeting",
    createdAt: new Date(new Date().getTime() - (1000 * 60 * 60 * 24)).toISOString(),
    deadline: new Date(new Date().getTime() + (1000 * 60 * 60)*2).toISOString(),
    toDoListID: 2,
    status: 1,
    remind: true,
    priority: 1 },
  { id: 8,
    title: "Go for a run",
    description: "Run for 30 minutes around the park",
    createdAt: new Date(new Date().getTime() - (1000 * 60 * 60 * 24)*7).toISOString(),
    deadline: new Date(new Date().getTime() + (1000 * 60 * 60)*2).toISOString(),
    toDoListID: 3,
    status: 2,
    remind: false,
    priority: 0 },
  { id: 9,
    title: "Call mom", description: "Check in with her and see how she's doing",
    createdAt: new Date(new Date().getTime() - (1000 * 60 * 60 * 24)*2).toISOString(),
    deadline: new Date(new Date().getTime() + (1000 * 60 * 60)*2).toISOString(),
    toDoListID: 3,
    status: 0,
    remind: false,
    priority: 1 },
  { id: 10,
    title: "Send birthday card",
    description: "Mail the birthday card to your friend",
    createdAt: new Date(new Date().getTime() - (1000 * 60 * 60 * 24)*5).toISOString(),
    deadline: new Date(new Date().getTime() + (1000 * 60 * 60)*2).toISOString(),
    toDoListID: 3,
    status: 2,
    remind: true,
    priority: 1 },
  { id: 11,
    title: "Schedule dentist appointment",
    description: "Make an appointment for a teeth cleaning",
    createdAt: new Date(new Date().getTime() - (1000 * 60 * 60 * 24)).toISOString(),
    deadline: new Date(new Date().getTime() + (1000 * 60 * 60)*2).toISOString(),
    toDoListID: 3,
    status: 0,
    remind: false,
    priority: 0 },
  { id: 12,
    title: "Read book",
    description: "Read the next chapter in the book club book",
    createdAt: new Date(new Date().getTime() - (1000 * 60 * 60 * 24)*6).toISOString(),
    deadline: new Date(new Date().getTime() + (1000 * 60 * 60)*2).toISOString(),
    toDoListID: 3,
    status: 0,
    remind: false,
    priority: 1 },
  { id: 13,
    title: "Buy birthday gift",
    description: "Choose and purchase a gift for the upcoming birthday",
    createdAt: new Date(new Date().getTime() - (1000 * 60 * 60 * 24)*5).toISOString(),
    deadline: new Date(new Date().getTime() + (1000 * 60 * 60)*2).toISOString(),
    toDoListID: 3,
    status: 1,
    remind: false,
    priority: 1 },
]
  constructor() { }

  getItemsByListID(listID: number){
    return of(this.items.filter(item => item.toDoListID == listID))
  }

  getItem(id: number){
    return of(this.items.find(item => item.id == id))
  }

  updateItem(itemToUpdate: TodoItem){
    let item = this.items.find(item => item.id == itemToUpdate.id)
    if (item != undefined){
      let id = this.items.indexOf(item);
      this.items[id] = itemToUpdate;
    }
    return of(itemToUpdate)
  }

  createItem(item: TodoItemCreate){
    let id = this.items.length == 0 ? 1 : this.items.slice(-1)[0].id + 1;
    let newItem: TodoItem = {
      id: id,
      title: item.title,
      description: item.description,
      priority: item.priority,
      remind: item.remind,
      status:item.status,
      createdAt: item.createdAt,
      deadline: item.deadline,
      toDoListID: item.toDoListID}
    this.items.push(newItem)
    return of(newItem)
  }

  deleteItem(id: number){
    this.items.splice(this.items.indexOf(this.items.find(item => item.id == id)!), 1)
    return new Observable<void>();
  }

  getTodayItems(){
    return of(this.items.filter(item =>
      new Date(item.deadline).toLocaleDateString() == new Date().toLocaleDateString()
    ));
  }

  getPrimaryItems(){
    return of(this.items.filter(item => item.priority == 2))
  }
}
