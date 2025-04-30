import {
  Component,
  OnInit,
  ViewChild,
  ElementRef,
  OnDestroy,
} from '@angular/core';
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
import { JobService, Job as BaseJob, JobFilter } from '../services/job.service';
import {
  Subject,
  debounceTime,
  distinctUntilChanged,
  takeUntil,
  of,
} from 'rxjs';
import { catchError, shareReplay, finalize, switchMap } from 'rxjs/operators';

// Extend the base Job interface to include optional properties needed for our UI
interface Job extends BaseJob {
  featured?: boolean;
  salary?: string;
  description: string;
}

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
    JobDetailsModalComponent,
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
          <mat-select
            [(ngModel)]="selectedExperience"
            (selectionChange)="onFilterChange()"
          >
            <mat-option value="">All Experience Levels</mat-option>
            <mat-option *ngFor="let level of experienceLevels" [value]="level">
              {{ level }}
            </mat-option>
          </mat-select>
        </mat-form-field>

        <mat-form-field appearance="outline" class="filter-field">
          <mat-label>Job Type</mat-label>
          <mat-select
            [(ngModel)]="selectedJobType"
            (selectionChange)="onFilterChange()"
          >
            <mat-option value="">All Job Types</mat-option>
            <mat-option *ngFor="let type of jobTypes" [value]="type">
              {{ type }}
            </mat-option>
          </mat-select>
        </mat-form-field>

        <mat-form-field appearance="outline" class="filter-field">
          <mat-label>Location</mat-label>
          <mat-select
            [(ngModel)]="selectedLocation"
            (selectionChange)="onFilterChange()"
          >
            <mat-option value="">All Locations</mat-option>
            <mat-option *ngFor="let location of locations" [value]="location">
              {{ location }}
            </mat-option>
          </mat-select>
        </mat-form-field>
        
        <mat-form-field appearance="outline" class="filter-field">
          <mat-label>Sort By</mat-label>
          <mat-select
            [(ngModel)]="sortBy"
            (selectionChange)="sortJobs()"
          >
            <mat-option value="newest">
              <div class="sort-option">
      
                <span>Newest Posts</span>
              </div>
            </mat-option>
            <mat-option value="oldest">
              <div class="sort-option">
                <mat-icon>arrow_downward</mat-icon>
                <span>Oldest Posts</span>
              </div>
            </mat-option>
            <mat-option value="salaryHigh">
              <div class="sort-option">
                <mat-icon>trending_up</mat-icon>
                <span>Highest Salary</span>
              </div>
            </mat-option>
            <mat-option value="salaryLow">
              <div class="sort-option">
                <mat-icon>trending_down</mat-icon>
                <span>Lowest Salary</span>
              </div>
            </mat-option>
          </mat-select>
          <mat-icon matSuffix>sort</mat-icon>
        </mat-form-field>
      </div>

      <!-- Active Filters as Chips -->
      <div class="active-filters" *ngIf="hasActiveFilters() || sortBy !== 'newest'">
        <span class="filters-label">Active Filters:</span>
        <mat-chip-listbox>
          <mat-chip *ngIf="searchTerm" (removed)="removeFilter('search')">
            Search: {{ searchTerm }}
            <button matChipRemove>
              <mat-icon>cancel</mat-icon>
            </button>
          </mat-chip>
          <mat-chip
            *ngIf="selectedExperience"
            (removed)="removeFilter('experience')"
          >
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
          <mat-chip
            *ngIf="selectedLocation"
            (removed)="removeFilter('location')"
          >
            Location: {{ selectedLocation }}
            <button matChipRemove>
              <mat-icon>cancel</mat-icon>
            </button>
          </mat-chip>
          <mat-chip
            *ngIf="sortBy !== 'newest'"
            (removed)="resetSort()"
            class="sort-chip"
          >
            Sorted by: {{ getSortLabel() }}
            <button matChipRemove>
              <mat-icon>cancel</mat-icon>
            </button>
          </mat-chip>
        </mat-chip-listbox>
        <button mat-button color="primary" (click)="clearAllFilters()">
          Clear All
        </button>
      </div>

      <div
        *ngIf="jobs.length === 1 && !loading && !hasActiveFilters()"
        class="info-banner"
        matTooltip="Currently only one job post is available"
      >
        <mat-icon>info</mat-icon>
        <span>Currently only one job post is available</span>
      </div>
      
      <!-- Job count and sort info -->
      <div class="job-count-info" *ngIf="jobs.length > 0 && !loading">
        <div class="count-info">
          <mat-icon>work</mat-icon>
          <span>Showing <strong>{{ jobs.length }}</strong> jobs</span>
        </div>
        <div class="sort-info">
          <mat-icon [ngClass]="getSortIcon()"></mat-icon>
          <span>Sorted by <strong>{{ getSortLabel() }}</strong></span>
        </div>
      </div>

      <!-- STEP 3: Setup Infinite Scrolling with Jobs List -->
      <div class="jobs-list" #jobsList>
        <div *ngFor="let job of jobs; trackBy: trackByJobId" class="job-card">
          <div class="card-header">
            <div class="title-container">
              <h3>{{ job.name }}</h3>
              <span *ngIf="job.featured" class="featured-badge">Featured</span>
            </div>
            <div class="posted-date">
              <mat-icon class="small-icon">schedule</mat-icon>
              <span>Posted {{ job.createdOn | date }}</span>
            </div>
          </div>
          
          <div class="card-content">
            <div class="detail-row">
              <mat-icon class="detail-icon experience-icon">military_tech</mat-icon>
              <span class="salary-label">Position:</span>

              <span>{{ job.experienceLevel }}</span>
            </div>
            <div class="detail-row">
              <mat-icon class="detail-icon location-icon">location_on</mat-icon>
              <span class="salary-label">Location:</span>
              <span>{{ job.location }}</span>
            </div>
            <div class="detail-row">
            
              <mat-icon class="detail-icon jobtype-icon">business_center</mat-icon>
              <span class="salary-label">Job Type:</span>
              <span>{{ job.jobType }}</span>
            </div>
            <div class="detail-row">
            <mat-icon class="detail-icon salary-icon">attach_money</mat-icon>
            <span class="salary-label">Salary:</span>
        
              <span class="salary-text">{{ job.salary || '' }}</span>
            </div>
            
            <div class="separator"></div>
            
            <div class="description-section">
            <p class="description-label">Description:</p>
              <p class="description-text">{{ job.description || job.requirments }}</p>
              
              <div class="requirements-section">
                <p class="requirements-label">Requirements:</p>
                <p class="requirements-text">{{ job.requirments }}</p>
              </div>
            </div>
          </div>
          
          <div class="card-actions">
            <button
              mat-raised-button
              color="primary"
              class="view-details-btn"
              (click)="viewJobDetails(job)"
            >
              <mat-icon>visibility</mat-icon>
              <span>View Details</span>
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
  styles: [
    `
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
        grid-template-columns: 2fr 1fr 1fr 1fr 1fr;
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
        transition: transform 0.2s, box-shadow 0.2s, border-color 0.3s;
        border: 1px solid rgba(0, 0, 0, 0.08);
        overflow: hidden;
      }

      .job-card:hover {
        transform: translateY(-2px);
        box-shadow: 0 4px 8px rgba(0, 0, 0, 0.1);
        border-color: rgba(25, 118, 210, 0.3);
      }

      .card-header {
        margin-bottom: 16px;
      }

      .title-container {
        display: flex;
        justify-content: space-between;
        align-items: flex-start;
        margin-bottom: 8px;
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

      .featured-badge {
        background-color: rgba(255, 193, 7, 0.2);
        color: #f57c00;
        padding: 4px 8px;
        border-radius: 16px;
        font-size: 12px;
        font-weight: 500;
      }

      .posted-date {
        display: flex;
        align-items: center;
        font-size: 12px;
        color: #666;
      }

      .small-icon {
        font-size: 14px;
        width: 14px;
        height: 14px;
        margin-right: 4px;
      }

      .card-content {
        flex-grow: 1;
      }

      .detail-row {
        display: flex;
        align-items: center;
        gap: 8px;
        margin-bottom: 10px;
        color: #666;
        transition: transform 0.2s;
      }

      .detail-row:hover {
        transform: translateX(3px);
      }

      .detail-icon {
        font-size: 18px;
        width: 18px;
        height: 18px;
      }

      .experience-icon {
        color: rgba(25, 118, 210, 0.7);
      }

      .location-icon {
        color: rgba(244, 67, 54, 0.7);
      }

      .jobtype-icon {
        color: rgba(76, 175, 80, 0.7);
      }

      .salary-icon {
        color: rgba(255, 193, 7, 0.7);
      }

      .salary-text {
        font-weight: 500;
      }

      .separator {
        height: 1px;
        background-color: rgba(0, 0, 0, 0.1);
        margin: 12px 0;
      }

      .description-section {
        margin-top: 10px;
      }

      .description-text {
        font-size: 12px;
        color: #666;
        margin-bottom: 10px;
        display: -webkit-box;
        -webkit-line-clamp: 2;
        -webkit-box-orient: vertical;
        overflow: hidden;
      }

      .requirements-section {
        margin-top: 5px;
      }

      .requirements-label {
        font-size: 12px;
        font-weight: 500;
        margin-bottom: 4px;
      }

      .salary-label {
        font-size: 15px;
        font-weight: 600;
      }


      .description-label {
        font-size: 13px;
        font-weight: 500;
        margin-bottom: 4px;
      }

      .requirements-text {
        font-size: 12px;
        color: #666;
        display: -webkit-box;
        -webkit-line-clamp: 2;
        -webkit-box-orient: vertical;
        overflow: hidden;
      }

      .card-actions {
        margin-top: auto;
        padding-top: 16px;
      }

      .view-details-btn {
        width: 100%;
        background: linear-gradient(to right, rgba(25, 118, 210, 0.9), rgba(25, 118, 210, 1));
        transition: all 0.3s;
      }

      .view-details-btn:hover {
        background: linear-gradient(to right, rgba(25, 118, 210, 1), rgba(25, 118, 210, 0.9));
      }

      .view-details-btn mat-icon {
        margin-right: 8px;
        transition: transform 0.3s;
      }

      .view-details-btn:hover mat-icon {
        transform: scale(1.1);
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

      .sort-option {
        display: flex;
        align-items: center;
        gap: 8px;
      }

      .sort-chip {
        background-color: rgba(25, 118, 210, 0.1);
        color: #1976d2;
      }

      .job-count-info {
        display: flex;
        justify-content: space-between;
        align-items: center;
        padding: 12px 16px;
        background-color: #f8f9fa;
        border-radius: 8px;
        margin-bottom: 16px;
        color: #555;
      }
      
      .count-info, .sort-info {
        display: flex;
        align-items: center;
        gap: 8px;
        font-size: 14px;
      }
      
      .count-info mat-icon, .sort-info mat-icon {
        font-size: 18px;
        width: 18px;
        height: 18px;
        color: #666;
      }
      
      .sort-info .arrow-down {
        color: #1976d2;
      }
      
      .sort-info .arrow-up {
        color: #9c27b0;
      }
      
      .sort-info .trending-up {
        color: #4caf50;
      }
      
      .sort-info .trending-down {
        color: #ff9800;
      }
    `,
  ],
})
export class JobPostsComponent implements OnInit, OnDestroy {
  // STEP 1: Integrate the Job List API with the UI
  jobs: Job[] = [];
  experienceLevels: string[] = [];
  jobTypes: string[] = [];

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
  sortBy: string = 'newest'; // Default sort by newest
  private destroy$ = new Subject<void>();
  private searchSubject = new Subject<string>();
  private previousJobIds = new Set<string>(); // Track previously loaded job IDs
  private activeObserver: IntersectionObserver | null = null; // Store active observer for cleanup
  private jobsCache = new Map<string, Job[]>(); // Cache for job data

  // For infinite scrolling
  @ViewChild('jobsList') jobsList!: ElementRef;

  constructor(private jobService: JobService, private snackBar: MatSnackBar) {
    // Debounce search input to reduce API calls
    this.searchSubject
      .pipe(debounceTime(300), distinctUntilChanged(), takeUntil(this.destroy$))
      .subscribe(() => {
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
      threshold: 0.1,
    };

    this.activeObserver = new IntersectionObserver((entries) => {
      entries.forEach((entry) => {
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
  private _hasActiveFiltersCache: { value: boolean; dirty: boolean } = {
    value: false,
    dirty: true,
  };

  hasActiveFilters(): boolean {
    if (this._hasActiveFiltersCache.dirty) {
      this._hasActiveFiltersCache.value = !!(
        this.searchTerm ||
        this.selectedExperience ||
        this.selectedJobType ||
        this.selectedLocation
      );
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
    this.sortBy = 'newest'; // Reset sort as well
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
    return `${filters.name || ''}-${filters.experienceLevel || ''}-${
      filters.jobType || ''
    }-${filters.location || ''}-${page}-${this.sortBy}`;
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
      page: this.currentPage,
    };

    // Check if we have cached data for this filter+page combination
    const cacheKey = this.getCacheKey(filters, this.currentPage);
    if (this.jobsCache.has(cacheKey)) {
      const cachedJobs = this.jobsCache.get(cacheKey) || [];
      this.handleNewJobs(cachedJobs);
      return;
    }

    this.jobService
      .getJobs(filters)
      .pipe(
        takeUntil(this.destroy$),
        shareReplay(1), // Cache the response for multiple subscribers
        catchError((error) => {
          console.error('Error fetching jobs:', error);

          // STEP 4: Handle Empty States and Errors
          this.snackBar.open('Error loading jobs. Please try again.', 'Close', {
            duration: 5000,
            panelClass: ['snack-bar-error'],
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
        },
      });
  }

  // Extract job handling logic to avoid code duplication
  private handleNewJobs(newJobs: Job[]) {
    // Check if we received duplicate jobs (indicating we've reached the end)
    const newJobIds = new Set(newJobs.map((job) => job.id));
    const duplicateFound = Array.from(newJobIds).some((id) =>
      this.previousJobIds.has(id)
    );

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
    newJobs.forEach((job) => this.previousJobIds.add(job.id));

    // Append new jobs to existing jobs
    this.jobs = [...this.jobs, ...newJobs];
    
    // Sort jobs according to current sort option
    this.sortJobs();
    
    this.loading = false;
    this.updateLocations();
    this.updateExperienceLevels();
    this.updateJobTypes();

    // Set allDataLoaded flag if only one job was returned in the first page
    if (this.jobs.length === 1 && this.currentPage === 1) {
      this.allDataLoaded = true;
    } else {
      // Only setup infinite scroll if we have more than 1 job and more jobs might be coming
      this.setupInfiniteScroll();
    }
  }

  private updateJobTypes() {
    // On first load or reset, build unique job types
    if (this.jobTypes.length === 0 || this.currentPage === 1) {
      const types = new Set(
        this.jobs.map((job) => job.jobType).filter((type) => !!type)
      );
      this.jobTypes = Array.from(types);
    }
  }

  private updateExperienceLevels() {
    // On first page or whenever you reset, recalc unique levels:
    if (this.experienceLevels.length === 0 || this.currentPage === 1) {
      const levels = new Set(
        this.jobs.map((job) => job.experienceLevel).filter((l) => !!l)
      );
      this.experienceLevels = Array.from(levels);
    }
  }

  private updateLocations() {
    // Extract unique locations from job data for the filter - this is a costly operation, run once
    if (this.locations.length === 0 || this.currentPage === 1) {
      const uniqueLocations = new Set(this.jobs.map((job) => job.location));
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

  applyForJob(jobData: { job: Job; coverLetter: string }) {
    this.loading = true;
    const jobId = jobData.job.id;
    const coverLetter = jobData.coverLetter;
    const userData = JSON.parse(sessionStorage.getItem('loggedInUser') || '{}');
    const candidateId = userData.userId || '';

    this.jobService
      .getJobApplications(jobId, candidateId) // 1) check existing
      .pipe(
        switchMap((apps: any) => {
          if (apps && apps.items.length > 0) {
            // Already applied
            this.snackBar.open(
              'You have already applied for this job.',
              'Close',
              { duration: 5000, panelClass: ['snack-bar-warn'] }
            );
            return of(null); // skip actual apply
          }
          // Not applied yet → proceed
          return this.jobService.applyForJob(jobId, coverLetter);
        }),
        catchError((err) => {
          console.error('Error checking or applying:', err);
          this.snackBar.open(
            'Something went wrong. Please try again later.',
            'Close',
            { duration: 5000, panelClass: ['snack-bar-error'] }
          );
          return of(null);
        }),
        finalize(() => (this.loading = false)),
        takeUntil(this.destroy$)
      )
      .subscribe((response) => {
        if (response) {
          this.snackBar.open('Application submitted successfully!', 'Close', {
            duration: 3000,
            panelClass: ['snack-bar-success'],
          });
          this.closeModal();
        }
      });
  }

  sortJobs() {
    this.jobs = [...this.jobs].sort((a, b) => {
      switch (this.sortBy) {
        case 'newest':
          return new Date(b.createdOn).getTime() - new Date(a.createdOn).getTime();
        case 'oldest':
          return new Date(a.createdOn).getTime() - new Date(b.createdOn).getTime();
        case 'salaryHigh':
          return this.extractSalary(b.salary || '0') - this.extractSalary(a.salary || '0');
        case 'salaryLow':
          return this.extractSalary(a.salary || '0') - this.extractSalary(b.salary || '0');
        default:
          return 0;
      }
    });
  }

  // Helper method to extract numeric salary value for sorting
  private extractSalary(salaryString: string): number {
    if (!salaryString) return 0;
    
    // Extract numbers from the salary string
    const matches = salaryString.match(/\$?([\d,]+)(?:\s*-\s*\$?([\d,]+))?/);
    if (!matches) return 0;
    
    // If there's a range, use the higher value for "salaryHigh" and lower for "salaryLow"
    const min = parseInt(matches[1].replace(/,/g, ''), 10);
    const max = matches[2] ? parseInt(matches[2].replace(/,/g, ''), 10) : min;
    
    return this.sortBy === 'salaryHigh' ? max : min;
  }

  resetSort() {
    this.sortBy = 'newest';
    this.sortJobs();
  }

  getSortLabel(): string {
    switch (this.sortBy) {
      case 'newest':
        return 'Newest Posts';
      case 'oldest':
        return 'Oldest Posts';
      case 'salaryHigh':
        return 'Highest Salary';
      case 'salaryLow':
        return 'Lowest Salary';
      default:
        return 'Newest Posts';
    }
  }

  getSortIcon(): string {
    switch (this.sortBy) {
      case 'newest':
        return 'arrow-down';
      case 'oldest':
        return 'arrow-up';
      case 'salaryHigh':
        return 'trending-up';
      case 'salaryLow':
        return 'trending-down';
      default:
        return '';
    }
  }
}
