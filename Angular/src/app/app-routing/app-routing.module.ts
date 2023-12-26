import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { InternalErrorComponent } from '../components/errors/internal-error/internal-error.component';
import { NotFoundComponent } from '../components/errors/not-found/not-found.component';
import { ListViewComponent } from '../components/list/list-view/list-view.component';
import { ListCreateComponent } from '../components/list/list-create/list-create.component';
import { TodayComponent } from '../components/today/today.component';
import { HighPriorityComponent } from '../components/high-priority/high-priority.component';
import { LoginComponent } from '../components/auth/login/login.component';
import { RegisterComponent } from '../components/auth/register/register.component';
import { AuthGuard } from './auth-guard';

const routes: Routes = [
  { path: '', component: TodayComponent, canActivate: [AuthGuard] },
  { path: 'today', component: TodayComponent, canActivate: [AuthGuard] },
  {
    path: 'primary',
    component: HighPriorityComponent,
    canActivate: [AuthGuard],
  },
  { path: 'login', component: LoginComponent },
  { path: 'register', component: RegisterComponent },
  {
    path: 'lists/create',
    component: ListCreateComponent,
    canActivate: [AuthGuard],
  },
  { path: 'lists/:id', component: ListViewComponent, canActivate: [AuthGuard] },
  {
    path: 'error',
    component: InternalErrorComponent,
    canActivate: [AuthGuard],
  },
  { path: 'not-found', component: NotFoundComponent, canActivate: [AuthGuard] },
  { path: '**', component: NotFoundComponent, canActivate: [AuthGuard] },
];

@NgModule({
  declarations: [],
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule],
})
export class AppRoutingModule {}
