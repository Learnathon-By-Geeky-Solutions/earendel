// Add proper import at the top
import { FormsModule } from '@angular/forms';
import { Component, type OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SkillService } from '../../admin-dashboard/services/skill.service';
import { Subscription } from 'rxjs';

interface Notification {
  id: number;
  message: string;
  timestamp: Date;
  isRead: boolean;
}

@Component({
  selector: 'app-notifications',
  standalone: true,
  imports: [CommonModule, FormsModule],
  template: `
    <div class="notifications-container">
      <h2>Notifications</h2>
      <div class="notifications-list">
        <div
          *ngFor="let notification of notifications"
          class="notification-item"
          [class.unread]="!notification.isRead"
          (click)="markAsRead(notification)"
        >
          <div class="notification-content">
            <p>{{ notification.message }}</p>
            <span class="timestamp">{{
              notification.timestamp | date : 'medium'
            }}</span>
          </div>
        </div>
      </div>
      <div *ngIf="notifications.length === 0" class="no-notifications">
        <i class="bi bi-bell-slash"></i>
        <p>No notifications at the moment</p>
      </div>
      <!-- Add this to the template -->
      <div class="pagination-controls" *ngIf="totalCount > 0">
        <div class="pagination-info">
          Showing {{ (currentPage - 1) * pageSize + 1 }} -
          {{ Math.min(currentPage * pageSize, totalCount) }} of
          {{ totalCount }} notifications
        </div>

        <div class="pagination-buttons">
          <select
            [(ngModel)]="pageSize"
            (change)="onPageSizeChange()"
            class="form-select"
          >
            <option value="5">5 per page</option>
            <option value="10">10 per page</option>
            <option value="20">20 per page</option>
            <option value="50">50 per page</option>
          </select>

          <nav aria-label="Page navigation">
            <ul class="pagination">
              <li class="page-item" [class.disabled]="!hasPrevious">
                <button class="page-link" (click)="previousPage()">
                  Previous
                </button>
              </li>

              <li
                class="page-item"
                *ngFor="let page of getPages()"
                [class.active]="page === currentPage"
              >
                <button class="page-link" (click)="goToPage(page)">
                  {{ page }}
                </button>
              </li>

              <li class="page-item" [class.disabled]="!hasNext">
                <button class="page-link" (click)="nextPage()">Next</button>
              </li>
            </ul>
          </nav>
        </div>
      </div>
    </div>
  `,
  styles: [
    `
      .notifications-container {
        max-width: 800px;
        margin: 0 auto;
      }

      h2 {
        font-size: 24px;
        font-weight: 600;
        margin-bottom: 24px;
        color: #333;
      }

      .notifications-list {
        display: flex;
        flex-direction: column;
        gap: 16px;
      }

      .notification-item {
        background-color: white;
        border-radius: 8px;
        padding: 16px;
        box-shadow: 0 2px 4px rgba(0, 0, 0, 0.1);
        display: flex;
        justify-content: space-between;
        align-items: center;
        transition: all 0.2s ease;
        cursor: pointer;

        &:hover {
          box-shadow: 0 4px 8px rgba(0, 0, 0, 0.1);
        }

        &.unread {
          border-left: 4px solid #0066ff;
          background-color: #f0f7ff;

          p {
            font-weight: 600;
          }
        }
      }

      .notification-content {
        flex: 1;

        p {
          margin: 0 0 8px;
          color: #333;
          font-size: 14px;
          line-height: 1.5;
        }

        .timestamp {
          font-size: 12px;
          color: #666;
        }
      }

      .notification-actions {
        display: flex;
        align-items: center;
      }

      .delete-btn {
        background: none;
        border: none;
        color: #dc3545;
        cursor: pointer;
        font-size: 18px;
        padding: 4px;
        transition: all 0.2s ease;

        &:hover {
          color: #bd2130;
        }
      }

      .no-notifications {
        text-align: center;
        padding: 48px 0;
        color: #666;

        i {
          font-size: 48px;
          margin-bottom: 16px;
        }

        p {
          font-size: 18px;
        }
      }

      @media (max-width: 768px) {
        .notifications-container {
          padding: 16px;
        }

        .notification-item {
          flex-direction: column;
          align-items: flex-start;
        }

        .notification-actions {
          margin-top: 12px;
          align-self: flex-end;
        }
      }

      .pagination-controls {
        margin-top: 20px;
        display: flex;
        flex-direction: column;
        gap: 15px;
      }

      .pagination-info {
        font-size: 14px;
        color: #666;
      }

      .pagination-buttons {
        display: flex;
        justify-content: space-between;
        align-items: center;
        gap: 15px;
      }

      .form-select {
        width: auto;
      }

      .pagination {
        margin: 0;
      }

      .page-link {
        cursor: pointer;
      }

      @media (max-width: 768px) {
        .pagination-buttons {
          flex-direction: column;
          align-items: stretch;
        }

        .form-select {
          width: 100%;
        }
      }
    `,
  ],
})
export class CandidateNotificationsComponent implements OnInit {
  public Math = Math;
  notifications: any[] = [];
  isLoading = false;
  error: string | null = null;
  isMarkingRead = false;
  markingReadId: string | null = null;

  // Pagination
  currentPage = 1;
  pageSize = 10;
  totalCount = 0;
  totalPages = 0;
  hasPrevious = false;
  hasNext = false;
  userId = '';

  private subscriptions: Subscription[] = [];

  constructor(private notificationService: SkillService) {}

  ngOnInit() {
    const user = JSON.parse(sessionStorage.getItem('loggedInUser') || '{}');
    this.userId = user.userId;

    this.loadNotifications();
  }

  ngOnDestroy() {
    // Clean up subscriptions to prevent memory leaks
    this.subscriptions.forEach((sub) => sub.unsubscribe());
  }

  // Pagination methods
  previousPage(): void {
    if (this.hasPrevious) {
      this.currentPage--;
      this.loadNotifications();
    }
  }

  nextPage(): void {
    if (this.hasNext) {
      this.currentPage++;
      this.loadNotifications();
    }
  }

  goToPage(page: number): void {
    if (page >= 1 && page <= this.totalPages) {
      this.currentPage = page;
      this.loadNotifications();
    }
  }

  onPageSizeChange(): void {
    this.currentPage = 1; // Reset to first page when changing page size
    this.loadNotifications();
  }

  getPages(): number[] {
    const pages = [];
    const maxVisiblePages = 5;
    let startPage = Math.max(
      1,
      this.currentPage - Math.floor(maxVisiblePages / 2)
    );
    const endPage = Math.min(this.totalPages, startPage + maxVisiblePages - 1);

    if (endPage - startPage < maxVisiblePages - 1) {
      startPage = Math.max(1, endPage - maxVisiblePages + 1);
    }

    for (let i = startPage; i <= endPage; i++) {
      pages.push(i);
    }
    return pages;
  }

  loadNotifications() {
    this.isLoading = true;
    this.error = null;

    const request = {
      userId: this.userId,
      pageNumber: this.currentPage,
      pageSize: this.pageSize,
    };

    console.log(request);

    const sub = this.notificationService
      .userNotificationDetailsData(request)
      .subscribe({
        next: (response: any) => {
          console.log(response);
          // Process the notifications to add read status and timestamp
          this.notifications = response.items.map((notification: any) => ({
            ...notification,
            read: false, // Default to unread, should be provided by API in real implementation
            timestamp: new Date(), // Default to current time, should be provided by API
          }));

          // Update pagination info
          this.totalCount = response.totalCount;
          this.totalPages = response.totalPages;
          this.hasPrevious = response.hasPrevious;
          this.hasNext = response.hasNext;

          this.isLoading = false;
        },
        error: (err: any) => {
          console.error('Error loading notifications:', err);
          this.error = 'Failed to load notifications. Please try again.';
          this.isLoading = false;
        },
      });

    this.subscriptions.push(sub);
  }

  markAsRead(notification: Notification) {}

  deleteNotification(notification: Notification, event: Event) {}
}
