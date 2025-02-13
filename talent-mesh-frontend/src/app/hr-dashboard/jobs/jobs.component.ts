import { Component, type OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatDialogModule,  MatDialog } from '@angular/material/dialog';
import  { MatSnackBar } from '@angular/material/snack-bar';
import { SidebarComponent } from '../sidebar/sidebar.component';
import { RejectModalComponent } from '../reject-modal/reject-modal.component';
import { CandidateProfileModalComponent } from '../candidate-profile/candidate-profile.component';

interface Candidate {
  id: number;
  name: string;
  email: string;
  phone: string;
  stage:
    | 'applied'
    | 'passed_mcq'
    | 'shortlisted'
    | 'interviewed'
    | 'selected'
    | 'rejected';
  timestamp: Date;
  skills: string[];
  education: {
    degree: string;
    university: string;
    year: string;
    score: string;
  }[];
  experience: {
    title: string;
    company: string;
    duration: string;
    description: string;
  }[];
  assessments: {
    type: string;
    score: number;
    maxScore: number;
    status: 'passed' | 'failed';
  }[];
}

interface Job {
  id: number;
  title: string;
  department: string;
  status: 'active' | 'closed';
  postedDate: Date;
  candidates: Candidate[];
}

@Component({
  selector: 'app-job',
  standalone: true,
  imports: [CommonModule, SidebarComponent, MatDialogModule],
  template: `
    <div class="d-flex">
      <app-sidebar></app-sidebar>

      <main class="main-content bg-light">
        <div class="container-fluid py-4">
          <h1 class="h3 mb-4">Hiring Process Timeline</h1>

          <!-- Job Postings Section -->
          <div class="row mb-4">
            <div class="col-md-4">
              <div class="card border-0 shadow-sm">
                <div class="card-body">
                  <h5 class="card-title mb-4">Job Postings</h5>

                  <div class="job-list">
                    <div
                      *ngFor="let job of jobs"
                      class="job-card p-3 mb-3 rounded"
                      [class.active]="selectedJob?.id === job.id"
                      (click)="selectJob(job)"
                    >
                      <div
                        class="d-flex justify-content-between align-items-start mb-2"
                      >
                        <h6 class="mb-1">{{ job.title }}</h6>
                        <span
                          class="badge"
                          [class.bg-success]="job.status === 'active'"
                          [class.bg-secondary]="job.status === 'closed'"
                        >
                          {{ job.status }}
                        </span>
                      </div>
                      <p class="text-muted small mb-2">{{ job.department }}</p>
                      <p class="text-muted small mb-0">
                        Posted on: {{ job.postedDate | date }}
                      </p>
                    </div>
                  </div>
                </div>
              </div>
            </div>

            <!-- Timeline Section -->
            <div class="col-md-8" *ngIf="selectedJob">
              <div class="card border-0 shadow-sm">
                <div class="card-body">
                  <h5 class="card-title mb-4">
                    Hiring Timeline for {{ selectedJob.title }}
                  </h5>

                  <div class="timeline">
                    <!-- Applied Stage -->
                    <div class="timeline-item">
                      <div class="timeline-marker bg-primary">1</div>
                      <div class="timeline-content">
                        <div
                          class="timeline-header"
                          (click)="toggleStage('applied')"
                        >
                          <h6 class="mb-0">Applied</h6>
                          <span class="badge bg-light text-dark">
                            {{ getCandidatesByStage('applied').length }}
                          </span>
                          <i
                            class="bi"
                            [class.bi-chevron-down]="!expandedStages.applied"
                            [class.bi-chevron-up]="expandedStages.applied"
                          ></i>
                        </div>

                        <div
                          class="timeline-body"
                          *ngIf="expandedStages.applied"
                        >
                          <div
                            *ngFor="
                              let candidate of getCandidatesByStage('applied')
                            "
                            class="candidate-card p-3 mb-2 bg-white rounded shadow-sm"
                            (click)="openCandidateProfile(candidate)"
                          >
                            <div
                              class="d-flex justify-content-between align-items-center"
                            >
                              <div>
                                <h6 class="mb-1">{{ candidate.name }}</h6>
                                <p class="text-muted small mb-0">
                                  {{ candidate.email }}
                                </p>
                              </div>
                              <div class="d-flex gap-2">
                                <button
                                  class="btn btn-sm btn-outline-success"
                                  (click)="moveForward(candidate)"
                                >
                                  Move Forward
                                </button>
                                <button
                                  class="btn btn-sm btn-outline-danger"
                                  (click)="openRejectModal(candidate)"
                                >
                                  Reject
                                </button>
                              </div>
                            </div>
                          </div>
                        </div>
                      </div>
                    </div>

                    <!-- Passed MCQ Stage -->
                    <div class="timeline-item">
                      <div class="timeline-marker bg-info">2</div>
                      <div class="timeline-content">
                        <div
                          class="timeline-header"
                          (click)="toggleStage('passed_mcq')"
                        >
                          <h6 class="mb-0">Passed MCQ</h6>
                          <span class="badge bg-light text-dark">
                            {{ getCandidatesByStage('passed_mcq').length }}
                          </span>
                          <i
                            class="bi"
                            [class.bi-chevron-down]="!expandedStages.passed_mcq"
                            [class.bi-chevron-up]="expandedStages.passed_mcq"
                          ></i>
                        </div>

                        <div
                          class="timeline-body"
                          *ngIf="expandedStages.passed_mcq"
                        >
                          <div
                            *ngFor="
                              let candidate of getCandidatesByStage(
                                'passed_mcq'
                              )
                            "
                            class="candidate-card p-3 mb-2 bg-white rounded shadow-sm"
                            (click)="openCandidateProfile(candidate)"
                          >
                            <div
                              class="d-flex justify-content-between align-items-center"
                            >
                              <div>
                                <h6 class="mb-1">{{ candidate.name }}</h6>
                                <p class="text-muted small mb-0">
                                  {{ candidate.email }}
                                </p>
                              </div>
                              <div class="d-flex gap-2">
                                <button
                                  class="btn btn-sm btn-outline-success"
                                  (click)="moveForward(candidate)"
                                >
                                  Move Forward
                                </button>
                                <button
                                  class="btn btn-sm btn-outline-danger"
                                  (click)="openRejectModal(candidate)"
                                >
                                  Reject
                                </button>
                              </div>
                            </div>
                          </div>
                        </div>
                      </div>
                    </div>

                    <!-- Shortlisted Stage -->
                    <div class="timeline-item">
                      <div class="timeline-marker bg-warning">3</div>
                      <div class="timeline-content">
                        <div
                          class="timeline-header"
                          (click)="toggleStage('shortlisted')"
                        >
                          <h6 class="mb-0">Shortlisted</h6>
                          <span class="badge bg-light text-dark">
                            {{ getCandidatesByStage('shortlisted').length }}
                          </span>
                          <i
                            class="bi"
                            [class.bi-chevron-down]="
                              !expandedStages.shortlisted
                            "
                            [class.bi-chevron-up]="expandedStages.shortlisted"
                          ></i>
                        </div>

                        <div
                          class="timeline-body"
                          *ngIf="expandedStages.shortlisted"
                        >
                          <div
                            *ngFor="
                              let candidate of getCandidatesByStage(
                                'shortlisted'
                              )
                            "
                            class="candidate-card p-3 mb-2 bg-white rounded shadow-sm"
                            (click)="openCandidateProfile(candidate)"
                          >
                            <div
                              class="d-flex justify-content-between align-items-center"
                            >
                              <div>
                                <h6 class="mb-1">{{ candidate.name }}</h6>
                                <p class="text-muted small mb-0">
                                  {{ candidate.email }}
                                </p>
                              </div>
                              <div class="d-flex gap-2">
                                <button
                                  class="btn btn-sm btn-outline-success"
                                  (click)="moveForward(candidate)"
                                >
                                  Move Forward
                                </button>
                                <button
                                  class="btn btn-sm btn-outline-danger"
                                  (click)="openRejectModal(candidate)"
                                >
                                  Reject
                                </button>
                              </div>
                            </div>
                          </div>
                        </div>
                      </div>
                    </div>

                    <!-- Interviewed Stage -->
                    <div class="timeline-item">
                      <div class="timeline-marker bg-success">4</div>
                      <div class="timeline-content">
                        <div
                          class="timeline-header"
                          (click)="toggleStage('interviewed')"
                        >
                          <h6 class="mb-0">Interviewed</h6>
                          <span class="badge bg-light text-dark">
                            {{ getCandidatesByStage('interviewed').length }}
                          </span>
                          <i
                            class="bi"
                            [class.bi-chevron-down]="
                              !expandedStages.interviewed
                            "
                            [class.bi-chevron-up]="expandedStages.interviewed"
                          ></i>
                        </div>

                        <div
                          class="timeline-body"
                          *ngIf="expandedStages.interviewed"
                        >
                          <div
                            *ngFor="
                              let candidate of getCandidatesByStage(
                                'interviewed'
                              )
                            "
                            class="candidate-card p-3 mb-2 bg-white rounded shadow-sm"
                            (click)="openCandidateProfile(candidate)"
                          >
                            <div
                              class="d-flex justify-content-between align-items-center"
                            >
                              <div>
                                <h6 class="mb-1">{{ candidate.name }}</h6>
                                <p class="text-muted small mb-0">
                                  {{ candidate.email }}
                                </p>
                              </div>
                              <div class="d-flex gap-2">
                                <button
                                  class="btn btn-sm btn-outline-success"
                                  (click)="moveForward(candidate)"
                                >
                                  Move Forward
                                </button>
                                <button
                                  class="btn btn-sm btn-outline-danger"
                                  (click)="openRejectModal(candidate)"
                                >
                                  Reject
                                </button>
                              </div>
                            </div>
                          </div>
                        </div>
                      </div>
                    </div>

                    <!-- Selected Stage -->
                    <div class="timeline-item">
                      <div class="timeline-marker bg-primary">5</div>
                      <div class="timeline-content">
                        <div
                          class="timeline-header"
                          (click)="toggleStage('selected')"
                        >
                          <h6 class="mb-0">Selected</h6>
                          <span class="badge bg-light text-dark">
                            {{ getCandidatesByStage('selected').length }}
                          </span>
                          <i
                            class="bi"
                            [class.bi-chevron-down]="!expandedStages.selected"
                            [class.bi-chevron-up]="expandedStages.selected"
                          ></i>
                        </div>

                        <div
                          class="timeline-body"
                          *ngIf="expandedStages.selected"
                        >
                          <div
                            *ngFor="
                              let candidate of getCandidatesByStage('selected')
                            "
                            class="candidate-card p-3 mb-2 bg-white rounded shadow-sm"
                            (click)="openCandidateProfile(candidate)"
                          >
                            <div
                              class="d-flex justify-content-between align-items-center"
                            >
                              <div>
                                <h6 class="mb-1">{{ candidate.name }}</h6>
                                <p class="text-muted small mb-0">
                                  {{ candidate.email }}
                                </p>
                              </div>
                            </div>
                          </div>
                        </div>
                      </div>
                    </div>

                    <!-- Rejected Stage -->
                    <div class="timeline-item">
                      <div class="timeline-marker bg-danger">6</div>
                      <div class="timeline-content">
                        <div
                          class="timeline-header"
                          (click)="toggleStage('rejected')"
                        >
                          <h6 class="mb-0">Rejected</h6>
                          <span class="badge bg-light text-dark">
                            {{ getCandidatesByStage('rejected').length }}
                          </span>
                          <i
                            class="bi"
                            [class.bi-chevron-down]="!expandedStages.rejected"
                            [class.bi-chevron-up]="expandedStages.rejected"
                          ></i>
                        </div>

                        <div
                          class="timeline-body"
                          *ngIf="expandedStages.rejected"
                        >
                          <div
                            *ngFor="
                              let candidate of getCandidatesByStage('rejected')
                            "
                            class="candidate-card p-3 mb-2 bg-white rounded shadow-sm"
                            (click)="openCandidateProfile(candidate)"
                          >
                            <div
                              class="d-flex justify-content-between align-items-center"
                            >
                              <div>
                                <h6 class="mb-1">{{ candidate.name }}</h6>
                                <p class="text-muted small mb-0">
                                  {{ candidate.email }}
                                </p>
                              </div>
                            </div>
                          </div>
                        </div>
                      </div>
                    </div>
                  </div>
                </div>
              </div>
            </div>
          </div>
        </div>
      </main>
    </div>
  `,
  styles: [
    `
      .main-content {
        margin-left: 240px;
        width: calc(100% - 240px);
        min-height: 100vh;
      }

      .job-card {
        background-color: #f8f9fa;
        border: 1px solid #e9ecef;
        cursor: pointer;
        transition: all 0.2s ease;
      }

      .job-card:hover {
        background-color: #e9ecef;
      }

      .job-card.active {
        background-color: #e9ecef;
        border-color: #dee2e6;
      }

      .timeline {
        position: relative;
        padding: 1rem 0;
      }

      .timeline-item {
        position: relative;
        padding-left: 3rem;
        margin-bottom: 2rem;
      }

      .timeline-item:last-child {
        margin-bottom: 0;
      }

      .timeline-item::before {
        content: '';
        position: absolute;
        left: 15px;
        top: 30px;
        bottom: -30px;
        width: 2px;
        background-color: #dee2e6;
      }

      .timeline-item:last-child::before {
        display: none;
      }

      .timeline-marker {
        position: absolute;
        left: 0;
        top: 0;
        width: 32px;
        height: 32px;
        border-radius: 50%;
        display: flex;
        align-items: center;
        justify-content: center;
        color: white;
        font-weight: 600;
      }

      .timeline-header {
        display: flex;
        align-items: center;
        gap: 1rem;
        padding: 0.5rem 1rem;
        background-color: #f8f9fa;
        border-radius: 0.5rem;
        cursor: pointer;
        transition: all 0.2s ease;
      }

      .timeline-header:hover {
        background-color: #e9ecef;
      }

      .timeline-body {
        padding: 1rem 0;
      }

      .candidate-card {
        transition: all 0.2s ease;
      }

      .candidate-card:hover {
        transform: translateY(-2px);
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
export class JobComponent implements OnInit {
  jobs: Job[] = [
    {
      id: 1,
      title: 'DevOps Developer',
      department: 'DevOps',
      status: 'active',
      postedDate: new Date('2025-01-25'),
      candidates: [
        {
          id: 1,
          name: 'David Lee',
          email: 'david.l@example.com',
          phone: '+1 (555) 876-5432',
          stage: 'applied',
          timestamp: new Date(),
          skills: ['AWS', 'Terraform', 'Jenkins', 'Python'],
          education: [
            {
              degree: 'Master of Science in Software Engineering',
              university: 'Carnegie Mellon University',
              year: '2024',
              score: '3.9 GPA',
            },
          ],
          experience: [
            {
              title: 'DevOps Engineer',
              company: 'Tech Corp',
              duration: '2020 - 2023',
              description:
                'Led infrastructure automation and CI/CD pipeline development.',
            },
          ],
          assessments: [
            {
              type: 'Technical MCQ',
              score: 85,
              maxScore: 100,
              status: 'passed',
            },
          ],
        },
        {
          id: 2,
          name: 'Emily Johnson',
          email: 'emily.j@example.com',
          phone: '+1 (555) 123-4567',
          stage: 'passed_mcq',
          timestamp: new Date(),
          skills: ['React', 'Angular', 'TypeScript', 'JavaScript'],
          education: [
            {
              degree: 'Bachelor of Science in Computer Science',
              university: 'University of California, Berkeley',
              year: '2023',
              score: '3.7 GPA',
            },
          ],
          experience: [],
          assessments: [],
        },
      ],
    },
    {
      id: 2,
      title: 'Frontend Developer',
      department: 'Engineering',
      status: 'active',
      postedDate: new Date('2025-01-24'),
      candidates: [],
    },
  ];

  selectedJob: Job | null = null;
  selectedCandidate: Candidate | null = null;

  expandedStages = {
    applied: true,
    passed_mcq: true,
    shortlisted: false,
    interviewed: false,
    selected: false,
    rejected: false,
  };

  constructor(private dialog: MatDialog, private snackBar: MatSnackBar) {}

  ngOnInit(): void {
    if (this.jobs.length > 0) {
      this.selectJob(this.jobs[0]);
    }
  }

  selectJob(job: Job) {
    this.selectedJob = job;
  }

  toggleStage(stage: string) {
    this.expandedStages[stage as keyof typeof this.expandedStages] =
      !this.expandedStages[stage as keyof typeof this.expandedStages];
  }

  getCandidatesByStage(stage: string): Candidate[] {
    return this.selectedJob?.candidates.filter((c) => c.stage === stage) || [];
  }

  moveForward(candidate: Candidate) {
    const stages: Candidate['stage'][] = [
      'applied',
      'passed_mcq',
      'shortlisted',
      'interviewed',
      'selected',
    ];
    const currentIndex = stages.indexOf(candidate.stage);

    if (currentIndex < stages.length - 1) {
      candidate.stage = stages[currentIndex + 1];
      this.snackBar.open(
        `${candidate.name} moved to ${candidate.stage} stage`,
        'Close',
        { duration: 3000 }
      );
    }
  }

  openRejectModal(candidate: Candidate) {
    const dialogRef = this.dialog.open(RejectModalComponent, {
      width: '500px',
      data: { candidate },
    });

    dialogRef.afterClosed().subscribe((result) => {
      if (result?.confirmed) {
        candidate.stage = 'rejected';
        this.snackBar.open(
          `${candidate.name} has been rejected. Reason: ${result.reason}`,
          'Close',
          { duration: 3000 }
        );
      }
    });
  }

  openCandidateProfile(candidate: Candidate) {
    const dialogRef = this.dialog.open(CandidateProfileModalComponent, {
      width: '700px',
      data: { candidate },
    });

    dialogRef.afterClosed().subscribe((result) => {
      if (result?.action === 'move') {
        this.moveForward(candidate);
      } else if (result?.action === 'reject') {
        this.openRejectModal(candidate);
      }
    });
  }
}
