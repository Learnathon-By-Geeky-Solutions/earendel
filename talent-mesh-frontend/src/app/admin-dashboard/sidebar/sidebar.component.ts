import { Component, Output, EventEmitter, HostListener } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { MatDialog } from '@angular/material/dialog';
import { LogoutModalComponent } from '../logout-modal/logout-modal.component';

@Component({
  selector: 'app-sidebar',
  standalone: true,
  imports: [CommonModule, RouterModule],
  template: `
    <!-- Toggle Button (Visible only on small screens) -->
    <button
      class="btn btn-dark d-lg-none sidebar-toggle"
      (click)="toggleSidebar()"
      *ngIf="showToggleButton"
    >
      <i class="bi bi-list"></i>
    </button>
    <div class="sidebar-container" [class.open]="isOpen">
      <div class="sidebar bg-white border-end h-100">
        <div
          class="sidebar-header p-4 border-bottom d-flex justify-content-between align-items-center"
        >
          <h1 class="h5 mb-0 fw-bold">Admin</h1>
          <button class="btn btn-link d-md-none" (click)="toggleSidebar()">
            <i class="bi bi-x-lg"></i>
          </button>
        </div>

        <nav class="sidebar-nav p-3">
          <ul class="nav flex-column">
            <li class="nav-item">
              <a
                class="nav-link d-flex align-items-center gap-2"
                routerLink="/admin-dashboard/overview"
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
                routerLink="/admin-dashboard/candidates"
                routerLinkActive="active"
                (click)="closeSidebarOnMobile()"
              >
                <i class="bi bi-people"></i>
                <span>Candidates</span>
              </a>
            </li>
            <li class="nav-item">
              <a
                class="nav-link d-flex align-items-center gap-2"
                routerLink="/admin-dashboard/hr"
                routerLinkActive="active"
                (click)="closeSidebarOnMobile()"
              >
                <i class="bi bi-person-badge"></i>
                <span>HR</span>
              </a>
            </li>
            <li class="nav-item">
              <a
                class="nav-link d-flex align-items-center gap-2"
                routerLink="/admin-dashboard/interviewers"
                routerLinkActive="active"
                (click)="closeSidebarOnMobile()"
              >
                <i class="bi bi-person-video2"></i>
                <span>Interviewers</span>
              </a>
            </li>
            <li class="nav-item">
              <a
                class="nav-link d-flex align-items-center gap-2"
                routerLink="/admin-dashboard/jobs"
                routerLinkActive="active"
                (click)="closeSidebarOnMobile()"
              >
                <i class="bi bi-briefcase"></i>
                <span>Jobs</span>
              </a>
            </li>
            <li class="nav-item">
              <a
                class="nav-link d-flex align-items-center gap-2"
                routerLink="/admin-dashboard/skills"
                routerLinkActive="active"
                (click)="closeSidebarOnMobile()"
              >
                <i class="bi bi-person-video2"></i>
                <span>Skills</span>
              </a>
            </li>
            <li class="nav-item">
              <a
                class="nav-link d-flex align-items-center gap-2"
                routerLink="/admin-dashboard/payments"
                routerLinkActive="active"
                (click)="closeSidebarOnMobile()"
              >
                <i class="bi bi-credit-card"></i>
                <span>Payments</span>
              </a>
            </li>
            <li class="nav-item">
              <a
                class="nav-link d-flex align-items-center gap-2 position-relative"
                routerLink="/admin-dashboard/notifications"
                routerLinkActive="active"
                (click)="closeSidebarOnMobile()"
              >
                <i class="bi bi-bell"></i>
                <span>Notifications</span>
                <span
                  class="position-absolute top-0 start-100 translate-middle badge rounded-pill bg-danger"
                >
                  {{ unreadNotificationsCount }}
                  <span class="visually-hidden">unread notifications</span>
                </span>
              </a>
            </li>
            <li class="nav-item">
              <a
                class="nav-link d-flex align-items-center gap-2"
                routerLink="/admin-dashboard/verification"
                routerLinkActive="active"
                (click)="closeSidebarOnMobile()"
              >
                <i class="bi bi-shield-check"></i>
                <span>Verification</span>
              </a>
            </li>
            <li class="nav-item">
              <a
                class="nav-link d-flex align-items-center gap-2"
                routerLink="/admin-dashboard/admin-list"
                routerLinkActive="active"
              >
                <i class="bi bi-people-fill"></i>
                <span>Admin List</span>
              </a>
            </li>
            <li class="nav-item">
              <a
                class="nav-link d-flex align-items-center gap-2"
                routerLink="/admin-dashboard/profile"
                routerLinkActive="active"
              >
                <i class="bi bi-people-fill"></i>
                <span>Profile</span>
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
  unreadNotificationsCount = 5;

  isOpen = window.innerWidth >= 1366; // Sidebar open by default for large screens
  showToggleButton = window.innerWidth <= 1366; // Show toggle button when screen is <= 1366px

  constructor(private dialog: MatDialog) {}

  @HostListener('window:resize', ['$event'])
  onResize(event: any) {
    const width = event.target.innerWidth;
    this.isOpen = width >= 1000; // Sidebar always open for large screens
    this.showToggleButton = width <= 1000; // Show toggle button when width <= 1366px
  }

  toggleSidebar() {
    if (window.innerWidth < 1360) {
      this.isOpen = !this.isOpen;
    }
  }

  closeSidebarOnMobile() {
    if (window.innerWidth < 1360) {
      this.isOpen = false;
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
