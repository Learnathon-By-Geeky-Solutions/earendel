import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, throwError } from 'rxjs';
import { tap, catchError } from 'rxjs/operators';
import { endpoint } from '../../endpoints/endpoint';

export interface Skill { 
  id: string; 
  name: string; 
  description?: string;
  subSkills?: any[];
  seniorityLevelJunctions?: any[];
}

export interface SkillSearchResponse { items: Skill[]; /* … */ }

@Injectable({ providedIn: 'root' })
export class JobService {
  constructor(private http: HttpClient) {}

  fetchSkills(pageNumber = 1, pageSize = 100): Observable<SkillSearchResponse> {
    const body = { pageNumber, pageSize };

    return this.http
      .post<SkillSearchResponse>(endpoint.skillSearchUrl, body)
      .pipe(
        // 2) Log the raw HTTP response
        tap(response => console.log('[JobService] ← success response:', response)),
        // 3) Log any error
        catchError(err => {
          console.error('[JobService] !! HTTP error:', err);
          return throwError(() => err);
        })
      );
  }
}
