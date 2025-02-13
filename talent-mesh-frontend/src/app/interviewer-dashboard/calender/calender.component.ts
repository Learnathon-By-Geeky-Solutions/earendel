import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-calendar',
  standalone: true,
  imports: [CommonModule],
  template: `
    <div class="card">
      <div class="card-body">
        <h5 class="card-title">Calendar</h5>
        <table class="table table-bordered">
          <thead>
            <tr>
              <th colspan="7" class="text-center">January 2025</th>
            </tr>
            <tr>
              <th>Su</th>
              <th>Mo</th>
              <th>Tu</th>
              <th>We</th>
              <th>Th</th>
              <th>Fr</th>
              <th>Sa</th>
            </tr>
          </thead>
          <tbody>
            <tr *ngFor="let week of calendarDays">
              <td *ngFor="let day of week" [class.text-muted]="day === 0">
                {{ day !== 0 ? day : '' }}
              </td>
            </tr>
          </tbody>
        </table>
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
      .table td {
        text-align: center;
      }
    `,
  ],
})
export class CalendarComponent {
  calendarDays = [
    [0, 0, 0, 1, 2, 3, 4],
    [5, 6, 7, 8, 9, 10, 11],
    [12, 13, 14, 15, 16, 17, 18],
    [19, 20, 21, 22, 23, 24, 25],
    [26, 27, 28, 29, 30, 31, 0],
  ];
}
