import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-navbar',
  standalone: true,
  imports: [CommonModule],
  template: `
    <nav class="navbar navbar-expand-lg bg-white border-bottom">
      <div class="container-fluid px-4">
        <a class="navbar-brand fw-bold" href="#">TalentMest</a>

        <div class="collapse navbar-collapse">
          <ul class="navbar-nav me-auto">
            <li class="nav-item">
              <a class="nav-link active" href="#">Dashboard</a>
            </li>
            <li class="nav-item">
              <a class="nav-link" href="#">Interviews</a>
            </li>
            <li class="nav-item">
              <a class="nav-link" href="#">Candidates</a>
            </li>
            <li class="nav-item">
              <a class="nav-link" href="#">Reports</a>
            </li>
          </ul>

          <div class="d-flex align-items-center gap-4">
            <div class="credits d-flex align-items-center gap-2">
              <i class="bi bi-coin"></i>
              <span>Credits: 150</span>
            </div>
            <button class="btn btn-link p-0">
              <i class="bi bi-bell"></i>
            </button>
            <button class="btn btn-link p-0">
              <i class="bi bi-gear"></i>
            </button>
            <div class="d-flex align-items-center gap-2">
              <div
                class="avatar rounded-circle bg-light d-flex align-items-center justify-content-center"
                style="width: 32px; height: 32px;"
              >
                SW
              </div>
              <div class="d-flex flex-column">
                <span class="fw-medium">Sarah Wilson</span>
                <small class="text-muted">Tech Recruiter</small>
              </div>
            </div>
          </div>
        </div>
      </div>
    </nav>
  `,
  styles: [
    `
      .navbar {
        height: 64px;
      }
      .navbar-brand {
        font-size: 1.25rem;
      }
      .nav-link {
        color: #4b5563;
        padding: 0.5rem 1rem;
      }
      .nav-link.active {
        color: #000;
        font-weight: 500;
      }
      .credits {
        padding: 0.5rem 1rem;
        background: #f3f4f6;
        border-radius: 6px;
        font-weight: 500;
      }
      .btn-link {
        color: #4b5563;
      }
      .avatar {
        font-size: 0.875rem;
        color: #4b5563;
      }
    `,
  ],
})
export class NavbarComponent {}
