import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';

interface SeniorityLevel {
  title: string;
  experience: string;
  icon: string;
  color: string;
  selected?: boolean;
  progressValue: number;
}

@Component({
  selector: 'app-seniority-select',
  standalone: true,
  imports: [CommonModule, RouterModule],
  template: `
    <div class="container-fluid py-4">
      <!-- Header -->
      <div class="d-flex align-items-center justify-content-between mb-5">
        <div class="d-flex align-items-center">
          <a routerLink=".." class="btn btn-link text-dark p-0 me-3">
            <i class="bi bi-arrow-left fs-4"></i>
          </a>
          <div>
            <h1 class="h4 mb-1 fw-bold">Select seniority</h1>
            <p class="text-muted mb-0">
              Choose the experience level for the position
            </p>
          </div>
        </div>
        <div class="progress-indicator">
          <span class="badge bg-primary-subtle text-primary-emphasis"
            >Step 2 of 4</span
          >
        </div>
      </div>

      <!-- Seniority Cards -->
      <div class="row g-4">
        <div class="col-md-6 col-lg-4" *ngFor="let level of seniorityLevels">
          <div
            class="seniority-card"
            [class.selected]="level.selected"
            (click)="selectLevel(level)"
          >
            <div class="card h-70 border-2 rounded-4 shadow-hover">
              <div class="card-body p-4">
                <div class="d-flex align-items-start mb-3">
                  <div
                    class="icon-wrapper rounded-3 me-3"
                    [style.backgroundColor]="level.color + '15'"
                    [style.color]="level.color"
                  >
                    <i [class]="'bi ' + level.icon"></i>
                  </div>
                  <div class="flex-grow-1">
                    <h2 class="h5 fw-bold mb-1">{{ level.title }}</h2>
                    <p class="text-muted mb-0">{{ level.experience }}</p>
                  </div>
                  <div class="selection-indicator">
                    <div class="circle" [class.active]="level.selected">
                      <i class="bi bi-check2"></i>
                    </div>
                  </div>
                </div>
                <div class="progress" style="height: 6px;">
                  <div
                    class="progress-bar"
                    [style.width.%]="level.progressValue"
                    [style.backgroundColor]="level.color"
                  ></div>
                </div>
              </div>
            </div>
          </div>
        </div>
      </div>

      <!-- Action Button -->
      <div
        class="position-fixed bottom-0 start-0 w-100 p-4 bg-white border-top"
      >
        <div class="container-fluid">
          <button
            class="btn btn-dark w-100 rounded-3 py-3"
            [disabled]="!getSelectedLevel()"
          >
            Continue with {{ getSelectedLevel()?.title || 'selected' }} level
          </button>
        </div>
      </div>
    </div>
  `,
  styles: [
    `
      :host {
        display: block;
        background-color: #fff;
        min-height: 100vh;
        padding-bottom: 100px;
      }

      .seniority-card {
        cursor: pointer;
        transition: all 0.3s ease;
      }

      .shadow-hover {
        transition: all 0.3s ease;
      }

      .seniority-card:hover .shadow-hover {
        transform: translateY(-4px);
        box-shadow: 0 10px 20px rgba(0, 0, 0, 0.1) !important;
      }

      .icon-wrapper {
        width: 48px;
        height: 48px;
        display: flex;
        align-items: center;
        justify-content: center;
        font-size: 1.5rem;
      }

      .selection-indicator .circle {
        width: 24px;
        height: 24px;
        border-radius: 50%;
        border: 2px solid #dee2e6;
        display: flex;
        align-items: center;
        justify-content: center;
        color: white;
        transition: all 0.2s ease;
      }

      .selection-indicator .circle.active {
        background-color: #000;
        border-color: #000;
      }

      .selection-indicator .circle i {
        opacity: 0;
        transform: scale(0);
        transition: all 0.2s ease;
      }

      .selection-indicator .circle.active i {
        opacity: 1;
        transform: scale(1);
      }

      .progress {
        background-color: #f8f9fa;
        border-radius: 10px;
      }

      .progress-bar {
        border-radius: 10px;
        transition: width 0.3s ease;
      }

      .selected .card {
        border: 2px solid #000 !important;
      }
    `,
  ],
})
export class SenioritySelectComponent {
  seniorityLevels: SeniorityLevel[] = [
    {
      title: 'Internship',
      experience: '0 yrs of experience',
      icon: 'bi-mortarboard',
      color: '#0ea5e9',
      progressValue: 15,
    },
    {
      title: 'Entry Level',
      experience: '0-1 yrs of experience',
      icon: 'bi-rocket',
      color: '#22c55e',
      progressValue: 30,
    },
    {
      title: 'Intermediate',
      experience: '1-3 yrs of experience',
      icon: 'bi-ladder',
      color: '#f59e0b',
      progressValue: 45,
    },
    {
      title: 'Mid-senior',
      experience: '3-5 yrs of experience',
      icon: 'bi-graph-up-arrow',
      color: '#ec4899',
      selected: true,
      progressValue: 60,
    },
    {
      title: 'Senior',
      experience: '5-8 yrs of experience',
      icon: 'bi-stars',
      color: '#8b5cf6',
      progressValue: 75,
    },
    {
      title: 'Senior+',
      experience: '8-10 yrs of experience',
      icon: 'bi-award',
      color: '#6366f1',
      progressValue: 90,
    },
    {
      title: 'Staff',
      experience: '10+ yrs of experience',
      icon: 'bi-trophy',
      color: '#475569',
      progressValue: 100,
    },
  ];

  selectLevel(level: SeniorityLevel) {
    this.seniorityLevels.forEach((l) => (l.selected = false));
    level.selected = true;
  }

  getSelectedLevel(): SeniorityLevel | undefined {
    return this.seniorityLevels.find((level) => level.selected);
  }
}
