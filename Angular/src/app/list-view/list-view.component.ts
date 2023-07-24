import { Component } from '@angular/core';
import {ListService} from "../services/list.service";
import {TodoList} from "../classes/todo-list";
import { ActivatedRoute } from '@angular/router';
import { Location } from '@angular/common';
import {faPenToSquare, faTrash, faCopy, faBoxArchive, faEye, faEyeSlash} from "@fortawesome/free-solid-svg-icons";
import {ItemService} from "../services/item.service";
import {TodoItem} from "../classes/todo-item";
import {CdkDragDrop, moveItemInArray, transferArrayItem} from "@angular/cdk/drag-drop";

@Component({
  selector: 'app-list-view',
  templateUrl: './list-view.component.html',
  styleUrls: ['./list-view.component.scss']
})
export class ListViewComponent {
  id: number = 0;
  list?: TodoList;
  items: TodoItem[] = []
  notStartedItems: TodoItem[] = []
  inProcessItems: TodoItem[] = []
  completedItems: TodoItem[] = []
  showHidden = false;
  faPenToSquare = faPenToSquare;
  faTrash = faTrash;
  faCopy = faCopy;
  faBoxArchive = faBoxArchive;
  faEye = faEye;
  faEyeSlash = faEyeSlash;

  constructor(private listService: ListService,
              private route: ActivatedRoute,
              private location: Location,
              private itemService: ItemService) {
    route.params.subscribe((params) => {
      this.id = params["id"];
      this.getList(this.id);
      this.getItems()
      this.completedItems = this.getCompletedItems();
      this.inProcessItems = this.getInProcessItems();
      this.notStartedItems = this.getNotStartedItems()
    });
  }

  ngOnInit(): void {
    this.getList(this.id);
    this.getItems()
    this.completedItems = this.getCompletedItems();
    this.inProcessItems = this.getInProcessItems();
    this.notStartedItems = this.getNotStartedItems()
  }

  getList(id: number): void{
    this.listService.getList(id).subscribe(list => this.list = list)
    if (this.list == undefined){
      document.location.href = "not-found"
    }
  }

  getItems(){
    this.itemService.getItemsByListID(this.id)
      .subscribe(items => this.items = items)
  }

  getNotStartedItems(){
    return this.items.filter(item => item.status == 0)
  }

  getNotStartedHiddenItems(){
    return this.items.filter(item => item.status == 0)
      .filter(item => item.priority == 0)
  }

  getNotStartedNotHiddenItems(){
    return this.items.filter(item => item.status == 0)
      .filter(item => item.priority != 0)
  }

  getInProcessItems(){
    return this.items.filter(item => item.status == 1)
  }

  getInProcessNotHiddenItems(){
    return this.items.filter(item => item.status == 1)
      .filter(item => item.priority != 0)
  }

  getInProcessHiddenItems(){
    return this.items.filter(item => item.status == 1)
      .filter(item => item.priority == 0)
  }

  getCompletedItems(){
    return this.items.filter(item => item.status == 2)
  }

  getCompletedNotHiddenItems(){
    return this.items.filter(item => item.status == 2)
      .filter(item => item.priority != 0)
  }

  getCompletedHiddenItems(){
    return this.items.filter(item => item.status == 2)
      .filter(item => item.priority == 0)
  }

  changeShowHidden(){
    this.showHidden = !this.showHidden
  }

  drop(event: CdkDragDrop<TodoItem[]>) {
    if (event.previousContainer === event.container) {
      moveItemInArray(event.container.data, event.previousIndex, event.currentIndex);
    } else {
      transferArrayItem(
        event.previousContainer.data,
        event.container.data,
        event.previousIndex,
        event.currentIndex,
      );
      let item = ((event.item.data as unknown) as TodoItem)
      item.status = Number(event.container.id.slice(-1))
      let newItem: TodoItem | undefined;
      this.itemService.updateItem(item).subscribe(item => newItem = item)
      this.getItems()
      this.completedItems = this.getCompletedItems();
      this.inProcessItems = this.getInProcessHiddenItems();
      this.notStartedItems = this.getNotStartedItems()
    }
  }
}
