import { TestBed } from '@angular/core/testing';

import { PersonEditorService } from './person-editor.service';

describe('PersonEditorService', () => {
  let service: PersonEditorService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(PersonEditorService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
