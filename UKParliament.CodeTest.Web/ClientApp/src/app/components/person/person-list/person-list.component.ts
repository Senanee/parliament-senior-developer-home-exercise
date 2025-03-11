import { Component, OnInit } from '@angular/core';
import { PersonViewModel } from '../../../models/person-view-model';
import { PersonService } from '../../../services/person-service/person.service';
import { PersonEditorService } from '../../../services/person-editor/person-editor.service';

@Component({
  selector: 'app-person-list',
  templateUrl: './person-list.component.html',
  styleUrls: ['./person-list.component.scss']
})
export class PersonListComponent implements OnInit {
  people: PersonViewModel[] = [];
  filteredPeople: PersonViewModel[] = [];
  searchTerm: string = '';
  selectedPerson: PersonViewModel | null = null;
  loading: boolean = true;
  addUser: boolean= false;

  constructor(private personService: PersonService, private personEditorService: PersonEditorService) { }

  ngOnInit(): void {
    this.loadPeople();
  }

  loadPeople(): void {
    this.personService.getPeople().subscribe(people => {
      this.people = people;
      this.filterPeople();
    });
    this.loading = false;
  }

  filterPeople(): void {
    this.filteredPeople = this.people.filter(person =>
      person.firstName.toLowerCase().includes(this.searchTerm.toLowerCase()) ||
      person.lastName.toLowerCase().includes(this.searchTerm.toLowerCase())
    );
  }

  clearSearch(): void {
    this.searchTerm = '';
    this.filteredPeople = this.people;
  }

  selectPerson(person: PersonViewModel): void {
    this.personEditorService.closeEditor();
    setTimeout(() => {
      this.selectedPerson = person;
    }, 0);
  }

  addPerson(): void {
    this.addUser=true;
    this.personEditorService.closeEditor();
    this.selectedPerson = null;
  }

  closePersonEditor(): void {
    this.addUser = false;
    this.selectedPerson = null;
    this.personEditorService.closeEditor();
    this.loadPeople();
  }
}
