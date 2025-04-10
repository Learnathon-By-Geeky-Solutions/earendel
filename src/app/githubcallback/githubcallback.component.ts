import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { LoginSignupService } from '../shared/services/login-signup.service';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { HttpClientModule } from '@angular/common/http';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';

@Component({
  selector: 'app-github-callback',
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
  ],
  template: `
    <div class="container">
      <h2>GitHub Callback</h2>
      <p *ngIf="code">Received Code: {{ code }}</p>
      <p *ngIf="!code">No code received</p>
    </div>
  `,
  styles: [
    `
      .container {
        margin: 40px;
        text-align: center;
      }
    `,
  ],
})
export class GithubcallbackComponent implements OnInit {
  code: string | null = null;

  constructor(
    private route: ActivatedRoute,
    private loginService: LoginSignupService,
    private router: Router,
    private snackBar: MatSnackBar
  ) {}

  ngOnInit(): void {
    // Option 1: Snapshot (simpler, works if route doesn't change dynamically)
    this.code = this.route.snapshot.queryParamMap.get('code');
    if (this.code) {
      this.loginService.githubLogin(this.code).subscribe(
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
            this.router.navigateByUrl('/login');
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
          this.snackBar.open(
            'Github login failed. Please try again.',
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
}
