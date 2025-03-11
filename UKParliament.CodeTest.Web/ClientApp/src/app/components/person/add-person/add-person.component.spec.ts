import { ComponentFixture, TestBed } from '@angular/core/testing';
import { ReactiveFormsModule } from '@angular/forms';
import { of, throwError } from 'rxjs';
import { AddPersonComponent } from './add-person.component';
import { DepartmentService } from '../../../services/department-service/department.service';
import { PersonService } from '../../../services/person-service/person.service';
import { PersonEditorService } from '../../../services/person-editor/person-editor.service';
import { DepartmentViewModel } from '../../../models/department-view-model';
import { PersonViewModel } from '../../../models/person-view-model';
import { Router } from '@angular/router';
import { By } from '@angular/platform-browser';

describe('AddPersonComponent', () => {
  let component: AddPersonComponent;
  let fixture: ComponentFixture<AddPersonComponent>;
  let mockDepartmentService: jasmine.SpyObj<DepartmentService>;
  let mockPersonService: jasmine.SpyObj<PersonService>;
  let mockPersonEditorService: jasmine.SpyObj<PersonEditorService>;
  let mockRouter: jasmine.SpyObj<Router>;

  beforeEach(async () => {
    mockDepartmentService = jasmine.createSpyObj('DepartmentService', ['getDepartments']);
    mockPersonService = jasmine.createSpyObj('PersonService', ['addPerson']);
    mockPersonEditorService = jasmine.createSpyObj('PersonEditorService', ['closeEditor$']);
    mockRouter = jasmine.createSpyObj('Router', ['navigateByUrl']);

    await TestBed.configureTestingModule({
      declarations: [AddPersonComponent],
      imports: [ReactiveFormsModule],
      providers: [
        { provide: DepartmentService, useValue: mockDepartmentService },
        { provide: PersonService, useValue: mockPersonService },
        { provide: PersonEditorService, useValue: mockPersonEditorService },
        { provide: Router, useValue: mockRouter }
      ]
    }).compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(AddPersonComponent);
    component = fixture.componentInstance;
    mockDepartmentService.getDepartments.and.returnValue(of([{ id: 1, name: 'HR' }] as DepartmentViewModel[]));
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should load departments on init', () => {
    expect(mockDepartmentService.getDepartments).toHaveBeenCalled();
    expect(component.departments.length).toBe(1);
  });

  it('should initialize the form', () => {
    expect(component.personForm).toBeDefined();
    expect(component.personForm.controls['firstName']).toBeDefined();
    expect(component.personForm.controls['lastName']).toBeDefined();
    expect(component.personForm.controls['dateOfBirth']).toBeDefined();
    expect(component.personForm.controls['email']).toBeDefined();
    expect(component.personForm.controls['departmentId']).toBeDefined();
  });

  it('should submit the form and add a person', () => {
    const person: PersonViewModel = {
      id: 1,
      firstName: 'John',
      lastName: 'Doe',
      dateOfBirth: new Date('1990-01-01'),
      email: 'john.doe@example.com',
      departmentId: 1
    };
    mockPersonService.addPerson.and.returnValue(of(person));
    component.personForm.setValue({
      firstName: 'John',
      lastName: 'Doe',
      dateOfBirth: '1990-01-01',
      email: 'john.doe@example.com',
      departmentId: 1
    });
    component.onSubmit();
    expect(mockPersonService.addPerson).toHaveBeenCalled();
    expect(mockRouter.navigateByUrl).toHaveBeenCalledWith('/people');
  });

  it('should handle form submission errors', () => {
    mockPersonService.addPerson.and.returnValue(throwError({ error: { errors: ['Error occurred'] } }));
    component.personForm.setValue({
      firstName: 'John',
      lastName: 'Doe',
      dateOfBirth: '1990-01-01',
      email: 'john.doe@example.com',
      departmentId: 1
    });
    component.onSubmit();
    expect(component.validationErrors.length).toBe(1);
  });

  it('should close the form', () => {
    spyOn(component.close, 'emit');
    component.closeForm();
    expect(component.close.emit).toHaveBeenCalled();
    expect(mockRouter.navigateByUrl).toHaveBeenCalledWith('/people');
  });
});
