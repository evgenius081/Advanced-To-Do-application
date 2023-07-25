import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { RouterTestingModule } from "@angular/router/testing";
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { MatSidenavModule } from '@angular/material/sidenav';
import { AppRoutingModule } from './app-routing/app-routing.module'
import { MatMenuModule } from "@angular/material/menu";
import { FormsModule } from "@angular/forms";

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
import { ListViewComponent } from './list-view/list-view.component';
import { ItemViewComponent } from './item-view/item-view.component';
import { DragDropModule } from '@angular/cdk/drag-drop';
import { ValidationMessageComponent } from './validation-message/validation-message.component';
import { ListCreateComponent } from './list-create/list-create.component';

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
    ListCreateComponent
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
    FormsModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
