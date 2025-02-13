import { Component, type OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { SidebarComponent } from '../sidebar/sidebar.component';
import { PaginationComponent } from '../pagination/pagination.component';
import { InterviewerDetailsModalComponent } from '../interviewers-details/interviewers-details.component';

interface Interviewer {
  id: number;
  name: string;
  email: string;
  expertise: 'Frontend' | 'Backend' | 'Full Stack' | 'DevOps';
  rating: number;
  verificationStatus: 'Verified' | 'Pending';
  experience:
    | 'Junior (0-2 years)'
    | 'Mid-level (3-5 years)'
    | 'Senior (6+ years)';
}

interface InterviewerStats {
  total: number;
  verified: number;
  pending: number;
}

interface ExpertiseCount {
  Frontend: number;
  Backend: number;
  'Full Stack': number;
  DevOps: number;
}

interface ExperienceCount {
  'Junior (0-2 years)': number;
  'Mid-level (3-5 years)': number;
  'Senior (6+ years)': number;
}

@Component({
  selector: 'app-interviewers',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    SidebarComponent,
    PaginationComponent,
    InterviewerDetailsModalComponent,
  ],
  template: `
    <div class="d-flex">
      <app-sidebar></app-sidebar>

      <main class="main-content bg-light">
        <div class="p-4">
          <!-- Header -->
          <div class="d-flex justify-content-between align-items-center mb-4">
            <h1 class="h4 mb-0">Interviewers</h1>
            <div class="d-flex gap-3 align-items-center">
              <div class="search-box">
                <input
                  type="text"
                  class="form-control"
                  placeholder="Search interviewers"
                  [(ngModel)]="searchQuery"
                />
              </div>
              <div class="dropdown">
                <button
                  class="btn btn-outline-secondary dropdown-toggle d-flex align-items-center gap-2"
                  type="button"
                  id="filterDropdown"
                  data-bs-toggle="dropdown"
                  aria-expanded="false"
                >
                  <i class="bi bi-funnel"></i>
                  Filter
                </button>
                <ul class="dropdown-menu" aria-labelledby="filterDropdown">
                  <li><h6 class="dropdown-header">Expertise</h6></li>
                  <li>
                    <a class="dropdown-item" (click)="filterByExpertise(null)"
                      >All</a
                    >
                  </li>
                  <li>
                    <a
                      class="dropdown-item"
                      (click)="filterByExpertise('Frontend')"
                      >Frontend</a
                    >
                  </li>
                  <li>
                    <a
                      class="dropdown-item"
                      (click)="filterByExpertise('Backend')"
                      >Backend</a
                    >
                  </li>
                  <li>
                    <a
                      class="dropdown-item"
                      (click)="filterByExpertise('Full Stack')"
                      >Full Stack</a
                    >
                  </li>
                  <li>
                    <a
                      class="dropdown-item"
                      (click)="filterByExpertise('DevOps')"
                      >DevOps</a
                    >
                  </li>
                  <li><hr class="dropdown-divider" /></li>
                  <li><h6 class="dropdown-header">Status</h6></li>
                  <li>
                    <a class="dropdown-item" (click)="filterByStatus(null)"
                      >All</a
                    >
                  </li>
                  <li>
                    <a
                      class="dropdown-item"
                      (click)="filterByStatus('Verified')"
                      >Verified</a
                    >
                  </li>
                  <li>
                    <a class="dropdown-item" (click)="filterByStatus('Pending')"
                      >Pending</a
                    >
                  </li>
                </ul>
              </div>
              <button
                class="btn btn-dark d-flex align-items-center gap-2"
                (click)="openAddModal()"
              >
                <i class="bi bi-plus"></i>
                Add Interviewer
              </button>
            </div>
          </div>

          <!-- Stats Cards -->
          <div class="row g-4 mb-4">
            <div class="col-md-4">
              <div class="card border-0 shadow-sm">
                <div class="card-body">
                  <h6 class="text-muted mb-2">Total Interviewers</h6>
                  <h2 class="mb-0">{{ stats.total }}</h2>
                </div>
              </div>
            </div>
            <div class="col-md-4">
              <div class="card border-0 shadow-sm">
                <div class="card-body">
                  <h6 class="text-muted mb-2">Verified Interviewers</h6>
                  <h2 class="mb-0">{{ stats.verified }}</h2>
                </div>
              </div>
            </div>
            <div class="col-md-4">
              <div class="card border-0 shadow-sm">
                <div class="card-body">
                  <h6 class="text-muted mb-2">Pending Verification</h6>
                  <h2 class="mb-0">{{ stats.pending }}</h2>
                </div>
              </div>
            </div>
          </div>

          <!-- Expertise & Experience Breakdown -->
          <div class="row g-4 mb-4">
            <div class="col-lg-6">
              <div class="card border-0 shadow-sm">
                <div class="card-body">
                  <h6 class="mb-3">Expertise Breakdown</h6>
                  <div class="expertise-list">
                    <div class="d-flex justify-content-between mb-2">
                      <span>Frontend:</span>
                      <span>{{ expertiseCounts.Frontend }}</span>
                    </div>
                    <div class="d-flex justify-content-between mb-2">
                      <span>Backend:</span>
                      <span>{{ expertiseCounts.Backend }}</span>
                    </div>
                    <div class="d-flex justify-content-between mb-2">
                      <span>Full Stack:</span>
                      <span>{{ expertiseCounts['Full Stack'] }}</span>
                    </div>
                    <div class="d-flex justify-content-between">
                      <span>DevOps:</span>
                      <span>{{ expertiseCounts.DevOps }}</span>
                    </div>
                  </div>
                </div>
              </div>
            </div>
            <div class="col-lg-6">
              <div class="card border-0 shadow-sm">
                <div class="card-body">
                  <h6 class="mb-3">Experience Levels</h6>
                  <div class="experience-list">
                    <div class="d-flex justify-content-between mb-2">
                      <span>Junior (0-2 years):</span>
                      <span>{{ experienceCounts['Junior (0-2 years)'] }}</span>
                    </div>
                    <div class="d-flex justify-content-between mb-2">
                      <span>Mid-level (3-5 years):</span>
                      <span>{{
                        experienceCounts['Mid-level (3-5 years)']
                      }}</span>
                    </div>
                    <div class="d-flex justify-content-between">
                      <span>Senior (6+ years):</span>
                      <span>{{ experienceCounts['Senior (6+ years)'] }}</span>
                    </div>
                  </div>
                </div>
              </div>
            </div>
          </div>

          <!-- Interviewers Table -->
          <div class="card border-0 shadow-sm">
            <div class="table-responsive">
              <table class="table table-hover mb-0">
                <thead class="bg-light">
                  <tr>
                    <th class="border-0 px-4 py-3">
                      <div
                        class="d-flex align-items-center gap-2"
                        (click)="sort('name')"
                      >
                        Name
                        <div class="sort-arrows">
                          <i
                            class="bi bi-chevron-up"
                            [class.active]="
                              sortField === 'name' && sortOrder === 'asc'
                            "
                          ></i>
                          <i
                            class="bi bi-chevron-down"
                            [class.active]="
                              sortField === 'name' && sortOrder === 'desc'
                            "
                          ></i>
                        </div>
                      </div>
                    </th>
                    <th class="border-0 px-4 py-3">
                      <div
                        class="d-flex align-items-center gap-2"
                        (click)="sort('email')"
                      >
                        Email
                        <div class="sort-arrows">
                          <i
                            class="bi bi-chevron-up"
                            [class.active]="
                              sortField === 'email' && sortOrder === 'asc'
                            "
                          ></i>
                          <i
                            class="bi bi-chevron-down"
                            [class.active]="
                              sortField === 'email' && sortOrder === 'desc'
                            "
                          ></i>
                        </div>
                      </div>
                    </th>
                    <th class="border-0 px-4 py-3">
                      <div
                        class="d-flex align-items-center gap-2"
                        (click)="sort('expertise')"
                      >
                        Expertise
                        <div class="sort-arrows">
                          <i
                            class="bi bi-chevron-up"
                            [class.active]="
                              sortField === 'expertise' && sortOrder === 'asc'
                            "
                          ></i>
                          <i
                            class="bi bi-chevron-down"
                            [class.active]="
                              sortField === 'expertise' && sortOrder === 'desc'
                            "
                          ></i>
                        </div>
                      </div>
                    </th>
                    <th class="border-0 px-4 py-3">
                      <div
                        class="d-flex align-items-center gap-2"
                        (click)="sort('rating')"
                      >
                        Rating
                        <div class="sort-arrows">
                          <i
                            class="bi bi-chevron-up"
                            [class.active]="
                              sortField === 'rating' && sortOrder === 'asc'
                            "
                          ></i>
                          <i
                            class="bi bi-chevron-down"
                            [class.active]="
                              sortField === 'rating' && sortOrder === 'desc'
                            "
                          ></i>
                        </div>
                      </div>
                    </th>
                    <th class="border-0 px-4 py-3">Verification Status</th>
                    <th class="border-0 px-4 py-3 text-end">Actions</th>
                  </tr>
                </thead>
                <tbody>
                  <tr *ngFor="let interviewer of paginatedInterviewers">
                    <td class="px-4 py-3">{{ interviewer.name }}</td>
                    <td class="px-4 py-3">{{ interviewer.email }}</td>
                    <td class="px-4 py-3">{{ interviewer.expertise }}</td>
                    <td class="px-4 py-3">{{ interviewer.rating }}</td>
                    <td class="px-4 py-3">
                      <span
                        [class]="getStatusClass(interviewer.verificationStatus)"
                      >
                        {{ interviewer.verificationStatus }}
                      </span>
                    </td>
                    <td class="px-4 py-3 text-end">
                      <div class="d-flex gap-3 justify-content-end">
                        <button
                          class="btn btn-link p-0 text-decoration-none"
                          (click)="viewDetails(interviewer)"
                        >
                          View Details
                        </button>
                        <button
                          class="btn btn-link p-0 text-decoration-none"
                          (click)="openEditModal(interviewer)"
                        >
                          <i class="bi bi-pencil me-1"></i>
                          Edit
                        </button>
                        <button
                          class="btn btn-link p-0 text-decoration-none text-danger"
                          (click)="openRemoveModal(interviewer)"
                        >
                          <i class="bi bi-trash me-1"></i>
                          Remove
                        </button>
                      </div>
                    </td>
                  </tr>
                </tbody>
              </table>

              <app-pagination
                [currentPage]="currentPage"
                [pageSize]="pageSize"
                [totalItems]="filteredInterviewers.length"
                (pageChange)="onPageChange($event)"
              ></app-pagination>
            </div>
          </div>
        </div>
      </main>
      <app-interviewer-details-modal
        *ngIf="showDetailsModal"
        [details]="selectedInterviewerDetails"
        (close)="showDetailsModal = false"
      ></app-interviewer-details-modal>

      <!-- Add Interviewer Modal -->
      <div
        class="modal"
        [class.show]="showAddModal"
        [style.display]="showAddModal ? 'block' : 'none'"
      >
        <div class="modal-dialog modal-dialog-centered">
          <div class="modal-content border-0">
            <div class="modal-header border-0">
              <h5 class="modal-title">Add New Interviewer</h5>
              <button
                type="button"
                class="btn-close"
                (click)="closeAddModal()"
              ></button>
            </div>
            <div class="modal-body">
              <p class="text-muted small mb-4">
                Enter the details of the new interviewer.
              </p>

              <div class="mb-3">
                <label class="form-label">Name</label>
                <input
                  type="text"
                  class="form-control"
                  [(ngModel)]="newInterviewer.name"
                />
              </div>

              <div class="mb-3">
                <label class="form-label">Email</label>
                <input
                  type="email"
                  class="form-control"
                  [(ngModel)]="newInterviewer.email"
                />
              </div>

              <div class="mb-3">
                <label class="form-label">Expertise</label>
                <select
                  class="form-select"
                  [(ngModel)]="newInterviewer.expertise"
                >
                  <option value="Frontend">Frontend</option>
                  <option value="Backend">Backend</option>
                  <option value="Full Stack">Full Stack</option>
                  <option value="DevOps">DevOps</option>
                </select>
              </div>

              <div class="mb-3">
                <label class="form-label">Experience Level</label>
                <select
                  class="form-select"
                  [(ngModel)]="newInterviewer.experience"
                >
                  <option value="Junior (0-2 years)">Junior (0-2 years)</option>
                  <option value="Mid-level (3-5 years)">
                    Mid-level (3-5 years)
                  </option>
                  <option value="Senior (6+ years)">Senior (6+ years)</option>
                </select>
              </div>
            </div>
            <div class="modal-footer border-0">
              <button
                type="button"
                class="btn btn-dark px-4"
                [disabled]="!isValidInterviewer(newInterviewer)"
                (click)="addInterviewer()"
              >
                Add Interviewer
              </button>
            </div>
          </div>
        </div>
      </div>

      <!-- Edit Interviewer Modal -->
      <div
        class="modal"
        [class.show]="showEditModal"
        [style.display]="showEditModal ? 'block' : 'none'"
      >
        <div class="modal-dialog modal-dialog-centered">
          <div class="modal-content border-0">
            <div class="modal-header border-0">
              <h5 class="modal-title">Edit Interviewer</h5>
              <button
                type="button"
                class="btn-close"
                (click)="closeEditModal()"
              ></button>
            </div>
            <div class="modal-body">
              <p class="text-muted small mb-4">
                Update the interviewer's information.
              </p>

              <div class="mb-3">
                <label class="form-label">Name</label>
                <input
                  type="text"
                  class="form-control"
                  [(ngModel)]="editingInterviewer.name"
                />
              </div>

              <div class="mb-3">
                <label class="form-label">Email</label>
                <input
                  type="email"
                  class="form-control"
                  [(ngModel)]="editingInterviewer.email"
                />
              </div>

              <div class="mb-3">
                <label class="form-label">Expertise</label>
                <select
                  class="form-select"
                  [(ngModel)]="editingInterviewer.expertise"
                >
                  <option value="Frontend">Frontend</option>
                  <option value="Backend">Backend</option>
                  <option value="Full Stack">Full Stack</option>
                  <option value="DevOps">DevOps</option>
                </select>
              </div>

              <div class="mb-3">
                <label class="form-label">Experience Level</label>
                <select
                  class="form-select"
                  [(ngModel)]="editingInterviewer.experience"
                >
                  <option value="Junior (0-2 years)">Junior (0-2 years)</option>
                  <option value="Mid-level (3-5 years)">
                    Mid-level (3-5 years)
                  </option>
                  <option value="Senior (6+ years)">Senior (6+ years)</option>
                </select>
              </div>

              <div class="mb-3">
                <label class="form-label">Verification Status</label>
                <select
                  class="form-select"
                  [(ngModel)]="editingInterviewer.verificationStatus"
                >
                  <option value="Verified">Verified</option>
                  <option value="Pending">Pending</option>
                </select>
              </div>

              <div class="mb-3">
                <label class="form-label">Rating</label>
                <input
                  type="number"
                  class="form-control"
                  min="0"
                  max="5"
                  step="0.1"
                  [(ngModel)]="editingInterviewer.rating"
                />
              </div>
            </div>
            <div class="modal-footer border-0">
              <button
                type="button"
                class="btn btn-dark px-4"
                [disabled]="!isValidInterviewer(editingInterviewer)"
                (click)="saveChanges()"
              >
                Save Changes
              </button>
            </div>
          </div>
        </div>
      </div>

      <!-- Remove Interviewer Modal -->
      <div
        class="modal"
        [class.show]="showRemoveModal"
        [style.display]="showRemoveModal ? 'block' : 'none'"
      >
        <div class="modal-dialog modal-dialog-centered">
          <div class="modal-content border-0">
            <div class="modal-header border-0">
              <h5 class="modal-title">Remove Interviewer</h5>
              <button
                type="button"
                class="btn-close"
                (click)="closeRemoveModal()"
              ></button>
            </div>
            <div class="modal-body">
              <p>
                Are you sure you want to remove {{ selectedInterviewer?.name }}?
              </p>
              <p class="text-muted small">This action cannot be undone.</p>
            </div>
            <div class="modal-footer border-0">
              <button
                type="button"
                class="btn btn-link text-dark"
                (click)="closeRemoveModal()"
              >
                Cancel
              </button>
              <button
                type="button"
                class="btn btn-danger"
                (click)="removeInterviewer()"
              >
                Remove
              </button>
            </div>
          </div>
        </div>
      </div>

      <!-- Modal Backdrop -->
      <div
        class="modal-backdrop fade show"
        *ngIf="showAddModal || showEditModal || showRemoveModal"
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

      .search-box {
        width: 300px;
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

      .sort-arrows {
        display: flex;
        flex-direction: column;
        line-height: 0;
        margin-left: 4px;
      }

      .sort-arrows i {
        font-size: 10px;
        color: #9ca3af;
        cursor: pointer;
      }

      .sort-arrows i.active {
        color: #111827;
      }
      th {
        cursor: pointer;
      }

      th:hover .sort-arrows i {
        color: #6b7280;
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

      .btn-link {
        font-size: 14px;
        color: #2563eb;
      }

      .btn-link:hover {
        color: #1d4ed8;
      }

      .modal-content {
        border-radius: 12px;
      }

      .form-control,
      .form-select {
        border-radius: 6px;
        padding: 0.5rem 0.75rem;
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
export class InterviewerComponent implements OnInit {
  // Pagination
  currentPage = 1;
  pageSize = 10;
  searchQuery = '';

  // Sorting
  sortField: keyof Interviewer = 'name';
  sortOrder: 'asc' | 'desc' = 'asc';

  // Filtering
  filterExpertise: string | null = null;
  filterVerificationStatus: string | null = null;

  // Modal states
  showAddModal = false;
  showEditModal = false;
  showRemoveModal = false;
  showDetailsModal = false;

  // Form data
  newInterviewer: Partial<Interviewer> = {};
  editingInterviewer: Partial<Interviewer> = {};
  selectedInterviewer: Interviewer | null = null;
  selectedInterviewerDetails: any = null;

  interviewers: Interviewer[] = [
    {
      id: 1,
      name: 'Samantha Lee',
      email: 'samantha@example.com',
      expertise: 'Backend',
      rating: 4.5,
      verificationStatus: 'Pending',
      experience: 'Mid-level (3-5 years)',
    },
    {
      id: 2,
      name: 'Rachel Green',
      email: 'rachel@example.com',
      expertise: 'DevOps',
      rating: 4.7,
      verificationStatus: 'Pending',
      experience: 'Senior (6+ years)',
    },
    {
      id: 3,
      name: 'Chris Taylor',
      email: 'chris@example.com',
      expertise: 'Full Stack',
      rating: 4.9,
      verificationStatus: 'Verified',
      experience: 'Senior (6+ years)',
    },
    {
      id: 4,
      name: 'Alex Johnson',
      email: 'alex@example.com',
      expertise: 'Frontend',
      rating: 4.8,
      verificationStatus: 'Verified',
      experience: 'Mid-level (3-5 years)',
    },
  ];

  get stats(): InterviewerStats {
    return {
      total: this.interviewers.length,
      verified: this.interviewers.filter(
        (i) => i.verificationStatus === 'Verified'
      ).length,
      pending: this.interviewers.filter(
        (i) => i.verificationStatus === 'Pending'
      ).length,
    };
  }

  get expertiseCounts(): ExpertiseCount {
    const counts = {
      Frontend: 0,
      Backend: 0,
      'Full Stack': 0,
      DevOps: 0,
    };
    this.interviewers.forEach((interviewer) => {
      counts[interviewer.expertise]++;
    });
    return counts;
  }

  get experienceCounts(): ExperienceCount {
    const counts = {
      'Junior (0-2 years)': 0,
      'Mid-level (3-5 years)': 0,
      'Senior (6+ years)': 0,
    };
    this.interviewers.forEach((interviewer) => {
      counts[interviewer.experience]++;
    });
    return counts;
  }

  get filteredInterviewers(): Interviewer[] {
    return this.interviewers
      .filter((interviewer) => {
        const matchesSearch =
          interviewer.name
            .toLowerCase()
            .includes(this.searchQuery.toLowerCase()) ||
          interviewer.email
            .toLowerCase()
            .includes(this.searchQuery.toLowerCase());
        const matchesExpertise =
          !this.filterExpertise ||
          interviewer.expertise === this.filterExpertise;
        const matchesStatus =
          !this.filterVerificationStatus ||
          interviewer.verificationStatus === this.filterVerificationStatus;
        return matchesSearch && matchesExpertise && matchesStatus;
      })
      .sort((a, b) => {
        const aValue = a[this.sortField];
        const bValue = b[this.sortField];
        if (aValue < bValue) return this.sortOrder === 'asc' ? -1 : 1;
        if (aValue > bValue) return this.sortOrder === 'asc' ? 1 : -1;
        return 0;
      });
  }

  get paginatedInterviewers(): Interviewer[] {
    const start = (this.currentPage - 1) * this.pageSize;
    const end = start + this.pageSize;
    return this.filteredInterviewers.slice(start, end);
  }

  ngOnInit() {}

  sort(field: keyof Interviewer) {
    if (this.sortField === field) {
      this.sortOrder = this.sortOrder === 'asc' ? 'desc' : 'asc';
    } else {
      this.sortField = field;
      this.sortOrder = 'asc';
    }
  }

  filterByExpertise(expertise: string | null) {
    this.filterExpertise = expertise;
  }

  filterByStatus(status: string | null) {
    this.filterVerificationStatus = status;
  }

  getStatusClass(status: string): string {
    return `status-badge status-${status.toLowerCase()}`;
  }

  onPageChange(page: number): void {
    this.currentPage = page;
  }

  // Modal handlers
  openAddModal(): void {
    this.newInterviewer = {};
    this.showAddModal = true;
  }

  openEditModal(interviewer: Interviewer): void {
    this.editingInterviewer = { ...interviewer };
    this.showEditModal = true;
  }

  openRemoveModal(interviewer: Interviewer): void {
    this.selectedInterviewer = interviewer;
    this.showRemoveModal = true;
  }

  closeAddModal(): void {
    this.showAddModal = false;
    this.newInterviewer = {};
  }

  closeEditModal(): void {
    this.showEditModal = false;
    this.editingInterviewer = {};
  }

  closeRemoveModal(): void {
    this.showRemoveModal = false;
    this.selectedInterviewer = null;
  }

  closeAllModals(): void {
    this.closeAddModal();
    this.closeEditModal();
    this.closeRemoveModal();
  }

  viewDetails(interviewer: Interviewer): void {
    this.selectedInterviewerDetails = {
      ...interviewer,
      experienceYears: 8,
      activeInterviews: 5,
      totalInterviews: 75,
      averageTime: 40,
      upcomingInterviews: [
        {
          candidateName: 'Emily Brown',
          position: 'Full Stack Developer',
          date: 'Jul 27, 2023, 9:00:00 AM',
        },
      ],
    };
    this.showDetailsModal = true;
  }

  // CRUD operations
  addInterviewer(): void {
    if (this.isValidInterviewer(this.newInterviewer)) {
      const newId = Math.max(...this.interviewers.map((i) => i.id)) + 1;
      const interviewer: Interviewer = {
        id: newId,
        name: this.newInterviewer.name!,
        email: this.newInterviewer.email!,
        expertise: this.newInterviewer.expertise!,
        rating: 0,
        verificationStatus: 'Pending',
        experience: this.newInterviewer.experience!,
      };
      this.interviewers.push(interviewer);
      this.closeAddModal();
    }
  }

  saveChanges(): void {
    if (this.isValidInterviewer(this.editingInterviewer)) {
      const index = this.interviewers.findIndex(
        (i) => i.id === this.editingInterviewer.id
      );
      if (index !== -1) {
        this.interviewers[index] = this.editingInterviewer as Interviewer;
        this.closeEditModal();
      }
    }
  }

  removeInterviewer(): void {
    if (this.selectedInterviewer) {
      this.interviewers = this.interviewers.filter(
        (i) => i.id !== this.selectedInterviewer!.id
      );
      this.closeRemoveModal();
    }
  }

  public isValidInterviewer(interviewer: Partial<Interviewer>): boolean {
    return !!(
      interviewer.name &&
      interviewer.email &&
      interviewer.expertise &&
      interviewer.experience
    );
  }
}
