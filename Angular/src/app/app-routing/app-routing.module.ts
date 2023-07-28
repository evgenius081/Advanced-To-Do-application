import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import {RouterModule, Routes} from "@angular/router";
import {InternalErrorComponent} from "../internal-error/internal-error.component";
import {NotFoundComponent} from "../not-found/not-found.component";
import {ListViewComponent} from "../list/list-view/list-view.component";
import { ListCreateComponent } from "../list/list-create/list-create.component";
import {TodayComponent} from "../today/today.component";
import {HighPriorityComponent} from "../high-priority/high-priority.component";
import {LoginComponent} from "../login/login.component";
import {RegisterComponent} from "../register/register.component";
import {AuthGuard} from "./auth-guard";

const routes: Routes = [
  { path: '', component: TodayComponent, canActivate: [AuthGuard] },
  { path: 'today', component: TodayComponent, canActivate: [AuthGuard] },
  { path: 'primary', component: HighPriorityComponent, canActivate: [AuthGuard] },
  { path: 'login', component: LoginComponent },
  { path: 'register', component: RegisterComponent },
  { path: 'lists/create', component: ListCreateComponent,canActivate: [AuthGuard] },
  { path: 'lists/:id', component: ListViewComponent, canActivate: [AuthGuard] },
  { path: 'error', component: InternalErrorComponent, canActivate: [AuthGuard] },
  { path: 'not-found', component: NotFoundComponent, canActivate: [AuthGuard] },
  { path: '**', component: NotFoundComponent, canActivate: [AuthGuard] }
];


@NgModule({
  declarations: [],
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
