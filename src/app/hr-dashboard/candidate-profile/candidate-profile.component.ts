import { Component, Inject, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import {
  MatDialogRef,
  MAT_DIALOG_DATA,
  MatDialogModule,
} from '@angular/material/dialog';
import { MatTabsModule } from '@angular/material/tabs';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatIconModule } from '@angular/material/icon';
import { MatChipsModule } from '@angular/material/chips';
import { MatProgressBarModule } from '@angular/material/progress-bar';
import { MatDividerModule } from '@angular/material/divider';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatBadgeModule } from '@angular/material/badge';

export interface QuizResult {
  score: number;
  totalQuestions: number;
  answers?: any[];
  createdOn?: string;
}

@Component({
  selector: 'app-candidate-profile-modal',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    MatDialogModule,
    MatTabsModule,
    MatButtonModule,
    MatCardModule,
    MatIconModule,
    MatChipsModule,
    MatProgressBarModule,
    MatDividerModule,
    MatProgressSpinnerModule,
    MatBadgeModule
  ],
  template: `
    <div class="modal-container">
      <div class="modal-header">
        <div class="candidate-avatar">
          <div *ngIf="data.avatarUrl; else initials">
            <img [src]="data.avatarUrl" alt="{{ data.name }}" />
          </div>
          <ng-template #initials>
            <div class="avatar-initials">{{ getInitials(data.name) }}</div>
          </ng-template>
        </div>
        <div class="candidate-details">
          <h2>{{ data.name }}</h2>
          <p class="email">{{ data.email }}</p>
          <p class="phone" *ngIf="data.phone">{{ data.phone }}</p>
          <div class="skills-container" *ngIf="data.skills && data.skills.length > 0">
            <span class="skill-tag" *ngFor="let skill of data.skills">{{ skill }}</span>
          </div>
        </div>
        <button mat-icon-button class="close-button" (click)="onClose()">
          <mat-icon>close</mat-icon>
        </button>
      </div>

      <mat-tab-group mat-stretch-tabs="false" mat-align-tabs="start">
        <mat-tab label="Cover Letter">
          <div class="tab-content">
            <div class="cover-letter" *ngIf="data.coverLetter">
              <p>{{ data.coverLetter }}</p>
            </div>
            <div class="no-data-message" *ngIf="!data.coverLetter">
              <p>No cover letter provided</p>
            </div>
          </div>
        </mat-tab>

        <mat-tab label="Quiz Results">
          <div class="tab-content">
            <!-- Loading state -->
            <div class="loading-container" *ngIf="!data.quizResult && !noQuizData">
              <mat-spinner diameter="40"></mat-spinner>
              <p>Loading quiz results...</p>
            </div>

            <!-- No data state -->
            <div class="no-data-message" *ngIf="noQuizData">
              <p>No quiz results available for this candidate</p>
            </div>

            <!-- Quiz results -->
            <div class="quiz-results" *ngIf="data.quizResult">
              <div class="results-summary">
                <div class="score-card">
                  <div class="score-circle" [style.background-color]="getScoreColor(data.quizResult.score, data.quizResult.totalQuestions)">
                    <div class="score-value">{{ getScorePercentage(data.quizResult.score, data.quizResult.totalQuestions) }}%</div>
                  </div>
                  <div class="score-text">
                    <strong>{{ data.quizResult.score }}</strong> out of {{ data.quizResult.totalQuestions }} correct
                  </div>
                  <!-- <div class="completed-date" *ngIf="data.quizResult.createdOn">
                    Completed {{ formatDate(data.quizResult.createdOn) }}
                  </div> -->
                </div>

                <div class="score-progress">
                  <mat-progress-bar 
                    mode="determinate" 
                    [value]="getScorePercentage(data.quizResult.score, data.quizResult.totalQuestions)"
                    [color]="getProgressBarColor(data.quizResult.score / data.quizResult.totalQuestions)">
                  </mat-progress-bar>
                </div>
              </div>

              <div class="quiz-answers" *ngIf="data.quizResult.answers && data.quizResult.answers.length > 0">
                <h3>Question Details</h3>
                <div class="answers-grid">
                  <div class="answer-item" *ngFor="let answer of data.quizResult.answers; let i = index">
                    <div class="answer-header" [class.correct]="answer.isCorrect" [class.incorrect]="!answer.isCorrect">
                      <span class="question-number">Q{{ i + 1 }}</span>
                      <mat-icon>{{ answer.isCorrect ? 'check_circle' : 'cancel' }}</mat-icon>
                    </div>
                    <div class="answer-body">
                      <p class="answer-text">Option {{ answer.selectedOption || 'Not answered' }}</p>
                    </div>
                  </div>
                </div>
              </div>
            </div>
          </div>
        </mat-tab>
      </mat-tab-group>

      <div class="modal-footer">
        <button mat-flat-button color="primary" (click)="moveForward()">Move Forward</button>
        <button mat-stroked-button color="warn" (click)="reject()">Reject</button>
      </div>
    </div>
  `,
  styles: [`
    .modal-container {
      display: flex;
      flex-direction: column;
      max-height: 90vh;
      width: 600px;
      overflow: hidden;
    }
    
    .modal-header {
      display: flex;
      padding: 24px;
      background-color: #f9f9fb;
      position: relative;
    }
    
    .candidate-avatar {
      width: 80px;
      height: 80px;
      border-radius: 50%;
      overflow: hidden;
      margin-right: 20px;
      background-color: #e0e0e0;
      flex-shrink: 0;
      border: 2px solid white;
      box-shadow: 0 2px 8px rgba(0,0,0,0.1);
    }
    
    .candidate-avatar img {
      width: 100%;
      height: 100%;
      object-fit: cover;
    }
    
    .avatar-initials {
      width: 100%;
      height: 100%;
      display: flex;
      align-items: center;
      justify-content: center;
      background-color: #3f51b5;
      color: white;
      font-size: 28px;
      font-weight: 500;
    }
    
    .candidate-details {
      flex: 1;
    }
    
    .candidate-details h2 {
      margin: 0 0 8px 0;
      font-size: 24px;
      font-weight: 500;
      color: #333;
    }
    
    .email, .phone {
      margin: 0 0 4px 0;
      color: #666;
      font-size: 14px;
      display: flex;
      align-items: center;
    }
    
    .skills-container {
      display: flex;
      flex-wrap: wrap;
      gap: 8px;
      margin-top: 12px;
    }
    
    .skill-tag {
      background-color: #f0f4ff;
      color: #3f51b5;
      padding: 4px 12px;
      border-radius: 16px;
      font-size: 12px;
      font-weight: 500;
    }
    
    .close-button {
      position: absolute;
      top: 12px;
      right: 12px;
    }
    
    .tab-content {
      padding: 24px;
      overflow-y: auto;
      max-height: 50vh;
    }
    
    .cover-letter {
      background-color: #f9f9fb;
      padding: 20px;
      border-radius: 8px;
      border-left: 3px solid #3f51b5;
      white-space: pre-line;
      line-height: 1.6;
    }
    
    .loading-container {
      display: flex;
      flex-direction: column;
      align-items: center;
      justify-content: center;
      padding: 40px 0;
    }
    
    .loading-container p {
      margin-top: 16px;
      color: #666;
    }
    
    .no-data-message {
      text-align: center;
      padding: 40px 0;
      color: #666;
      background-color: #f9f9fb;
      border-radius: 8px;
    }
    
    .quiz-results {
      padding: 0;
    }
    
    .results-summary {
      margin-bottom: 24px;
    }
    
    .score-card {
      display: flex;
      flex-direction: column;
      align-items: center;
      margin-bottom: 16px;
    }
    
    .score-circle {
      width: 100px;
      height: 100px;
      border-radius: 50%;
      display: flex;
      align-items: center;
      justify-content: center;
      margin-bottom: 16px;
    }
    
    .score-value {
      color: white;
      font-size: 24px;
      font-weight: 600;
    }
    
    .score-text {
      font-size: 16px;
      color: #333;
      margin-bottom: 8px;
    }
    
    .completed-date {
      font-size: 13px;
      color: #666;
    }
    
    .score-progress {
      margin-top: 16px;
    }
    
    .quiz-answers h3 {
      margin: 24px 0 16px;
      font-size: 18px;
      font-weight: 500;
      color: #333;
    }
    
    .answers-grid {
      display: grid;
      grid-template-columns: repeat(auto-fill, minmax(140px, 1fr));
      gap: 16px;
    }
    
    .answer-item {
      border-radius: 8px;
      overflow: hidden;
      box-shadow: 0 2px 4px rgba(0,0,0,0.05);
      background-color: white;
    }
    
    .answer-header {
      display: flex;
      justify-content: space-between;
      align-items: center;
      padding: 12px;
      background-color: #f1f1f1;
    }
    
    .answer-header.correct {
      background-color: rgba(76, 175, 80, 0.15);
    }
    
    .answer-header.incorrect {
      background-color: rgba(244, 67, 54, 0.15);
    }
    
    .answer-header mat-icon {
      font-size: 18px;
    }
    
    .answer-header.correct mat-icon {
      color: #4caf50;
    }
    
    .answer-header.incorrect mat-icon {
      color: #f44336;
    }
    
    .question-number {
      font-weight: 500;
      color: #333;
    }
    
    .answer-body {
      padding: 12px;
    }
    
    .answer-text {
      margin: 0;
      font-size: 14px;
    }
    
    .modal-footer {
      display: flex;
      justify-content: flex-end;
      gap: 12px;
      padding: 16px 24px;
      border-top: 1px solid #e0e0e0;
      background-color: #f9f9fb;
    }
  `]
})
export class CandidateProfileModalComponent implements OnInit {
  noQuizData = false;
  quizDataTimeout: any;

  constructor(
    public dialogRef: MatDialogRef<CandidateProfileModalComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any
  ) {}

  ngOnInit(): void {
    // Set a timeout to show "No quiz data" message if results don't load within 5 seconds
    this.quizDataTimeout = setTimeout(() => {
      if (!this.data.quizResult) {
        this.noQuizData = true;
      }
    }, 5000);
  }

  ngOnDestroy(): void {
    if (this.quizDataTimeout) {
      clearTimeout(this.quizDataTimeout);
    }
  }

  getInitials(name: string): string {
    if (!name) return '';
    return name
      .split(' ')
      .map(n => n[0])
      .join('')
      .toUpperCase()
      .substring(0, 2);
  }

  formatDate(dateString: string): string {
    if (!dateString) return '';
    const date = new Date(dateString);
    return date.toLocaleDateString('en-US', {
      month: 'short',
      day: 'numeric',
      year: 'numeric'
    });
  }

  getScorePercentage(score: number, totalQuestions: number): number {
    if (!score || !totalQuestions) return 0;
    return Math.round((score / totalQuestions) * 100);
  }

  getScoreColor(score: number, totalQuestions: number): string {
    const percentage = this.getScorePercentage(score, totalQuestions);
    if (percentage >= 90) return '#4caf50';
    if (percentage >= 75) return '#2196f3';
    if (percentage >= 60) return '#ff9800';
    return '#f44336';
  }

  getProgressBarColor(score: number): string {
    if (score >= 0.9) return 'primary';
    if (score >= 0.75) return 'accent';
    if (score >= 0.6) return 'warn';
    return 'warn';
  }

  moveForward(): void {
    this.dialogRef.close({ action: 'move' });
  }

  reject(): void {
    this.dialogRef.close({ action: 'reject' });
  }

  onClose(): void {
    this.dialogRef.close();
  }
}
