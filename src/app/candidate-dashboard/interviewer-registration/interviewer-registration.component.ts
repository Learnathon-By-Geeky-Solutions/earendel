import { Component, type OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import  { DomSanitizer } from '@angular/platform-browser';
import { FormsModule } from '@angular/forms';
import  { Router } from '@angular/router';
import { CandidateSidebarComponent } from '../candidate-sidebar/candidate-sidebar.component';
import  { VerificationService } from '../../admin-dashboard/services/verification.service';
import { PaginationComponent } from '../../admin-dashboard/pagination/pagination.component';

interface LoggedInUser {
  id: string;
  name: string;
  email: string;
  roles: string[];
  userId: string;
}

interface VerificationApplication {
  id: string;
  userId: string;
  submittedDate: string;
  status: 'pending' | 'approved' | 'rejected';
  additionalInfo: string;
  cv: string | null;
  workPermit: string | null;
  idCard: string | null;
  rejectionReason?: string | null;
}

@Component({
  selector: 'app-interviewer-registration',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    CandidateSidebarComponent,
    PaginationComponent,
  ],
  template: `
    <div class="dashboard-container">
      <main class="main-content">
        <!-- Loading State -->
        <div
          *ngIf="isLoading"
          class="d-flex justify-content-center align-items-center"
          style="height: 400px;"
        >
          <div class="spinner-border text-primary" role="status">
            <span class="visually-hidden">Loading...</span>
          </div>
        </div>

        <!-- Error State -->
        <div
          *ngIf="error"
          class="alert alert-danger mx-auto mt-4"
          style="max-width: 600px;"
        >
          {{ error }}
        </div>

        <!-- Registration Form -->
        <div
          *ngIf="!isLoading && showRegistrationForm && !hasPendingOrApproved"
          class="registration-container"
        >
          <div class="d-flex justify-content-between align-items-center mb-4">
            <h1>Become an Interviewer</h1>
            <button
              class="btn btn-outline-secondary"
              (click)="showRegistrationForm = false"
            >
              <i class="bi bi-arrow-left me-2"></i>
              Back to Applications
            </button>
          </div>

          <p class="subtitle">
            Please provide the following information and documents to complete
            your registration.
          </p>

          <!-- Previous Applications Alert -->
          <div
            *ngIf="previousApplications.length > 0"
            class="alert alert-info mb-4"
          >
            <i class="bi bi-info-circle me-2"></i>
            You have {{ getRejectedApplicationsCount() }} rejected
            application(s). Please review the feedback and submit a new
            application.
          </div>

          <form
            (ngSubmit)="onSubmit()"
            #registrationForm="ngForm"
            class="registration-form"
          >
            <!-- Additional Info Field -->
            <div class="form-group">
              <label for="additionalInfo">Additional Information</label>
              <textarea
                id="additionalInfo"
                name="additionalInfo"
                [(ngModel)]="additionalInfo"
                class="form-control"
                rows="4"
                placeholder="Tell us about your experience, skills, and why you want to be an interviewer"
                required
              ></textarea>
            </div>

            <div class="form-group">
              <label for="resume">Resume/CV</label>
              <div class="file-input-container">
                <input
                  type="file"
                  id="resume"
                  (change)="onFileSelected($event, 'resume')"
                  accept=".pdf,.doc,.docx"
                  required
                />
                <label for="resume" class="file-input-label">
                  <i class="bi bi-cloud-upload"></i>
                  <span>{{ files['resume']?.name || 'Choose file' }}</span>
                </label>
              </div>
            </div>

            <div class="d-flex gap-2 mt-3">
              <button
                type="button"
                class="btn btn-light"
                (click)="showRegistrationForm = false"
              >
                Cancel
              </button>
              <button
                type="submit"
                class="submit-btn"
                [disabled]="!registrationForm.form.valid || isSubmitting"
              >
                <span *ngIf="!isSubmitting">Submit Application</span>
                <span *ngIf="isSubmitting">
                  <span
                    class="spinner-border spinner-border-sm me-2"
                    role="status"
                    aria-hidden="true"
                  ></span>
                  Submitting...
                </span>
              </button>
            </div>
          </form>
        </div>

        <!-- Previous Applications List -->
        <div
          *ngIf="!isLoading && !showRegistrationForm"
          class="applications-container"
        >
          <div class="d-flex justify-content-between align-items-center mb-4">
            <h5>Applications</h5>
            <button
              *ngIf="!hasPendingOrApproved"
              class="btn btn-primary"
              (click)="showRegistrationForm = true"
            >
              <i class="bi bi-plus-lg me-2"></i>
              New Application
            </button>
            <div
              *ngIf="hasPendingOrApproved"
              class="alert alert-info mb-0 py-2"
            >
              <i class="bi bi-info-circle me-2"></i>
              You application have been proceed.
            </div>
          </div>

          <div
            *ngIf="previousApplications.length === 0"
            class="text-center p-5"
          >
            <div class="empty-state">
              <i class="bi bi-clipboard-x fs-1 text-muted mb-3"></i>
              <h5>No Applications Found</h5>
              <p class="text-muted">
                You haven't submitted any interviewer applications yet.
              </p>
              <button
                *ngIf="!hasPendingOrApproved"
                class="btn btn-primary mt-3"
                (click)="showRegistrationForm = true"
              >
                <i class="bi bi-plus-lg me-2"></i>
                Submit Your First Application
              </button>
            </div>
          </div>

          <div
            class="card mb-4"
            *ngFor="let application of previousApplications"
          >
            <div
              class="card-header d-flex justify-content-between align-items-center"
            >
              <div>
                <span class="fw-bold">Application ID: </span>
                <span class="text-muted">{{ application.id }}</span>
              </div>
              <span [class]="getStatusBadgeClass(application.status)">
                {{ application.status | titlecase }}
              </span>
            </div>
            <div class="card-body">
              <div class="mb-3">
                <p class="mb-1">
                  <span class="fw-bold">Submitted Date:</span>
                  {{ application.submittedDate | date : 'medium' }}
                </p>
              </div>

              <div class="mb-3">
                <h6 class="card-subtitle mb-2">Additional Information</h6>
                <p class="card-text">{{ application.additionalInfo }}</p>
              </div>

              <div class="mb-3">
                <h6 class="card-subtitle mb-2">Documents</h6>
                <div class="d-flex flex-wrap gap-2">
                  <button
                    class="btn btn-sm btn-outline-secondary"
                    [disabled]="!application.cv"
                    (click)="viewDocument(application.id, 'cv')"
                  >
                    <i class="bi bi-file-earmark-pdf me-1"></i> CV
                  </button>
                </div>
              </div>

              <div
                *ngIf="application.status === 'rejected'"
                class="alert alert-danger"
              >
                <i class="bi bi-exclamation-triangle me-2"></i>
                <span class="fw-bold">Rejection Reason:</span>
                <span>{{
                  application.rejectionReason || 'No reason provided'
                }}</span>
              </div>
            </div>
          </div>
          <div class="p-3">
            <app-pagination
              [currentPage]="currentPage"
              [pageSize]="pageSize"
              [totalItems]="totalCount"
              (pageChange)="onPageChange($event)"
            ></app-pagination>
          </div>
        </div>
      </main>

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
        *ngIf="showDocumentModal"
        (click)="closeDocumentModal()"
      ></div>
    </div>
  `,
  styles: [
    `
      .dashboard-container {
        display: flex;
        min-height: 100vh;
        background-color: #f8f9fa;
      }

      .main-content {
        flex: 1;
        padding: 2rem;
        transition: margin-left 0.3s ease-in-out;
      }

      .registration-container,
      .applications-container {
        max-width: 800px;
        margin: 0 auto;
        background-color: #ffffff;
        border-radius: 8px;
        padding: 2rem;
        box-shadow: 0 4px 6px rgba(0, 0, 0, 0.1);
      }

      h1 {
        font-size: 24px;
        font-weight: 600;
        color: #333;
        margin-bottom: 0.5rem;
      }

      .subtitle {
        font-size: 14px;
        color: #666;
        margin-bottom: 2rem;
      }

      .registration-form {
        display: flex;
        flex-direction: column;
        gap: 1.5rem;
      }

      .form-group {
        display: flex;
        flex-direction: column;
        gap: 0.5rem;
      }

      label {
        font-size: 14px;
        font-weight: 500;
        color: #333;
      }

      .form-control {
        border-radius: 4px;
        padding: 0.75rem 1rem;
        border: 1px solid #ddd;
        font-size: 14px;
      }

      .form-control:focus {
        border-color: #0066ff;
        box-shadow: 0 0 0 0.2rem rgba(0, 102, 255, 0.25);
      }

      .file-input-container {
        position: relative;
      }

      input[type='file'] {
        position: absolute;
        width: 1px;
        height: 1px;
        padding: 0;
        margin: -1px;
        overflow: hidden;
        clip: rect(0, 0, 0, 0);
        white-space: nowrap;
        border-width: 0;
      }

      .file-input-label {
        display: flex;
        align-items: center;
        gap: 0.5rem;
        padding: 0.75rem 1rem;
        background-color: #f0f0f0;
        border: 1px solid #ddd;
        border-radius: 4px;
        cursor: pointer;
        transition: background-color 0.2s ease;
      }

      .file-input-label:hover {
        background-color: #e8e8e8;
      }

      .file-input-label i {
        font-size: 1.25rem;
        color: #666;
      }

      .file-input-label span {
        font-size: 14px;
        color: #333;
        white-space: nowrap;
        overflow: hidden;
        text-overflow: ellipsis;
      }

      .submit-btn {
        background-color: #0066ff;
        color: #ffffff;
        border: none;
        border-radius: 4px;
        padding: 0.75rem 1rem;
        font-size: 16px;
        font-weight: 500;
        cursor: pointer;
        transition: background-color 0.2s ease;
      }

      .submit-btn:hover:not(:disabled) {
        background-color: #0052cc;
      }

      .submit-btn:disabled {
        background-color: #ccc;
        cursor: not-allowed;
      }

      .card {
        border: none;
        border-radius: 8px;
        box-shadow: 0 2px 4px rgba(0, 0, 0, 0.05);
        margin-bottom: 1.5rem;
      }

      .card-header {
        background-color: #f8f9fa;
        border-bottom: 1px solid #eee;
        padding: 1rem 1.5rem;
      }

      .card-body {
        padding: 1.5rem;
      }

      .badge {
        padding: 0.5rem 0.75rem;
        border-radius: 4px;
        font-weight: 500;
        font-size: 12px;
      }

      .badge-pending {
        background-color: #fff7ed;
        color: #c2410c;
      }

      .badge-approved {
        background-color: #ecfdf5;
        color: #059669;
      }

      .badge-rejected {
        background-color: #fee2e2;
        color: #dc2626;
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

      @media (max-width: 768px) {
        .main-content {
          margin-left: 0;
          padding: 1rem;
        }

        .registration-container,
        .applications-container {
          padding: 1.5rem;
        }

        h1 {
          font-size: 20px;
        }

        .subtitle {
          font-size: 12px;
        }
      }
    `,
  ],
})
export class InterviewerRegistrationComponent implements OnInit {
  // User data
  loggedInUser: LoggedInUser | null = null;

  currentPage = 1;
  pageSize = 5;
  totalCount = 0;
  totalPages = 1;

  // Application data
  previousApplications: VerificationApplication[] = [];
  showRegistrationForm = false;
  hasPendingOrApproved = false;

  // Form data
  additionalInfo = '';
  files: { [key: string]: File | null } = {
    resume: null,
  };

  // UI states
  isLoading = true;
  isSubmitting = false;
  error: string | null = null;

  // Document viewer
  showDocumentModal = false;
  documentUrl: any = null;
  documentTitle = '';
  isLoadingDocument = false;
  selectedApplicationId: string | null = null;

  constructor(
    private router: Router,
    private verificationService: VerificationService,
    private sanitizer: DomSanitizer
  ) {}

  ngOnInit(): void {
    // Check if user is logged in
    this.checkUserAndLoadData();
  }

  onPageChange(page: number): void {
    this.currentPage = page;
    this.loadVerificationApplications();
  }

  checkUserAndLoadData(): void {
    try {
      // Get logged in user from sessionStorage
      const userJson = sessionStorage.getItem('loggedInUser');
      if (!userJson) {
        this.error = 'User not logged in. Please log in to continue.';
        this.isLoading = false;
        return;
      }

      this.loggedInUser = JSON.parse(userJson);

      // Check if user has Interviewer role
      if (this.loggedInUser?.roles.includes('Interviewer')) {
        // Navigate to interviewer dashboard
        this.router.navigate(['/interviewer-dashboard']);
        return;
      }

      // Load verification applications
      this.loadVerificationApplications();
    } catch (error) {
      console.error('Error checking user:', error);
      this.error =
        'An error occurred while checking user data. Please try again.';
      this.isLoading = false;
    }
  }

  loadVerificationApplications(): void {
    if (!this.loggedInUser) {
      this.isLoading = false;
      return;
    }

    this.verificationService
      .getVerificationList({
        pageNumber: this.currentPage,
        pageSize: this.pageSize,
        userId: this.loggedInUser.userId,
      })
      .subscribe({
        next: (response: any) => {
          this.previousApplications = response.items;

          this.totalCount = response.totalCount;
          this.totalPages = response.totalPages;

          // Check if user has any pending or approved applications
          this.hasPendingOrApproved = this.previousApplications.some(
            (app) => app.status === 'pending' || app.status === 'approved'
          );

          // Always show the list view initially
          this.showRegistrationForm = false;
          this.isLoading = false;
        },
        error: (error: any) => {
          console.error('Error loading verification applications:', error);
          this.error =
            'Failed to load your previous applications. Please try again.';
          this.isLoading = false;
        },
      });
  }

  onFileSelected(event: Event, fileType: string): void {
    const input = event.target as HTMLInputElement;
    if (input.files && input.files.length > 0) {
      this.files[fileType] = input.files[0];
    }
  }

  onSubmit(): void {
    if (!this.loggedInUser) {
      this.error = 'User not logged in. Please log in to continue.';
      return;
    }

    this.isSubmitting = true;

    // Step 1: Submit basic application info
    const applicationData = {
      userId: this.loggedInUser.userId,
      additionalInfo: this.additionalInfo,
    };

    this.verificationService
      .submitVerificationApplication(applicationData)
      .subscribe({
        next: (response: any) => {
          console.log(response);
          const applicationId = response.id.requestId;

          // Step 2: Upload CV file
          if (this.files['resume']) {
            const formData = new FormData();
            formData.append('cv', this.files['resume']);

            this.verificationService
              .uploadFile(applicationId, 'cv', formData)
              .subscribe({
                next: () => {
                  // Reset form
                  this.resetForm();

                  // Hide registration form and show applications list
                  this.showRegistrationForm = false;
                  this.isSubmitting = false;

                  // Reload applications to update the list
                  this.loadVerificationApplications();
                },
                error: (error: any) => {
                  console.error('Error uploading CV:', error);
                  this.error = 'Failed to upload your CV. Please try again.';
                  this.isSubmitting = false;
                },
              });
          } else {
            // No CV file selected
            this.error = 'Please select a CV file to upload.';
            this.isSubmitting = false;
          }
        },
        error: (error) => {
          console.error('Error submitting application:', error);
          this.error = 'Failed to submit your application. Please try again.';
          this.isSubmitting = false;
        },
      });
  }

  resetForm(): void {
    this.additionalInfo = '';
    this.files = {
      resume: null,
    };
  }

  getRejectedApplicationsCount(): number {
    return this.previousApplications.filter((app) => app.status === 'rejected')
      .length;
  }

  getStatusBadgeClass(status: string): string {
    return `badge badge-${status.toLowerCase()}`;
  }

  viewDocument(applicationId: string, documentType: string): void {
    this.selectedApplicationId = applicationId;
    this.documentTitle = `${this.getDocumentTitle(documentType)}`;
    this.showDocumentModal = true;
    this.isLoadingDocument = true;
    this.documentUrl = null;

    this.verificationService
      .getVerificationDocument(applicationId, documentType)
      .subscribe({
        next: (blob) => {
          // Create a blob URL for the document
          const url = URL.createObjectURL(blob);
          this.documentUrl = this.sanitizer.bypassSecurityTrustResourceUrl(url);
          this.isLoadingDocument = false;
        },
        error: (error) => {
          console.error('Error loading document:', error);
          this.isLoadingDocument = false;
          alert('Failed to load document. Please try again.');
        },
      });
  }

  getDocumentTitle(documentType: string): string {
    switch (documentType) {
      case 'cv':
        return 'Resume/CV';
      case 'workPermit':
        return 'Work Permit';
      case 'idCard':
        return 'ID Card';
      default:
        return 'Document';
    }
  }

  closeDocumentModal(): void {
    this.showDocumentModal = false;
    this.documentUrl = null;
    this.documentTitle = '';
    this.selectedApplicationId = null;
  }
}

