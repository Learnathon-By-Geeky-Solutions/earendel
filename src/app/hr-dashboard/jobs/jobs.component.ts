import { Component, type OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatDialogModule, MatDialog } from '@angular/material/dialog';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';
import { SidebarComponent } from '../sidebar/sidebar.component';
import { RejectModalComponent } from '../reject-modal/reject-modal.component';
import { CandidateProfileModalComponent } from '../candidate-profile/candidate-profile.component';
import { JobPostingService, JobApplication, UserDetails } from '../services/job-posting.service';
import { MatPaginatorModule, PageEvent } from '@angular/material/paginator';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { forkJoin, of, Observable } from 'rxjs';
import { catchError, switchMap, map, tap } from 'rxjs/operators';
import { FormsModule } from '@angular/forms';

interface Candidate {
  id: number;
  name: string;
  email: string;
  phone: string;
  stage:
    | 'applied'
    | 'passed_mcq'
    | 'shortlisted'
    | 'select_interviewer'
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
  // Additional properties for job applications
  applicationDate?: Date;
  coverLetter?: string;
  candidateId?: string; // Store the original candidate ID for user details lookup
  userDetails?: UserDetails; // Store the fetched user details
  applicationId?: string; // Store the original application ID for API calls
}

interface Job {
  id: string | number;
  title: string;
  department: string;
  status: 'active' | 'closed';
  postedDate: Date;
  candidates: Candidate[];
}

@Component({
  selector: 'app-job',
  standalone: true,
  imports: [
    CommonModule, 
    SidebarComponent, 
    MatDialogModule, 
    MatPaginatorModule, 
    MatProgressSpinnerModule,
    MatSnackBarModule,
    FormsModule
  ],
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
                  <h5 class="card-title mb-4 d-flex justify-content-between align-items-center">
                    <span>Job Postings</span>
                    <div *ngIf="loading" class="spinner-border spinner-border-sm text-primary" role="status">
                      <span class="visually-hidden">Loading...</span>
                    </div>
                  </h5>

                  <div *ngIf="loading && jobs.length === 0" class="text-center py-4">
                    <div class="spinner-border text-primary" role="status">
                      <span class="visually-hidden">Loading...</span>
                    </div>
                    <p class="mt-2 text-muted">Loading jobs...</p>
                  </div>

                  <div class="job-list" *ngIf="!loading || jobs.length > 0">
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
                    
                    <!-- No jobs message -->
                    <div *ngIf="!loading && jobs.length === 0" class="text-center p-3">
                      <p class="mb-0 text-muted">No job postings found</p>
                    </div>
                  </div>
                  
                  <!-- Pagination -->
                  <mat-paginator
                    [length]="totalElements"
                    [pageSize]="pageSize"
                    [pageIndex]="pageNumber"
                    [pageSizeOptions]="[5, 10, 20, 50]"
                    (page)="handlePageEvent($event)"
                    *ngIf="totalElements > 0"
                    class="mt-3 custom-paginator"
                  ></mat-paginator>
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
                          
                          >
                            <div
                              class="d-flex justify-content-between align-items-center"
                            >
                              <div>
                                <h6 class="mb-1">{{ candidate.name }}</h6>
                                <p class="text-muted small mb-0">
                                  {{ candidate.email }}
                                </p>
                                <p class="text-muted small mb-0" *ngIf="candidate.applicationDate">
                                  Applied on: {{ candidate.applicationDate | date:'mediumDate' }}
                                </p>
                              </div>
                              <div class="d-flex gap-2">
                                <button
                                  class="btn btn-sm btn-outline-success"
                                  (click)="moveForward(candidate); $event.stopPropagation()"
                                >
                                  Move Forward
                                </button>
                                <button
                                  class="btn btn-sm btn-outline-danger"
                                  (click)="openRejectModal(candidate); $event.stopPropagation()"
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
                        
                          >
                            <div
                              class="d-flex justify-content-between align-items-center"
                            >
                              <div>
                                <h6 class="mb-1">{{ candidate.name }}</h6>
                                <p class="text-muted small mb-0">
                                  {{ candidate.email }}
                                </p>
                                <p class="text-muted small mb-0" *ngIf="candidate.applicationDate">
                                  Applied on: {{ candidate.applicationDate | date:'mediumDate' }}
                                </p>
                              </div>
                              <div class="d-flex gap-2">
                                <button
                                  class="btn btn-sm btn-outline-success"
                                  (click)="moveForward(candidate); $event.stopPropagation()"
                                >
                                  Move Forward
                                </button>
                                <button
                                  class="btn btn-sm btn-outline-warning"
                                  (click)="moveBack(candidate); $event.stopPropagation()"
                                >
                                  Move Back
                                </button>
                                <button
                                  class="btn btn-sm btn-outline-danger"
                                  (click)="openRejectModal(candidate); $event.stopPropagation()"
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
                                  class="btn btn-sm btn-outline-warning"
                                  (click)="moveBack(candidate)"
                                >
                                  Move Back
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

                    <!-- Select Interviewer Stage -->
                    <div class="timeline-item">
                      <div class="timeline-marker bg-info">4</div>
                      <div class="timeline-content">
                        <div
                          class="timeline-header"
                          (click)="toggleStage('select_interviewer')"
                        >
                          <h6 class="mb-0">Select your interviewer</h6>
                          <span class="badge bg-light text-dark">
                            {{ getCandidatesByStage('select_interviewer').length }}
                          </span>
                          <i
                            class="bi"
                            [class.bi-chevron-down]="
                              !expandedStages.select_interviewer
                            "
                            [class.bi-chevron-up]="expandedStages.select_interviewer"
                          ></i>
                        </div>

                        <div
                          class="timeline-body"
                          *ngIf="expandedStages.select_interviewer"
                        >
                          <!-- Loader for interviewers -->
                          <div *ngIf="loadingInterviewers" class="text-center py-2">
                            <div class="spinner-border spinner-border-sm text-primary" role="status">
                              <span class="visually-hidden">Loading interviewers...</span>
                            </div>
                            <p class="mb-0 mt-2 small text-muted">Loading available interviewers...</p>
                          </div>
                          
                          <!-- No interviewers message -->
                          <div *ngIf="!loadingInterviewers && interviewers.length === 0" class="alert alert-warning">
                            <p class="mb-0">No interviewers available. Please add interviewers to the system.</p>
                          </div>
                          
                          <div
                            *ngFor="
                              let candidate of getCandidatesByStage(
                                'select_interviewer'
                              )
                            "
                            class="candidate-card p-3 mb-2 bg-white rounded shadow-sm"
                          >
                            <div class="d-flex flex-column">
                              <!-- Candidate info -->
                              <div class="d-flex justify-content-between align-items-center mb-3">
                                <div>
                                  <h6 class="mb-1">{{ candidate.name }}</h6>
                                  <p class="text-muted small mb-0">
                                    {{ candidate.email }}
                                  </p>
                                </div>
                              </div>
                              
                              <!-- Interviewer selection -->
                              <div class="row align-items-center mb-3">
                                <div class="col-md-5">
                                  <label class="form-label mb-0">Select Interviewer:</label>
                                </div>
                                <div class="col-md-7">
                                  <select 
                                    class="form-select"
                                    [disabled]="loadingInterviewers"
                                    [(ngModel)]="selectedInterviewers[candidate.applicationId || '']"
                                    (change)="assignInterviewer(candidate, selectedInterviewers[candidate.applicationId || ''])"
                                  >
                                    <option value="" selected disabled>Choose an interviewer</option>
                                    <option *ngFor="let interviewer of interviewers" [value]="interviewer.id">
                                      {{ interviewer.userName }} ({{ interviewer.email }})
                                    </option>
                                  </select>
                                </div>
                              </div>
                              
                              <!-- Action buttons -->
                              <div class="d-flex justify-content-end gap-2">
                                <button
                                  class="btn btn-sm btn-outline-success"
                                  (click)="moveForward(candidate)"
                                  [disabled]="!selectedInterviewers[candidate.applicationId || '']"
                                >
                                  Move Forward
                                </button>
                                <button
                                  class="btn btn-sm btn-outline-warning"
                                  (click)="moveBack(candidate)"
                                >
                                  Move Back
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
                      <div class="timeline-marker bg-success">5</div>
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
                                  class="btn btn-sm btn-outline-primary"
                                  (click)="viewInterviewReport(candidate); $event.stopPropagation()"
                                >
                                  View Report
                                </button>
                                <button
                                  class="btn btn-sm btn-outline-success"
                                  (click)="moveForward(candidate)"
                                >
                                  Move Forward
                                </button>
                                <button
                                  class="btn btn-sm btn-outline-warning"
                                  (click)="moveBack(candidate)"
                                >
                                  Move Back
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
                      <div class="timeline-marker bg-primary">6</div>
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
                                  class="btn btn-sm btn-outline-warning"
                                  (click)="moveBack(candidate)"
                                >
                                  Move Back
                                </button>
                              </div>
                            </div>
                          </div>
                        </div>
                      </div>
                    </div>

                    <!-- Rejected Stage -->
                    <div class="timeline-item">
                      <div class="timeline-marker bg-danger">7</div>
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
            
            <!-- Message when no job is selected -->
            <div class="col-md-8" *ngIf="!selectedJob && !loading">
              <div class="card border-0 shadow-sm">
                <div class="card-body text-center p-5">
                  <p class="text-muted mb-0">Please select a job posting to view its hiring timeline</p>
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

      .custom-paginator {
        background-color: transparent;
        font-size: 0.875rem;
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
  jobs: Job[] = [];
  selectedJob: Job | null = null;
  selectedCandidate: Candidate | null = null;
  loading: boolean = false;
  loadingCandidates: boolean = false; // Add specific loading state for candidates
  loadingCandidatesCount: { [stage: string]: boolean } = {}; // Track loading state for each stage
  jobApplications: JobApplication[] = [];
  
  // Pagination variables
  pageNumber: number = 0;
  pageSize: number = 5;
  totalElements: number = 0;
  totalPages: number = 0;

  expandedStages = {
    applied: true,
    passed_mcq: true,
    shortlisted: false,
    select_interviewer: false,
    interviewed: false,
    selected: false,
    rejected: false,
  };

  // Add a property to store the available interviewers list
  interviewers: UserDetails[] = [];
  // Track the selected interviewer for each candidate
  selectedInterviewers: { [candidateId: string]: string } = {};
  // Track loading state for interviewers
  loadingInterviewers: boolean = false;
  // Cache for user details to avoid redundant API calls
  private userDetailsCache: Map<string, UserDetails> = new Map();

  constructor(
    private dialog: MatDialog, 
    private snackBar: MatSnackBar,
    private jobPostingService: JobPostingService
  ) {}

  ngOnInit(): void {
    this.loadJobsAndApplications();
    this.loadInterviewers();
  }

  /**
   * Load both jobs and job applications in parallel
   */
  loadJobsAndApplications(): void {
    this.loading = true;
    this.loadingCandidates = true;
    
    // Initialize loading states for all stages
    const stages: Candidate['stage'][] = [
      'applied',
      'passed_mcq',
      'shortlisted',
      'select_interviewer',
      'interviewed',
      'selected',
      'rejected'
    ];
    
    stages.forEach(stage => {
      this.loadingCandidatesCount[stage] = true;
    });
    
    // Load jobs and applications in parallel using forkJoin
    forkJoin({
      jobs: this.jobPostingService.getJobPostings(this.pageNumber, this.pageSize),
      applications: this.jobPostingService.getJobApplications()
    }).pipe(
      catchError(error => {
        console.error('Error loading data:', error);
        this.snackBar.open('Failed to load job data', 'Close', { duration: 3000 });
        this.loading = false;
        this.loadingCandidates = false;
        // Clear loading states
        stages.forEach(stage => {
          this.loadingCandidatesCount[stage] = false;
        });
        return of({ 
          jobs: { content: [], pageNumber: 0, pageSize: this.pageSize, totalElements: 0, totalPages: 0 },
          applications: []
        });
      })
    ).subscribe(({ jobs, applications }) => {
      console.log('Raw jobs response:', jobs);
      console.log('Job applications loaded:', applications);
      
      // Store applications for reference
      this.jobApplications = applications;
      
      // Handle both potential response formats for jobs
      let content = jobs.content;
      
      // If the response is already the array of jobs
      if (Array.isArray(jobs) && !content) {
        content = jobs;
        this.totalElements = jobs.length;
        this.totalPages = 1;
      }
      
      // Ensure content is an array before mapping
      if (Array.isArray(content)) {
        this.jobs = content.map(jobPosting => this.mapToJob(jobPosting));
        
        if (jobs.pageNumber !== undefined) {
          this.pageNumber = jobs.pageNumber;
        }
        
        if (jobs.pageSize !== undefined) {
          this.pageSize = jobs.pageSize;
        }
        
        if (jobs.totalElements !== undefined) {
          this.totalElements = jobs.totalElements;
        } else {
          this.totalElements = content.length;
        }
        
        if (jobs.totalPages !== undefined) {
          this.totalPages = jobs.totalPages;
        }
        
        if (this.jobs.length > 0 && !this.selectedJob) {
          this.selectJob(this.jobs[0]);
        }
        
        // Map applications to jobs now that we have both sets of data
        this.mapApplicationsToJobs();
      } else {
        // If content is not an array, show error
        console.error('Invalid response format:', jobs);
        this.snackBar.open('Invalid data format received from server', 'Close', { duration: 3000 });
        this.jobs = [];
        
        this.loadingCandidates = false;
        // Clear loading states since we have no data
        stages.forEach(stage => {
          this.loadingCandidatesCount[stage] = false;
        });
      }
      
      this.loading = false;
    });
  }

  /**
   * Map job applications to their respective jobs as candidates
   */
  mapApplicationsToJobs(): void {
    this.loadingCandidates = true;
    
    // Group applications by jobId
    const applicationsByJob = new Map<string, JobApplication[]>();
    
    this.jobApplications.forEach(app => {
      if (!applicationsByJob.has(app.jobId)) {
        applicationsByJob.set(app.jobId, []);
      }
      applicationsByJob.get(app.jobId)?.push(app);
    });
    
    // For each job, add the applications as candidates
    this.jobs.forEach(job => {
      const applications = applicationsByJob.get(job.id.toString()) || [];
      const candidates: Candidate[] = applications.map((app, index) => this.mapApplicationToCandidate(app, index));
      
      // Add these candidates to the job's existing candidates (if any)
      job.candidates = [...candidates];
      
      console.log(`Mapped ${candidates.length} candidates to job: ${job.title}`);
      
      // Fetch user details for all candidates
      this.fetchUserDetailsForCandidates(job.candidates);
    });
    
    // If a job is already selected, refresh its reference to update the view
    if (this.selectedJob) {
      const updatedJob = this.jobs.find(j => j.id === this.selectedJob?.id);
      if (updatedJob) {
        this.selectedJob = updatedJob;
      }
    }
    
    // We've mapped all candidates to jobs, so we can clear the loading state
    this.loadingCandidates = false;
    
    // Clear the loading state for each stage 
    // This ensures the UI shows candidates even if user details are still loading
    Object.keys(this.loadingCandidatesCount).forEach(stage => {
      this.loadingCandidatesCount[stage] = false;
    });
  }
  
  /**
   * Fetch user details for an array of candidates
   */
  fetchUserDetailsForCandidates(candidates: Candidate[]): void {
    candidates.forEach(candidate => {
      // Only proceed if candidateId is definitely defined
      const candidateId = candidate.candidateId;
      if (candidateId) {
        // Check if we already have this user's details in cache
        if (this.userDetailsCache.has(candidateId)) {
          // Use cached user details instead of making another API call
          const userDetails = this.userDetailsCache.get(candidateId);
          if (userDetails) {
            // Update the candidate with user details
            candidate.userDetails = userDetails;
            candidate.name = userDetails.userName;
            candidate.email = userDetails.email;
            
            // Force UI update if this is for the selected job
            if (this.selectedJob && candidate.id) {
              const candidateInSelectedJob = this.selectedJob.candidates.find(c => c.id === candidate.id);
              if (candidateInSelectedJob) {
                candidateInSelectedJob.name = userDetails.userName;
                candidateInSelectedJob.email = userDetails.email;
                candidateInSelectedJob.userDetails = userDetails;
              }
            }
          }
        } else {
          // Not in cache, fetch from API
          this.jobPostingService.getUserDetails(candidateId)
            .pipe(
              catchError(error => {
                console.error(`Error fetching user details for candidate ${candidateId}:`, error);
                return of(null);
              })
            )
            .subscribe(userDetails => {
              if (userDetails) {
                // Update the candidate with user details
                candidate.userDetails = userDetails;
                candidate.name = userDetails.userName;
                candidate.email = userDetails.email;
                
                // Force UI update if this is for the selected job
                if (this.selectedJob && candidate.id) {
                  const candidateInSelectedJob = this.selectedJob.candidates.find(c => c.id === candidate.id);
                  if (candidateInSelectedJob) {
                    candidateInSelectedJob.name = userDetails.userName;
                    candidateInSelectedJob.email = userDetails.email;
                    candidateInSelectedJob.userDetails = userDetails;
                  }
                }
                
                // Cache the user details for future use
                this.userDetailsCache.set(candidateId, userDetails);
              }
            });
        }
      }
    });
  }

  /**
   * Map a job application to a candidate object
   */
  mapApplicationToCandidate(application: JobApplication, index: number): Candidate {
    // Map the application status to our candidate stage
    let stage: Candidate['stage'] = 'applied';
    
    // Convert API application status to our stage format
    switch (application.status.toLowerCase()) {
      case 'applied':
        stage = 'applied';
        break;
      case 'screened':
      case 'mcq passed':
        stage = 'passed_mcq';
        break;
      case 'shortlisted':
        stage = 'shortlisted';
        break;
      case 'select interviewer':
        stage = 'select_interviewer';
        break;
      case 'interviewed':
        stage = 'interviewed';
        break;
      case 'selected':
      case 'hired':
        stage = 'selected';
        break;
      case 'rejected':
        stage = 'rejected';
        break;
      default:
        stage = 'applied';
    }
    
    return {
      id: index + 1, // Use index for UI display only
      name: `Candidate ${index + 1}`, // Default name if candidate details aren't available
      email: application.candidateId, // Use candidateId as email for now
      phone: '',
      stage: stage,
      timestamp: new Date(application.applicationDate),
      skills: [],
      education: [],
      experience: [],
      assessments: [],
      // Additional application data
      applicationDate: new Date(application.applicationDate),
      coverLetter: application.coverLetter,
      candidateId: application.candidateId, // Store the original candidate ID
      // Store the original application data for API calls
      applicationId: application.id
    };
  }

  loadJobs(withApplications: boolean = false): void {
    this.loading = true;
    this.jobPostingService.getJobPostings(this.pageNumber, this.pageSize)
      .subscribe({
        next: (response: any) => {
          console.log('Raw response:', response);
          
          // Handle both potential response formats
          let content = response.content;
          
          // If the response is already the array of jobs
          if (Array.isArray(response) && !content) {
            content = response;
            this.totalElements = response.length;
            this.totalPages = 1;
          }
          
          // Ensure content is an array before mapping
          if (Array.isArray(content)) {
            this.jobs = content.map(jobPosting => this.mapToJob(jobPosting));
            
            if (response.pageNumber !== undefined) {
              this.pageNumber = response.pageNumber;
            }
            
            if (response.pageSize !== undefined) {
              this.pageSize = response.pageSize;
            }
            
            if (response.totalElements !== undefined) {
              this.totalElements = response.totalElements;
            } else {
              this.totalElements = content.length;
            }
            
            if (response.totalPages !== undefined) {
              this.totalPages = response.totalPages;
            }
            
            if (this.jobs.length > 0 && !this.selectedJob) {
              this.selectJob(this.jobs[0]);
            }
          } else {
            // If content is not an array, show error
            console.error('Invalid response format:', response);
            this.snackBar.open('Invalid data format received from server', 'Close', { duration: 3000 });
            this.jobs = [];
          }
          
          this.loading = false;
          
          // After jobs are loaded, load applications if requested
          if (withApplications) {
            // Load job applications separately
            this.jobPostingService.getJobApplications()
              .pipe(
                catchError(error => {
                  console.error('Error loading job applications:', error);
                  this.snackBar.open('Failed to load job applications', 'Close', { duration: 3000 });
                  return of([]);
                })
              )
              .subscribe(applications => {
                this.jobApplications = applications;
                console.log('Job applications loaded:', applications);
                
                // Map applications to jobs
                this.mapApplicationsToJobs();
              });
          }
        },
        error: (error) => {
          console.error('Error loading job postings:', error);
          this.snackBar.open('Failed to load job postings', 'Close', { duration: 3000 });
          this.loading = false;
          this.jobs = [];
        }
      });
  }

  mapToJob(jobPosting: any): Job {
    if (!jobPosting) {
      return {
        id: 'unknown',
        title: 'Unknown Job',
        department: 'Unknown',
        status: 'closed',
        postedDate: new Date(),
        candidates: []
      };
    }
    
    // Map candidates if they exist, transforming API structure to match our local interface
    const mappedCandidates: Candidate[] = [];
    
    // Only try to map candidates if they exist and are an array
    if (jobPosting.candidates && Array.isArray(jobPosting.candidates)) {
      jobPosting.candidates.forEach((apiCandidate: any, index: number) => {
        if (apiCandidate) {
          mappedCandidates.push({
            id: apiCandidate.id || index + 1,
            name: apiCandidate.name || `Candidate ${index + 1}`,
            email: apiCandidate.email || '',
            phone: apiCandidate.phoneNumber || '',
            stage: apiCandidate.stage || 'applied',
            timestamp: apiCandidate.createdAt ? new Date(apiCandidate.createdAt) : new Date(),
            skills: apiCandidate.skills || [],
            education: apiCandidate.education || [],
            experience: apiCandidate.experience || [],
            assessments: apiCandidate.assessments || []
          });
        }
      });
    }

    return {
      id: jobPosting.id || 'unknown',
      title: jobPosting.name || 'Unknown Job',
      department: jobPosting.jobType || 'Unknown',
      status: (jobPosting.status && jobPosting.status.toLowerCase() === 'active') ? 'active' : 'closed',
      postedDate: jobPosting.createdAt ? new Date(jobPosting.createdAt) : new Date(),
      candidates: mappedCandidates
    };
  }

  handlePageEvent(event: PageEvent): void {
    this.pageNumber = event.pageIndex;
    this.pageSize = event.pageSize;
    this.loadJobsAndApplications();
  }

  selectJob(job: Job) {
    this.selectedJob = job;
  }

  toggleStage(stage: string) {
    this.expandedStages[stage as keyof typeof this.expandedStages] =
      !this.expandedStages[stage as keyof typeof this.expandedStages];
  }

  getCandidatesByStage(stage: string): Candidate[] {
    if (!this.selectedJob || !this.selectedJob.candidates) {
      return [];
    }
    try {
      // Filter only candidates that match the requested stage
      const candidates = this.selectedJob.candidates.filter((c) => c && c.stage === stage) || [];
      
      // Mark this stage as loaded once we have candidates
      if (candidates.length > 0 || !this.loadingCandidates) {
        this.loadingCandidatesCount[stage] = false;
      }
      
      return candidates;
    } catch (error) {
      console.error('Error filtering candidates by stage:', error);
      this.loadingCandidatesCount[stage] = false;
      return [];
    }
  }

  moveForward(candidate: Candidate) {
    const stages: Candidate['stage'][] = [
      'applied',
      'passed_mcq',
      'shortlisted',
      'select_interviewer',
      'interviewed',
      'selected',
    ];
    const currentIndex = stages.indexOf(candidate.stage);

    if (currentIndex < stages.length - 1) {
      // Check if we're moving from 'select_interviewer' stage and ensure an interviewer is assigned
      if (candidate.stage === 'select_interviewer' && 
          (!candidate.applicationId || !this.selectedInterviewers[candidate.applicationId])) {
        this.snackBar.open(
          'Please select an interviewer before moving the candidate forward',
          'Close',
          { duration: 3000 }
        );
        return;
      }

      const newStage = stages[currentIndex + 1];
      const originalStage = candidate.stage; // Store original stage for rollback if needed
      
      // Map the stage to the API status string
      const apiStatus = this.getApiStatusFromStage(newStage);
      
      // Optimistic UI update - immediately update the UI
      candidate.stage = newStage;
      
      // Only proceed with API call if we have the necessary application data
      if (candidate.applicationId && candidate.candidateId) {
        // Get the original application ID (UUID string from API)
        const applicationId = candidate.applicationId;
        
        // Create the application update payload
        const applicationUpdate = {
          id: applicationId,
          jobId: this.selectedJob?.id.toString() || '',
          candidateId: candidate.candidateId,
          status: apiStatus,
          coverLetter: candidate.coverLetter || ''
        };
        
        console.log('Updating application with ID:', applicationId);
        console.log('Update payload:', applicationUpdate);
        
        // Call the service to update the status
        this.jobPostingService.updateJobApplicationStatus(applicationId, applicationUpdate)
          .subscribe({
            next: (response) => {
              // Already updated the UI, just show success notification
              this.snackBar.open(
                `${candidate.name} moved to ${candidate.stage} stage`,
                'Close',
                { duration: 3000 }
              );
            },
            error: (error) => {
              // Rollback the optimistic update on error
              candidate.stage = originalStage;
              console.error('Error updating candidate stage:', error);
              this.snackBar.open(
                `Failed to update ${candidate.name}'s stage. Please try again.`,
                'Close',
                { duration: 3000 }
              );
            }
          });
      } else {
        // If we don't have the necessary data for API call, just update locally
        console.warn('Missing application data for API update. Updating UI only.');
        this.snackBar.open(
          `${candidate.name} moved to ${candidate.stage} stage (local update only)`,
          'Close',
          { duration: 3000 }
        );
      }
    }
  }

  moveBack(candidate: Candidate) {
    const stages: Candidate['stage'][] = [
      'applied',
      'passed_mcq',
      'shortlisted',
      'select_interviewer',
      'interviewed',
      'selected',
    ];
    const currentIndex = stages.indexOf(candidate.stage);

    if (currentIndex > 0) {
      const newStage = stages[currentIndex - 1];
      const originalStage = candidate.stage; // Store original stage for rollback if needed
      
      // Map the stage to the API status string
      const apiStatus = this.getApiStatusFromStage(newStage);
      
      // Optimistic UI update - immediately update the UI
      candidate.stage = newStage;
      
      // Only proceed with API call if we have the necessary application data
      if (candidate.applicationId && candidate.candidateId) {
        // Get the original application ID (UUID string from API)
        const applicationId = candidate.applicationId;
        
        // Create the application update payload
        const applicationUpdate = {
          id: applicationId,
          jobId: this.selectedJob?.id.toString() || '',
          candidateId: candidate.candidateId,
          status: apiStatus,
          coverLetter: candidate.coverLetter || ''
        };
        
        console.log('Updating application with ID:', applicationId);
        console.log('Update payload:', applicationUpdate);
        
        // Call the service to update the status
        this.jobPostingService.updateJobApplicationStatus(applicationId, applicationUpdate)
          .subscribe({
            next: (response) => {
              // Already updated the UI, just show success notification
              this.snackBar.open(
                `${candidate.name} moved back to ${candidate.stage} stage`,
                'Close',
                { duration: 3000 }
              );
            },
            error: (error) => {
              // Rollback the optimistic update on error
              candidate.stage = originalStage;
              console.error('Error updating candidate stage:', error);
              this.snackBar.open(
                `Failed to update ${candidate.name}'s stage. Please try again.`,
                'Close',
                { duration: 3000 }
              );
            }
          });
      } else {
        // If we don't have the necessary data for API call, just update locally
        console.warn('Missing application data for API update. Updating UI only.');
        this.snackBar.open(
          `${candidate.name} moved back to ${candidate.stage} stage (local update only)`,
          'Close',
          { duration: 3000 }
        );
      }
    }
  }

  /**
   * Convert the UI stage to the API status string
   */
  getApiStatusFromStage(stage: Candidate['stage']): string {
    switch (stage) {
      case 'applied':
        return 'Applied';
      case 'passed_mcq':
        return 'MCQ Passed';
      case 'shortlisted':
        return 'Shortlisted';
      case 'select_interviewer':
        return 'Select Interviewer';
      case 'interviewed':
        return 'Interviewed';
      case 'selected':
        return 'Selected';
      case 'rejected':
        return 'Rejected';
      default:
        return 'Applied';
    }
  }

  openRejectModal(candidate: Candidate) {
    const dialogRef = this.dialog.open(RejectModalComponent, {
      width: '500px',
      data: { candidate },
    });

    dialogRef.afterClosed().subscribe((result) => {
      if (result?.confirmed) {
        const originalStage = candidate.stage; // Store original stage for rollback if needed
        
        // Map the rejected stage to the API status
        const apiStatus = this.getApiStatusFromStage('rejected');
        
        // Optimistic UI update - immediately update the UI
        candidate.stage = 'rejected';
        
        // Only proceed with API call if we have the necessary application data
        if (candidate.applicationId && candidate.candidateId) {
          // Get the original application ID (UUID string from API)
          const applicationId = candidate.applicationId;
          
          // Create the application update payload
          const applicationUpdate = {
            id: applicationId,
            jobId: this.selectedJob?.id.toString() || '',
            candidateId: candidate.candidateId,
            status: apiStatus,
            coverLetter: candidate.coverLetter || ''
          };
          
          console.log('Updating application with ID:', applicationId);
          console.log('Update payload:', applicationUpdate);
          
          // Call the service to update the status
          this.jobPostingService.updateJobApplicationStatus(applicationId, applicationUpdate)
            .subscribe({
              next: (response) => {
                // Already updated the UI, just show success notification
                this.snackBar.open(
                  `${candidate.name} has been rejected. Reason: ${result.reason}`,
                  'Close',
                  { duration: 3000 }
                );
              },
              error: (error) => {
                // Rollback the optimistic update on error
                candidate.stage = originalStage;
                console.error('Error updating candidate stage to rejected:', error);
                this.snackBar.open(
                  `Failed to reject ${candidate.name}. Please try again.`,
                  'Close',
                  { duration: 3000 }
                );
              }
            });
        } else {
          // If we don't have the necessary data for API call, just update locally
          console.warn('Missing application data for API update. Updating UI only.');
          this.snackBar.open(
            `${candidate.name} has been rejected. Reason: ${result.reason} (local update only)`,
            'Close',
            { duration: 3000 }
          );
        }
      }
    });
  }

  // openCandidateProfile(candidate: Candidate) {
  //   console.log('Opening candidate profile:', candidate);
    
  //   // Pass the complete candidate data including application details and user details
  //   const dialogRef = this.dialog.open(CandidateProfileModalComponent, {
  //     width: '700px',
  //     data: { 
  //       candidate: {
  //         ...candidate,
  //         // Use user details if available
  //         name: candidate.userDetails?.userName || candidate.name,
  //         email: candidate.userDetails?.email || candidate.email
  //       },
  //       // Include any application-specific data
  //       applicationInfo: {
  //         applicationDate: candidate.applicationDate,
  //         coverLetter: candidate.coverLetter,
  //         jobName: this.selectedJob?.title || 'Unknown Job'
  //       },
  //       userDetails: candidate.userDetails
  //     }
  //   });

  //   dialogRef.afterClosed().subscribe((result) => {
  //     if (result?.action === 'move') {
  //       this.moveForward(candidate);
  //     } else if (result?.action === 'reject') {
  //       this.openRejectModal(candidate);
  //     }
  //   });
  // }

  /**
   * Load interviewers from the service
   */
  loadInterviewers(): void {
    this.loadingInterviewers = true;
    this.jobPostingService.getInterviewers()
      .pipe(
        catchError(error => {
          console.error('Error loading interviewers:', error);
          this.snackBar.open('Failed to load interviewers', 'Close', { duration: 3000 });
          return of([]);
        })
      )
      .subscribe(interviewers => {
        this.interviewers = interviewers;
        this.loadingInterviewers = false;
        console.log('Interviewers loaded:', interviewers);
      });
  }

  /**
   * Assign an interviewer to a candidate and create an interview
   */
  assignInterviewer(candidate: Candidate, interviewerId: string): void {
    if (!candidate.applicationId || !interviewerId || !candidate.candidateId) {
      this.snackBar.open('Missing required data to assign interviewer', 'Close', { duration: 3000 });
      return;
    }

    // At this point, we've verified that candidateId exists
    const candidateId = candidate.candidateId as string;
    const applicationId = candidate.applicationId;
    const originalStage = candidate.stage;
    
    // Store the selection in our local tracking object
    this.selectedInterviewers[applicationId] = interviewerId;
    
    // Get the interviewer name for display
    const interviewer = this.interviewers.find(i => i.id === interviewerId);
    const interviewerName = interviewer ? interviewer.userName : 'Unknown';

    // Create interview instead of just assigning the interviewer
    this.jobPostingService.createInterview(applicationId, interviewerId, candidateId, this.selectedJob?.id.toString() || '')
      .subscribe({
        next: (response) => {
          console.log('Interview created successfully:', response);
          
          // Optimistically update the UI
          const newStage: Candidate['stage'] = 'interviewed';
          candidate.stage = newStage;
          
          // Update the application status to interviewed since we've scheduled an interview
          this.jobPostingService.updateJobApplicationStatus(
            applicationId, 
            {
              id: applicationId,
              jobId: this.selectedJob?.id.toString() || '',
              candidateId: candidateId,
              status: this.getApiStatusFromStage(newStage),
              coverLetter: candidate.coverLetter || ''
            }
          ).subscribe({
            next: () => {
              this.snackBar.open(
                `Assigned ${interviewerName} to ${candidate.name} and scheduled an interview`,
                'Close',
                { duration: 3000 }
              );
            },
            error: (err) => {
              // Rollback the optimistic update on error
              candidate.stage = originalStage;
              console.error('Error updating application status after interview creation:', err);
              // We've already created the interview, so we'll consider this a partial success
              this.snackBar.open(
                `Assigned ${interviewerName} to ${candidate.name} but status update failed`,
                'Close',
                { duration: 3000 }
              );
            }
          });
        },
        error: (error) => {
          console.error('Error creating interview:', error);
          this.snackBar.open(
            `Failed to assign interviewer. Please try again.`,
            'Close',
            { duration: 3000 }
          );
        }
      });
  }

  /**
   * Get the currently selected interviewer for a candidate
   */
  getSelectedInterviewer(candidate: Candidate): string {
    return candidate.applicationId ? 
      this.selectedInterviewers[candidate.applicationId] || '' : '';
  }

  /**
   * View the interview report for a candidate
   * Opens in a new window with candidateId and jobId as URL parameters
   */
  viewInterviewReport(candidate: Candidate): void {
    if (!candidate.candidateId || !this.selectedJob?.id) {
      this.snackBar.open('Missing candidate ID or job ID', 'Close', { duration: 3000 });
      return;
    }

    const url = `hr-dashboard/jobs/interview-report?candidateId=${candidate.candidateId}&jobId=${this.selectedJob.id}`;
    window.open(url, '_blank');
  }
}
