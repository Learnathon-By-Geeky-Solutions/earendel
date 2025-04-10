import { Component, AfterViewInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import {
  FormBuilder,
  FormGroup,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { MatTabsModule } from '@angular/material/tabs';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { Router, RouterLink } from '@angular/router';
import { environment } from '../../environments/environment';
import { LoginSignupService } from '../shared/services/login-signup.service';
import { HttpClientModule } from '@angular/common/http';
import { MatSnackBar } from '@angular/material/snack-bar';

@Component({
  selector: 'app-registration',
  standalone: true,
  imports: [
    CommonModule,
    MatTabsModule,
    MatInputModule,
    MatButtonModule,
    RouterLink,
    MatIconModule,
    HttpClientModule,
    ReactiveFormsModule,
    MatButtonModule,
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
            <!-- User Registration Tab -->
            <mat-tab label="User">
              <!-- Summary error message for user form -->
              <div *ngIf="userFormError" class="error-summary">
                {{ userFormError }}
              </div>
              <form
                [formGroup]="userForm"
                (ngSubmit)="onSubmit('user')"
                class="mt-4"
              >
                <mat-form-field appearance="outline" class="w-100 mb-3">
                  <mat-label>Username</mat-label>
                  <input
                    matInput
                    type="text"
                    formControlName="userName"
                    required
                  />
                  <mat-error
                    *ngIf="
                      userForm.get('userName')?.invalid &&
                      userForm.get('userName')?.touched
                    "
                  >
                    Username is required.
                  </mat-error>
                </mat-form-field>

                <mat-form-field appearance="outline" class="w-100 mb-3">
                  <mat-label>Email</mat-label>
                  <input
                    matInput
                    type="email"
                    formControlName="email"
                    required
                  />
                  <mat-error
                    *ngIf="userForm.get('email')?.hasError('required')"
                  >
                    Email is required.
                  </mat-error>
                  <mat-error *ngIf="userForm.get('email')?.hasError('email')">
                    Invalid email format.
                  </mat-error>
                </mat-form-field>

                <mat-form-field appearance="outline" class="w-100 mb-3">
                  <mat-label>Password</mat-label>
                  <input
                    matInput
                    type="password"
                    formControlName="password"
                    required
                  />
                  <mat-error
                    *ngIf="
                      userForm.get('password')?.invalid &&
                      userForm.get('password')?.touched
                    "
                  >
                    Password must be at least 6 characters long.
                  </mat-error>
                </mat-form-field>

                <mat-form-field appearance="outline" class="w-100 mb-3">
                  <mat-label>Confirm Password</mat-label>
                  <input
                    matInput
                    type="password"
                    formControlName="confirmPassword"
                    required
                  />
                  <mat-error
                    *ngIf="
                      userForm.get('confirmPassword')?.hasError('mismatch')
                    "
                  >
                    Passwords do not match.
                  </mat-error>
                </mat-form-field>

                <button
                  mat-raised-button
                  color="primary"
                  class="w-100 mb-3"
                  type="submit"
                  [disabled]="userForm.invalid"
                >
                  Register
                </button>
              </form>

              <!-- Google Login -->
              <div class="text-center">
                <p class="mb-3">Or continue with</p>
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

            <!-- Hiring Registration Tab -->
            <mat-tab label="Hiring">
              <!-- Summary error message for hiring form -->
              <div *ngIf="hiringFormError" class="error-summary">
                {{ hiringFormError }}
              </div>
              <form
                [formGroup]="hiringForm"
                (ngSubmit)="onSubmit('hiring')"
                class="mt-4"
              >
                <mat-form-field appearance="outline" class="w-100 mb-3">
                  <mat-label>Username</mat-label>
                  <input
                    matInput
                    type="text"
                    formControlName="userName"
                    required
                  />
                  <mat-error
                    *ngIf="
                      hiringForm.get('userName')?.invalid &&
                      hiringForm.get('userName')?.touched
                    "
                  >
                    Username is required.
                  </mat-error>
                </mat-form-field>

                <mat-form-field appearance="outline" class="w-100 mb-3">
                  <mat-label>Email</mat-label>
                  <input
                    matInput
                    type="email"
                    formControlName="email"
                    required
                  />
                  <mat-error
                    *ngIf="hiringForm.get('email')?.hasError('required')"
                  >
                    Email is required.
                  </mat-error>
                  <mat-error *ngIf="hiringForm.get('email')?.hasError('email')">
                    Invalid email format.
                  </mat-error>
                </mat-form-field>

                <mat-form-field appearance="outline" class="w-100 mb-3">
                  <mat-label>Password</mat-label>
                  <input
                    matInput
                    type="password"
                    formControlName="password"
                    required
                  />
                  <mat-error
                    *ngIf="
                      hiringForm.get('password')?.invalid &&
                      hiringForm.get('password')?.touched
                    "
                  >
                    Password must be at least 6 characters long.
                  </mat-error>
                </mat-form-field>

                <mat-form-field appearance="outline" class="w-100 mb-3">
                  <mat-label>Confirm Password</mat-label>
                  <input
                    matInput
                    type="password"
                    formControlName="confirmPassword"
                    required
                  />
                  <mat-error
                    *ngIf="
                      hiringForm.get('confirmPassword')?.hasError('mismatch')
                    "
                  >
                    Passwords do not match.
                  </mat-error>
                </mat-form-field>

                <button
                  mat-raised-button
                  color="primary"
                  class="w-100 mb-3"
                  type="submit"
                  [disabled]="hiringForm.invalid"
                >
                  Register
                </button>
              </form>
            </mat-tab>
          </mat-tab-group>

          <!-- Already Have an Account -->
          <p class="text-center mb-0 mt-3">
            Already have an account? <a routerLink="/login">Login</a>
          </p>
        </div>
      </div>
    </div>
  `,
  styles: [
    `
      .google-icon-container {
        display: flex;
        justify-content: center;
        align-items: center;
        padding: 10px;
      }

      :host {
        display: block;
        min-height: 100vh;
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
      .error-summary {
        color: #f44336;
        margin-bottom: 1rem;
        font-size: 0.9rem;
        text-align: center;
      }
    `,
  ],
})
export class RegistrationComponent implements AfterViewInit {
  userForm: FormGroup;
  hiringForm: FormGroup;
  selectedTabIndex = 0;
  googleClientId = environment.googleClientId;
  token!: string;
  userFormError: string = '';
  hiringFormError: string = '';

  constructor(
    private fb: FormBuilder,
    private router: Router,
    private loginService: LoginSignupService,
    private snackBar: MatSnackBar // Inject MatSnackBar
  ) {
    this.userForm = this.createForm();
    this.hiringForm = this.createForm();
  }

  ngAfterViewInit() {
    this.loadGoogleScript()
      .then(() => {
        this.initializeGoogleSignIn();
      })
      .catch((error) => {
        console.error('Google Sign-In failed to load:', error);
      });
  }

  private loadGoogleScript(): Promise<void> {
    return new Promise((resolve, reject) => {
      if (typeof window === 'undefined')
        return reject('Window object not available');

      // Add global type declaration for Google
      (window as any).handleCredentialResponse =
        this.handleCredentialResponse.bind(this);

      const script = document.createElement('script');
      script.src = 'https://accounts.google.com/gsi/client';
      script.async = true;
      script.defer = true;
      script.onload = () => resolve();
      script.onerror = (error) => reject(error);
      document.head.appendChild(script);
    });
  }

  private initializeGoogleSignIn() {
    // Type assertion for Google API
    const google = (window as any).google;

    if (!google || !google.accounts || !google.accounts.id) {
      console.error('Google Identity Services not loaded');
      return;
    }

    google.accounts.id.initialize({
      client_id: this.googleClientId,
      callback: this.handleCredentialResponse.bind(this),
      context: 'signin',
      ux_mode: 'popup',
    });

    google.accounts.id.renderButton(
      document.getElementById('google-signin-button'),
      {
        type: 'icon',
        shape: 'circle',
        theme: 'outline',
        text: 'continue_with',
        size: 'large',
      }
    );
  }

  createForm(): FormGroup {
    return this.fb.group(
      {
        userName: ['', Validators.required],
        email: ['', [Validators.required, Validators.email]],
        password: ['', [Validators.required, Validators.minLength(6)]],
        confirmPassword: [''],
      },
      { validator: this.passwordMatchValidator }
    );
  }

  passwordMatchValidator(form: FormGroup) {
    return form.get('password')?.value === form.get('confirmPassword')?.value
      ? null
      : { mismatch: true };
  }

  onSubmit(type: 'hiring' | 'user') {
    if (type === 'user') {
      if (this.userForm.valid) {
        const registrationData = { ...this.userForm.value, role: 0 };
        this.userFormError = '';

        // Proceed with user registration...
        this.loginService.userRegistration(registrationData).subscribe(
          (data: any) => {
            if (data && data.userId) {
              this.snackBar.open('Registration successful!', 'Close', {
                duration: 3000, // duration in milliseconds
                panelClass: ['snack-bar-success'], // You can define custom styles in your CSS
              });

              // Redirect to the login page after successful registration
              this.router.navigateByUrl('/login');
            } else {
              this.snackBar.open(
                'Registration failed, please try again.',
                'Close',
                {
                  duration: 3000,
                  panelClass: ['snack-bar-error'],
                }
              );
            }
          },
          (error) => {
            // Handle error response and display messages
            console.log('Error response:', error);
            if (error.error.errors) {
              // Check if there are specific errors like duplicate username or email
              this.userFormError = error.error.errors.join('. ');
            } else {
              // Generic error message
              this.userFormError =
                'An error occurred while registering. Please try again later.';
            }
          }
        );

        // Reset form values after submission (even if error occurs)
        this.userForm.reset();
      } else {
        this.userFormError = 'Please fix the errors in the form.';
        // Mark all controls as touched to trigger validation messages
        Object.values(this.userForm.controls).forEach((control) => {
          control.markAsTouched();
        });
      }
    } else if (type === 'hiring') {
      if (this.hiringForm.valid) {
        const registrationData = { ...this.hiringForm.value, role: 1 };

        console.log('Hiring Form Data:', registrationData);
        this.hiringFormError = '';

        // Proceed with hiring registration...
        this.loginService.userRegistration(registrationData).subscribe(
          (data) => {
            console.log(data);
            // Redirect to login or another page after successful registration
            // this.router.navigateByUrl('/login');
          },
          (error) => {
            // Handle error response and display messages
            console.log('Error response:', error);
            if (error.error.errors) {
              // Check if there are specific errors like duplicate username or email
              this.hiringFormError = error.error.errors.join('. ');
            } else {
              // Generic error message
              this.hiringFormError =
                'An error occurred while registering. Please try again later.';
            }
          }
        );

        // Reset form values after submission (even if error occurs)
        this.hiringForm.reset();
      } else {
        this.hiringFormError = 'Please fix the errors in the form.';
        Object.values(this.hiringForm.controls).forEach((control) => {
          control.markAsTouched();
        });
      }
    }
  }

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
}
