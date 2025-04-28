import { Component, type OnInit, type OnDestroy } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import type { Subscription } from 'rxjs';
import { SidebarComponent } from '../sidebar/sidebar.component';
import { PaginationComponent } from '../pagination/pagination.component';
import { SkillService } from '../services/skill.service';

export interface QuizQuestion {
  id: string;
  questionText: string;
  option1: string;
  option2: string;
  option3: string;
  option4: string;
  correctOption: number;
}

export interface QuizQuestionApiResponse {
  items: QuizQuestion[];
  pageNumber: number;
  pageSize: number;
  totalCount: number;
  totalPages: number;
  hasPrevious: boolean;
  hasNext: boolean;
}

export interface CreateQuizQuestionRequest {
  questionText: string;
  option1: string;
  option2: string;
  option3: string;
  option4: string;
  correctOption: number;
}

export interface UpdateQuizQuestionRequest {
  id: string;
  questionText: string;
  option1: string;
  option2: string;
  option3: string;
  option4: string;
  correctOption: number;
}

@Component({
  selector: 'app-quiz-management',
  standalone: true,
  imports: [CommonModule, FormsModule, SidebarComponent, PaginationComponent],
  template: `
    <div class="d-flex">
      <app-sidebar></app-sidebar>

      <main class="main-content bg-light">
        <div class="p-4">
          <!-- Header -->
          <div class="d-flex justify-content-between align-items-center mb-4">
            <div>
              <h1 class="h4 mb-1">Quiz Questions</h1>
              <p class="text-muted mb-0">
                Manage quiz questions and answers for candidate assessments
              </p>
            </div>
            <button class="btn btn-primary" (click)="openAddQuestionModal()">
              <i class="bi bi-plus-lg me-2"></i>
              Add Question
            </button>
          </div>

          <!-- Search and Filter -->
          <div class="row mb-4">
            <div class="col-md-6 mb-3 mb-md-0">
              <div class="input-group">
                <span class="input-group-text bg-white border-end-0">
                  <i class="bi bi-search text-muted"></i>
                </span>
                <input
                  type="text"
                  class="form-control border-start-0"
                  placeholder="Search questions..."
                  [(ngModel)]="searchTerm"
                  (input)="onSearch()"
                />
              </div>
            </div>
          </div>

          <!-- Questions List -->
          <div class="quiz-container">
            <div *ngIf="isLoading" class="text-center py-5">
              <div class="spinner-border text-primary" role="status">
                <span class="visually-hidden">Loading...</span>
              </div>
            </div>

            <div
              *ngIf="!isLoading && filteredQuestions.length === 0"
              class="text-center py-5"
            >
              <div class="empty-state">
                <i class="bi bi-question-circle fs-1 text-muted mb-3"></i>
                <h5>No quiz questions found</h5>
                <p class="text-muted">
                  Try adjusting your search or add a new question.
                </p>
              </div>
            </div>

            <!-- Questions Cards -->
            <div class="row" *ngIf="!isLoading && filteredQuestions.length > 0">
              <div
                class="col-12 col-lg-6 mb-4"
                *ngFor="let question of filteredQuestions"
              >
                <div class="card border-0 shadow-sm h-100">
                  <div
                    class="card-header bg-white border-0 pt-4 pb-0 d-flex justify-content-between"
                  >
                    <h5 class="card-title mb-0">
                      <span class="badge bg-primary me-2">Q</span>
                      {{ question.questionText }}
                    </h5>
                    <div class="dropdown">
                      <button
                        class="btn btn-sm btn-light"
                        type="button"
                        data-bs-toggle="dropdown"
                        aria-expanded="false"
                      >
                        <i class="bi bi-three-dots-vertical"></i>
                      </button>
                      <ul class="dropdown-menu dropdown-menu-end">
                        <li>
                          <button
                            class="dropdown-item"
                            (click)="openEditQuestionModal(question)"
                          >
                            <i class="bi bi-pencil me-2"></i> Edit
                          </button>
                        </li>
                        <li>
                          <button
                            class="dropdown-item text-danger"
                            (click)="openDeleteQuestionModal(question)"
                          >
                            <i class="bi bi-trash me-2"></i> Delete
                          </button>
                        </li>
                      </ul>
                    </div>
                  </div>
                  <div class="card-body">
                    <div class="options-list">
                      <div
                        class="option-item"
                        *ngFor="
                          let option of getOptionsArray(question);
                          let i = index
                        "
                        [class.correct-option]="
                          question.correctOption === i + 1
                        "
                      >
                        <div class="option-marker">
                          <span class="option-number">{{
                            getOptionLetter(i)
                          }}</span>
                          <i
                            *ngIf="question.correctOption === i + 1"
                            class="bi bi-check-circle-fill text-success"
                          ></i>
                        </div>
                        <div class="option-text">{{ option }}</div>
                      </div>
                    </div>
                  </div>
                </div>
              </div>
            </div>

            <!-- Pagination -->
            <app-pagination
              *ngIf="totalCount > 0"
              [currentPage]="currentPage"
              [pageSize]="pageSize"
              [totalItems]="totalCount"
              (pageChange)="onPageChange($event)"
              class="mt-4"
            ></app-pagination>
          </div>
        </div>
      </main>

      <!-- Add Question Modal -->
      <div
        class="modal"
        [class.show]="showAddQuestionModal"
        [style.display]="showAddQuestionModal ? 'block' : 'none'"
      >
        <div class="modal-dialog modal-dialog-centered modal-lg">
          <div class="modal-content border-0">
            <div class="modal-header border-0">
              <h5 class="modal-title">Add New Question</h5>
              <button
                type="button"
                class="btn-close"
                (click)="closeAddQuestionModal()"
              ></button>
            </div>
            <div class="modal-body">
              <div class="mb-4">
                <label class="form-label fw-medium">Question Text</label>
                <textarea
                  class="form-control"
                  rows="3"
                  [(ngModel)]="newQuestion.questionText"
                  placeholder="Enter your question here"
                ></textarea>
              </div>

              <div class="mb-4">
                <label class="form-label fw-medium">Options</label>
                <p class="text-muted small mb-3">
                  Select the correct answer by clicking the radio button next to
                  it.
                </p>

                <div class="option-input-group mb-3">
                  <div class="form-check">
                    <input
                      class="form-check-input"
                      type="radio"
                      name="correctOption"
                      id="option1"
                      [value]="1"
                      [(ngModel)]="newQuestion.correctOption"
                    />
                    <label class="form-check-label" for="option1">
                      Option A
                    </label>
                  </div>
                  <input
                    type="text"
                    class="form-control"
                    [(ngModel)]="newQuestion.option1"
                    placeholder="Enter option A"
                  />
                </div>

                <div class="option-input-group mb-3">
                  <div class="form-check">
                    <input
                      class="form-check-input"
                      type="radio"
                      name="correctOption"
                      id="option2"
                      [value]="2"
                      [(ngModel)]="newQuestion.correctOption"
                    />
                    <label class="form-check-label" for="option2">
                      Option B
                    </label>
                  </div>
                  <input
                    type="text"
                    class="form-control"
                    [(ngModel)]="newQuestion.option2"
                    placeholder="Enter option B"
                  />
                </div>

                <div class="option-input-group mb-3">
                  <div class="form-check">
                    <input
                      class="form-check-input"
                      type="radio"
                      name="correctOption"
                      id="option3"
                      [value]="3"
                      [(ngModel)]="newQuestion.correctOption"
                    />
                    <label class="form-check-label" for="option3">
                      Option C
                    </label>
                  </div>
                  <input
                    type="text"
                    class="form-control"
                    [(ngModel)]="newQuestion.option3"
                    placeholder="Enter option C"
                  />
                </div>

                <div class="option-input-group mb-3">
                  <div class="form-check">
                    <input
                      class="form-check-input"
                      type="radio"
                      name="correctOption"
                      id="option4"
                      [value]="4"
                      [(ngModel)]="newQuestion.correctOption"
                    />
                    <label class="form-check-label" for="option4">
                      Option D
                    </label>
                  </div>
                  <input
                    type="text"
                    class="form-control"
                    [(ngModel)]="newQuestion.option4"
                    placeholder="Enter option D"
                  />
                </div>
              </div>
            </div>
            <div class="modal-footer border-0">
              <button
                type="button"
                class="btn btn-light"
                (click)="closeAddQuestionModal()"
              >
                Cancel
              </button>
              <button
                type="button"
                class="btn btn-primary"
                [disabled]="!isValidQuestion(newQuestion)"
                (click)="addQuestion()"
              >
                Add Question
              </button>
            </div>
          </div>
        </div>
      </div>

      <!-- Edit Question Modal -->
      <div
        class="modal"
        [class.show]="showEditQuestionModal"
        [style.display]="showEditQuestionModal ? 'block' : 'none'"
      >
        <div class="modal-dialog modal-dialog-centered modal-lg">
          <div class="modal-content border-0">
            <div class="modal-header border-0">
              <h5 class="modal-title">Edit Question</h5>
              <button
                type="button"
                class="btn-close"
                (click)="closeEditQuestionModal()"
              ></button>
            </div>
            <div class="modal-body">
              <div class="mb-4">
                <label class="form-label fw-medium">Question Text</label>
                <textarea
                  class="form-control"
                  rows="3"
                  [(ngModel)]="editingQuestion.questionText"
                  placeholder="Enter your question here"
                ></textarea>
              </div>

              <div class="mb-4">
                <label class="form-label fw-medium">Options</label>
                <p class="text-muted small mb-3">
                  Select the correct answer by clicking the radio button next to
                  it.
                </p>

                <div class="option-input-group mb-3">
                  <div class="form-check">
                    <input
                      class="form-check-input"
                      type="radio"
                      name="editCorrectOption"
                      id="editOption1"
                      [value]="1"
                      [(ngModel)]="editingQuestion.correctOption"
                    />
                    <label class="form-check-label" for="editOption1">
                      Option A
                    </label>
                  </div>
                  <input
                    type="text"
                    class="form-control"
                    [(ngModel)]="editingQuestion.option1"
                    placeholder="Enter option A"
                  />
                </div>

                <div class="option-input-group mb-3">
                  <div class="form-check">
                    <input
                      class="form-check-input"
                      type="radio"
                      name="editCorrectOption"
                      id="editOption2"
                      [value]="2"
                      [(ngModel)]="editingQuestion.correctOption"
                    />
                    <label class="form-check-label" for="editOption2">
                      Option B
                    </label>
                  </div>
                  <input
                    type="text"
                    class="form-control"
                    [(ngModel)]="editingQuestion.option2"
                    placeholder="Enter option B"
                  />
                </div>

                <div class="option-input-group mb-3">
                  <div class="form-check">
                    <input
                      class="form-check-input"
                      type="radio"
                      name="editCorrectOption"
                      id="editOption3"
                      [value]="3"
                      [(ngModel)]="editingQuestion.correctOption"
                    />
                    <label class="form-check-label" for="editOption3">
                      Option C
                    </label>
                  </div>
                  <input
                    type="text"
                    class="form-control"
                    [(ngModel)]="editingQuestion.option3"
                    placeholder="Enter option C"
                  />
                </div>

                <div class="option-input-group mb-3">
                  <div class="form-check">
                    <input
                      class="form-check-input"
                      type="radio"
                      name="editCorrectOption"
                      id="editOption4"
                      [value]="4"
                      [(ngModel)]="editingQuestion.correctOption"
                    />
                    <label class="form-check-label" for="editOption4">
                      Option D
                    </label>
                  </div>
                  <input
                    type="text"
                    class="form-control"
                    [(ngModel)]="editingQuestion.option4"
                    placeholder="Enter option D"
                  />
                </div>
              </div>
            </div>
            <div class="modal-footer border-0">
              <button
                type="button"
                class="btn btn-light"
                (click)="closeEditQuestionModal()"
              >
                Cancel
              </button>
              <button
                type="button"
                class="btn btn-primary"
                [disabled]="!isValidQuestion(editingQuestion)"
                (click)="updateQuestion()"
              >
                Update Question
              </button>
            </div>
          </div>
        </div>
      </div>

      <!-- Delete Question Modal -->
      <div
        class="modal"
        [class.show]="showDeleteQuestionModal"
        [style.display]="showDeleteQuestionModal ? 'block' : 'none'"
      >
        <div class="modal-dialog modal-dialog-centered">
          <div class="modal-content border-0">
            <div class="modal-header border-0">
              <h5 class="modal-title">Delete Question</h5>
              <button
                type="button"
                class="btn-close"
                (click)="closeDeleteQuestionModal()"
              ></button>
            </div>
            <div class="modal-body">
              <p>Are you sure you want to delete this question?</p>
              <div class="alert alert-secondary">
                <strong>{{ deletingQuestion?.questionText }}</strong>
              </div>
              <p class="text-danger">This action cannot be undone.</p>
            </div>
            <div class="modal-footer border-0">
              <button
                type="button"
                class="btn btn-light"
                (click)="closeDeleteQuestionModal()"
              >
                Cancel
              </button>
              <button
                type="button"
                class="btn btn-danger"
                (click)="confirmDeleteQuestion()"
              >
                Delete Question
              </button>
            </div>
          </div>
        </div>
      </div>

      <!-- Modal Backdrop -->
      <div
        class="modal-backdrop fade show"
        *ngIf="
          showAddQuestionModal ||
          showEditQuestionModal ||
          showDeleteQuestionModal
        "
        (click)="closeAllModals()"
      ></div>
    </div>
  `,
  styles: [
    `
      :host {
        display: block;
        min-height: 100vh;
        background-color: #f9fafb;
      }

      .main-content {
        margin-left: 240px;
        width: calc(100% - 240px);
        min-height: 100vh;
      }

      .quiz-container {
        max-height: calc(100vh - 240px);
        overflow-y: auto;
        padding-right: 1rem;
      }

      .card {
        transition: transform 0.2s ease, box-shadow 0.2s ease;
        border-radius: 12px;
        overflow: hidden;
      }

      .card:hover {
        transform: translateY(-5px);
        box-shadow: 0 10px 20px rgba(0, 0, 0, 0.1) !important;
      }

      .card-title {
        font-size: 1rem;
        line-height: 1.5;
      }

      .badge {
        font-weight: 500;
        padding: 0.35rem 0.65rem;
        border-radius: 6px;
      }

      .options-list {
        display: flex;
        flex-direction: column;
        gap: 0.75rem;
      }

      .option-item {
        display: flex;
        align-items: flex-start;
        gap: 1rem;
        padding: 0.75rem;
        border-radius: 8px;
        background-color: #f9fafb;
        transition: background-color 0.2s ease;
      }

      .option-item:hover {
        background-color: #f3f4f6;
      }

      .option-item.correct-option {
        background-color: rgba(16, 185, 129, 0.1);
        border: 1px solid rgba(16, 185, 129, 0.2);
      }

      .option-marker {
        display: flex;
        align-items: center;
        gap: 0.5rem;
      }

      .option-number {
        display: flex;
        align-items: center;
        justify-content: center;
        width: 24px;
        height: 24px;
        border-radius: 50%;
        background-color: #e5e7eb;
        color: #4b5563;
        font-weight: 600;
        font-size: 0.875rem;
      }

      .correct-option .option-number {
        background-color: rgba(16, 185, 129, 0.2);
        color: #065f46;
      }

      .option-text {
        flex: 1;
        font-size: 0.9375rem;
        color: #1f2937;
      }

      .empty-state {
        display: flex;
        flex-direction: column;
        align-items: center;
        padding: 2rem;
      }

      .option-input-group {
        display: flex;
        align-items: center;
        gap: 1rem;
      }

      .option-input-group .form-check {
        min-width: 100px;
      }

      .modal {
        position: fixed;
        top: 0;
        left: 0;
        width: 100%;
        height: 100%;
        background: rgba(0, 0, 0, 0.5);
        display: flex;
        align-items: center;
        justify-content: center;
        z-index: 1000;
      }

      .modal-content {
        background: white;
        border-radius: 12px;
        max-width: 800px;
        width: 100%;
      }

      .modal-backdrop {
        position: fixed;
        top: 0;
        left: 0;
        width: 100%;
        height: 100%;
        background: rgba(0, 0, 0, 0.5);
        z-index: 999;
      }

      @media (max-width: 991.98px) {
        .main-content {
          margin-left: 0;
          width: 100%;
        }
      }
    `,
  ],
})
export class QuizzesComponent implements OnInit, OnDestroy {
  questions: QuizQuestion[] = [];
  filteredQuestions: QuizQuestion[] = [];

  currentPage = 1;
  pageSize = 10;
  totalCount = 0;
  isLoading = false;

  searchTerm = '';

  // Modal states
  showAddQuestionModal = false;
  showEditQuestionModal = false;
  showDeleteQuestionModal = false;

  // Form data
  newQuestion: Partial<QuizQuestion> = {
    correctOption: 1, // Default to first option
  };
  editingQuestion: QuizQuestion = {
    id: '',
    questionText: '',
    option1: '',
    option2: '',
    option3: '',
    option4: '',
    correctOption: 1,
  };
  deletingQuestion: QuizQuestion | null = null;

  private subscriptions: Subscription[] = [];

  constructor(private quizService: SkillService) {}

  ngOnInit() {
    this.loadQuestions();
  }

  ngOnDestroy() {
    this.subscriptions.forEach((sub) => sub.unsubscribe());
  }

  loadQuestions() {
    this.isLoading = true;
    const sub = this.quizService
      .quizQuestionDetailsData({
        pageNumber: this.currentPage,
        pageSize: this.pageSize,
      })
      .subscribe({
        next: (response: any) => {
          this.questions = response.items;
          this.filteredQuestions = [...this.questions];
          this.totalCount = response.totalCount;
          this.isLoading = false;
        },
        error: (error: any) => {
          console.error('Error loading quiz questions:', error);
          this.isLoading = false;

          // Fallback for demo purposes
          this.questions = [
            {
              id: '1',
              questionText: 'What is the capital of France?',
              option1: 'London',
              option2: 'Paris',
              option3: 'Berlin',
              option4: 'Madrid',
              correctOption: 2,
            },
            {
              id: '2',
              questionText:
                'Which of the following is NOT a JavaScript framework?',
              option1: 'React',
              option2: 'Angular',
              option3: 'Vue',
              option4: 'Java',
              correctOption: 4,
            },
            {
              id: '3',
              questionText: 'What does HTML stand for?',
              option1: 'Hyper Text Markup Language',
              option2: 'High Tech Modern Language',
              option3: 'Hyper Transfer Markup Language',
              option4: 'Home Tool Markup Language',
              correctOption: 1,
            },
            {
              id: '4',
              questionText: 'Which of these is a CSS preprocessor?',
              option1: 'jQuery',
              option2: 'SASS',
              option3: 'Express',
              option4: 'TypeScript',
              correctOption: 2,
            },
          ];
          this.filteredQuestions = [...this.questions];
          this.totalCount = this.questions.length;
        },
      });
    this.subscriptions.push(sub);
  }

  onPageChange(page: number) {
    this.currentPage = page;
    this.loadQuestions();
  }

  onSearch() {
    if (!this.searchTerm.trim()) {
      this.filteredQuestions = [...this.questions];
      return;
    }

    const searchTermLower = this.searchTerm.toLowerCase();
    this.filteredQuestions = this.questions.filter(
      (question) =>
        question.questionText.toLowerCase().includes(searchTermLower) ||
        question.option1.toLowerCase().includes(searchTermLower) ||
        question.option2.toLowerCase().includes(searchTermLower) ||
        question.option3.toLowerCase().includes(searchTermLower) ||
        question.option4.toLowerCase().includes(searchTermLower)
    );
  }

  getOptionsArray(question: QuizQuestion): string[] {
    return [
      question.option1,
      question.option2,
      question.option3,
      question.option4,
    ];
  }

  getOptionLetter(index: number): string {
    return String.fromCharCode(65 + index); // A, B, C, D
  }

  // Question CRUD operations
  openAddQuestionModal() {
    this.newQuestion = {
      correctOption: 1, // Default to first option
    };
    this.showAddQuestionModal = true;
  }

  closeAddQuestionModal() {
    this.showAddQuestionModal = false;
    this.newQuestion = {
      correctOption: 1,
    };
  }

  openEditQuestionModal(question: QuizQuestion) {
    this.editingQuestion = { ...question };
    this.showEditQuestionModal = true;
  }

  closeEditQuestionModal() {
    this.showEditQuestionModal = false;
    this.editingQuestion = {
      id: '',
      questionText: '',
      option1: '',
      option2: '',
      option3: '',
      option4: '',
      correctOption: 1,
    };
  }

  openDeleteQuestionModal(question: QuizQuestion) {
    this.deletingQuestion = question;
    this.showDeleteQuestionModal = true;
  }

  closeDeleteQuestionModal() {
    this.showDeleteQuestionModal = false;
    this.deletingQuestion = null;
  }

  isValidQuestion(question: Partial<QuizQuestion>): boolean {
    return !!(
      question.questionText?.trim() &&
      question.option1?.trim() &&
      question.option2?.trim() &&
      question.option3?.trim() &&
      question.option4?.trim() &&
      question.correctOption
    );
  }

  addQuestion() {
    if (this.isValidQuestion(this.newQuestion)) {
      this.isLoading = true;

      const questionData = {
        questionText: this.newQuestion.questionText!,
        option1: this.newQuestion.option1!,
        option2: this.newQuestion.option2!,
        option3: this.newQuestion.option3!,
        option4: this.newQuestion.option4!,
        correctOption: this.newQuestion.correctOption!,
      };

      this.quizService.quizQuestionCreatedData(questionData).subscribe({
        next: (response) => {
          // Reload the questions to include the new one
          this.closeAddQuestionModal();
          this.loadQuestions();
        },
        error: (error: any) => {
          console.error('Error creating quiz question:', error);

          // Fallback for demo purposes
          const mockId = `mock-${Date.now()}`;
          const newQuestion = {
            id: mockId,
            ...questionData,
          };

          this.questions.unshift(newQuestion);
          this.filteredQuestions = [...this.questions];
          this.totalCount = this.questions.length;

          this.closeAddQuestionModal();
          this.isLoading = false;
        },
      });
    }
  }

  updateQuestion() {
    if (this.isValidQuestion(this.editingQuestion)) {
      this.isLoading = true;

      const questionData = {
        id: this.editingQuestion.id,
        questionText: this.editingQuestion.questionText,
        option1: this.editingQuestion.option1,
        option2: this.editingQuestion.option2,
        option3: this.editingQuestion.option3,
        option4: this.editingQuestion.option4,
        correctOption: this.editingQuestion.correctOption,
      };

      this.quizService.quizQuestionUpdateData(questionData).subscribe({
        next: (response) => {
          // Update the question in the local array
          const index = this.questions.findIndex(
            (q) => q.id === questionData.id
          );
          if (index !== -1) {
            this.questions[index] = { ...response };
            this.filteredQuestions = [...this.questions];
          }

          this.closeEditQuestionModal();
          this.loadQuestions();
          this.isLoading = false;
        },
        error: (error) => {
          console.error('Error updating quiz question:', error);

          // Fallback for demo purposes
          const index = this.questions.findIndex(
            (q) => q.id === questionData.id
          );
          if (index !== -1) {
            this.questions[index] = { ...questionData };
            this.filteredQuestions = [...this.questions];
          }

          this.closeEditQuestionModal();
          this.isLoading = false;
        },
      });
    }
  }

  confirmDeleteQuestion() {
    if (this.deletingQuestion) {
      this.isLoading = true;

      this.quizService
        .quizQuestionDeleteData(this.deletingQuestion.id)
        .subscribe({
          next: () => {
            // Remove the question from the local array
            this.questions = this.questions.filter(
              (q) => q.id !== this.deletingQuestion!.id
            );
            this.filteredQuestions = [...this.questions];
            this.totalCount = this.questions.length;

            this.closeDeleteQuestionModal();
            this.isLoading = false;
          },
          error: (error) => {
            console.error('Error deleting quiz question:', error);

            // Fallback for demo purposes
            this.questions = this.questions.filter(
              (q) => q.id !== this.deletingQuestion!.id
            );
            this.filteredQuestions = [...this.questions];
            this.totalCount = this.questions.length;

            this.closeDeleteQuestionModal();
            this.isLoading = false;
          },
        });
    }
  }

  closeAllModals() {
    this.closeAddQuestionModal();
    this.closeEditQuestionModal();
    this.closeDeleteQuestionModal();
  }
}
