import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { PersonEditorService } from '../../../services/person-editor/person-editor.service';
import { PersonService } from '../../../services/person-service/person.service';
import { DepartmentViewModel } from '../../../models/department-view-model';
import { PersonViewModel } from '../../../models/person-view-model';
import { DepartmentService } from '../../../services/department-service/department.service';

@Component({
  selector: 'app-add-person',
  templateUrl: './add-person.component.html',
  styleUrls: ['./add-person.component.scss']
})
export class AddPersonComponent implements OnInit {
  @Output() close = new EventEmitter<void>();
  personForm: FormGroup;
  loading: boolean = true;
  departments: DepartmentViewModel[] = [];
  person!: PersonViewModel;
  validationErrors: string[] = [];
  submitted: boolean = false;
  maxDate: string = new Date().toISOString().split('T')[0];

  constructor(private fb: FormBuilder, private personService: PersonService, private departmentService: DepartmentService, private router: Router, private personEditorService: PersonEditorService) {
    this.personForm = this.fb.group({
      firstName: ['', Validators.required],
      lastName: ['', Validators.required],
      dateOfBirth: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]],
      departmentId: ['', Validators.required]
    });
  }

  ngOnInit(): void {
    this.loadDepartments();
  }

  loadDepartments(): void {
    this.departmentService.getDepartments().subscribe(departments => this.departments = departments);
    this.loading = false;
  }

  onSubmit(): void {
    if (this.personForm.valid) {
      this.submitted = true;
      const updatedPerson = {
        ...this.person,
        ...this.personForm.value,
      };
      this.personService.addPerson(updatedPerson).subscribe({
        next: () => {
          alert(`${updatedPerson.firstName} ${updatedPerson.lastName} created successfully!`);
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

  closeForm(): void {
    this.close.emit();
    this.router.navigateByUrl('/people');
  }
}
