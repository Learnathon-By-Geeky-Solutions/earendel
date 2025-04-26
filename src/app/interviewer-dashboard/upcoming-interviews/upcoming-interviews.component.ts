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
  templateUrl: './upcoming-interviews.component.html',
  styleUrls: ['./upcoming-interviews.component.css']
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
    
    // Add refresh animation to button if it exists
    const refreshBtn = document.querySelector('.refresh-btn i');
    if (refreshBtn) {
      refreshBtn.classList.add('spinning');
    }
    
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
        
        // Remove spinning animation
        if (refreshBtn) {
          setTimeout(() => {
            refreshBtn.classList.remove('spinning');
          }, 500);
        }
      },
      error: (err) => {
        console.error('Error loading interviews:', err);
        this.error = 'Failed to load upcoming interviews. Please try again later.';
        this.isLoading = false;
        
        // Remove spinning animation
        if (refreshBtn) {
          refreshBtn.classList.remove('spinning');
        }
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
