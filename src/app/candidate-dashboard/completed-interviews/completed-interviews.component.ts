import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-completed-interviews',
  standalone: true,
  imports: [CommonModule, FormsModule],
  template: `
    <div class="completed-container">
      <h2>Completed Interviews</h2>
      <div class="search-container">
        <input
          type="text"
          [(ngModel)]="searchTerm"
          (ngModelChange)="search()"
          placeholder="Search completed interviews..."
          class="search-input"
        />
      </div>
      <div class="completed-list">
        <div
          *ngFor="let interview of paginatedInterviews"
          class="completed-card"
        >
          <div class="card-header">
            <h3>{{ interview.companyName }}</h3>
            <div class="rating">
              <i
                class="bi bi-star-fill"
                *ngFor="let star of getStars(interview.rating)"
              ></i>
            </div>
          </div>
          <div class="card-content">
            <div class="detail-row">
              <i class="bi bi-briefcase"></i>
              <span>{{ interview.position }}</span>
            </div>
            <div class="detail-row">
              <i class="bi bi-calendar"></i>
              <span>{{ interview.date }}</span>
            </div>
            <div class="detail-row">
              <i class="bi bi-person"></i>
              <span>{{ interview.interviewerName }}</span>
            </div>
            <div class="feedback" *ngIf="interview.feedback">
              <h4>Feedback</h4>
              <p>{{ interview.feedback }}</p>
            </div>
          </div>
        </div>
      </div>
      <div class="pagination">
        <button
          (click)="changePage(-1)"
          [disabled]="currentPage === 1"
          class="pagination-btn"
        >
          Previous
        </button>
        <span>Page {{ currentPage }} of {{ totalPages }}</span>
        <button
          (click)="changePage(1)"
          [disabled]="currentPage === totalPages"
          class="pagination-btn"
        >
          Next
        </button>
      </div>
    </div>
  `,
  styles: [
    `
      .completed-container {
        padding: 24px;
      }

      h2 {
        font-size: 24px;
        font-weight: 600;
        margin-bottom: 24px;
      }

      .search-container {
        margin-bottom: 24px;
      }

      .search-input {
        width: 100%;
        padding: 10px;
        border: 1px solid #dee2e6;
        border-radius: 8px;
        font-size: 14px;
      }

      .completed-list {
        display: grid;
        grid-template-columns: repeat(auto-fill, minmax(300px, 1fr));
        gap: 24px;
      }

      .completed-card {
        background: white;
        border-radius: 12px;
        padding: 20px;
        box-shadow: 0 1px 3px rgba(0, 0, 0, 0.1);
      }

      .card-header {
        display: flex;
        justify-content: space-between;
        align-items: center;
        margin-bottom: 16px;

        h3 {
          font-size: 18px;
          font-weight: 600;
          margin: 0;
          color: #333;
        }
      }

      .rating.rating {
        color: #ffc107;

        i {
          margin-left: 2px;
        }
      }

      .card-content {
        display: flex;
        flex-direction: column;
        gap: 12px;
      }

      .detail-row {
        display: flex;
        align-items: center;
        gap: 8px;
        color: #666;
        font-size: 14px;

        i {
          font-size: 16px;
          color: #999;
        }
      }

      .feedback {
        margin-top: 16px;
        padding-top: 16px;
        border-top: 1px solid #eee;

        h4 {
          font-size: 14px;
          font-weight: 600;
          margin: 0 0 8px;
          color: #333;
        }

        p {
          font-size: 14px;
          color: #666;
          margin: 0;
          line-height: 1.5;
        }
      }

      .pagination {
        display: flex;
        justify-content: center;
        align-items: center;
        margin-top: 24px;
        gap: 16px;
      }

      .pagination-btn {
        padding: 8px 16px;
        background: #f8f9fa;
        border: 1px solid #dee2e6;
        border-radius: 8px;
        font-size: 14px;
        cursor: pointer;
        transition: all 0.2s ease;

        &:hover:not(:disabled) {
          background: #e9ecef;
        }

        &:disabled {
          opacity: 0.5;
          cursor: not-allowed;
        }
      }

      @media (max-width: 768px) {
        .completed-container {
          padding: 16px;
        }

        .completed-list {
          grid-template-columns: 1fr;
        }
      }
    `,
  ],
})
export class CompletedInterviewsComponent {
  interviews: any[] = [
    {
      id: '1',
      companyName: 'Tech Corp',
      position: 'Senior Frontend Developer',
      date: '2024-01-20',
      feedback:
        'Excellent technical skills and communication. Strong understanding of React and system design.',
      rating: 5,
      interviewerName: 'John Smith',
    },
    {
      id: '2',
      companyName: 'Innovation Labs',
      position: 'Full Stack Developer',
      date: '2024-01-15',
      feedback:
        'Good problem-solving skills. Could improve on system design concepts.',
      rating: 4,
      interviewerName: 'Sarah Johnson',
    },
    {
      id: '2',
      companyName: 'Innovation Labs',
      position: 'Full Stack Developer',
      date: '2024-01-15',
      feedback:
        'Good problem-solving skills. Could improve on system design concepts.',
      rating: 4,
      interviewerName: 'Sarah Johnson',
    },
    {
      id: '2',
      companyName: 'Innovation Labs',
      position: 'Full Stack Developer',
      date: '2024-01-15',
      feedback:
        'Good problem-solving skills. Could improve on system design concepts.',
      rating: 4,
      interviewerName: 'Sarah Johnson',
    },
    {
      id: '2',
      companyName: 'Innovation Labs',
      position: 'Full Stack Developer',
      date: '2024-01-15',
      feedback:
        'Good problem-solving skills. Could improve on system design concepts.',
      rating: 4,
      interviewerName: 'Sarah Johnson',
    },
    {
      id: '2',
      companyName: 'Innovation Labs',
      position: 'Full Stack Developer',
      date: '2024-01-15',
      feedback:
        'Good problem-solving skills. Could improve on system design concepts.',
      rating: 4,
      interviewerName: 'Sarah Johnson',
    },
    {
      id: '2',
      companyName: 'Innovation Labs',
      position: 'Full Stack Developer',
      date: '2024-01-15',
      feedback:
        'Good problem-solving skills. Could improve on system design concepts.',
      rating: 4,
      interviewerName: 'Sarah Johnson',
    },
    {
      id: '2',
      companyName: 'Innovation Labs',
      position: 'Full Stack Developer',
      date: '2024-01-15',
      feedback:
        'Good problem-solving skills. Could improve on system design concepts.',
      rating: 4,
      interviewerName: 'Sarah Johnson',
    },
  ];

  filteredInterviews: any[] = [];
  paginatedInterviews: any[] = [];
  searchTerm = '';
  currentPage = 1;
  pageSize = 4;
  totalPages = 1;

  ngOnInit() {
    this.filteredInterviews = [...this.interviews];
    this.updatePagination();
  }

  search() {
    this.filteredInterviews = this.interviews.filter(
      (interview) =>
        interview.companyName
          .toLowerCase()
          .includes(this.searchTerm.toLowerCase()) ||
        interview.position
          .toLowerCase()
          .includes(this.searchTerm.toLowerCase()) ||
        interview.interviewerName
          .toLowerCase()
          .includes(this.searchTerm.toLowerCase())
    );
    this.currentPage = 1;
    this.updatePagination();
  }

  updatePagination() {
    this.totalPages = Math.ceil(this.filteredInterviews.length / this.pageSize);
    const startIndex = (this.currentPage - 1) * this.pageSize;
    this.paginatedInterviews = this.filteredInterviews.slice(
      startIndex,
      startIndex + this.pageSize
    );
  }

  changePage(delta: number) {
    this.currentPage += delta;
    this.updatePagination();
  }

  getStars(rating: number): number[] {
    return Array(rating).fill(0);
  }
}
