import { Component, Input } from '@angular/core';
import { TodoListWithStatistics } from '../../../shared/classes/list/todo-list-with-statistics';

@Component({
  selector: 'app-side-menu-list-element',
  templateUrl: './side-menu-list-element.component.html',
  styleUrls: ['./side-menu-list-element.component.scss'],
})
export class SideMenuListElementComponent {
  @Input() list?: TodoListWithStatistics;
  Math = Math;
}
