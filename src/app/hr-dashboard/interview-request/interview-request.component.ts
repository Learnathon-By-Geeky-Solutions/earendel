import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

interface Profile {
  icon: string;
  name: string;
}

interface Position {
  role: string;
  id: string;
  seniority: string;
  department: string;
  candidates: number;
}

@Component({
  selector: 'app-interview-request',
  standalone: true,
  imports: [CommonModule, FormsModule],
  template: `
    <div class="container-fluid">
      <!-- Header -->
      <header class="py-3 border-bottom">
        <div class="row align-items-center">
          <div class="col">
            <div class="d-flex align-items-center">
              <h1 class="h5 mb-0 me-4">TalentMest</h1>
              <nav class="nav">
                <a class="nav-link px-3" href="#">Internal Interviews</a>
                <a class="nav-link px-3" href="#">Outsourced Interviews</a>
                <a class="nav-link px-3" href="#">Assignments</a>
                <a class="nav-link px-3" href="#">Analytics</a>
              </nav>
            </div>
          </div>
          <div class="col-auto">
            <div class="d-flex align-items-center gap-3">
              <div class="position-relative">
                <input
                  type="search"
                  class="form-control"
                  placeholder="Search candidate name, profile etc."
                  style="width: 300px;"
                />
                <i
                  class="bi bi-search position-absolute top-50 end-0 translate-middle-y me-3"
                ></i>
              </div>
              <button class="btn btn-link p-0">
                <i class="bi bi-bell fs-5"></i>
              </button>
              <button class="btn btn-link p-0">
                <i class="bi bi-gear fs-5"></i>
              </button>
              <div class="avatar">
                <img
                  src="/assets/avatar.png"
                  alt="Profile"
                  class="rounded-circle"
                  width="32"
                  height="32"
                />
              </div>
            </div>
          </div>
        </div>
      </header>

      <!-- Main Content -->
      <div class="row g-4 py-4">
        <!-- Left Column -->
        <div class="col-8">
          <h2 class="h5 fw-bold mb-4">Select profile to request interviewer</h2>
          <div class="mb-4">
            <div class="position-relative">
              <input
                type="search"
                class="form-control form-control-lg"
                placeholder="Search profile"
              />
              <i
                class="bi bi-search position-absolute top-50 end-0 translate-middle-y me-3"
              ></i>
            </div>
          </div>
          <div class="row g-4">
            <div class="col-6 col-xl-3" *ngFor="let profile of profiles">
              <div class="card border-1 rounded-4 shadow-sm h-100">
                <div class="card-body text-center p-4">
                  <div class="rounded-3 bg-light d-inline-block p-3 mb-3">
                    <i [class]="profile.icon + ' fs-4'"></i>
                  </div>
                  <h3 class="card-title h6 mb-0">{{ profile.name }}</h3>
                </div>
              </div>
            </div>
          </div>
        </div>

        <!-- Right Column -->
        <div class="col-4">
          <h2 class="h5 fw-bold mb-4">
            Request an existing profile's interview
          </h2>
          <div class="d-flex flex-column gap-3">
            <div
              class="card border-1 rounded-4 shadow-sm"
              *ngFor="let position of positions"
            >
              <div class="card-body p-4">
                <div
                  class="d-flex justify-content-between align-items-start mb-2"
                >
                  <h3 class="h6 mb-1">
                    {{ position.role }} - {{ position.id }}
                  </h3>
                  <button class="btn btn-dark rounded-3">
                    Add candidates
                    <i class="bi bi-plus ms-2"></i>
                  </button>
                </div>
                <p class="text-muted mb-2">Senior {{ position.seniority }}</p>
                <div class="d-flex align-items-center justify-content-between">
                  <div class="text-muted">
                    {{ position.department }} •
                    {{ position.candidates }} candidate
                  </div>
                  <a href="#" class="text-decoration-none">
                    View details
                    <i class="bi bi-chevron-right ms-1"></i>
                  </a>
                </div>
              </div>
            </div>
            <button class="btn btn-link text-center">
              Show more
              <i class="bi bi-chevron-down ms-1"></i>
            </button>
          </div>
        </div>
      </div>
    </div>
  `,
  styles: [
    `
      :host {
        display: block;
        background-color: #fff;
      }

      .nav-link {
        color: #666;
      }

      .nav-link:hover {
        color: #000;
      }

      .card {
        transition: transform 0.2s;
      }

      .card:hover {
        transform: translateY(-2px);
      }

      .form-control {
        border-radius: 8px;
      }

      .form-control:focus {
        box-shadow: none;
        border-color: #000;
      }

      .btn-dark {
        padding: 8px 16px;
      }
    `,
  ],
})
export class InterviewRequestComponent {
  profiles: Profile[] = [
    { icon: 'bi bi-file-text', name: 'FrontEnd' },
    { icon: 'bi bi-code-slash', name: 'Backend' },
    { icon: 'bi bi-globe', name: 'Architect' },
    { icon: 'bi bi-gear', name: 'Automation Engineering' },
    { icon: 'bi bi-server', name: 'Backend' },
    { icon: 'bi bi-graph-up', name: 'Business Analyst' },
    { icon: 'bi bi-database', name: 'Data Engineering' },
    { icon: 'bi bi-shield-lock', name: 'Cyber Security' },
  ];

  positions: Position[] = [
    {
      role: 'Frontend role',
      id: 'e1Xve',
      seniority: '5-8 years',
      department: 'Frontend',
      candidates: 1,
    },
    {
      role: 'Backend role',
      id: 'E8-ls',
      seniority: '2-3 years',
      department: 'Backend',
      candidates: 1,
    },
    {
      role: 'SQA role',
      id: 'pDgQ4',
      seniority: '4-5 years',
      department: 'SQA',
      candidates: 1,
    },
    {
      role: 'Frontend role',
      id: 'av_ZP',
      seniority: '5-8 years',
      department: 'Frontend',
      candidates: 1,
    },
  ];
}
