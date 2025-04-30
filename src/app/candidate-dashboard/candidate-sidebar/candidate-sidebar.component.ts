import {
  Component,
  ElementRef,
  EventEmitter,
  HostListener,
  Output,
  Renderer2,
  ViewChild,
} from "@angular/core";
import { CommonModule } from "@angular/common";
import { Router, RouterModule } from "@angular/router";
import { MatDialog } from "@angular/material/dialog";
import { NotificationhubService } from "../../shared/services/signalr/notificationhub.service";
import { filter, Subscription } from "rxjs";
import { LogoutModalComponent } from "../../hr-dashboard/logout-modal/logout-modal.component";
import { LoginSignupService } from "../../shared/services/login-signup.service";

@Component({
  selector: "app-candidate-sidebar",
  standalone: true,
  imports: [CommonModule, RouterModule],
  template: `
    <button (click)="toggleSidebar()" class="sidebar-toggle">
      <i class="bi bi-list"></i>
    </button>
    <aside class="sidebar" [class.open]="isOpen">
      <div class="sidebar-header">
        <h1>Candidate</h1>
      </div>
      <nav class="sidebar-nav">
        <a
          routerLink="/candidate-dashboard/requested"
          routerLinkActive="active"
          class="nav-item"
        >
          <i class="bi bi-calendar-check"></i>
          Applications
        </a>

        <a
          routerLink="/candidate-dashboard/jobs"
          routerLinkActive="active"
          class="nav-item"
        >
          <i class="bi bi-briefcase"></i>
          Job Posts
        </a>
        <a
          routerLink="/candidate-dashboard/upcoming-interview"
          routerLinkActive="active"
          class="nav-item"
        >
          <i class="bi bi-briefcase"></i>
          Upcoming Interview
        </a>
        <a
          routerLink="/candidate-dashboard/quiz"
          routerLinkActive="active"
          class="nav-item"
        >
          <i class="bi bi-journal-check"></i>
          Quiz
        </a>
        <a
          routerLink="/candidate-dashboard/profile"
          routerLinkActive="active"
          class="nav-item"
        >
          <i class="bi bi-person"></i>
          Profile
        </a>
        <a
          routerLink="/candidate-dashboard/notifications"
          routerLinkActive="active"
          class="nav-item"
          [class.notification-pulse]="isAnimatingNotification"
        >
          <i class="bi bi-bell"></i>
          Notifications
        </a>
        <button (click)="openLogoutModal()" class="nav-item logout">
          <i class="bi bi-box-arrow-right"></i>
          Logout
        </button>
      </nav>
    </aside>

    <!-- Logout Modal -->
    <div
      class="modal-overlay"
      *ngIf="isLogoutModalOpen"
      (click)="closeLogoutModal()"
    >
      <div class="modal-content" (click)="stopPropagation($event)">
        <h5>Are you sure you want to logout?</h5>
        <div class="modal-actions">
          <button class="cancel-btn" (click)="closeLogoutModal()">
            Cancel
          </button>
          <button class="confirm-btn" (click)="confirmLogout()">Logout</button>
        </div>
      </div>
    </div>

    <!-- Notification container -->
    <div #notificationContainer class="notification-container"></div>
  `,
  styles: [
    `
      .sidebar {
        background: white;
        border-right: 1px solid #eee;
        padding: 24px 0;
        height: 100vh;
        position: fixed;
        width: 240px;
        z-index: 1000;
        transition: transform 0.3s ease;
      }

      .sidebar-header {
        padding: 0 24px 24px;
        border-bottom: 1px solid #eee;

        h1 {
          font-size: 24px;
          font-weight: 600;
          margin: 0;
          color: #333;
        }
      }

      .sidebar-nav {
        padding: 24px 12px;
        display: flex;
        flex-direction: column;
        gap: 8px;

        .nav-item {
          display: flex;
          align-items: center;
          gap: 12px;
          padding: 12px;
          color: #666;
          text-decoration: none;
          border-radius: 8px;
          transition: all 0.2s ease;
          font-size: 14px;
          border: none;
          background: none;
          width: 100%;
          text-align: left;
          cursor: pointer;

          i {
            font-size: 18px;
          }

          &:hover {
            background: #f8f9fa;
          }

          &.active {
            background: #f0f7ff;
            color: #0066ff;
          }

          &.logout {
            margin-top: auto;
            color: #dc3545;

            &:hover {
              background: #feeeee;
            }
          }
        }
      }
      .notification-badge {
        position: absolute;
        top: 58%;
        right: 12px;
        transform: translateY(-50%);
        background-color: #dc3545;
        color: white;
        font-size: 12px;
        font-weight: bold;
        padding: 2px 6px;
        border-radius: 10px;
        min-width: 20px;
        text-align: center;
      }

      .modal-overlay {
        position: fixed;
        top: 0;
        left: 0;
        width: 100%;
        height: 100%;
        background: rgba(0, 0, 0, 0.6);
        display: flex;
        justify-content: center;
        align-items: center;
        z-index: 2000;
      }

      .modal-content {
        background: white;
        padding: 24px;
        border-radius: 8px;
        box-shadow: 0 4px 6px rgba(0, 0, 0, 0.1);
        max-width: 400px;
        width: 90%;
        text-align: center;
      }

      .modal-actions {
        display: flex;
        justify-content: space-between;
        gap: 12px;
        margin-top: 16px;

        .cancel-btn {
          padding: 8px 16px;
          border: 1px solid #ccc;
          background: white;
          color: #333;
          border-radius: 4px;
          cursor: pointer;

          &:hover {
            background: #f8f9fa;
          }
        }

        .confirm-btn {
          padding: 8px 16px;
          border: none;
          background: #dc3545;
          color: white;
          border-radius: 4px;
          cursor: pointer;

          &:hover {
            background: #c82333;
          }
        }
      }

      .sidebar-toggle {
        position: fixed;
        top: 16px;
        left: 16px;
        z-index: 2001;
        background: #0066ff;
        color: white;
        border: none;
        padding: 8px 12px;
        border-radius: 4px;
        cursor: pointer;

        i {
          font-size: 20px;
        }

        &:hover {
          background: #0056cc;
        }
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

      @media (max-width: 768px) {
        .sidebar {
          transform: translateX(-100%);
        }

        .sidebar.open {
          transform: translateX(0);
        }
      }

      @media (min-width: 769px) {
        .sidebar-toggle {
          display: none;
        }
      }
    `,
  ],
})
export class CandidateSidebarComponent {
  unreadNotifications = 6;
  isOpen = false;
  isLogoutModalOpen = false;

  closeLogoutModal() {
    this.isLogoutModalOpen = false;
  }

  confirmLogout() {
    this.isLogoutModalOpen = false;
    // Implement actual logout logic here
    console.log("Logged out successfully!");
  }

  stopPropagation(event: MouseEvent) {
    event.stopPropagation();
  }

  @Output() sidebarToggle = new EventEmitter<boolean>();
  @ViewChild("notificationContainer") notificationContainer!: ElementRef;

  unreadNotificationsCount = 0;
  showToggleButton = false;
  hasUnreadNotifications = false;
  activeNotifications: { id: string; message: string; timeout: any }[] = [];
  isAnimatingNotification = false;

  private subscriptions: Subscription[] = [];

  constructor(
    private dialog: MatDialog,
    private notificationHubService: NotificationhubService,
    private logOutService: LoginSignupService,
    private router: Router,
    private renderer: Renderer2
  ) {}

  ngOnInit(): void {
    const user = JSON.parse(sessionStorage.getItem("loggedInUser") || "{}");
    this.notificationHubService.startConnection(user?.token);

    const connectionSub = this.notificationHubService.connectionEstablished$
      .pipe(filter((connected) => connected))
      .subscribe(() =>
        console.log("Connection established with notification hub")
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
    console.log("Received notification:", message);

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
      message.message || "You have a new notification";

    // Add to DOM
    this.showNotificationToast(notificationId, notificationMessage);
  }

  showNotificationToast(id: string, message: string): void {
    // Create notification element
    const notificationElement = this.renderer.createElement("div");
    this.renderer.addClass(notificationElement, "notification-toast");
    this.renderer.setAttribute(notificationElement, "id", id);

    // Create message content
    const messageElement = this.renderer.createElement("span");
    this.renderer.addClass(messageElement, "notification-message");
    const messageText = this.renderer.createText(message);
    this.renderer.appendChild(messageElement, messageText);

    // Create close button
    const closeButton = this.renderer.createElement("button");
    this.renderer.addClass(closeButton, "notification-close");
    const closeIcon = this.renderer.createElement("i");
    this.renderer.addClass(closeIcon, "bi");
    this.renderer.addClass(closeIcon, "bi-x");
    this.renderer.appendChild(closeButton, closeIcon);

    // Add event listener to close button
    this.renderer.listen(closeButton, "click", () => {
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
        this.renderer.addClass(element, "show");
      }
    }, 10);
  }

  dismissNotification(id: string): void {
    const element = document.getElementById(id);
    if (element) {
      // Animate out
      this.renderer.removeClass(element, "show");
      this.renderer.addClass(element, "hide");

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

  @HostListener("window:resize", ["$event"])
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
      width: "300px",
    });
    dialogRef.afterClosed().subscribe((result) => {
      if (!result) {
        return;
      }
      if (result) {
        const user = JSON.parse(sessionStorage.getItem("loggedInUser") || "{}");
        const userId = user.userId;

        // 1. Call logout API
        this.logOutService.logOut({userId}).subscribe({
          next: () => {
            // 2. On success: clear storage, navigate
            sessionStorage.removeItem("loggedInUser");
            this.router.navigate(["/login"]);
            console.log("Logged out successfully, navigating to login.");
          },
          error: (err) => {
            console.error("Logout failed", err);
            // Optionally show an error toast/snackbar
          },
        });
      }
    });
  }
}
