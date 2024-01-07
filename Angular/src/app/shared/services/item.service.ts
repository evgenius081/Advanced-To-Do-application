import { Injectable } from '@angular/core';
import { TodoItem } from '../classes/item/todo-item';
import { Observable, of, catchError } from 'rxjs';
import { TodoItemCreate } from '../classes/item/todo-item-create';
import { TokenService } from './token.service';
import { Router } from '@angular/router';
import { HttpService } from './http.service';
import { UserService } from './user.service';
import { TokenType } from '../enums/token-type';

@Injectable({
  providedIn: 'root',
})
export class ItemService {
  constructor(
    private tokenService: TokenService,
    private httpService: HttpService,
    private userService: UserService,
  ) {}

  getItemsByListID(listID: number): Observable<TodoItem[]> {
    const token: string = this.tokenService.getToken(TokenType.ACCESS) ?? '';
    return this.httpService
      .get<TodoItem[]>(`items/list/${listID}`, token)
      .pipe(catchError(this.userService.handleError<TodoItem[]>()));
  }

  updateItem(itemToUpdate: TodoItem): Observable<TodoItem> {
    const token: string = this.tokenService.getToken(TokenType.ACCESS) ?? '';
    return this.httpService
      .put<TodoItem>("items", token, itemToUpdate)
      .pipe(catchError(this.userService.handleError<TodoItem>()));
  }

  createItem(item: TodoItemCreate): Observable<object> {
    console.log(item);
    const token: string = this.tokenService.getToken(TokenType.ACCESS) ?? '';
    return this.httpService
      .post<TodoItemCreate>("items", token, item)
      .pipe(catchError(this.userService.handleError<object>()));
  }

  deleteItem(id: number): Observable<object> {
    const token: string = this.tokenService.getToken(TokenType.ACCESS) ?? '';
    return this.httpService
      .delete(`items/${id}`, token)
      .pipe(catchError(this.userService.handleError<object>()));
  }

  getTodayItems(): Observable<TodoItem[]> {
    const token: string = this.tokenService.getToken(TokenType.ACCESS) ?? '';
    return this.httpService
      .get<TodoItem[]>("items/today", token)
      .pipe(catchError(this.userService.handleError<TodoItem[]>()));
  }

  getPrimaryItems(): Observable<TodoItem[]> {
    const token: string = this.tokenService.getToken(TokenType.ACCESS) ?? '';
    return this.httpService
      .get<TodoItem[]>("items/primary", token)
      .pipe(catchError(this.userService.handleError<TodoItem[]>()));
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
}
