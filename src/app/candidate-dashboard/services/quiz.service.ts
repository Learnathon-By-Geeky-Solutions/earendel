import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { endpoint } from '../../endpoints/endpoint';

export interface QuizQuestion {
  id: string;
  questionText: string;
  option1: string;
  option2: string;
  option3: string;
  option4: string;
}

export interface QuizAttempt {
  attemptId: string;
}

export interface QuizSubmission {
  questionId: string;
  selectedOption: number;
}

@Injectable({
  providedIn: 'root'
})
export class QuizService {
  constructor(private http: HttpClient) {}

  // Start a new quiz attempt
  startQuiz(): Observable<QuizAttempt> {
    const headers = this.getAuthHeaders();
    const userDataStr = sessionStorage.getItem('loggedInUser');
    let userId = '';
    
    if (userDataStr) {
      try {
        const userData = JSON.parse(userDataStr);
        userId = userData.userId || userData.id || '';
      } catch (error) {
        console.error('Error parsing user data from session storage:', error);
      }
    }
    
    return this.http.post<QuizAttempt>(`${endpoint.quizStartUrl}`, { userId }, { headers });
  }

  // Fetch quiz question
  getQuizQuestion(attemptId: string): Observable<QuizQuestion | { message: string }> {
    const headers = this.getAuthHeaders();
    return this.http.get<QuizQuestion | { message: string }>(`${endpoint.quizQuestionUrl}/${attemptId}/question`, { headers });
  }

  // Submit quiz answer
  submitQuizAnswer(attemptId: string, submission: QuizSubmission): Observable<any> {
    const headers = this.getAuthHeaders();
    return this.http.post(`${endpoint.quizSubmitUrl}/${attemptId}/submit`, submission, { headers });
  }

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