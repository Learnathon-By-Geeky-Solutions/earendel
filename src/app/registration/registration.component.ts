import { Component, AfterViewInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { MatTabsModule } from '@angular/material/tabs';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { Router } from '@angular/router';
import { environment } from '../../environments/environment';

@Component({
  selector: 'app-registration',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    MatTabsModule,
    MatInputModule,
    MatButtonModule,
    MatIconModule,
  ],
  template: `
    <div
      class="container-fluid vh-100 d-flex align-items-center justify-content-center bg-light"
    >
      <div class="card shadow-lg" style="max-width: 400px; width: 100%;">
        <div class="card-body p-5">
          <h2 class="text-center mb-4">Create an Account</h2>
          <mat-tab-group
            mat-stretch-tabs="false"
            mat-align-tabs="center"
            animationDuration="0ms"
            (selectedIndexChange)="selectedTabIndex = $event"
          >
            <mat-tab label="User">
              <form (ngSubmit)="onSubmit('user')" class="mt-4">
                <mat-form-field appearance="outline" class="w-100 mb-3">
                  <mat-label>Email</mat-label>
                  <input
                    matInput
                    type="email"
                    [(ngModel)]="userEmail"
                    name="userEmail"
                    required
                  />
                </mat-form-field>
                <mat-form-field appearance="outline" class="w-100 mb-3">
                  <mat-label>Password</mat-label>
                  <input
                    matInput
                    type="password"
                    [(ngModel)]="userPassword"
                    name="userPassword"
                    required
                  />
                </mat-form-field>
                <button
                  mat-raised-button
                  color="primary"
                  class="w-100 mb-3"
                  type="submit"
                >
                  Register
                </button>
              </form>
              <div class="text-center">
                <p class="mb-3">Or continue with</p>
                <!-- Google Identity Services Interface -->
                <div
                  id="g_id_onload"
                  [attr.data-client_id]="googleClientId"
                  data-context="signin"
                  data-ux_mode="popup"
                  data-callback="handleCredentialResponse"
                  data-auto_prompt="false"
                ></div>
                <div class="google-icon-container">
                  <div
                    class="g_id_signin"
                    data-type="icon"
                    data-shape="circle"
                    data-theme="outline"
                    data-text="continue_with"
                    data-size="large"
                  ></div>
                </div>
              </div>
            </mat-tab>
            <mat-tab label="Hiring">
              <form (ngSubmit)="onSubmit('hiring')" class="mt-4">
                <mat-form-field appearance="outline" class="w-100 mb-3">
                  <mat-label>Email</mat-label>
                  <input
                    matInput
                    type="email"
                    [(ngModel)]="hiringEmail"
                    name="hiringEmail"
                    required
                  />
                </mat-form-field>
                <mat-form-field appearance="outline" class="w-100 mb-3">
                  <mat-label>Password</mat-label>
                  <input
                    matInput
                    type="password"
                    [(ngModel)]="hiringPassword"
                    name="hiringPassword"
                    required
                  />
                </mat-form-field>
                <button
                  mat-raised-button
                  color="primary"
                  class="w-100 mb-3"
                  type="submit"
                >
                  Register
                </button>
              </form>
            </mat-tab>
          </mat-tab-group>
          <p class="text-center mb-0 mt-3">
            Already have an account? <a href="/login">Login</a>
          </p>
        </div>
      </div>
    </div>
  `,
  styles: [
    `
      :host {
        display: block;
        height: 100vh;
        background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
      }
      .card {
        border-radius: 15px;
      }
      mat-form-field {
        width: 100%;
      }
      .g_id_signin,
      #g_id_onload {
        cursor: default !important;
      }
      /* Wrap the Google icon and center it */
      .google-icon-container {
        display: flex;
        justify-content: center;
        align-items: center;
      }
    `,
  ],
})
export class RegistrationComponent implements AfterViewInit {
  hiringEmail = '';
  hiringPassword = '';
  userEmail = '';
  userPassword = '';
  selectedTabIndex = 0; // 0: User, 1: Hiring

  constructor(private router: Router) {}

  ngAfterViewInit() {
    // Assign the global callback function for Google Identity Services
    (window as any).handleCredentialResponse =
      this.handleCredentialResponse.bind(this);
  }

  googleClientId = environment.googleClientId;

  onSubmit(type: 'hiring' | 'user') {
    console.log(
      `${type} registration:`,
      type === 'hiring' ? this.hiringEmail : this.userEmail
    );
  }

  // Decode JWT token function
  decodeJWTToken(token: string) {
    return JSON.parse(atob(token.split('.')[1]));
  }

  // Handle credential response from Google
  handleCredentialResponse(response: any) {
    console.log(response);
    const responsePayload = this.decodeJWTToken(response.credential);
    console.log(responsePayload);
    sessionStorage.setItem('loggedInUser', JSON.stringify(response.credential));
    window.location.href = '/candidate-dashboard';
  }

  socialLogin(provider: string) {
    console.log(`Social login with ${provider}`);
  }
}
