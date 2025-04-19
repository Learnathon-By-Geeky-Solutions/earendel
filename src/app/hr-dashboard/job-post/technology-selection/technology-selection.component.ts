import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router, ActivatedRoute } from '@angular/router';
import { SidebarComponent } from '../../sidebar/sidebar.component';
import { JobService, Skill, SubSkill } from '../../services/job.service';

@Component({
  selector: 'app-technology-selection',
  standalone: true,
  imports: [CommonModule, SidebarComponent],
  template: `
    <div class="container">
      <header class="header">
        <button class="back-btn" (click)="goBack()">
          <i class="bi bi-arrow-left"></i>
          {{ domain }}
        </button>
      </header>

      <main class="main-content">
        <div class="left-section">
          <section class="tech-section" *ngIf="subSkills.length > 0">
            <h3>Skill based</h3>
            <div class="tech-grid">
              <div
                class="tech-card"
                *ngFor="let tech of subSkills"
                (click)="selectSubSkill(tech)"
                [class.selected]="tech.id === selectedSubSkill?.id"
              >
                <img [src]="getIconForSubSkill(tech.name)" [alt]="tech.name" />
                <span>{{ tech.name }}</span>
              </div>
            </div>
          </section>
 
        </div>

        <div class="right-section">
          <div class="progress-indicator">
            <h2>You are 2 steps away</h2>
            <p>
              {{ interviewersCount }} Interviewers ready to be scheduled in 38
              mins
            </p>

            <div class="progress-dots">
              <div class="dot active">
                <span class="dot-label">Candidate</span>
              </div>
              <div class="dot-line"></div>
              <div class="dot-group">
                <div class="dot"></div>
                <div class="dot"></div>
                <div class="dot"></div>
              </div>
              <span class="group-label">Interviewers</span>
            </div>
          </div>
        </div>
      </main>
    </div>
  `,
  styles: [
    `
      .container {
        min-height: 100vh;
        background: #fff;
        padding: 2rem;
      }

      .header {
        margin-bottom: 2rem;
      }

      .back-btn {
        display: flex;
        align-items: center;
        gap: 0.5rem;
        background: none;
        border: none;
        font-size: 1rem;
        cursor: pointer;
        padding: 0;

        i {
          font-size: 1.25rem;
        }
      }

      .main-content {
        display: grid;
        grid-template-columns: 1fr 400px;
        gap: 2rem;
      }

      .tech-section {
        margin-bottom: 2rem;

        h3 {
          font-size: 0.875rem;
          color: #666;
          margin-bottom: 1rem;
        }
      }

      .tech-grid {
        display: grid;
        grid-template-columns: repeat(auto-fill, minmax(120px, 1fr));
        gap: 1rem;
      }

      .tech-card {
        padding: 1rem;
        border: 1px solid #eee;
        border-radius: 8px;
        text-align: center;
        cursor: pointer;
        transition: all 0.2s ease;

        &:hover {
          border-color: #000;
        }

        &.selected {
          border-color: #0066cc;
          background: #f0f7ff;
        }

        img {
          width: 32px;
          height: 32px;
          margin-bottom: 0.5rem;
        }

        span {
          font-size: 0.875rem;
          color: #333;
        }
      }

      .progress-indicator {
        background: #f8f9fa;
        padding: 2rem;
        border-radius: 12px;
        text-align: center;

        h2 {
          font-size: 1.5rem;
          margin-bottom: 0.5rem;
        }

        p {
          color: #666;
          font-size: 0.875rem;
          margin-bottom: 2rem;
        }
      }

      .progress-dots {
        display: flex;
        align-items: center;
        justify-content: center;
        position: relative;
        padding-top: 1.5rem;

        .dot {
          width: 24px;
          height: 24px;
          border-radius: 50%;
          background: #ddd;
          position: relative;

          &.active {
            background: #0066cc;
          }

          .dot-label {
            position: absolute;
            top: -1.5rem;
            left: 50%;
            transform: translateX(-50%);
            white-space: nowrap;
            font-size: 0.75rem;
            color: #666;
          }
        }

        .dot-line {
          flex: 1;
          height: 2px;
          background: #ddd;
          margin: 0 1rem;
        }

        .dot-group {
          display: flex;
          gap: 0.25rem;

          .dot {
            width: 16px;
            height: 16px;
          }
        }

        .group-label {
          position: absolute;
          bottom: -1.5rem;
          right: 0;
          font-size: 0.75rem;
          color: #666;
        }
      }

      @media (max-width: 1024px) {
        .main-content {
          grid-template-columns: 1fr;
        }
      }

      @media (max-width: 768px) {
        .container {
          padding: 1rem;
        }

        .tech-grid {
          grid-template-columns: repeat(auto-fill, minmax(100px, 1fr));
        }
      }
    `,
  ],
})
export class TechnologySelectionComponent implements OnInit {
  domain = '';
  selectedTech: any | null = null;
  selectedSubSkill: SubSkill | null = null;
  interviewersCount = 1274;
  subSkills: SubSkill[] = [];
  selectedSkill: Skill | null = null;
  loading = false;
  error = '';

  skillBasedOptions: any[] = [
    {
      id: 'nodejs',
      name: 'Node.js',
      icon: 'https://cdn.jsdelivr.net/gh/devicons/devicon/icons/nodejs/nodejs-original.svg',
      category: 'skill',
    },
    {
      id: 'python',
      name: 'Python',
      icon: 'https://cdn.jsdelivr.net/gh/devicons/devicon/icons/nodejs/nodejs-original.svg',
      category: 'skill',
    },
    {
      id: 'java',
      name: 'Java',
      icon: 'https://cdn.jsdelivr.net/gh/devicons/devicon/icons/nodejs/nodejs-original.svg',
      category: 'skill',
    },
    {
      id: 'go',
      name: 'Go',
      icon: 'https://cdn.jsdelivr.net/gh/devicons/devicon/icons/nodejs/nodejs-original.svg',
      category: 'skill',
    },
    {
      id: 'php',
      name: 'PHP',
      icon: 'https://cdn.jsdelivr.net/gh/devicons/devicon/icons/nodejs/nodejs-original.svg',
      category: 'skill',
    },
  ];

 

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private jobService: JobService
  ) {
    this.domain = this.route.snapshot.paramMap.get('domain') || 'Backend';
  }

  ngOnInit(): void {
    // Get the selected skill from the job service
    this.selectedSkill = this.jobService.getSelectedSkill();
    
    if (this.selectedSkill && this.selectedSkill.subSkills) {
      console.log('[TechnologySelectionComponent] Selected skill:', this.selectedSkill);
      this.subSkills = this.selectedSkill.subSkills;
    } else {
      console.warn('[TechnologySelectionComponent] No skill or subskills found');
      this.error = 'No subskills available for this skill';
    }
  }

  goBack() {
    this.router.navigate(['/hr-dashboard/job-post']);
  }

  selectTechnology(tech: any) {
    this.selectedTech = tech;
    this.router.navigate(['/hr-dashboard/job-post/seniority-selection', this.domain.toLowerCase(), tech.id]);
  }

  selectSubSkill(subSkill: SubSkill) {
    this.selectedSubSkill = subSkill;
    console.log('[TechnologySelectionComponent] Selected subskill:', subSkill);
    this.jobService.setSelectedSubSkill(subSkill);
    
    // Navigate to seniority selection with the selected skill and subskill
    this.router.navigate(['/hr-dashboard/job-post/seniority-selection']);
  }

  getIconForSubSkill(name: string): string {
    const map: Record<string, string> = {
      'Node.js': 'https://cdn.jsdelivr.net/gh/devicons/devicon/icons/nodejs/nodejs-original.svg',
      'Express.js': 'https://cdn.jsdelivr.net/gh/devicons/devicon/icons/express/express-original.svg',
      'React': 'https://cdn.jsdelivr.net/gh/devicons/devicon/icons/react/react-original.svg',
      'Angular': 'https://cdn.jsdelivr.net/gh/devicons/devicon/icons/angularjs/angularjs-original.svg',
      'Vue.js': 'https://cdn.jsdelivr.net/gh/devicons/devicon/icons/vuejs/vuejs-original.svg',
      'Python': 'https://cdn.jsdelivr.net/gh/devicons/devicon/icons/python/python-original.svg',
      'Django': 'https://cdn.jsdelivr.net/gh/devicons/devicon/icons/django/django-plain.svg',
      'Java': 'https://cdn.jsdelivr.net/gh/devicons/devicon/icons/java/java-original.svg',
      'Spring': 'https://cdn.jsdelivr.net/gh/devicons/devicon/icons/spring/spring-original.svg',
      'PHP': 'https://cdn.jsdelivr.net/gh/devicons/devicon/icons/php/php-original.svg',
      'Laravel': 'https://cdn.jsdelivr.net/gh/devicons/devicon/icons/laravel/laravel-plain.svg',
      'Go': 'https://cdn.jsdelivr.net/gh/devicons/devicon/icons/go/go-original.svg',
      'default': 'https://cdn.jsdelivr.net/gh/devicons/devicon/icons/nodejs/nodejs-original.svg'
    };
    return map[name] || map['default'];
  }
}
