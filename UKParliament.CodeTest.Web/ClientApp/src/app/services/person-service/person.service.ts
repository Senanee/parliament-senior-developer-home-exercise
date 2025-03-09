import { Inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, map } from 'rxjs';
import { PersonViewModel } from '../../models/person-view-model';
import { ResponseModel } from '../../models/response.model';

@Injectable({
  providedIn: 'root'
})
export class PersonService {

  constructor(private http: HttpClient, @Inject('BASE_URL') private baseUrl: string) { }

  getPeople(): Observable<PersonViewModel[]> {
    return this.http.get<PersonViewModel[]>(`${this.baseUrl}api/person`);
  }

  getPerson(id: number): Observable<PersonViewModel> {
    return this.http.get<PersonViewModel>(`${this.baseUrl}api/person/${id}`);
  }

  addPerson(person: PersonViewModel): Observable<PersonViewModel> {
    return this.http.post<ResponseModel<PersonViewModel>>(`${this.baseUrl}api/person`, person).pipe(
      map(response => {
        if (response.flag) {
          return response.data;
        } else {
          throw new Error(response.message);
        }
      })
    );
  }

  updatePerson(person: PersonViewModel): Observable<void> {
    return this.http.put<ResponseModel<void>>(`${this.baseUrl}api/person/${person.id}`, person).pipe(
      map(response => {
        if (response.flag) {
          return;
        } else {
          throw new Error(response.message);
        }
      })
    );
  }

  deletePerson(id: number): Observable<void> {
    return this.http.delete<ResponseModel<void>>(`${this.baseUrl}api/person/${id}`).pipe(
      map(response => {
        if (response.flag) {
          return;
        } else {
          throw new Error(response.message);
        }
      })
    );
  }
}

