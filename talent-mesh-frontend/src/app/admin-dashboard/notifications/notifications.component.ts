import { Component, type OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SidebarComponent } from '../sidebar/sidebar.component';

interface Notification {
  id: number;
  message: string;
  timestamp: Date;
  read: boolean;
}

@Component({
  selector: 'app-notifications',
  standalone: true,
  imports: [CommonModule, SidebarComponent],
  template: `
    <div class="d-flex">
      <app-sidebar></app-sidebar>

      <main class="main-content bg-light">
        <div class="container py-4">
          <h1 class="h3 mb-4">Notifications</h1>

          <div class="card border-0 shadow-sm">
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
                      <small class="text-muted">{{
                        notification.timestamp | date : 'medium'
                      }}</small>
                    </div>
                  </div>
                  <button
                    *ngIf="!notification.read"
                    class="btn btn-sm btn-outline-primary"
                    (click)="markAsRead(notification); $event.stopPropagation()"
                  >
                    Mark as Read
                  </button>
                </li>
              </ul>
            </div>
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
export class NotificationsComponent implements OnInit {
  notifications: Notification[] = [];

  ngOnInit() {
    this.notifications = this.generateMockNotifications();
  }

  generateMockNotifications(): Notification[] {
    return [
      {
        id: 1,
        message: 'New candidate application received',
        timestamp: new Date(),
        read: false,
      },
      {
        id: 2,
        message: 'Interview scheduled for tomorrow',
        timestamp: new Date(Date.now() - 86400000),
        read: false,
      },
      {
        id: 3,
        message: 'Reminder: Complete candidate evaluation',
        timestamp: new Date(Date.now() - 172800000),
        read: true,
      },
      {
        id: 4,
        message: 'New job posting approved',
        timestamp: new Date(Date.now() - 259200000),
        read: true,
      },
      {
        id: 5,
        message: 'Candidate John Doe accepted job offer',
        timestamp: new Date(Date.now() - 345600000),
        read: false,
      },
    ];
  }

  markAsRead(notification: Notification) {
    notification.read = true;
  }
}
