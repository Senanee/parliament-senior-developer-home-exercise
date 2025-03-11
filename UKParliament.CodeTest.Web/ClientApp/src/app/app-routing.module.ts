import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ContactComponent } from './components/contact/contact.component';
import { PersonListComponent } from './components/person/person-list/person-list.component';
import { PageNotFoundComponent } from './shared/components/page-not-found/page-not-found.component';
import { PersonEditorComponent } from './components/person/person-editor/person-editor.component';
import { AddPersonComponent } from './components/person/add-person/add-person.component';

const routes: Routes = [{ path: 'people', component: PersonListComponent },
{ path: '', redirectTo: '/people', pathMatch: 'full' },
{ path: 'create', component: AddPersonComponent },
{
  path: 'contact',
  component: ContactComponent,
},
{ path: '**', component: PageNotFoundComponent }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule],
})
export class AppRoutingModule { }
