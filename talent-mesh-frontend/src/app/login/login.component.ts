import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import  { Router } from '@angular/router';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
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
            <div class="d-flex justify-content-center gap-3 mb-3">
              <button mat-mini-fab color="warn" (click)="socialLogin('google')">
                <mat-icon>g_translate</mat-icon>
              </button>
              <button
                mat-mini-fab
                color="primary"
                (click)="socialLogin('linkedin')"
              >
                <mat-icon>linkedin</mat-icon>
              </button>
            </div>
            <p class="mb-0">
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
    `,
  ],
})
export class LoginComponent {
  email = '';
  password = '';

  constructor(private router: Router) {}

  onSubmit() {
    // Handle login logic here
    console.log('Login:', this.email);
  }

  socialLogin(provider: string) {
    // Handle social login logic here
    console.log(`Social login with ${provider}`);
  }

}
