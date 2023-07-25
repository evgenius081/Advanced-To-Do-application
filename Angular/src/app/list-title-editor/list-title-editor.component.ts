import { Component, EventEmitter, Input, Output } from "@angular/core";
import { TodoList } from "../classes/todo-list";
import { ListService } from "../services/list.service";
import { Router } from "@angular/router";

@Component({
  selector: 'app-list-title-editor',
  templateUrl: './list-title-editor.component.html',
  styleUrls: ['./list-title-editor.component.scss']
})
export class ListTitleEditorComponent {
  @Input() title: string = ""
  @Input() isArchived?: boolean;
  @Input() type: 0 | 1 = 0;
  @Input() list?: TodoList;
  @Input() editMode?: boolean;
  disabled = true
  validationMessages: string[] = []
  validationExpressions: [RegExp, string][] = [[new RegExp('.+'), "Title must be at least one symbol long"],
    [new RegExp('^[a-zA-Z0-9-_!?,.() ]*$'), "Title must not contain special characters"]]
  @Output() editModeEvent = new EventEmitter<boolean>();
  @Output() editTitleEvent = new EventEmitter<string>();

  constructor(private listService: ListService,
              private router: Router) {
  }

  ngOnInit(){
    this.validationMessages = []
    this.validationExpressions.map(exp => {
      if (!exp[0].test(this.title)){
        this.validationMessages.push(exp[1])
      }
    })
    this.disabled = this.validationMessages.length != 0
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
    this.validationMessages = []
    this.validationExpressions.map(exp => {
      if (!exp[0].test(this.title)){
        this.validationMessages.push(exp[1])
      }
    })
    if (this.validationMessages.length == 0 && this.type == 0){
      let createdList: TodoList;
      this.listService.createList(
        {title: this.title, isArchived: this.isArchived!, userID: 1}
      ).subscribe(l => createdList = l)
      this.router.navigate([`lists/${createdList!.id}`])
    }
    else if (this.validationMessages.length == 0 && this.type == 1){
      this.listService.updateList(
        {id: this.list!.id, title: this.title, isArchived: this.isArchived!, userID: this.list!.userID}
      ).subscribe()
      this.editModeEvent.emit(false);
      this.editTitleEvent.emit(this.title)
    }
  }
}
