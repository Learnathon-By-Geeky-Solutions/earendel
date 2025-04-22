import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router, ActivatedRoute } from '@angular/router';
import { HttpClient, HttpHeaders, HttpClientModule } from '@angular/common/http';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';
import { MatIconModule } from '@angular/material/icon';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { MatDividerModule } from '@angular/material/divider';
import { MatProgressBarModule } from '@angular/material/progress-bar';
import { MatChipsModule } from '@angular/material/chips';
import { QuizService } from '../services/quiz.service';
import { catchError, finalize, map } from 'rxjs/operators';
import { of } from 'rxjs';
import { endpoint } from '../../endpoints/endpoint';

// Define interfaces based on API response
export interface QuizAnswer {
  id: string;
  questionId: string;
  selectedOption: number;
  isCorrect: boolean;
  createdOn: string;
}

export interface QuizAttemptHistory {
  id: string;
  userId: string;
  score: number;
  totalQuestions: number;
  createdOn: string;
  answers: QuizAnswer[];
}

interface QuizResult {
  score: number;
  totalQuestions: number;
  answers?: QuizAnswer[];
  createdOn?: string;
}

@Component({
  selector: 'app-quiz-results',
  standalone: true,
  imports: [
    CommonModule,
    HttpClientModule,
    MatProgressSpinnerModule,
    MatSnackBarModule,
    MatIconModule,
    MatCardModule,
    MatButtonModule,
    MatDividerModule,
    MatProgressBarModule,
    MatChipsModule
  ],
  template: `
    <div class="results-container">
      <!-- Loading State -->
      <div *ngIf="loading" class="loading-spinner">
        <mat-spinner diameter="50"></mat-spinner>
        <p>Loading your quiz results...</p>
      </div>
      
      <!-- Error State -->
      <div *ngIf="error" class="error-message">
        <mat-icon>error_outline</mat-icon>
        <h2>Something went wrong</h2>
        <p>{{ errorMessage }}</p>
        <button class="primary-btn" (click)="goToDashboard()">
          Return to Dashboard
        </button>
      </div>

      <!-- Results Content -->
      <div *ngIf="!loading && !error && (score !== null)" class="results-card">
        <!-- Header with Score Summary -->
        <div class="results-header">
          <div class="header-content">
            <h1>Quiz Results</h1>
            <p class="date" *ngIf="createdOn">Completed on {{ formatDate(createdOn) }}</p>
          </div>
          <div class="score-badge" [ngClass]="getScoreBadgeClass()">
            {{ getScorePercentage() }}%
          </div>
        </div>
        
        <mat-divider></mat-divider>
        
        <!-- Score Visualization -->
        <div class="score-visualization">
          <div class="score-chart">
            <div class="score-circle" [style.background]="getScoreColor()">
              <span class="score-text">{{ score }}</span>
              <span class="total-text">/ {{ totalQuestions }}</span>
            </div>
          </div>
          
          <div class="score-details">
            <div class="detail-card correct">
              <div class="detail-icon">
                <mat-icon>check_circle</mat-icon>
              </div>
              <div class="detail-text">
                <span class="detail-value">{{ score }}</span>
                <span class="detail-label">Correct</span>
              </div>
            </div>
            
            <div class="detail-card incorrect">
              <div class="detail-icon">
                <mat-icon>cancel</mat-icon>
              </div>
              <div class="detail-text">
                <span class="detail-value">{{ totalQuestions && score !== null ? totalQuestions - score : 0 }}</span>
                <span class="detail-label">Incorrect</span>
              </div>
            </div>
            
            <div class="detail-card total">
              <div class="detail-icon">
                <mat-icon>assignment</mat-icon>
              </div>
              <div class="detail-text">
                <span class="detail-value">{{ totalQuestions }}</span>
                <span class="detail-label">Total</span>
              </div>
            </div>
          </div>
        </div>
        
        <mat-divider></mat-divider>
        
        <!-- Question Details -->
        <div *ngIf="answers && answers.length > 0" class="answers-section">
          <h2>Question Breakdown</h2>
          
          <div class="answer-list">
            <mat-card *ngFor="let answer of answers; let i = index" class="answer-card" [ngClass]="{'correct': answer.isCorrect, 'incorrect': !answer.isCorrect}">
              <div class="answer-header">
                <span class="question-number">Question {{ i + 1 }}</span>
                <mat-chip [color]="answer.isCorrect ? 'primary' : 'warn'" selected>
                  {{ answer.isCorrect ? 'Correct' : 'Incorrect' }}
                </mat-chip>
              </div>
              
              <div class="answer-content">
                <div class="answer-detail">
                  <strong>Your answer:</strong> Option {{ answer.selectedOption || 'Not answered' }}
                </div>
                <div class="answer-time">
                  <mat-icon>schedule</mat-icon>
                  <span>{{ formatDate(answer.createdOn) }}</span>
                </div>
              </div>
            </mat-card>
          </div>
        </div>
        
        <!-- Performance Summary -->
        <div class="performance-summary">
          <h2>Performance Summary</h2>
          <mat-progress-bar 
            mode="determinate" 
            [value]="getScorePercentage()" 
            [color]="getScorePercentage() >= 70 ? 'primary' : 'warn'">
          </mat-progress-bar>
          <p class="summary-text">
            {{ getPerformanceSummary() }}
          </p>
        </div>
        
        <!-- Monitoring Notice -->
        <div class="monitoring-notice">
          <mat-icon>security</mat-icon>
          <div>
            <strong>AI Monitoring Reminder</strong>
            <p>
              Please note that this exam was monitored by our AI system. Any
              detected irregularities or attempts at cheating may result in
              disqualification. We appreciate your honest participation.
            </p>
          </div>
        </div>

        <!-- Action Buttons -->
        <div class="action-buttons">
          <button mat-flat-button color="primary" (click)="retakeQuiz()">
            Retake Quiz
          </button>
          <button mat-stroked-button (click)="goToDashboard()">
            Back to Dashboard
          </button>
        </div>
      </div>
    </div>
  `,
  styles: [
    `
      .results-container {
        min-height: 100vh;
        padding: 32px;
        display: flex;
        align-items: center;
        justify-content: center;
        background: #f5f7fa;
        font-family: 'Roboto', sans-serif;
      }
      
      .loading-spinner {
        display: flex;
        flex-direction: column;
        align-items: center;
        justify-content: center;
        padding: 48px;
        background: white;
        border-radius: 12px;
        box-shadow: 0 8px 16px rgba(0, 0, 0, 0.05);
        text-align: center;
        
        p {
          margin-top: 24px;
          color: #666;
          font-size: 16px;
        }
      }
      
      .error-message {
        display: flex;
        flex-direction: column;
        align-items: center;
        justify-content: center;
        padding: 48px;
        background: white;
        border-radius: 12px;
        box-shadow: 0 8px 16px rgba(0, 0, 0, 0.05);
        text-align: center;
        
        mat-icon {
          font-size: 64px;
          width: 64px;
          height: 64px;
          color: #f44336;
          margin-bottom: 16px;
        }
        
        h2 {
          font-size: 24px;
          margin-bottom: 16px;
          color: #333;
        }
        
        p {
          margin-bottom: 32px;
          color: #666;
          font-size: 16px;
          max-width: 400px;
        }
      }

      .results-card {
        background: white;
        border-radius: 12px;
        padding: 32px;
        max-width: 800px;
        width: 100%;
        box-shadow: 0 8px 16px rgba(0, 0, 0, 0.05);
      }
      
      .results-header {
        display: flex;
        justify-content: space-between;
        align-items: center;
        margin-bottom: 24px;
        
        .header-content {
          h1 {
            font-size: 28px;
            font-weight: 600;
            margin: 0 0 8px;
            color: #2a3f5f;
          }
          
          .date {
            color: #6c757d;
            margin: 0;
            font-size: 14px;
          }
        }
        
        .score-badge {
          display: flex;
          align-items: center;
          justify-content: center;
          width: 70px;
          height: 70px;
          border-radius: 50%;
          font-size: 20px;
          font-weight: 700;
          color: white;
          
          &.excellent {
            background: #28a745;
          }
          
          &.good {
            background: #17a2b8;
          }
          
          &.average {
            background: #ffc107;
          }
          
          &.poor {
            background: #dc3545;
          }
        }
      }
      
      mat-divider {
        margin: 24px 0;
      }
      
      .score-visualization {
        display: flex;
        flex-direction: column;
        align-items: center;
        margin: 32px 0;
        
        @media (min-width: 768px) {
          flex-direction: row;
          justify-content: space-between;
        }
        
        .score-chart {
          display: flex;
          justify-content: center;
          margin-bottom: 24px;
          
          @media (min-width: 768px) {
            margin-bottom: 0;
            flex: 1;
          }
          
          .score-circle {
            display: flex;
            flex-direction: column;
            align-items: center;
            justify-content: center;
            width: 150px;
            height: 150px;
            border-radius: 50%;
            color: white;
            
            .score-text {
              font-size: 48px;
              font-weight: 700;
              line-height: 1;
              margin-bottom: 4px;
            }
            
            .total-text {
              font-size: 18px;
              opacity: 0.9;
            }
          }
        }
        
        .score-details {
          display: flex;
          flex-wrap: wrap;
          gap: 16px;
          justify-content: center;
          
          @media (min-width: 768px) {
            flex: 1;
            justify-content: space-between;
          }
          
          .detail-card {
            display: flex;
            align-items: center;
            background: #f8f9fa;
            padding: 12px;
            border-radius: 8px;
            min-width: 120px;
            
            .detail-icon {
              margin-right: 12px;
              
              mat-icon {
                width: 28px;
                height: 28px;
                font-size: 28px;
              }
            }
            
            .detail-text {
              display: flex;
              flex-direction: column;
              
              .detail-value {
                font-size: 18px;
                font-weight: 600;
                color: #333;
              }
              
              .detail-label {
                font-size: 12px;
                color: #6c757d;
              }
            }
            
            &.correct {
              .detail-icon mat-icon {
                color: #28a745;
              }
            }
            
            &.incorrect {
              .detail-icon mat-icon {
                color: #dc3545;
              }
            }
            
            &.total {
              .detail-icon mat-icon {
                color: #17a2b8;
              }
            }
          }
        }
      }
      
      .answers-section {
        margin-bottom: 32px;
        
        h2 {
          font-size: 20px;
          font-weight: 500;
          margin: 0 0 16px;
          color: #2a3f5f;
        }
        
        .answer-list {
          display: grid;
          grid-template-columns: repeat(auto-fill, minmax(300px, 1fr));
          gap: 16px;
        }
        
        .answer-card {
          padding: 16px;
          border-left: 4px solid #ccc;
          
          &.correct {
            border-left-color: #28a745;
          }
          
          &.incorrect {
            border-left-color: #dc3545;
          }
          
          .answer-header {
            display: flex;
            justify-content: space-between;
            align-items: center;
            margin-bottom: 12px;
            
            .question-number {
              font-weight: 500;
              color: #2a3f5f;
            }
          }
          
          .answer-content {
            .answer-detail {
              margin-bottom: 8px;
              font-size: 14px;
            }
            
            .answer-time {
              display: flex;
              align-items: center;
              color: #6c757d;
              font-size: 12px;
              
              mat-icon {
                font-size: 14px;
                width: 14px;
                height: 14px;
                margin-right: 4px;
              }
            }
          }
        }
      }
      
      .performance-summary {
        margin-bottom: 32px;
        
        h2 {
          font-size: 20px;
          font-weight: 500;
          margin: 0 0 16px;
          color: #2a3f5f;
        }
        
        mat-progress-bar {
          height: 8px;
          border-radius: 4px;
          margin-bottom: 16px;
        }
        
        .summary-text {
          color: #6c757d;
          margin: 0;
          font-size: 14px;
          line-height: 1.5;
        }
      }
      
      .monitoring-notice {
        display: flex;
        gap: 16px;
        padding: 16px;
        background: #e9ecef;
        border-radius: 8px;
        margin-bottom: 32px;

        mat-icon {
          color: #17a2b8;
        }

        strong {
          display: block;
          margin-bottom: 4px;
          color: #333;
          font-size: 14px;
        }

        p {
          margin: 0;
          font-size: 13px;
          color: #6c757d;
          line-height: 1.5;
        }
      }

      .action-buttons {
        display: flex;
        gap: 16px;
        
        button {
          flex: 1;
          padding: 12px;
        }
      }

      @media (max-width: 600px) {
        .results-container {
          padding: 16px;
        }

        .results-card {
          padding: 16px;
        }
        
        .results-header {
          flex-direction: column;
          align-items: flex-start;
          
          .score-badge {
            margin-top: 16px;
            align-self: center;
          }
        }
        
        .action-buttons {
          flex-direction: column;
        }
      }
    `,
  ],
})
export class QuizResultsComponent implements OnInit {
  quizAttempt: QuizAttemptHistory | null = null;
  loading = true;
  error = false;
  errorMessage = 'Failed to load quiz results. Please try again.';
  attemptId: string | null = null;
  score: number | null = null;
  totalQuestions: number | null = null;
  answers: QuizAnswer[] = [];
  createdOn: string | undefined;

  constructor(
    private router: Router,
    private route: ActivatedRoute,
    private quizService: QuizService,
    private http: HttpClient,
    private snackBar: MatSnackBar
  ) {}

  ngOnInit() {
    // Get attemptId from route params
    this.route.paramMap.subscribe(params => {
      this.attemptId = params.get('attemptId');
      
      if(this.attemptId) {
        this.loadQuizResults(this.attemptId);
      } else {
        this.error = true;
        this.errorMessage = 'No quiz attempt ID provided. Please start a new quiz.';
        this.loading = false;
      }
    });
  }

  loadQuizResults(attemptId: string) {
    this.loading = true;
    this.error = false;

    const userDataStr = sessionStorage.getItem('loggedInUser');
    if (!userDataStr) {
      this.error = true;
      this.errorMessage = 'User not logged in. Please log in to view quiz results.';
      this.loading = false;
      return;
    }

    let userId = '', token = '';
    try {
      const userData = JSON.parse(userDataStr);
      userId = userData.userId || userData.id || '';
      token = userData.token || '';
    } catch {
      this.error = true;
      this.errorMessage = 'Could not parse user data. Please log in again.';
      this.loading = false;
      return;
    }

    this.quizService
      .getQuizAttempts(userId, 1, 50)
      .pipe(
        map((attempts: QuizAttemptHistory[]) => {
          console.log('All attempts:', attempts);
          const attempt = attempts.find(a => a.id === attemptId);
          if (!attempt) {
            throw new Error('Quiz attempt not found');
          }
          return attempt;
        }),
        catchError(err => {
          console.error(err);
          this.error = true;
          this.errorMessage = 'Unable to load quiz result.';
          return of(null);
        }),
        finalize(() => (this.loading = false))
      )
      .subscribe((result: QuizAttemptHistory | null) => {
        if (result) {
          console.log('Quiz result:', result);
          this.score = result.score;
          this.totalQuestions = result.totalQuestions;
          this.answers = result.answers || [];
          this.createdOn = result.createdOn;
          
          // Sort answers by creation time
          if (this.answers && this.answers.length > 0) {
            this.answers.sort((a, b) => {
              return new Date(a.createdOn).getTime() - new Date(b.createdOn).getTime();
            });
          }
        }
      });
  }

  getUserName(): string {
    try {
      const userDataStr = sessionStorage.getItem('loggedInUser');
      if (userDataStr) {
        const userData = JSON.parse(userDataStr);
        return userData.name || 'Candidate';
      }
    } catch (error) {
      console.error('Error parsing user data:', error);
    }
    return 'Candidate';
  }

  getScorePercentage(): number {
    if (this.score === null || this.totalQuestions === null || this.totalQuestions === 0) {
      return 0;
    }
    return Math.round((this.score / this.totalQuestions) * 100);
  }
  
  getScoreBadgeClass(): string {
    const percentage = this.getScorePercentage();
    if (percentage >= 90) return 'excellent';
    if (percentage >= 70) return 'good';
    if (percentage >= 50) return 'average';
    return 'poor';
  }
  
  getScoreColor(): string {
    const percentage = this.getScorePercentage();
    if (percentage >= 90) return '#28a745';  // Excellent - Green
    if (percentage >= 70) return '#17a2b8';  // Good - Blue
    if (percentage >= 50) return '#ffc107';  // Average - Yellow
    return '#dc3545';  // Poor - Red
  }
  
  getPerformanceSummary(): string {
    const percentage = this.getScorePercentage();
    
    if (percentage >= 90) {
      return 'Excellent performance! You have demonstrated a strong understanding of the subject matter.';
    } else if (percentage >= 70) {
      return 'Good job! You have shown a solid grasp of most concepts in this quiz.';
    } else if (percentage >= 50) {
      return 'You have passed the quiz, but there is room for improvement in some areas.';
    } else {
      return 'You might need to review the material and try again. Do not give up!';
    }
  }
  
  formatDate(dateString?: string): string {
    if (!dateString || dateString === '0001-01-01T00:00:00+00:00') {
      return 'Date unavailable';
    }
    
    try {
      const date = new Date(dateString);
      return new Intl.DateTimeFormat('en-US', {
        year: 'numeric',
        month: 'short',
        day: 'numeric',
        hour: '2-digit',
        minute: '2-digit'
      }).format(date);
    } catch (error) {
      console.error('Error formatting date:', error);
      return 'Invalid date';
    }
  }

  retakeQuiz() {
    this.router.navigate(['/candidate-dashboard/quiz/start']);
  }

  goToDashboard() {
    this.router.navigate(['/candidate-dashboard/quiz']);
  }
}
