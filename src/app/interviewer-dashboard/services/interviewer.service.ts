import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, of } from 'rxjs';
import { map } from 'rxjs/operators';
import { endpoint } from '../../endpoints/endpoint';

export interface InterviewSearchParams {
  pageNumber?: number;
  pageSize?: number;
  interviewerId?: string;
  status?: 'Pending' | 'Accepted' | 'Declined';
}

export interface InterviewResponse {
  items: Interview[];
  pageNumber: number;
  pageSize: number;
  totalCount: number;
  totalPages: number;
  hasPrevious: boolean;
  hasNext: boolean;
}

export interface Interview {
  id: string;
  applicationId: string;
  interviewerId: string;
  interviewDate: string;
  status: string;
  notes: string;
  meetingId: string;
  candidate?: string;  // Will be populated from application data if available
  role?: string;       // Will be populated from application data if available
  company?: string;    // Will be populated from application data if available
}

export interface InterviewRequest {
  id: number | string;
  candidate: string;
  role: string;
  company: string;
  requestedDate: string;
  status: string;
}

export interface AvailabilitySlot {
  id?: number;
  startTime: string;
  endTime: string;
}

@Injectable({
  providedIn: 'root'
})
export class InterviewerService {

  constructor(private http: HttpClient) { }

  /**
   * Search for interviews with the given parameters
   * @param params Search parameters like status, interviewerId, etc.
   */
  searchInterviews(params: InterviewSearchParams): Observable<InterviewResponse> {
    const userId = this.getUserId();
    
    // Set interviewerId to userId if not provided
    if (!params.interviewerId && userId) {
      params.interviewerId = userId;
    }
    
    return this.http.post<InterviewResponse>(endpoint.interviewSearchUrl, params);
  }
  
  /**
   * Get pending interview requests for the current user
   */
  getPendingInterviews(): Observable<InterviewRequest[]> {
    const userId = this.getUserId();
    
    if (!userId) {
      // Return empty array if no user ID is found
      console.error('No user ID found in session');
      return of([]);
    }
    
    const params: InterviewSearchParams = {
      interviewerId: userId,
      status: 'Pending',
      pageNumber: 1,
      pageSize: 200
    };
    
    return this.searchInterviews(params).pipe(
      map(response => {
        // Transform the response to the format expected by the component
        return response.items.map(interview => {
          // Calculate requested date as interview date - 1 day
          const interviewDate = new Date(interview.interviewDate);
          interviewDate.setDate(interviewDate.getDate() - 1);
          
          return {
            id: interview.id,
            candidate: interview.candidate || `Candidate (${interview.applicationId.substring(0, 8)})`,
            role: interview.role || 'Position not specified',
            company: interview.company || 'Company not specified',
            requestedDate: interviewDate.toISOString().split('T')[0],
            status: interview.status.toLowerCase()
          };
        });
      })
    );
  }

  /* Placeholder method for searching availabilities */
  searchAvailabilities(): Observable<any> {
    return of({ items: [] });
  }
  
  /**
   * Gets the current user ID from session storage
   */
  private getUserId(): string | null {
    try {
      const userData = sessionStorage.getItem('userData') || sessionStorage.getItem('loggedInUser');
      if (userData) {
        const parsed = JSON.parse(userData);
        return parsed.userId || parsed.id || null;
      }
      return null;
    } catch (error) {
      console.error('Error retrieving user ID:', error);
      return null;
    }
  }
}
