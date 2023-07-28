import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import {RouterModule, Routes} from "@angular/router";
import {InternalErrorComponent} from "../internal-error/internal-error.component";
import {NotFoundComponent} from "../not-found/not-found.component";
import {ListViewComponent} from "../list/list-view/list-view.component";
import { ListCreateComponent } from "../list/list-create/list-create.component";
import {TodayComponent} from "../today/today.component";
import {HighPriorityComponent} from "../high-priority/high-priority.component";

const routes: Routes = [
  { path: '', component: TodayComponent },
  { path: 'today', component: TodayComponent},
  { path: 'primary', component: HighPriorityComponent},
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
