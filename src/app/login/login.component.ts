import { AfterViewInit, Component, OnDestroy, OnInit } from '@angular/core';
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
import { BearAvatarComponent } from '../shared/components/bear-avatar/bear-avatar.component';
import { AnimatedInputComponent } from '../shared/components/animated-input/animated-input.component';
import { BearAnimationService } from '../shared/services/bear-animation.service';
import { Subscription } from 'rxjs';

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
    MatSnackBarModule,
    BearAvatarComponent,
    AnimatedInputComponent
  ],
  template: `
    <div
      class="container-fluid vh-100 d-flex align-items-center justify-content-center bg-light"
    >
      <div class="card shadow-lg" style="max-width: 400px; width: 100%;">
        <div class="card-body p-5">
          <h2 class="text-center mb-4">Login</h2>
          
          <!-- Bear Avatar -->
          <div class="d-flex justify-content-center mb-4">
            <div style="width: 130px; height: 130px;" class="position-relative">
              <div class="position-absolute top-0 start-0 bottom-0 end-0 d-flex align-items-center justify-content-center">
                <app-bear-avatar 
                  [currentImage]="bearAnimationService.currentBearImage || ''" 
                  [size]="130"
                ></app-bear-avatar>
              </div>
            </div>
          </div>
          
          <form (ngSubmit)="onSubmit()">
            <!-- Email Input with Animation -->
            <div class="w-100 mb-3">
              <app-animated-input
                type="email"
                placeholder="Email"
                name="email"
                [autocomplete]="'email'"
                [(ngModel)]="email"
                (ngModelChange)="onEmailChange()"
                (focus)="onEmailFocus()"
                (input)="updateAnimation()"
              ></app-animated-input>
            </div>
            
            <!-- Password Input with Animation and Toggle -->
            <div class="w-100 mb-3 position-relative">
              <app-animated-input
                [type]="showPassword ? 'text' : 'password'"
                placeholder="Password"
                name="password"
                [autocomplete]="'current-password'"
                [(ngModel)]="password"
                (focus)="onPasswordFocus()"
              ></app-animated-input>
              
              <!-- Password Toggle Button -->
              <button
                type="button"
                (click)="togglePassword()"
                [disabled]="bearAnimationService.isAnimating"
                class="btn position-absolute top-50 end-0 translate-middle-y me-2"
                style="background: none; border: none;"
              >
                <img 
                  [src]="showPassword ? '/assets/icons/eye_off.svg' : '/assets/icons/eye_on.svg'" 
                  [alt]="showPassword ? 'Hide password' : 'Show password'"
                  width="24"
                  height="24"
                  class="transition"
                  style="transition: transform 0.3s; transform: rotate(0); cursor: pointer;"
                  onmouseover="this.style.transform='scale(1.1)'"
                  onmouseout="this.style.transform='scale(1)'"
                />
              </button>
            </div>
            
            <button
              mat-raised-button
              color="primary"
              class="w-100 mb-3 py-2"
              type="submit"
              style="background-color: #ff731d;"
            >
              Login
            </button>
          </form>
          
          <div class="text-center">
            <p class="mb-3">Or login with</p>

            <!-- Google Identity Services -->
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

            <!-- Circular GitHub Button -->
            <button class="github-button" (click)="onGithubLogin()">
              <i class="bi bi-github"></i>
            </button>

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
      }
      .card {
        border-radius: 15px;
      }
      .transition {
        transition: all 0.3s ease;
      }
      .google-icon-container {
        display: flex;
        justify-content: center;
        align-items: center;
      }
      /* GitHub Button Styles */
      .github-button {
        background-color: #24292e;
        border: none;
        border-radius: 50%;
        width: 40px;
        height: 40px;
        color: white;
        font-size: 24px;
        display: inline-flex;
        justify-content: center;
        align-items: center;
        margin-top: 10px;
        cursor: pointer;
        transition: background-color 0.3s;
      }
      .github-button:hover {
        background-color: #444d56;
      }
    `,
  ],
})
export class LoginComponent implements AfterViewInit, OnInit, OnDestroy {
  email = '';
  password = '';
  token!: string;
  googleClientId = environment.googleClientId;
  githubClientId = environment.githubClientId;
  showPassword = false;
  
  private subscription = new Subscription();

  constructor(
    private formBuilder: FormBuilder,
    private router: Router,
    private loginService: LoginSignupService,
    private snackBar: MatSnackBar,
    public bearAnimationService: BearAnimationService
  ) {}

  ngOnInit(): void {
    // Set initial focus to EMAIL
    this.bearAnimationService.setCurrentFocus('EMAIL');
  }

  ngAfterViewInit() {
    // Initial animation update
    setTimeout(() => {
      this.updateAnimation();
    }, 100);
    
    this.loadGoogleScript()
      .then(() => {
        this.initializeGoogleSignIn();
      })
      .catch((error) => {
        console.error('Google Sign-In failed to load:', error);
      });
  }

  ngOnDestroy(): void {
    this.subscription.unsubscribe();
  }

  onEmailFocus(): void {
    this.bearAnimationService.setCurrentFocus('EMAIL');
    this.updateAnimation();
  }

  onPasswordFocus(): void {
    this.bearAnimationService.setCurrentFocus('PASSWORD');
    this.updateAnimation();
  }

  onEmailChange(): void {
    // Update animation when email changes - will be called for each keystroke with ngModelChange
    if (this.bearAnimationService.currentFocus === 'EMAIL') {
      this.updateAnimation();
    }
  }

  togglePassword(): void {
    if (!this.bearAnimationService.isAnimating) {
      this.showPassword = !this.showPassword;
      this.updateAnimation();
    }
  }

  public updateAnimation(): void {
    this.bearAnimationService.updateAnimation(this.email.length, this.showPassword);
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

  // Handle credential response from Google
  handleCredentialResponse(response: any) {
    this.token = response.credential;
    this.loginService.googleLogin(this.token).subscribe(
      (data) => {
        console.log(data.userId);
        if (
          data?.userId ===
          'Email is already registered with a different method.'
        ) {
          this.snackBar.open(
            'Email is already registered with a different method.',
            'Close',
            {
              duration: 3000,
              panelClass: ['snack-bar-error'],
            }
          );
        } else {
          sessionStorage.setItem('loggedInUser', JSON.stringify(data));
          this.snackBar.open('Login successful!', 'Close', {
            duration: 3000,
            panelClass: ['snack-bar-success'],
          });
          this.router.navigateByUrl('/candidate-dashboard');
        }
      },
      (error) => {
        this.snackBar.open('Google login failed. Please try again.', 'Close', {
          duration: 3000,
          panelClass: ['snack-bar-error'],
        });
      }
    );
  }

  // Function to redirect on GitHub button click
  onGithubLogin() {
    window.location.href = `https://github.com/login/oauth/authorize?client_id=${this.githubClientId}&scope=user`;
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
