import { Component, EventEmitter, Input, Output } from '@angular/core';
import { TodoItem } from '../../../shared/classes/item/todo-item';
import {
  faPenToSquare,
  faGripVertical,
  faStar,
  faTrash,
  faBell,
} from '@fortawesome/free-solid-svg-icons';
import { ItemService } from '../../../shared/services/item.service';
import { ChoiceDialogComponent } from '../../../shared/choice-dialog/choice-dialog.component';
import { MatDialog } from '@angular/material/dialog';
import { ItemPriority } from 'src/app/shared/enums/item-priority';
import { ListService } from 'src/app/shared/services/list.service';

@Component({
  selector: 'app-item-view',
  templateUrl: './item-view.component.html',
  styleUrls: ['./item-view.component.scss'],
})
export class ItemViewComponent {
  @Input() item?: TodoItem;
  editMode = false;
  faGripVertical = faGripVertical;
  faStar = faStar;
  @Output() itemChangedPriority = new EventEmitter<ItemPriority>();
  @Output() itemDeletedEvent = new EventEmitter<number>();
  @Output() itemChanged = new EventEmitter<TodoItem>();
  @Input() canDrag = false;
  deadline: Date = new Date();
  today: Date = new Date();
  readonly MILLIS_PER_SECOND = 1000;
  readonly MILLIS_PER_MINUTE = this.MILLIS_PER_SECOND * 60;
  readonly MILLIS_PER_HOUR = this.MILLIS_PER_MINUTE * 60;
  readonly MILLIS_PER_DAY = this.MILLIS_PER_HOUR * 24;
  Math = Math;
  icon = false;
  faBell = faBell;
  faPenToSquare = faPenToSquare;
  faTrash = faTrash;

  constructor(private itemService: ItemService, public dialog: MatDialog, private listService: ListService) {}

  ngOnInit() {
    if (this.item) {
      this.deadline = new Date(this.item.deadline);
    }
  }

  click() {
    this.icon = !this.icon;
  }

  handleChangePriority(event: Event) {
    event.stopPropagation();
    if (this.item) {
      this.item.priority =
        this.item?.priority === ItemPriority.STANDARD
          ? ItemPriority.HIGH
          : ItemPriority.STANDARD;
      this.itemService.updateItem(this.item).subscribe(() => {
        this.itemChangedPriority.emit(this.item!.priority);
        this.itemChanged.emit(this.item!);
      });
    }
  }

  getDeadline(): string {
    const deadlineInDays: number = Math.round(
      (this.deadline?.getTime() - this.today.getTime()) / this.MILLIS_PER_DAY
    );
    const deadlineTime: string = `${
      this.deadline.getHours() < 10
        ? '0' + this.deadline.getHours()
        : this.deadline.getHours()
    }:${
      this.deadline.getMinutes() < 10
        ? '0' + this.deadline.getMinutes()
        : this.deadline.getMinutes()
    }`;
    return `Deadline: ${
      deadlineInDays > 1 ? ` in ${deadlineInDays} days` : ' today'
    } at ${deadlineTime}`;
  }

  openDialog(): void {
    let ref = this.dialog.open(ChoiceDialogComponent, {
      width: '250px',
      enterAnimationDuration: '0ms',
      exitAnimationDuration: '0ms',
      data: {
        title: 'Delete task?',
        message: `Are you sure you want to
      delete task <b>${this.item?.title}</b>?`,
        id: this.item?.id,
        type: 1,
      },
    });

    ref.afterClosed().subscribe((res) => {
      if (res && this.item) {
        const itemId: number = this.item.id;
        this.itemService.deleteItem(itemId).subscribe(() => {
          if (this.item){
            this.listService.updateStats(this.item.toDoListID, this.item.status, -1);
          }
          this.itemDeletedEvent.emit(itemId);
        });
      }
    });
  }

  handleOpen(event: Event) {
    event.stopPropagation();
  }

  handleEdit() {
    this.editMode = true;
  }

  changeItem(value: TodoItem) {
    this.item = value;
    this.itemChanged.emit(value);
  }

  changeEditMode(value: boolean) {
    this.editMode = value;
  }
}
