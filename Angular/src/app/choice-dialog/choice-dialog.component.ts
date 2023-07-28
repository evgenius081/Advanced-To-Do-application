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
              @Inject(MAT_DIALOG_DATA) public data: DialogData) {}
}
