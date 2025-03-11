import { TestBed } from '@angular/core/testing';
import { PersonEditorService } from './person-editor.service';

describe('PersonEditorService', () => {
  let service: PersonEditorService;

  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [PersonEditorService]
    });
    service = TestBed.inject(PersonEditorService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });

  it('should emit a value when closeEditor is called', (done: DoneFn) => {
    service.closeEditor$.subscribe(() => {
      expect(true).toBeTrue();
      done();
    });

    service.closeEditor();
  });
});
