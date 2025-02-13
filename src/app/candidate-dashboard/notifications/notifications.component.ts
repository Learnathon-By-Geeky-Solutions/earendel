import { Component, type OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';

interface Notification {
  id: number;
  message: string;
  timestamp: Date;
  isRead: boolean;
}

@Component({
  selector: 'app-notifications',
  standalone: true,
  imports: [CommonModule],
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
          <div class="notification-actions">
            <button
              class="delete-btn"
              (click)="deleteNotification(notification, $event)"
            >
              <i class="bi bi-trash"></i>
            </button>
          </div>
        </div>
      </div>
      <div *ngIf="notifications.length === 0" class="no-notifications">
        <i class="bi bi-bell-slash"></i>
        <p>No notifications at the moment</p>
      </div>
    </div>
  `,
  styles: [
    `
      .notifications-container {
        padding: 24px;
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
    `,
  ],
})
export class CandidateNotificationsComponent implements OnInit {
  notifications: Notification[] = [];

  ngOnInit() {
    // Simulating fetched notifications
    this.notifications = [
      {
        id: 1,
        message:
          'Your interview for Software Engineer position at Tech Corp has been scheduled for tomorrow at 2 PM.',
        timestamp: new Date(Date.now() - 1000 * 60 * 60), // 1 hour ago
        isRead: false,
      },
      {
        id: 2,
        message:
          'New job posting matching your profile: Senior Frontend Developer at Innovation Labs',
        timestamp: new Date(Date.now() - 1000 * 60 * 60 * 24), // 1 day ago
        isRead: false,
      },
      {
        id: 3,
        message:
          'Your application for Full Stack Developer at Acme Corp has been viewed',
        timestamp: new Date(Date.now() - 1000 * 60 * 60 * 72), // 3 days ago
        isRead: true,
      },
    ];
  }

  markAsRead(notification: Notification) {
    if (!notification.isRead) {
      notification.isRead = true;
      // Here you would typically update the backend as well
    }
  }

  deleteNotification(notification: Notification, event: Event) {
    event.stopPropagation(); // Prevent triggering markAsRead
    this.notifications = this.notifications.filter(
      (n) => n.id !== notification.id
    );
    // Here you would typically update the backend as well
  }
}
