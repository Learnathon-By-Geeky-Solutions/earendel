import { Component, OnInit, ViewChild, ElementRef, OnDestroy } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';
import { MatTooltipModule } from '@angular/material/tooltip';
import { MatChipsModule } from '@angular/material/chips';
import { JobDetailsModalComponent } from '../job-details/job-details.component';
import { JobService, Job, JobFilter } from '../services/job.service';
import { Subject, debounceTime, distinctUntilChanged, takeUntil, of } from 'rxjs';
import { catchError, shareReplay, finalize } from 'rxjs/operators';

@Component({
  selector: 'app-job-posts',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    MatInputModule,
    MatSelectModule,
    MatButtonModule,
    MatIconModule,
    MatProgressSpinnerModule,
    MatSnackBarModule,
    MatChipsModule,
    MatTooltipModule,
    JobDetailsModalComponent
  ],
  template: `
    <div class="jobs-container">
      <h2>Job Posts</h2>
      
      <!-- STEP 2: Implement Filtering UI -->
      <div class="search-filter-container">
        <mat-form-field appearance="outline" class="search-field">
          <mat-label>Search jobs</mat-label>
          <input
            matInput
            [(ngModel)]="searchTerm"
            (ngModelChange)="onSearchChange($event)"
            placeholder="Search by title or keywords"
          />
          <mat-icon matSuffix>search</mat-icon>
        </mat-form-field>

        <mat-form-field appearance="outline" class="filter-field">
          <mat-label>Experience Level</mat-label>
          <mat-select [(ngModel)]="selectedExperience" (selectionChange)="onFilterChange()">
            <mat-option value="">All Experience Levels</mat-option>
            <mat-option value="Entry Level">Entry Level</mat-option>
            <mat-option value="Mid Level">Mid Level</mat-option>
            <mat-option value="Senior Level">Senior Level</mat-option>
          </mat-select>
        </mat-form-field>

        <mat-form-field appearance="outline" class="filter-field">
          <mat-label>Job Type</mat-label>
          <mat-select [(ngModel)]="selectedJobType" (selectionChange)="onFilterChange()">
            <mat-option value="">All Job Types</mat-option>
            <mat-option value="Full-time">Full-time</mat-option>
            <mat-option value="Part-time">Part-time</mat-option>
            <mat-option value="Contract">Contract</mat-option>
            <mat-option value="Driver">Driver</mat-option>
          </mat-select>
        </mat-form-field>

        <mat-form-field appearance="outline" class="filter-field">
          <mat-label>Location</mat-label>
          <mat-select [(ngModel)]="selectedLocation" (selectionChange)="onFilterChange()">
            <mat-option value="">All Locations</mat-option>
            <mat-option *ngFor="let location of locations" [value]="location">
              {{ location }}
            </mat-option>
          </mat-select>
        </mat-form-field>
      </div>

      <!-- Active Filters as Chips -->
      <div class="active-filters" *ngIf="hasActiveFilters()">
        <span class="filters-label">Active Filters:</span>
        <mat-chip-listbox>
          <mat-chip *ngIf="searchTerm" (removed)="removeFilter('search')">
            Search: {{ searchTerm }}
            <button matChipRemove>
              <mat-icon>cancel</mat-icon>
            </button>
          </mat-chip>
          <mat-chip *ngIf="selectedExperience" (removed)="removeFilter('experience')">
            Experience: {{ selectedExperience }}
            <button matChipRemove>
              <mat-icon>cancel</mat-icon>
            </button>
          </mat-chip>
          <mat-chip *ngIf="selectedJobType" (removed)="removeFilter('jobType')">
            Type: {{ selectedJobType }}
            <button matChipRemove>
              <mat-icon>cancel</mat-icon>
            </button>
          </mat-chip>
          <mat-chip *ngIf="selectedLocation" (removed)="removeFilter('location')">
            Location: {{ selectedLocation }}
            <button matChipRemove>
              <mat-icon>cancel</mat-icon>
            </button>
          </mat-chip>
        </mat-chip-listbox>
        <button mat-button color="primary" (click)="clearAllFilters()">Clear All</button>
      </div>

      <div *ngIf="jobs.length === 1 && !loading && !hasActiveFilters()" class="info-banner" matTooltip="Currently only one job post is available">
        <mat-icon>info</mat-icon>
        <span>Currently only one job post is available</span>
      </div>

      <!-- STEP 3: Setup Infinite Scrolling with Jobs List -->
      <div class="jobs-list" #jobsList>
        <div *ngFor="let job of jobs; trackBy: trackByJobId" class="job-card">
          <div class="card-header">
            <h3>{{ job.name }}</h3>
          </div>
          <div class="card-content">
            <div class="company-info">
              <span class="posted-date">Posted {{ job.createdOn | date }}</span>
            </div>
            <div class="detail-row">
              <mat-icon>work</mat-icon>
              <span>{{ job.experienceLevel }}</span>
            </div>
            <div class="detail-row">
              <mat-icon>location_on</mat-icon>
              <span>{{ job.location }}</span>
            </div>
            <div class="detail-row">
              <mat-icon>business</mat-icon>
              <span>{{ job.jobType }}</span>
            </div>
            <div class="skills-list">
              <span class="skill-tag">{{ job.requirments }}</span>
            </div>
          </div>
          <div class="card-actions">
            <button mat-raised-button color="primary" (click)="viewJobDetails(job)">
              View Details
            </button>
          </div>
        </div>
      </div>

      <!-- STEP 4: Handle Empty States and Errors -->
      <div *ngIf="loading" class="loading-spinner">
        <mat-spinner diameter="40"></mat-spinner>
      </div>

      <div *ngIf="!loading && jobs.length === 0" class="no-jobs">
        <mat-icon>search_off</mat-icon>
        <p>No jobs found. Try adjusting your filters.</p>
      </div>

      <!-- Job Details Modal -->
      <app-job-details-modal
        *ngIf="selectedJob"
        [isOpen]="isModalOpen"
        [job]="selectedJob"
        (closeModal)="closeModal()"
        (applyForJob)="applyForJob($event)"
      ></app-job-details-modal>
    </div>
  `,
  styles: [`
    /* STEP 5: Final Polish & UX Considerations */
    .jobs-container {
      padding: 24px;
      max-width: 1200px;
      margin: 0 auto;
    }

    h2 {
      font-size: 24px;
      font-weight: 600;
      margin-bottom: 24px;
      color: #333;
    }

    .search-filter-container {
      display: grid;
      grid-template-columns: 2fr 1fr 1fr 1fr;
      gap: 16px;
      margin-bottom: 16px;
      position: sticky;
      top: 0;
      background: white;
      z-index: 10;
      padding: 12px 0;
    }

    .search-field {
      width: 100%;
    }

    .filter-field {
      width: 100%;
    }

    .active-filters {
      display: flex;
      align-items: center;
      flex-wrap: wrap;
      gap: 10px;
      margin-bottom: 24px;
    }

    .filters-label {
      font-size: 14px;
      color: #666;
    }
    
    .info-banner {
      display: flex;
      align-items: center;
      background-color: #e3f2fd;
      border-radius: 8px;
      padding: 12px 16px;
      margin-bottom: 24px;
      color: #0277bd;
    }
    
    .info-banner mat-icon {
      margin-right: 8px;
    }

    .jobs-list {
      display: grid;
      grid-template-columns: repeat(auto-fill, minmax(300px, 1fr));
      gap: 24px;
    }

    .job-card {
      background: white;
      border-radius: 12px;
      padding: 20px;
      box-shadow: 0 2px 4px rgba(0, 0, 0, 0.1);
      display: flex;
      flex-direction: column;
      height: 100%;
      transition: transform 0.2s, box-shadow 0.2s;
    }

    .job-card:hover {
      transform: translateY(-2px);
      box-shadow: 0 4px 8px rgba(0, 0, 0, 0.1);
    }

    .card-header {
      margin-bottom: 16px;
    }

    .card-header h3 {
      font-size: 18px;
      font-weight: 600;
      margin: 0;
      color: #333;
      line-height: 1.4;
      display: -webkit-box;
      -webkit-line-clamp: 2;
      -webkit-box-orient: vertical;
      overflow: hidden;
      min-height: 50px;
    }

    .card-content {
      flex-grow: 1;
    }

    .company-info {
      margin-bottom: 16px;
    }

    .posted-date {
      font-size: 12px;
      color: #666;
    }

    .detail-row {
      display: flex;
      align-items: center;
      gap: 8px;
      margin-bottom: 8px;
      color: #666;
    }

    .detail-row mat-icon {
      font-size: 18px;
      width: 18px;
      height: 18px;
      color: #999;
    }

    .skills-list {
      margin: 16px 0;
      min-height: 40px;
    }

    .skill-tag {
      display: inline-block;
      padding: 4px 12px;
      background: #f5f5f5;
      border-radius: 16px;
      font-size: 12px;
      color: #666;
      margin: 4px;
    }

    .card-actions {
      margin-top: auto;
      padding-top: 16px;
    }

    .loading-spinner {
      display: flex;
      justify-content: center;
      padding: 40px;
    }

    .no-jobs {
      text-align: center;
      padding: 40px;
      color: #666;
    }

    .no-jobs mat-icon {
      font-size: 48px;
      width: 48px;
      height: 48px;
      margin-bottom: 16px;
    }

    @media (max-width: 768px) {
      .search-filter-container {
        grid-template-columns: 1fr;
      }
    }
  `]
})
export class JobPostsComponent implements OnInit, OnDestroy {
  // STEP 1: Integrate the Job List API with the UI
  jobs: Job[] = [];
  loading = false;
  currentPage = 1;
  searchTerm = '';
  selectedExperience = '';
  selectedJobType = '';
  selectedLocation = '';
  selectedJob: Job | null = null;
  isModalOpen = false;
  locations: string[] = [];
  allDataLoaded = false;
  private destroy$ = new Subject<void>();
  private searchSubject = new Subject<string>();
  private previousJobIds = new Set<string>(); // Track previously loaded job IDs
  private activeObserver: IntersectionObserver | null = null; // Store active observer for cleanup
  private jobsCache = new Map<string, Job[]>(); // Cache for job data

  // For infinite scrolling
  @ViewChild('jobsList') jobsList!: ElementRef;

  constructor(
    private jobService: JobService,
    private snackBar: MatSnackBar
  ) {
    // Debounce search input to reduce API calls
    this.searchSubject.pipe(
      debounceTime(300),
      distinctUntilChanged(),
      takeUntil(this.destroy$)
    ).subscribe(() => {
      this.resetAndFetch();
    });
  }

  ngOnInit() {
    this.fetchJobs();
  }

  ngOnDestroy() {
    this.destroy$.next();
    this.destroy$.complete();
    // Clean up IntersectionObserver
    if (this.activeObserver) {
      this.activeObserver.disconnect();
      this.activeObserver = null;
    }
  }

  // Optimize rendering with trackBy
  trackByJobId(index: number, job: Job): string {
    return job.id;
  }

  // STEP 3: Setup Infinite Scrolling - modified to not setup for single job post
  private setupInfiniteScroll() {
    // Don't set up infinite scroll if we have only one job
    if (this.jobs.length <= 1) {
      this.allDataLoaded = true;
      return;
    }
    
    // Disconnect previous observer if exists
    if (this.activeObserver) {
      this.activeObserver.disconnect();
    }
    
    const options = {
      root: null,
      rootMargin: '100px',
      threshold: 0.1
    };

    this.activeObserver = new IntersectionObserver((entries) => {
      entries.forEach(entry => {
        if (entry.isIntersecting && !this.loading && !this.allDataLoaded) {
          this.loadMore();
        }
      });
    }, options);

    // Start observing the last element with a delay to ensure DOM is ready
    setTimeout(() => {
      if (this.activeObserver) {
        this.observeLastElement(this.activeObserver);
      }
    }, 1000);
  }

  private observeLastElement(observer: IntersectionObserver) {
    const elements = this.jobsList?.nativeElement?.children;
    if (elements?.length) {
      observer.observe(elements[elements.length - 1]);
    }
  }

  // STEP 2: Implement Filtering UI
  onSearchChange(value: string) {
    this.searchSubject.next(value);
  }

  onFilterChange() {
    this.resetAndFetch();
  }

  // Memoized version of hasActiveFilters for performance
  private _hasActiveFiltersCache: { value: boolean, dirty: boolean } = { value: false, dirty: true };
  
  hasActiveFilters(): boolean {
    if (this._hasActiveFiltersCache.dirty) {
      this._hasActiveFiltersCache.value = !!(this.searchTerm || this.selectedExperience || this.selectedJobType || this.selectedLocation);
      this._hasActiveFiltersCache.dirty = false;
    }
    return this._hasActiveFiltersCache.value;
  }

  removeFilter(filterType: string) {
    switch (filterType) {
      case 'search':
        this.searchTerm = '';
        break;
      case 'experience':
        this.selectedExperience = '';
        break;
      case 'jobType':
        this.selectedJobType = '';
        break;
      case 'location':
        this.selectedLocation = '';
        break;
    }
    this._hasActiveFiltersCache.dirty = true;
    this.resetAndFetch();
  }

  clearAllFilters() {
    this.searchTerm = '';
    this.selectedExperience = '';
    this.selectedJobType = '';
    this.selectedLocation = '';
    this._hasActiveFiltersCache.dirty = true;
    this.resetAndFetch();
  }

  private resetAndFetch() {
    this.currentPage = 1;
    this.jobs = [];
    this.allDataLoaded = false;
    this.previousJobIds.clear(); // Clear tracked IDs when resetting
    this.fetchJobs();
  }

  // Create a cache key based on filters
  private getCacheKey(filters: JobFilter, page: number): string {
    return `${filters.name || ''}-${filters.experienceLevel || ''}-${filters.jobType || ''}-${filters.location || ''}-${page}`;
  }

  // STEP 1: Integrate the Job List API with the UI - modified to handle single job case
  private fetchJobs() {
    // Don't fetch if all data is already loaded (except when filtering)
    if (this.allDataLoaded && this.currentPage > 1) {
      return;
    }
    
    this.loading = true;
    const filters: JobFilter = {
      name: this.searchTerm,
      experienceLevel: this.selectedExperience,
      jobType: this.selectedJobType,
      location: this.selectedLocation,
      page: this.currentPage
    };

    // Check if we have cached data for this filter+page combination
    const cacheKey = this.getCacheKey(filters, this.currentPage);
    if (this.jobsCache.has(cacheKey)) {
      const cachedJobs = this.jobsCache.get(cacheKey) || [];
      this.handleNewJobs(cachedJobs);
      return;
    }

    this.jobService.getJobs(filters)
      .pipe(
        takeUntil(this.destroy$),
        shareReplay(1), // Cache the response for multiple subscribers
        catchError(error => {
          console.error('Error fetching jobs:', error);
          
          // STEP 4: Handle Empty States and Errors
          this.snackBar.open('Error loading jobs. Please try again.', 'Close', {
            duration: 5000,
            panelClass: ['snack-bar-error']
          });
          
          return of([]);
        })
      )
      .subscribe({
        next: (newJobs) => {
          // Cache the results
          this.jobsCache.set(cacheKey, newJobs);
          this.handleNewJobs(newJobs);
        },
        error: () => {
          // Error is already handled by catchError
          this.loading = false;
        }
      });
  }
  
  // Extract job handling logic to avoid code duplication
  private handleNewJobs(newJobs: Job[]) {
    // Check if we received duplicate jobs (indicating we've reached the end)
    const newJobIds = new Set(newJobs.map(job => job.id));
    const duplicateFound = Array.from(newJobIds).some(id => this.previousJobIds.has(id));
    
    if (newJobs.length === 0 || duplicateFound) {
      // No new jobs or we got duplicates, set allDataLoaded to true
      this.allDataLoaded = true;
      this.loading = false;
      
      if (this.currentPage > 1 && duplicateFound) {
        // Show a message only if we've loaded at least one page and found duplicates
        this.snackBar.open('All available jobs have been loaded', 'Close', {
          duration: 3000,
        });
      }
      return;
    }
    
    // Add new job IDs to the tracking set
    newJobs.forEach(job => this.previousJobIds.add(job.id));
    
    // Append new jobs to existing jobs
    this.jobs = [...this.jobs, ...newJobs];
    this.loading = false;
    this.updateLocations();
    
    // Set allDataLoaded flag if only one job was returned in the first page
    if (this.jobs.length === 1 && this.currentPage === 1) {
      this.allDataLoaded = true;
    } else {
      // Only setup infinite scroll if we have more than 1 job and more jobs might be coming
      this.setupInfiniteScroll();
    }
  }

  private updateLocations() {
    // Extract unique locations from job data for the filter - this is a costly operation, run once
    if (this.locations.length === 0 || this.currentPage === 1) {
      const uniqueLocations = new Set(this.jobs.map(job => job.location));
      this.locations = Array.from(uniqueLocations);
    }
  }

  // STEP 3: Setup Infinite Scrolling
  loadMore() {
    if (!this.loading && !this.allDataLoaded) {
      this.currentPage++;
      this.fetchJobs();
    }
  }

  viewJobDetails(job: Job) {
    this.selectedJob = job;
    this.isModalOpen = true;
  }

  closeModal() {
    this.isModalOpen = false;
    this.selectedJob = null;
  }

  applyForJob(jobData: {job: Job, coverLetter: string}) {
    this.loading = true;
    
    this.jobService.applyForJob(jobData.job.id, jobData.coverLetter)
      .pipe(
        takeUntil(this.destroy$),
        catchError(error => {
          console.error('Error applying for job:', error);
          this.snackBar.open('Failed to submit application. Please try again later.', 'Close', {
            duration: 5000,
            panelClass: ['snack-bar-error']
          });
          return of(null);
        }),
        finalize(() => {
          this.loading = false;
        })
      )
      .subscribe(response => {
        if (response) {
          this.snackBar.open('Application submitted successfully!', 'Close', {
            duration: 3000,
            panelClass: ['snack-bar-success']
          });
          this.closeModal();
        }
      });
  }
}
