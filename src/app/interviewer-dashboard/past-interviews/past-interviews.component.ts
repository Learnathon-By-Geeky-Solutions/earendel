import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { SidebarComponent } from '../sidebar/sidebar.component';
import { InterviewerService } from '../services/interviewer.service';
import { Observable, of } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { PaginationComponent } from '../pagination/pagination.component';

interface ProcessedInterview {
  id: string;
  candidate: string;
  role: string;
  company: string;
  date: string;
  time: string;
  description?: string;
  requirements?: string;
  experienceLevel?: string;
  meetingId?: string;
}

@Component({
  selector: 'app-past-interviews',
  standalone: true,
  imports: [CommonModule, RouterModule, SidebarComponent, PaginationComponent],
  templateUrl: './past-interviews.component.html',
  styleUrl: './past-interviews.component.css'
})
export class PastInterviewsComponent implements OnInit {
  interviews: ProcessedInterview[] = [];
  paginatedInterviews: ProcessedInterview[] = [];
  isLoading = true;
  error = '';
  
  // Pagination
  currentPage = 1;
  pageSize = 5;
  totalItems = 0;

  // Modal
  showDetailsModal = false;
  selectedInterview: ProcessedInterview | null = null;

  constructor(private interviewerService: InterviewerService) {}

  ngOnInit(): void {
    this.loadPastInterviews();
  }

  loadPastInterviews(): void {
    this.isLoading = true;
    this.error = '';
    
    this.interviewerService.getPastInterviews().subscribe({
      next: (interviews) => {
        this.interviews = interviews.map(interview => this.processInterview(interview));
        this.totalItems = this.interviews.length;
        this.updatePaginatedInterviews();
        this.isLoading = false;
      },
      error: (err) => {
        console.error('Error loading past interviews:', err);
        this.error = 'Failed to load past interviews. Please try again.';
        this.isLoading = false;
      }
    });
  }

  processInterview(interview: any): ProcessedInterview {
    try {
      const dateTime = this.parseInterviewDate(interview.requestedDate);
      return {
        id: interview.id,
        candidate: interview.candidate || 'Unknown Candidate',
        role: interview.role || 'Role not specified',
        company: interview.company || 'Company not specified',
        date: dateTime.date,
        time: dateTime.time,
        description: interview.description,
        requirements: interview.requirements,
        experienceLevel: interview.experienceLevel,
        meetingId: interview.meetingId
      };
    } catch (error) {
      console.error('Error processing interview:', error);
      return {
        id: interview.id || 'unknown-id',
        candidate: interview.candidate || 'Unknown Candidate',
        role: interview.role || 'Role not specified',
        company: interview.company || 'Company not specified',
        date: 'Invalid Date',
        time: 'Invalid Time'
      };
    }
  }

  parseInterviewDate(dateString: string): { date: string; time: string } {
    try {
      const date = new Date(dateString);
      if (isNaN(date.getTime())) {
        throw new Error('Invalid date');
      }
      
      // Format date as "MMM DD, YYYY"
      const dateFormatter = new Intl.DateTimeFormat('en-US', {
        month: 'short',
        day: 'numeric',
        year: 'numeric'
      });
      
      // Format time as "hh:mm AM/PM"
      const timeFormatter = new Intl.DateTimeFormat('en-US', {
        hour: 'numeric',
        minute: '2-digit',
        hour12: true
      });
      
      return {
        date: dateFormatter.format(date),
        time: timeFormatter.format(date)
      };
    } catch (error) {
      console.error('Error parsing date:', error);
      return { date: 'Invalid Date', time: 'Invalid Time' };
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
}
