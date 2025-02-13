import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';

interface Skill {
  name: string;
  category: string;
  growth: string;
}

@Component({
  selector: 'app-top-skills',
  standalone: true,
  imports: [CommonModule],
  template: `
    <div class="card border-0 shadow-sm">
      <div class="card-body p-4">
        <h6 class="mb-4">Top Skills</h6>

        <div class="skills-list">
          <div
            *ngFor="let skill of skills"
            class="skill-item d-flex justify-content-between align-items-center py-3"
          >
            <div>
              <h6 class="mb-1">{{ skill.name }}</h6>
              <small class="text-muted">{{ skill.category }}</small>
            </div>
            <span class="text-success">{{ skill.growth }}</span>
          </div>
        </div>
      </div>
    </div>
  `,
  styles: [
    `
      .card {
        border-radius: 8px;
      }

      .skill-item {
        border-bottom: 1px solid #e5e7eb;
      }

      .skill-item:last-child {
        border-bottom: none;
      }

      h6 {
        font-size: 15px;
        margin-bottom: 0;
      }
    `,
  ],
})
export class TopSkillsComponent {
  skills: Skill[] = [
    {
      name: 'React',
      category: 'Frontend Development',
      growth: '+25%',
    },
    {
      name: 'Python',
      category: 'Data Science',
      growth: '+18%',
    },
    {
      name: 'Java',
      category: 'Backend Development',
      growth: '+12%',
    },
  ];
}
