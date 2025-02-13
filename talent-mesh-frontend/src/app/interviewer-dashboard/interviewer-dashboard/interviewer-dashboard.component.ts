import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { Chart } from 'chart.js/auto';
import { SidebarComponent } from '../sidebar/sidebar.component';

@Component({
  selector: 'app-interviewer-dashboard',
  standalone: true,
  imports: [CommonModule, RouterModule, SidebarComponent],
  template: `
    <div class="d-flex">
    <app-sidebar> </app-sidebar>

      <!-- Main Content -->
      <div class="flex-grow-1 p-4">
        <div class="container-fluid">
          <!-- Welcome Message -->
          <div class="d-flex justify-content-between align-items-center mb-4">
            <h1 class="h3 mb-0">Welcome back, Sarah</h1>
            <button
              class="btn btn-light rounded-circle"
              style="width: 40px; height: 40px;"
            >
              <span class="text-muted">S</span>
            </button>
          </div>

          <!-- Statistics Cards -->
          <div class="row g-4 mb-4">
            <div class="col-md-3">
              <div class="card border-0 shadow-sm">
                <div class="card-body">
                  <div class="d-flex justify-content-between mb-2">
                    <span class="text-muted">Total Interviews</span>
                    <i class="bi bi-info-circle text-muted"></i>
                  </div>
                  <h3 class="mb-0">245</h3>
                  <small class="text-muted">+20% from last month</small>
                </div>
              </div>
            </div>
            <div class="col-md-3">
              <div class="card border-0 shadow-sm">
                <div class="card-body">
                  <div class="d-flex justify-content-between mb-2">
                    <span class="text-muted">Earnings</span>
                    <i class="bi bi-info-circle text-muted"></i>
                  </div>
                  <h3 class="mb-0">$12,450</h3>
                  <small class="text-muted">+15% from last period</small>
                </div>
              </div>
            </div>
            <div class="col-md-3">
              <div class="card border-0 shadow-sm">
                <div class="card-body">
                  <div class="d-flex justify-content-between mb-2">
                    <span class="text-muted">Avg Rating</span>
                    <i class="bi bi-info-circle text-muted"></i>
                  </div>
                  <h3 class="mb-0">4.8</h3>
                  <small class="text-muted">+0.2 from last month</small>
                </div>
              </div>
            </div>
            <div class="col-md-3">
              <div class="card border-0 shadow-sm">
                <div class="card-body">
                  <div class="d-flex justify-content-between mb-2">
                    <span class="text-muted">Completion Rate</span>
                    <i class="bi bi-info-circle text-muted"></i>
                  </div>
                  <h3 class="mb-0">98%</h3>
                  <small class="text-muted">+2% from last month</small>
                </div>
              </div>
            </div>
          </div>

          <!-- Charts and Activity -->
          <div class="row g-4">
            <div class="col-md-8">
              <div class="card border-0 shadow-sm">
                <div class="card-body">
                  <h5 class="card-title">Interview Performance</h5>
                  <canvas id="performanceChart" height="300"></canvas>
                </div>
              </div>
            </div>
            <div class="col-md-4">
              <div class="card border-0 shadow-sm">
                <div class="card-body">
                  <h5 class="card-title">Upcoming Interviews</h5>
                  <div class="upcoming-interviews">
                    <div class="interview-item p-3 border-bottom">
                      <div class="d-flex align-items-center mb-2">
                        <div class="bg-light rounded p-2 me-3">
                          <i class="bi bi-building"></i>
                        </div>
                        <div>
                          <h6 class="mb-0">TechCorp - Senior Developer</h6>
                          <small class="text-muted">7/15/2023 at 14:00</small>
                        </div>
                      </div>
                    </div>
                    <div class="interview-item p-3 border-bottom">
                      <div class="d-flex align-items-center mb-2">
                        <div class="bg-light rounded p-2 me-3">
                          <i class="bi bi-building"></i>
                        </div>
                        <div>
                          <h6 class="mb-0">InnoSoft - Product Manager</h6>
                          <small class="text-muted">7/16/2023 at 15:30</small>
                        </div>
                      </div>
                    </div>
                    <div class="interview-item p-3">
                      <div class="d-flex align-items-center mb-2">
                        <div class="bg-light rounded p-2 me-3">
                          <i class="bi bi-building"></i>
                        </div>
                        <div>
                          <h6 class="mb-0">DataDrive - Data Scientist</h6>
                          <small class="text-muted">7/18/2023 at 10:45</small>
                        </div>
                      </div>
                    </div>
                  </div>
                </div>
              </div>
            </div>
          </div>

          <!-- Recent Activity and Calendar -->
          <div class="row g-4 mt-2">
            <div class="col-md-8">
              <div class="card border-0 shadow-sm">
                <div class="card-body">
                  <h5 class="card-title">Recent Activity</h5>
                  <div class="activity-list">
                    <div
                      class="activity-item d-flex align-items-center p-3 border-bottom"
                    >
                      <div
                        class="activity-icon bg-light rounded-circle p-2 me-3"
                      >
                        <i class="bi bi-check-circle"></i>
                      </div>
                      <div>
                        <p class="mb-0">Completed interview with DataDrive</p>
                        <small class="text-muted">2 hours ago</small>
                      </div>
                    </div>
                    <div
                      class="activity-item d-flex align-items-center p-3 border-bottom"
                    >
                      <div
                        class="activity-icon bg-light rounded-circle p-2 me-3"
                      >
                        <i class="bi bi-star"></i>
                      </div>
                      <div>
                        <p class="mb-0">
                          Received feedback for TechCorp interview
                        </p>
                        <small class="text-muted">5 hours ago</small>
                      </div>
                    </div>
                    <div class="activity-item d-flex align-items-center p-3">
                      <div
                        class="activity-icon bg-light rounded-circle p-2 me-3"
                      >
                        <i class="bi bi-calendar-event"></i>
                      </div>
                      <div>
                        <p class="mb-0">
                          Scheduled new interview with InnoSoft
                        </p>
                        <small class="text-muted">Yesterday at 15:30</small>
                      </div>
                    </div>
                  </div>
                </div>
              </div>
            </div>
            
          </div>
        </div>
      </div>
    </div>
  `,
  styles: [
    `
      .nav-link {
        color: #666;
        transition: all 0.3s;
      }
      .nav-link:hover,
      .nav-link.active {
        background-color: #f8f9fa;
        color: #000;
      }
      .card {
        transition: transform 0.2s;
      }
      .card:hover {
        transform: translateY(-2px);
      }
      .activity-icon {
        width: 40px;
        height: 40px;
        display: flex;
        align-items: center;
        justify-content: center;
      }
      .upcoming-interviews {
        max-height: 300px;
        overflow-y: auto;
      }
    `,
  ],
})
export class InterviewerDashboardComponent {
  ngOnInit() {
    this.initChart();
  }

  initChart() {
    const ctx = document.getElementById(
      'performanceChart'
    ) as HTMLCanvasElement;
    new Chart(ctx, {
      type: 'bar',
      data: {
        labels: ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun'],
        datasets: [
          {
            label: 'Interviews',
            data: [10, 18, 15, 22, 25, 17],
            backgroundColor: 'rgba(147, 51, 234, 0.5)',
            borderColor: 'rgba(147, 51, 234, 1)',
            borderWidth: 1,
          },
        ],
      },
      options: {
        responsive: true,
        scales: {
          y: {
            beginAtZero: true,
            ticks: {
              stepSize: 7,
            },
          },
        },
        plugins: {
          legend: {
            display: false,
          },
        },
      },
    });
  }
}
