import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';

@Component({
  selector: 'app-candidate-sidebar',
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
          Requested
        </a>
        <a
          routerLink="/candidate-dashboard/completed"
          routerLinkActive="active"
          class="nav-item"
        >
          <i class="bi bi-check-circle"></i>
          Completed
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
        >
          <i class="bi bi-bell"></i>
          Notifications
          <span class="notification-badge" *ngIf="unreadNotifications > 0">{{
            unreadNotifications
          }}</span>
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
          position: relative;
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
        top: 50%; /* Centers it relative to the icon/text */
        right: 10px; /* Adjust to align it properly */
        transform: translateY(-50%); /* Keeps it vertically centered */
        background-color: #dc3545;
        color: white;
        font-size: 12px;
        font-weight: bold;
        padding: 2px 6px;
        border-radius: 10px;
        min-width: 20px;
        text-align: center;
        display: flex;
        justify-content: center;
        align-items: center;
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

  toggleSidebar() {
    this.isOpen = !this.isOpen;
  }

  openLogoutModal() {
    this.isLogoutModalOpen = true;
  }

  closeLogoutModal() {
    this.isLogoutModalOpen = false;
  }

  confirmLogout() {
    this.isLogoutModalOpen = false;
    // Implement actual logout logic here
    console.log('Logged out successfully!');
  }

  stopPropagation(event: MouseEvent) {
    event.stopPropagation();
  }
}
