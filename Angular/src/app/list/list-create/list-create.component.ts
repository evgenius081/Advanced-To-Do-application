import { Component, Input } from "@angular/core";
import { ListService } from "../../services/list.service";
import { TodoList } from "../../classes/todo-list";
import { Router } from "@angular/router";

@Component({
  selector: 'app-list-create',
  templateUrl: './list-create.component.html',
  styleUrls: ['./list-create.component.scss']
})
export class ListCreateComponent {
  @Input() isArchived: boolean = false;
}



