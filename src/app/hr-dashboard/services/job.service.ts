import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { endpoint } from '../../endpoints/endpoint';

// Interfaces for API response
export interface SubSkill {
  id: string;
  name: string;
  description: string;
  skillId: string;
}

export interface Seniority {
  id: string;
  name: string;
  description: string;
}

export interface SeniorityLevelJunction {
  id: string;
  seniorityLevelId: string;
  skillId: string;
  seniority: Seniority;
}

export interface Skill {
  id: string;
  name: string;
  description: string;
  subSkills: SubSkill[];
  seniorityLevelJunctions: SeniorityLevelJunction[];
}

export interface SkillSearchResponse {
  items: Skill[];
  pageNumber: number;
  pageSize: number;
  totalCount: number;
  totalPages: number;
  hasPrevious: boolean;
  hasNext: boolean;
}

@Injectable({
  providedIn: 'root'
})
export class JobService {

  constructor(private http: HttpClient) {}

  fetchSkills(pageNumber: number = 1, pageSize: number = 100): Observable<SkillSearchResponse> {
    const body = { pageNumber, pageSize };
    return this.http.post<SkillSearchResponse>(endpoint.skillSearchUrl, body);
  }
}
