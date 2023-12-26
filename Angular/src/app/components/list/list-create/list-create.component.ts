import { Component, Input } from '@angular/core';

@Component({
  selector: 'app-list-create',
  templateUrl: './list-create.component.html',
  styleUrls: ['./list-create.component.scss'],
})
export class ListCreateComponent {
  @Input() isArchived: boolean = false;
}
