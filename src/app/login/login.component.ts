import { AfterViewInit, Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { Router, RouterLink } from '@angular/router';
import { environment } from '../../environments/environment';
import { HttpClientModule } from '@angular/common/http';
import { LoginSignupService } from '../shared/services/login-signup.service';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    MatInputModule,
    MatButtonModule,
    MatIconModule,
    RouterLink,
    HttpClientModule,
    ReactiveFormsModule,
    FormsModule,
    MatSnackBarModule, // Import MatSnackBarModule
  ],
  template: `
    <div
      class="container-fluid vh-100 d-flex align-items-center justify-content-center bg-light"
    >
      <div class="card shadow-lg" style="max-width: 400px; width: 100%;">
        <div class="card-body p-5">
          <h2 class="text-center mb-4">Login</h2>
          <form (ngSubmit)="onSubmit()">
            <mat-form-field appearance="outline" class="w-100 mb-3">
              <mat-label>Email</mat-label>
              <input
                matInput
                type="email"
                [(ngModel)]="email"
                name="email"
                required
              />
            </mat-form-field>
            <mat-form-field appearance="outline" class="w-100 mb-3">
              <mat-label>Password</mat-label>
              <input
                matInput
                type="password"
                [(ngModel)]="password"
                name="password"
                required
              />
            </mat-form-field>
            <button
              mat-raised-button
              color="primary"
              class="w-100 mb-3"
              type="submit"
            >
              Login
            </button>
          </form>
          <div class="text-center">
            <p class="mb-3">Or login with</p>

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
            <p class="mb-0 mt-3">
              Don't have an account? <a routerLink="/register">Register</a>
            </p>
          </div>
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
      .google-icon-container {
        display: flex;
        justify-content: center;
        align-items: center;
      }
    `,
  ],
})
export class LoginComponent implements AfterViewInit {
  email = '';
  password = '';
  token!: string;
  googleClientId = environment.googleClientId;

  constructor(
    private formBuilder: FormBuilder,
    private router: Router,
    private loginService: LoginSignupService,
    private snackBar: MatSnackBar // Inject MatSnackBar
  ) {}

  ngAfterViewInit() {
    (window as any).handleCredentialResponse =
      this.handleCredentialResponse.bind(this);
  }

  // Handle credential response from Google
  handleCredentialResponse(response: any) {
    this.token = response.credential;
    this.loginService.googleLogin(this.token).subscribe(
      (data) => {
        sessionStorage.setItem('loggedInUser', JSON.stringify(data));
        this.snackBar.open('Login successful!', 'Close', {
          duration: 3000,
          panelClass: ['snack-bar-success'],
        });
        this.router.navigateByUrl('/candidate-dashboard');
      },
      (error) => {
        this.snackBar.open('Google login failed. Please try again.', 'Close', {
          duration: 3000,
          panelClass: ['snack-bar-error'],
        });
      }
    );
  }

  onSubmit() {
    if (!this.email || !this.password) {
      this.snackBar.open('Please enter both email and password.', 'Close', {
        duration: 3000,
        panelClass: ['snack-bar-error'],
      });
      return;
    }

    this.loginService
      .userLogin({ email: this.email, password: this.password })
      .subscribe(
        (data: any) => {
          if (data && data.token && data.refreshToken) {
            sessionStorage.setItem('loggedInUser', JSON.stringify(data));

            this.snackBar.open('Login successful!', 'Close', {
              duration: 3000,
              panelClass: ['snack-bar-success'],
            });

            this.router.navigateByUrl('/candidate-dashboard');
          } else {
            this.snackBar.open(
              'Invalid credentials. Please try again.',
              'Close',
              {
                duration: 3000,
                panelClass: ['snack-bar-error'],
              }
            );
          }
        },
        (error: any) => {
          this.snackBar.open(
            'Login failed. Please check your credentials.',
            'Close',
            {
              duration: 3000,
              panelClass: ['snack-bar-error'],
            }
          );
        }
      );
  }
}
