import { Component, Input } from "@angular/core";
import { ListService } from "../services/list.service";
import { TodoList } from "../classes/todo-list";
import { Router } from "@angular/router";

@Component({
  selector: 'app-list-create',
  templateUrl: './list-create.component.html',
  styleUrls: ['./list-create.component.scss']
})
export class ListCreateComponent {
  @Input() title: string = ""
  @Input() isArchived: boolean = false;
  disabled = true
  validationMessages: string[] = []
  validationExpressions: [RegExp, string][] = [[new RegExp('.+'), "Title must be at least one symbol long"],
  [new RegExp('^[a-zA-Z0-9-_!?,.()]*$'), "Title must not contain special characters"]]

  constructor(private listService: ListService,
              private router: Router) {
  }

  onChange(e: Event){
    this.validationMessages = []
    this.validationExpressions.map(exp => {
      if (!exp[0].test(this.title)){
        this.validationMessages.push(exp[1])
      }
    })
    this.disabled = this.validationMessages.length != 0
  }

  handleSubmit(e: Event){
    e.preventDefault()
    let createdList: TodoList;
    this.listService.createList(
      {title: this.title, isArchived: this.isArchived, userID: 1}
    ).subscribe(l => createdList = l)
    this.router.navigate([`lists/${createdList!.id}`])
  }
}



