import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router, RouterModule } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { JobService } from '../../services/job.service';

@Component({
  selector: 'app-profile-selection',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterModule],
  templateUrl: './profile-selection.component.html',
  styleUrls: ['./profile-selection.component.scss']
})
export class ProfileSelectionComponent implements OnInit {
  profiles: any[] = [];

  constructor(
    private router: Router,
    private dialog: MatDialog,
    private jobService: JobService
  ) {}

  ngOnInit(): void {
    this.loadProfiles();
  }

  loadProfiles(): void {
    this.jobService.fetchSkills().subscribe({
      next: (res) => {
        this.profiles = res.items.map((item: any) => ({
          id: item.id,
          title: item.name,
          icon: 'bi bi-person-badge' // use appropriate icon logic here
        }));
      },
      error: (err) => {
        console.error('Failed to load profiles', err);
      }
    });
  }

  selectProfile(id: string) {
    // Get the selected skill and store it in the service before navigating
    const selectedSkill = this.jobService.getSelectedSkill();
    if (!selectedSkill) {
      const skill = this.profiles.find(profile => profile.id === id);
      if (skill) {
        this.jobService.setSelectedSkill(skill);
      }
    }
    this.router.navigate(['/hr-dashboard/job-post/technology-selection']);
  }
}
