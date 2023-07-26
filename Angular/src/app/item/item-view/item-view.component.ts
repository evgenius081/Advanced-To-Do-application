import {Component, Input} from '@angular/core';
import {TodoItem} from "../../classes/todo-item";
import {faGripVertical} from "@fortawesome/free-solid-svg-icons";

@Component({
  selector: 'app-item-view',
  templateUrl: './item-view.component.html',
  styleUrls: ['./item-view.component.scss']
})
export class ItemViewComponent {
  @Input() item?: TodoItem;
  faGripVertical = faGripVertical
}
