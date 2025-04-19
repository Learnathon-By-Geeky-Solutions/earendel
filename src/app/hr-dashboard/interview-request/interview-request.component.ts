import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { Router } from '@angular/router';
import { JobService, Skill } from '../services/job.service';

interface Profile {
  icon: string;
  name: string;
}

@Component({
  selector: 'app-interview-request',
  standalone: true,
  imports: [CommonModule, FormsModule, HttpClientModule],
  templateUrl: './interview-request.component.html',
  styleUrls: ['./interview-request.component.scss']
})
export class InterviewRequestComponent implements OnInit {
  profiles: Profile[] = [];
  loading = false;
  error = '';

  constructor(
    private jobService: JobService,
    private router: Router
  ) {}

  ngOnInit(): void {
    console.log('[InterviewRequestComponent] ngOnInit fired');
    this.loadProfiles();
  }

  loadProfiles(): void {
    console.log('[InterviewRequestComponent] loadProfiles() called');
    this.loading = true;
    this.error = '';

    this.jobService.fetchSkills(1, 100).subscribe({
      next: (res) => {
        console.log('[InterviewRequestComponent] fetchSkills next:', res);
        this.profiles = res.items.map((skill: Skill) => ({
          name: skill.name,
          icon: this.getIconForSkill(skill.name)
        }));
        this.loading = false;
      },
      error: (err) => {
        console.error('[InterviewRequestComponent] fetchSkills error:', err);
        this.error = 'Failed to load profiles';
        this.loading = false;
      }
    });
  }

  getIconForSkill(name: string): string {
    const map: Record<string,string> = {
      'Frontend': 'bi bi-window',
      'Backend': 'bi bi-code-slash',
      'Architect': 'bi bi-diagram-3',
      'Automation Engineering': 'bi bi-gear',
      'Database': 'bi bi-database',
      'Business Analyst': 'bi bi-graph-up',
      'Data Engineering': 'bi bi-cpu',
      'Cyber Security': 'bi bi-shield-lock',
      'default': 'bi bi-lightbulb'
    };
    return map[name] || map['default'];
  }

  selectProfile(profile: Profile): void {
    console.log('[InterviewRequestComponent] selectProfile:', profile);
    // TODO: save selected skill in service, e.g. JobService.setSelectedSkills(...)
    this.router.navigate(['hr-dashboard/job-post/technology-selection']);
  }
}
