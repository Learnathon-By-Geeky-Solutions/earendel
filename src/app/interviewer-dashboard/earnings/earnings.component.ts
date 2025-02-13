import {
  Component,
  ViewChild,
  type ElementRef,
  type AfterViewInit,
} from '@angular/core';
import { CommonModule } from '@angular/common';
import Chart from 'chart.js/auto';
import { SidebarComponent } from '../sidebar/sidebar.component';
import { PaginationComponent } from '../pagination/pagination.component';

interface Transaction {
  company: string;
  date: string;
  amount: string;
}

@Component({
  selector: 'app-earnings',
  standalone: true,
  imports: [CommonModule, SidebarComponent, PaginationComponent],
  template: `
    <div class="d-flex">
      <app-sidebar> </app-sidebar>

      <div class="container-fluid py-4">
        <h1 class="h3 mb-4">Earnings Overview</h1>

        <!-- Tabs -->
        <div class="mb-4">
          <ul class="nav nav-tabs border-0">
            <li class="nav-item">
              <a
                class="nav-link"
                [class.active]="activeTab === 'overview'"
                (click)="setTab('overview')"
                href="javascript:void(0)"
                >Overview</a
              >
            </li>
            <li class="nav-item">
              <a
                class="nav-link"
                [class.active]="activeTab === 'transactions'"
                (click)="setTab('transactions')"
                href="javascript:void(0)"
                >Transactions</a
              >
            </li>
          </ul>
        </div>

        <!-- Overview Tab -->
        <div *ngIf="activeTab === 'overview'">
          <!-- Stats Cards -->
          <div class="row g-4 mb-4">
            <div class="col-md-3">
              <div class="card">
                <div class="card-body">
                  <div
                    class="d-flex justify-content-between align-items-center mb-2"
                  >
                    <span class="text-muted">Total Earnings</span>
                    <i class="bi bi-currency-dollar text-muted"></i>
                  </div>
                  <h3 class="mb-1">$30,406.00</h3>
                  <small class="text-success">+20.7% from last month</small>
                </div>
              </div>
            </div>
            <div class="col-md-3">
              <div class="card">
                <div class="card-body">
                  <div
                    class="d-flex justify-content-between align-items-center mb-2"
                  >
                    <span class="text-muted">Interviews Conducted</span>
                    <i class="bi bi-people text-muted"></i>
                  </div>
                  <h3 class="mb-1">45</h3>
                  <small class="text-success">+15% from last month</small>
                </div>
              </div>
            </div>
            <div class="col-md-3">
              <div class="card">
                <div class="card-body">
                  <div
                    class="d-flex justify-content-between align-items-center mb-2"
                  >
                    <span class="text-muted">Average per Interview</span>
                    <i class="bi bi-graph-up text-muted"></i>
                  </div>
                  <h3 class="mb-1">$675.69</h3>
                  <small class="text-success">+5% from last month</small>
                </div>
              </div>
            </div>
            <div class="col-md-3">
              <div class="card">
                <div class="card-body">
                  <div
                    class="d-flex justify-content-between align-items-center mb-2"
                  >
                    <span class="text-muted">Next Payment</span>
                    <i class="bi bi-calendar text-muted"></i>
                  </div>
                  <h3 class="mb-1">July 31, 2023</h3>
                  <small class="text-muted">in 12 days</small>
                </div>
              </div>
            </div>
          </div>

          <!-- Charts -->
          <div class="row g-4">
            <div class="col-md-8 mb-4">
              <div class="card">
                <div class="card-body">
                  <h5 class="card-title mb-4">Earnings Over Time</h5>
                  <div style="height: 300px;">
                    <canvas #earningsChart id="earningsChart"></canvas>
                  </div>
                </div>
              </div>
            </div>
            <div class="col-md-4 mb-4">
              <div class="card">
                <div class="card-body">
                  <h5 class="card-title mb-4">Earnings by Interview Type</h5>
                  <div style="height: 300px;">
                    <canvas
                      #interviewTypeChart
                      id="interviewTypeChart"
                    ></canvas>
                  </div>
                </div>
              </div>
            </div>
            <div class="col-md-12">
              <div class="card">
                <div class="card-body">
                  <h5 class="card-title mb-4">Earnings by Company</h5>
                  <div style="height: 300px;">
                    <canvas #companyChart id="companyChart"></canvas>
                  </div>
                </div>
              </div>
            </div>
          </div>
        </div>

        <!-- Transactions Tab -->
        <div *ngIf="activeTab === 'transactions'">
          <div class="card">
            <div class="card-body">
              <h5 class="mb-4">Recent Transactions</h5>
              <p class="text-muted small mb-4">
                Your most recent earnings from interviews
              </p>

              <div class="table-responsive">
                <table class="table table-hover">
                  <thead>
                    <tr>
                      <th class="border-0">Company</th>
                      <th class="border-0">Date</th>
                      <th class="border-0">Amount</th>
                    </tr>
                  </thead>
                  <tbody>
                    <tr *ngFor="let transaction of transactions">
                      <td>{{ transaction.company }}</td>
                      <td>{{ transaction.date }}</td>
                      <td>$ {{ transaction.amount }}</td>
                    </tr>
                  </tbody>
                </table>
              </div>
              <app-pagination
                [currentPage]="currentPage"
                [pageSize]="pageSize"
                [totalItems]="transactions.length"
                (pageChange)="handlePageChange($event)"
              ></app-pagination>
            </div>
          </div>
        </div>
      </div>
    </div>
  `,
  styles: [
    `
      .nav-tabs {
        border-bottom: none;
      }
      .nav-tabs .nav-link {
        color: #6c757d;
        border: none;
        padding: 0.75rem 1rem;
        margin-right: 1rem;
        font-weight: 500;
      }
      .nav-tabs .nav-link.active {
        color: #000;
        border-bottom: 2px solid #000;
        background: none;
      }
      .nav-tabs .nav-link:hover {
        border: none;
        border-bottom: 2px solid #e9ecef;
      }
      .nav-tabs .nav-link.active:hover {
        border-bottom: 2px solid #000;
      }
      .card {
        border: 1px solid #e5e7eb;
        border-radius: 8px;
        box-shadow: 0 1px 3px rgba(0, 0, 0, 0.1);
        transition: transform 0.2s;
      }
      .card:hover {
        transform: translateY(-2px);
      }
      .card-title {
        color: #000;
        font-size: 1rem;
        font-weight: 500;
      }
      .table th {
        color: #6c757d;
        font-weight: 500;
        font-size: 0.875rem;
        text-transform: uppercase;
      }
      .table td {
        padding: 1rem;
        color: #000;
        border-color: #e5e7eb;
      }
    `,
  ],
})
export class EarningsComponent implements AfterViewInit {
  activeTab = 'overview';
  transactions: Transaction[] = [
    { company: 'TechCorp', date: '2023-07-15', amount: '150.00' },
    { company: 'InnoSoft', date: '2023-07-14', amount: '200.00' },
    { company: 'WebWizards', date: '2023-07-13', amount: '175.00' },
    { company: 'CloudTech', date: '2023-07-12', amount: '225.00' },
    { company: 'AppMakers', date: '2023-07-11', amount: '190.00' },
    // Additional transactions here...
  ];

  currentPage = 1;
  pageSize = 2; // Number of transactions per page

  get paginatedTransactions(): Transaction[] {
    const start = (this.currentPage - 1) * this.pageSize;
    return this.transactions.slice(start, start + this.pageSize);
  }

  handlePageChange(page: number): void {
    this.currentPage = page;
  }

  @ViewChild('earningsChart') earningsChartRef!: ElementRef;
  @ViewChild('interviewTypeChart') interviewTypeChartRef!: ElementRef;
  @ViewChild('companyChart') companyChartRef!: ElementRef;

  private earningsChart: Chart | null = null;
  private interviewTypeChart: Chart | null = null;
  private companyChart: Chart | null = null;

  ngAfterViewInit() {
    if (this.activeTab === 'overview') {
      this.initCharts();
    }
  }

  setTab(tab: string) {
    this.activeTab = tab;
    if (tab === 'overview') {
      setTimeout(() => this.initCharts(), 0);
    }
  }

  private destroyCharts() {
    if (this.earningsChart) this.earningsChart.destroy();
    if (this.interviewTypeChart) this.interviewTypeChart.destroy();
    if (this.companyChart) this.companyChart.destroy();
  }

  initCharts() {
    this.destroyCharts();
    this.initEarningsChart();
    this.initInterviewTypeChart();
    this.initCompanyChart();
  }

  initEarningsChart() {
    if (!this.earningsChartRef) return;
    const ctx = this.earningsChartRef.nativeElement;
    this.earningsChart = new Chart(ctx, {
      type: 'line',
      data: {
        labels: ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul'],
        datasets: [
          {
            label: 'Earnings',
            data: [2500, 2000, 10000, 4500, 5000, 4000, 4500],
            borderColor: 'rgb(147, 51, 234)',
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
              callback: (value) => '$' + value,
            },
          },
        },
      },
    });
  }

  initInterviewTypeChart() {
    if (!this.interviewTypeChartRef) return;
    const ctx = this.interviewTypeChartRef.nativeElement;
    new Chart(ctx, {
      type: 'bar',
      data: {
        labels: [
          'Behavioral',
          'Technical',
          'System Design',
          'Pair Programming',
        ],
        datasets: [
          {
            data: [4000, 3000, 2500, 1800],
            backgroundColor: 'rgba(147, 51, 234, 0.5)',
            borderColor: 'rgb(147, 51, 234)',
            borderWidth: 1,
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
              callback: (value) => '$' + value,
            },
          },
        },
      },
    });
  }

  initCompanyChart() {
    if (!this.companyChartRef) return;
    const ctx = this.companyChartRef.nativeElement;
    new Chart(ctx, {
      type: 'pie',
      data: {
        labels: ['TechCorp', 'InnoSoft', 'WebWizards', 'CloudTech'],
        datasets: [
          {
            data: [30, 25, 25, 20],
            backgroundColor: ['#3B82F6', '#F97316', '#22C55E', '#06B6D4'],
          },
        ],
      },
      options: {
        responsive: true,
        maintainAspectRatio: false,
        plugins: {
          legend: {
            position: 'bottom',
          },
        },
      },
    });
  }
}
