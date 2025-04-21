import { Component, OnDestroy, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { SidebarComponent } from '../sidebar/sidebar.component';
import { PaginationComponent } from '../pagination/pagination.component';
import {
  Subscription,
  Subject,
  debounceTime,
  distinctUntilChanged,
  filter,
} from 'rxjs';
import { NotificationhubService } from '../../shared/services/signalr/notificationhub.service';
import { HrService } from '../services/hr.service';
import { HttpClientModule } from '@angular/common/http';

interface HRPersonnel {
  id: string;
  name: string;
  email: string;
  department: string;
  activeJobs: number;
}

interface HRNotificationMessage {
  HRs?: Array<{
    Id: string;
    UserName: string;
    Email: string;
    JobCount: number;
    Jobs: any[];
  }>;
  TotalRecords?: number;
  RequestedBy?: string;
}

@Component({
  selector: 'app-hr',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    SidebarComponent,
    PaginationComponent,
    HttpClientModule,
  ],
  template: `
    <div class="d-flex">
      <app-sidebar></app-sidebar>

      <main class="main-content bg-light">
        <div class="p-4">
          <!-- Loading Overlay -->
          <div class="loading-overlay" *ngIf="loading">
            <div class="spinner-border text-primary" role="status">
              <span class="visually-hidden">Loading...</span>
            </div>
          </div>

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
                  (input)="onSearchInput()"
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
                  <h2 class="mb-0">{{ totalRecords }}</h2>
                </div>
              </div>
            </div>
            <div class="col-md-3">
              <div class="card border-0 shadow-sm">
                <div class="card-body">
                  <h6 class="text-muted mb-2">Average Active Jobs</h6>
                  <h2 class="mb-0">
                    {{ averageActiveJobs | number : '1.1-1' }}
                  </h2>
                </div>
              </div>
            </div>
            <div class="col-md-3">
              <div class="card border-0 shadow-sm">
                <div class="card-body">
                  <h6 class="text-muted mb-2">Max Active Jobs</h6>
                  <h2 class="mb-0">{{ maxActiveJobs }}</h2>
                </div>
              </div>
            </div>
            <div class="col-md-3">
              <div class="card border-0 shadow-sm">
                <div class="card-body">
                  <h6 class="text-muted mb-2">Min Active Jobs</h6>
                  <h2 class="mb-0">{{ minActiveJobs }}</h2>
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
                    <th
                      class="border-0 px-4 py-3"
                      (click)="toggleSort('UserName')"
                    >
                      <div class="d-flex align-items-center gap-2">
                        Name
                        <div class="sort-arrows">
                          <i class="bi {{ getSortIcon('UserName') }}"></i>
                        </div>
                      </div>
                    </th>
                    <th
                      class="border-0 px-4 py-3"
                      (click)="toggleSort('Email')"
                    >
                      <div class="d-flex align-items-center gap-2">
                        Email
                        <div class="sort-arrows">
                          <i class="bi {{ getSortIcon('Email') }}"></i>
                        </div>
                      </div>
                    </th>
                    <th class="border-0 px-4 py-3">Department</th>
                    <th class="border-0 px-4 py-3">Active Jobs</th>
                    <th class="border-0 px-4 py-3 text-end">Actions</th>
                  </tr>
                </thead>
                <tbody>
                  <tr *ngFor="let person of personnel">
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
                  <tr *ngIf="personnel.length === 0 && !loading">
                    <td colspan="5" class="text-center py-4">
                      No HR personnel found
                    </td>
                  </tr>
                </tbody>
              </table>

              <app-pagination
                [currentPage]="currentPage"
                [pageSize]="pageSize"
                [totalItems]="totalRecords"
                (pageChange)="onPageChange($event)"
              ></app-pagination>
            </div>
          </div>
        </div>
      </main>

      <!-- Modals (keep existing modal templates) -->
      <!-- ... -->
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

      th {
        cursor: pointer;
      }

      .loading-overlay {
        position: fixed;
        top: 0;
        left: 0;
        width: 100%;
        height: 100%;
        background: rgba(255, 255, 255, 0.8);
        display: flex;
        align-items: center;
        justify-content: center;
        z-index: 1000;
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
export class HrComponent implements OnInit, OnDestroy {
  personnel: HRPersonnel[] = [];
  totalRecords = 0;
  currentPage = 1;
  pageSize = 6;
  searchQuery = '';
  sortBy: any = null;
  sortDirection: any = null;
  loading = false;

  // Modals and form data (keep existing)
  showAddModal = false;
  showEditModal = false;
  showRemoveModal = false;
  newPerson: Partial<HRPersonnel> = {};
  editingPerson: Partial<HRPersonnel> = {};
  selectedPerson: HRPersonnel | null = null;

  private searchSubject = new Subject<string>();
  private subscriptions: Subscription[] = [];

  constructor(
    private notificationHubService: NotificationhubService,
    private hrService: HrService
  ) {
    this.searchSubject
      .pipe(debounceTime(300), distinctUntilChanged())
      .subscribe(() => this.loadHRData());
  }

  ngOnInit(): void {
    const user = JSON.parse(sessionStorage.getItem('loggedInUser') || '{}');
    this.notificationHubService.startConnection(user?.token);

    const connectionSub = this.notificationHubService.connectionEstablished$
      .pipe(filter((connected) => connected))
      .subscribe(() => this.loadHRData());

    const notificationSub =
      this.notificationHubService.userNotifications$.subscribe(
        (message: unknown) => {
          const parsed = this.parseNotification(message);
          if (parsed?.HRs) this.processHRData(parsed);
        }
      );

    this.subscriptions.push(connectionSub, notificationSub);
  }

  private parseNotification(message: unknown): HRNotificationMessage | null {
    try {
      if (typeof message === 'string') {
        return JSON.parse(message) as HRNotificationMessage;
      }
      return message as HRNotificationMessage;
    } catch (error) {
      console.error('Error parsing notification:', error);
      return null;
    }
  }

  ngOnDestroy(): void {
    this.subscriptions.forEach((sub) => sub.unsubscribe());
    this.notificationHubService.stopConnection();
  }

  get averageActiveJobs(): number {
    if (!this.personnel.length) return 0;
    return (
      this.personnel.reduce((sum, person) => sum + person.activeJobs, 0) /
      this.personnel.length
    );
  }

  get maxActiveJobs(): number {
    return Math.max(...this.personnel.map((p) => p.activeJobs), 0);
  }

  get minActiveJobs(): number {
    return Math.min(...this.personnel.map((p) => p.activeJobs), Infinity) || 0;
  }

  loadHRData(): void {
    this.loading = true;
    this.hrService
      .hrDetailsData(
        this.currentPage,
        this.pageSize,
        this.searchQuery,
        this.sortBy, // Convert null to undefined
        this.sortDirection
      )
      .subscribe({
        next: (response: unknown) => {
          const data = response as HRNotificationMessage;
          this.processHRData(data);
          this.loading = false;
        },
        error: (error) => {
          console.error('Error loading HR data:', error);
          this.loading = false;
        },
      });
  }

  private processHRData(data: HRNotificationMessage): void {
    if (!data?.HRs) return;

    this.personnel = data.HRs.map((hr) => ({
      id: hr.Id,
      name: hr.UserName,
      email: hr.Email,
      department: 'HR',
      activeJobs: hr.JobCount,
    }));

    this.totalRecords = data.TotalRecords ?? 0;
  }

  onSearchInput(): void {
    this.currentPage = 1;
    this.searchSubject.next(this.searchQuery);
  }

  toggleSort(field: string): void {
    if (this.sortBy === field) {
      this.sortDirection = this.sortDirection === 'asc' ? 'desc' : 'asc';
    } else {
      this.sortBy = field;
      this.sortDirection = 'asc';
    }
    this.currentPage = 1;
    this.loadHRData();
  }

  getSortIcon(field: string): string {
    if (this.sortBy !== field) return 'bi-chevron-expand';
    return this.sortDirection === 'asc' ? 'bi-chevron-up' : 'bi-chevron-down';
  }

  onPageChange(page: number): void {
    this.currentPage = page;
    this.loadHRData();
  }

  // Existing modal methods (keep implementation same)
  openAddModal(): void {
    /* ... */
  }
  openEditModal(person: HRPersonnel): void {
    /* ... */
  }
  openRemoveModal(person: HRPersonnel): void {
    /* ... */
  }
  closeAddModal(): void {
    /* ... */
  }
  closeEditModal(): void {
    /* ... */
  }
  closeRemoveModal(): void {
    /* ... */
  }
  closeAllModals(): void {
    /* ... */
  }
  addPerson(): void {
    /* ... */
  }
  saveChanges(): void {
    /* ... */
  }
  removePerson(): void {
    /* ... */
  }
  public isValidPerson(person: Partial<HRPersonnel>): boolean {
    return !!(person.name && person.email && person.department);
  }
}
