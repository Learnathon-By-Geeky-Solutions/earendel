import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import  { Router, ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-seniority-selection',
  standalone: true,
  imports: [CommonModule],
  template: `
    <div class="container">
      <header class="header">
        <button class="back-btn" (click)="goBack()">
          <i class="bi bi-arrow-left"></i>
          Select seniority
        </button>
        <span class="step-indicator">Step 2 of 4</span>
      </header>

      <main class="main-content">
        <h2>Choose the experience level for the position</h2>

        <div class="seniority-grid">
          <div
            class="seniority-card"
            *ngFor="let level of seniorityLevels"
            (click)="selectLevel(level)"
            [class.selected]="level.id === selectedLevel?.id"
          >
            <div
              class="card-icon"
              [style.background-color]="level.progressColor + '20'"
            >
              <i [class]="level.icon"></i>
            </div>
            <div class="card-content">
              <h3>{{ level.title }}</h3>
              <p>{{ level.years }} yrs of experience</p>
            </div>
            <div
              class="progress-bar"
              [style.background-color]="level.progressColor"
            ></div>
            <div class="check-icon" *ngIf="level.id === selectedLevel?.id">
              <i class="bi bi-check-lg"></i>
            </div>
          </div>
        </div>

        <button
          class="continue-btn"
          [disabled]="!selectedLevel"
          (click)="continue()"
        >
          Continue with {{ selectedLevel?.title || 'selected' }} level
        </button>
      </main>
    </div>
  `,
  styles: [
    `
      .container {
        min-height: 100vh;
        background: #fff;
        padding: 2rem;
      }

      .header {
        display: flex;
        justify-content: space-between;
        align-items: center;
        margin-bottom: 2rem;
      }

      .back-btn {
        display: flex;
        align-items: center;
        gap: 0.5rem;
        background: none;
        border: none;
        font-size: 1rem;
        cursor: pointer;
        padding: 0;

        i {
          font-size: 1.25rem;
        }
      }

      .step-indicator {
        padding: 0.5rem 1rem;
        background: #e3f2fd;
        color: #0066cc;
        border-radius: 999px;
        font-size: 0.875rem;
      }

      .main-content {
        max-width: 800px;
        margin: 0 auto;

        h2 {
          font-size: 1.25rem;
          margin-bottom: 2rem;
          color: #666;
        }
      }

      .seniority-grid {
        display: grid;
        grid-template-columns: repeat(auto-fill, minmax(300px, 1fr));
        gap: 1rem;
        margin-bottom: 2rem;
      }

      .seniority-card {
        position: relative;
        padding: 1.5rem;
        border: 1px solid #eee;
        border-radius: 12px;
        cursor: pointer;
        transition: all 0.2s ease;
        display: flex;
        align-items: center;
        gap: 1rem;
        overflow: hidden;

        &:hover {
          border-color: #000;
        }

        &.selected {
          border-color: #0066cc;
        }

        .card-icon {
          width: 48px;
          height: 48px;
          border-radius: 8px;
          display: flex;
          align-items: center;
          justify-content: center;

          i {
            font-size: 1.5rem;
            color: #0066cc;
          }
        }

        .card-content {
          flex: 1;

          h3 {
            font-size: 1rem;
            margin: 0 0 0.25rem;
          }

          p {
            color: #666;
            font-size: 0.875rem;
            margin: 0;
          }
        }

        .progress-bar {
          position: absolute;
          bottom: 0;
          left: 0;
          height: 4px;
          width: 100%;
          opacity: 0.2;
        }

        .check-icon {
          width: 24px;
          height: 24px;
          background: #0066cc;
          border-radius: 50%;
          display: flex;
          align-items: center;
          justify-content: center;
          color: white;
        }
      }

      .continue-btn {
        width: 100%;
        padding: 1rem;
        background: #000;
        color: white;
        border: none;
        border-radius: 8px;
        font-size: 1rem;
        cursor: pointer;
        transition: background-color 0.2s ease;

        &:hover {
          background: #333;
        }

        &:disabled {
          background: #ccc;
          cursor: not-allowed;
        }
      }

      @media (max-width: 768px) {
        .container {
          padding: 1rem;
        }

        .seniority-grid {
          grid-template-columns: 1fr;
        }
      }
    `,
  ],
})
export class SenioritySelectionComponent {
  selectedLevel: any | null = null;

  seniorityLevels: any[] = [
    {
      id: 'internship',
      title: 'Internship',
      years: '0',
      icon: 'bi bi-mortarboard',
      progressColor: '#2196f3',
    },
    {
      id: 'entry',
      title: 'Entry Level',
      years: '0-1',
      icon: 'bi bi-star',
      progressColor: '#4caf50',
    },
    {
      id: 'intermediate',
      title: 'Intermediate',
      years: '1-3',
      icon: 'bi bi-graph-up',
      progressColor: '#ff9800',
    },
    {
      id: 'mid-senior',
      title: 'Mid-senior',
      years: '3-5',
      icon: 'bi bi-award',
      progressColor: '#e91e63',
    },
    {
      id: 'senior',
      title: 'Senior',
      years: '5-8',
      icon: 'bi bi-trophy',
      progressColor: '#9c27b0',
    },
    {
      id: 'senior-plus',
      title: 'Senior+',
      years: '8-10',
      icon: 'bi bi-stars',
      progressColor: '#673ab7',
    },
    {
      id: 'staff',
      title: 'Staff',
      years: '10+',
      icon: 'bi bi-bookmark-star',
      progressColor: '#3f51b5',
    },
  ];

  constructor(private router: Router, private route: ActivatedRoute) {}

  goBack() {
    const { domain, tech } = this.route.snapshot.params;
    this.router.navigate(['/hr-dashboard/customize', domain]);
  }

  selectLevel(level: any) {
    this.selectedLevel = level;
  }

  continue() {
    if (this.selectedLevel) {
      const { domain, tech } = this.route.snapshot.params;
      this.router.navigate(['/hr-dashboard/customized', domain, tech, this.selectedLevel.id]);
    }
  }
}
