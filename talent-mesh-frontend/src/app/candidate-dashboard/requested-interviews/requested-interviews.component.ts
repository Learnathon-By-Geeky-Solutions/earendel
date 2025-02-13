import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-requested-interviews',
  standalone: true,
  imports: [CommonModule, FormsModule],
  template: `
    <div class="interviews-container">
      <h2>Requested Interviews</h2>
      <div class="search-container">
        <input
          type="text"
          [(ngModel)]="searchTerm"
          (ngModelChange)="search()"
          placeholder="Search interviews..."
          class="search-input"
        />
      </div>
      <div class="interviews-list">
        <div
          *ngFor="let interview of paginatedInterviews"
          class="interview-card"
        >
          <div class="card-header">
            <h3>{{ interview.companyName }}</h3>
            <span [class]="'status-badge ' + interview.status">
              {{ interview.status }}
            </span>
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
              <i class="bi bi-clock"></i>
              <span>{{ interview.time }}</span>
            </div>
            <div class="detail-row">
              <i class="bi bi-person"></i>
              <span>{{ interview.interviewerName }}</span>
            </div>
          </div>
          <div class="card-actions">
            <button class="action-btn" *ngIf="interview.status === 'scheduled'">
              Join Interview
            </button>
            <button class="cancel-btn" *ngIf="interview.status === 'pending'">
              Cancel Request
            </button>
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
      .interviews-container {
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

      .interviews-list {
        display: grid;
        grid-template-columns: repeat(auto-fill, minmax(300px, 1fr));
        gap: 24px;
      }

      .interview-card {
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

      .status-badge {
        padding: 4px 12px;
        border-radius: 20px;
        font-size: 12px;
        font-weight: 500;
      }

      .pending {
        background: #fff3e0;
        color: #f57c00;
      }

      .scheduled {
        background: #e6f4ea;
        color: #1e7e34;
      }

      .cancelled {
        background: #feeeee;
        color: #dc3545;
      }

      .card-content {
        display: flex;
        flex-direction: column;
        gap: 12px;
        margin-bottom: 20px;
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

      .card-actions {
        display: flex;
        gap: 12px;
      }

      .action-btn,
      .cancel-btn {
        flex: 1;
        padding: 8px 16px;
        border: none;
        border-radius: 8px;
        font-size: 14px;
        cursor: pointer;
        transition: all 0.2s ease;
      }

      .action-btn {
        background: #0066ff;
        color: white;

        &:hover {
          background: #0052cc;
        }
      }

      .cancel-btn {
        background: #feeeee;
        color: #dc3545;

        &:hover {
          background: #fdd;
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
        .interviews-container {
          padding: 16px;
        }

        .interviews-list {
          grid-template-columns: 1fr;
        }
      }
    `,
  ],
})
export class RequestedInterviewsComponent {
  interviews: any[] = [
    {
      id: '1',
      companyName: 'Tech Corp',
      position: 'Senior Frontend Developer',
      date: '2024-01-25',
      time: '10:00 AM',
      status: 'scheduled',
      interviewerName: 'John Smith',
    },
    {
      id: '2',
      companyName: 'Innovation Labs',
      position: 'Full Stack Developer',
      date: '2024-01-26',
      time: '2:00 PM',
      status: 'pending',
      interviewerName: 'Sarah Johnson',
    },
    {
      id: '3',
      companyName: 'Innovation Labs',
      position: 'Full Stack Developer',
      date: '2024-01-26',
      time: '2:00 PM',
      status: 'pending',
      interviewerName: 'Sarah Johnson',
    },
    {
      id: '4',
      companyName: 'Innovation Labs',
      position: 'Full Stack Developer',
      date: '2024-01-26',
      time: '2:00 PM',
      status: 'pending',
      interviewerName: 'Sarah Johnson',
    },
    {
      id: '5',
      companyName: 'Innovation Labs',
      position: 'Full Stack Developer',
      date: '2024-01-26',
      time: '2:00 PM',
      status: 'pending',
      interviewerName: 'Sarah Johnson',
    },
    {
      id: '6',
      companyName: 'Innovation Labs',
      position: 'Full Stack Developer',
      date: '2024-01-26',
      time: '2:00 PM',
      status: 'pending',
      interviewerName: 'Sarah Johnson',
    },
    {
      id: '7',
      companyName: 'Innovation Labs',
      position: 'Full Stack Developer',
      date: '2024-01-26',
      time: '2:00 PM',
      status: 'pending',
      interviewerName: 'Sarah Johnson',
    },
    {
      id: '8',
      companyName: 'Innovation Labs',
      position: 'Full Stack Developer',
      date: '2024-01-26',
      time: '2:00 PM',
      status: 'pending',
      interviewerName: 'Sarah Johnson',
    },
    {
      id: '9',
      companyName: 'Tech Corp',
      position: 'Senior Frontend Developer',
      date: '2024-01-25',
      time: '10:00 AM',
      status: 'scheduled',
      interviewerName: 'John Smith',
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
}
