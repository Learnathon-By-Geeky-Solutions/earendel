// import { Component, type OnInit } from '@angular/core';
// import { CommonModule } from '@angular/common';
// import { SidebarComponent } from '../sidebar/sidebar.component';

// interface Notification {
//   id: number;
//   message: string;
//   timestamp: Date;
//   read: boolean;
// }

// @Component({
//   selector: 'app-notifications',
//   standalone: true,
//   imports: [CommonModule, SidebarComponent],
//   template: `
//     <div class="d-flex">
//       <app-sidebar></app-sidebar>

//       <main class="main-content bg-light">
//         <div class="container py-4">
//           <h1 class="h3 mb-4">Notifications</h1>

//           <div class="card border-0 shadow-sm">
//             <div class="card-body p-0">
//               <ul class="list-group list-group-flush">
//                 <li
//                   *ngFor="let notification of notifications"
//                   class="list-group-item d-flex justify-content-between align-items-center p-3"
//                   [class.fw-bold]="!notification.read"
//                   (click)="markAsRead(notification)"
//                 >
//                   <div class="d-flex align-items-center">
//                     <div
//                       class="notification-icon me-3"
//                       [class.unread]="!notification.read"
//                     >
//                       <i class="bi bi-bell-fill"></i>
//                     </div>
//                     <div>
//                       <p class="mb-0">{{ notification.message }}</p>
//                       <small class="text-muted">{{
//                         notification.timestamp | date : 'medium'
//                       }}</small>
//                     </div>
//                   </div>
//                   <button
//                     *ngIf="!notification.read"
//                     class="btn btn-sm btn-outline-primary"
//                     (click)="markAsRead(notification); $event.stopPropagation()"
//                   >
//                     Mark as Read
//                   </button>
//                 </li>
//               </ul>
//             </div>
//           </div>
//         </div>
//       </main>
//     </div>
//   `,
//   styles: [
//     `
//       .main-content {
//         margin-left: 240px;
//         width: calc(100% - 240px);
//         min-height: 100vh;
//       }

//       .notification-icon {
//         width: 40px;
//         height: 40px;
//         border-radius: 50%;
//         background-color: #e9ecef;
//         display: flex;
//         align-items: center;
//         justify-content: center;
//         font-size: 1.2rem;
//         color: #6c757d;
//         transition: all 0.3s ease;
//       }

//       .notification-icon.unread {
//         background-color: #cfe2ff;
//         color: #0d6efd;
//       }

//       .list-group-item {
//         transition: background-color 0.3s ease;
//         cursor: pointer;
//       }

//       .list-group-item:hover {
//         background-color: #f8f9fa;
//       }

//       @media (max-width: 991.98px) {
//         .main-content {
//           margin-left: 0;
//           width: 100%;
//         }
//       }
//     `,
//   ],
// })
// export class NotificationsComponent implements OnInit {
//   notifications: Notification[] = [];

//   ngOnInit() {
//     this.notifications = this.generateMockNotifications();
//   }

//   generateMockNotifications(): Notification[] {
//     return [
//       {
//         id: 1,
//         message: 'New candidate application received',
//         timestamp: new Date(),
//         read: false,
//       },
//       {
//         id: 2,
//         message: 'Interview scheduled for tomorrow',
//         timestamp: new Date(Date.now() - 86400000),
//         read: false,
//       },
//       {
//         id: 3,
//         message: 'Reminder: Complete candidate evaluation',
//         timestamp: new Date(Date.now() - 172800000),
//         read: true,
//       },
//       {
//         id: 4,
//         message: 'New job posting approved',
//         timestamp: new Date(Date.now() - 259200000),
//         read: true,
//       },
//       {
//         id: 5,
//         message: 'Candidate John Doe accepted job offer',
//         timestamp: new Date(Date.now() - 345600000),
//         read: false,
//       },
//     ];
//   }

//   markAsRead(notification: Notification) {
//     notification.read = true;
//   }
// }

import { Component, type OnInit, type OnDestroy } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SidebarComponent } from '../sidebar/sidebar.component';
import type { Subscription } from 'rxjs';
import { SkillService } from '../services/skill.service';

@Component({
  selector: 'app-notifications',
  standalone: true,
  imports: [CommonModule, SidebarComponent],
  template: `
    <div class="d-flex">
      <app-sidebar></app-sidebar>

      <main class="main-content bg-light">
        <div class="container py-4">
          <div class="d-flex justify-content-between align-items-center mb-4">
            <h1 class="h3 mb-0">Notifications</h1>
            <button
              class="btn btn-outline-primary btn-sm"
              (click)="loadNotifications()"
              [disabled]="isLoading"
            >
              <i class="bi bi-arrow-clockwise me-1"></i> Refresh
            </button>
          </div>

          <!-- Loading state -->
          <div *ngIf="isLoading" class="text-center py-5">
            <div class="spinner-border text-primary" role="status">
              <span class="visually-hidden">Loading...</span>
            </div>
            <p class="mt-2 text-muted">Loading notifications...</p>
          </div>

          <!-- Error state -->
          <div *ngIf="error" class="alert alert-danger">
            <i class="bi bi-exclamation-triangle-fill me-2"></i>
            {{ error }}
            <button
              class="btn btn-sm btn-outline-danger ms-2"
              (click)="loadNotifications()"
            >
              Try Again
            </button>
          </div>

          <!-- Empty state -->
          <div
            *ngIf="!isLoading && !error && notifications.length === 0"
            class="text-center py-5"
          >
            <i class="bi bi-bell-slash fs-1 text-muted mb-3"></i>
            <h5>No notifications</h5>
            <p class="text-muted">
              You don't have any notifications at the moment.
            </p>
          </div>

          <!-- Notifications list -->
          <div
            class="card border-0 shadow-sm"
            *ngIf="!isLoading && !error && notifications.length > 0"
          >
            <div class="card-body p-0">
              <ul class="list-group list-group-flush">
                <li
                  *ngFor="let notification of notifications"
                  class="list-group-item d-flex justify-content-between align-items-center p-3"
                  [class.fw-bold]="!notification.read"
                  (click)="markAsRead(notification)"
                >
                  <div class="d-flex align-items-center">
                    <div
                      class="notification-icon me-3"
                      [class.unread]="!notification.read"
                    >
                      <i class="bi bi-bell-fill"></i>
                    </div>
                    <div>
                      <p class="mb-0">{{ notification.message }}</p>
                      <small class="text-muted">
                        {{ notification.timestamp | date : 'medium' }}
                        <span
                          *ngIf="notification.entityType"
                          class="badge bg-light text-dark ms-2"
                        >
                          {{ notification.entityType }}
                        </span>
                      </small>
                    </div>
                  </div>
                  <button
                    *ngIf="!notification.read"
                    class="btn btn-sm btn-outline-primary"
                    (click)="markAsRead(notification); $event.stopPropagation()"
                    [disabled]="isMarkingRead"
                  >
                    <span
                      *ngIf="isMarkingRead && markingReadId === notification.id"
                      class="spinner-border spinner-border-sm me-1"
                      role="status"
                    ></span>
                    Mark as Read
                  </button>
                </li>
              </ul>
            </div>
          </div>

          <!-- Pagination -->
          <div
            *ngIf="totalPages > 1"
            class="d-flex justify-content-center mt-4"
          >
            <nav>
              <ul class="pagination">
                <li class="page-item" [class.disabled]="!hasPrevious">
                  <a
                    class="page-link"
                    href="#"
                    (click)="
                      $event.preventDefault(); changePage(currentPage - 1)
                    "
                    aria-label="Previous"
                  >
                    <span aria-hidden="true">&laquo;</span>
                  </a>
                </li>
                <li
                  class="page-item"
                  *ngFor="let page of getPageNumbers()"
                  [class.active]="page === currentPage"
                >
                  <a
                    class="page-link"
                    href="#"
                    (click)="$event.preventDefault(); changePage(page)"
                    >{{ page }}</a
                  >
                </li>
                <li class="page-item" [class.disabled]="!hasNext">
                  <a
                    class="page-link"
                    href="#"
                    (click)="
                      $event.preventDefault(); changePage(currentPage + 1)
                    "
                    aria-label="Next"
                  >
                    <span aria-hidden="true">&raquo;</span>
                  </a>
                </li>
              </ul>
            </nav>
          </div>
        </div>
      </main>
    </div>
  `,
  styles: [
    `
      .main-content {
        margin-left: 240px;
        width: calc(100% - 240px);
        min-height: 100vh;
      }

      .notification-icon {
        width: 40px;
        height: 40px;
        border-radius: 50%;
        background-color: #e9ecef;
        display: flex;
        align-items: center;
        justify-content: center;
        font-size: 1.2rem;
        color: #6c757d;
        transition: all 0.3s ease;
      }

      .notification-icon.unread {
        background-color: #cfe2ff;
        color: #0d6efd;
      }

      .list-group-item {
        transition: background-color 0.3s ease;
        cursor: pointer;
      }

      .list-group-item:hover {
        background-color: #f8f9fa;
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
export class NotificationsComponent implements OnInit, OnDestroy {
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
  // User ID - In a real app, this would come from an auth service

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

          // Fallback to mock data for demo purposes
          if (err) {
            this.notifications = this.generateMockNotifications();
            this.totalCount = this.notifications.length;
            this.totalPages = 1;
          }
        },
      });

    this.subscriptions.push(sub);
  }

  markAsRead(notification: Notification) {
    // if (notification.read) return;
    // this.isMarkingRead = true;
    // this.markingReadId = notification.id;
    // const sub = this.notificationService
    //   .markNotificationAsRead(notification.id)
    //   .subscribe({
    //     next: () => {
    //       notification.read = true;
    //       this.isMarkingRead = false;
    //       this.markingReadId = null;
    //     },
    //     error: (err: any) => {
    //       console.error('Error marking notification as read:', err);
    //       // For demo purposes, still mark as read in the UI
    //       notification.read = true;
    //       this.isMarkingRead = false;
    //       this.markingReadId = null;
    //     },
    //   });
    // this.subscriptions.push(sub);
  }

  changePage(page: number) {
    if (page < 1 || page > this.totalPages) return;

    this.currentPage = page;
    this.loadNotifications();
  }

  getPageNumbers(): number[] {
    const pages: number[] = [];
    const maxPagesToShow = 5;

    if (this.totalPages <= maxPagesToShow) {
      // Show all pages if there are 5 or fewer
      for (let i = 1; i <= this.totalPages; i++) {
        pages.push(i);
      }
    } else {
      // Show a subset of pages with current page in the middle when possible
      let startPage = Math.max(
        1,
        this.currentPage - Math.floor(maxPagesToShow / 2)
      );
      let endPage = startPage + maxPagesToShow - 1;

      if (endPage > this.totalPages) {
        endPage = this.totalPages;
        startPage = Math.max(1, endPage - maxPagesToShow + 1);
      }

      for (let i = startPage; i <= endPage; i++) {
        pages.push(i);
      }
    }

    return pages;
  }

  // Fallback method for demo purposes
  generateMockNotifications(): any[] {
    return [
      {
        id: '1',
        userId: this.userId,
        entity: 'application-1',
        entityType: 'Application',
        message: 'New candidate application received',
        read: false,
        timestamp: new Date(),
      },
      {
        id: '2',
        userId: this.userId,
        entity: 'interview-1',
        entityType: 'Interview',
        message: 'Interview scheduled for tomorrow',
        read: false,
        timestamp: new Date(Date.now() - 86400000),
      },
      {
        id: '3',
        userId: this.userId,
        entity: 'evaluation-1',
        entityType: 'Evaluation',
        message: 'Reminder: Complete candidate evaluation',
        read: true,
        timestamp: new Date(Date.now() - 172800000),
      },
      {
        id: '4',
        userId: this.userId,
        entity: 'job-1',
        entityType: 'Job',
        message: 'New job posting approved',
        read: true,
        timestamp: new Date(Date.now() - 259200000),
      },
      {
        id: '5',
        userId: this.userId,
        entity: 'offer-1',
        entityType: 'Offer',
        message: 'Candidate John Doe accepted job offer',
        read: false,
        timestamp: new Date(Date.now() - 345600000),
      },
    ];
  }
}
