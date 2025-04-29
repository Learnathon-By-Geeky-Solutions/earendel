import { Component, Inject } from '@angular/core';
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



interface Assessment {
  type: string;
  score: number;
  maxScore: number;
  status: 'passed' | 'failed';
}

interface QuizAnswer {
  id: string;
  questionId: string;
  selectedOption: number;
  isCorrect: boolean;
  createdOn: string;
}

interface QuizResult {
  score: number;
  totalQuestions: number;
  answers?: QuizAnswer[];
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
    MatDividerModule
  ],
  template: `
    <div class="candidate-profile-modal">
      <div class="modal-header">
        <div class="candidate-info">
          <div class="avatar">
            {{ getInitials(data.name) }}
          </div>
          <div class="info">
            <h2>{{ data.name }}</h2>
            <p>{{ data.email }}</p>
          </div>
        </div>
        <div class="action-buttons">
          <button mat-raised-button color="primary" (click)="moveForward()">
            Move Forward
          </button>
          <button mat-stroked-button color="warn" (click)="reject()">
            Reject
          </button>
        </div>
      </div>

      <div class="modal-body">
        <div class="skills-section" *ngIf="data.skills && data.skills.length > 0">
          <h3>Skills</h3>
          <div class="skills-list">
            <span class="skill-tag" *ngFor="let skill of data.skills">{{ skill }}</span>
          </div>
        </div>

        <mat-tab-group>
          <mat-tab label="Cover Letter" *ngIf="data.coverLetter">
            <div class="tab-content">
              <div class="cover-letter-section">
                <h3>Cover Letter</h3>
                <div class="cover-letter-content">
                  <p>{{ data.coverLetter }}</p>
                </div>
              </div>
            </div>
          </mat-tab>


          <mat-tab label="Quiz Results" *ngIf="data.quizResult">
            <div class="tab-content">
              <div class="quiz-results-section">
                <div class="results-header">
                  <div class="header-content">
                    <h3>Quiz Results</h3>
                    <p class="date" *ngIf="data.quizResult.createdOn">Completed on {{ formatDate(data.quizResult.createdOn) }}</p>
                  </div>
                  <div class="score-badge" [ngClass]="getScoreBadgeClass(data.quizResult.score, data.quizResult.totalQuestions)">
                    {{ getScorePercentage(data.quizResult.score, data.quizResult.totalQuestions) }}%
                  </div>
                </div>
                
                <mat-divider></mat-divider>
                
                <div class="score-visualization">
                  <div class="score-chart">
                    <div class="score-circle" [style.background]="getScoreColor(data.quizResult.score, data.quizResult.totalQuestions)">
                      <span class="score-text">{{ data.quizResult.score }}</span>
                      <span class="total-text">/ {{ data.quizResult.totalQuestions }}</span>
                    </div>
                  </div>
                  
                  <div class="score-details">
                    <div class="detail-card correct">
                      <div class="detail-icon">
                        <mat-icon>check_circle</mat-icon>
                      </div>
                      <div class="detail-text">
                        <span class="detail-value">{{ data.quizResult.score }}</span>
                        <span class="detail-label">Correct</span>
                      </div>
                    </div>
                    
                    <div class="detail-card incorrect">
                      <div class="detail-icon">
                        <mat-icon>cancel</mat-icon>
                      </div>
                      <div class="detail-text">
                        <span class="detail-value">{{ data.quizResult.totalQuestions - data.quizResult.score }}</span>
                        <span class="detail-label">Incorrect</span>
                      </div>
                    </div>
                    
                    <div class="detail-card total">
                      <div class="detail-icon">
                        <mat-icon>assignment</mat-icon>
                      </div>
                      <div class="detail-text">
                        <span class="detail-value">{{ data.quizResult.totalQuestions }}</span>
                        <span class="detail-label">Total</span>
                      </div>
                    </div>
                  </div>
                </div>
                
                <mat-divider></mat-divider>
                
                <div *ngIf="data.quizResult.answers && data.quizResult.answers.length > 0" class="answers-section">
                  <h3>Question Breakdown</h3>
                  
                  <div class="answer-list">
                    <mat-card *ngFor="let answer of data.quizResult.answers; let i = index" class="answer-card" [ngClass]="{'correct': answer.isCorrect, 'incorrect': !answer.isCorrect}">
                      <div class="answer-header">
                        <span class="question-number">Question {{ i + 1 }}</span>
                        <mat-chip [color]="answer.isCorrect ? 'primary' : 'warn'" selected>
                          {{ answer.isCorrect ? 'Correct' : 'Incorrect' }}
                        </mat-chip>
                      </div>
                      
                      <div class="answer-content">
                        <div class="answer-detail">
                          <strong>Answer:</strong> Option {{ answer.selectedOption || 'Not answered' }}
                        </div>
                      </div>
                    </mat-card>
                  </div>
                </div>
              </div>
            </div>
          </mat-tab>
        </mat-tab-group>
      </div>
    </div>
  `,
  styles: [
    `
      .candidate-profile-modal {
        max-width: 800px;
        margin: 0 auto;
      }

      .modal-header {
        display: flex;
        justify-content: space-between;
        align-items: flex-start;
        padding: 24px;
        background-color: #f8f9fa;
        border-bottom: 1px solid #e9ecef;
      }

      .candidate-info {
        display: flex;
        align-items: center;
        gap: 16px;
      }

      .avatar {
        width: 64px;
        height: 64px;
        border-radius: 50%;
        background-color: #007bff;
        color: white;
        display: flex;
        align-items: center;
        justify-content: center;
        font-size: 24px;
        font-weight: bold;
      }

      .info h2 {
        margin: 0;
        font-size: 24px;
        font-weight: 600;
      }

      .info p {
        margin: 4px 0 0;
        color: #6c757d;
      }

      .action-buttons {
        display: flex;
        gap: 8px;
      }

      .modal-body {
        padding: 24px;
      }

      .skills-section {
        margin-bottom: 24px;
      }

      .skills-section h3 {
        font-size: 18px;
        font-weight: 600;
        margin-bottom: 12px;
      }

      .skills-list {
        display: flex;
        flex-wrap: wrap;
        gap: 8px;
      }

      .skill-tag {
        background-color: #e9ecef;
        color: #495057;
        padding: 4px 12px;
        border-radius: 16px;
        font-size: 14px;
      }

      ::ng-deep .mat-tab-labels {
        background-color: #f8f9fa;
      }

      ::ng-deep .mat-tab-label {
        height: 48px;
        padding: 0 24px;
        opacity: 1;
        color: #495057;
      }

      ::ng-deep .mat-tab-label-active {
        color: #007bff;
        font-weight: 600;
      }

      ::ng-deep .mat-ink-bar {
        background-color: #007bff !important;
      }

      .tab-content {
        padding: 24px 0;
      }

      .education-item,
      .experience-item,
      .assessment-item {
        margin-bottom: 24px;
      }

      .education-item h4,
      .experience-item h4 {
        font-size: 18px;
        font-weight: 600;
        margin: 0 0 8px;
      }

      .education-item p,
      .experience-item p {
        margin: 0 0 4px;
        color: #6c757d;
      }

      .education-details {
        display: flex;
        justify-content: space-between;
        color: #6c757d;
        font-size: 14px;
      }

      .experience-item .duration {
        font-style: italic;
        color: #6c757d;
      }

      .assessment-header {
        display: flex;
        justify-content: space-between;
        align-items: center;
        margin-bottom: 8px;
      }

      .assessment-header h4 {
        font-size: 18px;
        font-weight: 600;
        margin: 0;
      }

      .status-badge {
        padding: 4px 8px;
        border-radius: 4px;
        font-size: 14px;
        font-weight: 600;
      }

      .status-badge.passed {
        background-color: #d4edda;
        color: #155724;
      }

      .status-badge.failed {
        background-color: #f8d7da;
        color: #721c24;
      }

      .progress-bar {
        height: 8px;
        background-color: #e9ecef;
        border-radius: 4px;
        overflow: hidden;
        margin-bottom: 8px;
      }

      .progress {
        height: 100%;
        background-color: #007bff;
      }

      .score {
        font-size: 14px;
        color: #6c757d;
        margin: 0;
      }

      /* Cover Letter styles */
      .cover-letter-section h3 {
        font-size: 18px;
        font-weight: 600;
        margin-bottom: 16px;
      }

      .cover-letter-content {
        background-color: #f8f9fa;
        border-radius: 8px;
        padding: 16px;
        color: #495057;
        line-height: 1.6;
        white-space: pre-line;
      }

      /* Quiz Results styles */
      .quiz-results-section {
        padding: 0;
      }

      .results-header {
        display: flex;
        justify-content: space-between;
        align-items: center;
        margin-bottom: 24px;
      }
        
      .header-content h3 {
        font-size: 20px;
        font-weight: 600;
        margin: 0 0 8px;
        color: #2a3f5f;
      }
        
      .date {
        color: #6c757d;
        margin: 0;
        font-size: 14px;
      }
        
      .score-badge {
        display: flex;
        align-items: center;
        justify-content: center;
        width: 60px;
        height: 60px;
        border-radius: 50%;
        font-size: 18px;
        font-weight: 700;
        color: white;
      }
        
      .score-badge.excellent {
        background: #28a745;
      }
        
      .score-badge.good {
        background: #17a2b8;
      }
        
      .score-badge.average {
        background: #ffc107;
      }
        
      .score-badge.poor {
        background: #dc3545;
      }
      
      .score-visualization {
        display: flex;
        flex-direction: row;
        align-items: center;
        justify-content: space-between;
        margin: 24px 0;
      }
      
      .score-chart {
        display: flex;
        justify-content: center;
        flex: 1;
      }
      
      .score-circle {
        display: flex;
        flex-direction: column;
        align-items: center;
        justify-content: center;
        width: 120px;
        height: 120px;
        border-radius: 50%;
        color: white;
      }
      
      .score-text {
        font-size: 36px;
        font-weight: 700;
        line-height: 1;
        margin-bottom: 4px;
      }
      
      .total-text {
        font-size: 16px;
        opacity: 0.9;
      }
      
      .score-details {
        display: flex;
        flex-wrap: wrap;
        gap: 16px;
        justify-content: space-between;
        flex: 1;
      }
      
      .detail-card {
        display: flex;
        align-items: center;
        background: #f8f9fa;
        padding: 12px;
        border-radius: 8px;
        min-width: 100px;
      }
      
      .detail-icon {
        margin-right: 12px;
      }
      
      .detail-icon mat-icon {
        width: 24px;
        height: 24px;
        font-size: 24px;
      }
      
      .detail-text {
        display: flex;
        flex-direction: column;
      }
      
      .detail-value {
        font-size: 16px;
        font-weight: 600;
        color: #333;
      }
      
      .detail-label {
        font-size: 12px;
        color: #6c757d;
      }
      
      .detail-card.correct .detail-icon mat-icon {
        color: #28a745;
      }
      
      .detail-card.incorrect .detail-icon mat-icon {
        color: #dc3545;
      }
      
      .detail-card.total .detail-icon mat-icon {
        color: #17a2b8;
      }
      
      .answers-section {
        margin: 24px 0;
      }
      
      .answers-section h3 {
        font-size: 18px;
        font-weight: 500;
        margin: 0 0 16px;
        color: #2a3f5f;
      }
      
      .answer-list {
        display: grid;
        grid-template-columns: repeat(auto-fill, minmax(250px, 1fr));
        gap: 16px;
      }
      
      .answer-card {
        padding: 16px;
        border-left: 4px solid #ccc;
      }
      
      .answer-card.correct {
        border-left-color: #28a745;
      }
      
      .answer-card.incorrect {
        border-left-color: #dc3545;
      }
      
      .answer-header {
        display: flex;
        justify-content: space-between;
        align-items: center;
        margin-bottom: 12px;
      }
      
      .question-number {
        font-weight: 500;
        color: #2a3f5f;
      }
      
      .answer-content .answer-detail {
        margin-bottom: 8px;
        font-size: 14px;
      }
    `,
  ],
})
export class CandidateProfileModalComponent {
  constructor(
    public dialogRef: MatDialogRef<CandidateProfileModalComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any
  ) {}

  moveForward() {
    this.dialogRef.close({ action: 'move' });
  }

  reject() {
    this.dialogRef.close({ action: 'reject' });
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
      month: 'long',
      day: 'numeric',
      year: 'numeric',
    });
  }

  getScorePercentage(score: number, totalQuestions: number): number {
    if (!score || !totalQuestions) return 0;
    return Math.round((score / totalQuestions) * 100);
  }

  getScoreBadgeClass(score: number, totalQuestions: number): string {
    const percentage = this.getScorePercentage(score, totalQuestions);
    if (percentage >= 90) return 'excellent';
    if (percentage >= 75) return 'good';
    if (percentage >= 60) return 'average';
    return 'poor';
  }

  getScoreColor(score: number, totalQuestions: number): string {
    const percentage = this.getScorePercentage(score, totalQuestions);
    if (percentage >= 90) return '#28a745'; // Green
    if (percentage >= 75) return '#17a2b8'; // Blue
    if (percentage >= 60) return '#ffc107'; // Yellow
    return '#dc3545'; // Red
  }
}
