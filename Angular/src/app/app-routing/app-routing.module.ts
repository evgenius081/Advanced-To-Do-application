import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import {RouterModule, Routes} from "@angular/router";
import {InternalErrorComponent} from "../internal-error/internal-error.component";
import {NotFoundComponent} from "../not-found/not-found.component";
import {ListViewComponent} from "../list-view/list-view.component";
import { ListCreateComponent } from "../list-create/list-create.component";

const routes: Routes = [
  { path: 'lists/create', component: ListCreateComponent },
  { path: 'lists/:id', component: ListViewComponent },
  { path: 'error', component: InternalErrorComponent },
  { path: 'not-found', component: NotFoundComponent },
  { path: '**', component: NotFoundComponent }
];


@NgModule({
  declarations: [],
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
