import {
  Component,
  OnInit,
  ViewChild,
  type ElementRef,
  type AfterViewInit,
} from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

import { Chart, registerables } from 'chart.js';
import { SidebarComponent } from '../sidebar/sidebar.component';
import { PaginationComponent } from '../pagination/pagination.component';

Chart.register(...registerables);

interface Candidate {
  id: number;
  name: string;
  email: string;
  appliedFor: string;
  testScore: number | null;
  status: 'Shortlisted' | 'Pending' | 'Rejected';
  skills: string[];
}

interface CandidateStats {
  total: number;
  shortlisted: number;
  pending: number;
  rejected: number;
}

@Component({
  selector: 'app-candidates',
  standalone: true,
  imports: [CommonModule, FormsModule, SidebarComponent, PaginationComponent],
  template: `
    <div class="d-flex">
      <app-sidebar></app-sidebar>

      <main class="main-content bg-light">
        <div class="p-4">
          <!-- Header -->
          <div class="d-flex justify-content-between align-items-center mb-4">
            <h1 class="h4 mb-0">Candidates</h1>
            <div class="d-flex gap-3 align-items-center">
              <div class="search-box">
                <input
                  type="text"
                  class="form-control"
                  placeholder="Search candidates"
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
                  Filter: {{ filterStatus }}
                </button>
                <ul class="dropdown-menu" aria-labelledby="filterDropdown">
                  <li>
                    <a class="dropdown-item" (click)="filterStatus = 'All'"
                      >All</a
                    >
                  </li>
                  <li>
                    <a
                      class="dropdown-item"
                      (click)="filterStatus = 'Shortlisted'"
                      >Shortlisted</a
                    >
                  </li>
                  <li>
                    <a class="dropdown-item" (click)="filterStatus = 'Pending'"
                      >Pending</a
                    >
                  </li>
                  <li>
                    <a class="dropdown-item" (click)="filterStatus = 'Rejected'"
                      >Rejected</a
                    >
                  </li>
                </ul>
              </div>
              <button
                class="btn btn-dark d-flex align-items-center gap-2"
                (click)="openAddModal()"
              >
                <i class="bi bi-plus"></i>
                Add Candidate
              </button>
            </div>
          </div>

          <!-- Stats Cards -->
          <div class="row g-4 mb-4">
            <div class="col-md-3">
              <div class="card border-0 shadow-sm">
                <div class="card-body">
                  <div
                    class="d-flex justify-content-between align-items-center"
                  >
                    <div>
                      <p class="text-muted mb-1">Total Candidates</p>
                      <h3 class="mb-0">{{ stats.total }}</h3>
                    </div>
                    <div class="rounded-circle bg-light p-3">
                      <i class="bi bi-people text-dark"></i>
                    </div>
                  </div>
                </div>
              </div>
            </div>
            <div class="col-md-3">
              <div class="card border-0 shadow-sm">
                <div class="card-body">
                  <div
                    class="d-flex justify-content-between align-items-center"
                  >
                    <div>
                      <p class="text-muted mb-1">Shortlisted Candidates</p>
                      <h3 class="mb-0">{{ stats.shortlisted }}</h3>
                    </div>
                    <div class="rounded-circle bg-light p-3">
                      <i class="bi bi-check-circle text-success"></i>
                    </div>
                  </div>
                </div>
              </div>
            </div>
            <div class="col-md-3">
              <div class="card border-0 shadow-sm">
                <div class="card-body">
                  <div
                    class="d-flex justify-content-between align-items-center"
                  >
                    <div>
                      <p class="text-muted mb-1">Pending Candidates</p>
                      <h3 class="mb-0">{{ stats.pending }}</h3>
                    </div>
                    <div class="rounded-circle bg-light p-3">
                      <i class="bi bi-hourglass text-warning"></i>
                    </div>
                  </div>
                </div>
              </div>
            </div>
            <div class="col-md-3">
              <div class="card border-0 shadow-sm">
                <div class="card-body">
                  <div
                    class="d-flex justify-content-between align-items-center"
                  >
                    <div>
                      <p class="text-muted mb-1">Rejected Candidates</p>
                      <h3 class="mb-0">{{ stats.rejected }}</h3>
                    </div>
                    <div class="rounded-circle bg-light p-3">
                      <i class="bi bi-x-circle text-danger"></i>
                    </div>
                  </div>
                </div>
              </div>
            </div>
          </div>

          <!-- Charts Row -->
          <div class="row g-4 mb-4">
            <div class="col-md-6">
              <div class="card border-0 shadow-sm">
                <div class="card-body">
                  <h5 class="card-title mb-4">Candidate Status Distribution</h5>
                  <canvas #statusChart></canvas>
                </div>
              </div>
            </div>
            <div class="col-md-6">
              <div class="card border-0 shadow-sm">
                <div class="card-body">
                  <h5 class="card-title mb-4">Top 5 Candidate Skills</h5>
                  <canvas #skillsChart></canvas>
                </div>
              </div>
            </div>
          </div>

          <!-- Candidates Table -->
          <div class="card border-0 shadow-sm">
            <div class="table-responsive">
              <table class="table table-hover mb-0">
                <thead class="bg-light">
                  <tr>
                    <th class="border-0 px-4 py-3">Name</th>
                    <th class="border-0 px-4 py-3">Applied For</th>
                    <th class="border-0 px-4 py-3">Test Score</th>
                    <th class="border-0 px-4 py-3">Status</th>
                    <th class="border-0 px-4 py-3 text-end">Actions</th>
                  </tr>
                </thead>
                <tbody>
                  <tr *ngFor="let candidate of paginatedCandidates">
                    <td class="px-4 py-3">{{ candidate.name }}</td>
                    <td class="px-4 py-3">{{ candidate.appliedFor }}</td>
                    <td class="px-4 py-3">
                      {{
                        candidate.testScore
                          ? candidate.testScore + '%'
                          : 'No score'
                      }}
                    </td>
                    <td class="px-4 py-3">
                      <span [class]="getStatusClass(candidate.status)">
                        {{ candidate.status }}
                      </span>
                    </td>
                    <td class="px-4 py-3 text-end">
                      <div class="d-flex gap-3 justify-content-end">
                        <button
                          class="btn btn-link p-0 text-decoration-none"
                          (click)="openEditModal(candidate)"
                        >
                          <i class="bi bi-pencil me-1"></i>
                          Edit
                        </button>
                        <button
                          class="btn btn-link p-0 text-decoration-none"
                          (click)="openRemoveModal(candidate)"
                        >
                          <i class="bi bi-trash me-1"></i>
                          Remove
                        </button>
                        <button
                          *ngIf="candidate.status === 'Pending'"
                          class="btn btn-link p-0 text-decoration-none"
                          (click)="approveCandidate(candidate)"
                        >
                          <i class="bi bi-check-circle me-1"></i>
                          Approve
                        </button>
                      </div>
                    </td>
                  </tr>
                </tbody>
              </table>

              <app-pagination
                [currentPage]="currentPage"
                [pageSize]="pageSize"
                [totalItems]="filteredCandidates.length"
                (pageChange)="onPageChange($event)"
              ></app-pagination>
            </div>
          </div>
        </div>
      </main>

      <!-- Add Candidate Modal -->
      <div
        class="modal"
        [class.show]="showAddModal"
        [style.display]="showAddModal ? 'block' : 'none'"
      >
        <div class="modal-dialog modal-dialog-centered">
          <div class="modal-content border-0">
            <div class="modal-header border-0">
              <h5 class="modal-title">Add New Candidate</h5>
              <button
                type="button"
                class="btn-close"
                (click)="closeAddModal()"
              ></button>
            </div>
            <div class="modal-body">
              <p class="text-muted small mb-4">
                Enter the details of the new candidate.
              </p>

              <div class="mb-3">
                <label class="form-label">Name</label>
                <input
                  type="text"
                  class="form-control"
                  [(ngModel)]="newCandidate.name"
                />
              </div>

              <div class="mb-3">
                <label class="form-label">Email</label>
                <input
                  type="email"
                  class="form-control"
                  [(ngModel)]="newCandidate.email"
                />
              </div>

              <div class="mb-3">
                <label class="form-label">Applied For</label>
                <input
                  type="text"
                  class="form-control"
                  [(ngModel)]="newCandidate.appliedFor"
                />
              </div>

              <div class="mb-3">
                <label class="form-label">Skills</label>
                <input
                  type="text"
                  class="form-control"
                  placeholder="Separate skills with commas"
                  [(ngModel)]="skillsInput"
                />
              </div>
            </div>
            <div class="modal-footer border-0">
              <button
                type="button"
                class="btn btn-dark px-4"
                [disabled]="!isValidCandidate(newCandidate)"
                (click)="addCandidate()"
              >
                Add Candidate
              </button>
            </div>
          </div>
        </div>
      </div>

      <!-- Edit Candidate Modal -->
      <div
        class="modal"
        [class.show]="showEditModal"
        [style.display]="showEditModal ? 'block' : 'none'"
      >
        <div class="modal-dialog modal-dialog-centered">
          <div class="modal-content border-0">
            <div class="modal-header border-0">
              <h5 class="modal-title">Edit Candidate</h5>
              <button
                type="button"
                class="btn-close"
                (click)="closeEditModal()"
              ></button>
            </div>
            <div class="modal-body">
              <p class="text-muted small mb-4">
                Update the candidate's information.
              </p>

              <div class="mb-3">
                <label class="form-label">Name</label>
                <input
                  type="text"
                  class="form-control"
                  [(ngModel)]="editingCandidate.name"
                />
              </div>

              <div class="mb-3">
                <label class="form-label">Email</label>
                <input
                  type="email"
                  class="form-control"
                  [(ngModel)]="editingCandidate.email"
                />
              </div>

              <div class="mb-3">
                <label class="form-label">Applied For</label>
                <input
                  type="text"
                  class="form-control"
                  [(ngModel)]="editingCandidate.appliedFor"
                />
              </div>

              <div class="mb-3">
                <label class="form-label">Status</label>
                <select
                  class="form-select"
                  [(ngModel)]="editingCandidate.status"
                >
                  <option value="Shortlisted">Shortlisted</option>
                  <option value="Pending">Pending</option>
                  <option value="Rejected">Rejected</option>
                </select>
              </div>

              <div class="mb-3">
                <label class="form-label">Test Score</label>
                <input
                  type="number"
                  class="form-control"
                  [(ngModel)]="editingCandidate.testScore"
                />
              </div>

              <div class="mb-3">
                <label class="form-label">Skills</label>
                <input
                  type="text"
                  class="form-control"
                  placeholder="Separate skills with commas"
                  [(ngModel)]="skillsInput"
                />
              </div>
            </div>
            <div class="modal-footer border-0">
              <button
                type="button"
                class="btn btn-dark px-4"
                [disabled]="!isValidCandidate(editingCandidate)"
                (click)="saveChanges()"
              >
                Save changes
              </button>
            </div>
          </div>
        </div>
      </div>

      <!-- Remove Candidate Modal -->
      <div
        class="modal"
        [class.show]="showRemoveModal"
        [style.display]="showRemoveModal ? 'block' : 'none'"
      >
        <div class="modal-dialog modal-dialog-centered">
          <div class="modal-content border-0">
            <div class="modal-header border-0">
              <h5 class="modal-title">Remove Candidate</h5>
              <button
                type="button"
                class="btn-close"
                (click)="closeRemoveModal()"
              ></button>
            </div>
            <div class="modal-body">
              <p>
                Are you sure you want to remove {{ selectedCandidate?.name }}?
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
                (click)="removeCandidate()"
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

      .status-badge {
        padding: 4px 8px;
        border-radius: 4px;
        font-size: 13px;
        font-weight: 500;
      }

      .status-shortlisted {
        background-color: #ecfdf5;
        color: #059669;
      }

      .status-pending {
        background-color: #fef3c7;
        color: #d97706;
      }

      .status-rejected {
        background-color: #fee2e2;
        color: #dc2626;
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
export class CandidateComponent implements AfterViewInit {
  // Pagination
  currentPage = 1;
  pageSize = 10;
  searchQuery = '';

  // Modal states
  showAddModal = false;
  showEditModal = false;
  showRemoveModal = false;

  // Form data
  newCandidate: Partial<Candidate> = {};
  editingCandidate: Partial<Candidate> = {};
  selectedCandidate: Candidate | null = null;
  skillsInput = '';

  // Charts
  @ViewChild('statusChart') statusChartRef!: ElementRef;
  @ViewChild('skillsChart') skillsChartRef!: ElementRef;
  private statusChart: Chart | null = null;
  private skillsChart: Chart | null = null;

  filterStatus = 'All';

  candidates: Candidate[] = [
    {
      id: 1,
      name: 'John Doe',
      email: 'john@example.com',
      appliedFor: 'Frontend Developer',
      testScore: 85,
      status: 'Shortlisted',
      skills: ['React', 'JavaScript', 'CSS'],
    },
    {
      id: 2,
      name: 'Jane Smith',
      email: 'jane@example.com',
      appliedFor: 'Backend Developer',
      testScore: 72,
      status: 'Pending',
      skills: ['Node.js', 'Python', 'SQL'],
    },
    // Add more sample data...
  ];

  get stats(): CandidateStats {
    return {
      total: this.candidates.length,
      shortlisted: this.candidates.filter((c) => c.status === 'Shortlisted')
        .length,
      pending: this.candidates.filter((c) => c.status === 'Pending').length,
      rejected: this.candidates.filter((c) => c.status === 'Rejected').length,
    };
  }

  get filteredCandidates(): Candidate[] {
    return this.candidates.filter(
      (candidate) =>
        (candidate.name
          .toLowerCase()
          .includes(this.searchQuery.toLowerCase()) ||
          candidate.appliedFor
            .toLowerCase()
            .includes(this.searchQuery.toLowerCase())) &&
        (this.filterStatus === 'All' || candidate.status === this.filterStatus)
    );
  }

  get paginatedCandidates(): Candidate[] {
    const start = (this.currentPage - 1) * this.pageSize;
    const end = start + this.pageSize;
    return this.filteredCandidates.slice(start, end);
  }

  ngAfterViewInit() {
    this.initializeCharts();
  }

  private initializeCharts() {
    if (this.statusChartRef && this.skillsChartRef) {
      const statusCtx = this.statusChartRef.nativeElement.getContext('2d');
      const skillsCtx = this.skillsChartRef.nativeElement.getContext('2d');

      if (statusCtx) {
        this.statusChart = new Chart(statusCtx, {
          type: 'bar',
          data: {
            labels: ['Shortlisted', 'Pending', 'Rejected'],
            datasets: [
              {
                data: [
                  this.stats.shortlisted,
                  this.stats.pending,
                  this.stats.rejected,
                ],
                backgroundColor: '#111827',
                borderRadius: 4,
                barThickness: 24,
              },
            ],
          },
          options: {
            responsive: true,
            plugins: {
              legend: {
                display: false,
              },
            },
            scales: {
              y: {
                beginAtZero: true,
                ticks: {
                  stepSize: 1,
                },
              },
            },
          },
        });
      } else {
        console.error('Failed to get 2D context for status chart');
      }

      if (skillsCtx) {
        const skillsCount = this.getTopSkills();
        this.skillsChart = new Chart(skillsCtx, {
          type: 'bar',
          data: {
            labels: Object.keys(skillsCount).slice(0, 5),
            datasets: [
              {
                data: Object.values(skillsCount).slice(0, 5),
                backgroundColor: '#111827',
                borderRadius: 4,
                barThickness: 24,
              },
            ],
          },
          options: {
            responsive: true,
            plugins: {
              legend: {
                display: false,
              },
            },
            scales: {
              y: {
                beginAtZero: true,
                ticks: {
                  stepSize: 1,
                },
              },
            },
          },
        });
      } else {
        console.error('Failed to get 2D context for skills chart');
      }
    } else {
      console.error('Chart references not available');
    }
  }

  private getTopSkills(): Record<string, number> {
    const skillsCount: Record<string, number> = {};
    this.candidates.forEach((candidate) => {
      candidate.skills.forEach((skill) => {
        skillsCount[skill] = (skillsCount[skill] || 0) + 1;
      });
    });
    return Object.fromEntries(
      Object.entries(skillsCount).sort(([, a], [, b]) => b - a)
    );
  }

  getStatusClass(status: string): string {
    return `status-badge status-${status.toLowerCase()}`;
  }

  onPageChange(page: number): void {
    this.currentPage = page;
  }

  // Modal handlers
  openAddModal(): void {
    this.newCandidate = {};
    this.skillsInput = '';
    this.showAddModal = true;
  }

  openEditModal(candidate: Candidate): void {
    this.editingCandidate = { ...candidate };
    this.skillsInput = candidate.skills.join(', ');
    this.showEditModal = true;
  }

  openRemoveModal(candidate: Candidate): void {
    this.selectedCandidate = candidate;
    this.showRemoveModal = true;
  }

  closeAddModal(): void {
    this.showAddModal = false;
    this.newCandidate = {};
    this.skillsInput = '';
  }

  closeEditModal(): void {
    this.showEditModal = false;
    this.editingCandidate = {};
    this.skillsInput = '';
  }

  closeRemoveModal(): void {
    this.showRemoveModal = false;
    this.selectedCandidate = null;
  }

  closeAllModals(): void {
    this.closeAddModal();
    this.closeEditModal();
    this.closeRemoveModal();
  }

  // CRUD operations
  addCandidate(): void {
    if (this.isValidCandidate(this.newCandidate)) {
      const newId = Math.max(...this.candidates.map((c) => c.id)) + 1;
      const candidate: Candidate = {
        id: newId,
        name: this.newCandidate.name!,
        email: this.newCandidate.email!,
        appliedFor: this.newCandidate.appliedFor!,
        testScore: null,
        status: 'Pending',
        skills: this.skillsInput
          .split(',')
          .map((s) => s.trim())
          .filter(Boolean),
      };
      this.candidates.push(candidate);
      this.closeAddModal();
      this.updateCharts();
    }
  }

  saveChanges(): void {
    if (this.isValidCandidate(this.editingCandidate)) {
      const index = this.candidates.findIndex(
        (c) => c.id === this.editingCandidate.id
      );
      if (index !== -1) {
        this.candidates[index] = {
          ...(this.editingCandidate as Candidate),
          skills: this.skillsInput
            .split(',')
            .map((s) => s.trim())
            .filter(Boolean),
        };
        this.closeEditModal();
        this.updateCharts();
      }
    }
  }

  removeCandidate(): void {
    if (this.selectedCandidate) {
      this.candidates = this.candidates.filter(
        (c) => c.id !== this.selectedCandidate!.id
      );
      this.closeRemoveModal();
      this.updateCharts();
    }
  }

  approveCandidate(candidate: Candidate): void {
    const index = this.candidates.findIndex((c) => c.id === candidate.id);
    if (index !== -1) {
      this.candidates[index].status = 'Shortlisted';
      this.updateCharts();
    }
  }

  private updateCharts(): void {
    if (this.statusChart) {
      this.statusChart.data.datasets[0].data = [
        this.stats.shortlisted,
        this.stats.pending,
        this.stats.rejected,
      ];
      this.statusChart.update();
    }

    if (this.skillsChart) {
      const skillsCount = this.getTopSkills();
      this.skillsChart.data.labels = Object.keys(skillsCount).slice(0, 5);
      this.skillsChart.data.datasets[0].data = Object.values(skillsCount).slice(
        0,
        5
      );
      this.skillsChart.update();
    }
  }

  public isValidCandidate(candidate: Partial<Candidate>): boolean {
    return !!(candidate.name && candidate.email && candidate.appliedFor);
  }
}
