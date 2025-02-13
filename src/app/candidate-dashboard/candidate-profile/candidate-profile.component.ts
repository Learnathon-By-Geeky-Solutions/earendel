import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import  { Router } from '@angular/router';

@Component({
  selector: 'app-candidate-profile',
  standalone: true,
  imports: [CommonModule, FormsModule],
  template: `
    <div class="profile-container">
      <div class="profile-header">
        <div class="avatar-section">
          <div class="avatar-container">
            <img
              [src]="profile.avatarUrl || 'https://via.placeholder.com/150'"
              alt="Profile picture"
              class="avatar"
            />
            <button class="change-avatar">
              <i class="bi bi-camera"></i>
            </button>
          </div>
        </div>
        <div class="header-actions">
          <button class="switch-btn" (click)="switchToInterviewer()">
            <i class="bi bi-arrow-repeat"></i>
            Switch to Interviewer
          </button>
        </div>
      </div>

      <form class="profile-form" (ngSubmit)="updateProfile()">
        <div class="form-grid">
          <div class="form-group">
            <label for="name">Full Name</label>
            <input
              type="text"
              id="name"
              [(ngModel)]="profile.name"
              name="name"
              class="form-input"
            />
          </div>

          <div class="form-group">
            <label for="email">Email</label>
            <input
              type="email"
              id="email"
              [(ngModel)]="profile.email"
              name="email"
              class="form-input"
              disabled
            />
          </div>

          <div class="form-group">
            <label for="phone">Phone Number</label>
            <input
              type="tel"
              id="phone"
              [(ngModel)]="profile.phone"
              name="phone"
              class="form-input"
            />
          </div>

          <div class="form-group">
            <label for="company">Current Company</label>
            <input
              type="text"
              id="company"
              [(ngModel)]="profile.currentCompany"
              name="company"
              class="form-input"
            />
          </div>

          <div class="form-group">
            <label for="experience">Years of Experience</label>
            <input
              type="text"
              id="experience"
              [(ngModel)]="profile.experience"
              name="experience"
              class="form-input"
            />
          </div>
        </div>

        <div class="form-group">
          <label>Skills</label>
          <div class="skills-container">
            <span *ngFor="let skill of profile.skills" class="skill-tag">
              {{ skill }}
              <button
                type="button"
                class="remove-skill"
                (click)="removeSkill(skill)"
              >
                <i class="bi bi-x"></i>
              </button>
            </span>
            <input
              type="text"
              placeholder="Add a skill"
              class="skill-input"
              (keydown.enter)="addSkill($event)"
            />
          </div>
        </div>

        <div class="form-actions">
          <button type="submit" class="save-btn">Save Changes</button>
        </div>
      </form>

      <div class="security-section">
        <h3>Security</h3>
        <form class="password-form" (ngSubmit)="updatePassword()">
          <div class="form-group">
            <label for="currentPassword">Current Password</label>
            <input
              type="password"
              id="currentPassword"
              [(ngModel)]="passwordForm.current"
              name="currentPassword"
              class="form-input"
            />
          </div>

          <div class="form-group">
            <label for="newPassword">New Password</label>
            <input
              type="password"
              id="newPassword"
              [(ngModel)]="passwordForm.new"
              name="newPassword"
              class="form-input"
            />
          </div>

          <div class="form-group">
            <label for="confirmPassword">Confirm New Password</label>
            <input
              type="password"
              id="confirmPassword"
              [(ngModel)]="passwordForm.confirm"
              name="confirmPassword"
              class="form-input"
            />
          </div>

          <button type="submit" class="update-password-btn">
            Update Password
          </button>
        </form>
      </div>
    </div>
  `,
  styles: [
    `
      .profile-container {
        padding: 24px;
        max-width: 800px;
        margin: 0 auto;
      }

      .profile-header {
        display: flex;
        justify-content: space-between;
        align-items: center;
        margin-bottom: 32px;
      }

      .avatar-section {
        display: flex;
        align-items: center;
        gap: 24px;
      }

      .avatar-container {
        position: relative;
        width: 100px;
        height: 100px;
        border-radius: 50%;
        overflow: hidden;
        background: #f8f9fa;
      }

      .avatar {
        width: 100%;
        height: 100%;
        object-fit: cover;
      }

      .change-avatar {
        position: absolute;
        bottom: 0;
        left: 0;
        right: 0;
        background: rgba(0, 0, 0, 0.6);
        color: white;
        border: none;
        padding: 8px;
        cursor: pointer;
        transition: all 0.2s ease;

        &:hover {
          background: rgba(0, 0, 0, 0.8);
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
      }

      .profile-form {
        background: white;
        border-radius: 12px;
        padding: 24px;
        box-shadow: 0 1px 3px rgba(0, 0, 0, 0.1);
        margin-bottom: 24px;
      }

      .form-grid {
        display: grid;
        grid-template-columns: repeat(2, 1fr);
        gap: 24px;
        margin-bottom: 24px;
      }

      .form-group {
        display: flex;
        flex-direction: column;
        gap: 8px;

        label {
          font-size: 14px;
          color: #666;
        }
      }

      .form-input {
        padding: 10px;
        border: 1px solid #dee2e6;
        border-radius: 8px;
        font-size: 14px;

        &:focus {
          outline: none;
          border-color: #0066ff;
        }

        &:disabled {
          background: #f8f9fa;
          cursor: not-allowed;
        }
      }

      .skills-container {
        display: flex;
        flex-wrap: wrap;
        gap: 8px;
        padding: 8px;
        border: 1px solid #dee2e6;
        border-radius: 8px;
      }

      .skill-tag {
        display: flex;
        align-items: center;
        gap: 4px;
        padding: 4px 8px;
        background: #f8f9fa;
        border-radius: 16px;
        font-size: 14px;
        color: #666;

        .remove-skill {
          border: none;
          background: none;
          padding: 0;
          color: #999;
          cursor: pointer;
          display: flex;
          align-items: center;

          &:hover {
            color: #dc3545;
          }
        }
      }

      .skill-input {
        border: none;
        padding: 4px;
        font-size: 14px;
        flex: 1;
        min-width: 100px;

        &:focus {
          outline: none;
        }
      }

      .form-actions {
        display: flex;
        justify-content: flex-end;
      }

      .save-btn {
        padding: 10px 24px;
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
      }

      .security-section {
        background: white;
        border-radius: 12px;
        padding: 24px;
        box-shadow: 0 1px 3px rgba(0, 0, 0, 0.1);

        h3 {
          font-size: 18px;
          font-weight: 600;
          margin: 0 0 24px;
          color: #333;
        }
      }

      .password-form {
        display: flex;
        flex-direction: column;
        gap: 16px;
        max-width: 400px;
      }

      .update-password-btn {
        padding: 10px;
        background: #f8f9fa;
        color: #666;
        border: 1px solid #dee2e6;
        border-radius: 8px;
        font-size: 14px;
        cursor: pointer;
        transition: all 0.2s ease;

        &:hover {
          background: #e9ecef;
        }
      }

      @media (max-width: 768px) {
        .profile-container {
          padding: 16px;
        }

        .profile-header {
          flex-direction: column;
          align-items: center;
          gap: 16px;
          text-align: center;
        }

        .form-grid {
          grid-template-columns: 1fr;
        }
      }
    `,
  ],
})
export class CandidateProfileComponent {
  profile: any = {
    name: 'John Doe',
    email: 'john@example.com',
    phone: '+1 234 567 8900',
    currentCompany: 'Tech Corp',
    experience: '5 years',
    skills: ['JavaScript', 'React', 'Node.js', 'TypeScript'],
  };

  passwordForm = {
    current: '',
    new: '',
    confirm: '',
  };

  constructor(private router: Router) {}

  switchToInterviewer() {
    this.router.navigate(['/candidate-dashboard/interviewer-registration']);
    // this.router.navigate(['/interviewer-dashboard']);
  }

  addSkill(event: any) {
    const input = event.target as HTMLInputElement;
    const skill = input.value.trim();

    if (skill && !this.profile.skills.includes(skill)) {
      this.profile.skills.push(skill);
      input.value = '';
    }

    event.preventDefault();
  }

  removeSkill(skill: string) {
    this.profile.skills = this.profile.skills.filter((s: any) => s !== skill);
  }

  updateProfile() {
    // Implement profile update logic
  }

  updatePassword() {
    // Implement password update logic
  }
}
