import {
  Component,
  type OnInit,
  ViewChildren,
  type QueryList,
  type ElementRef,
} from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { SidebarComponent } from '../sidebar/sidebar.component';

interface Candidate {
  id: number;
  name: string;
  email: string;
  experience: string;
  skills: string[];
  appliedDate: string;
  avatar: string;
}

interface Job {
  id: number;
  title: string;
  department: string;
  location: string;
  description: string;
  postedDate: string;
  salary: string;
  candidates: {
    applied: Candidate[];
    rejected: Candidate[];
    shortlisted: Candidate[];
    selected: Candidate[];
  };
  expanded: boolean;
  displayCount: {
    applied: number;
    rejected: number;
    shortlisted: number;
    selected: number;
  };
  activeTab: 'applied' | 'rejected' | 'shortlisted' | 'selected';
}

@Component({
  selector: 'app-jobs',
  standalone: true,
  imports: [CommonModule, FormsModule, SidebarComponent],
  template: `
    <div class="d-flex">
      <app-sidebar></app-sidebar>

      <main class="main-content bg-light">
        <div class="p-4">
          <h1 class="h4 mb-4">Jobs</h1>

          <!-- Search and filter -->
          <div class="card border-0 shadow-sm mb-4">
            <div class="card-body">
              <div class="row g-3">
                <div class="col-md-6">
                  <input
                    type="text"
                    class="form-control"
                    placeholder="Search jobs..."
                    [(ngModel)]="searchQuery"
                    (ngModelChange)="filterJobs()"
                  />
                </div>
                <div class="col-md-3">
                  <select
                    class="form-select"
                    [(ngModel)]="filterDepartment"
                    (ngModelChange)="filterJobs()"
                  >
                    <option value="">All Departments</option>
                    <option value="Engineering">Engineering</option>
                    <option value="Design">Design</option>
                    <option value="Marketing">Marketing</option>
                    <option value="Sales">Sales</option>
                  </select>
                </div>
                <div class="col-md-3">
                  <select
                    class="form-select"
                    [(ngModel)]="filterLocation"
                    (ngModelChange)="filterJobs()"
                  >
                    <option value="">All Locations</option>
                    <option value="Remote">Remote</option>
                    <option value="New York">New York</option>
                    <option value="San Francisco">San Francisco</option>
                    <option value="London">London</option>
                  </select>
                </div>
              </div>
            </div>
          </div>

          <!-- Jobs list -->
          <div class="row row-cols-1 row-cols-md-2 row-cols-xl-3 g-4">
            <div class="col" *ngFor="let job of displayedJobs; let i = index">
              <div
                class="card border-0 shadow-sm h-100 job-card"
                [class.expanded]="job.expanded"
              >
                <div class="card-body">
                  <div
                    class="d-flex justify-content-between align-items-center mb-3"
                  >
                    <h5 class="card-title mb-0">{{ job.title }}</h5>
                    <span class="badge bg-primary">{{ job.department }}</span>
                  </div>
                  <p class="card-text text-muted mb-2">
                    <i class="bi bi-geo-alt me-1"></i>{{ job.location }}
                  </p>
                  <p class="card-text text-muted mb-2">
                    <i class="bi bi-calendar me-1"></i>Posted:
                    {{ job.postedDate }}
                  </p>
                  <p class="card-text text-muted mb-3">
                    <i class="bi bi-cash me-1"></i>{{ job.salary }}
                  </p>
                  <p class="card-text mb-3">{{ job.description }}</p>
                  <button
                    class="btn btn-outline-primary w-100"
                    (click)="toggleJobExpansion(job)"
                  >
                    {{ job.expanded ? 'Hide Details' : 'View Details' }}
                  </button>
                </div>

                <!-- Expanded job details -->
                <div
                  class="card-footer bg-white border-top-0"
                  *ngIf="job.expanded"
                >
                  <!-- Tabs -->
                  <ul class="nav nav-tabs" role="tablist">
                    <li class="nav-item" role="presentation">
                      <button
                        class="nav-link"
                        [class.active]="job.activeTab === 'applied'"
                        [id]="'applied-tab-' + job.id"
                        (click)="changeTab(job, 'applied')"
                        type="button"
                        role="tab"
                        [attr.aria-controls]="'applied-' + job.id"
                        [attr.aria-selected]="job.activeTab === 'applied'"
                      >
                        Applied ({{ job.candidates.applied.length }})
                      </button>
                    </li>
                    <li class="nav-item" role="presentation">
                      <button
                        class="nav-link"
                        [class.active]="job.activeTab === 'rejected'"
                        [id]="'rejected-tab-' + job.id"
                        (click)="changeTab(job, 'rejected')"
                        type="button"
                        role="tab"
                        [attr.aria-controls]="'rejected-' + job.id"
                        [attr.aria-selected]="job.activeTab === 'rejected'"
                      >
                        Rejected ({{ job.candidates.rejected.length }})
                      </button>
                    </li>
                    <li class="nav-item" role="presentation">
                      <button
                        class="nav-link"
                        [class.active]="job.activeTab === 'shortlisted'"
                        [id]="'shortlisted-tab-' + job.id"
                        (click)="changeTab(job, 'shortlisted')"
                        type="button"
                        role="tab"
                        [attr.aria-controls]="'shortlisted-' + job.id"
                        [attr.aria-selected]="job.activeTab === 'shortlisted'"
                      >
                        Shortlisted ({{ job.candidates.shortlisted.length }})
                      </button>
                    </li>
                    <li class="nav-item" role="presentation">
                      <button
                        class="nav-link"
                        [class.active]="job.activeTab === 'selected'"
                        [id]="'selected-tab-' + job.id"
                        (click)="changeTab(job, 'selected')"
                        type="button"
                        role="tab"
                        [attr.aria-controls]="'selected-' + job.id"
                        [attr.aria-selected]="job.activeTab === 'selected'"
                      >
                        Selected ({{ job.candidates.selected.length }})
                      </button>
                    </li>
                  </ul>

                  <!-- Tab content -->
                  <div class="tab-content mt-3">
                    <div
                      class="tab-pane fade"
                      [class.show]="job.activeTab === 'applied'"
                      [class.active]="job.activeTab === 'applied'"
                      [id]="'applied-' + job.id"
                      role="tabpanel"
                      [attr.aria-labelledby]="'applied-tab-' + job.id"
                    >
                      <div
                        class="candidate-list"
                        #candidateList
                        (scroll)="onScroll($event, job, 'applied')"
                      >
                        <div
                          *ngFor="
                            let candidate of job.candidates.applied.slice(
                              0,
                              job.displayCount.applied
                            )
                          "
                          class="candidate-item"
                        >
                          <div class="d-flex align-items-center mb-2">
                            <img
                              [src]="candidate.avatar"
                              alt="{{ candidate.name }}"
                              class="rounded-circle me-2"
                              width="40"
                              height="40"
                            />
                            <div>
                              <h6 class="mb-0">{{ candidate.name }}</h6>
                              <p class="mb-0 text-muted small">
                                {{ candidate.email }}
                              </p>
                            </div>
                          </div>
                          <p class="mb-1 text-muted small">
                            Experience: {{ candidate.experience }}
                          </p>
                          <p class="mb-2 text-muted small">
                            Applied: {{ candidate.appliedDate }}
                          </p>
                          <div class="mb-2">
                            <span
                              *ngFor="let skill of candidate.skills"
                              class="badge bg-light text-dark me-1"
                              >{{ skill }}</span
                            >
                          </div>
                          <div class="d-flex justify-content-end">
                            <button
                              class="btn btn-outline-success btn-sm me-2"
                              (click)="shortlistCandidate(job, candidate)"
                            >
                              Shortlist
                            </button>
                            <button
                              class="btn btn-outline-danger btn-sm"
                              (click)="rejectCandidate(job, candidate)"
                            >
                              Reject
                            </button>
                          </div>
                        </div>
                      </div>
                    </div>
                    <div
                      class="tab-pane fade"
                      [class.show]="job.activeTab === 'rejected'"
                      [class.active]="job.activeTab === 'rejected'"
                      [id]="'rejected-' + job.id"
                      role="tabpanel"
                      [attr.aria-labelledby]="'rejected-tab-' + job.id"
                    >
                      <div
                        class="candidate-list"
                        (scroll)="onScroll($event, job, 'rejected')"
                      >
                        <div
                          *ngFor="
                            let candidate of job.candidates.rejected.slice(
                              0,
                              job.displayCount.rejected
                            )
                          "
                          class="candidate-item"
                        >
                          <div class="d-flex align-items-center mb-2">
                            <img
                              [src]="candidate.avatar"
                              alt="{{ candidate.name }}"
                              class="rounded-circle me-2"
                              width="40"
                              height="40"
                            />
                            <div>
                              <h6 class="mb-0">{{ candidate.name }}</h6>
                              <p class="mb-0 text-muted small">
                                {{ candidate.email }}
                              </p>
                            </div>
                          </div>
                          <p class="mb-1 text-muted small">
                            Experience: {{ candidate.experience }}
                          </p>
                          <p class="mb-2 text-muted small">
                            Applied: {{ candidate.appliedDate }}
                          </p>
                          <div>
                            <span
                              *ngFor="let skill of candidate.skills"
                              class="badge bg-light text-dark me-1"
                              >{{ skill }}</span
                            >
                          </div>
                        </div>
                      </div>
                    </div>
                    <div
                      class="tab-pane fade"
                      [class.show]="job.activeTab === 'shortlisted'"
                      [class.active]="job.activeTab === 'shortlisted'"
                      [id]="'shortlisted-' + job.id"
                      role="tabpanel"
                      [attr.aria-labelledby]="'shortlisted-tab-' + job.id"
                    >
                      <div
                        class="candidate-list"
                        (scroll)="onScroll($event, job, 'shortlisted')"
                      >
                        <div
                          *ngFor="
                            let candidate of job.candidates.shortlisted.slice(
                              0,
                              job.displayCount.shortlisted
                            )
                          "
                          class="candidate-item"
                        >
                          <div class="d-flex align-items-center mb-2">
                            <img
                              [src]="candidate.avatar"
                              alt="{{ candidate.name }}"
                              class="rounded-circle me-2"
                              width="40"
                              height="40"
                            />
                            <div>
                              <h6 class="mb-0">{{ candidate.name }}</h6>
                              <p class="mb-0 text-muted small">
                                {{ candidate.email }}
                              </p>
                            </div>
                          </div>
                          <p class="mb-1 text-muted small">
                            Experience: {{ candidate.experience }}
                          </p>
                          <p class="mb-2 text-muted small">
                            Applied: {{ candidate.appliedDate }}
                          </p>
                          <div>
                            <span
                              *ngFor="let skill of candidate.skills"
                              class="badge bg-light text-dark me-1"
                              >{{ skill }}</span
                            >
                          </div>
                        </div>
                      </div>
                    </div>
                    <div
                      class="tab-pane fade"
                      [class.show]="job.activeTab === 'selected'"
                      [class.active]="job.activeTab === 'selected'"
                      [id]="'selected-' + job.id"
                      role="tabpanel"
                      [attr.aria-labelledby]="'selected-tab-' + job.id"
                    >
                      <div
                        class="candidate-list"
                        (scroll)="onScroll($event, job, 'selected')"
                      >
                        <div
                          *ngFor="
                            let candidate of job.candidates.selected.slice(
                              0,
                              job.displayCount.selected
                            )
                          "
                          class="candidate-item"
                        >
                          <div class="d-flex align-items-center mb-2">
                            <img
                              [src]="candidate.avatar"
                              alt="{{ candidate.name }}"
                              class="rounded-circle me-2"
                              width="40"
                              height="40"
                            />
                            <div>
                              <h6 class="mb-0">{{ candidate.name }}</h6>
                              <p class="mb-0 text-muted small">
                                {{ candidate.email }}
                              </p>
                            </div>
                          </div>
                          <p class="mb-1 text-muted small">
                            Experience: {{ candidate.experience }}
                          </p>
                          <p class="mb-2 text-muted small">
                            Applied: {{ candidate.appliedDate }}
                          </p>
                          <div>
                            <span
                              *ngFor="let skill of candidate.skills"
                              class="badge bg-light text-dark me-1"
                              >{{ skill }}</span
                            >
                          </div>
                        </div>
                      </div>
                    </div>
                  </div>
                </div>
              </div>
            </div>
          </div>

          <!-- View More button -->
          <div
            class="text-center mt-4"
            *ngIf="displayedJobs.length < filteredJobs.length"
          >
            <button class="btn btn-primary" (click)="loadMoreJobs()">
              View More
            </button>
          </div>
        </div>
      </main>
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
        transition: all 0.3s ease;
      }

      .job-card:hover {
        transform: translateY(-5px);
        box-shadow: 0 10px 20px rgba(0, 0, 0, 0.1) !important;
      }

      .job-card.expanded {
        transform: none;
      }

      .nav-tabs {
        border-bottom: 1px solid #e5e7eb;
      }

      .nav-tabs .nav-link {
        border: none;
        color: #6b7280;
        font-size: 0.875rem;
        padding: 0.5rem 1rem;
        transition: all 0.2s ease;
      }

      .nav-tabs .nav-link:hover {
        color: #111827;
      }

      .nav-tabs .nav-link.active {
        color: #111827;
        font-weight: 500;
        border-bottom: 2px solid #111827;
      }

      .candidate-list {
        max-height: 300px;
        overflow-y: auto;
      }

      .candidate-item {
        padding: 1rem;
        border-bottom: 1px solid #e5e7eb;
        transition: background-color 0.2s ease;
      }

      .candidate-item:hover {
        background-color: #f9fafb;
      }

      .candidate-item:last-child {
        border-bottom: none;
      }

      .badge {
        font-weight: 500;
      }

      .btn-sm {
        padding: 0.25rem 0.5rem;
        font-size: 0.75rem;
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
export class JobsComponent implements OnInit {
  @ViewChildren('candidateList') candidateLists!: QueryList<ElementRef>;

  jobs: Job[] = [];
  filteredJobs: Job[] = [];
  displayedJobs: Job[] = [];
  searchQuery = '';
  filterDepartment = '';
  filterLocation = '';
  jobsPerPage = 6;

  ngOnInit() {
    this.jobs = this.generateMockJobs();
    this.filteredJobs = [...this.jobs];
    this.displayedJobs = this.filteredJobs.slice(0, this.jobsPerPage);
  }

  filterJobs() {
    this.filteredJobs = this.jobs.filter((job) => {
      const matchesSearch =
        job.title.toLowerCase().includes(this.searchQuery.toLowerCase()) ||
        job.department.toLowerCase().includes(this.searchQuery.toLowerCase()) ||
        job.location.toLowerCase().includes(this.searchQuery.toLowerCase());
      const matchesDepartment =
        !this.filterDepartment || job.department === this.filterDepartment;
      const matchesLocation =
        !this.filterLocation || job.location === this.filterLocation;
      return matchesSearch && matchesDepartment && matchesLocation;
    });
    this.displayedJobs = this.filteredJobs.slice(0, this.jobsPerPage);
  }

  loadMoreJobs() {
    const currentLength = this.displayedJobs.length;
    const nextJobs = this.filteredJobs.slice(
      currentLength,
      currentLength + this.jobsPerPage
    );
    this.displayedJobs = [...this.displayedJobs, ...nextJobs];
  }

  toggleJobExpansion(job: Job) {
    job.expanded = !job.expanded;
    if (job.expanded) {
      this.setInitialDisplayCount(job);
    }
  }

  setInitialDisplayCount(job: Job) {
    setTimeout(() => {
      const candidateList = this.candidateLists.find(
        (item, index) =>
          index === this.displayedJobs.findIndex((j) => j.id === job.id)
      );
      if (candidateList) {
        const containerHeight = candidateList.nativeElement.clientHeight;
        const itemHeight = 100; // Approximate height of a candidate item
        const initialCount = Math.floor(containerHeight / itemHeight);
        job.displayCount = {
          applied: initialCount,
          rejected: initialCount,
          shortlisted: initialCount,
          selected: initialCount,
        };
      } else {
        job.displayCount = {
          applied: 5,
          rejected: 5,
          shortlisted: 5,
          selected: 5,
        };
      }
    });
  }

  changeTab(
    job: Job,
    tab: 'applied' | 'rejected' | 'shortlisted' | 'selected'
  ) {
    job.activeTab = tab;
    // Simulating data fetching for the selected tab
    this.fetchCandidates(job, tab);
  }

  fetchCandidates(
    job: Job,
    tab: 'applied' | 'rejected' | 'shortlisted' | 'selected'
  ) {
    // Simulating an API call to fetch candidates
    setTimeout(() => {
      job.candidates[tab] = this.generateMockCandidates(
        Math.floor(Math.random() * 20) + 5
      );
      job.displayCount[tab] = Math.min(
        job.candidates[tab].length,
        job.displayCount[tab]
      );
    }, 500);
  }

  onScroll(
    event: any,
    job: Job,
    tab: 'applied' | 'rejected' | 'shortlisted' | 'selected'
  ) {
    const element = event.target;
    if (element.scrollHeight - element.scrollTop === element.clientHeight) {
      if (job.displayCount[tab] < job.candidates[tab].length) {
        job.displayCount[tab] += Math.min(
          5,
          job.candidates[tab].length - job.displayCount[tab]
        );
      }
    }
  }

  shortlistCandidate(job: Job, candidate: Candidate) {
    const index = job.candidates.applied.findIndex(
      (c) => c.id === candidate.id
    );
    if (index !== -1) {
      job.candidates.applied.splice(index, 1);
      job.candidates.shortlisted.push(candidate);
    }
  }

  rejectCandidate(job: Job, candidate: Candidate) {
    const index = job.candidates.applied.findIndex(
      (c) => c.id === candidate.id
    );
    if (index !== -1) {
      job.candidates.applied.splice(index, 1);
      job.candidates.rejected.push(candidate);
    }
  }

  private generateMockJobs(): Job[] {
    const mockJobs: Job[] = [];
    const departments = ['Engineering', 'Design', 'Marketing', 'Sales'];
    const locations = ['Remote', 'New York', 'San Francisco', 'London'];
    const skills = [
      'JavaScript',
      'React',
      'Node.js',
      'Python',
      'UI/UX',
      'SEO',
      'Sales',
      'Communication',
    ];

    for (let i = 1; i <= 20; i++) {
      mockJobs.push({
        id: i,
        title: `Job ${i}`,
        department: departments[Math.floor(Math.random() * departments.length)],
        location: locations[Math.floor(Math.random() * locations.length)],
        description: `This is a description for Job ${i}. It includes details about the role and responsibilities.`,
        postedDate: new Date(
          Date.now() - Math.floor(Math.random() * 30) * 24 * 60 * 60 * 1000
        ).toLocaleDateString(),
        salary: `$${Math.floor(Math.random() * 50 + 50)}k - $${Math.floor(
          Math.random() * 50 + 100
        )}k`,
        candidates: {
          applied: this.generateMockCandidates(10),
          rejected: this.generateMockCandidates(5),
          shortlisted: this.generateMockCandidates(3),
          selected: this.generateMockCandidates(2),
        },
        expanded: false,
        displayCount: {
          applied: 5,
          rejected: 5,
          shortlisted: 5,
          selected: 5,
        },
        activeTab: 'applied',
      });
    }

    return mockJobs;
  }

  private generateMockCandidates(count: number): Candidate[] {
    const candidates: Candidate[] = [];
    const skills = [
      'JavaScript',
      'React',
      'Node.js',
      'Python',
      'UI/UX',
      'SEO',
      'Sales',
      'Communication',
    ];

    for (let i = 1; i <= count; i++) {
      candidates.push({
        id: i,
        name: `Candidate ${i}`,
        email: `candidate${i}@example.com`,
        experience: `${Math.floor(Math.random() * 10) + 1} years`,
        skills: skills.slice(0, Math.floor(Math.random() * 3) + 2),
        appliedDate: new Date(
          Date.now() - Math.floor(Math.random() * 30) * 24 * 60 * 60 * 1000
        ).toLocaleDateString(),
        avatar: `https://i.pravatar.cc/40?img=${Math.floor(
          Math.random() * 70
        )}`,
      });
    }

    return candidates;
  }
}
