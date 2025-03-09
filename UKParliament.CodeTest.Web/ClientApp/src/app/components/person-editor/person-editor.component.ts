import { formatDate } from '@angular/common';
import { Component, EventEmitter, Input, OnChanges, OnDestroy, OnInit, Output, SimpleChanges } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { DepartmentViewModel } from '../../models/department-view-model';
import { PersonViewModel } from '../../models/person-view-model';
import { DepartmentService } from '../../services/department-service/department.service';
import { Subscription } from 'rxjs';
import { PersonEditorService } from '../../services/person-editor/person-editor.service';

@Component({
  selector: 'app-person-editor',
  templateUrl: './person-editor.component.html',
  styleUrls: ['./person-editor.component.scss']
})
export class PersonEditorComponent implements OnInit, OnDestroy, OnChanges {
  @Input() person!: PersonViewModel;
  @Output() save = new EventEmitter<PersonViewModel>();
  @Output() delete = new EventEmitter<PersonViewModel>();
  @Output() close = new EventEmitter<void>();
  departments: DepartmentViewModel[] = [];
  personForm!: FormGroup;
  loading: boolean = true;
  isEditMode: boolean = false;
  isNewPerson: boolean = false;
  initialPersonValues!: PersonViewModel;
  private closeEditorSubscription!: Subscription;


  constructor(private fb: FormBuilder, private departmentService: DepartmentService, private personEditorService: PersonEditorService) { }

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

    if (!this.person.id) {
      this.isNewPerson = true;
      this.isEditMode = true;
      this.personForm.enable();
    }

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
        departmentId: [{ value: this.person.departmentId || '', disabled: !this.isEditMode }, Validators.required]
      }
    );
  }

  updateForm(): void {
    this.personForm.patchValue({
      firstName: this.person.firstName,
      lastName: this.person.lastName,
      dateOfBirth: formatDate(this.person.dateOfBirth, 'yyyy-MM-dd', 'en'),
      departmentId: this.person.departmentId
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
      const updatedPerson = {
        ...this.person,
        ...this.personForm.value,
      };
      this.save.emit(updatedPerson);
    }
  }

  resetForm(): void {
    this.personForm.reset({
      firstName: this.initialPersonValues.firstName,
      lastName: this.initialPersonValues.lastName,
      dateOfBirth: formatDate(this.initialPersonValues.dateOfBirth, 'yyyy-MM-dd', 'en'),
      departmentId: this.initialPersonValues.departmentId
    });
  }

  closeEditor(): void {
    this.isEditMode = false;
    this.personForm.disable();
    this.resetForm();
  }

  confirmDelete(): void {
    if (confirm('Are you sure you want to delete this person?')) {
      this.delete.emit(this.person);
    }
  }

  closeForm(): void {
    this.close.emit();
  }
}
