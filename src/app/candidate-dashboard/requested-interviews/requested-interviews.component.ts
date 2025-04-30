import { Component, OnInit, OnDestroy } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Subscription, forkJoin, of } from 'rxjs';
import { switchMap, catchError, finalize } from 'rxjs/operators';
import { JobService, Job } from '../services/job.service';

interface Application {
  id: string;
  jobId: string;
  interviewerName: string;
  date: string;
  time: string;
  status: string;
}

interface EnrichedInterview {
  applicationId: string;
  jobId: string;
  companyName: string;
  position: string;
  location: string;
  jobType: string;
  salary: string;
  date: string;
  time: string;
  status: string;
  interviewerName: string;
}

@Component({
  selector: "app-requested-interviews",
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: "./requested-interviews.component.html",
  styleUrls: ["./requested-interviews.component.css"],
})
export class RequestedInterviewsComponent implements OnInit, OnDestroy {
  rawApps: Application[] = [];
  interviews: EnrichedInterview[] = [];
  filtered: EnrichedInterview[] = [];
  paginatedInterviews: EnrichedInterview[] = [];
  
  isLoading = false;
  hasError = false;
  errorMessage = '';
  noData = false;

  searchTerm = "";
  currentPage = 1;
  pageSize = 4;
  totalPages = 1;

  private subs = new Subscription();

  constructor(private jobSvc: JobService) {}

  ngOnInit() {
    this.loadInterviews();
  }

  loadInterviews() {
    const user = JSON.parse(sessionStorage.getItem("loggedInUser") || "{}");
    const candidateId = user.userId;
    
    if (!candidateId) {
      this.noData = true;
      this.errorMessage = 'User not found. Please log in again.';
      return;
    }

    this.isLoading = true;
    this.hasError = false;
    this.noData = false;

    this.subs.add(
      this.jobSvc
        .getApplications(candidateId)
        .pipe(
          switchMap((appsResponse: any) => {
            // Handle paginated response
            this.rawApps = appsResponse.items || [];

            if (this.rawApps.length === 0) {
              this.noData = true;
              this.interviews = [];
              return of([]);
            }

            // Fetch job details for each application
            const jobCalls = this.rawApps.map((app) =>
              this.jobSvc.getJob(app.jobId).pipe(catchError(() => of(null)))
            );

            return forkJoin(jobCalls);
          }),
          catchError(err => {
            this.hasError = true;
            this.errorMessage = 'Failed to load interviews. Please try again later.';
            console.error('Error loading interviews:', err);
            return of([]);
          }),
          finalize(() => {
            this.isLoading = false;
          })
        )
        .subscribe({
          next: (jobs: (Job | null)[]) => {
            // Map applications with job data
            this.interviews = this.rawApps.map((app, index) => {
              const job = jobs[index];
              return {
                applicationId: app.id,
                jobId: app.jobId,
                companyName: job?.name || "Unknown Company",
                position: job?.experienceLevel || "Unknown Position",
                location: job?.location || "Unknown Location",
                jobType: job?.jobType || "Unknown Job Type",
                salary: "Not specified", // Use a string directly as Job doesn't have a salary property
                date: app.date ? new Date(app.date).toLocaleDateString() : "N/A",
                time: app.time || "N/A",
                status: app.status || "pending",
                interviewerName: app.interviewerName || "Not assigned",
              };
            });

            this.applyFilter();
          },
          error: (err) => {
            this.hasError = true;
            this.errorMessage = 'An unexpected error occurred. Please try again.';
            console.error('Error in subscription:', err);
          }
        })
    );
  }

  applyFilter() {
    const term = this.searchTerm.toLowerCase().trim();
    
    if (!term) {
      this.filtered = [...this.interviews];
    } else {
      this.filtered = this.interviews.filter(
        (iv) =>
          iv.companyName.toLowerCase().includes(term) ||
          iv.position.toLowerCase().includes(term) ||
          iv.interviewerName.toLowerCase().includes(term)
      );
    }
    
    this.currentPage = 1;
    this.updatePagination();
  }

  updatePagination() {
    this.totalPages = Math.max(1, Math.ceil(this.filtered.length / this.pageSize));
    const start = (this.currentPage - 1) * this.pageSize;
    this.paginatedInterviews = this.filtered.slice(
      start,
      start + this.pageSize
    );
  }

  changePage(delta: number) {
    this.currentPage = Math.min(
      this.totalPages,
      Math.max(1, this.currentPage + delta)
    );
    this.updatePagination();
  }

  retryLoading() {
    this.loadInterviews();
  }

  ngOnDestroy() {
    if (this.subs) {
      this.subs.unsubscribe();
    }
  }
}