import { Component, type OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import  { Router, ActivatedRoute } from '@angular/router';
import { SidebarComponent } from '../../sidebar/sidebar.component';

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
            <span>Frontend</span>
            <button class="edit-btn">Edit</button>
          </div>
        </header>

        <div class="content">
          <div class="javascript-section">
            <h2>JavaScript</h2>
            <p class="subtitle">Choose to have skills</p>

            <div class="experience-level">
              <div class="level-info">
                <h3>Senior (5-8 years)</h3>
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

      <div class="rubrics-panel">
        <div class="panel-header">
          <h2>Configure rubrics</h2>
          <p>
            Adjust depth on each skill to be tested & overall round duration
            here
          </p>
        </div>

        <div class="duration-wrapper">
          <div class="duration-control">
            <div class="duration-label">
              <i class="bi bi-clock"></i>
              Duration
            </div>
            <div class="duration-value">
              <span>1 hr</span>
              <i class="bi bi-chevron-down"></i>
            </div>
          </div>
        </div>

        <div class="must-have-section">
          <h3>Must have rubrics</h3>

          <div class="rubric-details">
            <div class="rubric-item" *ngFor="let rubric of selectedRubrics">
              <div class="rubric-header">
                <i class="bi bi-check-circle-fill"></i>
                <span>{{ rubric.title }}</span>
                <button class="expand-btn">
                  <i class="bi bi-chevron-down"></i>
                </button>
              </div>
              <p>{{ rubric.description }}</p>
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

        .panel-header {
          margin-bottom: 32px;

          h2 {
            font-size: 20px;
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

      .duration-wrapper {
        margin-bottom: 32px;
      }

      .duration-control {
        display: flex;
        justify-content: space-between;
        align-items: center;
        padding: 16px;
        background: white;
        border-radius: 8px;
        box-shadow: 0 1px 3px rgba(0, 0, 0, 0.1);

        .duration-label {
          display: flex;
          align-items: center;
          gap: 8px;
          color: #666;
          font-size: 14px;
        }

        .duration-value {
          display: flex;
          align-items: center;
          gap: 8px;
          font-size: 14px;
          color: #111;
        }
      }

      .must-have-section {
        h3 {
          font-size: 14px;
          color: #666;
          margin: 0 0 16px;
        }
      }

      .rubric-details {
        display: flex;
        flex-direction: column;
        gap: 16px;

        .rubric-item {
          background: white;
          border-radius: 8px;
          padding: 16px;
          box-shadow: 0 1px 3px rgba(0, 0, 0, 0.1);

          .rubric-header {
            display: flex;
            align-items: center;
            gap: 8px;
            margin-bottom: 8px;

            i {
              color: #0066ff;
            }

            span {
              flex: 1;
              font-size: 14px;
              font-weight: 500;
            }

            .expand-btn {
              background: none;
              border: none;
              padding: 4px;
              color: #666;
              cursor: pointer;
            }
          }

          p {
            color: #666;
            font-size: 14px;
            margin: 0;
            line-height: 1.5;
          }
        }
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
  rubrics: any[] = [
    {
      id: 'async',
      title: 'Asynchronous programming',
      description:
        'Promises, Async/await, Callbacks, Observables, Generators etc.',
      isSelected: true,
    },
    {
      id: 'redux',
      title: 'Redux',
      description: 'Understanding of redux fundamentals',
      isSelected: true,
    },
    {
      id: 'browser',
      title: 'Browser Documents, Events and Interfaces',
      description:
        'Browser APIs, Events & event loop, DOM manipulation & tree, Forms',
      isSelected: true,
    },
    {
      id: 'fundamentals',
      title: 'Javascript fundamentals',
      description:
        'Operators, Objects, Data types, Advanced working with functions, Variable scope, closures, hoisting, polyfills, Currying, ES7/8/9 etc.',
      isSelected: true,
    },
  ];

  constructor(private router: Router, private route: ActivatedRoute) {}

  ngOnInit() {
    // Initialize component with route params
    const { domain, tech, seniority } = this.route.snapshot.params;
  }

  get selectedRubrics() {
    return this.rubrics.filter((r) => r.isSelected);
  }

  goBack() {
    const { domain, tech } = this.route.snapshot.params;
    this.router.navigate(['/hr-dashboard/job-post/seniority-selection']);
  }

  toggleRubric(rubric: any) {
    rubric.isSelected = !rubric.isSelected;
  }
}
