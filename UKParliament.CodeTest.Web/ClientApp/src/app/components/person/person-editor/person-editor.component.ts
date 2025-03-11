import { formatDate } from '@angular/common';
import { Component, EventEmitter, Input, OnChanges, OnDestroy, OnInit, Output, SimpleChanges } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { DepartmentViewModel } from '../../../models/department-view-model';
import { PersonViewModel } from '../../../models/person-view-model';
import { DepartmentService } from '../../../services/department-service/department.service';
import { Subscription } from 'rxjs';
import { PersonEditorService } from '../../../services/person-editor/person-editor.service';
import { PersonService } from '../../../services/person-service/person.service';
import { MaxDateValidator } from '../../../shared/validations/max-date-validator';

@Component({
  selector: 'app-person-editor',
  templateUrl: './person-editor.component.html',
  styleUrls: ['./person-editor.component.scss']
})
export class PersonEditorComponent implements OnInit, OnDestroy, OnChanges {
  @Input() person!: PersonViewModel;
  @Output() close = new EventEmitter<void>();
  departments: DepartmentViewModel[] = [];
  personForm!: FormGroup;
  loading: boolean = true;
  isEditMode: boolean = false;
  initialPersonValues!: PersonViewModel;
  private closeEditorSubscription!: Subscription;
  validationErrors: string[] = [];
  submitted: boolean = false;
  maxDate: string = new Date().toISOString().split('T')[0];

  constructor(private fb: FormBuilder, private departmentService: DepartmentService, private personEditorService: PersonEditorService, private personService: PersonService) { }

  ngOnChanges(changes: SimpleChanges): void {
    if (changes['person'] && !changes['person'].firstChange) {
      this.initialPersonValues = { ...this.person };
      this.updateForm();
    }
  }

  ngOnInit(): void {
    this.loadDepartments();
    this.initForm();
    this.initialPersonValues = { ...this.person };

    this.closeEditorSubscription = this.personEditorService.closeEditor$.subscribe(() => this.closeEditor());
  }

  ngOnDestroy(): void {
    this.closeEditorSubscription.unsubscribe();
  }

  initForm(): void {
    this.personForm = this.fb.group(
      {
        firstName: [{ value: this.person.firstName || '', disabled: !this.isEditMode }, Validators.required],
        lastName: [{ value: this.person.lastName || '', disabled: !this.isEditMode }, Validators.required],
        dateOfBirth: [{ value: formatDate(this.person.dateOfBirth, 'yyyy-MM-dd', 'en') || '', disabled: !this.isEditMode }, [Validators.required]],
        departmentId: [{ value: this.person.departmentId || '', disabled: !this.isEditMode }, Validators.required],
        email: [{ value: this.person.email || '', disabled: !this.isEditMode }, [Validators.required, Validators.email]]
      }
    );
  }

  updateForm(): void {
    this.personForm.patchValue({
      firstName: this.person.firstName,
      lastName: this.person.lastName,
      dateOfBirth: formatDate(this.person.dateOfBirth, 'yyyy-MM-dd', 'en'),
      departmentId: this.person.departmentId,
      email: this.person.email
    });
  }

  loadDepartments(): void {
    this.departmentService.getDepartments().subscribe(departments => this.departments = departments);
    this.loading = false;
  }

  toggleEditMode(): void {
    this.isEditMode = !this.isEditMode;
    if (this.isEditMode) {
      this.personForm.enable();
    } else {
      this.personForm.disable();
      this.resetForm();
    }
  }

  onSave(): void {
    if (this.personForm.valid) {
      this.submitted = true;
      const updatedPerson = {
        ...this.person,
        ...this.personForm.value,
      };
      this.personService.updatePerson(updatedPerson).subscribe({
        next: (response) => {
          alert(response);
          this.closeForm();
        },
        error: (errorResponse: { error: { errors: string[]; }; }) => {
          if (errorResponse.error && errorResponse.error.errors) {
            this.validationErrors.push(JSON.stringify(errorResponse.error.errors));
          } else {
            this.validationErrors = ['An unexpected error occurred.'];
          }
        }
      });
    }
  }

  resetForm(): void {
    this.personForm.reset({
      firstName: this.initialPersonValues.firstName,
      lastName: this.initialPersonValues.lastName,
      dateOfBirth: formatDate(this.initialPersonValues.dateOfBirth, 'yyyy-MM-dd', 'en'),
      departmentId: this.initialPersonValues.departmentId,
      email: this.initialPersonValues.email
    });
  }

  closeEditor(): void {
    this.isEditMode = false;
    this.personForm.disable();
    this.resetForm();
  }

  confirmDelete(): void {
    if (confirm('Are you sure you want to delete this person?')) {
      this.personService.deletePerson(this.person.id).subscribe({
        next: () => {
          this.closeForm();
        },
        error: (errorResponse: { error: { errors: string[]; }; }) => {
          if (errorResponse.error && errorResponse.error.errors) {
            this.validationErrors = errorResponse.error.errors;
          }
        }
      });
    }
  }

  closeForm(): void {
    this.close.emit();
  }
}
