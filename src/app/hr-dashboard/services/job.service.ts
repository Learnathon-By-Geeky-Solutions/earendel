import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, throwError, BehaviorSubject, of } from 'rxjs';
import { tap, catchError, map } from 'rxjs/operators';
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

export interface Rubric {
  id: string;
  title: string;
  rubricDescription: string;
  subSkillId: string;
  seniorityId: string;
  weight: number;
}

export interface RubricSearchResponse {
  items: Rubric[];
  pageNumber: number;
  pageSize: number;
  totalCount: number;
  totalPages: number;
  hasPrevious: boolean;
  hasNext: boolean;
}

export interface RubricSearchRequest {
  pageNumber: number;
  pageSize: number;
  searchTerm?: string;
  filters?: Record<string, any>;
}

@Injectable({ providedIn: 'root' })
export class JobService {
  private readonly STORAGE_KEY_SKILL = 'selectedSkill';
  private readonly STORAGE_KEY_SUBSKILL = 'selectedSubSkill';
  private readonly STORAGE_KEY_SENIORITY = 'selectedSeniority';

  private selectedSkillSubject = new BehaviorSubject<Skill | null>(null);
  private selectedSubSkillSubject = new BehaviorSubject<SubSkill | null>(null);
  private selectedSenioritySubject = new BehaviorSubject<SeniorityLevel | null>(null);
  private rubricsSubject = new BehaviorSubject<Rubric[]>([]);

  constructor(private http: HttpClient) {
    this.loadFromStorage();
  }

  private loadFromStorage(): void {
    try {
      // Load selected skill from localStorage
      const storedSkill = localStorage.getItem(this.STORAGE_KEY_SKILL);
      if (storedSkill) {
        this.selectedSkillSubject.next(JSON.parse(storedSkill));
      }

      // Load selected subskill from localStorage
      const storedSubSkill = localStorage.getItem(this.STORAGE_KEY_SUBSKILL);
      if (storedSubSkill) {
        this.selectedSubSkillSubject.next(JSON.parse(storedSubSkill));
      }

      // Load selected seniority from localStorage
      const storedSeniority = localStorage.getItem(this.STORAGE_KEY_SENIORITY);
      if (storedSeniority) {
        this.selectedSenioritySubject.next(JSON.parse(storedSeniority));
      }
    } catch (error) {
      console.error('[JobService] Error loading data from localStorage:', error);
      // In case of error, clear storage to prevent future loading issues
      this.clearStoredSelections();
    }
  }

  private saveToStorage<T>(key: string, value: T | null): void {
    try {
      if (value) {
        localStorage.setItem(key, JSON.stringify(value));
      } else {
        localStorage.removeItem(key);
      }
    } catch (error) {
      console.error(`[JobService] Error saving to localStorage (${key}):`, error);
    }
  }

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

  fetchRubrics(pageNumber = 1, pageSize = 100): Observable<RubricSearchResponse> {
    const body: RubricSearchRequest = { pageNumber, pageSize };

    // Get authorization token if available
    const token = sessionStorage.getItem('token');
    const headers: Record<string, string> = {
      'Content-Type': 'application/json'
    };
    
    if (token) {
      headers['Authorization'] = `Bearer ${token}`;
    }

    return this.http
      .post<RubricSearchResponse>(endpoint.rubricsSearchUrl, body, { headers })
      .pipe(
        tap(response => console.log('[JobService] ← rubrics response:', response)),
        catchError(err => {
          // Log detailed error information
          console.error('[JobService] HTTP error details for rubrics:', {
            status: err.status,
            statusText: err.statusText,
            message: err.message,
            url: endpoint.rubricsSearchUrl,
            requestBody: body
          });
          
          return throwError(() => err);
        })
      );
  }

  getFilteredRubrics(): Observable<Rubric[]> {
    const selectedSubSkill = this.getSelectedSubSkill();
    const selectedSeniority = this.getSelectedSeniority();

    console.log('[JobService] Preparing to filter rubrics with:', { 
      selectedSubSkill, 
      selectedSeniority 
    });

    if (!selectedSubSkill || !selectedSeniority) {
      console.log('[JobService] Missing subskill or seniority selection for rubrics filtering');
      
      // For development/testing: if selections are missing, still fetch all rubrics
      // This is helpful during development to see if the API call works at all
      return this.fetchRubrics().pipe(
        map(response => {
          console.log('[JobService] Fetched all rubrics without filtering:', response.items);
          this.rubricsSubject.next(response.items);
          return response.items;
        }),
        catchError(error => {
          console.error('[JobService] Error fetching all rubrics:', error);
          return of([]);
        })
      );
    }

    // Removed sample data code as requested

    return this.fetchRubrics().pipe(
      map(response => {
        console.log('[JobService] All rubrics from API:', response.items);
        
        const filteredRubrics = response.items.filter(rubric => 
          rubric.subSkillId === selectedSubSkill.id && 
          rubric.seniorityId === selectedSeniority.id
        );
        
        console.log('[JobService] Filtered rubrics:', {
          total: response.items.length,
          filtered: filteredRubrics.length,
          subSkillId: selectedSubSkill.id,
          seniorityId: selectedSeniority.id
        });
        
        this.rubricsSubject.next(filteredRubrics);
        return filteredRubrics;
      }),
      catchError(error => {
        console.error('[JobService] Error filtering rubrics:', error);
        return of([]);
      })
    );
  }

  getRubrics(): Observable<Rubric[]> {
    return this.rubricsSubject.asObservable();
  }

  // Methods to manage selected skill
  setSelectedSkill(skill: Skill): void {
    console.log('[JobService] Setting selected skill:', skill);
    this.selectedSkillSubject.next(skill);
    this.saveToStorage(this.STORAGE_KEY_SKILL, skill);
  }

  getSelectedSkill(): Skill | null {
    return this.selectedSkillSubject.getValue();
  }

  // Methods to manage selected subskill
  setSelectedSubSkill(subSkill: SubSkill): void {
    console.log('[JobService] Setting selected subskill:', subSkill);
    this.selectedSubSkillSubject.next(subSkill);
    this.saveToStorage(this.STORAGE_KEY_SUBSKILL, subSkill);
  }

  getSelectedSubSkill(): SubSkill | null {
    return this.selectedSubSkillSubject.getValue();
  }

  // Methods to manage selected seniority level
  setSelectedSeniority(seniority: SeniorityLevel): void {
    console.log('[JobService] Setting selected seniority level:', seniority);
    this.selectedSenioritySubject.next(seniority);
    this.saveToStorage(this.STORAGE_KEY_SENIORITY, seniority);
  }

  getSelectedSeniority(): SeniorityLevel | null {
    return this.selectedSenioritySubject.getValue();
  }

  // Clear all selections from memory and storage
  clearSelections(): void {
    this.selectedSkillSubject.next(null);
    this.selectedSubSkillSubject.next(null);
    this.selectedSenioritySubject.next(null);
    this.rubricsSubject.next([]);
    this.clearStoredSelections();
  }

  // Clear just the storage values
  clearStoredSelections(): void {
    localStorage.removeItem(this.STORAGE_KEY_SKILL);
    localStorage.removeItem(this.STORAGE_KEY_SUBSKILL);
    localStorage.removeItem(this.STORAGE_KEY_SENIORITY);
  }

  // Helper method to determine if we're in development mode
  private isDevelopmentMode(): boolean {
    // We could check various conditions here to determine if we're in dev mode
    // For now, let's just return true to ensure sample data is always available
    return true;
  }

 
}
