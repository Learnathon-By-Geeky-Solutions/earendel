import { Component, type OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { SidebarComponent } from '../sidebar/sidebar.component';
import { MatSnackBar } from '@angular/material/snack-bar';

@Component({
  selector: 'app-profile',
  standalone: true,
  imports: [CommonModule, FormsModule, SidebarComponent],
  template: `
    <div class="d-flex">
      <app-sidebar></app-sidebar>

      <main class="main-content bg-light">
        <div class="container-fluid py-4">
          <h1 class="h3 mb-4">Profile Settings</h1>

          <div class="row">
            <div class="col-md-6">
              <div class="card border-0 shadow-sm mb-4">
                <div class="card-body">
                  <h5 class="card-title mb-4">Personal Information</h5>
                  <form (ngSubmit)="updateProfile()">
                    <div class="mb-3">
                      <label for="name" class="form-label">Name</label>
                      <input
                        type="text"
                        class="form-control"
                        id="name"
                        [(ngModel)]="user.name"
                        name="name"
                        required
                      />
                    </div>
                    <div class="mb-3">
                      <label for="email" class="form-label">Email</label>
                      <input
                        type="email"
                        class="form-control"
                        id="email"
                        [value]="user.email"
                        disabled
                      />
                    </div>
                    <button type="submit" class="btn btn-dark">
                      Update Profile
                    </button>
                  </form>
                </div>
              </div>

              <div class="card border-0 shadow-sm mb-4">
                <div class="card-body">
                  <h5 class="card-title mb-4">Change Password</h5>
                  <form (ngSubmit)="changePassword()">
                    <div class="mb-3">
                      <label for="currentPassword" class="form-label"
                        >Current Password</label
                      >
                      <input
                        type="password"
                        class="form-control"
                        id="currentPassword"
                        [(ngModel)]="passwordForm.currentPassword"
                        name="currentPassword"
                        required
                      />
                    </div>
                    <div class="mb-3">
                      <label for="newPassword" class="form-label"
                        >New Password</label
                      >
                      <input
                        type="password"
                        class="form-control"
                        id="newPassword"
                        [(ngModel)]="passwordForm.newPassword"
                        name="newPassword"
                        required
                      />
                    </div>
                    <div class="mb-3">
                      <label for="confirmPassword" class="form-label"
                        >Confirm New Password</label
                      >
                      <input
                        type="password"
                        class="form-control"
                        id="confirmPassword"
                        [(ngModel)]="passwordForm.confirmPassword"
                        name="confirmPassword"
                        required
                      />
                    </div>
                    <button type="submit" class="btn btn-dark">
                      Change Password
                    </button>
                  </form>
                </div>
              </div>
            </div>

            <div class="col-md-6">
              <div class="card border-0 shadow-sm mb-4">
                <div class="card-body">
                  <h5 class="card-title mb-4">Profile Picture</h5>
                  <div class="text-center mb-3">
                    <img
                      [src]="user.imageUrl"
                      alt="Profile Picture"
                      class="rounded-circle img-thumbnail"
                      style="width: 150px; height: 150px; object-fit: cover;"
                    />
                  </div>
                  <div class="mb-3">
                    <label for="profileImage" class="form-label"
                      >Upload New Picture</label
                    >
                    <input
                      type="file"
                      class="form-control"
                      id="profileImage"
                      (change)="onFileSelected($event)"
                      accept="image/*"
                    />
                  </div>
                  <button
                    type="button"
                    class="btn btn-dark"
                    (click)="uploadImage()"
                    [disabled]="!selectedFile"
                  >
                    Upload Image
                  </button>
                </div>
              </div>
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

      @media (max-width: 991.98px) {
        .main-content {
          margin-left: 0;
          width: 100%;
        }
      }
    `,
  ],
})
export class AdminProfileComponent implements OnInit {
  user = {
    name: 'John Doe',
    email: 'john@example.com',
    imageUrl: 'https://via.placeholder.com/150',
  };

  passwordForm = {
    currentPassword: '',
    newPassword: '',
    confirmPassword: '',
  };

  selectedFile: File | null = null;

  constructor(private snackBar: MatSnackBar) {}

  ngOnInit(): void {
    // In a real application, you would fetch the user's profile data here
  }

  updateProfile() {
    // In a real application, you would send an API request to update the profile
    this.snackBar.open('Profile updated successfully', 'Close', {
      duration: 3000,
    });
  }

  changePassword() {
    if (this.passwordForm.newPassword !== this.passwordForm.confirmPassword) {
      this.snackBar.open('New passwords do not match', 'Close', {
        duration: 3000,
      });
      return;
    }
    // In a real application, you would send an API request to change the password
    this.snackBar.open('Password changed successfully', 'Close', {
      duration: 3000,
    });
    this.passwordForm = {
      currentPassword: '',
      newPassword: '',
      confirmPassword: '',
    };
  }

  onFileSelected(event: Event) {
    const input = event.target as HTMLInputElement;
    if (input.files && input.files.length > 0) {
      this.selectedFile = input.files[0];
    }
  }

  uploadImage() {
    if (this.selectedFile) {
      // In a real application, you would upload the image to a server and get a URL back
      const reader = new FileReader();
      reader.onload = (e) => {
        this.user.imageUrl = e.target?.result as string;
        this.snackBar.open('Profile picture updated successfully', 'Close', {
          duration: 3000,
        });
      };
      reader.readAsDataURL(this.selectedFile);
    }
  }
}
