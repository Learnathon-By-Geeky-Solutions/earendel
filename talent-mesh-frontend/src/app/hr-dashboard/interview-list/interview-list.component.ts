import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';

interface RecentResult {
  initials: string;
  name: string;
  role: string;
  score: string;
  date: string;
  status: 'Passed' | 'Failed';
}

@Component({
  selector: 'app-interview-list',
  standalone: true,
  imports: [CommonModule],
  template: `
    <div class="card border-0 shadow-sm">
      <div class="card-body p-0">
        <ul class="nav nav-tabs border-0 px-4">
          <li class="nav-item">
            <a
              class="nav-link"
              [class.active]="activeTab === 'upcoming'"
              (click)="setTab('upcoming')"
              href="javascript:void(0)"
            >
              Upcoming Interviews
            </a>
          </li>
          <li class="nav-item">
            <a
              class="nav-link"
              [class.active]="activeTab === 'recent'"
              (click)="setTab('recent')"
              href="javascript:void(0)"
            >
              Recent Results
            </a>
          </li>
        </ul>

        <div class="interview-list p-4">
          <!-- Upcoming Interviews Tab -->
          <div *ngIf="activeTab === 'upcoming'">
            <div
              class="interview-item d-flex align-items-center justify-content-between py-3"
            >
              <div class="d-flex align-items-center gap-3">
                <div
                  class="avatar rounded-circle bg-light d-flex align-items-center justify-content-center"
                >
                  JD
                </div>
                <div>
                  <h6 class="mb-1">John Doe</h6>
                  <small class="text-muted">Senior Frontend Developer</small>
                </div>
              </div>
              <div class="d-flex align-items-center gap-3">
                <div>
                  <h6 class="mb-1">Mike Johnson</h6>
                  <small class="text-muted">Interviewer</small>
                </div>
                <div class="text-end">
                  <h6 class="mb-1">Today, 2:00 PM</h6>
                  <span class="badge bg-dark">Scheduled</span>
                </div>
              </div>
            </div>

            <div
              class="interview-item d-flex align-items-center justify-content-between py-3"
            >
              <div class="d-flex align-items-center gap-3">
                <div
                  class="avatar rounded-circle bg-light d-flex align-items-center justify-content-center"
                >
                  AS
                </div>
                <div>
                  <h6 class="mb-1">Alice Smith</h6>
                  <small class="text-muted">Junior Backend Engineer</small>
                </div>
              </div>
              <div class="d-flex align-items-center gap-3">
                <div>
                  <h6 class="mb-1">Sarah Chen</h6>
                  <small class="text-muted">Interviewer</small>
                </div>
                <div class="text-end">
                  <h6 class="mb-1">Tomorrow, 10:00 AM</h6>
                  <span class="badge bg-success">Confirmed</span>
                </div>
              </div>
            </div>

            <div
              class="interview-item d-flex align-items-center justify-content-between py-3"
            >
              <div class="d-flex align-items-center gap-3">
                <div
                  class="avatar rounded-circle bg-light d-flex align-items-center justify-content-center"
                >
                  RW
                </div>
                <div>
                  <h6 class="mb-1">Robert Wilson</h6>
                  <small class="text-muted">Full Stack Developer</small>
                </div>
              </div>
              <div class="d-flex align-items-center gap-3">
                <div>
                  <h6 class="mb-1">David Lee</h6>
                  <small class="text-muted">Interviewer</small>
                </div>
                <div class="text-end">
                  <h6 class="mb-1">Jan 22, 3:30 PM</h6>
                  <span class="badge bg-secondary">Pending</span>
                </div>
              </div>
            </div>
          </div>

          <!-- Recent Results Tab -->
          <div *ngIf="activeTab === 'recent'">
            <div
              *ngFor="let result of recentResults"
              class="interview-item d-flex align-items-center justify-content-between py-3"
            >
              <div class="d-flex align-items-center gap-3">
                <div
                  class="avatar rounded-circle bg-light d-flex align-items-center justify-content-center"
                >
                  {{ result.initials }}
                </div>
                <div>
                  <h6 class="mb-1">{{ result.name }}</h6>
                  <small class="text-muted">{{ result.role }}</small>
                </div>
              </div>
              <div class="d-flex align-items-center gap-5">
                <div>
                  <span class="text-muted me-2">Score</span>
                  <span class="fw-medium">{{ result.score }}</span>
                </div>
                <div class="text-end" style="min-width: 100px;">
                  <h6 class="mb-1">{{ result.date }}</h6>
                  <span
                    class="badge"
                    [class.bg-success]="result.status === 'Passed'"
                    [class.bg-danger]="result.status === 'Failed'"
                  >
                    {{ result.status }}
                  </span>
                </div>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
  `,
  styles: [
    `
      .nav-tabs {
        border-bottom: 1px solid #e5e7eb;
      }
      .nav-link {
        color: #4b5563;
        border: none;
        padding: 1rem;
        margin-right: 1rem;
        cursor: pointer;
      }
      .nav-link.active {
        color: #000;
        font-weight: 500;
        border-bottom: 2px solid #000;
      }
      .avatar {
        width: 40px;
        height: 40px;
        font-size: 1rem;
        color: #4b5563;
      }
      .interview-item {
        border-bottom: 1px solid #e5e7eb;
      }
      .interview-item:last-child {
        border-bottom: none;
      }
      .badge {
        font-weight: 500;
        padding: 0.25rem 0.5rem;
      }
    `,
  ],
})
export class InterviewListComponent {
  activeTab: 'upcoming' | 'recent' = 'upcoming';

  recentResults: RecentResult[] = [
    {
      initials: 'EB',
      name: 'Emily Brown',
      role: 'Frontend Developer',
      score: '8.5/10',
      date: 'Jan 18',
      status: 'Passed',
    },
    {
      initials: 'MC',
      name: 'Michael Clark',
      role: 'Backend Developer',
      score: '5.5/10',
      date: 'Jan 17',
      status: 'Failed',
    },
    {
      initials: 'SJ',
      name: 'Sarah Jones',
      role: 'Full Stack Developer',
      score: '9/10',
      date: 'Jan 16',
      status: 'Passed',
    },
  ];

  setTab(tab: 'upcoming' | 'recent') {
    this.activeTab = tab;
  }
}
