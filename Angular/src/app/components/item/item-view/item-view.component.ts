import { Component, EventEmitter, Input, Output } from '@angular/core';
import { TodoItem } from '../../../shared/classes/todo-item';
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
  @Output() itemChangedPriority = new EventEmitter<0 | 1 | 2>();
  @Output() itemDeletedEvent = new EventEmitter<number>();
  @Output() itemChanged = new EventEmitter<TodoItem>();
  @Input() canDrag = false;
  deadline?: Date;
  today: Date = new Date();
  readonly MILLIS_PER_SECOND = 1000;
  readonly MILLIS_PER_MINUTE = this.MILLIS_PER_SECOND * 60; //     60,000
  readonly MILLIS_PER_HOUR = this.MILLIS_PER_MINUTE * 60; //  3,600,000
  readonly MILLIS_PER_DAY = this.MILLIS_PER_HOUR * 24; // 86,400,000
  Math = Math;
  icon = false;
  faBell = faBell;

  constructor(private itemService: ItemService, public dialog: MatDialog) {}

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
    this.item!.priority = this.item!.priority == 1 ? 2 : 1;
    this.itemService.updateItem(this.item!).subscribe();
    this.itemChangedPriority.emit(this.item!.priority);
    this.itemChanged.emit(this.item!);
  }

  faPenToSquare = faPenToSquare;
  faTrash = faTrash;

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
      if (res) {
        this.itemService.deleteItem(this.item!.id);
        this.itemDeletedEvent.emit(this.item!.id);
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
