import { Component, forwardRef } from "@angular/core";
import {ListService} from "../services/list.service";
import {TodoList} from "../classes/todo-list";
import { ActivatedRoute, Router } from "@angular/router";
import { Location } from '@angular/common';
import {faPenToSquare, faTrash, faCopy, faBoxArchive, faEye, faEyeSlash} from "@fortawesome/free-solid-svg-icons";
import {ItemService} from "../services/item.service";
import {TodoItem} from "../classes/todo-item";
import {CdkDragDrop, moveItemInArray, transferArrayItem} from "@angular/cdk/drag-drop";
import { MatDialog } from "@angular/material/dialog";
import { ChoiceDialogComponent } from "../choice-dialog/choice-dialog.component";
import { NG_VALUE_ACCESSOR } from "@angular/forms";
import { ListTitleEditorComponent } from "../list-title-editor/list-title-editor.component";

@Component({
  selector: 'app-list-view',
  templateUrl: './list-view.component.html',
  styleUrls: ['./list-view.component.scss']
})

export class ListViewComponent {
  id: number = 0;
  list?: TodoList;
  items: TodoItem[] = [];
  notStartedItems: TodoItem[] = [];
  inProcessItems: TodoItem[] = [];
  completedItems: TodoItem[] = [];
  faPenToSquare = faPenToSquare;
  faTrash = faTrash;
  faCopy = faCopy;
  faBoxArchive = faBoxArchive;
  editMode = false;

  constructor(private listService: ListService,
              private route: ActivatedRoute,
              private router: Router,
              private itemService: ItemService,
              public dialog: MatDialog) {
    route.params.subscribe((params) => {
      this.id = params["id"];
      this.getList(this.id);
      this.getItems();
      this.completedItems = this.getCompletedItems();
      this.inProcessItems = this.getInProcessItems();
      this.notStartedItems = this.getNotStartedItems()
    });
  }

  ngOnInit(): void {
    this.getList(this.id);
    this.getItems();
    this.completedItems = this.getCompletedItems();
    this.inProcessItems = this.getInProcessItems();
    this.notStartedItems = this.getNotStartedItems()
  }

  changeEditMode(value: boolean){
    this.editMode = value;
  }

  changeTitle(value: string){
    this.list!.title = value;
  }

  getList(id: number): void{
    this.listService.getList(id).subscribe(list => this.list = list);
    if (this.list == undefined){
      this.router.navigate(["not-found"]);
    }
  }

  openDialog(): void {
    this.dialog.open(ChoiceDialogComponent, {
      width: '250px',
      enterAnimationDuration: '0ms',
      exitAnimationDuration: '0ms',
      data: {title: "Delete collection?", message: `Are you sure you want to
      delete collection <b>${this.list?.title}</b> and all its tasks?`,
      id: this.list?.id, type: 0}
    });
  }

  getItems(){
    this.itemService.getItemsByListID(this.id)
      .subscribe(items => this.items = items);
  }

  getNotStartedItems(){
    return this.items.filter(item => item.status == 0);
  }

  getInProcessItems(){
    return this.items.filter(item => item.status == 1);
  }

  getCompletedItems(){
    return this.items.filter(item => item.status == 2);
  }

  handleArchive(){
    this.list!.isArchived = !this.list!.isArchived;
    this.listService.updateList(this.list!);
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
      let item = ((event.item.data as unknown) as TodoItem);
      let status = Number(event.container.id.slice(-1));
      if (status == 0 || status == 1 || status == 2){
        item.status = status;
      }
      let newItem: TodoItem | undefined;
      this.itemService.updateItem(item).subscribe(item => newItem = item);
      this.getItems();
      this.completedItems = this.getCompletedItems();
      this.inProcessItems = this.getInProcessItems();
      this.notStartedItems = this.getNotStartedItems();
    }
  }
}
