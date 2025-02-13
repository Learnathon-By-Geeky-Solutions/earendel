import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';

@Component({
  selector: 'app-quiz-list',
  standalone: true,
  imports: [CommonModule, FormsModule],
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

      @media (max-width: 768px) {
        .quiz-container {
          padding: 16px;
        }

        .header-actions {
          flex-direction: column;
          align-items: stretch;
          gap: 16px;
        }

        .filters {
          flex-direction: column;
        }

        .table-container {
          margin: 0 -16px;
          border-radius: 0;
        }

        th,
        td {
          padding: 12px;
        }
      }
    `,
  ],
})
export class QuizListComponent {
  quizzes: any[] = [
    {
      id: '1',
      quizName: 'JavaScript Fundamentals',
      date: '2024-01-20',
      score: 85,
      totalQuestions: 100,
      duration: '45 minutes',
      status: 'completed',
    },
    {
      id: '2',
      quizName: 'React Advanced Concepts',
      date: '2024-01-15',
      score: 45,
      totalQuestions: 100,
      duration: '60 minutes',
      status: 'failed',
    },
    {
      id: '3',
      quizName: 'System Design Principles',
      date: '2024-01-10',
      score: 0,
      totalQuestions: 100,
      duration: '30 minutes',
      status: 'disqualified',
    },
  ];

  filteredQuizzes: any[] = [];
  paginatedQuizzes: any[] = [];
  searchTerm = '';
  selectedStatus = '';
  currentPage = 1;
  pageSize = 10;
  totalPages = 1;

  constructor(private router: Router) {}

  ngOnInit() {
    this.filteredQuizzes = [...this.quizzes];
    this.updatePagination();
  }

  search() {
    this.filter();
  }

  filter() {
    this.filteredQuizzes = this.quizzes.filter(
      (quiz) =>
        quiz.quizName.toLowerCase().includes(this.searchTerm.toLowerCase()) &&
        (this.selectedStatus === '' || quiz.status === this.selectedStatus)
    );
    this.currentPage = 1;
    this.updatePagination();
  }

  updatePagination() {
    this.totalPages = Math.ceil(this.filteredQuizzes.length / this.pageSize);
    const startIndex = (this.currentPage - 1) * this.pageSize;
    this.paginatedQuizzes = this.filteredQuizzes.slice(
      startIndex,
      startIndex + this.pageSize
    );
  }

  changePage(delta: number) {
    this.currentPage += delta;
    this.updatePagination();
  }

  startNewQuiz() {
    this.router.navigate(['/candidate-dashboard/quiz/start']);
  }
}
