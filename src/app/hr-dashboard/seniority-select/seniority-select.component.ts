// import { Component, OnInit } from '@angular/core';
// import { CommonModule } from '@angular/common';
// import { RouterModule, Router } from '@angular/router';
// import { JobService, Skill, SeniorityLevel as ApiSeniorityLevel } from '../services/job.service';

// interface SeniorityLevel {
//   id: string;
//   title: string;
//   experience: string;
//   icon: string;
//   color: string;
//   selected?: boolean;
//   progressValue: number;
// }

// @Component({
//   selector: 'app-seniority-select',
//   standalone: true,
//   imports: [CommonModule, RouterModule],
//   template: `
//     <div class="container-fluid py-4">
//       <!-- Header -->
//       <div class="d-flex align-items-center justify-content-between mb-5">
//         <div class="d-flex align-items-center">
//           <a routerLink=".." class="btn btn-link text-dark p-0 me-3">
//             <i class="bi bi-arrow-left fs-4"></i>
//           </a>
//           <div>
//             <h1 class="h4 mb-1 fw-bold">Select seniority</h1>
//             <p class="text-muted mb-0">
//               Choose the experience level for the position
//             </p>
//           </div>
//         </div>
//         <div class="progress-indicator">
//           <span class="badge bg-primary-subtle text-primary-emphasis"
//             >Step 2 of 4</span
//           >
//         </div>
//       </div>

//       <!-- Loading State -->
//       <div *ngIf="loading" class="text-center my-5">
//         <div class="spinner-border" role="status">
//           <span class="visually-hidden">Loading...</span>
//         </div>
//         <p class="mt-2">Loading seniority levels...</p>
//       </div>

//       <!-- Error State -->
//       <div *ngIf="error" class="alert alert-danger my-5">
//         {{ error }}
//       </div>

//       <!-- Seniority Cards -->
//       <div class="row g-4" *ngIf="!loading && !error">
//         <div class="col-md-6 col-lg-4" *ngFor="let level of seniorityLevels">
//           <div
//             class="seniority-card"
//             [class.selected]="level.selected"
//             (click)="selectLevel(level)"
//           >
//             <div class="card h-70 border-2 rounded-4 shadow-hover">
//               <div class="card-body p-4">
//                 <div class="d-flex align-items-start mb-3">
//                   <div
//                     class="icon-wrapper rounded-3 me-3"
//                     [style.backgroundColor]="level.color + '15'"
//                     [style.color]="level.color"
//                   >
//                     <i [class]="'bi ' + level.icon"></i>
//                   </div>
//                   <div class="flex-grow-1">
//                     <h2 class="h5 fw-bold mb-1">{{ level.title }}</h2>
//                     <p class="text-muted mb-0">{{ level.experience }}</p>
//                   </div>
//                   <div class="selection-indicator">
//                     <div class="circle" [class.active]="level.selected">
//                       <i class="bi bi-check2"></i>
//                     </div>
//                   </div>
//                 </div>
//                 <div class="progress" style="height: 6px;">
//                   <div
//                     class="progress-bar"
//                     [style.width.%]="level.progressValue"
//                     [style.backgroundColor]="level.color"
//                   ></div>
//                 </div>
//               </div>
//             </div>
//           </div>
//         </div>
//       </div>

//       <!-- Action Button -->
//       <div
//         class="position-fixed bottom-0 start-0 w-100 p-4 bg-white border-top"
//       >
//         <div class="container-fluid">
//           <button
//             class="btn btn-dark w-100 rounded-3 py-3"
//             [disabled]="!getSelectedLevel()"
//             (click)="continue()"
//           >
//             Continue with {{ getSelectedLevel()?.title || 'selected' }} level
//           </button>
//         </div>
//       </div>
//     </div>
//   `,
//   styles: [
//     `
//       :host {
//         display: block;
//         background-color: #fff;
//         min-height: 100vh;
//         padding-bottom: 100px;
//       }

//       .seniority-card {
//         cursor: pointer;
//         transition: all 0.3s ease;
//       }

//       .shadow-hover {
//         transition: all 0.3s ease;
//       }

//       .seniority-card:hover .shadow-hover {
//         transform: translateY(-4px);
//         box-shadow: 0 10px 20px rgba(0, 0, 0, 0.1) !important;
//       }

//       .icon-wrapper {
//         width: 48px;
//         height: 48px;
//         display: flex;
//         align-items: center;
//         justify-content: center;
//         font-size: 1.5rem;
//       }

//       .selection-indicator .circle {
//         width: 24px;
//         height: 24px;
//         border-radius: 50%;
//         border: 2px solid #dee2e6;
//         display: flex;
//         align-items: center;
//         justify-content: center;
//         color: white;
//         transition: all 0.2s ease;
//       }

//       .selection-indicator .circle.active {
//         background-color: #000;
//         border-color: #000;
//       }

//       .selection-indicator .circle i {
//         opacity: 0;
//         transform: scale(0);
//         transition: all 0.2s ease;
//       }

//       .selection-indicator .circle.active i {
//         opacity: 1;
//         transform: scale(1);
//       }

//       .progress {
//         background-color: #f8f9fa;
//         border-radius: 10px;
//       }

//       .progress-bar {
//         border-radius: 10px;
//         transition: width 0.3s ease;
//       }

//       .selected .card {
//         border: 2px solid #000 !important;
//       }
//     `,
//   ],
// })
// export class SenioritySelectComponent implements OnInit {
//   seniorityLevels: SeniorityLevel[] = [];
//   loading = false;
//   error = '';
//   selectedSkill: Skill | null = null;

//   // Map for seniority level icons and colors based on title
//   private seniorityIconMap: Record<string, { icon: string, color: string, progressValue: number }> = {
//     'Internship': { icon: 'bi-mortarboard', color: '#0ea5e9', progressValue: 15 },
//     'Entry Level': { icon: 'bi-rocket', color: '#22c55e', progressValue: 30 },
//     'Junior': { icon: 'bi-ladder', color: '#f59e0b', progressValue: 45 },
//     'Mid Level': { icon: 'bi-graph-up-arrow', color: '#ec4899', progressValue: 60 },
//     'Senior': { icon: 'bi-stars', color: '#8b5cf6', progressValue: 75 },
//     'Lead': { icon: 'bi-award', color: '#6366f1', progressValue: 90 },
//     'Principal': { icon: 'bi-trophy', color: '#475569', progressValue: 100 },
//     'default': { icon: 'bi-person', color: '#475569', progressValue: 50 }
//   };

//   // Map for years of experience based on seniority level name
//   private experienceYearsMap: Record<string, string> = {
//     'Internship': '0 yrs of experience',
//     'Entry Level': '0-1 yrs of experience',
//     'Junior': '1-3 yrs of experience',
//     'Mid Level': '3-5 yrs of experience',
//     'Senior': '5-8 yrs of experience',
//     'Lead': '8-10 yrs of experience',
//     'Principal': '10+ yrs of experience',
//     'default': '3-5 yrs of experience'
//   };

//   constructor(private jobService: JobService, private router: Router) {}

//   ngOnInit(): void {
//     this.loading = true;
//     this.error = '';
    
//     // Get selected skill from the service
//     this.selectedSkill = this.jobService.getSelectedSkill();
    
//     if (this.selectedSkill && this.selectedSkill.seniorityLevelJunctions) {
//       // Process junctions to create seniority levels
//       this.processSeniorityLevels();
//     } else {
//       // Fetch skills if no selected skill is available
//       this.jobService.fetchSkills().subscribe({
//         next: (res) => {
//           if (res.items && res.items.length > 0) {
//             this.selectedSkill = res.items[0]; // Use first skill as fallback
//             this.processSeniorityLevels();
//           } else {
//             this.error = 'No skills available. Please go back and select a skill.';
//             this.loading = false;
//           }
//         },
//         error: (err) => {
//           console.error('Failed to load skills:', err);
//           this.error = 'Failed to load seniority levels. Please try again.';
//           this.loading = false;
//         }
//       });
//     }
//   }

//   private processSeniorityLevels(): void {
//     if (this.selectedSkill && this.selectedSkill.seniorityLevelJunctions) {
//       if (this.selectedSkill.seniorityLevelJunctions.length > 0) {
//         this.seniorityLevels = this.selectedSkill.seniorityLevelJunctions.map(junction => {
//           const seniority = junction.seniority;
//           const displayInfo = this.seniorityIconMap[seniority.name] || this.seniorityIconMap['default'];
//           const experience = this.experienceYearsMap[seniority.name] || this.experienceYearsMap['default'];
          
//           return {
//             id: seniority.id,
//             title: seniority.name,
//             experience: experience,
//             icon: displayInfo.icon,
//             color: displayInfo.color,
//             progressValue: displayInfo.progressValue
//           };
//         });
//       } else {
//         this.error = 'No seniority levels found for the selected skill.';
//       }
//     } else {
//       this.error = 'No skill selected. Please go back and select a skill.';
//     }
    
//     this.loading = false;
//   }

//   selectLevel(level: SeniorityLevel) {
//     this.seniorityLevels.forEach((l) => (l.selected = false));
//     level.selected = true;
    
//     // Store the selected seniority level in the service
//     if (this.selectedSkill) {
//       const apiSeniority = this.selectedSkill.seniorityLevelJunctions?.find(
//         junction => junction.seniority.id === level.id
//       )?.seniority;
      
//       if (apiSeniority) {
//         this.jobService.setSelectedSeniority(apiSeniority);
//       }
//     }
//   }

//   getSelectedLevel(): SeniorityLevel | undefined {
//     return this.seniorityLevels.find((level) => level.selected);
//   }
  
//   continue(): void {
//     if (this.getSelectedLevel()) {
//       this.router.navigate(['/hr-dashboard/job-post/customize-interview']);
//     }
//   }
// }

