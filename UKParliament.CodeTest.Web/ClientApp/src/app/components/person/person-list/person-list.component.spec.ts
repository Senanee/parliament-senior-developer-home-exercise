import { ComponentFixture, TestBed } from '@angular/core/testing';
import { of } from 'rxjs';
import { PersonListComponent } from './person-list.component';
import { PersonService } from '../../../services/person-service/person.service';
import { PersonEditorService } from '../../../services/person-editor/person-editor.service';
import { PersonViewModel } from '../../../models/person-view-model';

describe('PersonListComponent', () => {
  let component: PersonListComponent;
  let fixture: ComponentFixture<PersonListComponent>;
  let personServiceMock: any;
  let personEditorServiceMock: any;

  const mockPeople: PersonViewModel[] = [
    {
      id: 1,
      firstName: 'John',
      lastName: 'Doe',
      dateOfBirth: new Date('1990-01-01'),
      email: 'john.doe@example.com',
      departmentId: 1
    },
    { id: 2, firstName: 'Jane', lastName: 'Smith', dateOfBirth: new Date('1995-02-12'), email: 'jane.smith@test.com', departmentId: 2 },
  ];

  beforeEach(async () => {
    // Mocking services
    personServiceMock = jasmine.createSpyObj('PersonService', ['getPeople']);
    personServiceMock.getPeople.and.returnValue(of(mockPeople));

    personEditorServiceMock = jasmine.createSpyObj('PersonEditorService', ['closeEditor']);

    await TestBed.configureTestingModule({
      declarations: [PersonListComponent],
      providers: [
        { provide: PersonService, useValue: personServiceMock },
        { provide: PersonEditorService, useValue: personEditorServiceMock },
      ],
    }).compileComponents();

    fixture = TestBed.createComponent(PersonListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges(); // Trigger ngOnInit
  });

  it('should create the component', () => {
    expect(component).toBeTruthy();
  });

  it('should load people on initialization', () => {
    expect(component.people).toEqual(mockPeople);
    expect(component.filteredPeople).toEqual(mockPeople);
    expect(component.loading).toBeFalse();
  });

  it('should filter people by searchTerm', () => {
    component.searchTerm = 'Jane';
    component.filterPeople();
    expect(component.filteredPeople).toEqual([{ id: 2, firstName: 'Jane', lastName: 'Smith', dateOfBirth: new Date('1995-02-12'), email: 'jane.smith@test.com', departmentId: 2 }]);
  });

  it('should clear the search term and reset filtered people', () => {
    component.searchTerm = 'Some Search';
    component.clearSearch();
    expect(component.searchTerm).toBe('');
    expect(component.filteredPeople).toEqual(mockPeople);
  });

  it('should select a person and close the editor', () => {
    const selectedPerson = mockPeople[0];
    component.selectPerson(selectedPerson);
    expect(personEditorServiceMock.closeEditor).toHaveBeenCalled();
    setTimeout(() => {
      expect(component.selectedPerson).toBe(selectedPerson);
    });
  });

  it('should add a person and reset selection', () => {
    component.addPerson();
    expect(component.addUser).toBeTrue();
    expect(personEditorServiceMock.closeEditor).toHaveBeenCalled();
    expect(component.selectedPerson).toBeNull();
  });

  it('should close the editor and reload people', () => {
    component.closePersonEditor();
    expect(component.addUser).toBeFalse();
    expect(component.selectedPerson).toBeNull();
    expect(personEditorServiceMock.closeEditor).toHaveBeenCalled();
    expect(personServiceMock.getPeople).toHaveBeenCalled();
  });
});
