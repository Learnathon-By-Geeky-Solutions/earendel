import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { endpoint } from '../../endpoints/endpoint';

@Injectable({
  providedIn: 'root'
})
export class CandidateService {
  constructor(private http: HttpClient) {}

  // Helper method to get auth headers
  private getAuthHeaders(): HttpHeaders {
    let headers = new HttpHeaders();
    const userDataStr = sessionStorage.getItem('loggedInUser');
    
    if (userDataStr) {
      try {
        const userData = JSON.parse(userDataStr);
        if (userData.token) {
          headers = headers.set('Authorization', `Bearer ${userData.token}`);
        }
      } catch (error) {
        console.error('Error parsing user data from session storage:', error);
      }
    }
    
    return headers;
  }
}
