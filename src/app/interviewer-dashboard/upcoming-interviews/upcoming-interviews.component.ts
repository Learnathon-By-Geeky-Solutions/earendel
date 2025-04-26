import { Component, OnInit } from '@angular/core';
import { CommonModule, formatDate } from '@angular/common';
import { RouterModule } from '@angular/router';
import { SidebarComponent } from '../sidebar/sidebar.component';
import { PaginationComponent } from '../pagination/pagination.component';
import { InterviewerService, InterviewRequest, Interview as ApiInterview } from '../services/interviewer.service';

interface Interview {
  id: string;
  candidate: string;
  role: string;
  company: string;
  date: string;
  time: string;
  description?: string;
  requirements?: string;
  experienceLevel?: string;
  meetingId?: string; // Zoom meeting ID
  interviewerId?: string; // Interviewer ID for Zoom meeting URL
}

interface ProcessedInterview extends InterviewRequest {
  date: string;
  time: string;
}

@Component({
  selector: 'app-upcoming-interviews',
  standalone: true,
  imports: [CommonModule, RouterModule, SidebarComponent, PaginationComponent],
  template: `
    <div class="d-flex">
      <app-sidebar> </app-sidebar>
      <div class="container-fluid py-4">
        <h1 class="h3 mb-2">Upcoming Interviews</h1>
        <p class="text-muted mb-4">
          You have {{ totalItems }} upcoming interviews
        </p>

        <!-- Loading state -->
        <div *ngIf="isLoading" class="text-center py-5">
          <div class="spinner-border text-primary" role="status">
            <span class="visually-hidden">Loading...</span>
          </div>
          <p class="mt-3 text-muted">Loading your upcoming interviews...</p>
        </div>

        <!-- Error state -->
        <div *ngIf="error && !isLoading" class="alert alert-danger">
          <i class="bi bi-exclamation-triangle-fill me-2"></i>
          {{ error }}
          <button class="btn btn-sm btn-outline-danger ms-3" (click)="loadInterviews()">Try Again</button>
        </div>

        <!-- No data state -->
        <div *ngIf="!isLoading && !error && interviews.length === 0" class="text-center py-5">
          <i class="bi bi-calendar-x fs-1 text-muted mb-3"></i>
          <p class="lead">No upcoming interviews found</p>
          <p class="text-muted">When you accept interview requests, they will appear here.</p>
        </div>

        <!-- Data state -->
        <div class="card border-2 shadow-md" *ngIf="!isLoading && !error && interviews.length > 0">
          <div class="card-body p-0">
            <div class="table-responsive">
              <table class="table table-hover mb-0">
                <thead>
                  <tr>
                    <th class="ps-4">Candidate</th>
                    <th>Role</th>
                    <th>Company</th>
                    <th>Date</th>
                    <th>Time</th>
                    <th class="pe-4">Actions</th>
                  </tr>
                </thead>
                <tbody>
                  <tr *ngFor="let interview of paginatedInterviews">
                    <td class="ps-4">{{ interview.candidate }}</td>
                    <td>{{ interview.role }}</td>
                    <td>{{ interview.company }}</td>
                    <td>{{ interview.date }}</td>
                    <td>{{ interview.time }}</td>
                    <td class="pe-4">
                      <div class="d-flex gap-2">
                        <button class="btn btn-outline-dark btn-sm" (click)="viewDetails(interview)">
                          View Details
                        </button>
                        <button 
                          *ngIf="hasValidMeeting(interview)" 
                          class="btn btn-primary btn-sm" 
                          (click)="joinZoomMeeting(interview, $event)">
                          Join Meeting
                        </button>
                      </div>
                    </td>
                  </tr>
                </tbody>
              </table>
            </div>
            <div class="border-top">
              <app-pagination
                [currentPage]="currentPage"
                [pageSize]="pageSize"
                [totalItems]="totalItems"
                (pageChange)="onPageChange($event)"
              ></app-pagination>
            </div>
          </div>
        </div>
      </div>
    </div>

    <!-- Interview Details Modal -->
    <div class="modal fade" [class.show]="showDetailsModal" [class.d-block]="showDetailsModal" tabindex="-1" role="dialog">
      <div class="modal-dialog modal-dialog-centered" role="document">
        <div class="modal-content" *ngIf="selectedInterview">
          <div class="modal-header">
            <h5 class="modal-title">Interview Details</h5>
            <button type="button" class="btn-close" (click)="closeDetailsModal()"></button>
          </div>
          <div class="modal-body">
            <div class="mb-3">
              <h6 class="fw-bold mb-1">Candidate</h6>
              <p>{{ selectedInterview.candidate }}</p>
            </div>
            <div class="mb-3">
              <h6 class="fw-bold mb-1">Role</h6>
              <p>{{ selectedInterview.role }}</p>
            </div>
            <div class="mb-3">
              <h6 class="fw-bold mb-1">Company</h6>
              <p>{{ selectedInterview.company }}</p>
            </div>
            <div class="mb-3">
              <h6 class="fw-bold mb-1">Date & Time</h6>
              <p>{{ selectedInterview.date }} at {{ selectedInterview.time }}</p>
            </div>
            <div class="mb-3" *ngIf="selectedInterview.description">
              <h6 class="fw-bold mb-1">Job Description</h6>
              <p class="text-muted">{{ selectedInterview.description }}</p>
            </div>
            <div class="mb-3" *ngIf="selectedInterview.requirements">
              <h6 class="fw-bold mb-1">Requirements</h6>
              <p class="text-muted">{{ selectedInterview.requirements }}</p>
            </div>
            <div class="mb-3" *ngIf="selectedInterview.experienceLevel">
              <h6 class="fw-bold mb-1">Experience Level</h6>
              <p class="text-muted">{{ selectedInterview.experienceLevel }}</p>
            </div>
          </div>
          <div class="modal-footer">
            <button 
              *ngIf="hasValidMeeting(selectedInterview)" 
              class="btn btn-primary" 
              (click)="joinZoomMeeting(selectedInterview, $event)">
              Join Meeting
            </button>
            <button type="button" class="btn btn-outline-secondary" (click)="closeDetailsModal()">Close</button>
          </div>
        </div>
      </div>
    </div>
    <div class="modal-backdrop fade" [class.show]="showDetailsModal" *ngIf="showDetailsModal"></div>
  `,
  styles: [
    `
      .nav-link {
        color: #666;
        transition: all 0.3s;
      }
      .nav-link:hover,
      .nav-link.active {
        background-color: #f8f9fa;
        color: #000;
      }
      .card {
        transition: transform 0.2s;
      }
      .card:hover {
        transform: translateY(-2px);
      }
      .activity-icon {
        width: 40px;
        height: 40px;
        display: flex;
        align-items: center;
        justify-content: center;
      }
      .upcoming-interviews {
        max-height: 300px;
        overflow-y: auto;
      }
      .table {
        margin-bottom: 0;
      }
      .table th {
        border-top: none;
        border-bottom-width: 1px;
        font-weight: 500;
        text-transform: uppercase;
        font-size: 0.75rem;
        color: #6c757d;
        padding: 1rem;
      }
      .table td {
        padding: 1rem;
        vertical-align: middle;
      }
      .btn-outline-dark {
        border-color: #dee2e6;
      }
      .btn-outline-dark:hover {
        background-color: #000;
        border-color: #000;
      }
      .modal-backdrop.show {
        opacity: 0.5;
      }
    `,
  ],
})
export class UpcomingInterviewsComponent implements OnInit {
  interviews: ProcessedInterview[] = [];
  paginatedInterviews: ProcessedInterview[] = [];
  isLoading = true;
  error: string | null = null;
  
  // Pagination
  currentPage = 1;
  pageSize = 10;
  totalItems = 0;
  
  // Modal
  showDetailsModal = false;
  selectedInterview: ProcessedInterview | null = null;

  constructor(private interviewerService: InterviewerService) { }

  ngOnInit(): void {
    this.loadInterviews();
  }

  loadInterviews(): void {
    this.isLoading = true;
    this.error = null;
    
    this.interviewerService.getAcceptedInterviews().subscribe({
      next: (data) => {
        // Process the interviews to add formatted date and time
        this.interviews = data.map(interview => {
          const { date, time } = this.parseInterviewDate(interview.requestedDate);
          return {
            ...interview,
            date,
            time
          };
        });
        
        this.totalItems = this.interviews.length;
        this.updatePaginatedInterviews();
        this.isLoading = false;
      },
      error: (err) => {
        console.error('Error loading interviews:', err);
        this.error = 'Failed to load upcoming interviews. Please try again later.';
        this.isLoading = false;
      }
    });
  }

  parseInterviewDate(dateString: string): { date: string, time: string } {
    try {
      // Create a date object from the ISO string
      const date = new Date(dateString);
      
      // Format date and time for display in local timezone
      return {
        date: formatDate(date, 'MMM d, yyyy', 'en-US'),
        time: formatDate(date, 'h:mm a', 'en-US')
      };
    } catch (error) {
      console.error('Error parsing date:', error);
      return {
        date: 'Invalid Date',
        time: 'Invalid Time'
      };
    }
  }

  updatePaginatedInterviews(): void {
    const startIndex = (this.currentPage - 1) * this.pageSize;
    this.paginatedInterviews = this.interviews.slice(startIndex, startIndex + this.pageSize);
  }

  onPageChange(page: number): void {
    this.currentPage = page;
    this.updatePaginatedInterviews();
  }

  viewDetails(interview: ProcessedInterview): void {
    this.selectedInterview = interview;
    this.showDetailsModal = true;
  }

  closeDetailsModal(): void {
    this.showDetailsModal = false;
    this.selectedInterview = null;
  }

  hasValidMeeting(interview: ProcessedInterview): boolean {
    return !!interview.meetingId;
  }

  joinZoomMeeting(interview: ProcessedInterview, event: Event): void {
    event.stopPropagation();
    
    if (!interview.meetingId) {
      console.error('No meeting ID available for this interview');
      return;
    }
    
    // Format the Zoom meeting URL
    const meetingId = interview.meetingId;
    const interviewerId = interview.interviewerId;
    
    // Construct the Zoom meeting URL
    const zoomUrl = `https://talent-mesh-frontend.netlify.app/meeting?interviewerId=${interviewerId}&meetingNumber=${meetingId}`;
    
    // Open in a new tab
    window.open(zoomUrl, '_blank');
  }
}
