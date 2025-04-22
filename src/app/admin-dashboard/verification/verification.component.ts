// import { Component } from '@angular/core';
// import { CommonModule } from '@angular/common';
// import { FormsModule } from '@angular/forms';
// import { SidebarComponent } from '../sidebar/sidebar.component';
// import { PaginationComponent } from '../pagination/pagination.component';

// interface Interviewer {
//   name: string;
//   personalEmail: string;
//   workEmail: string;
//   submittedDate: string;
//   idCard: 'Uploaded' | 'Missing';
//   workPermit: 'Uploaded' | 'Missing';
// }

// @Component({
//   selector: 'app-verification',
//   standalone: true,
//   imports: [CommonModule, FormsModule, SidebarComponent, PaginationComponent],
//   template: `
//     <div class="d-flex">
//       <app-sidebar></app-sidebar>

//       <main class="main-content bg-light">
//         <div class="p-4">
//           <h1 class="h4 mb-4">Interviewer Verification</h1>

//           <div class="card border-0 shadow-sm">
//             <div class="table-responsive">
//               <table class="table table-hover mb-0">
//                 <thead class="bg-light">
//                   <tr>
//                     <th class="border-0 px-4 py-3">Name</th>
//                     <th class="border-0 px-4 py-3">Personal Email</th>
//                     <th class="border-0 px-4 py-3">Work Email</th>
//                     <th class="border-0 px-4 py-3">Submitted Date</th>
//                     <th class="border-0 px-4 py-3">ID Card</th>
//                     <th class="border-0 px-4 py-3">Work Permit</th>
//                     <th class="border-0 px-4 py-3">Actions</th>
//                   </tr>
//                 </thead>
//                 <tbody>
//                   <tr *ngFor="let interviewer of paginatedInterviewers">
//                     <td class="px-4 py-3">{{ interviewer.name }}</td>
//                     <td class="px-4 py-3">{{ interviewer.personalEmail }}</td>
//                     <td class="px-4 py-3">{{ interviewer.workEmail }}</td>
//                     <td class="px-4 py-3">{{ interviewer.submittedDate }}</td>
//                     <td class="px-4 py-3">
//                       <span [class]="getStatusClass(interviewer.idCard)">
//                         {{ interviewer.idCard }}
//                       </span>
//                     </td>
//                     <td class="px-4 py-3">
//                       <span [class]="getStatusClass(interviewer.workPermit)">
//                         {{ interviewer.workPermit }}
//                       </span>
//                     </td>
//                     <td class="px-4 py-3">
//                       <div class="d-flex gap-2">
//                         <button
//                           class="btn btn-link p-0 text-decoration-none"
//                           (click)="openReviewModal(interviewer)"
//                         >
//                           Review
//                         </button>
//                         <button
//                           class="btn btn-link p-0 text-decoration-none text-danger"
//                           (click)="openRejectModal(interviewer)"
//                         >
//                           Reject
//                         </button>
//                       </div>
//                     </td>
//                   </tr>
//                 </tbody>
//               </table>
//               <app-pagination
//                 [currentPage]="currentPage"
//                 [pageSize]="pageSize"
//                 [totalItems]="interviewers.length"
//                 (pageChange)="onPageChange($event)"
//               ></app-pagination>
//             </div>
//           </div>
//         </div>
//       </main>

//       <!-- Review Modal -->
//       <div
//         class="modal"
//         [class.show]="showReviewModal"
//         [style.display]="showReviewModal ? 'block' : 'none'"
//       >
//         <div class="modal-dialog modal-dialog-centered">
//           <div class="modal-content border-0">
//             <div class="modal-header border-0 pb-0">
//               <h5 class="modal-title">
//                 Review Interviewer: {{ selectedInterviewer?.name }}
//               </h5>
//               <button
//                 type="button"
//                 class="btn-close"
//                 (click)="closeReviewModal()"
//               ></button>
//             </div>
//             <div class="modal-body pt-2">
//               <p class="text-muted small mb-4">
//                 Review the interviewer's details and uploaded documents.
//               </p>

//               <div class="mb-3">
//                 <label class="form-label">Name</label>
//                 <input
//                   type="text"
//                   class="form-control"
//                   [value]="selectedInterviewer?.name"
//                   readonly
//                 />
//               </div>

//               <div class="mb-3">
//                 <label class="form-label">Work Email</label>
//                 <input
//                   type="email"
//                   class="form-control"
//                   [value]="selectedInterviewer?.workEmail"
//                   readonly
//                 />
//               </div>

//               <div class="mb-3">
//                 <label class="form-label">ID Card</label>
//                 <button class="btn btn-outline-secondary d-block">
//                   View ID Card
//                 </button>
//               </div>

//               <div class="mb-3">
//                 <label class="form-label">Work Permit</label>
//                 <button class="btn btn-outline-secondary d-block">
//                   View Work Permit
//                 </button>
//               </div>
//             </div>
//             <div class="modal-footer border-0">
//               <button
//                 type="button"
//                 class="btn btn-dark px-4"
//                 (click)="approveInterviewer()"
//               >
//                 Approve
//               </button>
//             </div>
//           </div>
//         </div>
//       </div>

//       <!-- Reject Modal -->
//       <div
//         class="modal"
//         [class.show]="showRejectModal"
//         [style.display]="showRejectModal ? 'block' : 'none'"
//       >
//         <div class="modal-dialog modal-dialog-centered">
//           <div class="modal-content border-0">
//             <div class="modal-header border-0 pb-0">
//               <h5 class="modal-title">
//                 Reject Interviewer: {{ selectedInterviewer?.name }}
//               </h5>
//               <button
//                 type="button"
//                 class="btn-close"
//                 (click)="closeRejectModal()"
//               ></button>
//             </div>
//             <div class="modal-body pt-2">
//               <p class="text-muted small mb-4">
//                 Provide a reason for rejecting this interviewer.
//               </p>

//               <div class="mb-3">
//                 <label class="form-label">Reason</label>
//                 <textarea
//                   class="form-control"
//                   rows="4"
//                   [(ngModel)]="rejectionReason"
//                   placeholder="Enter rejection reason"
//                 >
//                 </textarea>
//               </div>
//             </div>
//             <div class="modal-footer border-0">
//               <button
//                 type="button"
//                 class="btn btn-danger px-4"
//                 [disabled]="!rejectionReason"
//                 (click)="confirmReject()"
//               >
//                 Confirm Rejection
//               </button>
//             </div>
//           </div>
//         </div>
//       </div>

//       <!-- Modal Backdrop -->
//       <div
//         class="modal-backdrop fade show"
//         *ngIf="showReviewModal || showRejectModal"
//         (click)="closeAllModals()"
//       ></div>
//     </div>
//   `,
//   styles: [
//     `
//       :host {
//         display: block;
//         min-height: 100vh;
//         background-color: #f9fafb;
//       }

//       .main-content {
//         margin-left: 240px;
//         width: calc(100% - 240px);
//         min-height: 100vh;
//       }

//       .card {
//         border-radius: 8px;
//       }

//       .table {
//         margin-bottom: 0;
//       }

//       .table th {
//         font-weight: 500;
//         font-size: 14px;
//         color: #6b7280;
//       }

//       .table td {
//         font-size: 14px;
//         vertical-align: middle;
//       }

//       .status-badge {
//         padding: 4px 8px;
//         border-radius: 4px;
//         font-size: 13px;
//         font-weight: 500;
//       }

//       .status-uploaded {
//         background-color: #ecfdf5;
//         color: #059669;
//       }

//       .status-missing {
//         background-color: #fee2e2;
//         color: #dc2626;
//       }

//       .btn-link {
//         font-size: 14px;
//       }

//       .modal-content {
//         border-radius: 12px;
//       }

//       .modal-header {
//         padding: 1.5rem 1.5rem 0.5rem;
//       }

//       .modal-body {
//         padding: 1rem 1.5rem;
//       }

//       .modal-footer {
//         padding: 1rem 1.5rem 1.5rem;
//       }

//       .form-control {
//         border-radius: 6px;
//         padding: 0.5rem 0.75rem;
//       }

//       .form-control:read-only {
//         background-color: #f9fafb;
//       }

//       .btn-outline-secondary {
//         border-color: #e5e7eb;
//         color: #374151;
//       }

//       .btn-outline-secondary:hover {
//         background-color: #f9fafb;
//         border-color: #d1d5db;
//         color: #111827;
//       }

//       @media (max-width: 991.98px) {
//         .main-content {
//           margin-left: 0;
//           width: 100%;
//         }
//       }
//     `,
//   ],
// })
// export class VerificationComponent {
//   interviewers: Interviewer[] = [
//     {
//       name: 'John Smith',
//       personalEmail: 'john@example.com',
//       workEmail: 'john@company.com',
//       submittedDate: '2023-07-10',
//       idCard: 'Uploaded',
//       workPermit: 'Uploaded',
//     },
//     {
//       name: 'Emma Watson',
//       personalEmail: 'emma@example.com',
//       workEmail: 'emma@company.com',
//       submittedDate: '2023-07-11',
//       idCard: 'Uploaded',
//       workPermit: 'Missing',
//     },
//     {
//       name: 'Michael Brown',
//       personalEmail: 'michael@example.com',
//       workEmail: 'michael@company.com',
//       submittedDate: '2023-07-12',
//       idCard: 'Uploaded',
//       workPermit: 'Uploaded',
//     },
//   ];

//   currentPage = 1;
//   pageSize = 10;

//   showReviewModal = false;
//   showRejectModal = false;
//   selectedInterviewer: Interviewer | null = null;
//   rejectionReason = '';

//   get paginatedInterviewers(): Interviewer[] {
//     const start = (this.currentPage - 1) * this.pageSize;
//     const end = start + this.pageSize;
//     return this.interviewers.slice(start, end);
//   }

//   getStatusClass(status: string): string {
//     const baseClass = 'status-badge';
//     return `${baseClass} status-${status.toLowerCase()}`;
//   }

//   openReviewModal(interviewer: Interviewer) {
//     this.selectedInterviewer = interviewer;
//     this.showReviewModal = true;
//   }

//   openRejectModal(interviewer: Interviewer) {
//     this.selectedInterviewer = interviewer;
//     this.showRejectModal = true;
//   }

//   closeReviewModal() {
//     this.showReviewModal = false;
//     this.selectedInterviewer = null;
//   }

//   closeRejectModal() {
//     this.showRejectModal = false;
//     this.selectedInterviewer = null;
//     this.rejectionReason = '';
//   }

//   closeAllModals() {
//     this.closeReviewModal();
//     this.closeRejectModal();
//   }

//   approveInterviewer() {
//     console.log('Approved interviewer:', this.selectedInterviewer);
//     this.closeReviewModal();
//   }

//   confirmReject() {
//     console.log('Rejected interviewer:', this.selectedInterviewer);
//     console.log('Reason:', this.rejectionReason);
//     this.closeRejectModal();
//   }
//   onPageChange(page: number): void {
//     this.currentPage = page;
//   }
// }





import { Component, type OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import  { DomSanitizer, SafeResourceUrl } from '@angular/platform-browser';
import { forkJoin, of } from 'rxjs';
import { catchError, finalize, map } from 'rxjs/operators';
import  {
  VerificationService,
  InterviewerVerification,
} from '../services/verification.service';
import { SidebarComponent } from '../sidebar/sidebar.component';
import { PaginationComponent } from '../pagination/pagination.component';

@Component({
  selector: 'app-verification',
  standalone: true,
  imports: [CommonModule, FormsModule, SidebarComponent, PaginationComponent],
  template: `
    <div class="d-flex">
      <app-sidebar></app-sidebar>

      <main class="main-content bg-light">
        <div class="p-4">
          <div class="d-flex justify-content-between align-items-center mb-4">
            <div>
              <h1 class="h4 mb-1">Interviewer Verification</h1>
              <p class="text-muted mb-0">
                Review and approve interviewer applications
              </p>
            </div>
            <div class="d-flex gap-2">
              <div class="dropdown">
                <button
                  class="btn btn-outline-secondary dropdown-toggle"
                  type="button"
                  id="filterDropdown"
                  data-bs-toggle="dropdown"
                  aria-expanded="false"
                >
                  Filter: {{ currentFilter }}
                </button>
                <ul class="dropdown-menu" aria-labelledby="filterDropdown">
                  <li>
                    <a class="dropdown-item" (click)="setFilter('All')">All</a>
                  </li>
                  <li>
                    <a class="dropdown-item" (click)="setFilter('Pending')"
                      >Pending</a
                    >
                  </li>
                  <li>
                    <a class="dropdown-item" (click)="setFilter('Approved')"
                      >Approved</a
                    >
                  </li>
                  <li>
                    <a class="dropdown-item" (click)="setFilter('Rejected')"
                      >Rejected</a
                    >
                  </li>
                </ul>
              </div>
              <div class="input-group">
                <input
                  type="text"
                  class="form-control"
                  placeholder="Search interviewers..."
                  [(ngModel)]="searchTerm"
                  (input)="onSearch()"
                />
                <span class="input-group-text bg-white">
                  <i class="bi bi-search"></i>
                </span>
              </div>
            </div>
          </div>

          <div class="card border-0 shadow-sm">
            <div *ngIf="isLoading" class="text-center p-5">
              <div class="spinner-border text-primary" role="status">
                <span class="visually-hidden">Loading...</span>
              </div>
            </div>

            <div
              *ngIf="!isLoading && paginatedInterviewers.length === 0"
              class="text-center p-5"
            >
              <div class="empty-state">
                <i class="bi bi-person-x fs-1 text-muted mb-3"></i>
                <h5>No interviewers found</h5>
                <p class="text-muted">
                  There are no interviewers pending verification at this time.
                </p>
              </div>
            </div>

            <div
              *ngIf="!isLoading && paginatedInterviewers.length > 0"
              class="table-responsive"
            >
              <table class="table table-hover mb-0">
                <thead class="bg-light">
                  <tr>
                    <th class="border-0 px-4 py-3">Name</th>
                    <th class="border-0 px-4 py-3">Email</th>
                    <th class="border-0 px-4 py-3">Submitted Date</th>
                    <th class="border-0 px-4 py-3">CV</th>
                    <th class="border-0 px-4 py-3">Status</th>
                    <th class="border-0 px-4 py-3">Actions</th>
                  </tr>
                </thead>
                <tbody>
                  <tr *ngFor="let interviewer of paginatedInterviewers">
                    <td class="px-4 py-3">{{ interviewer.name || 'N/A' }}</td>
                    <td class="px-4 py-3">
                      {{
                        interviewer.personalEmail ||
                          interviewer.workEmail ||
                          'N/A'
                      }}
                    </td>
                    <td class="px-4 py-3">
                      {{ formatDate(interviewer.created) }}
                    </td>
                    <td class="px-4 py-3">
                      <span [class]="getDocumentStatusClass(interviewer.cv)">
                        {{ getDocumentStatus(interviewer.cv) }}
                      </span>
                    </td>
                    <td class="px-4 py-3">
                      <span [class]="getStatusClass(interviewer.status)">
                        {{ interviewer.status | titlecase }}
                      </span>
                    </td>
                    <td class="px-4 py-3">
                      <div class="d-flex gap-2">
                        <button
                          class="btn btn-link p-0 text-decoration-none"
                          (click)="openReviewModal(interviewer)"
                          [disabled]="interviewer.status !== 'pending'"
                        >
                          Review
                        </button>
                        <button
                          *ngIf="interviewer.status === 'pending'"
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
              <div class="p-3">
                <app-pagination
                  [currentPage]="currentPage"
                  [pageSize]="pageSize"
                  [totalItems]="totalCount"
                  (pageChange)="onPageChange($event)"
                ></app-pagination>
              </div>
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
        <div class="modal-dialog modal-dialog-centered modal-lg">
          <div class="modal-content border-0">
            <div class="modal-header border-0 pb-0">
              <h5 class="modal-title">Review Interviewer Application</h5>
              <button
                type="button"
                class="btn-close"
                (click)="closeReviewModal()"
              ></button>
            </div>
            <div class="modal-body pt-2">
              <p class="text-muted small mb-4">
                Review the interviewer's details and uploaded documents before
                making a decision.
              </p>

              <div class="row mb-4">
                <div class="col-md-6 mb-3">
                  <label class="form-label">Name</label>
                  <input
                    type="text"
                    class="form-control"
                    [value]="selectedInterviewer?.name || 'Not provided'"
                    readonly
                  />
                </div>
                <div class="col-md-6 mb-3">
                  <label class="form-label">Email</label>
                  <input
                    type="email"
                    class="form-control"
                    [value]="
                      selectedInterviewer?.personalEmail ||
                      selectedInterviewer?.workEmail ||
                      'Not provided'
                    "
                    readonly
                  />
                </div>
              </div>

              <div class="mb-4">
                <label class="form-label">Additional Information</label>
                <textarea
                  class="form-control"
                  rows="3"
                  [value]="
                    selectedInterviewer?.additionalInfo ||
                    'No additional information provided.'
                  "
                  readonly
                ></textarea>
              </div>

              <div class="row mb-6">
                <div class="col-md-6 mb-6">
                  <label class="form-label">CV</label>
                  <div class="d-grid">
                    <button
                      class="btn btn-outline-secondary"
                      (click)="viewDocument(null, 'CV')"
                    >
                      <i class="bi bi-file-earmark-pdf me-2"></i>
                      View CV
                    </button>
                  </div>
                </div>
               
              </div>

              <div class="mb-3">
                <label class="form-label">Notes (Optional)</label>
                <textarea
                  class="form-control"
                  rows="3"
                  [(ngModel)]="approvalNotes"
                  placeholder="Add any notes about this application..."
                ></textarea>
              </div>
            </div>
            <div class="modal-footer border-0">
              <button
                type="button"
                class="btn btn-light"
                (click)="closeReviewModal()"
              >
                Cancel
              </button>
              <button
                type="button"
                class="btn btn-danger me-2"
                (click)="openRejectModal(selectedInterviewer)"
              >
                Reject
              </button>
              <button
                type="button"
                class="btn btn-success"
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
              <h5 class="modal-title">Reject Interviewer Application</h5>
              <button
                type="button"
                class="btn-close"
                (click)="closeRejectModal()"
              ></button>
            </div>

            <div class="modal-footer border-0">
              <button
                type="button"
                class="btn btn-light"
                (click)="closeRejectModal()"
              >
                Cancel
              </button>
              <button
                type="button"
                class="btn btn-danger"
                (click)="confirmReject()"
              >
                Confirm Rejection
              </button>
            </div>
          </div>
        </div>
      </div>

      <!-- Document Viewer Modal -->
      <div
        class="modal"
        [class.show]="showDocumentModal"
        [style.display]="showDocumentModal ? 'block' : 'none'"
      >
        <div class="modal-dialog modal-dialog-centered modal-lg">
          <div class="modal-content border-0">
            <div class="modal-header border-0">
              <h5 class="modal-title">
                {{ documentTitle }}
              </h5>
              <button
                type="button"
                class="btn-close"
                (click)="closeDocumentModal()"
              ></button>
            </div>
            <div class="modal-body p-0">
              <div *ngIf="isLoadingDocument" class="text-center p-5">
                <div class="spinner-border text-primary" role="status">
                  <span class="visually-hidden">Loading document...</span>
                </div>
              </div>

              <div
                *ngIf="!isLoadingDocument && documentUrl"
                class="document-viewer"
              >
                <object
                  [data]="documentUrl"
                  type="application/pdf"
                  width="100%"
                  height="600"
                  class="w-100"
                >
                  <div class="text-center p-5">
                    <p>
                      It appears your browser doesn't support embedded PDFs.
                    </p>
                    <a
                      [href]="documentUrl"
                      target="_blank"
                      class="btn btn-primary"
                    >
                      <i class="bi bi-box-arrow-up-right me-2"></i>
                      Open PDF in new tab
                    </a>
                  </div>
                </object>
              </div>

              <div
                *ngIf="!isLoadingDocument && !documentUrl"
                class="text-center p-5"
              >
                <div class="empty-state">
                  <i class="bi bi-file-earmark-x fs-1 text-muted mb-3"></i>
                  <h5>Document Not Available</h5>
                  <p class="text-muted">
                    The requested document could not be loaded.
                  </p>
                </div>
              </div>
            </div>
            <div class="modal-footer border-0">
              <button
                type="button"
                class="btn btn-light"
                (click)="closeDocumentModal()"
              >
                Close
              </button>
              <a
                *ngIf="documentUrl"
                [href]="documentUrl"
                download
                class="btn btn-primary"
              >
                <i class="bi bi-download me-2"></i>
                Download
              </a>
            </div>
          </div>
        </div>
      </div>

      <!-- Modal Backdrop -->
      <div
        class="modal-backdrop fade show"
        *ngIf="showReviewModal || showRejectModal || showDocumentModal"
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
        border-radius: 12px;
        overflow: hidden;
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

      .status-pending {
        background-color: #fff7ed;
        color: #c2410c;
      }

      .status-approved {
        background-color: #ecfdf5;
        color: #059669;
      }

      .status-rejected {
        background-color: #fee2e2;
        color: #dc2626;
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

      .btn-link:disabled {
        color: #9ca3af;
        pointer-events: none;
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

      .btn-outline-secondary:hover:not(:disabled) {
        background-color: #f9fafb;
        border-color: #d1d5db;
        color: #111827;
      }

      .btn-outline-secondary:disabled {
        color: #9ca3af;
        background-color: #f3f4f6;
      }

      .document-viewer {
        width: 100%;
        height: 600px;
        overflow: hidden;
      }

      .empty-state {
        display: flex;
        flex-direction: column;
        align-items: center;
        padding: 2rem;
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
export class VerificationComponent implements OnInit {
  interviewers: any[] = [];
  filteredInterviewers: any[] = [];
  paginatedInterviewers: any[] = [];

  currentPage = 1;
  pageSize = 10;
  totalCount = 0;
  totalPages = 1;

  isLoading = true;
  isLoadingDocument = false;

  searchTerm = '';
  currentFilter = 'All';

  showReviewModal = false;
  showRejectModal = false;
  showDocumentModal = false;

  selectedInterviewer: InterviewerVerification | null = null;
  rejectionReason = '';
  approvalNotes = '';
  showRejectionError = false;

  documentUrl: SafeResourceUrl | null = null;
  documentTitle = '';

  constructor(
    private verificationService: VerificationService,
    private sanitizer: DomSanitizer
  ) {}

  ngOnInit(): void {
    this.loadInterviewers();
  }

  formatDate(date: any) {
    if (!date) return 'N/A';
    return new Date(date).toLocaleString('en-US', {
      dateStyle: 'medium',
      timeStyle: 'short',
    });
  }

  // Update the loadInterviewers method to fetch user details for each interviewer
  loadInterviewers(): void {
    this.isLoading = true;

    const params = {
      pageNumber: this.currentPage,
      pageSize: this.pageSize,
    };

    this.verificationService.getVerificationList(params).subscribe({
      next: (response) => {
        // Store the basic interviewer data
        const interviewers = response.items;
        this.totalCount = response.totalCount;
        this.totalPages = response.totalPages;

        // If there are no interviewers, we can skip the profile fetching
        if (interviewers.length === 0) {
          this.interviewers = [];
          this.filteredInterviewers = [];
          this.paginatedInterviewers = [];
          this.isLoading = false;
          return;
        }

        // Create an array of observables for fetching user profiles
        const profileRequests = interviewers.map((interviewer: any) =>
          this.verificationService.getUserProfile(interviewer.userId).pipe(
            // Map the profile data to the interviewer object
            map((profile: any) => ({
              ...interviewer,
              name: profile.userName,
              personalEmail: profile.email,
            })),
            // Handle errors for individual profile requests
            catchError((error) => {
              console.error(
                `Error fetching profile for user ${interviewer.userId}:`,
                error
              );
              // Return the original interviewer object without profile data
              return of(interviewer);
            })
          )
        );

        // Execute all profile requests in parallel
        forkJoin(profileRequests)
          .pipe(
            finalize(() => {
              this.isLoading = false;
            })
          )
          .subscribe({
            next: (enrichedInterviewers: any) => {
              this.interviewers = enrichedInterviewers;
              this.filteredInterviewers = [...this.interviewers];
              this.paginatedInterviewers = [...this.filteredInterviewers];
            },
            error: (error) => {
              console.error('Error processing interviewer profiles:', error);
              // Use the basic interviewer data without profiles
              this.interviewers = interviewers;
              this.filteredInterviewers = [...this.interviewers];
              this.paginatedInterviewers = [...this.filteredInterviewers];
            },
          });
      },
      error: (error) => {
        console.error('Error loading interviewers:', error);
        this.isLoading = false;
        // Initialize empty arrays in case of error
        this.interviewers = [];
        this.filteredInterviewers = [];
        this.paginatedInterviewers = [];
      },
    });
  }

  onPageChange(page: number): void {
    this.currentPage = page;
    this.loadInterviewers();
  }

  onSearch(): void {
    this.currentPage = 1;
    this.loadInterviewers();
  }

  setFilter(filter: string): void {
    this.currentFilter = filter;
    this.currentPage = 1;
    this.loadInterviewers();
  }

  getStatusClass(status: string): string {
    return `status-badge status-${status.toLowerCase()}`;
  }

  getDocumentStatus(document: string | null): string {
    return document ? 'Uploaded' : 'Missing';
  }

  getDocumentStatusClass(document: string | null): string {
    return `status-badge status-${document ? 'uploaded' : 'missing'}`;
  }

  openReviewModal(interviewer: any): void {
    this.selectedInterviewer = interviewer;
    this.approvalNotes = '';
    this.showReviewModal = true;
  }

  openRejectModal(interviewer: InterviewerVerification | null): void {
    if (interviewer) {
      this.selectedInterviewer = interviewer;
    }
    this.rejectionReason = '';
    this.showRejectionError = false;
    this.showRejectModal = true;

    // Close the review modal if it's open
    if (this.showReviewModal) {
      this.showReviewModal = false;
    }
  }

  closeReviewModal(): void {
    this.showReviewModal = false;
    this.selectedInterviewer = null;
    this.approvalNotes = '';
  }

  closeRejectModal(): void {
    this.showRejectModal = false;
    this.rejectionReason = '';
    this.showRejectionError = false;

    // If we came from the review modal, go back to it
    if (
      this.selectedInterviewer &&
      this.selectedInterviewer.status === 'pending'
    ) {
      this.showReviewModal = true;
    } else {
      this.selectedInterviewer = null;
    }
  }

  closeDocumentModal(): void {
    this.showDocumentModal = false;
    this.documentUrl = null;
    this.documentTitle = '';
  }

  closeAllModals(): void {
    this.closeReviewModal();
    this.closeRejectModal();
    this.closeDocumentModal();
  }

  approveInterviewer(): void {
    if (!this.selectedInterviewer) return;

    const interviewerId = this.selectedInterviewer.id;
    console.log(interviewerId);

    this.verificationService.approveVerification(interviewerId).subscribe({
      next: (response) => {
        // Update the interviewer in the local array
        const index = this.interviewers.findIndex(
          (i) => i.id === interviewerId
        );
        if (index !== -1) {
          this.interviewers[index] = {
            ...this.interviewers[index],
            ...response,
          };
        }

        // Reload the interviewers to reflect the changes
        this.loadInterviewers();
        this.closeReviewModal();
      },
      error: (error) => {
        console.error('Error approving interviewer:', error);
        // Show error message to user
        alert('Failed to approve interviewer. Please try again.');
      },
    });
  }

  confirmReject(): void {
    if (!this.selectedInterviewer) return;
    // if (!this.rejectionReason.trim()) {
    //   this.showRejectionError = true;
    //   return;
    // }
    const interviewerId = this.selectedInterviewer.id;
    this.verificationService
      .rejectVerification(interviewerId)
      .subscribe({
        next: (response) => {
          // Update the interviewer in the local array
          const index = this.interviewers.findIndex(
            (i) => i.id === interviewerId
          );
          if (index !== -1) {
            this.interviewers[index] = {
              ...this.interviewers[index],
              ...response,
            };
          }
          // Reload the interviewers to reflect the changes
          this.loadInterviewers();
          this.closeRejectModal();
        },
        error: (error) => {
          console.error('Error rejecting interviewer:', error);
          // Show error message to user
          alert('Failed to reject interviewer. Please try again.');
        },
      });
  }

  // Update the viewDocument method to ignore the document path and just use the interviewer ID and document type
  viewDocument(
    documentPath: string | null | undefined,
    documentType: string
  ): void {
    if (!this.selectedInterviewer) return;

    this.documentTitle = `${documentType} Document`;
    this.showDocumentModal = true;
    this.isLoadingDocument = true;
    this.documentUrl = null;

    // Simplify to just use the document type directly
    const docType =
      documentType.toLowerCase() === 'cv'
        ? 'cv'
        : documentType.toLowerCase() === 'id card'
        ? 'id'
        : documentType.toLowerCase() === 'work permit'
        ? 'permit'
        : 'cv';

    console.log('Viewing document:', {
      interviewerId: this.selectedInterviewer.id,
      documentType: docType,
    });

    // Call the service method with the interviewer ID and document type
    this.verificationService
      .getVerificationDocument(this.selectedInterviewer.id, docType)
      .subscribe({
        next: (blob) => {
          console.log('Document loaded successfully', blob);
          // Create a blob URL for the document
          const url = URL.createObjectURL(blob);
          // Sanitize the URL to prevent XSS attacks
          this.documentUrl = this.sanitizer.bypassSecurityTrustResourceUrl(url);
          this.isLoadingDocument = false;
        },
        error: (error) => {
          console.error('Error loading document:', error);
          this.isLoadingDocument = false;
          // Show error message to user
          alert('Failed to load document. Please try again.');
        },
      });
  }
}
