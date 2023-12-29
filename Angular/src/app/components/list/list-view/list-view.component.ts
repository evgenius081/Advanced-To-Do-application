import { Component, EventEmitter, Output } from '@angular/core';
import { ListService } from '../../../shared/services/list.service';
import { TodoList } from '../../../shared/classes/list/todo-list';
import { ActivatedRoute, Router } from '@angular/router';
import {
  faPenToSquare,
  faTrash,
  faCopy,
  faBoxArchive,
  faPlus,
} from '@fortawesome/free-solid-svg-icons';
import { ItemService } from '../../../shared/services/item.service';
import { TodoItem } from '../../../shared/classes/item/todo-item';
import {
  CdkDragDrop,
  moveItemInArray,
  transferArrayItem,
} from '@angular/cdk/drag-drop';
import { MatDialog } from '@angular/material/dialog';
import { ChoiceDialogComponent } from '../../../shared/choice-dialog/choice-dialog.component';
import { ItemPriority } from 'src/app/shared/enums/item-priority';
import { ItemStatus } from 'src/app/shared/enums/item-status';

@Component({
  selector: 'app-list-view',
  templateUrl: './list-view.component.html',
  styleUrls: ['./list-view.component.scss'],
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
  faPlus = faPlus;
  editMode = false;
  createItem = false;

  constructor(
    private listService: ListService,
    private route: ActivatedRoute,
    private router: Router,
    private itemService: ItemService,
    public dialog: MatDialog
  ) {
    route.params.subscribe((params) => {
      this.id = params['id'];
      this.getList(this.id);
      this.getItems();
    });
  }

  deleteItem(id: number) {
    this.items.splice(
      this.items.indexOf(this.items.find((item) => item.id == id)!),
      1
    );
    this.updateLists();
  }

  updateLists() {
    this.completedItems = this.getCompletedItems();
    this.inProcessItems = this.getInProcessItems();
    this.notStartedItems = this.getNotStartedItems();
  }

  addItem(value: TodoItem) {
    this.items.push(value);
    this.updateLists();
  }

  changeItemCreate(value: boolean) {
    this.createItem = value;
  }

  changeEditMode(value: boolean) {
    this.editMode = value;
  }

  changeTitle(value: string) {
    this.list!.title = value;
  }

  getList(id: number): void {
    this.listService.getList(id).subscribe((list) => {
      this.list = list;
      if (this.list == undefined) {
        this.router.navigate(['not-found']);
      }
    });
  }

  openDialog(): void {
    let dialogRef = this.dialog.open(ChoiceDialogComponent, {
      width: '250px',
      enterAnimationDuration: '0ms',
      exitAnimationDuration: '0ms',
      data: {
        title: 'Delete collection?',
        message: `Are you sure you want to
      delete collection <b>${this.list?.title}</b> and all its tasks?`,
        id: this.list?.id,
      },
    });

    dialogRef.afterClosed().subscribe((res) => {
      if (res) {
        this.listService.deleteList(this.id).subscribe(() => {
          this.router.navigate(['']);
        });
      }
    });
  }

  getItems() {
    this.itemService.getItemsByListID(this.id).subscribe((i) => {
      this.items = i.sort((a, b) => this.itemService.compareItems(a, b));
      this.updateLists();
    });
  }

  getNotStartedItems() {
    return this.items
      .filter((item) => item.status === ItemStatus.NOT_STARTED)
      .sort((a, b) => this.itemService.compareItems(a, b));
  }

  getInProcessItems() {
    return this.items
      .filter((item) => item.status === ItemStatus.IN_PROCESS)
      .sort((a, b) => this.itemService.compareItems(a, b));
  }

  getCompletedItems() {
    return this.items
      .filter((item) => item.status === ItemStatus.COMPLETED)
      .sort((a, b) => this.itemService.compareItems(a, b));
  }

  handleArchive() {
    if (this.list) {
      this.list.isArchived = !this.list.isArchived;
      this.listService.updateList(this.list).subscribe();
    }
  }

  drop(event: CdkDragDrop<TodoItem[]>) {
    if (event.previousContainer === event.container) {
      moveItemInArray(
        event.container.data,
        event.previousIndex,
        event.currentIndex
      );
    } else {
      transferArrayItem(
        event.previousContainer.data,
        event.container.data,
        event.previousIndex,
        event.currentIndex
      );
      const item = event.item.data as TodoItem;
      const prevStatus: ItemStatus = item.status;
      const newStatus: number = Number(event.container.id.slice(-1));
      if (Object.values(ItemStatus).includes(newStatus)) {
        item.status = newStatus;
      }
      this.itemService.updateItem(item).subscribe((item) => {
        this.listService.updateStats(item.toDoListID, prevStatus, -1);
        this.listService.updateStats(item.toDoListID, newStatus, 1);
        this.updateLists();
      });
    }
  }

  itemChangedPriority(value: ItemPriority) {
    this.updateLists();
  }

  handleCopy() {
    this.listService.copyList(this.id).subscribe();
  }

  handleCreateItem() {
    this.createItem = true;
  }

  changeItem(value: TodoItem) {
    let item = this.items.find((i) => i.id == value.id);
    if (item != undefined) {
      this.items[this.items.indexOf(item)] = value;
      this.updateLists();
    }
  }
}
