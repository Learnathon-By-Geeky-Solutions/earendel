import { Injectable } from '@angular/core';
import { Observable, of, throwError } from 'rxjs';
import { delay } from 'rxjs/operators';
import { QuizAttempt, QuizQuestion, QuizSubmission } from './quiz.service';

@Injectable({
  providedIn: 'root'
})
export class MockQuizService {
  private questions: QuizQuestion[] = [
    {
      id: '1',
      questionText: 'What is the capital of France?',
      option1: 'London',
      option2: 'Paris',
      option3: 'Berlin',
      option4: 'Rome'
    },
    {
      id: '2',
      questionText: 'Which programming language is used for Angular development?',
      option1: 'Java',
      option2: 'C#',
      option3: 'TypeScript',
      option4: 'Python'
    },
    {
      id: '3',
      questionText: 'What does HTML stand for?',
      option1: 'Hyper Text Markup Language',
      option2: 'High Tech Multi Language',
      option3: 'Hyperlinks and Text Markup Language',
      option4: 'Home Tool Markup Language'
    }
  ];

  private mockAttemptId: string = '';
  private currentQuestionIndex: number = 0;

  constructor() {}

  startQuiz(): Observable<QuizAttempt> {
    // Check if user is logged in (simulate auth check)
    const userDataStr = sessionStorage.getItem('loggedInUser');
    if (!userDataStr) {
      return throwError(() => new Error('Not authenticated'));
    }
    
    // Generate a random attempt ID
    this.mockAttemptId = Math.random().toString(36).substring(2, 15);
    this.currentQuestionIndex = 0;
    
    // Return simulated successful response
    return of({ attemptId: this.mockAttemptId }).pipe(delay(800));
  }

  getQuizQuestion(attemptId: string): Observable<QuizQuestion | { message: string }> {
    // Check if attempt ID matches
    if (attemptId !== this.mockAttemptId) {
      return throwError(() => new Error('Invalid attempt ID'));
    }
    
    // Check if we've reached the end of questions
    if (this.currentQuestionIndex >= this.questions.length) {
      return of({ message: 'Finished The Quiz' }).pipe(delay(500));
    }
    
    // Return the current question
    return of(this.questions[this.currentQuestionIndex]).pipe(delay(700));
  }

  submitQuizAnswer(attemptId: string, submission: QuizSubmission): Observable<any> {
    // Check if attempt ID matches
    if (attemptId !== this.mockAttemptId) {
      return throwError(() => new Error('Invalid attempt ID'));
    }
    
    // Increment the question index
    this.currentQuestionIndex++;
    
    // Return simulated successful response
    return of({ success: true }).pipe(delay(500));
  }
} 