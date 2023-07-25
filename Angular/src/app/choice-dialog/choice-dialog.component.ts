import { Component, Inject, Input } from "@angular/core";
import { MAT_DIALOG_DATA, MatDialogRef } from "@angular/material/dialog";
import { DialogData } from "../classes/dialog-data";
import { ListService } from "../services/list.service";
import { Router } from "@angular/router";
import { ItemService } from "../services/item.service";

@Component({
  selector: 'app-choice-dialog',
  templateUrl: './choice-dialog.component.html',
  styleUrls: ['./choice-dialog.component.scss']
})
export class ChoiceDialogComponent {
  constructor(public dialogRef: MatDialogRef<ChoiceDialogComponent>,
              @Inject(MAT_DIALOG_DATA) public data: DialogData,
              private listService: ListService,
              private itemService: ItemService,
              private router: Router) {}

  handleDelete(){
    if (this.data.type == 0){
      this.listService.deleteList(this.data.id).subscribe()
      this.router.navigate(["/"])
    }
    else{
      this.itemService.deleteItem(this.data.id).subscribe()
    }
  }
}
