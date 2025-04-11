import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable, catchError, throwError } from 'rxjs';
import { endpoint } from '../../endpoints/endpoint';

export interface AvailabilitySlot {
  startTime: string;
  endTime: string;
}

@Injectable({
  providedIn: 'root'
})
export class InterviewerService {
  constructor(private http: HttpClient) {}

  /**
   * Creates interviewer availability
   */
  createAvailability(slots: AvailabilitySlot[]): Observable<any> {
    const userData = this.getUserData();
    
    console.log('Creating availability with user ID:', userData.userId);
    console.log('Slots to save:', slots);
    
    const request = {
      interviewerId: userData.userId,
      availabilitySlots: slots
    };

    const headers = this.getAuthHeaders();
    
    console.log('Request data:', request);
    console.log('Request endpoint:', endpoint.interviewerAvailabilityCreateUrl);
    
    return this.http.post(endpoint.interviewerAvailabilityCreateUrl, request, { headers })
      .pipe(
        catchError(error => {
          console.error('API Error creating availability:', error);
          return throwError(() => error);
        })
      );
  }

  /**
   * Search for interviewer availabilities
   */
  searchAvailabilities(): Observable<any> {
    const userData = this.getUserData();
    
    console.log('Searching availabilities for user ID:', userData.userId);
    
    // Correctly format the request according to the API specification
    const request = {
      pageNumber: 1,
      pageSize: 50,
      orderBy: ["startTime"],
      interviewerId: userData.userId,
      isAvailable: true
    };

    const headers = this.getAuthHeaders();
    
    console.log('Search request:', request);
    console.log('Search endpoint:', endpoint.interviewerAvailabilitySearchUrl);
    
    return this.http.post(endpoint.interviewerAvailabilitySearchUrl, request, { headers })
      .pipe(
        catchError(error => {
          console.error('API Error searching availabilities:', error);
          return throwError(() => error);
        })
      );
  }

  /**
   * Get auth headers with token
   */
  private getAuthHeaders(): HttpHeaders {
    const userData = this.getUserData();
    let headers = new HttpHeaders({
      'Content-Type': 'application/json'
    });
    
    if (userData && userData.token) {
      console.log('Using token for auth:', userData.token.substring(0, 20) + '...');
      headers = headers.set('Authorization', `Bearer ${userData.token}`);
    } else {
      console.warn('No token found in userData. Looking for token directly in session storage.');
      
      // Try to get token directly from session storage as a fallback
      const loggedInUser = sessionStorage.getItem('loggedInUser');
      const userData = sessionStorage.getItem('userData');
      
      let token = null;
      
      if (loggedInUser) {
        try {
          const parsed = JSON.parse(loggedInUser);
          if (parsed && parsed.token) {
            token = parsed.token;
            console.log('Found token in loggedInUser:', token.substring(0, 20) + '...');
          }
        } catch (e) {
          console.error('Error parsing loggedInUser:', e);
        }
      }
      
      if (!token && userData) {
        try {
          const parsed = JSON.parse(userData);
          if (parsed && parsed.token) {
            token = parsed.token;
            console.log('Found token in userData:', token.substring(0, 20) + '...');
          }
        } catch (e) {
          console.error('Error parsing userData:', e);
        }
      }
      
      if (token) {
        headers = headers.set('Authorization', `Bearer ${token}`);
      } else {
        console.error('No authentication token found in any storage location.');
      }
    }
    
    // Print the final headers
    const authHeader = headers.get('Authorization');
    console.log('Authorization header set:', !!authHeader);
    console.log('Headers ready for API call:', headers.keys());
    
    return headers;
  }

  /**
   * Get user data from session storage
   */
  private getUserData(): any {
    const sessionData = sessionStorage.getItem('loggedInUser') || sessionStorage.getItem('userData');
    console.log('Session data found:', !!sessionData);
    
    if (sessionData) {
      try {
        const userData = JSON.parse(sessionData);
        console.log('User data parsed successfully, user ID:', userData.userId);
        return userData;
      } catch (e) {
        console.error('Error parsing user data:', e);
        console.log('Raw session data:', sessionData);
        return { userId: '' };
      }
    }
    
    console.warn('No user data found in session storage');
    return { userId: '' };
  }
} 