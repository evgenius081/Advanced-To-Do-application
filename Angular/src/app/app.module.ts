import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { RouterTestingModule } from "@angular/router/testing";
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { MatSidenavModule } from '@angular/material/sidenav';
import { AppRoutingModule } from './app-routing/app-routing.module'
import { MatMenuModule } from "@angular/material/menu";
import { FormsModule, ReactiveFormsModule } from "@angular/forms";
import { MatDialogModule } from "@angular/material/dialog";
import { MatSelectModule } from "@angular/material/select";
import { NgxMatDatetimePickerModule } from "@angular-material-components/datetime-picker";
import { NgxMatMomentModule } from "@angular-material-components/moment-adapter"
import { MatInputModule } from "@angular/material/input"
import { CommonModule  } from '@angular/common';

import { AppComponent } from './app.component';
import { NotFoundComponent } from './not-found/not-found.component'
import { RouterLink } from "@angular/router";
import { NgOptimizedImage } from "@angular/common";
import { InternalErrorComponent } from './internal-error/internal-error.component';
import { SideMenuComponent } from './side-menu/side-menu.component';
import { SideMenuListElementComponent } from './side-menu/side-menu-list-element/side-menu-list-element.component';
import { MatExpansionModule } from "@angular/material/expansion";
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import { CallbackPipe } from './callback.pipe';
import { NgCircleProgressModule } from 'ng-circle-progress';
import { MatIconModule } from "@angular/material/icon";
import { ListViewComponent } from './list/list-view/list-view.component';
import { ItemViewComponent } from './item/item-view/item-view.component';
import { DragDropModule } from '@angular/cdk/drag-drop';
import { ValidationMessageComponent } from './validation-message/validation-message.component';
import { ListCreateComponent } from './list/list-create/list-create.component';
import { ChoiceDialogComponent } from './choice-dialog/choice-dialog.component';
import { ListTitleEditorComponent } from './list/list-title-editor/list-title-editor.component';
import { ItemCreateComponent } from './item/item-create/item-create.component';
import {MatDatepickerModule} from "@angular/material/datepicker";
import {SelectableTextDirective} from "./directives/selectable-text.directive";
import { TodayComponent } from './today/today.component';
import { HighPriorityComponent } from './high-priority/high-priority.component';
import { LoginComponent } from './login/login.component';
import { RegisterComponent } from './register/register.component';

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
    RegisterComponent
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
      radius: 20
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
    CommonModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
