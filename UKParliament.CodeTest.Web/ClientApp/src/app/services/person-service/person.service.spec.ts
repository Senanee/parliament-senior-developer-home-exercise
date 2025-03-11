import { TestBed } from '@angular/core/testing';
import { HttpClientTestingModule, HttpTestingController } from '@angular/common/http/testing';
import { PersonService } from './person.service';
import { PersonViewModel } from '../../models/person-view-model';
import { ResponseModel } from '../../models/response.model';

describe('PersonService', () => {
  let service: PersonService;
  let httpMock: HttpTestingController;
  const baseUrl = 'http://localhost/';

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule],
      providers: [
        PersonService,
        { provide: 'BASE_URL', useValue: baseUrl },
      ],
    });
    service = TestBed.inject(PersonService);
    httpMock = TestBed.inject(HttpTestingController);
  });

  afterEach(() => {
    httpMock.verify();
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });

  describe('#getPeople', () => {
    it('should fetch people via GET', () => {
      const dummyPeople: PersonViewModel[] = [
        {
          id: 1,
          firstName: 'John',
          lastName: 'Doe',
          dateOfBirth: new Date('2000-01-01'),
          departmentId: 1,
          email: 'john.doe@example.com'
        },
        {
          id: 2,
          firstName: 'Jane',
          lastName: 'Doe',
          dateOfBirth: new Date('2000-02-01'),
          departmentId: 2,
          email: 'jane.doe@example.com'
        }
      ];

      service.getPeople().subscribe(people => {
        expect(people.length).toBe(2);
        expect(people).toEqual(dummyPeople);
      });

      const req = httpMock.expectOne(`${baseUrl}api/person`);
      expect(req.request.method).toBe('GET');
      req.flush(dummyPeople);
    });
  });

  describe('#getPerson', () => {
    it('should fetch a person via GET', () => {
      const dummyPerson: PersonViewModel = {
        id: 1,
        firstName: 'John',
        lastName: 'Doe',
        dateOfBirth: new Date('2000-01-01'),
        departmentId: 1,
        email: 'john@example.com'
      };

      service.getPerson(1).subscribe(person => {
        expect(person).toEqual(dummyPerson);
      });

      const req = httpMock.expectOne(`${baseUrl}api/person/1`);
      expect(req.request.method).toBe('GET');
      req.flush(dummyPerson);
    });
  });

  describe('#addPerson', () => {
    it('should add a person successfully', () => {
      const newPerson: PersonViewModel = {
        id: 0,
        firstName: 'Test',
        lastName: 'User',
        dateOfBirth: new Date('2001-01-01'),
        departmentId: 1,
        email: 'test@example.com'
      };

      const response: ResponseModel<PersonViewModel> = {
        flag: true,
        data: { ...newPerson, id: 123 },
        message: 'Success'
      };

      service.addPerson(newPerson).subscribe(person => {
        expect(person).toEqual(response.data);
      });

      const req = httpMock.expectOne(`${baseUrl}api/person`);
      expect(req.request.method).toBe('POST');
      expect(req.request.body).toEqual(newPerson);
      req.flush(response);
    });

    it('should throw error when addPerson response flag is false', () => {
      const newPerson: PersonViewModel = {
        id: 0,
        firstName: 'Test',
        lastName: 'User',
        dateOfBirth: new Date('2001-01-01'),
        departmentId: 1,
        email: 'test@example.com'
      };

      const response: ResponseModel<PersonViewModel> = {
        flag: false,
        data: newPerson,
        message: 'Failed to add person'
      };

      service.addPerson(newPerson).subscribe({
        next: () => fail('expected error'),
        error: error => {
          expect(error.message).toEqual(response.message);
        }
      });

      const req = httpMock.expectOne(`${baseUrl}api/person`);
      expect(req.request.method).toBe('POST');
      req.flush(response);
    });
  });

  describe('#updatePerson', () => {
    it('should update a person successfully', () => {
      const updatedPerson: PersonViewModel = {
        id: 1,
        firstName: 'Updated',
        lastName: 'User',
        dateOfBirth: new Date('1999-12-12'),
        departmentId: 1,
        email: 'updated@example.com'
      };

      const response: ResponseModel<void> = {
        flag: true,
        data: undefined,
        message: 'Updated'
      };

      service.updatePerson(updatedPerson).subscribe(result => {
        expect(result).toBeUndefined();
      });

      const req = httpMock.expectOne(`${baseUrl}api/person/1`);
      expect(req.request.method).toBe('PUT');
      expect(req.request.body).toEqual(updatedPerson);
      req.flush(response);
    });

    it('should throw error when updatePerson response flag is false', () => {
      const updatedPerson: PersonViewModel = {
        id: 1,
        firstName: 'Updated',
        lastName: 'User',
        dateOfBirth: new Date('1999-12-12'),
        departmentId: 1,
        email: 'updated@example.com'
      };

      const response: ResponseModel<void> = {
        flag: false,
        data: undefined,
        message: 'Update failed'
      };

      service.updatePerson(updatedPerson).subscribe({
        next: () => fail('expected error'),
        error: error => {
          expect(error.message).toEqual(response.message);
        }
      });

      const req = httpMock.expectOne(`${baseUrl}api/person/1`);
      expect(req.request.method).toBe('PUT');
      req.flush(response);
    });
  });

  describe('#deletePerson', () => {
    it('should delete a person successfully', () => {
      const response: ResponseModel<void> = {
        flag: true,
        data: undefined,
        message: 'Deleted'
      };

      service.deletePerson(1).subscribe(result => {
        expect(result).toBeUndefined();
      });

      const req = httpMock.expectOne(`${baseUrl}api/person/1`);
      expect(req.request.method).toBe('DELETE');
      req.flush(response);
    });

    it('should throw error when deletePerson response flag is false', () => {
      const response: ResponseModel<void> = {
        flag: false,
        data: undefined,
        message: 'Delete failed'
      };

      service.deletePerson(1).subscribe({
        next: () => fail('expected error'),
        error: error => {
          expect(error.message).toEqual(response.message);
        }
      });

      const req = httpMock.expectOne(`${baseUrl}api/person/1`);
      expect(req.request.method).toBe('DELETE');
      req.flush(response);
    });
  });
});
