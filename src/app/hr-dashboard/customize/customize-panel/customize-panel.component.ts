import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-customize-panel',
  standalone: true,
  imports: [CommonModule],
  template: `
    <div class="customize-panel bg-white rounded-3 p-4">
      <div class="d-flex align-items-center justify-content-between mb-4">
        <div class="d-flex align-items-center">
          <button class="btn btn-link text-dark p-0 me-3">
            <i class="bi bi-arrow-left"></i>
          </button>
          <h5 class="mb-0">Customize</h5>
        </div>
        <div class="tag-container">
          <span class="tag me-2">
            Frontend
          </span>
        </div>
      </div>




      <div class="mb-4">
        <div
          class="config-item d-flex justify-content-between align-items-center p-3 rounded-3"
        >
          <span class="fw-medium">Choose a round based interview</span>
          <i class="bi bi-chevron-right"></i>
        </div>
      </div>

      <button class="btn submit-btn w-100">
        <div class="d-flex align-items-center justify-content-between">
          <div class="d-flex align-items-center">
            <div class="avatar-group me-2">
              <span class="avatar">👤</span>
            </div>
            <span>Submit & add candidates</span>
          </div>
          <div class="duration-selector d-flex align-items-center">
            Duration
            <div class="ms-2 d-flex align-items-center">
              1 hr
              <i class="bi bi-chevron-down ms-1"></i>
            </div>
          </div>
        </div>
      </button>
    </div>
  `,
  styles: [
    `
      .customize-panel {
        width: 480px;
        box-shadow: 0 2px 8px rgba(0, 0, 0, 0.1);
        margin-left: 48px;
      }
      .tag {
        display: inline-flex;
        align-items: center;
        padding: 4px 12px;
        background-color: #f8f9fa;
        border-radius: 16px;
        font-size: 14px;
      }
      .tag .btn-link {
        color: #0d6efd;
        text-decoration: none;
        font-size: 14px;
      }
      .seniority-selector,
      .config-item {
        background-color: #f8f9fa;
        cursor: pointer;
        transition: background-color 0.2s;
        border: 1px solid #f1f3f5;
      }
      .seniority-selector:hover,
      .config-item:hover {
        background-color: #f1f3f5;
      }
      .avatar-group {
        display: flex;
        align-items: center;
      }
      .avatar {
        width: 24px;
        height: 24px;
        border-radius: 50%;
        background-color: #e9ecef;
        display: flex;
        align-items: center;
        justify-content: center;
        font-size: 12px;
      }
      .submit-btn {
        background-color: #212529;
        color: white;
        padding: 12px 16px;
        border: none;
        border-radius: 6px;
      }
      .duration-selector {
        font-size: 14px;
        color: rgba(255, 255, 255, 0.8);

        > div {
          background-color: rgba(255, 255, 255, 0.1);
          padding: 4px 8px;
          border-radius: 4px;
        }
      }
    `,
  ],
})
export class CustomizePanelComponent {}
