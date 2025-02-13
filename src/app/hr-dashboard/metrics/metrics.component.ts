import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-metrics',
  standalone: true,
  imports: [CommonModule],
  template: `
    <div class="row g-4 mt-4">
      <div class="col-md-4">
        <div class="card border-0 shadow-sm">
          <div class="card-body">
            <h5 class="card-title mb-4">Department Breakdown</h5>

            <div class="department-item mb-3">
              <div class="d-flex justify-content-between mb-2">
                <span>Frontend</span>
                <span>45 interviews</span>
              </div>
              <div class="progress" style="height: 6px;">
                <div class="progress-bar bg-dark" style="width: 75%"></div>
              </div>
            </div>

            <div class="department-item mb-3">
              <div class="d-flex justify-content-between mb-2">
                <span>Backend</span>
                <span>32 interviews</span>
              </div>
              <div class="progress" style="height: 6px;">
                <div class="progress-bar bg-dark" style="width: 60%"></div>
              </div>
            </div>

            <div class="department-item mb-3">
              <div class="d-flex justify-content-between mb-2">
                <span>Full Stack</span>
                <span>28 interviews</span>
              </div>
              <div class="progress" style="height: 6px;">
                <div class="progress-bar bg-dark" style="width: 45%"></div>
              </div>
            </div>

            <div class="department-item">
              <div class="d-flex justify-content-between mb-2">
                <span>DevOps</span>
                <span>15 interviews</span>
              </div>
              <div class="progress" style="height: 6px;">
                <div class="progress-bar bg-dark" style="width: 30%"></div>
              </div>
            </div>
          </div>
        </div>
      </div>

      <div class="col-md-4">
        <div class="card border-0 shadow-sm">
          <div class="card-body">
            <h5 class="card-title mb-4">Interview Status</h5>

            <div
              class="status-item d-flex justify-content-between align-items-center mb-3"
            >
              <div class="d-flex align-items-center">
                <i class="bi bi-clock text-warning me-2"></i>
                <span>Scheduled</span>
              </div>
              <span class="fw-medium">24</span>
            </div>

            <div
              class="status-item d-flex justify-content-between align-items-center mb-3"
            >
              <div class="d-flex align-items-center">
                <i class="bi bi-check-circle text-success me-2"></i>
                <span>Completed</span>
              </div>
              <span class="fw-medium">156</span>
            </div>

            <div
              class="status-item d-flex justify-content-between align-items-center"
            >
              <div class="d-flex align-items-center">
                <i class="bi bi-x-circle text-danger me-2"></i>
                <span>Cancelled</span>
              </div>
              <span class="fw-medium">12</span>
            </div>
          </div>
        </div>
      </div>

      <div class="col-md-4">
        <div class="card border-0 shadow-sm">
          <div class="card-body">
            <h5 class="card-title mb-4">Cost Overview</h5>

            <div class="cost-item mb-4">
              <span class="text-muted d-block mb-2"
                >Average Cost per Interview</span
              >
              <h3 class="mb-0">$85</h3>
            </div>

            <div class="cost-item mb-4">
              <span class="text-muted d-block mb-2">Monthly Spend</span>
              <h3 class="mb-0">$4,250</h3>
            </div>

            <div class="cost-item">
              <span class="text-muted d-block mb-2">Total Spend</span>
              <h3 class="mb-0">$7,750</h3>
            </div>
          </div>
        </div>
      </div>
    </div>
  `,
  styles: [
    `
      .card-title {
        font-size: 1rem;
        font-weight: 500;
      }
      .progress {
        border-radius: 3px;
        background-color: #f3f4f6;
      }
      .status-item {
        padding: 0.5rem 0;
      }
    `,
  ],
})
export class MetricsComponent {}
