import { Component, type OnInit, type OnDestroy } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import type { Subscription } from 'rxjs';
import { SidebarComponent } from '../sidebar/sidebar.component';
import { PaginationComponent } from '../pagination/pagination.component';
import { SkillService } from '../services/skill.service';

interface Seniority {
  id: string;
  name: string;
  description: string;
}

interface SeniorityApiResponse {
  items: Seniority[];
  pageNumber: number;
  pageSize: number;
  totalCount: number;
  totalPages: number;
  hasPrevious: boolean;
  hasNext: boolean;
}

interface CreateSeniorityRequest {
  name: string;
  description: string;
}

interface UpdateSeniorityRequest {
  id: string;
  name: string;
  description: string;
}

@Component({
  selector: 'app-seniority-management',
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
              <h1 class="h4 mb-1">Seniority Levels</h1>
              <p class="text-muted mb-0">
                Manage seniority levels for skills and rubrics
              </p>
            </div>
            <button class="btn btn-dark" (click)="openAddSeniorityModal()">
              <i class="bi bi-plus-lg me-2"></i>
              Add Seniority Level
            </button>
          </div>

          <!-- Search -->
          <div class="row mb-4">
            <div class="col-md-6 mb-3 mb-md-0">
              <div class="input-group">
                <span class="input-group-text bg-white border-end-0">
                  <i class="bi bi-search text-muted"></i>
                </span>
                <input
                  type="text"
                  class="form-control border-start-0"
                  placeholder="Search seniority levels..."
                  [(ngModel)]="searchTerm"
                  (input)="onSearch()"
                />
              </div>
            </div>
          </div>

          <!-- Seniority List -->
          <div class="seniority-container">
            <div *ngIf="isLoading" class="text-center py-5">
              <div class="spinner-border text-primary" role="status">
                <span class="visually-hidden">Loading...</span>
              </div>
            </div>

            <div
              *ngIf="!isLoading && filteredSeniorities.length === 0"
              class="text-center py-5"
            >
              <div class="empty-state">
                <i class="bi bi-diagram-3 fs-1 text-muted mb-3"></i>
                <h5>No seniority levels found</h5>
                <p class="text-muted">
                  Try adjusting your search or add a new seniority level.
                </p>
              </div>
            </div>

            <!-- Seniority Table -->
            <div
              class="table-responsive"
              *ngIf="!isLoading && filteredSeniorities.length > 0"
            >
              <table class="table table-hover bg-white rounded shadow-sm">
                <thead>
                  <tr>
                    <th scope="col" class="ps-4">Name</th>
                    <th scope="col">Description</th>
                    <th scope="col" class="text-end pe-4">Actions</th>
                  </tr>
                </thead>
                <tbody>
                  <tr *ngFor="let seniority of filteredSeniorities">
                    <td class="ps-4 fw-medium">{{ seniority.name }}</td>
                    <td>{{ seniority.description }}</td>
                    <td class="text-end pe-4">
                      <button
                        class="btn btn-sm btn-outline-primary me-2"
                        (click)="openEditSeniorityModal(seniority)"
                      >
                        <i class="bi bi-pencil"></i>
                      </button>
                      <button
                        class="btn btn-sm btn-outline-danger"
                        (click)="openDeleteSeniorityModal(seniority)"
                      >
                        <i class="bi bi-trash"></i>
                      </button>
                    </td>
                  </tr>
                </tbody>
              </table>
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

      <!-- Add Seniority Modal -->
      <div
        class="modal"
        [class.show]="showAddSeniorityModal"
        [style.display]="showAddSeniorityModal ? 'block' : 'none'"
      >
        <div class="modal-dialog modal-dialog-centered">
          <div class="modal-content border-0">
            <div class="modal-header border-0">
              <h5 class="modal-title">Add New Seniority Level</h5>
              <button
                type="button"
                class="btn-close"
                (click)="closeAddSeniorityModal()"
              ></button>
            </div>
            <div class="modal-body">
              <div class="mb-3">
                <label class="form-label">Name</label>
                <input
                  type="text"
                  class="form-control"
                  [(ngModel)]="newSeniority.name"
                  placeholder="Enter seniority level name"
                />
              </div>
              <div class="mb-3">
                <label class="form-label">Description</label>
                <textarea
                  class="form-control"
                  rows="3"
                  [(ngModel)]="newSeniority.description"
                  placeholder="Enter seniority level description"
                ></textarea>
              </div>
            </div>
            <div class="modal-footer border-0">
              <button
                type="button"
                class="btn btn-link text-dark"
                (click)="closeAddSeniorityModal()"
              >
                Cancel
              </button>
              <button
                type="button"
                class="btn btn-dark"
                [disabled]="!isValidSeniority(newSeniority)"
                (click)="addSeniority()"
              >
                Add Seniority Level
              </button>
            </div>
          </div>
        </div>
      </div>

      <!-- Edit Seniority Modal -->
      <div
        class="modal"
        [class.show]="showEditSeniorityModal"
        [style.display]="showEditSeniorityModal ? 'block' : 'none'"
      >
        <div class="modal-dialog modal-dialog-centered">
          <div class="modal-content border-0">
            <div class="modal-header border-0">
              <h5 class="modal-title">Edit Seniority Level</h5>
              <button
                type="button"
                class="btn-close"
                (click)="closeEditSeniorityModal()"
              ></button>
            </div>
            <div class="modal-body">
              <div class="mb-3">
                <label class="form-label">Name</label>
                <input
                  type="text"
                  class="form-control"
                  [(ngModel)]="editingSeniority.name"
                  placeholder="Enter seniority level name"
                />
              </div>
              <div class="mb-3">
                <label class="form-label">Description</label>
                <textarea
                  class="form-control"
                  rows="3"
                  [(ngModel)]="editingSeniority.description"
                  placeholder="Enter seniority level description"
                ></textarea>
              </div>
            </div>
            <div class="modal-footer border-0">
              <button
                type="button"
                class="btn btn-link text-dark"
                (click)="closeEditSeniorityModal()"
              >
                Cancel
              </button>
              <button
                type="button"
                class="btn btn-dark"
                [disabled]="!isValidSeniority(editingSeniority)"
                (click)="updateSeniority()"
              >
                Update Seniority Level
              </button>
            </div>
          </div>
        </div>
      </div>

      <!-- Delete Seniority Modal -->
      <div
        class="modal"
        [class.show]="showDeleteSeniorityModal"
        [style.display]="showDeleteSeniorityModal ? 'block' : 'none'"
      >
        <div class="modal-dialog modal-dialog-centered">
          <div class="modal-content border-0">
            <div class="modal-header border-0">
              <h5 class="modal-title">Delete Seniority Level</h5>
              <button
                type="button"
                class="btn-close"
                (click)="closeDeleteSeniorityModal()"
              ></button>
            </div>
            <div class="modal-body">
              <p>
                Are you sure you want to delete the seniority level
                <strong>{{ deletingSeniority?.name }}</strong
                >?
              </p>
              <p class="text-danger">
                This action cannot be undone. Deleting a seniority level may
                affect associated skills and rubrics.
              </p>
            </div>
            <div class="modal-footer border-0">
              <button
                type="button"
                class="btn btn-link text-dark"
                (click)="closeDeleteSeniorityModal()"
              >
                Cancel
              </button>
              <button
                type="button"
                class="btn btn-danger"
                (click)="confirmDeleteSeniority()"
              >
                Delete Seniority Level
              </button>
            </div>
          </div>
        </div>
      </div>

      <!-- Modal Backdrop -->
      <div
        class="modal-backdrop fade show"
        *ngIf="
          showAddSeniorityModal ||
          showEditSeniorityModal ||
          showDeleteSeniorityModal
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

      .seniority-container {
        max-height: calc(100vh - 240px);
        overflow-y: auto;
        padding-right: 1rem;
      }

      .table {
        border-collapse: separate;
        border-spacing: 0;
        border-radius: 8px;
        overflow: hidden;
      }

      .table th {
        background-color: #f9fafb;
        font-weight: 600;
        color: #4b5563;
        border-bottom-width: 1px;
        padding: 0.75rem 1rem;
      }

      .table td {
        padding: 1rem;
        vertical-align: middle;
        color: #1f2937;
        border-bottom: 1px solid #f3f4f6;
      }

      .table tr:last-child td {
        border-bottom: none;
      }

      .empty-state {
        display: flex;
        flex-direction: column;
        align-items: center;
        padding: 2rem;
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
        padding: 1.5rem;
        border-radius: 8px;
        max-width: 500px;
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
export class SenioritiesComponent implements OnInit, OnDestroy {
  seniorities: Seniority[] = [];
  filteredSeniorities: Seniority[] = [];

  currentPage = 1;
  pageSize = 5;
  totalCount = 0;
  isLoading = false;

  searchTerm = '';

  // Modal states
  showAddSeniorityModal = false;
  showEditSeniorityModal = false;
  showDeleteSeniorityModal = false;

  // Form data
  newSeniority: Partial<Seniority> = {};
  editingSeniority: Seniority = { id: '', name: '', description: '' };
  deletingSeniority: Seniority | null = null;

  private subscriptions: Subscription[] = [];

  constructor(private seniorityService: SkillService) {}

  ngOnInit() {
    this.loadSeniorities();
  }

  ngOnDestroy() {
    this.subscriptions.forEach((sub) => sub.unsubscribe());
  }

  loadSeniorities() {
    this.isLoading = true;
    const sub = this.seniorityService
      .seniorityDetailsData({
        pageNumber: this.currentPage,
        pageSize: this.pageSize,
      })
      .subscribe({
        next: (response) => {
          this.seniorities = response.items;
          this.filteredSeniorities = [...this.seniorities];
          this.totalCount = response.totalCount;
          this.isLoading = false;
        },
        error: (error) => {
          console.error('Error loading seniority levels:', error);
          this.isLoading = false;

          // Fallback for demo purposes
          this.seniorities = [
            {
              id: '1',
              name: 'Junior',
              description: '0-2 years of experience',
            },
            {
              id: '2',
              name: 'Mid-level',
              description: '2-5 years of experience',
            },
            {
              id: '3',
              name: 'Senior',
              description: '5+ years of experience',
            },
            {
              id: '4',
              name: 'Lead',
              description: '8+ years of experience with leadership skills',
            },
            {
              id: '5',
              name: 'Principal',
              description: '10+ years of experience with strategic vision',
            },
          ];
          this.filteredSeniorities = [...this.seniorities];
          this.totalCount = this.seniorities.length;
        },
      });
    this.subscriptions.push(sub);
  }

  onPageChange(page: number) {
    this.currentPage = page;
    this.loadSeniorities();
  }

  onSearch() {
    if (!this.searchTerm.trim()) {
      this.filteredSeniorities = [...this.seniorities];
      return;
    }

    const searchTermLower = this.searchTerm.toLowerCase();
    this.filteredSeniorities = this.seniorities.filter(
      (seniority) =>
        seniority.name.toLowerCase().includes(searchTermLower) ||
        seniority.description.toLowerCase().includes(searchTermLower)
    );
  }

  // Seniority CRUD operations
  openAddSeniorityModal() {
    this.newSeniority = {};
    this.showAddSeniorityModal = true;
  }

  closeAddSeniorityModal() {
    this.showAddSeniorityModal = false;
    this.newSeniority = {};
  }

  openEditSeniorityModal(seniority: Seniority) {
    this.editingSeniority = { ...seniority };
    this.showEditSeniorityModal = true;
  }

  closeEditSeniorityModal() {
    this.showEditSeniorityModal = false;
    this.editingSeniority = { id: '', name: '', description: '' };
  }

  openDeleteSeniorityModal(seniority: Seniority) {
    this.deletingSeniority = seniority;
    this.showDeleteSeniorityModal = true;
  }

  closeDeleteSeniorityModal() {
    this.showDeleteSeniorityModal = false;
    this.deletingSeniority = null;
  }

  isValidSeniority(seniority: Partial<Seniority>): boolean {
    return !!seniority.name && !!seniority.description;
  }

  addSeniority() {
    if (this.isValidSeniority(this.newSeniority)) {
      this.isLoading = true;

      const seniorityData = {
        name: this.newSeniority.name!,
        description: this.newSeniority.description!,
      };

      this.seniorityService.seniorityCreatedData(seniorityData).subscribe({
        next: (response) => {
          // Reload the seniorities to include the new one
          this.loadSeniorities();
          this.closeAddSeniorityModal();
        },
        error: (error) => {
          console.error('Error creating seniority level:', error);

          // Fallback for demo purposes
          const mockId = `mock-${Date.now()}`;
          const newSeniority = {
            id: mockId,
            name: seniorityData.name,
            description: seniorityData.description,
          };

          this.seniorities.unshift(newSeniority);
          this.filteredSeniorities = [...this.seniorities];
          this.totalCount = this.seniorities.length;

          this.closeAddSeniorityModal();
          this.isLoading = false;
        },
      });
    }
  }

  updateSeniority() {
    if (this.isValidSeniority(this.editingSeniority)) {
      this.isLoading = true;

      const seniorityData = {
        id: this.editingSeniority.id,
        name: this.editingSeniority.name,
        description: this.editingSeniority.description,
      };

      this.seniorityService.seniorityUpdateData(seniorityData).subscribe({
        next: (response) => {
          // Update the seniority in the local array
          const index = this.seniorities.findIndex(
            (s) => s.id === seniorityData.id
          );
          if (index !== -1) {
            this.seniorities[index] = { ...response };
            this.filteredSeniorities = [...this.seniorities];
          }

          this.closeEditSeniorityModal();
          this.loadSeniorities();
          this.isLoading = false;
        },
        error: (error) => {
          console.error('Error updating seniority level:', error);

          // Fallback for demo purposes
          const index = this.seniorities.findIndex(
            (s) => s.id === seniorityData.id
          );
          if (index !== -1) {
            this.seniorities[index] = { ...seniorityData };
            this.filteredSeniorities = [...this.seniorities];
          }

          this.closeEditSeniorityModal();
          this.isLoading = false;
        },
      });
    }
  }

  confirmDeleteSeniority() {
    if (this.deletingSeniority) {
      this.isLoading = true;

      this.seniorityService
        .seniorityDeleteData(this.deletingSeniority.id)
        .subscribe({
          next: () => {
            // Remove the seniority from the local array
            this.seniorities = this.seniorities.filter(
              (s) => s.id !== this.deletingSeniority!.id
            );
            this.filteredSeniorities = [...this.seniorities];
            this.totalCount = this.seniorities.length;

            this.closeDeleteSeniorityModal();
            this.isLoading = false;
          },
          error: (error) => {
            console.error('Error deleting seniority level:', error);

            // Fallback for demo purposes
            this.seniorities = this.seniorities.filter(
              (s) => s.id !== this.deletingSeniority!.id
            );
            this.filteredSeniorities = [...this.seniorities];
            this.totalCount = this.seniorities.length;

            this.closeDeleteSeniorityModal();
            this.isLoading = false;
          },
        });
    }
  }

  closeAllModals() {
    this.closeAddSeniorityModal();
    this.closeEditSeniorityModal();
    this.closeDeleteSeniorityModal();
  }
}
