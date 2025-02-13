import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SidebarComponent } from '../sidebar/sidebar.component';
import { StatsCardComponent } from '../stats-card/stats-card.component';
import { InterviewTrendsComponent } from '../interview-trends/interview-trends.component';
import { TopSkillsComponent } from '../top-skills/top-skills.component';

@Component({
  selector: 'app-hr-dashboard',
  standalone: true,
  imports: [
    CommonModule,
    SidebarComponent,
    StatsCardComponent,
    InterviewTrendsComponent,
    TopSkillsComponent,
  ],
  template: `
    <div class="d-flex">
      <app-sidebar></app-sidebar>

      <main class="main-content bg-light">
        <div class="p-4">
          <h1 class="h4 mb-4">Dashboard</h1>

          <div class="row g-4 mb-4">
            <div class="col-md-6 col-xl-3">
              <app-stats-card
                title="Total Candidates"
                value="1,234"
                growth="+15%"
                icon="bi-people"
              ></app-stats-card>
            </div>

            <div class="col-md-6 col-xl-3">
              <app-stats-card
                title="Active HR"
                value="56"
                growth="+5%"
                icon="bi-person-badge"
              ></app-stats-card>
            </div>

            <div class="col-md-6 col-xl-3">
              <app-stats-card
                title="Verified Interviewers"
                value="789"
                growth="+20%"
                icon="bi-person-check"
              ></app-stats-card>
            </div>

            <div class="col-md-6 col-xl-3">
              <app-stats-card
                title="Total Earnings"
                value="$98,765"
                growth="+25%"
                icon="bi-currency-dollar"
              ></app-stats-card>
            </div>
          </div>

          <div class="row g-4">
            <div class="col-lg-8">
              <app-interview-trends></app-interview-trends>
            </div>
            <div class="col-lg-4">
              <app-top-skills></app-top-skills>
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

      @media (max-width: 991.98px) {
        .main-content {
          margin-left: 0;
          width: 100%;
        }
      }
    `,
  ],
})
export class HrDashboardComponent {}
