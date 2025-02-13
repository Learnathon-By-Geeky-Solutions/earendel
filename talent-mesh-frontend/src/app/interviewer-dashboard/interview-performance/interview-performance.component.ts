import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Chart, registerables } from 'chart.js';

Chart.register(...registerables);

@Component({
  selector: 'app-interview-performance',
  standalone: true,
  imports: [CommonModule],
  template: `
    <div class="card">
      <div class="card-body">
        <h5 class="card-title">Interview Performance</h5>
        <canvas id="interviewChart"></canvas>
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
export class InterviewPerformanceComponent implements OnInit {
  ngOnInit() {
    this.createChart();
  }

  createChart() {
    const ctx = document.getElementById('interviewChart') as HTMLCanvasElement;
    new Chart(ctx, {
      type: 'bar',
      data: {
        labels: ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun'],
        datasets: [
          {
            label: 'Number of Interviews',
            data: [28, 21, 14, 7, 14, 21],
            backgroundColor: 'rgba(75, 192, 192, 0.6)',
            borderColor: 'rgba(75, 192, 192, 1)',
            borderWidth: 1,
          },
        ],
      },
      options: {
        responsive: true,
        scales: {
          y: {
            beginAtZero: true,
          },
        },
      },
    });
  }
}
