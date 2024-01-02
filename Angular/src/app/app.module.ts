import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { RouterTestingModule } from '@angular/router/testing';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { MatSidenavModule } from '@angular/material/sidenav';
import { AppRoutingModule } from './app-routing/app-routing.module';
import { MatMenuModule } from '@angular/material/menu';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MatDialogModule } from '@angular/material/dialog';
import { MatSelectModule } from '@angular/material/select';
import { NgxMatDatetimePickerModule } from '@angular-material-components/datetime-picker';
import { NgxMatMomentModule } from '@angular-material-components/moment-adapter';
import { MatInputModule } from '@angular/material/input';
import { CommonModule } from '@angular/common';

import { AppComponent } from './app.component';
import { NotFoundComponent } from './components/errors/not-found/not-found.component';
import { RouterLink } from '@angular/router';
import { NgOptimizedImage } from '@angular/common';
import { InternalErrorComponent } from './components/errors/internal-error/internal-error.component';
import { SideMenuComponent } from './components/side-menu/side-menu.component';
import { SideMenuListElementComponent } from './components/side-menu/side-menu-list-element/side-menu-list-element.component';
import { MatExpansionModule } from '@angular/material/expansion';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import { CallbackPipe } from './callback.pipe';
import { NgCircleProgressModule } from 'ng-circle-progress';
import { MatIconModule } from '@angular/material/icon';
import { ListViewComponent } from './components/list/list-view/list-view.component';
import { ItemViewComponent } from './components/item/item-view/item-view.component';
import { DragDropModule } from '@angular/cdk/drag-drop';
import { ValidationMessageComponent } from './shared/validation-message/validation-message.component';
import { ListCreateComponent } from './components/list/list-create/list-create.component';
import { ChoiceDialogComponent } from './shared/choice-dialog/choice-dialog.component';
import { ListTitleEditorComponent } from './components/list/list-title-editor/list-title-editor.component';
import { ItemCreateComponent } from './components/item/item-create/item-create.component';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { SelectableTextDirective } from './shared/directives/selectable-text.directive';
import { TodayComponent } from './components/today/today.component';
import { HighPriorityComponent } from './components/high-priority/high-priority.component';
import { LoginComponent } from './components/auth/login/login.component';
import { RegisterComponent } from './components/auth/register/register.component';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { ApiInterceptor } from './shared/services/http-interceptor';
import { NotificationsComponent } from './components/notifications/notifications.component';
import { NotificationComponent } from './components/notifications/notification/notification.component';

@NgModule({
  declarations: [
    AppComponent,
    NotFoundComponent,
    InternalErrorComponent,
    SideMenuComponent,
    SideMenuListElementComponent,
    CallbackPipe,
    ListViewComponent,
    ItemViewComponent,
    ValidationMessageComponent,
    ListCreateComponent,
    ChoiceDialogComponent,
    ListTitleEditorComponent,
    ItemCreateComponent,
    SelectableTextDirective,
    TodayComponent,
    HighPriorityComponent,
    LoginComponent,
    RegisterComponent,
    NotificationsComponent,
    NotificationComponent,
  ],
  imports: [
    BrowserModule,
    BrowserAnimationsModule,
    MatSidenavModule,
    RouterLink,
    RouterTestingModule,
    NgOptimizedImage,
    MatExpansionModule,
    FontAwesomeModule,
    NgCircleProgressModule.forRoot({
      radius: 20,
    }),
    MatIconModule,
    AppRoutingModule,
    MatMenuModule,
    DragDropModule,
    FormsModule,
    MatDialogModule,
    MatSelectModule,
    NgxMatDatetimePickerModule,
    MatDatepickerModule,
    NgxMatMomentModule,
    ReactiveFormsModule,
    MatInputModule,
    CommonModule,
    HttpClientModule,
  ],
  providers: [
    { provide: HTTP_INTERCEPTORS, useClass: ApiInterceptor, multi: true },
  ],
  bootstrap: [AppComponent],
})
export class AppModule {}
