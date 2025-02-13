import {
  Component,
  ViewChild,
  type ElementRef,
  type AfterViewInit,
} from '@angular/core';
import { CommonModule } from '@angular/common';
import { Chart, registerables } from 'chart.js';

Chart.register(...registerables);

@Component({
  selector: 'app-interview-trends',
  standalone: true,
  imports: [CommonModule],
  template: `
    <div class="card border-0 shadow-sm">
      <div class="card-body p-4">
        <h6 class="mb-4">Interview Trends</h6>
        <div class="chart-container">
          <canvas #chartCanvas></canvas>
        </div>
      </div>
    </div>
  `,
  styles: [
    `
      .card {
        border-radius: 8px;
      }

      .chart-container {
        position: relative;
        height: 300px;
        width: 100%;
      }

      canvas {
        width: 100% !important;
        height: 100% !important;
      }
    `,
  ],
})
export class InterviewTrendsComponent implements AfterViewInit {
  @ViewChild('chartCanvas') chartCanvas!: ElementRef<HTMLCanvasElement>;
  private chart!: Chart;

  ngAfterViewInit() {
    this.initChart();
  }

  private initChart() {
    const ctx = this.chartCanvas.nativeElement.getContext('2d');
    if (!ctx) return;

    this.chart = new Chart(ctx, {
      type: 'bar',
      data: {
        labels: [
          'Jan',
          'Feb',
          'Mar',
          'Apr',
          'May',
          'Jun',
          'Jul',
          'Aug',
          'Sep',
          'Oct',
          'Nov',
          'Dec',
        ],
        datasets: [
          {
            data: [
              4500, 2800, 3800, 2500, 1500, 1500, 5000, 3500, 2000, 2800, 4800,
              5000,
            ],
            backgroundColor: '#111827',
            borderRadius: 4,
            barThickness: 24,
            maxBarThickness: 24,
          },
        ],
      },
      options: {
        responsive: true,
        maintainAspectRatio: false,
        plugins: {
          legend: {
            display: false,
          },
          tooltip: {
            callbacks: {
              label: (context) => `$${context.parsed.y}`,
            },
          },
        },
        scales: {
          y: {
            beginAtZero: true,
            max: 6000,
            ticks: {
              stepSize: 1500,
              callback: (value) => `$${value}`,
              font: {
                size: 12,
              },
            },
            grid: {
              color: '#E5E7EB',
              // drawBorder: false,
            },
            border: {
              display: false,
            },
          },
          x: {
            grid: {
              display: false,
            },
            ticks: {
              font: {
                size: 12,
              },
            },
            border: {
              display: false,
            },
          },
        },
      },
    });
  }

  ngOnDestroy() {
    if (this.chart) {
      this.chart.destroy();
    }
  }
}
