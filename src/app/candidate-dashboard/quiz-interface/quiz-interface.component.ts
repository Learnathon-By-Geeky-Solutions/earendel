import {
  Component,
  HostListener,
  ChangeDetectorRef,
  OnInit,
  OnDestroy,
} from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { QuizService, QuizQuestion, QuizSubmission } from '../services/quiz.service';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';
import { MatProgressBarModule } from '@angular/material/progress-bar';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { environment } from '../../../environments/environment';

@Component({
  selector: 'app-quiz-interface',
  standalone: true,
  imports: [
    CommonModule, 
    FormsModule, 
    MatSnackBarModule, 
    MatProgressBarModule,
    MatProgressSpinnerModule
  ],
  template: `
    <div class="quiz-container">
      <!-- Only show header if not finished -->
      <div *ngIf="!finished" class="quiz-header">
        <h1>Candidate Quiz</h1>
        <div class="quiz-progress">
          <span>Question {{ currentQuestionIndex + 1 }}</span>
          <div class="timer">{{ formatTime(timeLeft) }}</div>
        </div>
        <div class="progress-bar">
          <div
            class="progress-fill"
            [style.width]="(timeLeft / questionTimeLimit) * 100 + '%'"
          ></div>
        </div>
      </div>

      <!-- Loading indicator -->
      <div *ngIf="loading && !finished" class="loading-container">
        <mat-spinner diameter="50"></mat-spinner>
        <p>Loading question...</p>
      </div>

      <!-- Question display - only shown if not finished and not loading -->
      <div *ngIf="!loading && !finished && currentQuestion" class="quiz-content">
        <div class="question">
          <h2>{{ currentQuestion.questionText }}</h2>
          <div class="options">
            <label
              *ngFor="let option of questionOptions; let i = index"
              class="option"
              [class.selected]="selectedOption === i + 1"
            >
              <input
                type="radio"
                [value]="i + 1"
                [(ngModel)]="selectedOption"
                name="answer"
              />
              <span class="radio-custom"></span>
              <span class="option-text">{{ option }}</span>
            </label>
          </div>
        </div>
      </div>

      <!-- Quiz completion message -->
      <div *ngIf="finished" class="finish-message">
        <h2>Quiz Completed!</h2>
        <p>Thank you for completing the quiz.</p>
        <button class="next-btn" (click)="navigateToResults()">
          View Results
        </button>
      </div>

      <!-- Next button for questions -->
      <div *ngIf="!loading && !finished && currentQuestion" class="quiz-footer">
        <button
          class="next-btn"
          (click)="nextQuestion()"
        >
          Next Question
        </button>
      </div>
    </div>
  `,
  styles: [
    `
      .quiz-container {
        max-width: 800px;
        margin: 0 auto;
        padding: 24px;
        user-select: none;
      }

      .quiz-header {
        margin-bottom: 32px;

        h1 {
          font-size: 24px;
          font-weight: 600;
          margin: 0 0 16px;
        }
      }

      .quiz-progress {
        display: flex;
        justify-content: space-between;
        align-items: center;
        margin-bottom: 8px;
        font-size: 14px;
        color: #666;
      }

      .timer {
        font-weight: 600;
        color: #333;
      }

      .progress-bar {
        height: 4px;
        background: #eee;
        border-radius: 2px;
        overflow: hidden;
      }

      .progress-fill {
        height: 100%;
        background: #0066ff;
        transition: width 0.3s ease;
      }

      .loading-container {
        display: flex;
        flex-direction: column;
        align-items: center;
        justify-content: center;
        padding: 40px;
        background: white;
        border-radius: 12px;
        margin-bottom: 24px;
        box-shadow: 0 1px 3px rgba(0, 0, 0, 0.1);
        
        p {
          margin-top: 16px;
          color: #666;
        }
      }

      .quiz-content {
        background: white;
        border-radius: 12px;
        padding: 24px;
        margin-bottom: 24px;
        box-shadow: 0 1px 3px rgba(0, 0, 0, 0.1);
      }

      .question {
        h2 {
          font-size: 18px;
          font-weight: 500;
          margin: 0 0 24px;
          color: #333;
        }
      }

      .options {
        display: flex;
        flex-direction: column;
        gap: 16px;
      }

      .option {
        display: flex;
        align-items: center;
        gap: 12px;
        padding: 16px;
        border: 1px solid #dee2e6;
        border-radius: 8px;
        cursor: pointer;
        transition: all 0.2s ease;

        &:hover {
          background: #f8f9fa;
        }

        &.selected {
          background: #f0f7ff;
          border-color: #0066ff;
        }

        input[type='radio'] {
          display: none;
        }
      }

      .radio-custom {
        width: 20px;
        height: 20px;
        border: 2px solid #dee2e6;
        border-radius: 50%;
        position: relative;

        &:after {
          content: '';
          position: absolute;
          top: 50%;
          left: 50%;
          transform: translate(-50%, -50%);
          width: 10px;
          height: 10px;
          background: #0066ff;
          border-radius: 50%;
          opacity: 0;
          transition: opacity 0.2s ease;
        }
      }

      input[type='radio']:checked + .radio-custom:after {
        opacity: 1;
      }

      .option-text {
        font-size: 16px;
        color: #333;
      }

      .finish-message {
        background: white;
        border-radius: 12px;
        padding: 32px;
        margin-bottom: 24px;
        box-shadow: 0 1px 3px rgba(0, 0, 0, 0.1);
        text-align: center;
        
        h2 {
          font-size: 24px;
          font-weight: 600;
          margin: 0 0 16px;
          color: #333;
        }
        
        p {
          margin: 0 0 24px;
          color: #666;
        }
      }

      .quiz-footer {
        display: flex;
        justify-content: flex-end;
      }

      .next-btn {
        padding: 12px 24px;
        background: #0066ff;
        color: white;
        border: none;
        border-radius: 8px;
        font-size: 16px;
        cursor: pointer;
        transition: all 0.2s ease;

        &:hover:not(:disabled) {
          background: #0052cc;
        }

        &:disabled {
          opacity: 0.5;
          cursor: not-allowed;
        }
      }

      @media (max-width: 768px) {
        .quiz-container {
          padding: 16px;
        }

        .quiz-content {
          padding: 16px;
        }

        .option {
          padding: 12px;
        }
      }
    `,
  ],
})
export class QuizInterfaceComponent implements OnInit, OnDestroy {
  // Flag to track loading state
  loading = true;
  // Flag to track if quiz is finished
  finished = false;
  // Store the current question
  currentQuestion: QuizQuestion | null = null;
  // Track the current question index (for UI display only)
  currentQuestionIndex = 0;
  // Selected option (1, 2, 3, 4)
  selectedOption = 0;
  // Time left for current question
  timeLeft = 30;
  // Default time limit per question
  questionTimeLimit = 30;
  // Timer reference for cleanup
  timer: any;
  // Store the quiz attempt ID from sessionStorage
  attemptId = '';

  // Computed property for question options
  get questionOptions(): string[] {
    if (!this.currentQuestion) return [];
    return [
      this.currentQuestion.option1,
      this.currentQuestion.option2,
      this.currentQuestion.option3,
      this.currentQuestion.option4
    ];
  }

  constructor(
    private router: Router,
    private quizService: QuizService,
    private cdr: ChangeDetectorRef,
    private snackBar: MatSnackBar
  ) {}

  // Anti-cheating: Detect tab/window switching
  @HostListener('window:blur', ['$event'])
  onWindowBlur() {
    // Delete attemptId from sessionStorage to invalidate the quiz attempt
    this.invalidateQuiz('You switched to another tab or window. The quiz has been terminated.');
  }

  // Anti-cheating: Prevent copy-paste
  @HostListener('copy', ['$event'])
  @HostListener('cut', ['$event'])
  @HostListener('paste', ['$event'])
  onCopy(e: Event) {
    e.preventDefault();
    this.snackBar.open('Copy-paste actions are not allowed during the quiz.', 'Close', {
      duration: 3000,
    });
  }

  ngOnInit() {
    // Get quiz attempt ID from sessionStorage
    this.attemptId = sessionStorage.getItem('quizAttemptId') || '';
    
    if (!this.attemptId) {
      // No attempt ID found, redirect to quiz dashboard
      this.snackBar.open('Quiz session not found. Please start a new quiz.', 'Close', {
        duration: 3000,
      });
      this.router.navigate(['/candidate-dashboard/quiz']);
      return;
    }
    
    // Fetch first question
    this.fetchQuestion();
  }

  ngOnDestroy() {
    this.clearTimer();
  }

  // Format time as MM:SS
  formatTime(seconds: number): string {
    const mins = Math.floor(seconds / 60);
    const secs = seconds % 60;
    return `${mins.toString().padStart(2, '0')}:${secs.toString().padStart(2, '0')}`;
  }

  // Start timer for current question
  startTimer() {
    this.clearTimer();
    this.timer = setInterval(() => {
      this.timeLeft--;
      this.cdr.detectChanges();
      
      if (this.timeLeft <= 0) {
        this.handleTimeUp();
      }
    }, 1000);
  }

  // Clear timer
  clearTimer() {
    if (this.timer) {
      clearInterval(this.timer);
      this.timer = null;
    }
  }

  // Handle when time is up
  handleTimeUp() {
    this.clearTimer();
    this.submitAnswer();
  }

  // Fetch next question from API
  fetchQuestion() {
    this.loading = true;
    this.selectedOption = 0;
    
    this.quizService.getQuizQuestion(this.attemptId).subscribe({
      next: (response) => {
        // Check if quiz is finished
        if ('message' in response && response.message === 'Finished The Quiz') {
          this.finished = true;
          this.loading = false;
          this.currentQuestion = null; // Clear current question when finished
          // Delete attemptId from sessionStorage
          sessionStorage.removeItem('quizAttemptId');
          return;
        }
        
        // Store current question
        this.currentQuestion = response as QuizQuestion;
        this.timeLeft = this.questionTimeLimit;
        this.loading = false;
        
        // Start timer
        this.startTimer();
      },
      error: (error) => {
        console.error('Error fetching question:', error);
        this.loading = false;
        this.snackBar.open('Failed to load question. Please try again.', 'Close', {
          duration: 3000,
        });
        
        // On error, redirect to quiz dashboard
        this.router.navigate(['/candidate-dashboard/quiz']);
      }
    });
  }

  // Submit answer and fetch next question
  submitAnswer() {
    if (!this.currentQuestion) return;
    
    const submission: QuizSubmission = {
      questionId: this.currentQuestion.id,
      selectedOption: this.selectedOption
    };
    
    this.quizService.submitQuizAnswer(this.attemptId, submission).subscribe({
      next: () => {
        // Increment question index before fetching next question
        this.currentQuestionIndex++;
        this.fetchQuestion();
      },
      error: (error) => {
        console.error('Error submitting answer:', error);
        this.snackBar.open('Failed to submit answer. Please try again.', 'Close', {
          duration: 3000,
        });
      }
    });
  }

  // Next question button handler
  nextQuestion() {
    this.clearTimer();
    this.submitAnswer();
  }

  // Navigate to results
  navigateToResults() {
    this.router.navigate(['/candidate-dashboard/quiz']);
  }

  // Invalidate quiz (for anti-cheating)
  private invalidateQuiz(message: string) {
    // Clear quiz attempt from sessionStorage
    sessionStorage.removeItem('quizAttemptId');
    
    // Show message
    this.snackBar.open(message, 'Close', {
      duration: 5000,
    });
    
    // Redirect to quiz dashboard
    setTimeout(() => {
      this.router.navigate(['/candidate-dashboard/quiz']);
    }, 1000);
  }
}
