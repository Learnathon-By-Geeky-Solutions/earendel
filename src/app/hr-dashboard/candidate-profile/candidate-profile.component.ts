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

interface Education {
  degree: string;
  university: string;
  year: string;
  score: string;
}

interface Experience {
  title: string;
  company: string;
  duration: string;
  description: string;
}

interface Assessment {
  type: string;
  score: number;
  maxScore: number;
  status: 'passed' | 'failed';
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
  ],
  template: `
    <div class="candidate-profile-modal">
      <div class="modal-header">
        <div class="candidate-info">
          <div class="avatar">{{ data.candidate.name.charAt(0) }}</div>
          <div class="info">
            <h2>{{ data.candidate.name }}</h2>
            <p>{{ data.candidate.email }}</p>
            <p>{{ data.candidate.phone }}</p>
          </div>
        </div>
        <div class="action-buttons">
          <button mat-raised-button color="primary" (click)="moveForward()">
            Move Forward
          </button>
          <button mat-raised-button color="warn" (click)="reject()">
            Reject
          </button>
          <button mat-icon-button (click)="dialogRef.close()">
            <i class="bi bi-x-lg"></i>
          </button>
        </div>
      </div>

      <div class="modal-body">
        <div class="skills-section">
          <h3>Skills</h3>
          <div class="skills-list">
            <span *ngFor="let skill of data.candidate.skills" class="skill-tag">
              {{ skill }}
            </span>
          </div>
        </div>

        <mat-tab-group mat-stretch-tabs="false" mat-align-tabs="start">
          <mat-tab label="Education">
            <div class="tab-content">
              <div
                *ngFor="let edu of data.candidate.education"
                class="education-item"
              >
                <h4>{{ edu.degree }}</h4>
                <p>{{ edu.university }}</p>
                <div class="education-details">
                  <span>Year: {{ edu.year }}</span>
                  <span>Score: {{ edu.score }}</span>
                </div>
              </div>
            </div>
          </mat-tab>

          <mat-tab label="Experience">
            <div class="tab-content">
              <div
                *ngFor="let exp of data.candidate.experience"
                class="experience-item"
              >
                <h4>{{ exp.title }}</h4>
                <p>{{ exp.company }}</p>
                <p class="duration">{{ exp.duration }}</p>
                <p>{{ exp.description }}</p>
              </div>
            </div>
          </mat-tab>

          <mat-tab label="Assessment">
            <div class="tab-content">
              <div
                *ngFor="let assessment of data.candidate.assessments"
                class="assessment-item"
              >
                <div class="assessment-header">
                  <h4>{{ assessment.type }}</h4>
                  <span
                    class="status-badge"
                    [class.passed]="assessment.status === 'passed'"
                    [class.failed]="assessment.status === 'failed'"
                  >
                    {{ assessment.status }}
                  </span>
                </div>
                <div class="progress-bar">
                  <div
                    class="progress"
                    [style.width.%]="
                      (assessment.score / assessment.maxScore) * 100
                    "
                  ></div>
                </div>
                <p class="score">
                  Score: {{ assessment.score }}/{{ assessment.maxScore }}
                </p>
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
}
