import { Injectable } from '@angular/core';
import { endpoint } from '../../endpoints/endpoint';
import { HttpClient } from '@angular/common/http';
import { ApiService } from '../../core/service/api.service';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class SkillService {
  skillDetailsUrl = `${endpoint.skillDetailsUrl}`;
  skillCreatedUrl = `${endpoint.skillCreatedUrl}`;
  skillUpdatedUrl = `${endpoint.skillUpdatedUrl}`;
  skillDeletedUrl = `${endpoint.skillDeletedUrl}`;

  subskillCreatedUrl = `${endpoint.subskillCreatedUrl}`;
  subskillUpdatedUrl = `${endpoint.subskillUpdatedUrl}`;
  subskillDeletedUrl = `${endpoint.subskillDeletedUrl}`;

  seniorityDetailsUrl = `${endpoint.seniorityDetailsUrl}`;
  seniorityCreatedUrl = `${endpoint.seniorityCreatedUrl}`;
  seniorityUpdatedsUrl = `${endpoint.seniorityUpdatedsUrl}`;
  seniorityDeletedUrl = `${endpoint.seniorityDeletedUrl}`;

  quizQuestionDetailsUrl = `${endpoint.quizQuestionDetailsUrl}`;
  quizQuestionCreatedUrl = `${endpoint.quizQuestionCreatedUrl}`;
  quizQuestionUpdatedUrl = `${endpoint.quizQuestionUpdatedUrl}`;
  quizQuestionDeletedUrl = `${endpoint.quizQuestionDeletedUrl}`;

  constructor(
    private readonly http: HttpClient,
    private readonly apiService: ApiService
  ) {}

  skillDetailsData(skillData: any): Observable<any> {
    return this.http.post(this.skillDetailsUrl.trim(), skillData);
  }
  skillCreatedData(skillData: any): Observable<any> {
    return this.http.post(this.skillCreatedUrl.trim(), skillData);
  }
  subskillCreatedData(skillId: string, subskillData: any): Observable<any> {
    return this.http.post(this.subskillCreatedUrl.trim(), {
      skillId,
      name: subskillData.name,
      description: subskillData.description,
    });
  }
  skillUpdatedData(skillData: {
    id: string;
    name: string;
    description: string;
    seniorityLevelIds: string[];
  }): Observable<any> {
    return this.http.put(
      `${this.skillUpdatedUrl.trim()}/${skillData.id}`,
      skillData
    );
  }

  skillDeletedData(id: string): Observable<any> {
    return this.http.delete(`${this.skillDeletedUrl.trim()}/${id}`);
  }

  subskillUpdatedData(subSkillId: any, subskillData: any): Observable<any> {
    console.log(subskillData);
    return this.http.put(
      `${this.subskillUpdatedUrl.trim()}/${subSkillId}`,
      subskillData
    );
  }

  subskillDeletedData(id: string): Observable<any> {
    return this.http.delete(`${this.subskillDeletedUrl.trim()}/${id}`);
  }

  seniorityDetailsData(seniorityData: any): Observable<any> {
    return this.http.post(this.seniorityDetailsUrl.trim(), seniorityData);
  }
  seniorityCreatedData(data: any): Observable<any> {
    return this.http.post(this.seniorityCreatedUrl, data);
  }

  seniorityUpdateData(data: any): Observable<any> {
    return this.http.put(`${this.seniorityUpdatedsUrl}/${data.id}`, data);
  }

  seniorityDeleteData(id: string): Observable<any> {
    return this.http.delete(`${this.seniorityDeletedUrl}/${id}`);
  }

  quizQuestionDetailsData(quizData: any): Observable<any> {
    return this.http.post(this.quizQuestionDetailsUrl.trim(), quizData);
  }
  quizQuestionCreatedData(quizData: any): Observable<any> {
    return this.http.post(this.quizQuestionCreatedUrl, quizData);
  }

  quizQuestionUpdateData(quizData: any): Observable<any> {
    return this.http.put(
      `${this.quizQuestionUpdatedUrl}/${quizData.id}`,
      quizData
    );
  }

  quizQuestionDeleteData(id: string): Observable<any> {
    return this.http.delete(`${this.quizQuestionDeletedUrl}/${id}`);
  }
}
