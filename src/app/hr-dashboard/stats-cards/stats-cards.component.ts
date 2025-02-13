import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-stats-cards',
  standalone: true,
  imports: [CommonModule],
  template: `
    <div class="row g-4">
      <div class="col-md-3">
        <div class="card border-0 shadow-sm">
          <div class="card-body">
            <div class="d-flex justify-content-between align-items-center mb-3">
              <span class="text-muted">Scheduled Today</span>
              <i class="bi bi-calendar text-muted"></i>
            </div>
            <h3 class="mb-1">12</h3>
            <small class="text-muted">4 completed, 8 upcoming</small>
          </div>
        </div>
      </div>

      <div class="col-md-3">
        <div class="card border-0 shadow-sm">
          <div class="card-body">
            <div class="d-flex justify-content-between align-items-center mb-3">
              <span class="text-muted">Credits Remaining</span>
              <i class="bi bi-coin text-muted"></i>
            </div>
            <h3 class="mb-1">150</h3>
            <div class="progress" style="height: 6px;">
              <div class="progress-bar bg-dark" style="width: 60%"></div>
            </div>
          </div>
        </div>
      </div>

      <div class="col-md-3">
        <div class="card border-0 shadow-sm">
          <div class="card-body">
            <div class="d-flex justify-content-between align-items-center mb-3">
              <span class="text-muted">Success Rate</span>
              <i class="bi bi-graph-up text-muted"></i>
            </div>
            <h3 class="mb-1">75%</h3>
            <small class="text-success">+5% from last month</small>
          </div>
        </div>
      </div>

      <div class="col-md-3">
        <div class="card border-0 shadow-sm">
          <div class="card-body">
            <div class="d-flex justify-content-between align-items-center mb-3">
              <span class="text-muted">Total Interviews</span>
              <i class="bi bi-people text-muted"></i>
            </div>
            <h3 class="mb-1">245</h3>
            <small class="text-success">+20% from last month</small>
          </div>
        </div>
      </div>
    </div>
  `,
  styles: [
    `
      .card {
        border-radius: 8px;
      }
      .card-body {
        padding: 1.5rem;
      }
      .progress {
        border-radius: 3px;
        background-color: #f3f4f6;
      }
    `,
  ],
})
export class StatsCardsComponent {}
