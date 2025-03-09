import { Injectable } from '@angular/core';
import { Subject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class PersonEditorService {
  private closeEditorSubject = new Subject<void>();

  get closeEditor$() {
    return this.closeEditorSubject.asObservable();
  }

  closeEditor() {
    this.closeEditorSubject.next();
  }
}
