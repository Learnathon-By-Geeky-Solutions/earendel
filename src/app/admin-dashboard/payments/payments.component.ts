import {
  Component,
  ViewChild,
  type ElementRef,
  type AfterViewInit,
} from '@angular/core';
import { CommonModule } from '@angular/common';

import { Chart, registerables } from 'chart.js';
import { SidebarComponent } from '../sidebar/sidebar.component';
import { PaginationComponent } from '../pagination/pagination.component';
import { FormsModule } from '@angular/forms';

Chart.register(...registerables);

interface HRPayment {
  company: string;
  amount: number;
  date: string;
  status: 'Paid' | 'Pending';
}

interface InterviewerPayment {
  interviewer: string;
  amount: number;
  date: string;
  status: 'Completed' | 'Pending';
}

@Component({
  selector: 'app-payments',
  standalone: true,
  imports: [CommonModule, SidebarComponent, PaginationComponent, FormsModule],
  template: `
    <div class="d-flex">
      <app-sidebar></app-sidebar>

      <main class="main-content bg-light">
        <div class="p-4">
          <h1 class="h4 mb-4">Payments</h1>

          <!-- Stats Cards -->
          <div class="row g-4 mb-4">
            <div class="col-md-3">
              <div class="card border-0 shadow-sm">
                <div class="card-body">
                  <h6 class="text-muted mb-2">Total Revenue</h6>
                  <h2 class="mb-0">$128000</h2>
                </div>
              </div>
            </div>
            <div class="col-md-3">
              <div class="card border-0 shadow-sm">
                <div class="card-body">
                  <h6 class="text-muted mb-2">Total Disbursed</h6>
                  <h2 class="mb-0">$2300</h2>
                </div>
              </div>
            </div>
            <div class="col-md-3">
              <div class="card border-0 shadow-sm">
                <div class="card-body">
                  <h6 class="text-muted mb-2">Pending Disbursements</h6>
                  <h2 class="mb-0">2</h2>
                </div>
              </div>
            </div>
            <div class="col-md-3">
              <div class="card border-0 shadow-sm">
                <div class="card-body">
                  <h6 class="text-muted mb-2">Profit</h6>
                  <h2 class="mb-0">$125700</h2>
                </div>
              </div>
            </div>
          </div>

          <!-- Monthly Revenue Chart -->
          <div class="card border-0 shadow-sm mb-4">
            <div class="card-body">
              <h6 class="mb-4">Monthly Revenue</h6>
              <div class="chart-container">
                <canvas #revenueChart></canvas>
              </div>
            </div>
          </div>

          <!-- HR Payments Section -->
          <div class="mb-5">
            <h2 class="h5 mb-4">HR Payments</h2>
            <div class="card border-0 shadow-sm">
              <div class="table-responsive">
                <table class="table table-hover mb-0">
                  <thead class="bg-light">
                    <tr>
                      <th class="border-0 px-4 py-3">Company</th>
                      <th class="border-0 px-4 py-3">Amount</th>
                      <th class="border-0 px-4 py-3">Date</th>
                      <th class="border-0 px-4 py-3">Status</th>
                      <th class="border-0 px-4 py-3">Actions</th>
                    </tr>
                  </thead>
                  <tbody>
                    <tr *ngFor="let payment of paginatedHRPayments">
                      <td class="px-4 py-3">{{ payment.company }}</td>
                      <td class="px-4 py-3">$ {{ payment.amount }}</td>
                      <td class="px-4 py-3">{{ payment.date }}</td>
                      <td class="px-4 py-3">
                        <span [class]="getStatusClass(payment.status)">
                          {{ payment.status }}
                        </span>
                      </td>
                      <td class="px-4 py-3">
                        <button
                          class="btn btn-link p-0 text-decoration-none"
                          (click)="viewDetails(payment)"
                        >
                          View Details
                        </button>
                      </td>
                    </tr>
                  </tbody>
                </table>
                <app-pagination
                  [currentPage]="currentHRPage"
                  [pageSize]="pageSize"
                  [totalItems]="hrPayments.length"
                  (pageChange)="onHRPageChange($event)"
                ></app-pagination>
              </div>
            </div>
          </div>

          <!-- Interviewer Disbursements Section -->
          <div class="mb-5">
            <div class="d-flex justify-content-between align-items-center mb-4">
              <h2 class="h5 mb-0">Interviewer Disbursements</h2>
              <button class="btn btn-dark" (click)="disbursePayment()">
                Disburse Payment
              </button>
            </div>
            <div class="card border-0 shadow-sm">
              <div class="table-responsive">
                <table class="table table-hover mb-0">
                  <thead class="bg-light">
                    <tr>
                      <th class="border-0 px-4 py-3">Interviewer</th>
                      <th class="border-0 px-4 py-3">Amount</th>
                      <th class="border-0 px-4 py-3">Date</th>
                      <th class="border-0 px-4 py-3">Status</th>
                      <th class="border-0 px-4 py-3">Actions</th>
                    </tr>
                  </thead>
                  <tbody>
                    <tr *ngFor="let payment of paginatedInterviewerPayments">
                      <td class="px-4 py-3">{{ payment.interviewer }}</td>
                      <td class="px-4 py-3">$ {{ payment.amount }}</td>
                      <td class="px-4 py-3">{{ payment.date }}</td>
                      <td class="px-4 py-3">
                        <span
                          [class]="getInterviewerStatusClass(payment.status)"
                        >
                          {{ payment.status }}
                        </span>
                      </td>
                      <td class="px-4 py-3">
                        <button
                          class="btn btn-link p-0 text-decoration-none"
                          (click)="processPayment(payment)"
                        >
                          Process
                        </button>
                      </td>
                    </tr>
                  </tbody>
                </table>
                <app-pagination
                  [currentPage]="currentInterviewerPage"
                  [pageSize]="pageSize"
                  [totalItems]="interviewerPayments.length"
                  (pageChange)="onInterviewerPageChange($event)"
                ></app-pagination>
              </div>
            </div>
          </div>

          <!-- Disbursements by Status Chart -->
          <div class="card border-0 shadow-sm">
            <div class="card-body">
              <h6 class="mb-4">Disbursements by Status</h6>
              <div class="chart-container">
                <canvas #statusChart></canvas>
              </div>
            </div>
          </div>

          <!-- Disburse Payment Modal -->
          <div
            class="modal"
            [class.show]="showDisburseModal"
            [style.display]="showDisburseModal ? 'block' : 'none'"
          >
            <div class="modal-dialog modal-dialog-centered">
              <div class="modal-content border-0">
                <div class="modal-header border-0">
                  <h5 class="modal-title">Disburse Payment</h5>
                  <button
                    type="button"
                    class="btn-close"
                    (click)="closeDisburseModal()"
                  ></button>
                </div>
                <div class="modal-body">
                  <div class="mb-3">
                    <label class="form-label">Interviewer</label>
                    <select
                      class="form-select"
                      [(ngModel)]="newPayment.interviewer"
                    >
                      <option value="">Select interviewer</option>
                      <option value="Alex Johnson">Alex Johnson</option>
                      <option value="Samantha Lee">Samantha Lee</option>
                      <option value="Chris Taylor">Chris Taylor</option>
                    </select>
                  </div>

                  <div class="mb-3">
                    <label class="form-label">Amount</label>
                    <input
                      type="number"
                      class="form-control"
                      [(ngModel)]="newPayment.amount"
                    />
                  </div>
                </div>
                <div class="modal-footer border-0">
                  <button
                    type="button"
                    class="btn btn-link text-dark"
                    (click)="closeDisburseModal()"
                  >
                    Cancel
                  </button>
                  <button
                    type="button"
                    class="btn btn-dark"
                    [disabled]="!isValidPayment(newPayment)"
                    (click)="submitPayment()"
                  >
                    Submit Payment
                  </button>
                </div>
              </div>
            </div>
          </div>
        </div>
      </main>
    </div>
  `,
  styles: [
    `
      :host {
        display: block;
        min-height: 100vh;
        background-color: #f9fafb;
      }

      .main-content {
        margin-left: 240px;
        width: calc(100% - 240px);
        min-height: 100vh;
      }

      .card {
        border-radius: 8px;
      }

      .chart-container {
        position: relative;
        height: 300px;
        width: 100%;
      }

      .table {
        margin-bottom: 0;
      }

      .table th {
        font-weight: 500;
        font-size: 14px;
        color: #6b7280;
      }

      .table td {
        font-size: 14px;
        vertical-align: middle;
      }

      .status-badge {
        padding: 4px 8px;
        border-radius: 4px;
        font-size: 13px;
        font-weight: 500;
      }

      .status-paid,
      .status-completed {
        background-color: #ecfdf5;
        color: #059669;
      }

      .status-pending {
        background-color: #fef3c7;
        color: #d97706;
      }

      .btn-link {
        font-size: 14px;
        color: #2563eb;
      }

      .btn-link:hover {
        color: #1d4ed8;
      }

      @media (max-width: 991.98px) {
        .main-content {
          margin-left: 0;
          width: 100%;
        }
      }
    `,
  ],
})
export class PaymentsComponent implements AfterViewInit {
  @ViewChild('revenueChart') revenueChartRef!: ElementRef;
  @ViewChild('statusChart') statusChartRef!: ElementRef;

  private revenueChart!: Chart;
  private statusChart!: Chart;

  // Pagination
  currentHRPage = 1;
  currentInterviewerPage = 1;
  pageSize = 10;

  hrPayments: HRPayment[] = [
    {
      company: 'TechCorp',
      amount: 5000,
      date: '2023-07-01',
      status: 'Paid',
    },
    {
      company: 'InnoSoft',
      amount: 3000,
      date: '2023-07-05',
      status: 'Pending',
    },
    {
      company: 'DataDynamics',
      amount: 7000,
      date: '2023-07-10',
      status: 'Paid',
    },
  ];

  interviewerPayments: InterviewerPayment[] = [
    {
      interviewer: 'Alex Johnson',
      amount: 500,
      date: '2023-07-15',
      status: 'Pending',
    },
    {
      interviewer: 'Samantha Lee',
      amount: 750,
      date: '2023-07-14',
      status: 'Completed',
    },
    {
      interviewer: 'Chris Taylor',
      amount: 600,
      date: '2023-07-16',
      status: 'Pending',
    },
    {
      interviewer: 'Rachel Green',
      amount: 450,
      date: '2023-07-13',
      status: 'Completed',
    },
  ];

  get paginatedHRPayments(): HRPayment[] {
    const start = (this.currentHRPage - 1) * this.pageSize;
    const end = start + this.pageSize;
    return this.hrPayments.slice(start, end);
  }

  get paginatedInterviewerPayments(): InterviewerPayment[] {
    const start = (this.currentInterviewerPage - 1) * this.pageSize;
    const end = start + this.pageSize;
    return this.interviewerPayments.slice(start, end);
  }

  showDisburseModal = false;
  newPayment: Partial<InterviewerPayment> = {};

  ngAfterViewInit() {
    this.initializeCharts();
  }

  private initializeCharts() {
    this.initializeRevenueChart();
    this.initializeStatusChart();
  }

  private initializeRevenueChart() {
    const ctx = this.revenueChartRef.nativeElement.getContext('2d');

    this.revenueChart = new Chart(ctx, {
      type: 'line',
      data: {
        labels: ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun'],
        datasets: [
          {
            label: 'Revenue',
            data: [15000, 17000, 19000, 18500, 20000, 22000],
            borderColor: '#111827',
            tension: 0.4,
            fill: false,
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
        },
        scales: {
          y: {
            beginAtZero: true,
            ticks: {
              callback: (value) => `$${value}`,
            },
          },
        },
      },
    });
  }

  private initializeStatusChart() {
    const ctx = this.statusChartRef.nativeElement.getContext('2d');

    this.statusChart = new Chart(ctx, {
      type: 'bar',
      data: {
        labels: ['Completed', 'Pending'],
        datasets: [
          {
            label: 'Count',
            data: [2, 2],
            backgroundColor: '#111827',
            borderRadius: 4,
            barThickness: 40,
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
        },
        scales: {
          y: {
            beginAtZero: true,
            ticks: {
              stepSize: 1,
            },
          },
        },
      },
    });
  }

  getStatusClass(status: string): string {
    return `status-badge status-${status.toLowerCase()}`;
  }

  getInterviewerStatusClass(status: string): string {
    return `status-badge status-${status.toLowerCase()}`;
  }

  viewDetails(payment: HRPayment) {
    console.log('View details:', payment);
  }

  processPayment(payment: InterviewerPayment) {
    console.log('Process payment:', payment);
  }

  closeDisburseModal(): void {
    this.showDisburseModal = false;
    this.newPayment = {};
  }

  isValidPayment(payment: Partial<InterviewerPayment>): boolean {
    return !!(payment.interviewer && payment.amount && payment.amount > 0);
  }

  submitPayment(): void {
    if (this.isValidPayment(this.newPayment)) {
      const payment: InterviewerPayment = {
        interviewer: this.newPayment.interviewer!,
        amount: this.newPayment.amount!,
        date: new Date().toISOString().split('T')[0],
        status: 'Pending',
      };
      this.interviewerPayments.push(payment);
      this.closeDisburseModal();
    }
  }

  disbursePayment(): void {
    this.showDisburseModal = true;
  }

  onHRPageChange(page: number): void {
    this.currentHRPage = page;
  }

  onInterviewerPageChange(page: number): void {
    this.currentInterviewerPage = page;
  }
}
