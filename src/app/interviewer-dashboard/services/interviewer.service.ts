import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable, of } from 'rxjs';
import { map, catchError } from 'rxjs/operators';
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
  id?: number | string;
  startTime: string;
  endTime: string;
}

export interface TimeSlot {
  id: number | string;
  start: string;
  end: string;
}

export interface AvailableDate {
  date: string;
  dayOfWeek: string;
  slots: TimeSlot[];
}

export interface AvailabilityResponse {
  items: AvailabilityItem[];
  pageNumber: number;
  pageSize: number;
  totalCount: number;
  totalPages: number;
  hasPrevious: boolean;
  hasNext: boolean;
}

export interface AvailabilityItem {
  id: string;
  interviewerId: string;
  startTime: string;
  endTime: string;
  isAvailable: boolean;
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
    
    return this.http.post<InterviewResponse>(endpoint.interviewSearchUrl, params)
      .pipe(
        catchError(error => {
          console.error('Error searching interviews:', error);
          return of({ items: [], pageNumber: 1, pageSize: 10, totalCount: 0, totalPages: 0, hasPrevious: false, hasNext: false });
        })
      );
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
  
  /**
   * Get available time slots for the interviewer
   * Groups time slots by date and returns them in the format expected by the component
   */
  getAvailableTimeSlots(): Observable<AvailableDate[]> {
    const userId = this.getUserId();
    
    if (!userId) {
      console.error('No user ID found in session');
      return of([]);
    }
    
    const request = {
      pageNumber: 1,
      pageSize: 1000,
      interviewerId: userId,
      isAvailable: true
    };
    
    const headers = this.getAuthHeaders();
    
    return this.http.post<AvailabilityResponse>(
      endpoint.interviewerAvailabilitySearchUrl, 
      request, 
      { headers }
    ).pipe(
      map(response => this.transformAvailabilityData(response.items)),
      catchError(error => {
        console.error('Error fetching availability slots:', error);
        return of([]);
      })
    );
  }
  
  /**
   * Transform availability data from API to the format expected by the component
   */
  private transformAvailabilityData(items: AvailabilityItem[]): AvailableDate[] {
    // Group slots by date
    const slotsByDate: Record<string, AvailabilityItem[]> = {};
    
    // Process all slots
    items.forEach(slot => {
      const startDate = new Date(slot.startTime);
      const dateKey = startDate.toISOString().split('T')[0]; // YYYY-MM-DD format
      
      if (!slotsByDate[dateKey]) {
        slotsByDate[dateKey] = [];
      }
      
      slotsByDate[dateKey].push(slot);
    });
    
    // Convert to array format
    return Object.keys(slotsByDate).map(dateKey => {
      const date = new Date(dateKey);
      const dayNames = ['Sunday', 'Monday', 'Tuesday', 'Wednesday', 'Thursday', 'Friday', 'Saturday'];
      const dayOfWeek = dayNames[date.getDay()];
      
      // Sort slots by start time
      const sortedSlots = slotsByDate[dateKey].sort((a, b) => {
        return new Date(a.startTime).getTime() - new Date(b.startTime).getTime();
      });
      
      // Transform to TimeSlot format
      const timeSlots = sortedSlots.map(slot => {
        const startTime = new Date(slot.startTime);
        const endTime = new Date(slot.endTime);
        
        return {
          id: slot.id,
          start: this.formatTimeForDisplay(startTime),
          end: this.formatTimeForDisplay(endTime)
        };
      });
      
      return {
        date: dateKey,
        dayOfWeek,
        slots: timeSlots
      };
    })
    // Sort dates in ascending order
    .sort((a, b) => a.date.localeCompare(b.date));
  }
  
  /**
   * Format a date to 24-hour time format (HH:MM)
   */
  private formatTimeForDisplay(date: Date): string {
    const hours = date.getHours().toString().padStart(2, '0');
    const minutes = date.getMinutes().toString().padStart(2, '0');
    return `${hours}:${minutes}`;
  }

  /**
   * Get authentication headers for API requests
   */
  private getAuthHeaders(): HttpHeaders {
    let headers = new HttpHeaders({
      'Content-Type': 'application/json'
    });
    
    try {
      const userData = sessionStorage.getItem('userData') || sessionStorage.getItem('loggedInUser');
      if (userData) {
        const parsed = JSON.parse(userData);
        if (parsed && parsed.token) {
          headers = headers.set('Authorization', `Bearer ${parsed.token}`);
        }
      }
    } catch (error) {
      console.error('Error setting auth headers:', error);
    }
    
    return headers;
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
  
  /* Placeholder to avoid errors */
  searchAvailabilities(): Observable<any> {
    return this.getAvailableTimeSlots().pipe(
      map(data => ({ items: data }))
    );
  }
}
