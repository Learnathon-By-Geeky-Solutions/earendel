import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { CandidateSidebarComponent } from '../candidate-sidebar/candidate-sidebar.component';

@Component({
  selector: 'app-candidate-layout',
  standalone: true,
  imports: [CommonModule, RouterModule, CandidateSidebarComponent],
  template: `
    <div class="dashboard-container">
      <app-candidate-sidebar></app-candidate-sidebar>
      <main class="main-content">
        <router-outlet></router-outlet>
      </main>
    </div>
  `,
  styles: [
    `
      .dashboard-container {
        display: grid;
        grid-template-columns: 240px 1fr;
        min-height: 100vh;
        background: #f8f9fa;
      }

      .main-content {
        padding: 24px;
        overflow-y: auto;
      }

      @media (max-width: 768px) {
        .dashboard-container {
          grid-template-columns: 1fr;
        }

        .main-content {
          padding: 16px;
        }
      }
    `,
  ],
})
export class CandidateLayoutComponent {}
