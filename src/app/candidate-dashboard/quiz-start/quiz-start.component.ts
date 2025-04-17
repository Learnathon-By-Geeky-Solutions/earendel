import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { QuizService } from '../services/quiz.service';
import { MockQuizService } from '../services/mock-quiz.service';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { environment } from '../../../environments/environment';

@Component({
  selector: 'app-quiz-start',
  standalone: true,
  imports: [CommonModule, MatSnackBarModule, MatProgressSpinnerModule],
  template: `
    <div class="quiz-start-container">
      <div class="quiz-notice">
        <h1>Important Notice: AI-Monitored Quiz</h1>

        <div class="warning-box">
          <i class="bi bi-exclamation-triangle"></i>
          <div class="warning-content">
            <strong>Your activity will be tracked by AI</strong>
            <p>
              This quiz uses advanced AI technology to monitor your actions and
              ensure the integrity of the assessment process.
            </p>
          </div>
        </div>

        <div class="rules-section">
          <h2>Please be aware of the following:</h2>
          <ul>
            <li>
              <i class="bi bi-camera-video"></i>
              Your video feed will be monitored throughout the quiz.
            </li>
            <li>
              <i class="bi bi-clipboard"></i>
              Your clipboard activity will be tracked to prevent copy-pasting.
            </li>
            <li>
              <i class="bi bi-window"></i>
              Opening new browser tabs or switching applications is not allowed.
            </li>
            <li>
              <i class="bi bi-clock"></i>
              Each question has a time limit. Once the time is up, you'll
              automatically move to the next question.
            </li>
            <li>
              <i class="bi bi-arrow-left"></i>
              You cannot return to previous questions once answered.
            </li>
          </ul>
        </div>

        <div class="warning-message">
          Any suspicious activity detected by our AI system may result in
          disqualification from the hiring process.
        </div>

        <div *ngIf="errorMessage" class="error-message">
          {{ errorMessage }}
        </div>

        <div class="consent-message">
          By clicking "Start Quiz", you acknowledge that you understand and
          agree to these conditions.
        </div>

        <button class="start-btn" (click)="startQuiz()" [disabled]="isStarting">
          <span *ngIf="isStarting" class="loading-spinner">
            <mat-spinner diameter="20"></mat-spinner>
          </span>
          {{ isStarting ? 'Starting...' : 'Start Quiz' }}
        </button>
        
        <div *ngIf="!isUserLoggedIn" class="login-warning">
          <p>You must be logged in to take the quiz. Please log in first.</p>
        </div>
        
        <div *ngIf="devMode" class="debug-info">
          <p>Using {{ useMockApi ? 'mock' : 'real' }} API for testing.</p>
          <div>
            <button class="debug-btn" (click)="createTestUser()">Create Test User</button>
            <button class="debug-btn" (click)="toggleApiMode()">
              {{ useMockApi ? 'Use Real API' : 'Use Mock API' }}
            </button>
          </div>
        </div>
      </div>
    </div>
  `,
  styles: [
    `
      .quiz-start-container {
        min-height: 100vh;
        padding: 24px;
        display: flex;
        align-items: center;
        justify-content: center;
        background: #f8f9fa;
      }

      .quiz-notice {
        background: white;
        border-radius: 12px;
        padding: 32px;
        max-width: 800px;
        width: 100%;
        box-shadow: 0 1px 3px rgba(0, 0, 0, 0.1);
      }

      h1 {
        font-size: 24px;
        font-weight: 600;
        margin: 0 0 24px;
        color: #333;
      }

      .warning-box {
        display: flex;
        gap: 16px;
        padding: 16px;
        background: #fff3e0;
        border-radius: 8px;
        margin-bottom: 24px;

        i {
          font-size: 24px;
          color: #f57c00;
        }

        .warning-content {
          strong {
            display: block;
            margin-bottom: 4px;
            color: #333;
          }

          p {
            margin: 0;
            color: #666;
            font-size: 14px;
          }
        }
      }

      .rules-section {
        margin-bottom: 24px;

        h2 {
          font-size: 18px;
          font-weight: 600;
          margin: 0 0 16px;
          color: #333;
        }

        ul {
          list-style: none;
          padding: 0;
          margin: 0;

          li {
            display: flex;
            align-items: center;
            gap: 12px;
            padding: 12px 0;
            color: #666;
            font-size: 14px;

            i {
              font-size: 18px;
              color: #999;
            }
          }
        }
      }

      .warning-message {
        padding: 16px;
        background: #feeeee;
        color: #dc3545;
        border-radius: 8px;
        margin-bottom: 24px;
        font-size: 14px;
      }
      
      .error-message {
        padding: 16px;
        background: #feeeee;
        color: #dc3545;
        border-radius: 8px;
        margin-bottom: 24px;
        font-size: 14px;
        font-weight: bold;
      }

      .consent-message {
        text-align: center;
        color: #666;
        font-size: 14px;
        margin-bottom: 24px;
      }

      .start-btn {
        display: flex;
        justify-content: center;
        align-items: center;
        width: 100%;
        padding: 16px;
        background: #0066ff;
        color: white;
        border: none;
        border-radius: 8px;
        font-size: 16px;
        font-weight: 500;
        cursor: pointer;
        transition: all 0.2s ease;

        &:hover:not(:disabled) {
          background: #0052cc;
        }

        &:disabled {
          background: #99c2ff;
          cursor: not-allowed;
        }
        
        .loading-spinner {
          margin-right: 8px;
        }
      }
      
      .login-warning {
        margin-top: 24px;
        text-align: center;
        padding: 16px;
        background: #fef8e0;
        color: #856404;
        border-radius: 8px;
        font-size: 14px;
      }
      
      .debug-info {
        margin-top: 24px;
        text-align: center;
        padding: 16px;
        background: #e6f4fa;
        color: #055160;
        border-radius: 8px;
        font-size: 14px;
        
        .debug-btn {
          margin: 8px 4px 0;
          padding: 8px 16px;
          background: #0dcaf0;
          color: white;
          border: none;
          border-radius: 4px;
          cursor: pointer;
          
          &:hover {
            background: #0ba8c1;
          }
        }
      }

      @media (max-width: 768px) {
        .quiz-start-container {
          padding: 16px;
        }

        .quiz-notice {
          padding: 24px;
        }

        h1 {
          font-size: 20px;
        }
      }
    `,
  ],
})
export class QuizStartComponent implements OnInit {
  isStarting = false;
  errorMessage = '';
  isUserLoggedIn = false;
  // Flag to use mock API for testing
  useMockApi = false; // Set to false by default to use real API
  // Flag to show debug UI in development 
  devMode = true; // Set to false in production

  constructor(
    private router: Router,
    private quizService: QuizService,
    private mockQuizService: MockQuizService,
    private snackBar: MatSnackBar
  ) {}
  
  ngOnInit() {
    // Check if user is logged in
    const userDataStr = sessionStorage.getItem('loggedInUser');
    this.isUserLoggedIn = !!userDataStr;
    
    if (userDataStr) {
      try {
        const userData = JSON.parse(userDataStr);
        console.log('User data found:', { 
          hasId: !!userData.id || !!userData.userId,
          hasToken: !!userData.token
        });
      } catch (error) {
        console.error('Error parsing user data:', error);
      }
    } else {
      console.warn('No user data found in session storage');
    }
  }

  startQuiz() {
    if (!this.isUserLoggedIn && !this.useMockApi) {
      this.errorMessage = 'You must be logged in to take the quiz. Please log in first.';
      this.snackBar.open('You must be logged in to take the quiz.', 'Close', {
        duration: 5000,
      });
      return;
    }
    
    this.isStarting = true;
    this.errorMessage = '';
    
    // Use mock service for testing if useMockApi is true
    const quizService = this.useMockApi ? this.mockQuizService : this.quizService;
    
    quizService.startQuiz().subscribe({
      next: (response) => {
        console.log('Quiz started successfully:', response);
        // Store attemptId in sessionStorage for future API calls
        sessionStorage.setItem('quizAttemptId', response.attemptId);
        // Store API mode in sessionStorage for quiz interface
        sessionStorage.setItem('useMockApi', this.useMockApi.toString());
        this.router.navigate(['/candidate-dashboard/quiz/interface']);
      },
      error: (error) => {
        console.error('Failed to start quiz:', error);
        this.isStarting = false;
        
        let errorMsg = 'Failed to start quiz. Please try again.';
        
        if (error.status === 401) {
          errorMsg = 'Authentication error. Please log out and log in again.';
        } else if (error.error && error.error.message) {
          errorMsg = error.error.message;
        }
        
        this.errorMessage = errorMsg;
        this.snackBar.open(errorMsg, 'Close', {
          duration: 5000,
        });
      }
    });
  }
  
  // Toggle between mock API and real API
  toggleApiMode() {
    this.useMockApi = !this.useMockApi;
    this.snackBar.open(`Now using ${this.useMockApi ? 'mock' : 'real'} API`, 'Close', {
      duration: 2000,
    });
  }
  
  // For testing purposes - creates a mock user in sessionStorage
  createTestUser() {
    const testUser = {
      id: '12345',
      userId: '12345',
      name: 'Test User',
      email: 'test@example.com',
      token: 'mock-jwt-token'
    };
    
    sessionStorage.setItem('loggedInUser', JSON.stringify(testUser));
    this.isUserLoggedIn = true;
    this.snackBar.open('Test user created successfully!', 'Close', {
      duration: 3000,
    });
  }
}
