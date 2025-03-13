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
              Don't have an account?
              <a href="/register">Register</a>
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
      /* Wrap the Google icon and center it */
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

  constructor(
    private formBuilder: FormBuilder,
    private router: Router,
    private loginService: LoginSignupService
  ) {}

  ngAfterViewInit() {
    // Assign the global callback function for Google Identity Services
    (window as any).handleCredentialResponse =
      this.handleCredentialResponse.bind(this);
  }

  googleClientId = environment.googleClientId;
  // Decode JWT token function
  decodeJWTToken(token: string) {
    return JSON.parse(atob(token.split('.')[1]));
  }

  // Handle credential response from Google
  handleCredentialResponse(response: any) {
    this.token = response.credential;
    this.loginService.googleLogin(this.token).subscribe((data) => {
      console.log('Response:', data);
      sessionStorage.setItem('loggedInUser', JSON.stringify(data));
      this.router.navigateByUrl('/candidate-dashboard');
    });
  }
  onSubmit() {
    // Handle login logic here
    console.log('Login:', this.email);
  }

  socialLogin(provider: string) {
    // Handle social login logic here
    console.log(`Social login with ${provider}`);
  }
}
