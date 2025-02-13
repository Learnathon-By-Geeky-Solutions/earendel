import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-recent-activity',
  standalone: true,
  imports: [CommonModule],
  template: `
    <div class="card">
      <div class="card-body">
        <h5 class="card-title">Recent Activity</h5>
        <ul class="list-group list-group-flush">
          <li class="list-group-item">
            <p class="mb-1">Completed interview with DataDrive</p>
            <small class="text-muted">2 hours ago</small>
          </li>
          <li class="list-group-item">
            <p class="mb-1">Received feedback for TechCorp interview</p>
            <small class="text-muted">5 hours ago</small>
          </li>
          <li class="list-group-item">
            <p class="mb-1">Scheduled new interview with InnoSoft</p>
            <small class="text-muted">Yesterday at 15:30</small>
          </li>
        </ul>
      </div>
    </div>
  `,
  styles: [
    `
      .card {
        border: none;
        border-radius: 10px;
        box-shadow: 0 4px 6px rgba(0, 0, 0, 0.1);
      }
      .card-title {
        color: #000;
        font-weight: 600;
      }
    `,
  ],
})
export class RecentActivityComponent {}
