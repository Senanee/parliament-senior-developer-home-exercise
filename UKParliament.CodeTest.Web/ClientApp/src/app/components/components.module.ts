import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { PersonListComponent } from './person/person-list/person-list.component';
import { PersonEditorComponent } from './person/person-editor/person-editor.component';
import { ContactComponent } from './contact/contact.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { BrowserModule } from '@angular/platform-browser';
import { SharedModule } from '../shared/shared.module';
import { AddPersonComponent } from './person/add-person/add-person.component';

@NgModule({
  declarations: [
    PersonListComponent,
    PersonEditorComponent,
    ContactComponent,
    AddPersonComponent
  ],
  imports: [
    CommonModule,
    BrowserModule,
    FormsModule,
    ReactiveFormsModule,
    SharedModule
  ],
  exports: [
    PersonListComponent,
    PersonEditorComponent,
    ContactComponent,
    AddPersonComponent
  ]
})
export class ComponentsModule { }
