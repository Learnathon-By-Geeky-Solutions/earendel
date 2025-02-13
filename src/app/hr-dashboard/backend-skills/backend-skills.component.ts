import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';

interface Skill {
  icon: string;
  name: string;
  backgroundColor: string;
}

@Component({
  selector: 'app-backend-skills',
  standalone: true,
  imports: [CommonModule, RouterModule],
  template: `
    <div class="container-fluid py-4">
      <div class="d-flex align-items-center mb-4">
        <a routerLink=".." class="btn btn-link text-dark p-0 me-3">
          <i class="bi bi-arrow-left fs-4"></i>
        </a>
        <h1 class="mb-0 fw-bold">Backend</h1>
      </div>

      <!-- Skill based section -->
      <section class="mb-5">
        <h2 class="h5 fw-bold mb-4">Skill based</h2>
        <div class="row g-2">
          <div class="col-6 col-md-4 col-lg-3" *ngFor="let skill of skillBased">
            <div class="card border-1 rounded-4 shadow-sm h-100">
              <div class="card-body text-center p-3">
                <div
                  [style.backgroundColor]="skill.backgroundColor"
                  class="rounded-3 d-inline-block p-2 mb-2"
                >
                  <img
                    [src]="skill.icon"
                    [alt]="skill.name"
                    height="30"
                    width="30"
                  />
                </div>
                <h3 class="card-title h6 mb-0" style="font-size: 0.9rem;">
                  {{ skill.name }}
                </h3>
              </div>
            </div>
          </div>
        </div>
      </section>

      <!-- Round based section -->
      <section class="mb-5">
        <h2 class="h5 fw-bold mb-4">Round based</h2>
        <div class="row g-2">
          <div class="col-6 col-md-4 col-lg-3" *ngFor="let skill of roundBased">
            <div class="card border-1 rounded-4 shadow-sm h-100">
              <div class="card-body text-center p-3">
                <div
                  [style.backgroundColor]="skill.backgroundColor"
                  class="rounded-3 d-inline-block p-2 mb-2"
                >
                  <img
                    [src]="skill.icon"
                    [alt]="skill.name"
                    height="30"
                    width="30"
                  />
                </div>
                <h3 class="card-title h6 mb-0" style="font-size: 0.9rem;">
                  {{ skill.name }}
                </h3>
              </div>
            </div>
          </div>
        </div>
      </section>

      <!-- Experience based section -->
      <section>
        <h2 class="h5 fw-bold mb-4">Experience based</h2>
        <div class="row g-2">
          <div
            class="col-6 col-md-4 col-lg-3"
            *ngFor="let skill of experienceBased"
          >
            <div class="card border-1 rounded-4 shadow-sm h-100">
              <div class="card-body text-center p-3">
                <div
                  [style.backgroundColor]="skill.backgroundColor"
                  class="rounded-3 d-inline-block p-2 mb-2"
                >
                  <img
                    [src]="skill.icon"
                    [alt]="skill.name"
                    height="30"
                    width="30"
                  />
                </div>
                <h3 class="card-title h6 mb-0" style="font-size: 0.9rem;">
                  {{ skill.name }}
                </h3>
              </div>
            </div>
          </div>
        </div>
      </section>
    </div>
  `,
  styles: [
    `
      :host {
        display: block;
        background-color: #fff;
      }

      .card {
        transition: transform 0.2s;
        max-width: 90%;
      }

      .card:hover {
        transform: translateY(-2px);
      }

      .row.g-2 > [class*='col-'] {
        padding-left: 0.5rem;
        padding-right: 0.5rem;
      }

      .card-body {
        padding: 0.5rem !important;
      }
    `,
  ],
})
export class BackendSkillsComponent {
  skillBased: Skill[] = [
    {
      icon: 'https://cdn.jsdelivr.net/gh/devicons/devicon/icons/nodejs/nodejs-original.svg',
      name: 'Node.js',
      backgroundColor: '#E9F9EE',
    },
    {
      icon: 'https://cdn.jsdelivr.net/gh/devicons/devicon/icons/nodejs/nodejs-original.svg',
      name: 'Python',
      backgroundColor: '#E9F2FF',
    },
    {
      icon: 'https://cdn.jsdelivr.net/gh/devicons/devicon/icons/nodejs/nodejs-original.svg',
      name: 'Java',
      backgroundColor: '#FFE9E9',
    },
    {
      icon: 'https://cdn.jsdelivr.net/gh/devicons/devicon/icons/nodejs/nodejs-original.svg',
      name: 'Go',
      backgroundColor: '#E9FAFF',
    },
    {
      icon: 'https://cdn.jsdelivr.net/gh/devicons/devicon/icons/nodejs/nodejs-original.svg',
      name: 'PHP',
      backgroundColor: '#F2E9FF',
    },
  ];

  roundBased: Skill[] = [
    {
      icon: 'https://cdn.jsdelivr.net/gh/devicons/devicon/icons/nodejs/nodejs-original.svg',
      name: 'System Design',
      backgroundColor: '#FFF9E9',
    },
    {
      icon: 'https://cdn.jsdelivr.net/gh/devicons/devicon/icons/nodejs/nodejs-original.svg',
      name: 'Data Structure & Algorithms',
      backgroundColor: '#E9F2FF',
    },
  ];

  experienceBased: Skill[] = [
    {
      icon: 'https://cdn.jsdelivr.net/gh/devicons/devicon/icons/nodejs/nodejs-original.svg',
      name: 'Backend Architect',
      backgroundColor: '#F5F5F5',
    },
  ];
}
