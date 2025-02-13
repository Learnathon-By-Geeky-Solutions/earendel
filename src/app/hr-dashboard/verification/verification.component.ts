import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { SidebarComponent } from '../sidebar/sidebar.component';
import { PaginationComponent } from '../pagination/pagination.component';

interface Interviewer {
  name: string;
  personalEmail: string;
  workEmail: string;
  submittedDate: string;
  idCard: 'Uploaded' | 'Missing';
  workPermit: 'Uploaded' | 'Missing';
}

@Component({
  selector: 'app-verification',
  standalone: true,
  imports: [CommonModule, FormsModule, SidebarComponent, PaginationComponent],
  template: `
    <div class="d-flex">
      <app-sidebar></app-sidebar>

      <main class="main-content bg-light">
        <div class="p-4">
          <h1 class="h4 mb-4">Interviewer Verification</h1>

          <div class="card border-0 shadow-sm">
            <div class="table-responsive">
              <table class="table table-hover mb-0">
                <thead class="bg-light">
                  <tr>
                    <th class="border-0 px-4 py-3">Name</th>
                    <th class="border-0 px-4 py-3">Personal Email</th>
                    <th class="border-0 px-4 py-3">Work Email</th>
                    <th class="border-0 px-4 py-3">Submitted Date</th>
                    <th class="border-0 px-4 py-3">ID Card</th>
                    <th class="border-0 px-4 py-3">Work Permit</th>
                    <th class="border-0 px-4 py-3">Actions</th>
                  </tr>
                </thead>
                <tbody>
                  <tr *ngFor="let interviewer of paginatedInterviewers">
                    <td class="px-4 py-3">{{ interviewer.name }}</td>
                    <td class="px-4 py-3">{{ interviewer.personalEmail }}</td>
                    <td class="px-4 py-3">{{ interviewer.workEmail }}</td>
                    <td class="px-4 py-3">{{ interviewer.submittedDate }}</td>
                    <td class="px-4 py-3">
                      <span [class]="getStatusClass(interviewer.idCard)">
                        {{ interviewer.idCard }}
                      </span>
                    </td>
                    <td class="px-4 py-3">
                      <span [class]="getStatusClass(interviewer.workPermit)">
                        {{ interviewer.workPermit }}
                      </span>
                    </td>
                    <td class="px-4 py-3">
                      <div class="d-flex gap-2">
                        <button
                          class="btn btn-link p-0 text-decoration-none"
                          (click)="openReviewModal(interviewer)"
                        >
                          Review
                        </button>
                        <button
                          class="btn btn-link p-0 text-decoration-none text-danger"
                          (click)="openRejectModal(interviewer)"
                        >
                          Reject
                        </button>
                      </div>
                    </td>
                  </tr>
                </tbody>
              </table>
              <app-pagination
                [currentPage]="currentPage"
                [pageSize]="pageSize"
                [totalItems]="interviewers.length"
                (pageChange)="onPageChange($event)"
              ></app-pagination>
            </div>
          </div>
        </div>
      </main>

      <!-- Review Modal -->
      <div
        class="modal"
        [class.show]="showReviewModal"
        [style.display]="showReviewModal ? 'block' : 'none'"
      >
        <div class="modal-dialog modal-dialog-centered">
          <div class="modal-content border-0">
            <div class="modal-header border-0 pb-0">
              <h5 class="modal-title">
                Review Interviewer: {{ selectedInterviewer?.name }}
              </h5>
              <button
                type="button"
                class="btn-close"
                (click)="closeReviewModal()"
              ></button>
            </div>
            <div class="modal-body pt-2">
              <p class="text-muted small mb-4">
                Review the interviewer's details and uploaded documents.
              </p>

              <div class="mb-3">
                <label class="form-label">Name</label>
                <input
                  type="text"
                  class="form-control"
                  [value]="selectedInterviewer?.name"
                  readonly
                />
              </div>

              <div class="mb-3">
                <label class="form-label">Work Email</label>
                <input
                  type="email"
                  class="form-control"
                  [value]="selectedInterviewer?.workEmail"
                  readonly
                />
              </div>

              <div class="mb-3">
                <label class="form-label">ID Card</label>
                <button class="btn btn-outline-secondary d-block">
                  View ID Card
                </button>
              </div>

              <div class="mb-3">
                <label class="form-label">Work Permit</label>
                <button class="btn btn-outline-secondary d-block">
                  View Work Permit
                </button>
              </div>
            </div>
            <div class="modal-footer border-0">
              <button
                type="button"
                class="btn btn-dark px-4"
                (click)="approveInterviewer()"
              >
                Approve
              </button>
            </div>
          </div>
        </div>
      </div>

      <!-- Reject Modal -->
      <div
        class="modal"
        [class.show]="showRejectModal"
        [style.display]="showRejectModal ? 'block' : 'none'"
      >
        <div class="modal-dialog modal-dialog-centered">
          <div class="modal-content border-0">
            <div class="modal-header border-0 pb-0">
              <h5 class="modal-title">
                Reject Interviewer: {{ selectedInterviewer?.name }}
              </h5>
              <button
                type="button"
                class="btn-close"
                (click)="closeRejectModal()"
              ></button>
            </div>
            <div class="modal-body pt-2">
              <p class="text-muted small mb-4">
                Provide a reason for rejecting this interviewer.
              </p>

              <div class="mb-3">
                <label class="form-label">Reason</label>
                <textarea
                  class="form-control"
                  rows="4"
                  [(ngModel)]="rejectionReason"
                  placeholder="Enter rejection reason"
                >
                </textarea>
              </div>
            </div>
            <div class="modal-footer border-0">
              <button
                type="button"
                class="btn btn-danger px-4"
                [disabled]="!rejectionReason"
                (click)="confirmReject()"
              >
                Confirm Rejection
              </button>
            </div>
          </div>
        </div>
      </div>

      <!-- Modal Backdrop -->
      <div
        class="modal-backdrop fade show"
        *ngIf="showReviewModal || showRejectModal"
        (click)="closeAllModals()"
      ></div>
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

      .status-uploaded {
        background-color: #ecfdf5;
        color: #059669;
      }

      .status-missing {
        background-color: #fee2e2;
        color: #dc2626;
      }

      .btn-link {
        font-size: 14px;
      }

      .modal-content {
        border-radius: 12px;
      }

      .modal-header {
        padding: 1.5rem 1.5rem 0.5rem;
      }

      .modal-body {
        padding: 1rem 1.5rem;
      }

      .modal-footer {
        padding: 1rem 1.5rem 1.5rem;
      }

      .form-control {
        border-radius: 6px;
        padding: 0.5rem 0.75rem;
      }

      .form-control:read-only {
        background-color: #f9fafb;
      }

      .btn-outline-secondary {
        border-color: #e5e7eb;
        color: #374151;
      }

      .btn-outline-secondary:hover {
        background-color: #f9fafb;
        border-color: #d1d5db;
        color: #111827;
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
export class VerificationComponent {
  interviewers: Interviewer[] = [
    {
      name: 'John Smith',
      personalEmail: 'john@example.com',
      workEmail: 'john@company.com',
      submittedDate: '2023-07-10',
      idCard: 'Uploaded',
      workPermit: 'Uploaded',
    },
    {
      name: 'Emma Watson',
      personalEmail: 'emma@example.com',
      workEmail: 'emma@company.com',
      submittedDate: '2023-07-11',
      idCard: 'Uploaded',
      workPermit: 'Missing',
    },
    {
      name: 'Michael Brown',
      personalEmail: 'michael@example.com',
      workEmail: 'michael@company.com',
      submittedDate: '2023-07-12',
      idCard: 'Uploaded',
      workPermit: 'Uploaded',
    },
  ];

  currentPage = 1;
  pageSize = 10;

  showReviewModal = false;
  showRejectModal = false;
  selectedInterviewer: Interviewer | null = null;
  rejectionReason = '';

  get paginatedInterviewers(): Interviewer[] {
    const start = (this.currentPage - 1) * this.pageSize;
    const end = start + this.pageSize;
    return this.interviewers.slice(start, end);
  }

  getStatusClass(status: string): string {
    const baseClass = 'status-badge';
    return `${baseClass} status-${status.toLowerCase()}`;
  }

  openReviewModal(interviewer: Interviewer) {
    this.selectedInterviewer = interviewer;
    this.showReviewModal = true;
  }

  openRejectModal(interviewer: Interviewer) {
    this.selectedInterviewer = interviewer;
    this.showRejectModal = true;
  }

  closeReviewModal() {
    this.showReviewModal = false;
    this.selectedInterviewer = null;
  }

  closeRejectModal() {
    this.showRejectModal = false;
    this.selectedInterviewer = null;
    this.rejectionReason = '';
  }

  closeAllModals() {
    this.closeReviewModal();
    this.closeRejectModal();
  }

  approveInterviewer() {
    console.log('Approved interviewer:', this.selectedInterviewer);
    this.closeReviewModal();
  }

  confirmReject() {
    console.log('Rejected interviewer:', this.selectedInterviewer);
    console.log('Reason:', this.rejectionReason);
    this.closeRejectModal();
  }
  onPageChange(page: number): void {
    this.currentPage = page;
  }
}
