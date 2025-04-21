import { Injectable } from '@angular/core';
import { endpoint } from '../../endpoints/endpoint';
import { HttpClient } from '@angular/common/http';
import { ApiService } from '../../core/service/api.service';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class RubricService {
  searchRubricUrl = `${endpoint.searchRubricUrl}`;
  rubricCreatedUrl = `${endpoint.rubricCreatedUrl}`;
  rubricUpdatedUrl = `${endpoint.rubricUpdatedUrl}`;
  rubricDeletedUrl = `${endpoint.rubricDeletedUrl}`;

  constructor(
    private readonly http: HttpClient,
    private readonly apiService: ApiService
  ) {}

  rubricDetailsData(rubricData: any): Observable<any> {
    return this.http.post(this.searchRubricUrl.trim(), rubricData);
  }

  rubricCreatedData(rubricData: any): Observable<any> {
    return this.http.post(this.rubricCreatedUrl.trim(), rubricData);
  }

  rubricUpdatedData(rubricData: {
    id: string;
    title: string;
    rubricDescription: string;
    seniorityId: string;
    subSkillId: string;
    weight: number;
  }): Observable<any> {
    return this.http.put(
      `${this.rubricUpdatedUrl.trim()}/${rubricData.id}`,
      rubricData
    );
  }

  rubricDeletedData(id: string): Observable<any> {
    return this.http.delete(`${this.rubricDeletedUrl.trim()}/${id}`);
  }
}
