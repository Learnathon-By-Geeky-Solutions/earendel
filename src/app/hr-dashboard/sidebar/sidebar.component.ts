import {
  AfterViewInit,
  Component,
  HostListener,
  OnDestroy,
  OnInit,
  Renderer2,
} from "@angular/core";
import { CommonModule } from "@angular/common";
import { Router, RouterModule } from "@angular/router";
import { MatDialog } from "@angular/material/dialog";
import { LogoutModalComponent } from "../logout-modal/logout-modal.component";
import { filter, Subscription } from "rxjs";
import { NotificationhubService } from "../../shared/services/signalr/notificationhub.service";
import { LoginSignupService } from "../../shared/services/login-signup.service";

@Component({
  selector: "app-sidebar",
  standalone: true,
  imports: [CommonModule, RouterModule],
  template: `
    <!-- Toggle Button (Only visible on width <= 1366px) -->
    <button
      class="btn btn-dark d-lg-none sidebar-toggle"
      (click)="toggleSidebar()"
      *ngIf="showToggleButton"
    >
      <i class="bi bi-list"></i>
    </button>

    <!-- Sidebar Container -->
    <div class="sidebar-container" [class.open]="isOpen">
      <div class="sidebar bg-white border-end h-100">
        <div
          class="sidebar-header p-4 border-bottom d-flex justify-content-between align-items-center"
        >
          <h1 class="h5 mb-0 fw-bold">HR</h1>
          <button class="btn btn-link d-lg-none" (click)="toggleSidebar()">
            <i class="bi bi-x-lg"></i>
          </button>
        </div>

        <nav class="sidebar-nav p-3">
          <ul class="nav flex-column">
            <li class="nav-item">
              <a
                class="nav-link d-flex align-items-center gap-2"
                routerLink="/hr-dashboard/overview"
                routerLinkActive="active"
                [routerLinkActiveOptions]="{ exact: true }"
                (click)="closeSidebarOnMobile()"
              >
                <i class="bi bi-grid"></i>
                <span>Overview</span>
              </a>
            </li>

            <li class="nav-item">
              <a
                class="nav-link d-flex align-items-center gap-2"
                routerLink="/hr-dashboard/jobs"
                routerLinkActive="active"
                (click)="closeSidebarOnMobile()"
              >
                <i class="bi bi-briefcase"></i>
                <span>Jobs</span>
              </a>
            </li>

            <li class="nav-item">
              <a
                class="nav-link d-flex align-items-center gap-2 position-relative"
                routerLink="/hr-dashboard/notifications"
                routerLinkActive="active"
                [class.notification-pulse]="isAnimatingNotification"
                (click)="closeSidebarOnMobile()"
              >
                <i class="bi bi-bell"></i>
                <span>Notifications</span>
              </a>
            </li>

            <li class="nav-item">
              <a
                class="nav-link d-flex align-items-center gap-2"
                routerLink="/hr-dashboard/job-post"
                routerLinkActive="active"
                (click)="closeSidebarOnMobile()"
              >
                <i class="bi bi-file-earmark-plus"></i>
                <span>Post Job</span>
              </a>
            </li>

            <li class="nav-item">
              <a
                class="nav-link d-flex align-items-center gap-2"
                (click)="openLogoutModal()"
              >
                <i class="bi bi-box-arrow-right"></i>
                <span>Logout</span>
              </a>
            </li>
          </ul>
        </nav>
      </div>

      <!-- Backdrop -->
      <div class="sidebar-backdrop" (click)="toggleSidebar()"></div>
    </div>
  `,
  styles: [
    `
      .sidebar-toggle {
        position: fixed;
        top: 1rem;
        left: 1rem;
        z-index: 1040;
      }

      .sidebar-container {
        position: fixed;
        top: 0;
        left: 0;
        height: 100%;
        width: 240px;
        z-index: 1030;
        transform: translateX(-100%);
        transition: transform 0.3s ease-in-out;
      }

      .sidebar-container.open {
        transform: translateX(0);
        z-index: 1040;
      }

      .sidebar {
        width: 240px;
        height: 100%;
        overflow-y: auto;
        position: relative;
        background: white;
        z-index: 1050;
      }

      .sidebar-backdrop {
        position: fixed;
        top: 0;
        left: 0;
        width: 100%;
        height: 100%;
        background-color: rgba(0, 0, 0, 0.5);
        z-index: 1029;
        opacity: 0;
        transition: opacity 0.3s ease-in-out;
        pointer-events: none;
      }

      .sidebar-container.open .sidebar-backdrop {
        opacity: 1;
        pointer-events: auto;
      }

      .nav-link {
        color: #4b5563;
        padding: 0.75rem 1rem;
        border-radius: 6px;
        font-size: 15px;
        transition: all 0.2s ease;
      }

      .nav-link:hover {
        background-color: #f3f4f6;
        color: #111827;
      }

      .nav-link.active {
        background-color: #f3f4f6;
        color: #111827;
        font-weight: 500;
      }

      .nav-link i {
        font-size: 18px;
      }

      @keyframes notificationPulse {
        0% {
          background-color: transparent;
        }
        20% {
          background-color: rgba(220, 53, 69, 0.1);
        }
        40% {
          background-color: rgba(220, 53, 69, 0.2);
        }
        60% {
          background-color: rgba(220, 53, 69, 0.1);
        }
        80% {
          background-color: rgba(220, 53, 69, 0.3);
        }
        100% {
          background-color: transparent;
        }
      }

      .notification-pulse {
        animation: notificationPulse 2s ease-in-out;
        animation-iteration-count: 5;
      }

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
        border-radius: 50%;
        width: 24px;
        height: 24px;
        display: flex;
        align-items: center;
        justify-content: center;
      }

      .notification-close:hover {
        background-color: #f8f9fa;
        color: #212529;
      }
    `,
  ],
})
export class SidebarComponent implements AfterViewInit, OnInit, OnDestroy {
  isOpen = false;
  showToggleButton = false;
  unreadNotificationsCount = 0;
  isAnimatingNotification = false;

  activeNotifications: { id: string; message: string; timeout: any }[] = [];

  private subscriptions: Subscription[] = [];

  constructor(
    private dialog: MatDialog,
    private notificationHubService: NotificationhubService,
    private renderer: Renderer2,
    private logOutService: LoginSignupService,
    private router: Router
  ) {}

  ngAfterViewInit() {
    this.updateSidebar(window.innerWidth);
  }

  ngOnInit(): void {
    const user = JSON.parse(sessionStorage.getItem("loggedInUser") || "{}");
    this.notificationHubService.startConnection(user?.token);

    this.subscriptions.push(
      this.notificationHubService.connectionEstablished$
        .pipe(filter((connected) => connected))
        .subscribe(() => console.log("Connected")),

      this.notificationHubService.systemAlerts$.subscribe((message: any) => {
        this.handleNotification(message);
      })
    );
  }

  ngOnDestroy(): void {
    this.subscriptions.forEach((sub) => sub.unsubscribe());
  }

  handleNotification(message: any): void {
    this.unreadNotificationsCount++;

    // Extended pulse animation trigger
    this.isAnimatingNotification = true;

    setTimeout(() => (this.isAnimatingNotification = false), 10000);
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

  @HostListener("window:resize", ["$event"])
  onResize(event: any) {
    this.updateSidebar(event.target.innerWidth);
  }

  updateSidebar(width: number) {
    this.isOpen = width >= 1000;
    this.showToggleButton = width <= 1366;
  }

  toggleSidebar() {
    if (window.innerWidth < 1366) this.isOpen = !this.isOpen;
  }

  closeSidebarOnMobile() {
    if (window.innerWidth < 1366) this.isOpen = false;
  }

  openLogoutModal() {
    const ref = this.dialog.open(LogoutModalComponent, { width: "300px" });
    ref.afterClosed().subscribe((result) => {
      if (!result) {
        return;
      }
      if (result) {
        const user = JSON.parse(sessionStorage.getItem("loggedInUser") || "{}");
        const userId = user.userId;

        // 1. Call logout API
        this.logOutService.logOut({ userId }).subscribe({
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
