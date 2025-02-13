import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';

@Component({
  selector: 'app-action-buttons',
  standalone: true,
  imports: [CommonModule, RouterModule],
  template: `
    <div class="d-flex gap-3 my-4">
      <button routerLink="/job-post" class="btn btn-success">
        <i class="bi bi-plus-lg me-2"></i>
        Post Job & Get Interviewed
      </button>
      <button class="btn btn-outline-dark">
        <i class="bi bi-coin me-2"></i>
        Buy Credits
      </button>
      <button class="btn btn-outline-dark">
        <i class="bi bi-graph-up me-2"></i>
        View Reports
      </button>
    </div>
  `,
  styles: [
    `
      .btn {
        padding: 0.625rem 1rem;
        font-weight: 500;
      }
      .btn-success {
        background-color: #22c55e;
        border-color: #22c55e;
      }
    `,
  ],
})
export class ActionButtonsComponent {}
