import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { SidebarComponent } from '../sidebar/sidebar.component';

@Component({
  selector: 'app-profile',
  standalone: true,
  imports: [CommonModule, FormsModule, SidebarComponent],
  template: `
    <div class="dashboard-container">
      <!-- Import existing sidebar -->
      <app-sidebar></app-sidebar>

      <main class="main-content">
        <header class="content-header">
          <h1>Your Profile</h1>
          <button class="switch-btn" (click)="switchToCandidate()">
            <i class="bi bi-arrow-repeat"></i>
            Switch to Candidate
          </button>
        </header>

        <div class="profile-grid">
          <!-- Profile Section -->
          <div class="profile-section">
            <div class="profile-header">
              <div class="avatar-container">
                <img
                  [src]="
                    profile.avatarUrl ||
                    'https://hebbkx1anhila5yf.public.blob.vercel-storage.com/i-9.PNG-u7b40D9hFNL4HVyFx0el0mXkJaEfFX.png'
                  "
                  alt="Profile picture"
                  class="avatar"
                />
              </div>
              <div class="profile-info">
                <h2>{{ profile.name }}</h2>
                <p class="email">{{ profile.email }}</p>
              </div>
            </div>

            <div class="profile-details">
              <div class="detail-group">
                <label>Company</label>
                <p>{{ profile.company }}</p>
              </div>

              <div class="detail-group">
                <label>Role</label>
                <p>{{ profile.role }}</p>
              </div>

              <div class="detail-group">
                <label>Bio</label>
                <p>{{ profile.bio }}</p>
              </div>

              <div class="detail-group">
                <label>Skills</label>
                <div class="skills-container">
                  <span *ngFor="let skill of profile.skills" class="skill-tag">
                    {{ skill }}
                  </span>
                </div>
              </div>

              <div class="verification-badge" *ngIf="profile.isVerified">
                <i class="bi bi-patch-check-fill"></i>
                Verified Interviewer
              </div>

              <button class="edit-btn">Edit Profile</button>
            </div>
          </div>

          <!-- Wallet Section -->
          <div class="wallet-section">
            <h2>Wallet</h2>
            <p class="wallet-subtitle">Manage your earnings and cash out</p>

            <div class="balance-card">
              <div class="balance-header">
                <span>Available Balance</span>
                <i class="bi bi-currency-dollar"></i>
              </div>
              <div class="balance-amount">
                $ {{ wallet.balance.toFixed(2) }}
              </div>
            </div>

            <div class="cashout-section">
              <label>Cashout Amount</label>
              <div class="input-group">
                <input
                  type="number"
                  placeholder="Enter amount"
                  [(ngModel)]="cashoutAmount"
                  class="cashout-input"
                />
                <button class="cashout-btn" [disabled]="!isValidCashout()">
                  Cash Out
                </button>
              </div>
            </div>

            <div class="wallet-actions">
              <button class="action-btn">
                <i class="bi bi-link-45deg"></i>
                Linked Accounts
              </button>
              <button class="action-btn">
                <i class="bi bi-clock-history"></i>
                Transaction History
                <i class="bi bi-chevron-right"></i>
              </button>
            </div>
          </div>
        </div>
      </main>
    </div>
  `,
  styles: [
    `
      .dashboard-container {
        display: grid;
        grid-template-columns: 240px 1fr;
        min-height: 100vh;
        background: #f8f9fa;
      }

      .main-content {
        padding: 24px;
      }

      .content-header {
        display: flex;
        justify-content: space-between;
        align-items: center;
        margin-bottom: 24px;

        h1 {
          font-size: 24px;
          font-weight: 600;
          margin: 0;
        }
      }

      .switch-btn {
        display: flex;
        align-items: center;
        gap: 8px;
        padding: 8px 16px;
        background: #f8f9fa;
        border: 1px solid #dee2e6;
        border-radius: 8px;
        color: #495057;
        font-size: 14px;
        cursor: pointer;
        transition: all 0.2s ease;

        &:hover {
          background: #e9ecef;
        }

        i {
          font-size: 16px;
        }
      }

      .profile-grid {
        display: grid;
        grid-template-columns: 1fr 400px;
        gap: 24px;
      }

      .profile-section,
      .wallet-section {
        background: white;
        border-radius: 12px;
        padding: 24px;
        box-shadow: 0 1px 3px rgba(0, 0, 0, 0.1);
      }

      .profile-header {
        display: flex;
        gap: 16px;
        margin-bottom: 24px;
      }

      .avatar-container {
        width: 80px;
        height: 80px;
        border-radius: 50%;
        overflow: hidden;
        background: #f8f9fa;
      }

      .avatar {
        width: 100%;
        height: 100%;
        object-fit: cover;
      }

      .profile-info {
        h2 {
          font-size: 20px;
          font-weight: 600;
          margin: 0 0 4px;
        }

        .email {
          color: #6c757d;
          margin: 0;
          font-size: 14px;
        }
      }

      .profile-details {
        .detail-group {
          margin-bottom: 20px;

          label {
            display: block;
            font-size: 14px;
            color: #6c757d;
            margin-bottom: 4px;
          }

          p {
            margin: 0;
            font-size: 16px;
            color: #212529;
          }
        }
      }

      .skills-container {
        display: flex;
        flex-wrap: wrap;
        gap: 8px;
      }

      .skill-tag {
        padding: 4px 12px;
        background: #f8f9fa;
        border-radius: 16px;
        font-size: 14px;
        color: #495057;
      }

      .verification-badge {
        display: inline-flex;
        align-items: center;
        gap: 8px;
        padding: 8px 16px;
        background: #e6f4ea;
        color: #1e7e34;
        border-radius: 20px;
        font-size: 14px;
        margin-bottom: 20px;

        i {
          color: #1e7e34;
        }
      }

      .edit-btn {
        width: 100%;
        padding: 12px;
        background: #f8f9fa;
        border: 1px solid #dee2e6;
        border-radius: 8px;
        color: #495057;
        font-size: 14px;
        cursor: pointer;
        transition: all 0.2s ease;

        &:hover {
          background: #e9ecef;
        }
      }

      .wallet-section {
        h2 {
          font-size: 20px;
          font-weight: 600;
          margin: 0 0 4px;
        }

        .wallet-subtitle {
          color: #6c757d;
          font-size: 14px;
          margin: 0 0 24px;
        }
      }

      .balance-card {
        background: #f8f9fa;
        border-radius: 12px;
        padding: 20px;
        margin-bottom: 24px;

        .balance-header {
          display: flex;
          justify-content: space-between;
          align-items: center;
          margin-bottom: 8px;
          color: #6c757d;
          font-size: 14px;

          i {
            font-size: 20px;
          }
        }

        .balance-amount {
          font-size: 32px;
          font-weight: 600;
          color: #212529;
        }
      }

      .cashout-section {
        margin-bottom: 24px;

        label {
          display: block;
          font-size: 14px;
          color: #6c757d;
          margin-bottom: 8px;
        }

        .input-group {
          display: flex;
          gap: 8px;
        }

        .cashout-input {
          flex: 1;
          padding: 8px 12px;
          border: 1px solid #dee2e6;
          border-radius: 8px;
          font-size: 14px;

          &:focus {
            outline: none;
            border-color: #0066ff;
          }
        }

        .cashout-btn {
          padding: 8px 16px;
          background: #0066ff;
          color: white;
          border: none;
          border-radius: 8px;
          font-size: 14px;
          cursor: pointer;
          transition: all 0.2s ease;

          &:hover {
            background: #0052cc;
          }

          &:disabled {
            background: #e9ecef;
            cursor: not-allowed;
          }
        }
      }

      .wallet-actions {
        display: flex;
        flex-direction: column;
        gap: 8px;

        .action-btn {
          display: flex;
          align-items: center;
          justify-content: space-between;
          width: 100%;
          padding: 12px;
          background: white;
          border: 1px solid #dee2e6;
          border-radius: 8px;
          color: #495057;
          font-size: 14px;
          cursor: pointer;
          transition: all 0.2s ease;

          &:hover {
            background: #f8f9fa;
          }

          i:first-child {
            margin-right: 8px;
          }
        }
      }

      @media (max-width: 1200px) {
        .profile-grid {
          grid-template-columns: 1fr;
        }
      }

      @media (max-width: 768px) {
        .dashboard-container {
          grid-template-columns: 1fr;
        }

        .main-content {
          padding: 16px;
        }

        .content-header {
          flex-direction: column;
          align-items: flex-start;
          gap: 16px;
        }

        .switch-btn {
          width: 100%;
          justify-content: center;
        }
      }
    `,
  ],
})
export class ProfileComponent {
  profile: any = {
    name: 'Sarah Johnson',
    email: 'sarah.johnson@example.com',
    company: 'TechCorp Inc.',
    role: 'Senior Software Engineer',
    bio: 'Experienced software engineer with a passion for mentoring and conducting technical interviews.',
    skills: ['JavaScript', 'React', 'Node.js', 'Python', 'System Design'],
    isVerified: true,
  };

  wallet: any = {
    balance: 1250.75,
    linkedAccounts: true,
  };

  cashoutAmount = 0;

  constructor(private router: Router) {}

  switchToCandidate() {
    this.router.navigate(['/candidate-dashboard']);
  }

  isValidCashout(): boolean {
    return this.cashoutAmount > 0 && this.cashoutAmount <= this.wallet.balance;
  }
}
