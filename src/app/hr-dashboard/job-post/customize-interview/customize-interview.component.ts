import { Component, type OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router, ActivatedRoute } from '@angular/router';
import { SidebarComponent } from '../../sidebar/sidebar.component';
import { JobService, Skill, SubSkill, SeniorityLevel, Rubric } from '../../services/job.service';

@Component({
  selector: 'app-customized-interview',
  standalone: true,
  imports: [CommonModule],
  template: `
    <div class="container">
      <div class="main-section">
        <header class="header">
          <button class="back-btn" (click)="goBack()">
            <i class="bi bi-arrow-left"></i>
            Customize
          </button>
          <div class="selected-tech">
            <span>{{ getSelectedDomain() }}</span>
            <button class="edit-btn">Edit</button>
          </div>
        </header>

        <div class="content">
          <div class="javascript-section">
            <h2>{{ getSelectedSkill()?.name || 'Select a Skill' }}</h2>
            <p class="subtitle">Choose to have skills</p>

            <div class="experience-level">
              <div class="level-info">
                <h3>{{ getSelectedSeniority()?.name || 'Select Seniority' }}</h3>
                <span class="level-tag">Medium</span>
              </div>
            </div>

            <div class="rubrics-list">
              <div
                *ngFor="let rubric of rubrics"
                class="rubric-item"
                [class.selected]="rubric.isSelected"
                (click)="toggleRubric(rubric)"
              >
                <div class="checkbox">
                  <i class="bi bi-check" *ngIf="rubric.isSelected"></i>
                </div>
                <div class="rubric-content">
                  <h4>{{ rubric.title }}</h4>
                  <p>{{ rubric.description }}</p>
                </div>
              </div>
            </div>

            <button class="advanced-config-btn">
              Advanced configuration
              <i class="bi bi-chevron-right"></i>
            </button>

            <button class="round-based-btn">
              Choose a round based Interview
              <i class="bi bi-chevron-right"></i>
            </button>

            <div class="submit-section">
              <button class="submit-btn">
                <div class="avatars">
                  <div class="avatar-group">
                    <img
                      src="https://via.placeholder.com/32"
                      alt="Interviewer 1"
                    />
                    <img
                      src="https://via.placeholder.com/32"
                      alt="Interviewer 2"
                    />
                    <img
                      src="https://via.placeholder.com/32"
                      alt="Interviewer 3"
                    />
                  </div>
                </div>
                Submit & add candidates
                <div class="duration-select">
                  <span>1 hr</span>
                  <i class="bi bi-chevron-down"></i>
                </div>
              </button>
            </div>
          </div>
        </div>
      </div>

    </div>
  `,
  styles: [
    `
      .container {
        display: grid;
        grid-template-columns: 1fr 400px;
        min-height: 100vh;
        background: #fff;
      }

      .main-section {
        padding: 24px;
        max-width: 900px;
      }

      .header {
        display: flex;
        align-items: center;
        gap: 16px;
        margin-bottom: 32px;
      }

      .back-btn {
        display: flex;
        align-items: center;
        gap: 8px;
        background: none;
        border: none;
        font-size: 16px;
        color: #111;
        cursor: pointer;
        padding: 0;

        i {
          font-size: 20px;
        }
      }

      .selected-tech {
        display: flex;
        align-items: center;
        gap: 12px;
        padding: 8px 16px;
        background: #f5f5f5;
        border-radius: 4px;
        font-size: 14px;

        .edit-btn {
          background: none;
          border: none;
          color: #0066ff;
          font-size: 14px;
          cursor: pointer;
          padding: 0;
        }
      }

      .javascript-section {
        h2 {
          font-size: 24px;
          margin: 0 0 8px;
        }

        .subtitle {
          color: #666;
          margin: 0 0 32px;
        }
      }

      .experience-level {
        margin-bottom: 24px;

        .level-info {
          display: flex;
          align-items: center;
          gap: 12px;

          h3 {
            font-size: 16px;
            margin: 0;
          }

          .level-tag {
            padding: 4px 12px;
            background: #fff3e0;
            color: #f57c00;
            border-radius: 16px;
            font-size: 12px;
          }
        }
      }

      .rubrics-list {
        display: flex;
        flex-direction: column;
        gap: 16px;
        margin-bottom: 24px;
      }

      .rubric-item {
        display: flex;
        gap: 16px;
        padding: 16px;
        border: 1px solid #e0e0e0;
        border-radius: 8px;
        cursor: pointer;
        transition: all 0.2s ease;

        &:hover {
          border-color: #0066ff;
        }

        &.selected {
          background: #f0f7ff;
          border-color: #0066ff;

          .checkbox {
            background: #0066ff;
            border-color: #0066ff;
            color: white;
          }
        }

        .checkbox {
          width: 20px;
          height: 20px;
          border: 2px solid #bdbdbd;
          border-radius: 4px;
          display: flex;
          align-items: center;
          justify-content: center;
          flex-shrink: 0;
          margin-top: 2px;
        }

        .rubric-content {
          h4 {
            font-size: 16px;
            margin: 0 0 8px;
          }

          p {
            color: #666;
            font-size: 14px;
            margin: 0;
            line-height: 1.5;
          }
        }
      }

      .advanced-config-btn,
      .round-based-btn {
        width: 100%;
        display: flex;
        justify-content: space-between;
        align-items: center;
        padding: 16px;
        background: none;
        border: 1px solid #e0e0e0;
        border-radius: 8px;
        font-size: 14px;
        color: #111;
        cursor: pointer;
        margin-bottom: 16px;

        &:hover {
          border-color: #000;
        }

        i {
          color: #666;
        }
      }

      .submit-section {
        margin-top: 32px;
      }

      .submit-btn {
        width: 100%;
        display: flex;
        align-items: center;
        justify-content: center;
        gap: 16px;
        padding: 16px;
        background: #111;
        border: none;
        border-radius: 8px;
        color: white;
        font-size: 14px;
        cursor: pointer;

        &:hover {
          background: #000;
        }

        .avatars {
          display: flex;
          align-items: center;

          .avatar-group {
            display: flex;
            align-items: center;

            img {
              width: 24px;
              height: 24px;
              border-radius: 50%;
              border: 2px solid white;
              margin-left: -8px;

              &:first-child {
                margin-left: 0;
              }
            }
          }
        }

        .duration-select {
          display: flex;
          align-items: center;
          gap: 8px;
          margin-left: auto;
          padding: 4px 8px;
          border: 1px solid rgba(255, 255, 255, 0.2);
          border-radius: 4px;
        }
      }

      .rubrics-panel {
        background: #fafafa;
        border-left: 1px solid #e0e0e0;
        padding: 24px;
      }

      @media (max-width: 1200px) {
        .container {
          grid-template-columns: 1fr;
        }

        .rubrics-panel {
          border-left: none;
          border-top: 1px solid #e0e0e0;
        }
      }

      @media (max-width: 768px) {
        .main-section {
          padding: 16px;
        }

        .rubrics-panel {
          padding: 16px;
        }
      }
    `,
  ],
})
export class CustomizedInterviewComponent implements OnInit {
  // Store rubrics loaded from API
  rubrics: {
    id: string;
    title: string;
    description: string;
    isSelected: boolean;
    weight?: number;
    subSkillId?: string;
    seniorityId?: string;
  }[] = [];
  
  // Store domain for display
  private domain: string = 'Frontend';

  constructor(
    private router: Router, 
    private route: ActivatedRoute,
    private jobService: JobService
  ) {}

  ngOnInit() {
    // Extract route parameters to set skill and seniority if not set
    const domain = this.route.snapshot.paramMap.get('domain');
    const tech = this.route.snapshot.paramMap.get('tech');
    const seniority = this.route.snapshot.paramMap.get('seniority');
    
    if (domain) {
      this.domain = domain;
    }
    
    console.log('Route params:', { domain, tech, seniority });

    // Log the current selections from JobService
    console.log('Current skill:', this.jobService.getSelectedSkill());
    console.log('Current subskill:', this.jobService.getSelectedSubSkill());
    console.log('Current seniority:', this.jobService.getSelectedSeniority());
    
    // Load rubrics data based on current selections
    this.loadRubrics();
  }

  loadRubrics(): void {
    this.jobService.getFilteredRubrics().subscribe({
      next: (rubrics) => {
        // If we get rubrics from the API, update our local array with that data
        if (rubrics && rubrics.length > 0) {
          this.rubrics = rubrics.map(r => ({
            id: r.id,
            title: r.title,
            description: r.rubricDescription,
            weight: r.weight,
            isSelected: true, // Default to selected
            subSkillId: r.subSkillId,
            seniorityId: r.seniorityId
          }));
          console.log('Updated rubrics from API:', this.rubrics);
        } else {
          console.log('No rubrics returned from API');
          this.rubrics = [];
        }
      },
      error: (err) => {
        console.error('Error loading rubrics:', err);
        this.rubrics = [];
      }
    });
  }

  getSelectedSkill(): Skill | null {
    return this.jobService.getSelectedSkill();
  }
  
  getSelectedSubSkill(): SubSkill | null {
    return this.jobService.getSelectedSubSkill();
  }
  
  getSelectedSeniority(): SeniorityLevel | null {
    return this.jobService.getSelectedSeniority();
  }
  
  getSelectedDomain(): string {
    return this.domain;
  }

  get selectedRubrics() {
    return this.rubrics.filter((r) => r.isSelected);
  }

  goBack() {
    this.router.navigateByUrl('/hr-dashboard/job-post/seniority-selection');
  }

  toggleRubric(rubric: any) {
    rubric.isSelected = !rubric.isSelected;
  }
}
