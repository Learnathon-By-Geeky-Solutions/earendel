import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { SidebarComponent } from '../sidebar/sidebar.component';
import { PaginationComponent } from '../pagination/pagination.component';

interface HRPersonnel {
  id: number;
  name: string;
  email: string;
  department: string;
  activeJobs: number;
}

@Component({
  selector: 'app-hr',
  standalone: true,
  imports: [CommonModule, FormsModule, SidebarComponent, PaginationComponent],
  template: `
    <div class="d-flex">
      <app-sidebar></app-sidebar>

      <main class="main-content bg-light">
        <div class="p-4">
          <!-- Header -->
          <div class="d-flex justify-content-between align-items-center mb-4">
            <h1 class="h4 mb-0">HR Personnel</h1>
            <div class="d-flex gap-3 align-items-center">
              <div class="search-box">
                <input
                  type="text"
                  class="form-control"
                  placeholder="Search HR personnel"
                  [(ngModel)]="searchQuery"
                />
              </div>
              <div class="dropdown">
                <button
                  class="btn btn-outline-secondary dropdown-toggle"
                  type="button"
                  data-bs-toggle="dropdown"
                >
                  <i class="bi bi-funnel me-2"></i>
                  Filter
                </button>
                <ul class="dropdown-menu">
                  <li><a class="dropdown-item" href="#">Department</a></li>
                  <li><a class="dropdown-item" href="#">Active Jobs</a></li>
                </ul>
              </div>
              <button class="btn btn-dark" (click)="openAddModal()">
                <i class="bi bi-plus me-2"></i>
                Add HR Personnel
              </button>
            </div>
          </div>

          <!-- Stats Cards -->
          <div class="row g-4 mb-4">
            <div class="col-md-3">
              <div class="card border-0 shadow-sm">
                <div class="card-body">
                  <h6 class="text-muted mb-2">Total HR Personnel</h6>
                  <h2 class="mb-0">4</h2>
                </div>
              </div>
            </div>
            <div class="col-md-3">
              <div class="card border-0 shadow-sm">
                <div class="card-body">
                  <h6 class="text-muted mb-2">Average Active Jobs</h6>
                  <h2 class="mb-0">8.75</h2>
                </div>
              </div>
            </div>
            <div class="col-md-3">
              <div class="card border-0 shadow-sm">
                <div class="card-body">
                  <h6 class="text-muted mb-2">Max Active Jobs</h6>
                  <h2 class="mb-0">12</h2>
                </div>
              </div>
            </div>
            <div class="col-md-3">
              <div class="card border-0 shadow-sm">
                <div class="card-body">
                  <h6 class="text-muted mb-2">Min Active Jobs</h6>
                  <h2 class="mb-0">5</h2>
                </div>
              </div>
            </div>
          </div>

          <!-- HR Personnel Table -->
          <div class="card border-0 shadow-sm">
            <div class="table-responsive">
              <table class="table table-hover mb-0">
                <thead class="bg-light">
                  <tr>
                    <th class="border-0 px-4 py-3">
                      <div class="d-flex align-items-center gap-2">
                        Name
                        <div class="sort-arrows">
                          <i class="bi bi-chevron-up"></i>
                          <i class="bi bi-chevron-down"></i>
                        </div>
                      </div>
                    </th>
                    <th class="border-0 px-4 py-3">
                      <div class="d-flex align-items-center gap-2">
                        Email
                        <div class="sort-arrows">
                          <i class="bi bi-chevron-up"></i>
                          <i class="bi bi-chevron-down"></i>
                        </div>
                      </div>
                    </th>
                    <th class="border-0 px-4 py-3">
                      <div class="d-flex align-items-center gap-2">
                        Department
                        <div class="sort-arrows">
                          <i class="bi bi-chevron-up"></i>
                          <i class="bi bi-chevron-down"></i>
                        </div>
                      </div>
                    </th>
                    <th class="border-0 px-4 py-3">
                      <div class="d-flex align-items-center gap-2">
                        Active Jobs
                        <div class="sort-arrows">
                          <i class="bi bi-chevron-up"></i>
                          <i class="bi bi-chevron-down"></i>
                        </div>
                      </div>
                    </th>
                    <th class="border-0 px-4 py-3 text-end">Actions</th>
                  </tr>
                </thead>
                <tbody>
                  <tr *ngFor="let person of paginatedPersonnel">
                    <td class="px-4 py-3">{{ person.name }}</td>
                    <td class="px-4 py-3">{{ person.email }}</td>
                    <td class="px-4 py-3">{{ person.department }}</td>
                    <td class="px-4 py-3">{{ person.activeJobs }}</td>
                    <td class="px-4 py-3 text-end">
                      <div class="d-flex gap-3 justify-content-end">
                        <button
                          class="btn btn-link p-0 text-decoration-none"
                          (click)="openEditModal(person)"
                        >
                          Edit
                        </button>
                        <button
                          class="btn btn-link p-0 text-decoration-none"
                          (click)="openRemoveModal(person)"
                        >
                          Remove
                        </button>
                      </div>
                    </td>
                  </tr>
                </tbody>
              </table>

              <app-pagination
                [currentPage]="currentPage"
                [pageSize]="pageSize"
                [totalItems]="filteredPersonnel.length"
                (pageChange)="onPageChange($event)"
              ></app-pagination>
            </div>
          </div>
        </div>
      </main>

      <!-- Add HR Personnel Modal -->
      <div
        class="modal"
        [class.show]="showAddModal"
        [style.display]="showAddModal ? 'block' : 'none'"
      >
        <div class="modal-dialog modal-dialog-centered">
          <div class="modal-content border-0">
            <div class="modal-header border-0">
              <h5 class="modal-title">Add HR Personnel</h5>
              <button
                type="button"
                class="btn-close"
                (click)="closeAddModal()"
              ></button>
            </div>
            <div class="modal-body">
              <div class="mb-3">
                <label class="form-label">Name</label>
                <input
                  type="text"
                  class="form-control"
                  [(ngModel)]="newPerson.name"
                />
              </div>

              <div class="mb-3">
                <label class="form-label">Email</label>
                <input
                  type="email"
                  class="form-control"
                  [(ngModel)]="newPerson.email"
                />
              </div>

              <div class="mb-3">
                <label class="form-label">Department</label>
                <select class="form-select" [(ngModel)]="newPerson.department">
                  <option value="Tech">Tech</option>
                  <option value="Finance">Finance</option>
                  <option value="Marketing">Marketing</option>
                  <option value="Sales">Sales</option>
                </select>
              </div>
            </div>
            <div class="modal-footer border-0">
              <button
                type="button"
                class="btn btn-link text-dark"
                (click)="closeAddModal()"
              >
                Cancel
              </button>
              <button
                type="button"
                class="btn btn-dark"
                [disabled]="!isValidPerson(newPerson)"
                (click)="addPerson()"
              >
                Add Personnel
              </button>
            </div>
          </div>
        </div>
      </div>

      <!-- Edit HR Personnel Modal -->
      <div
        class="modal"
        [class.show]="showEditModal"
        [style.display]="showEditModal ? 'block' : 'none'"
      >
        <div class="modal-dialog modal-dialog-centered">
          <div class="modal-content border-0">
            <div class="modal-header border-0">
              <h5 class="modal-title">Edit HR Personnel</h5>
              <button
                type="button"
                class="btn-close"
                (click)="closeEditModal()"
              ></button>
            </div>
            <div class="modal-body">
              <div class="mb-3">
                <label class="form-label">Name</label>
                <input
                  type="text"
                  class="form-control"
                  [(ngModel)]="editingPerson.name"
                />
              </div>

              <div class="mb-3">
                <label class="form-label">Email</label>
                <input
                  type="email"
                  class="form-control"
                  [(ngModel)]="editingPerson.email"
                />
              </div>

              <div class="mb-3">
                <label class="form-label">Department</label>
                <select
                  class="form-select"
                  [(ngModel)]="editingPerson.department"
                >
                  <option value="Tech">Tech</option>
                  <option value="Finance">Finance</option>
                  <option value="Marketing">Marketing</option>
                  <option value="Sales">Sales</option>
                </select>
              </div>

              <div class="mb-3">
                <label class="form-label">Active Jobs</label>
                <input
                  type="number"
                  class="form-control"
                  [(ngModel)]="editingPerson.activeJobs"
                />
              </div>
            </div>
            <div class="modal-footer border-0">
              <button
                type="button"
                class="btn btn-link text-dark"
                (click)="closeEditModal()"
              >
                Cancel
              </button>
              <button
                type="button"
                class="btn btn-dark"
                [disabled]="!isValidPerson(editingPerson)"
                (click)="saveChanges()"
              >
                Save Changes
              </button>
            </div>
          </div>
        </div>
      </div>

      <!-- Remove HR Personnel Modal -->
      <div
        class="modal"
        [class.show]="showRemoveModal"
        [style.display]="showRemoveModal ? 'block' : 'none'"
      >
        <div class="modal-dialog modal-dialog-centered">
          <div class="modal-content border-0">
            <div class="modal-header border-0">
              <h5 class="modal-title">Remove HR Personnel</h5>
              <button
                type="button"
                class="btn-close"
                (click)="closeRemoveModal()"
              ></button>
            </div>
            <div class="modal-body">
              <p>Are you sure you want to remove {{ selectedPerson?.name }}?</p>
              <p class="text-muted small">This action cannot be undone.</p>
            </div>
            <div class="modal-footer border-0">
              <button
                type="button"
                class="btn btn-link text-dark"
                (click)="closeRemoveModal()"
              >
                Cancel
              </button>
              <button
                type="button"
                class="btn btn-danger"
                (click)="removePerson()"
              >
                Remove
              </button>
            </div>
          </div>
        </div>
      </div>

      <!-- Modal Backdrop -->
      <div
        class="modal-backdrop fade show"
        *ngIf="showAddModal || showEditModal || showRemoveModal"
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

      .search-box {
        width: 300px;
      }

      .card {
        border-radius: 8px;
      }

      .table {
        margin-bottom: 0;
      }

      .table th {
        font-weight: 500;
        font-size: 14px;
        color: #6b7280;
      }

      .table td {
        font-size: 14px;
        vertical-align: middle;
      }

      .sort-arrows {
        display: flex;
        flex-direction: column;
        line-height: 0;
        margin-left: 4px;
      }

      .sort-arrows i {
        font-size: 10px;
        color: #9ca3af;
        cursor: pointer;
      }

      .sort-arrows i.active {
        color: #111827;
      }

      th {
        cursor: pointer;
      }

      th:hover .sort-arrows i {
        color: #6b7280;
      }

      .btn-link {
        font-size: 14px;
        color: #2563eb;
      }

      .btn-link:hover {
        color: #1d4ed8;
      }

      .modal-content {
        border-radius: 12px;
      }

      .form-control,
      .form-select {
        border-radius: 6px;
        padding: 0.5rem 0.75rem;
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
export class HRComponent {
  // Pagination
  currentPage = 1;
  pageSize = 10;
  searchQuery = '';

  // Modal states
  showAddModal = false;
  showEditModal = false;
  showRemoveModal = false;

  // Form data
  newPerson: Partial<HRPersonnel> = {};
  editingPerson: Partial<HRPersonnel> = {};
  selectedPerson: HRPersonnel | null = null;

  personnel: HRPersonnel[] = [
    {
      id: 1,
      name: 'Emily Davis',
      email: 'emily@example.com',
      department: 'Tech',
      activeJobs: 10,
    },
    {
      id: 2,
      name: 'Michael Wilson',
      email: 'michael@example.com',
      department: 'Finance',
      activeJobs: 5,
    },
    {
      id: 3,
      name: 'Sarah Thompson',
      email: 'sarah@example.com',
      department: 'Marketing',
      activeJobs: 8,
    },
    {
      id: 4,
      name: 'David Lee',
      email: 'david@example.com',
      department: 'Sales',
      activeJobs: 12,
    },
  ];

  get filteredPersonnel(): HRPersonnel[] {
    return this.personnel.filter(
      (person) =>
        person.name.toLowerCase().includes(this.searchQuery.toLowerCase()) ||
        person.email.toLowerCase().includes(this.searchQuery.toLowerCase()) ||
        person.department.toLowerCase().includes(this.searchQuery.toLowerCase())
    );
  }

  get paginatedPersonnel(): HRPersonnel[] {
    const start = (this.currentPage - 1) * this.pageSize;
    const end = start + this.pageSize;
    return this.filteredPersonnel.slice(start, end);
  }

  onPageChange(page: number): void {
    this.currentPage = page;
  }

  // Modal handlers
  openAddModal(): void {
    this.newPerson = {};
    this.showAddModal = true;
  }

  openEditModal(person: HRPersonnel): void {
    this.editingPerson = { ...person };
    this.showEditModal = true;
  }

  openRemoveModal(person: HRPersonnel): void {
    this.selectedPerson = person;
    this.showRemoveModal = true;
  }

  closeAddModal(): void {
    this.showAddModal = false;
    this.newPerson = {};
  }

  closeEditModal(): void {
    this.showEditModal = false;
    this.editingPerson = {};
  }

  closeRemoveModal(): void {
    this.showRemoveModal = false;
    this.selectedPerson = null;
  }

  closeAllModals(): void {
    this.closeAddModal();
    this.closeEditModal();
    this.closeRemoveModal();
  }

  // CRUD operations
  addPerson(): void {
    if (this.isValidPerson(this.newPerson)) {
      const newId = Math.max(...this.personnel.map((p) => p.id)) + 1;
      const person: HRPersonnel = {
        id: newId,
        name: this.newPerson.name!,
        email: this.newPerson.email!,
        department: this.newPerson.department!,
        activeJobs: 0,
      };
      this.personnel.push(person);
      this.closeAddModal();
    }
  }

  saveChanges(): void {
    if (this.isValidPerson(this.editingPerson)) {
      const index = this.personnel.findIndex(
        (p) => p.id === this.editingPerson.id
      );
      if (index !== -1) {
        this.personnel[index] = this.editingPerson as HRPersonnel;
        this.closeEditModal();
      }
    }
  }

  removePerson(): void {
    if (this.selectedPerson) {
      this.personnel = this.personnel.filter(
        (p) => p.id !== this.selectedPerson!.id
      );
      this.closeRemoveModal();
    }
  }

  public isValidPerson(person: Partial<HRPersonnel>): boolean {
    return !!(person.name && person.email && person.department);
  }
}
