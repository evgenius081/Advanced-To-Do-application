import { Injectable, Signal, WritableSignal, signal } from '@angular/core';
import { TodoList } from '../classes/list/todo-list';
import { Observable, of, catchError, tap } from 'rxjs';
import { TodoListWithStatistics } from '../classes/list/todo-list-with-statistics';
import { TodoListCreate } from '../classes/list/todo-list-create';
import { TokenService } from './token.service';
import { Router } from '@angular/router';
import { HttpService } from './http.service';
import { UserService } from './user.service';
import { TokenType } from '../enums/token-type';
import { ItemStatus } from '../enums/item-status';

@Injectable({
  providedIn: 'root',
})
export class ListService {
  constructor(
    private tokenService: TokenService,
    private httpService: HttpService,
    private userService: UserService
  ) {}

  dummyTodoListWithStatistics: TodoListWithStatistics = {
    id: -1,
    isArchived: false,
    title: '',
    userID: -1,
    itemsCompleted: 0,
    itemsInProcess: 0,
    itemsNotStarted: 0,
  };

  dummyTodoList: TodoList = {
    id: -1,
    isArchived: false,
    title: '',
    userID: -1,
  };

  listChangedSignal$: WritableSignal<TodoListWithStatistics[]> = signal<TodoListWithStatistics[]>([]);

  getList(id: number): Observable<TodoList> {
    const token: string = this.tokenService.getToken(TokenType.ACCESS) ?? '';
    return this.httpService
      .get<TodoList>(`lists/${id}`, token)
      .pipe(catchError(this.userService.handleError<TodoList>()));
  }

  getLists(): Observable<TodoListWithStatistics[]> {
    const token: string = this.tokenService.getToken(TokenType.ACCESS) ?? '';
    return this.httpService
      .get<TodoListWithStatistics[]>('lists/details', token)
      .pipe(tap((result) => {
        this.notifyAboutListSet(result as TodoListWithStatistics[]);
        return result;
      }),
      catchError(this.userService.handleError<any>()));
  }

  createList(createdList: TodoListCreate): Observable<object> {
    const token: string = this.tokenService.getToken(TokenType.ACCESS) ?? '';
    return this.httpService
      .post<TodoListCreate>('lists', token, createdList)
      .pipe(
        tap((result) => {
          this.notifyAboutListAdd(result as TodoList);
          return result;
        }),
        catchError(this.userService.handleError<object>())
      );
  }

  updateList(listToUpdate: TodoList): Observable<TodoList> {
    const token: string = this.tokenService.getToken(TokenType.ACCESS) ?? '';
    console.log(listToUpdate)
    return this.httpService
      .put<TodoList>('lists', token, listToUpdate)
      .pipe(tap(
        (result) => {
          console.log(result);
          this.notifyAboutListUpdate(result);
          return result;
        }
      ), catchError(this.userService.handleError<TodoList>()));
  }

  deleteList(id: number): Observable<object> {
    const token: string = this.tokenService.getToken(TokenType.ACCESS) ?? '';
    return this.httpService
      .delete(`lists/${id}`, token)
      .pipe(tap((result) => {
        this.notifyAboutListRemove(id);
        return result;
      }),
      catchError(this.userService.handleError<object>()));
  }

  copyList(id: number) {
    const token: string = this.tokenService.getToken(TokenType.ACCESS) ?? '';
    return this.httpService
      .get(`lists/copy/${id}`, token)
      .pipe(tap((result) => {
        this.notifyAboutListCopy(result as TodoListWithStatistics);
        return result;
      }),
      catchError(this.userService.handleError<void>()));
  }

  updateStats(listId: number, status: ItemStatus, value: number): void {
    this.listChangedSignal$.update((prevState) => {
      const foundList: TodoListWithStatistics | undefined = prevState.find(list => list.id == listId);
      if (foundList){
        switch (status){
          case (ItemStatus.COMPLETED): {
            foundList.itemsCompleted += value;
            break;
          }
          case (ItemStatus.NOT_STARTED): {
            foundList.itemsNotStarted += value;
            break;
          }
          case (ItemStatus.IN_PROCESS): {
            foundList.itemsInProcess += value;
            break;
          }
        }
        prevState[prevState.findIndex((list) => list.id == foundList.id)] = foundList;
      }
      return prevState;
    });
  }

  private notifyAboutListUpdate(list: TodoList): void {
    const foundList: TodoListWithStatistics | undefined = this.listChangedSignal$().find(l => l.id == list.id);

    if (foundList){
      const updatedList: TodoListWithStatistics = {
        ...list,
        itemsCompleted: foundList.itemsCompleted,
        itemsInProcess: foundList.itemsInProcess,
        itemsNotStarted: foundList.itemsNotStarted
      }
      this.listChangedSignal$.update(prevState => {
        prevState[prevState.indexOf(foundList)] = updatedList;
        return prevState;
      })
    }
  }

  private notifyAboutListSet(lists: TodoListWithStatistics[]): void {
    this.listChangedSignal$.set(lists);
  }

  private notifyAboutListAdd(list: TodoList): void {
    const createdListWithStats: TodoListWithStatistics = {
      id: list.id,
      isArchived: list.isArchived,
      title: list.title,
      userID: list.userID,
      itemsCompleted: 0,
      itemsInProcess: 0,
      itemsNotStarted: 0,
    };
    this.listChangedSignal$.update((lists) => {
      lists.push(createdListWithStats);
      return lists;
    });
  }

  private notifyAboutListCopy(list: TodoListWithStatistics): void {
    this.listChangedSignal$.update((lists) => {
      lists.push(list);
      return lists;
    });
  }

  private notifyAboutListRemove(id: number): void {
    this.listChangedSignal$.update((lists) => lists.filter((list) => list.id != id));
  }
}
