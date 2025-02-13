import { Component, Input, Output, EventEmitter } from '@angular/core';
import { CommonModule } from '@angular/common';

interface InterviewerDetails {
  name: string;
  email: string;
  expertise: string;
  rating: number;
  verificationStatus: string;
  experienceYears: number;
  activeInterviews: number;
  totalInterviews: number;
  averageTime: number;
  upcomingInterviews: Array<{
    candidateName: string;
    position: string;
    date: string;
  }>;
}

@Component({
  selector: 'app-interviewer-details-modal',
  standalone: true,
  imports: [CommonModule],
  template: `
    <div class="modal" [class.show]="true" style="display: block">
      <div class="modal-dialog modal-dialog-centered modal-lg">
        <div class="modal-content border-0">
          <div class="modal-header border-0 pb-0">
            <h5 class="modal-title">Interviewer Details</h5>
            <button
              type="button"
              class="btn-close"
              (click)="close.emit()"
            ></button>
          </div>
          <div class="modal-body pt-2">
            <div class="row g-4">
              <!-- Personal Information -->
              <div class="col-md-6">
                <div class="card border-0 shadow-sm h-100">
                  <div class="card-body">
                    <h6 class="mb-4">Personal Information</h6>

                    <div class="mb-3">
                      <label class="text-muted small d-block">Name:</label>
                      <span class="fw-medium">{{ details.name }}</span>
                    </div>

                    <div class="mb-3">
                      <label class="text-muted small d-block">Email:</label>
                      <span>{{ details.email }}</span>
                    </div>

                    <div class="mb-3">
                      <label class="text-muted small d-block">Expertise:</label>
                      <span>{{ details.expertise }}</span>
                    </div>

                    <div class="mb-3">
                      <label class="text-muted small d-block">Rating:</label>
                      <span>{{ details.rating }}</span>
                    </div>

                    <div class="mb-3">
                      <label class="text-muted small d-block"
                        >Verification Status:</label
                      >
                      <span
                        [class]="
                          'status-badge status-' +
                          details.verificationStatus.toLowerCase()
                        "
                      >
                        {{ details.verificationStatus }}
                      </span>
                    </div>

                    <div>
                      <label class="text-muted small d-block"
                        >Experience Years:</label
                      >
                      <span>{{ details.experienceYears }} years</span>
                    </div>
                  </div>
                </div>
              </div>

              <!-- Statistics -->
              <div class="col-md-6">
                <div class="card border-0 shadow-sm h-100">
                  <div class="card-body">
                    <h6 class="mb-4">Statistics</h6>

                    <div class="mb-3">
                      <label class="text-muted small d-block"
                        >Active Interviews:</label
                      >
                      <span class="fw-medium">{{
                        details.activeInterviews
                      }}</span>
                    </div>

                    <div class="mb-3">
                      <label class="text-muted small d-block"
                        >Total Interviews Conducted:</label
                      >
                      <span>{{ details.totalInterviews }}</span>
                    </div>

                    <div>
                      <label class="text-muted small d-block"
                        >Average Interview Time:</label
                      >
                      <span>{{ details.averageTime }} minutes</span>
                    </div>
                  </div>
                </div>
              </div>
            </div>

            <!-- Upcoming Interviews -->
            <div class="card border-0 shadow-sm mt-4">
              <div class="card-body">
                <h6 class="mb-4">Upcoming Interviews</h6>

                <div class="table-responsive">
                  <table class="table table-hover mb-0">
                    <thead class="bg-light">
                      <tr>
                        <th class="border-0 px-0">Candidate Name</th>
                        <th class="border-0">Position</th>
                        <th class="border-0">Date</th>
                        <th class="border-0 text-end">Actions</th>
                      </tr>
                    </thead>
                    <tbody>
                      <tr *ngFor="let interview of details.upcomingInterviews">
                        <td class="px-0">{{ interview.candidateName }}</td>
                        <td>{{ interview.position }}</td>
                        <td>{{ interview.date }}</td>
                        <td class="text-end">
                          <button
                            class="btn btn-link btn-sm p-0 text-decoration-none me-3"
                          >
                            Edit
                          </button>
                          <button
                            class="btn btn-link btn-sm p-0 text-decoration-none text-danger"
                          >
                            Delete
                          </button>
                        </td>
                      </tr>
                    </tbody>
                  </table>
                </div>

                <button
                  class="btn btn-dark mt-4"
                  (click)="assignNewInterview()"
                >
                  Assign New Interview
                </button>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
    <div class="modal-backdrop fade show"></div>
  `,
  styles: [
    `
      .modal-lg {
        max-width: 800px;
      }

      .card {
        border-radius: 8px;
      }

      .status-badge {
        padding: 4px 8px;
        border-radius: 4px;
        font-size: 13px;
        font-weight: 500;
      }

      .status-verified {
        background-color: #ecfdf5;
        color: #059669;
      }

      .status-pending {
        background-color: #fef3c7;
        color: #d97706;
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

      .btn-link {
        font-size: 14px;
        color: #2563eb;
      }

      .btn-link:hover {
        color: #1d4ed8;
      }
    `,
  ],
})
export class InterviewerDetailsModalComponent {
  @Input() details!: InterviewerDetails;
  @Output() close = new EventEmitter<void>();

  assignNewInterview() {
    console.log('Assign new interview');
  }
}
