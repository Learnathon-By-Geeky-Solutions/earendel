import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';

@Component({
  selector: 'app-quiz-list',
  standalone: true,
  imports: [CommonModule, FormsModule, MatSnackBarModule],
  template: `
    <div class="quiz-container">
      <div class="header-actions">
        <h2>My Quizzes</h2>
        <button class="participate-btn" (click)="startNewQuiz()">
          <i class="bi bi-play-circle"></i>
          Participate in Quiz
        </button>
      </div>

      <div class="filters">
        <input
          type="text"
          [(ngModel)]="searchTerm"
          (ngModelChange)="search()"
          placeholder="Search quizzes..."
          class="search-input"
        />
        <select
          [(ngModel)]="selectedStatus"
          (ngModelChange)="filter()"
          class="filter-select"
        >
          <option value="">All Status</option>
          <option value="completed">Completed</option>
          <option value="failed">Failed</option>
          <option value="disqualified">Disqualified</option>
        </select>
      </div>

      <div class="table-container">
        <table>
          <thead>
            <tr>
              <th>Quiz Name</th>
              <th>Date</th>
              <th>Score</th>
              <th>Questions</th>
              <th>Duration</th>
              <th>Status</th>
            </tr>
          </thead>
          <tbody>
            <tr *ngFor="let quiz of paginatedQuizzes">
              <td>{{ quiz.quizName }}</td>
              <td>{{ quiz.date }}</td>
              <td>{{ quiz.score }}/{{ quiz.totalQuestions }}</td>
              <td>{{ quiz.totalQuestions }} questions</td>
              <td>{{ quiz.duration }}</td>
              <td>
                <span [class]="'status-badge ' + quiz.status">
                  {{ quiz.status }}
                </span>
              </td>
            </tr>
          </tbody>
        </table>
      </div>

      <div *ngIf="paginatedQuizzes.length === 0" class="no-results">
        <p>No quiz records found.</p>
        <button class="primary-btn" (click)="startNewQuiz()">Take Your First Quiz</button>
      </div>

      <div class="pagination" *ngIf="paginatedQuizzes.length > 0">
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
      .quiz-container {
        padding: 24px;
      }

      .header-actions {
        display: flex;
        justify-content: space-between;
        align-items: center;
        margin-bottom: 24px;

        h2 {
          font-size: 24px;
          font-weight: 600;
          margin: 0;
        }
      }

      .participate-btn {
        display: flex;
        align-items: center;
        gap: 8px;
        padding: 10px 20px;
        background: #0066ff;
        color: white;
        border: none;
        border-radius: 8px;
        font-size: 14px;
        cursor: pointer;
        transition: all 0.2s ease;

        &:hover {
          background: #0052cc;
        }

        i {
          font-size: 18px;
        }
      }

      .filters {
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

      .table-container {
        background: white;
        border-radius: 12px;
        box-shadow: 0 1px 3px rgba(0, 0, 0, 0.1);
        overflow: auto;
        margin-bottom: 24px;
      }

      table {
        width: 100%;
        border-collapse: collapse;
      }

      th,
      td {
        padding: 16px;
        text-align: left;
        border-bottom: 1px solid #eee;
      }

      th {
        font-weight: 500;
        color: #666;
        background: #f8f9fa;
        font-size: 14px;
      }

      td {
        font-size: 14px;
        color: #333;
      }

      .status-badge {
        padding: 4px 12px;
        border-radius: 20px;
        font-size: 12px;
        font-weight: 500;
      }

      .completed {
        background: #e6f4ea;
        color: #1e7e34;
      }

      .failed {
        background: #fff3e0;
        color: #f57c00;
      }

      .disqualified {
        background: #feeeee;
        color: #dc3545;
      }

      .no-results {
        background: white;
        border-radius: 12px;
        padding: 32px;
        text-align: center;
        box-shadow: 0 1px 3px rgba(0, 0, 0, 0.1);
        margin-bottom: 24px;
        
        p {
          color: #666;
          margin-bottom: 16px;
        }
      }

      .primary-btn {
        padding: 10px 24px;
        background: #0066ff;
        color: white;
        border: none;
        border-radius: 8px;
        font-size: 14px;
        cursor: pointer;
        transition: all 0.2s ease;

        &:hover {
          background: #0052cc;
        }
      }

      .pagination {
        display: flex;
        justify-content: center;
        align-items: center;
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
    `,
  ],
})
export class QuizListComponent implements OnInit {
  quizzes: any[] = [
    {
      id: 1,
      quizName: 'Frontend Developer Quiz',
      date: '2023-03-15',
      score: 8,
      totalQuestions: 10,
      duration: '12 min 30 sec',
      status: 'completed',
    },
    {
      id: 2,
      quizName: 'Backend Developer Quiz',
      date: '2023-02-22',
      score: 6,
      totalQuestions: 10,
      duration: '15 min 45 sec',
      status: 'completed',
    },
    {
      id: 3,
      quizName: 'Mobile Developer Quiz',
      date: '2023-01-10',
      score: 4,
      totalQuestions: 10,
      duration: '11 min 20 sec',
      status: 'failed',
    }
  ];

  filteredQuizzes: any[] = [];
  paginatedQuizzes: any[] = [];
  searchTerm = '';
  selectedStatus = '';
  currentPage = 1;
  pageSize = 10;
  totalPages = 1;

  constructor(
    private router: Router,
    private snackBar: MatSnackBar
  ) {}

  ngOnInit() {
    this.filteredQuizzes = [...this.quizzes];
    this.updatePagination();
  }

  search() {
    this.filter(); // This will handle searching and filtering
  }

  filter() {
    this.filteredQuizzes = this.quizzes.filter((quiz) => {
      const matchesSearch = this.searchTerm
        ? quiz.quizName
            .toLowerCase()
            .includes(this.searchTerm.toLowerCase())
        : true;
      const matchesStatus = this.selectedStatus
        ? quiz.status === this.selectedStatus
        : true;
      return matchesSearch && matchesStatus;
    });
    
    this.currentPage = 1; // Reset to first page when filter changes
    this.updatePagination();
  }

  updatePagination() {
    const startIndex = (this.currentPage - 1) * this.pageSize;
    const endIndex = startIndex + this.pageSize;
    this.paginatedQuizzes = this.filteredQuizzes.slice(startIndex, endIndex);
    this.totalPages = Math.max(
      1,
      Math.ceil(this.filteredQuizzes.length / this.pageSize)
    );
  }

  changePage(delta: number) {
    this.currentPage += delta;
    this.updatePagination();
  }

  startNewQuiz() {
    // Check if there's an existing quiz attempt
    if (sessionStorage.getItem('quizAttemptId')) {
      this.snackBar.open('You have an active quiz session. Do you want to continue?', 'Continue', {
        duration: 5000
      }).onAction().subscribe(() => {
        this.router.navigate(['/candidate-dashboard/quiz/interface']);
      });
    } else {
      // Start new quiz
      this.router.navigate(['/candidate-dashboard/quiz/start']);
    }
  }
}
