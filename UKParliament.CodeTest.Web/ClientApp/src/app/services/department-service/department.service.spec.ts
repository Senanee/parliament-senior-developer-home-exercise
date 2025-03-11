import { TestBed } from '@angular/core/testing';
import { HttpClientTestingModule, HttpTestingController } from '@angular/common/http/testing';
import { DepartmentService } from './department.service';
import { DepartmentViewModel } from '../../models/department-view-model';

describe('DepartmentService', () => {
  let service: DepartmentService;
  let httpMock: HttpTestingController;
  const baseUrl = 'http://localhost:5000/';

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule],
      providers: [
        DepartmentService,
        { provide: 'BASE_URL', useValue: baseUrl }
      ]
    });

    service = TestBed.inject(DepartmentService);
    httpMock = TestBed.inject(HttpTestingController);
  });

  afterEach(() => {
    httpMock.verify();
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });

  it('should fetch departments', () => {
    const mockDepartments: DepartmentViewModel[] = [
      { id: 1, name: 'HR' },
      { id: 2, name: 'IT' }
    ];

    service.getDepartments().subscribe(departments => {
      expect(departments.length).toBe(2);
      expect(departments).toEqual(mockDepartments);
    });

    const req = httpMock.expectOne(baseUrl + 'api/department');
    expect(req.request.method).toBe('GET');
    req.flush(mockDepartments);
  });
});
