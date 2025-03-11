import { ComponentFixture, TestBed, fakeAsync, tick } from '@angular/core/testing';
import { ReactiveFormsModule, FormBuilder } from '@angular/forms';
import { of, throwError, Subject } from 'rxjs';
import { formatDate } from '@angular/common';
import { PersonEditorComponent } from './person-editor.component';
import { DepartmentService } from '../../../services/department-service/department.service';
import { PersonEditorService } from '../../../services/person-editor/person-editor.service';
import { PersonService } from '../../../services/person-service/person.service';
import { DepartmentViewModel } from '../../../models/department-view-model';
import { PersonViewModel } from '../../../models/person-view-model';

describe('PersonEditorComponent', () => {
  let component: PersonEditorComponent;
  let fixture: ComponentFixture<PersonEditorComponent>;
  let departmentServiceMock: any;
  let personEditorServiceMock: any;
  let personServiceMock: any;
  let closeEditorSubject: Subject<void>;

  const mockPerson: PersonViewModel = {
    id: 1,
    firstName: 'John',
    lastName: 'Doe',
    dateOfBirth: new Date('1990-01-01'),
    departmentId: 2,
    email: 'john.doe@example.com'
  };

  const mockDepartments: DepartmentViewModel[] = [
    { id: 1, name: 'HR' },
    { id: 2, name: 'Engineering' }
  ];

  beforeEach(async () => {
    departmentServiceMock = jasmine.createSpyObj('DepartmentService', ['getDepartments']);
    departmentServiceMock.getDepartments.and.returnValue(of(mockDepartments));

    closeEditorSubject = new Subject<void>();
    personEditorServiceMock = {
      closeEditor$: closeEditorSubject.asObservable(),
      closeEditor: jasmine.createSpy('closeEditor')
    };

    personServiceMock = jasmine.createSpyObj('PersonService', ['updatePerson', 'deletePerson']);

    await TestBed.configureTestingModule({
      imports: [ReactiveFormsModule],
      declarations: [PersonEditorComponent],
      providers: [
        FormBuilder,
        { provide: DepartmentService, useValue: departmentServiceMock },
        { provide: PersonEditorService, useValue: personEditorServiceMock },
        { provide: PersonService, useValue: personServiceMock }
      ]
    }).compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(PersonEditorComponent);
    component = fixture.componentInstance;
    component.person = { ...mockPerson };
    fixture.detectChanges();
  });

  it('should create the component', () => {
    expect(component).toBeTruthy();
  });

  it('should initialize form and load departments on init', () => {
    expect(component.personForm).toBeDefined();
    expect(component.departments).toEqual(mockDepartments);
    expect(component.loading).toBeFalse();
    expect(component.isEditMode).toBeFalse();
  });

  it('should update the form when ngOnChanges is triggered', () => {
    spyOn(component, 'updateForm').and.callThrough();
    const newPerson: PersonViewModel = {
      id: 1,
      firstName: 'Alice',
      lastName: 'Wonderland',
      dateOfBirth: new Date('1985-05-05'),
      departmentId: 1,
      email: 'alice@example.com'
    };

    component.person = newPerson;
    component.ngOnChanges({
      person: {
        currentValue: newPerson,
        previousValue: mockPerson,
        firstChange: false,
        isFirstChange: () => false
      }
    });

    expect(component.initialPersonValues).toEqual(newPerson);
    expect(component.updateForm).toHaveBeenCalled();
    expect(component.personForm.get('firstName')?.value).toBe(newPerson.firstName);
    expect(component.personForm.get('lastName')?.value).toBe(newPerson.lastName);
    expect(component.personForm.get('email')?.value).toBe(newPerson.email);
  });

  it('toggleEditMode should toggle isEditMode and enable/disable the form accordingly', () => {
    component.personForm.disable();
    component.toggleEditMode();
    expect(component.isEditMode).toBeTrue();
    expect(component.personForm.enabled).toBeTrue();

    component.toggleEditMode();
    expect(component.isEditMode).toBeFalse();
    expect(component.personForm.disabled).toBeTrue();
  });

  it('onSave should call personService.updatePerson when the form is valid', () => {
    component.isEditMode = true;
    component.personForm.enable();
    component.personForm.setValue({
      firstName: 'Updated',
      lastName: 'User',
      dateOfBirth: formatDate(new Date('2000-12-12'), 'yyyy-MM-dd', 'en'),
      departmentId: 2,
      email: 'updated@example.com'
    });
    personServiceMock.updatePerson.and.returnValue(of({}));
    spyOn(component, 'closeForm').and.callThrough();

    component.onSave();
    expect(component.submitted).toBeTrue();
    expect(personServiceMock.updatePerson).toHaveBeenCalled();
    expect(component.closeForm).toHaveBeenCalled();
  });

  it('onSave should handle error response from updatePerson', () => {
    component.isEditMode = true;
    component.personForm.enable();
    component.personForm.setValue({
      firstName: 'Updated',
      lastName: 'User',
      dateOfBirth: formatDate(new Date('2000-12-12'), 'yyyy-MM-dd', 'en'),
      departmentId: 2,
      email: 'updated@example.com'
    });
    const errorResponse = { error: { errors: ['Error occurred'] } };
    personServiceMock.updatePerson.and.returnValue(throwError(errorResponse));

    component.onSave();
    expect(component.submitted).toBeTrue();
    expect(component.validationErrors).toContain(JSON.stringify(errorResponse.error.errors));
  });

  it('resetForm should reset form values to initialPersonValues', () => {
    component.personForm.patchValue({
      firstName: 'Changed',
      lastName: 'Changed',
      dateOfBirth: formatDate(new Date('1999-09-09'), 'yyyy-MM-dd', 'en'),
      departmentId: 99,
      email: 'changed@example.com'
    });
    component.resetForm();
    expect(component.personForm.get('firstName')?.value).toBe(component.initialPersonValues.firstName);
    expect(component.personForm.get('lastName')?.value).toBe(component.initialPersonValues.lastName);
  });

  it('closeEditor should disable edit mode and reset the form', () => {
    component.isEditMode = true;
    component.personForm.enable();
    component.personForm.patchValue({ firstName: 'Changed' });
    component.closeEditor();
    expect(component.isEditMode).toBeFalse();
    expect(component.personForm.disabled).toBeTrue();
    expect(component.personForm.get('firstName')?.value).toBe(component.initialPersonValues.firstName);
  });

  it('confirmDelete should call deletePerson and then closeForm when confirmed', () => {
    spyOn(window, 'confirm').and.returnValue(true);
    personServiceMock.deletePerson.and.returnValue(of({}));
    spyOn(component, 'closeForm').and.callThrough();

    component.confirmDelete();
    expect(window.confirm).toHaveBeenCalledWith('Are you sure you want to delete this person?');
    expect(personServiceMock.deletePerson).toHaveBeenCalledWith(mockPerson.id);
    expect(component.closeForm).toHaveBeenCalled();
  });

  it('confirmDelete should do nothing if deletion is not confirmed', () => {
    spyOn(window, 'confirm').and.returnValue(false);
    component.confirmDelete();
    expect(window.confirm).toHaveBeenCalledWith('Are you sure you want to delete this person?');
    expect(personServiceMock.deletePerson).not.toHaveBeenCalled();
  });

  it('closeForm should emit the close event', () => {
    spyOn(component.close, 'emit');
    component.closeForm();
    expect(component.close.emit).toHaveBeenCalled();
  });

  it('should unsubscribe from closeEditorSubscription on destroy', () => {
    const spyUnsubscribe = spyOn(component['closeEditorSubscription'], 'unsubscribe');
    component.ngOnDestroy();
    expect(spyUnsubscribe).toHaveBeenCalled();
  });

  it('should handle closeEditor call via personEditorService.closeEditor$ subscription', () => {
    component.isEditMode = true;
    component.personForm.enable();
    closeEditorSubject.next();
    expect(component.isEditMode).toBeFalse();
    expect(component.personForm.disabled).toBeTrue();
    expect(component.personForm.get('firstName')?.value).toBe(component.initialPersonValues.firstName);
  });
});
