import { Component, Input, Output, EventEmitter } from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-job-details-modal',
  standalone: true,
  imports: [CommonModule],
  template: `
    <div *ngIf="isOpen" class="modal-overlay" (click)="close()">
      <div class="modal-content" (click)="$event.stopPropagation()">
        <div class="modal-header">
          <div>
            <h2>{{ job.position }}</h2>
            <p class="company-name">{{ job.companyName }}</p>
          </div>
          <button class="close-btn" (click)="close()">
            <i class="bi bi-x"></i>
          </button>
        </div>

        <div class="modal-body">
          <div class="info-grid">
            <div class="info-item">
              <i class="bi bi-briefcase"></i>
              <div>
                <label>Experience</label>
                <span>{{ job.experience }} years</span>
              </div>
            </div>
            <div class="info-item">
              <i class="bi bi-currency-dollar"></i>
              <div>
                <label>Salary</label>
                <span>{{ job.salary }}</span>
              </div>
            </div>
            <div class="info-item">
              <i class="bi bi-geo-alt"></i>
              <div>
                <label>Location</label>
                <span>{{ job.location || 'Remote' }}</span>
              </div>
            </div>
            <div class="info-item">
              <i class="bi bi-clock"></i>
              <div>
                <label>Employment Type</label>
                <span>{{ job.employmentType || 'Full Time' }}</span>
              </div>
            </div>
          </div>

          <div class="section">
            <h3>Description</h3>
            <p>{{ job.description || 'No description provided.' }}</p>
          </div>

          <div class="section">
            <h3>Requirements</h3>
            <ul>
              <li *ngFor="let req of job.requirements">{{ req }}</li>
            </ul>
          </div>

          <div class="section">
            <h3>Responsibilities</h3>
            <ul>
              <li *ngFor="let resp of job.responsibilities">{{ resp }}</li>
            </ul>
          </div>

          <div class="section">
            <h3>Required Skills</h3>
            <div class="skills-list">
              <span *ngFor="let skill of job.skills" class="skill-tag">
                {{ skill }}
              </span>
            </div>
          </div>

          <div class="section">
            <h3>Benefits</h3>
            <ul>
              <li *ngFor="let benefit of job.benefits">{{ benefit }}</li>
            </ul>
          </div>
        </div>

        <div class="modal-footer">
          <button
            class="apply-btn"
            [disabled]="job.status === 'closed'"
            (click)="apply()"
          >
            Apply Now
          </button>
        </div>
      </div>
    </div>
  `,
  styles: [
    `
      .modal-overlay {
        position: fixed;
        top: 0;
        left: 0;
        right: 0;
        bottom: 0;
        background: rgba(0, 0, 0, 0.5);
        display: flex;
        align-items: center;
        justify-content: center;
        padding: 24px;
        z-index: 1000;
      }

      .modal-content {
        background: white;
        border-radius: 12px;
        width: 100%;
        max-width: 800px;
        max-height: 90vh;
        display: flex;
        flex-direction: column;
      }

      .modal-header {
        padding: 24px;
        border-bottom: 1px solid #eee;
        display: flex;
        justify-content: space-between;
        align-items: start;

        h2 {
          font-size: 24px;
          font-weight: 600;
          margin: 0 0 8px;
          color: #333;
        }

        .company-name {
          font-size: 16px;
          color: #666;
          margin: 0;
        }
      }

      .close-btn {
        background: none;
        border: none;
        font-size: 24px;
        color: #666;
        cursor: pointer;
        padding: 4px;
        line-height: 1;

        &:hover {
          color: #333;
        }
      }

      .modal-body {
        padding: 24px;
        overflow-y: auto;
      }

      .info-grid {
        display: grid;
        grid-template-columns: repeat(auto-fit, minmax(200px, 1fr));
        gap: 24px;
        margin-bottom: 32px;
      }

      .info-item {
        display: flex;
        gap: 12px;
        align-items: start;

        i {
          font-size: 20px;
          color: #666;
        }

        label {
          display: block;
          font-size: 12px;
          color: #666;
          margin-bottom: 4px;
        }

        span {
          font-size: 14px;
          color: #333;
          font-weight: 500;
        }
      }

      .section {
        margin-bottom: 24px;

        h3 {
          font-size: 18px;
          font-weight: 600;
          margin: 0 0 16px;
          color: #333;
        }

        p {
          font-size: 14px;
          line-height: 1.6;
          color: #666;
          margin: 0;
        }

        ul {
          list-style: none;
          padding: 0;
          margin: 0;

          li {
            position: relative;
            padding-left: 24px;
            margin-bottom: 12px;
            font-size: 14px;
            color: #666;
            line-height: 1.6;

            &:before {
              content: '•';
              position: absolute;
              left: 8px;
              color: #999;
            }
          }
        }
      }

      .skills-list {
        display: flex;
        flex-wrap: wrap;
        gap: 8px;
      }

      .skill-tag {
        padding: 4px 12px;
        background: #f8f9fa;
        border-radius: 16px;
        font-size: 12px;
        color: #666;
      }

      .modal-footer {
        padding: 24px;
        border-top: 1px solid #eee;
      }

      .apply-btn {
        width: 100%;
        padding: 12px;
        background: #0066ff;
        color: white;
        border: none;
        border-radius: 8px;
        font-size: 16px;
        font-weight: 500;
        cursor: pointer;
        transition: all 0.2s ease;

        &:hover:not(:disabled) {
          background: #0052cc;
        }

        &:disabled {
          background: #e9ecef;
          color: #666;
          cursor: not-allowed;
        }
      }

      @media (max-width: 768px) {
        .modal-overlay {
          padding: 16px;
        }

        .modal-header,
        .modal-body,
        .modal-footer {
          padding: 16px;
        }

        .info-grid {
          grid-template-columns: 1fr;
          gap: 16px;
        }
      }
    `,
  ],
})
export class JobDetailsModalComponent {
  @Input() isOpen = false;
  @Input() job!: any;
  @Output() closeModal = new EventEmitter<void>();
  @Output() applyForJob = new EventEmitter<string>();

  close() {
    this.closeModal.emit();
  }

  apply() {
    this.applyForJob.emit(this.job.id);
    this.close();
  }
}
