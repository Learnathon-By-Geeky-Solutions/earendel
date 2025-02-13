import { Component, Input } from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-stats-card',
  standalone: true,
  imports: [CommonModule],
  template: `
    <div class="card border-0 shadow-sm h-100">
      <div class="card-body p-4">
        <div class="d-flex justify-content-between align-items-center mb-3">
          <h6 class="text-muted mb-0">{{ title }}</h6>
          <i [class]="'bi ' + icon + ' text-muted'"></i>
        </div>
        <div class="d-flex align-items-baseline gap-2">
          <h2 class="mb-0 fw-bold">{{ value }}</h2>
          <small class="text-success">{{ growth }} from last month</small>
        </div>
      </div>
    </div>
  `,
  styles: [
    `
      .card {
        border-radius: 8px;
      }

      h2 {
        font-size: 32px;
        line-height: 1.2;
      }

      .bi {
        font-size: 20px;
      }
    `,
  ],
})
export class StatsCardComponent {
  @Input() title: string = '';
  @Input() value: string = '';
  @Input() growth: string = '';
  @Input() icon: string = '';
}
