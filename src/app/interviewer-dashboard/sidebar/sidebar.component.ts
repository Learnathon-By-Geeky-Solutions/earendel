import { CommonModule } from '@angular/common';
import {
  Component,
  ElementRef,
  EventEmitter,
  HostListener,
  Output,
  Renderer2,
  ViewChild,
} from '@angular/core';
import { RouterModule } from '@angular/router';
import { LogoutModalComponent } from '../../hr-dashboard/logout-modal/logout-modal.component';
import { filter, Subscription } from 'rxjs';
import { MatDialog } from '@angular/material/dialog';
import { NotificationhubService } from '../../shared/services/signalr/notificationhub.service';

@Component({
  selector: 'app-sidebar',
  standalone: true,
  imports: [RouterModule, CommonModule],
  template: `
    <!-- Sidebar -->
    <div
      class="sidebar bg-white border-end"
      style="width: 240px; height: 100vh;"
    >
      <div class="p-4">
        <h4 class="h4 mb-4">Interviewer Dashboard</h4>
        <nav class="nav flex-column gap-2">
          <a
            class="nav-link px-3 py-2 rounded-2"
            routerLink="/interviewer-dashboard"
            routerLinkActive="active"
            [routerLinkActiveOptions]="{ exact: true }"
          >
            Dashboard
          </a>
          <a
            class="nav-link px-3 py-2 rounded-2"
            routerLinkActive="active"
            [routerLinkActiveOptions]="{ exact: true }"
            routerLink="/interviewer-dashboard/availability"
          >
            Availability
          </a>
          <a
            class="nav-link px-3 py-2 rounded-2"
            routerLinkActive="active"
            [routerLinkActiveOptions]="{ exact: true }"
            routerLink="/interviewer-dashboard/pending-request"
          >
            Requested Interviews
          </a>
          <a
            class="nav-link px-3 py-2 rounded-2"
            routerLinkActive="active"
            [routerLinkActiveOptions]="{ exact: true }"
            routerLink="/interviewer-dashboard/upcoming"
          >
            Upcoming Interviews
          </a>
          <a
            class="nav-link px-3 py-2 rounded-2"
            routerLinkActive="active"
            [routerLinkActiveOptions]="{ exact: true }"
            [class.notification-pulse]="isAnimatingNotification"
            routerLink="/interviewer-dashboard/notifications"
          >
            Notifications
          </a>
          <a
            class="nav-link px-3 py-2 rounded-2"
            routerLinkActive="active"
            [routerLinkActiveOptions]="{ exact: true }"
            routerLink="/interviewer-dashboard/past-interviews"
          >
            Past Interviews
          </a>
        
          <a
            class="nav-link px-3 py-2 rounded-2"
            routerLinkActive="active"
            [routerLinkActiveOptions]="{ exact: true }"
            routerLink="/interviewer-dashboard/earnings"
          >
            Earnings
          </a>
          <a
            class="nav-link px-3 py-2 rounded-2"
            routerLinkActive="active"
            [routerLinkActiveOptions]="{ exact: true }"
            routerLink="/interviewer-dashboard/profile"
          >
            Profile
          </a>
        </nav>
      </div>
    </div>
    <!-- Notification container -->
    <div #notificationContainer class="notification-container"></div>
  `,
  styles: [
    `
      .nav-link {
        color: #666;
        transition: all 0.3s;
      }
      .nav-link:hover,
      .nav-link.active {
        background-color: #f8f9fa;
        color: #000;
      }
      .card {
        transition: transform 0.2s;
      }
      .card:hover {
        transform: translateY(-2px);
      }
      .activity-icon {
        width: 40px;
        height: 40px;
        display: flex;
        align-items: center;
        justify-content: center;
      }
      .upcoming-interviews {
        max-height: 300px;
        overflow-y: auto;
      }

      /* Notification indicator */
      .notification-indicator {
        position: relative;
      }

      .notification-indicator::after {
        content: '';
        position: absolute;
        top: -2px;
        right: -2px;
        width: 8px;
        height: 8px;
        background-color: #dc3545;
        border-radius: 50%;
        border: 2px solid white;
      }

      /* Notification animation */
      @keyframes notificationPulse {
        0% {
          background-color: transparent;
        }
        25% {
          background-color: rgba(220, 53, 69, 0.1);
        }
        50% {
          background-color: rgba(220, 53, 69, 0.2);
        }
        75% {
          background-color: rgba(220, 53, 69, 0.1);
        }
        100% {
          background-color: transparent;
        }
      }

      .notification-pulse {
        animation: notificationPulse 1.5s ease-in-out;
        animation-iteration-count: 3;
      }

      /* Notification toast styling */
      .notification-toast {
        position: fixed;
        top: 20px;
        right: 20px;
        background-color: white;
        border-left: 4px solid #0d6efd;
        box-shadow: 0 4px 12px rgba(0, 0, 0, 0.15);
        padding: 12px 16px;
        border-radius: 4px;
        display: flex;
        align-items: center;
        justify-content: space-between;
        min-width: 300px;
        max-width: 400px;
        z-index: 9999;
        transform: translateX(120%);
        opacity: 0;
        transition: transform 0.3s ease, opacity 0.3s ease;
      }

  

        h4 {
          font-size: 18px;
          font-weight: 800;
          margin: 0;
          color: #333;
        }
      


      .notification-toast.show {
        transform: translateX(0);
        opacity: 1;
      }

      .notification-toast.hide {
        transform: translateX(120%);
        opacity: 0;
      }

      .notification-message {
        flex: 1;
        margin-right: 12px;
        font-size: 14px;
      }

      .notification-close {
        background: none;
        border: none;
        color: #6c757d;
        cursor: pointer;
        padding: 4px;
        display: flex;
        align-items: center;
        justify-content: center;
        border-radius: 50%;
        width: 24px;
        height: 24px;
      }

      .notification-close:hover {
        background-color: #f8f9fa;
        color: #212529;
      }

      .notification-container {
        position: fixed;
        top: 20px;
        right: 20px;
        z-index: 9999;
        display: flex;
        flex-direction: column;
        gap: 10px;
      }

      @media (min-width: 1366px) {
        .sidebar-toggle {
          display: none;
        }

        .sidebar-container {
          transform: translateX(0);
        }

        .sidebar-backdrop {
          display: none;
        }
      }
    `,
  ],
})
export class SidebarComponent {
  @Output() sidebarToggle = new EventEmitter<boolean>();
  @ViewChild('notificationContainer') notificationContainer!: ElementRef;

  unreadNotificationsCount = 0;
  isOpen = false;
  showToggleButton = false;
  hasUnreadNotifications = false;
  activeNotifications: { id: string; message: string; timeout: any }[] = [];
  isAnimatingNotification = false;

  private subscriptions: Subscription[] = [];

  constructor(
    private dialog: MatDialog,
    private notificationHubService: NotificationhubService,
    private renderer: Renderer2
  ) {}

  ngOnInit(): void {
    const user = JSON.parse(sessionStorage.getItem('loggedInUser') || '{}');
    this.notificationHubService.startConnection(user?.token);

    const connectionSub = this.notificationHubService.connectionEstablished$
      .pipe(filter((connected) => connected))
      .subscribe(() =>
        console.log('Connection established with notification hub')
      );

    const notificationSub = this.notificationHubService.systemAlerts$.subscribe(
      (message: unknown) => {
        this.handleNotification(message);
      }
    );

    this.subscriptions.push(connectionSub, notificationSub);
  }

  ngOnDestroy(): void {
    // Clean up subscriptions to prevent memory leaks
    this.subscriptions.forEach((sub) => sub.unsubscribe());

    // Clear any remaining notification timeouts
    this.activeNotifications.forEach((notification) => {
      clearTimeout(notification.timeout);
    });
  }

  handleNotification(message: any): void {
    console.log('Received notification:', message);

    // Increment unread count
    this.hasUnreadNotifications = true;

    // Trigger animation
    this.isAnimatingNotification = true;
    setTimeout(() => {
      this.isAnimatingNotification = false;
    }, 4500); // 3 iterations of 1.5s animation

    // Create notification toast
    const notificationId = `notification-${Date.now()}`;
    const notificationMessage =
      message.message || 'You have a new notification';

    // Add to DOM
    this.showNotificationToast(notificationId, notificationMessage);
  }

  showNotificationToast(id: string, message: string): void {
    // Create notification element
    const notificationElement = this.renderer.createElement('div');
    this.renderer.addClass(notificationElement, 'notification-toast');
    this.renderer.setAttribute(notificationElement, 'id', id);

    // Create message content
    const messageElement = this.renderer.createElement('span');
    this.renderer.addClass(messageElement, 'notification-message');
    const messageText = this.renderer.createText(message);
    this.renderer.appendChild(messageElement, messageText);

    // Create close button
    const closeButton = this.renderer.createElement('button');
    this.renderer.addClass(closeButton, 'notification-close');
    const closeIcon = this.renderer.createElement('i');
    this.renderer.addClass(closeIcon, 'bi');
    this.renderer.addClass(closeIcon, 'bi-x');
    this.renderer.appendChild(closeButton, closeIcon);

    // Add event listener to close button
    this.renderer.listen(closeButton, 'click', () => {
      this.dismissNotification(id);
    });

    // Append elements to notification
    this.renderer.appendChild(notificationElement, messageElement);
    this.renderer.appendChild(notificationElement, closeButton);

    // Append to container
    this.renderer.appendChild(document.body, notificationElement);

    // Set timeout to auto-dismiss
    const timeout = setTimeout(() => {
      this.dismissNotification(id);
    }, 5000);

    // Store reference to notification
    this.activeNotifications.push({ id, message, timeout });

    // Animate in
    setTimeout(() => {
      const element = document.getElementById(id);
      if (element) {
        this.renderer.addClass(element, 'show');
      }
    }, 10);
  }

  dismissNotification(id: string): void {
    const element = document.getElementById(id);
    if (element) {
      // Animate out
      this.renderer.removeClass(element, 'show');
      this.renderer.addClass(element, 'hide');

      // Remove after animation
      setTimeout(() => {
        this.renderer.removeChild(document.body, element);
      }, 300);
    }

    // Clear timeout and remove from active notifications
    const index = this.activeNotifications.findIndex((n) => n.id === id);
    if (index !== -1) {
      clearTimeout(this.activeNotifications[index].timeout);
      this.activeNotifications.splice(index, 1);
    }
  }

  ngAfterViewInit() {
    // Set initial state based on window width
    this.updateSidebar(window.innerWidth);
  }

  @HostListener('window:resize', ['$event'])
  onResize(event: any) {
    this.updateSidebar(event.target.innerWidth);
  }

  updateSidebar(width: number) {
    this.isOpen = width >= 1000; // Sidebar always open for large screens
    this.showToggleButton = width <= 1000; // Show toggle button when width <= 1366px
  }

  toggleSidebar() {
    if (window.innerWidth < 1366) {
      this.isOpen = !this.isOpen;
      this.sidebarToggle.emit(this.isOpen);
    }
  }

  closeSidebarOnMobile() {
    if (window.innerWidth < 1366) {
      this.isOpen = false;
      this.sidebarToggle.emit(false);
    }
  }

  openLogoutModal() {
    const dialogRef = this.dialog.open(LogoutModalComponent, {
      width: '300px',
    });
    dialogRef.afterClosed().subscribe((result) => {
      if (result) {
        // Perform logout action here
        console.log('User confirmed logout');
      }
    });
  }
}
