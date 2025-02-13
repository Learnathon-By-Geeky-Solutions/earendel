import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-statistics',
  standalone: true,
  imports: [CommonModule],
  template: `
    <div class="row">
      <div class="col-md-3 mb-4">
        <div class="card">
          <div class="card-body">
            <h5 class="card-title">Total Interviews</h5>
            <p class="card-text display-4">245</p>
            <p class="card-text text-success">+20% from last month</p>
          </div>
        </div>
      </div>
      <div class="col-md-3 mb-4">
        <div class="card">
          <div class="card-body">
            <h5 class="card-title">Earnings</h5>
            <p class="card-text display-4">$12,450</p>
            <p class="card-text text-success">+15% from last month</p>
          </div>
        </div>
      </div>
      <div class="col-md-3 mb-4">
        <div class="card">
          <div class="card-body">
            <h5 class="card-title">Avg. Rating</h5>
            <p class="card-text display-4">4.8</p>
            <p class="card-text text-success">+0.2 from last month</p>
          </div>
        </div>
      </div>
      <div class="col-md-3 mb-4">
        <div class="card">
          <div class="card-body">
            <h5 class="card-title">Completion Rate</h5>
            <p class="card-text display-4">98%</p>
            <p class="card-text text-success">+2% from last month</p>
          </div>
        </div>
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
      .card-text {
        color: #000;
      }
    `,
  ],
})
export class StatisticsComponent {}
