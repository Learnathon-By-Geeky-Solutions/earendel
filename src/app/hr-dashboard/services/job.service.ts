import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, throwError, BehaviorSubject } from 'rxjs';
import { tap, catchError } from 'rxjs/operators';
import { endpoint } from '../../endpoints/endpoint';

export interface Skill { 
  id: string; 
  name: string; 
  description?: string;
  subSkills?: SubSkill[];
  seniorityLevelJunctions?: SeniorityLevelJunction[];
}

export interface SubSkill {
  id: string;
  name: string;
  description?: string;
  skillId: string;
}

export interface SeniorityLevel {
  id: string;
  name: string;
  description?: string;
}

export interface SeniorityLevelJunction {
  id: string;
  seniorityLevelId: string;
  skillId: string;
  seniority: SeniorityLevel;
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

export interface SkillSearchRequest {
  pageNumber: number;
  pageSize: number;
  searchTerm?: string;
  filters?: Record<string, any>;
}

@Injectable({ providedIn: 'root' })
export class JobService {
  private selectedSkillSubject = new BehaviorSubject<Skill | null>(null);
  private selectedSubSkillSubject = new BehaviorSubject<SubSkill | null>(null);
  private selectedSenioritySubject = new BehaviorSubject<SeniorityLevel | null>(null);

  constructor(private http: HttpClient) {}

  fetchSkills(pageNumber = 1, pageSize = 100): Observable<SkillSearchResponse> {
    const body: SkillSearchRequest = { pageNumber, pageSize };

    // Get authorization token if available
    const token = sessionStorage.getItem('token');
    const headers: Record<string, string> = {
      'Content-Type': 'application/json'
    };
    
    if (token) {
      headers['Authorization'] = `Bearer ${token}`;
    }

    return this.http
      .post<SkillSearchResponse>(endpoint.skillSearchUrl, body, { headers })
      .pipe(
        tap(response => console.log('[JobService] ← success response:', response)),
        catchError(err => {
          // Log detailed error information
          console.error('[JobService] HTTP error details:', {
            status: err.status,
            statusText: err.statusText,
            message: err.message,
            url: endpoint.skillSearchUrl,
            requestBody: body
          });
          
          // For client-side errors or network issues
          if (err.error instanceof ErrorEvent) {
            console.error('[JobService] Client-side error:', err.error.message);
          } 
          // For server-side errors
          else {
            console.error(`[JobService] Server error: ${err.status} ${err.statusText}`, err.error);
          }
          
          return throwError(() => err);
        })
      );
  }

  // Methods to manage selected skill
  setSelectedSkill(skill: Skill): void {
    console.log('[JobService] Setting selected skill:', skill);
    this.selectedSkillSubject.next(skill);
  }

  getSelectedSkill(): Skill | null {
    return this.selectedSkillSubject.getValue();
  }

  // Methods to manage selected subskill
  setSelectedSubSkill(subSkill: SubSkill): void {
    console.log('[JobService] Setting selected subskill:', subSkill);
    this.selectedSubSkillSubject.next(subSkill);
  }

  getSelectedSubSkill(): SubSkill | null {
    return this.selectedSubSkillSubject.getValue();
  }

  // Methods to manage selected seniority level
  setSelectedSeniority(seniority: SeniorityLevel): void {
    console.log('[JobService] Setting selected seniority level:', seniority);
    this.selectedSenioritySubject.next(seniority);
  }

  getSelectedSeniority(): SeniorityLevel | null {
    return this.selectedSenioritySubject.getValue();
  }

  // Clear all selections
  clearSelections(): void {
    this.selectedSkillSubject.next(null);
    this.selectedSubSkillSubject.next(null);
    this.selectedSenioritySubject.next(null);
  }
}
