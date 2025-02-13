import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { MatTabsModule } from '@angular/material/tabs';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import  { Router } from '@angular/router';

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
          >
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
            </mat-tab>
          </mat-tab-group>
          <div class="text-center">
            <p class="mb-3">Or continue with</p>
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
              Already have an account?
              <a href="/login" >Login</a>
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
export class RegistrationComponent {
  hiringEmail = '';
  hiringPassword = '';
  userEmail = '';
  userPassword = '';

  constructor(private router: Router) {}

  onSubmit(type: 'hiring' | 'user') {
    // Handle registration logic here
    console.log(
      `${type} registration:`,
      type === 'hiring' ? this.hiringEmail : this.userEmail
    );
  }

  socialLogin(provider: string) {
    // Handle social login logic here
    console.log(`Social login with ${provider}`);
  }

}
