import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { JobDetailsModalComponent } from '../job-details/job-details.component';

@Component({
  selector: 'app-job-posts',
  standalone: true,
  imports: [CommonModule, FormsModule, JobDetailsModalComponent],
  template: `
    <div class="jobs-container">
      <h2>Job Posts</h2>
      <div class="search-filter-container">
        <input
          type="text"
          [(ngModel)]="searchTerm"
          (ngModelChange)="search()"
          placeholder="Search jobs..."
          class="search-input"
        />
        <select
          [(ngModel)]="selectedExperience"
          (ngModelChange)="filter()"
          class="filter-select"
        >
          <option value="">All Experience</option>
          <option value="0-2">0-2 years</option>
          <option value="3-5">3-5 years</option>
          <option value="5+">5+ years</option>
        </select>
        <select
          [(ngModel)]="selectedStatus"
          (ngModelChange)="filter()"
          class="filter-select"
        >
          <option value="">All Status</option>
          <option value="active">Active</option>
          <option value="closed">Closed</option>
        </select>
      </div>
      <div class="jobs-list">
        <div *ngFor="let job of paginatedJobs" class="job-card">
          <div class="card-header">
            <h3>{{ job.position }}</h3>
            <span [class]="'status-badge ' + job.status">
              {{ job.status }}
            </span>
          </div>
          <div class="card-content">
            <div class="company-info">
              <h4>{{ job.companyName }}</h4>
              <span class="posted-date">Posted {{ job.posted }}</span>
            </div>
            <div class="detail-row">
              <i class="bi bi-briefcase"></i>
              <span>{{ job.experience }} years experience</span>
            </div>
            <div class="detail-row">
              <i class="bi bi-currency-dollar"></i>
              <span>{{ job.salary }}</span>
            </div>
            <div class="skills-list">
              <span *ngFor="let skill of job.skills" class="skill-tag">
                {{ skill }}
              </span>
            </div>
          </div>
          <div class="card-actions">
            <button
              class="view-details-btn"
              [disabled]="job.status === 'closed'"
              (click)="viewJobDetails(job)"
            >
              View Details
            </button>
          </div>
        </div>
      </div>
      <app-job-details-modal
        *ngIf="selectedJob"
        [isOpen]="isModalOpen"
        [job]="selectedJob"
        (closeModal)="closeModal()"
        (applyForJob)="applyForJob($event)"
      ></app-job-details-modal>
      <div class="view-more-btn-container">
        <button
          *ngIf="!allJobsLoaded"
          (click)="loadMoreJobs()"
          class="view-more-btn"
        >
          View More
        </button>
        <button
          *ngIf="allJobsLoaded"
          (click)="hideJobs()"
          class="view-more-btn"
        >
          Hide Jobs
        </button>
      </div>
    </div>
  `,
  styles: [
    `
      .jobs-container {
        padding: 24px;
      }

      h2 {
        font-size: 24px;
        font-weight: 600;
        margin-bottom: 24px;
      }

      .search-filter-container {
        display: flex;
        gap: 16px;
        margin-bottom: 24px;
      }

      .search-input {
        flex: 1;
        padding: 10px;
        border: 1px solid #dee2e6;
        border-radius: 8px;
        font-size: 14px;
      }

      .filter-select {
        padding: 10px;
        border: 1px solid #dee2e6;
        border-radius: 8px;
        font-size: 14px;
        background-color: white;
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
        box-shadow: 0 1px 3px rgba(0, 0, 0, 0.1);
        display: flex;
        flex-direction: column;
        height: 100%;
      }

      .card-header {
        display: flex;
        justify-content: space-between;
        align-items: flex-start;
        margin-bottom: 16px;

        h3 {
          font-size: 18px;
          font-weight: 600;
          margin: 0;
          color: #333;
          flex: 1;
          min-height: 54px; /* Approximately 3 lines of text */
          display: -webkit-box;
          -webkit-line-clamp: 3;
          -webkit-box-orient: vertical;
          overflow: hidden;
        }
      }

      .status-badge {
        padding: 4px 12px;
        border-radius: 20px;
        font-size: 12px;
        font-weight: 500;
        white-space: nowrap;
        margin-left: 8px;
      }

      .active {
        background: #e6f4ea;
        color: #1e7e34;
      }

      .closed {
        background: #feeeee;
        color: #dc3545;
      }

      .card-content {
        flex-grow: 1;
        display: flex;
        flex-direction: column;
      }

      .company-info {
        margin-bottom: 16px;

        h4 {
          font-size: 16px;
          font-weight: 500;
          margin: 0 0 4px;
          color: #333;
          white-space: nowrap;
          overflow: hidden;
          text-overflow: ellipsis;
        }

        .posted-date {
          font-size: 12px;
          color: #666;
        }
      }

      .detail-row {
        display: flex;
        align-items: center;
        gap: 8px;
        color: #666;
        font-size: 14px;
        margin-bottom: 8px;

        i {
          font-size: 16px;
          color: #999;
          flex-shrink: 0;
        }

        span {
          white-space: nowrap;
          overflow: hidden;
          text-overflow: ellipsis;
        }
      }

      .skills-list {
        display: flex;
        flex-wrap: wrap;
        gap: 8px;
        margin: 16px 0;
        min-height: 60px; /* Ensure space for at least two rows of skills */
      }

      .skill-tag {
        padding: 4px 12px;
        background: #f8f9fa;
        border-radius: 16px;
        font-size: 12px;
        color: #666;
      }

      .card-actions {
        margin-top: auto;
        padding-top: 20px;
      }

      .view-details-btn {
        width: 100%;
        padding: 10px;
        background: #f8f9fa;
        color: #666;
        border: 1px solid #dee2e6;
        border-radius: 8px;
        font-size: 14px;
        cursor: pointer;
        transition: all 0.2s ease;

        &:hover:not(:disabled) {
          background: #e9ecef;
        }

        &:disabled {
          background: #e9ecef;
          color: #666;
          cursor: not-allowed;
        }
      }

      .view-more-btn-container {
        text-align: center;
        margin-top: 24px;
      }

      .view-more-btn {
        padding: 10px 20px;
        background: #007bff;
        color: white;
        border: none;
        border-radius: 8px;
        font-size: 16px;
        cursor: pointer;
        transition: all 0.2s ease;
      }

      .view-more-btn:hover {
        background: #0056b3;
      }

      @media (max-width: 768px) {
        .jobs-container {
          padding: 16px;
        }

        .search-filter-container {
          flex-direction: column;
        }

        .jobs-list {
          grid-template-columns: 1fr;
        }
      }
    `,
  ],
})
export class JobPostsComponent {
  jobs: any[] = [
    {
      id: '1',
      companyName: 'Tech Corp',
      position: 'Senior Frontend Developer',
      experience: '5+',
      salary: '$120k - $150k',
      skills: ['React', 'TypeScript', 'Node.js'],
      posted: '2 days ago',
      status: 'active',
    },
    {
      id: '2',
      companyName: 'Innovation Labs',
      position: 'Full Stack Developer',
      experience: '3-5',
      salary: '$90k - $120k',
      skills: ['Angular', 'Express.js', 'MongoDB'],
      posted: '5 days ago',
      status: 'active',
    },
    {
      id: '3',
      companyName: 'Future Enterprises',
      position: 'Backend Engineer',
      experience: '0-2',
      salary: '$70k - $90k',
      skills: ['Node.js', 'Python', 'SQL'],
      posted: '1 week ago',
      status: 'closed',
    },
    {
      id: '2',
      companyName: 'Innovation Labs',
      position: 'Full Stack Developer',
      experience: '3-5',
      salary: '$90k - $120k',
      skills: ['Angular', 'Express.js', 'MongoDB'],
      posted: '5 days ago',
      status: 'active',
    },
    {
      id: '2',
      companyName: 'Innovation Labs',
      position: 'Full Stack Developer',
      experience: '3-5',
      salary: '$90k - $120k',
      skills: ['Angular', 'Express.js', 'MongoDB'],
      posted: '5 days ago',
      status: 'active',
    },
    {
      id: '2',
      companyName: 'Innovation Labs',
      position: 'Full Stack Developer',
      experience: '3-5',
      salary: '$90k - $120k',
      skills: ['Angular', 'Express.js', 'MongoDB'],
      posted: '5 days ago',
      status: 'active',
    },
    {
      id: '2',
      companyName: 'Innovation Labs',
      position: 'Full Stack Developer',
      experience: '3-5',
      salary: '$90k - $120k',
      skills: ['Angular', 'Express.js', 'MongoDB'],
      posted: '5 days ago',
      status: 'active',
    },
    {
      id: '2',
      companyName: 'Innovation Labs',
      position: 'Full Stack Developer',
      experience: '3-5',
      salary: '$90k - $120k',
      skills: ['Angular', 'Express.js', 'MongoDB'],
      posted: '5 days ago',
      status: 'active',
    },
    {
      id: '2',
      companyName: 'Innovation Labs',
      position: 'Full Stack Developer',
      experience: '3-5',
      salary: '$90k - $120k',
      skills: ['Angular', 'Express.js', 'MongoDB'],
      posted: '5 days ago',
      status: 'active',
    },
    {
      id: '2',
      companyName: 'Innovation Labs',
      position: 'Full Stack Developer',
      experience: '3-5',
      salary: '$90k - $120k',
      skills: ['Angular', 'Express.js', 'MongoDB'],
      posted: '5 days ago',
      status: 'active',
    },
    {
      id: '2',
      companyName: 'Innovation Labs',
      position: 'Full Stack Developer',
      experience: '3-5',
      salary: '$90k - $120k',
      skills: ['Angular', 'Express.js', 'MongoDB'],
      posted: '5 days ago',
      status: 'active',
    },
  ];

  paginatedJobs: any[] = [];
  pageSize = 6; // Number of jobs to display per click
  currentPage = 0;
  allJobsLoaded = false;
  searchTerm = '';
  selectedExperience = '';
  selectedStatus = '';
  selectedJob: any = null;
  isModalOpen = false;

  ngOnInit() {
    this.loadMoreJobs();
  }

  loadMoreJobs() {
    const startIndex = this.currentPage * this.pageSize;
    const endIndex = startIndex + this.pageSize;
    const filteredJobs = this.filteredJobs();

    this.paginatedJobs = [
      ...this.paginatedJobs,
      ...filteredJobs.slice(startIndex, endIndex),
    ];

    this.currentPage++;

    if (filteredJobs.length <= this.paginatedJobs.length) {
      this.allJobsLoaded = true;
    }
  }

  hideJobs() {
    this.paginatedJobs = this.filteredJobs().slice(0, this.pageSize);
    this.allJobsLoaded = false;
    this.currentPage = 1;
  }

  filteredJobs() {
    return this.jobs.filter((job) => {
      const matchesSearchTerm =
        !this.searchTerm ||
        job.position.toLowerCase().includes(this.searchTerm.toLowerCase()) ||
        job.companyName.toLowerCase().includes(this.searchTerm.toLowerCase());

      const matchesExperience =
        !this.selectedExperience || job.experience === this.selectedExperience;

      const matchesStatus =
        !this.selectedStatus || job.status === this.selectedStatus;

      return matchesSearchTerm && matchesExperience && matchesStatus;
    });
  }

  search() {
    this.currentPage = 0;
    this.paginatedJobs = [];
    this.loadMoreJobs();
  }

  filter() {
    this.currentPage = 0;
    this.paginatedJobs = [];
    this.loadMoreJobs();
  }

  viewJobDetails(job: any) {
    this.selectedJob = job;
    this.isModalOpen = true;
  }

  closeModal() {
    this.isModalOpen = false;
  }

  applyForJob(job: any) {
    alert(`Applied for job: ${job.position}`);
    this.closeModal();
  }
}
