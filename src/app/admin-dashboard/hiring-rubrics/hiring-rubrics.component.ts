// import { Component, type OnInit, type OnDestroy } from '@angular/core';
// import { CommonModule } from '@angular/common';
// import { FormsModule } from '@angular/forms';
// import type { Subscription } from 'rxjs';
// import { SidebarComponent } from '../sidebar/sidebar.component';
// import { PaginationComponent } from '../pagination/pagination.component';
// import { RubricService } from '../services/rubric.service';
// import { SkillService } from '../services/skill.service';

// @Component({
//   selector: 'app-hiring-rubrics',
//   standalone: true,
//   imports: [CommonModule, FormsModule, SidebarComponent, PaginationComponent],
//   template: `
//     <div class="d-flex">
//       <app-sidebar></app-sidebar>

//       <main class="main-content bg-light">
//         <div class="p-4">
//           <!-- Header -->
//           <div class="d-flex justify-content-between align-items-center mb-4">
//             <div>
//               <h1 class="h4 mb-1">Hiring Rubrics</h1>
//               <p class="text-muted mb-0">
//                 Manage evaluation criteria for different skills and seniority
//                 levels
//               </p>
//             </div>
//             <button class="btn btn-dark" (click)="openAddRubricModal()">
//               <i class="bi bi-plus-lg me-2"></i>
//               Add Rubric
//             </button>
//           </div>

//           <!-- Search and Filter -->
//           <div class="row mb-4">
//             <div class="col-md-6 mb-3 mb-md-0">
//               <div class="input-group">
//                 <span class="input-group-text bg-white border-end-0">
//                   <i class="bi bi-search text-muted"></i>
//                 </span>
//                 <input
//                   type="text"
//                   class="form-control border-start-0"
//                   placeholder="Search skills, subskills, or rubrics..."
//                   [(ngModel)]="searchTerm"
//                   (input)="onSearch()"
//                 />
//               </div>
//             </div>
//             <div class="col-md-3 mb-3 mb-md-0">
//               <select
//                 class="form-select"
//                 [(ngModel)]="filterSkill"
//                 (change)="onFilter()"
//               >
//                 <option value="">All Skills</option>
//                 <option *ngFor="let skill of skills" [value]="skill.id">
//                   {{ skill.name }}
//                 </option>
//               </select>
//             </div>
//             <div class="col-md-3">
//               <select
//                 class="form-select"
//                 [(ngModel)]="filterSeniority"
//                 (change)="onFilter()"
//               >
//                 <option value="">All Seniority Levels</option>
//                 <option
//                   *ngFor="let seniority of allSeniorities"
//                   [value]="seniority.seniority.id"
//                 >
//                   {{ seniority.seniority.name }}
//                 </option>
//               </select>
//             </div>
//           </div>

//           <!-- Skills and Subskills List -->
//           <div class="rubrics-container">
//             <div *ngIf="isLoading" class="text-center py-5">
//               <div class="spinner-border text-primary" role="status">
//                 <span class="visually-hidden">Loading...</span>
//               </div>
//             </div>

//             <div
//               *ngIf="!isLoading && filteredSkills.length === 0"
//               class="text-center py-5"
//             >
//               <div class="empty-state">
//                 <i class="bi bi-clipboard-check fs-1 text-muted mb-3"></i>
//                 <h5>No skills or rubrics found</h5>
//                 <p class="text-muted">
//                   Try adjusting your search or filters, or add a new rubric.
//                 </p>
//               </div>
//             </div>

//             <!-- Skills List -->
//             <div *ngFor="let skill of filteredSkills" class="rubric-card mb-4">
//               <!-- Skill Header -->
//               <div class="rubric-header" (click)="toggleSkill(skill)">
//                 <div
//                   class="d-flex flex-column flex-md-row align-items-md-center gap-2"
//                 >
//                   <h3 class="rubric-title mb-0">{{ skill.name }}</h3>
//                   <div class="badges">
//                     <span class="badge bg-primary me-2"
//                       >{{ skill.subSkills.length }} Subskills</span
//                     >
//                     <span class="badge bg-secondary"
//                       >{{ skill.seniorityLevelJunctions.length }} Seniority
//                       Levels</span
//                     >
//                   </div>
//                 </div>
//                 <div class="rubric-actions">
//                   <i
//                     class="bi ms-2"
//                     [class.bi-chevron-down]="!skill.expanded"
//                     [class.bi-chevron-up]="skill.expanded"
//                   ></i>
//                 </div>
//               </div>

//               <!-- Subskills List -->
//               <div class="subskills-list p-3" *ngIf="skill.expanded">
//                 <div
//                   *ngFor="let subskill of skill.subSkills"
//                   class="subskill-card mb-3"
//                 >
//                   <!-- Subskill Header -->
//                   <div
//                     class="subskill-header p-3"
//                     (click)="toggleSubskill(subskill, skill)"
//                   >
//                     <div
//                       class="d-flex justify-content-between align-items-center"
//                     >
//                       <h4 class="subskill-title mb-0">{{ subskill.name }}</h4>
//                       <div class="subskill-actions">
//                         <i
//                           class="bi"
//                           [class.bi-chevron-down]="!subskill.expanded"
//                           [class.bi-chevron-up]="subskill.expanded"
//                         ></i>
//                       </div>
//                     </div>
//                   </div>

//                   <!-- Seniority and Rubrics List -->
//                   <div
//                     class="seniority-list p-3 bg-light"
//                     *ngIf="subskill.expanded"
//                   >
//                     <div
//                       *ngIf="subskill.isLoadingRubrics"
//                       class="text-center py-3"
//                     >
//                       <div
//                         class="spinner-border spinner-border-sm text-primary"
//                         role="status"
//                       >
//                         <span class="visually-hidden">Loading rubrics...</span>
//                       </div>
//                     </div>

//                     <div
//                       *ngIf="
//                         !subskill.isLoadingRubrics &&
//                         (!subskill.rubrics || subskill.rubrics.length === 0)
//                       "
//                       class="text-center py-3"
//                     >
//                       <p class="text-muted mb-2">
//                         No rubrics found for this subskill.
//                       </p>
//                       <button
//                         class="btn btn-sm btn-outline-primary"
//                         (click)="openAddRubricModalForSubskill(subskill, skill)"
//                       >
//                         <i class="bi bi-plus-lg me-1"></i>Add Rubric
//                       </button>
//                     </div>

//                     <div
//                       *ngIf="
//                         !subskill.isLoadingRubrics &&
//                         subskill.rubrics &&
//                         subskill.rubrics.length > 0
//                       "
//                     >
//                       <div
//                         class="d-flex justify-content-between align-items-center mb-3"
//                       >
//                         <h5 class="mb-0">Rubrics</h5>
//                         <button
//                           class="btn btn-sm btn-outline-primary"
//                           (click)="
//                             openAddRubricModalForSubskill(subskill, skill)
//                           "
//                         >
//                           <i class="bi bi-plus-lg me-1"></i>Add Rubric
//                         </button>
//                       </div>

//                       <div
//                         *ngFor="let rubric of subskill.rubrics"
//                         class="rubric-item mb-3"
//                       >
//                         <!-- Rubric Header -->
//                         <div
//                           class="rubric-item-header p-3 bg-white rounded"
//                           (click)="toggleRubric(rubric)"
//                         >
//                           <div
//                             class="d-flex justify-content-between align-items-center"
//                           >
//                             <div>
//                               <h5 class="rubric-item-title mb-1">
//                                 {{ rubric.title }}
//                               </h5>
//                               <div class="badges">
//                                 <span class="badge bg-info me-2">
//                                   Weight: {{ rubric.weight }}
//                                 </span>
//                                 <span
//                                   class="badge bg-secondary"
//                                   *ngIf="rubric.seniorityName"
//                                 >
//                                   {{ rubric.seniorityName }}
//                                 </span>
//                               </div>
//                             </div>
//                             <div class="rubric-item-actions">
//                               <button
//                                 class="btn btn-light btn-sm me-2"
//                                 (click)="
//                                   openEditRubricModal(rubric);
//                                   $event.stopPropagation()
//                                 "
//                               >
//                                 <i class="bi bi-pencil"></i>
//                               </button>
//                               <button
//                                 class="btn btn-light btn-sm"
//                                 (click)="
//                                   openDeleteRubricModal(rubric);
//                                   $event.stopPropagation()
//                                 "
//                               >
//                                 <i class="bi bi-trash"></i>
//                               </button>
//                               <i
//                                 class="bi ms-2"
//                                 [class.bi-chevron-down]="!rubric.expanded"
//                                 [class.bi-chevron-up]="rubric.expanded"
//                               ></i>
//                             </div>
//                           </div>
//                         </div>

//                         <!-- Rubric Details -->
//                         <div
//                           class="rubric-details p-3 bg-white rounded ms-3"
//                           *ngIf="rubric.expanded"
//                         >
//                           <div class="mb-3">
//                             <h6 class="mb-1">Description</h6>
//                             <p class="text-muted mb-0">
//                               {{
//                                 rubric.rubricDescription ||
//                                   'No description provided.'
//                               }}
//                             </p>
//                           </div>
//                         </div>
//                       </div>

//                       <!-- Pagination for rubrics -->
//                       <app-pagination
//                         *ngIf="subskill.totalRubrics > subskill.rubricPageSize"
//                         [currentPage]="subskill.currentRubricPage"
//                         [pageSize]="subskill.rubricPageSize"
//                         [totalItems]="subskill.totalRubrics"
//                         (pageChange)="
//                           onRubricPageChange(subskill, skill, $event)
//                         "
//                         class="mt-4"
//                       ></app-pagination>
//                     </div>
//                   </div>
//                 </div>
//               </div>
//             </div>

//             <!-- Pagination -->
//             <app-pagination
//               *ngIf="totalCount > 0"
//               [currentPage]="currentPage"
//               [pageSize]="pageSize"
//               [totalItems]="totalCount"
//               (pageChange)="onPageChange($event)"
//               class="mt-4"
//             ></app-pagination>
//           </div>
//         </div>
//       </main>

//       <!-- Add Rubric Modal -->
//       <div
//         class="modal"
//         [class.show]="showAddRubricModal"
//         [style.display]="showAddRubricModal ? 'block' : 'none'"
//       >
//         <div class="modal-dialog modal-dialog-centered">
//           <div class="modal-content border-0">
//             <div class="modal-header border-0">
//               <h5 class="modal-title">Add New Rubric</h5>
//               <button
//                 type="button"
//                 class="btn-close"
//                 (click)="closeAddRubricModal()"
//               ></button>
//             </div>
//             <div class="modal-body">
//               <div class="mb-3">
//                 <label class="form-label">Title</label>
//                 <input
//                   type="text"
//                   class="form-control"
//                   [(ngModel)]="newRubric.title"
//                   placeholder="Enter rubric title"
//                 />
//               </div>
//               <div class="mb-3">
//                 <label class="form-label">Description</label>
//                 <textarea
//                   class="form-control"
//                   rows="3"
//                   [(ngModel)]="newRubric.rubricDescription"
//                   placeholder="Enter rubric description"
//                 ></textarea>
//               </div>
//               <div class="mb-3">
//                 <label class="form-label">Skill</label>
//                 <select
//                   class="form-select"
//                   [(ngModel)]="selectedSkillId"
//                   (change)="onSkillChange()"
//                 >
//                   <option value="" disabled selected>Select a skill</option>
//                   <option *ngFor="let skill of skills" [value]="skill.id">
//                     {{ skill.name }}
//                   </option>
//                 </select>
//               </div>
//               <div class="mb-3">
//                 <label class="form-label">Subskill</label>
//                 <select
//                   class="form-select"
//                   [(ngModel)]="selectedSubskillId"
//                   [disabled]="!selectedSkillId"
//                 >
//                   <option value="" disabled selected>Select a subskill</option>
//                   <option
//                     *ngFor="let subskill of availableSubskills"
//                     [value]="subskill.id"
//                   >
//                     {{ subskill.name }}
//                   </option>
//                 </select>
//               </div>
//               <div class="mb-3">
//                 <label class="form-label">Seniority Level (Optional)</label>
//                 <select class="form-select" [(ngModel)]="selectedSeniorityId">
//                   <option value="">None (Apply to all seniority levels)</option>
//                   <option
//                     *ngFor="let junction of availableSeniorities"
//                     [value]="junction.seniority.id"
//                   >
//                     {{ junction.seniority.name }}
//                   </option>
//                 </select>
//               </div>
//               <div class="mb-3">
//                 <label class="form-label">Weight</label>
//                 <input
//                   type="number"
//                   class="form-control"
//                   [(ngModel)]="newRubric.weight"
//                   placeholder="Enter weight"
//                   min="1"
//                   max="10"
//                 />
//               </div>
//             </div>
//             <div class="modal-footer border-0">
//               <button
//                 type="button"
//                 class="btn btn-link text-dark"
//                 (click)="closeAddRubricModal()"
//               >
//                 Cancel
//               </button>
//               <button
//                 type="button"
//                 class="btn btn-dark"
//                 [disabled]="!isValidRubric()"
//                 (click)="addRubric()"
//               >
//                 Add Rubric
//               </button>
//             </div>
//           </div>
//         </div>
//       </div>

//       <!-- Edit Rubric Modal -->
//       <div
//         class="modal"
//         [class.show]="showEditRubricModal"
//         [style.display]="showEditRubricModal ? 'block' : 'none'"
//       >
//         <div class="modal-dialog modal-dialog-centered">
//           <div class="modal-content border-0">
//             <div class="modal-header border-0">
//               <h5 class="modal-title">Edit Rubric</h5>
//               <button
//                 type="button"
//                 class="btn-close"
//                 (click)="closeEditRubricModal()"
//               ></button>
//             </div>
//             <div class="modal-body">
//               <div class="mb-3">
//                 <label class="form-label">Title</label>
//                 <input
//                   type="text"
//                   class="form-control"
//                   [(ngModel)]="editingRubric.title"
//                   placeholder="Enter rubric title"
//                 />
//               </div>
//               <div class="mb-3">
//                 <label class="form-label">Description</label>
//                 <textarea
//                   class="form-control"
//                   rows="3"
//                   [(ngModel)]="editingRubric.rubricDescription"
//                   placeholder="Enter rubric description"
//                 ></textarea>
//               </div>
//               <div class="mb-3">
//                 <label class="form-label">Weight</label>
//                 <input
//                   type="number"
//                   class="form-control"
//                   [(ngModel)]="editingRubric.weight"
//                   placeholder="Enter weight"
//                   min="1"
//                   max="10"
//                 />
//               </div>
//               <!-- We don't allow changing skill, subskill, or seniority for existing rubrics -->
//             </div>
//             <div class="modal-footer border-0">
//               <button
//                 type="button"
//                 class="btn btn-link text-dark"
//                 (click)="closeEditRubricModal()"
//               >
//                 Cancel
//               </button>
//               <button
//                 type="button"
//                 class="btn btn-dark"
//                 [disabled]="!editingRubric.title"
//                 (click)="updateRubric()"
//               >
//                 Update Rubric
//               </button>
//             </div>
//           </div>
//         </div>
//       </div>

//       <!-- Delete Rubric Modal -->
//       <div
//         class="modal"
//         [class.show]="showDeleteRubricModal"
//         [style.display]="showDeleteRubricModal ? 'block' : 'none'"
//       >
//         <div class="modal-dialog modal-dialog-centered">
//           <div class="modal-content border-0">
//             <div class="modal-header border-0">
//               <h5 class="modal-title">Delete Rubric</h5>
//               <button
//                 type="button"
//                 class="btn-close"
//                 (click)="closeDeleteRubricModal()"
//               ></button>
//             </div>
//             <div class="modal-body">
//               <p>
//                 Are you sure you want to delete the rubric
//                 <strong>{{ deletingRubric?.title }}</strong
//                 >?
//               </p>
//               <p class="text-danger">
//                 This action cannot be undone and will delete all associated
//                 criteria.
//               </p>
//             </div>
//             <div class="modal-footer border-0">
//               <button
//                 type="button"
//                 class="btn btn-link text-dark"
//                 (click)="closeDeleteRubricModal()"
//               >
//                 Cancel
//               </button>
//               <button
//                 type="button"
//                 class="btn btn-danger"
//                 (click)="confirmDeleteRubric()"
//               >
//                 Delete Rubric
//               </button>
//             </div>
//           </div>
//         </div>
//       </div>

//       <!-- Modal Backdrop -->
//       <div
//         class="modal-backdrop fade show"
//         *ngIf="
//           showAddRubricModal || showEditRubricModal || showDeleteRubricModal
//         "
//         (click)="closeAllModals()"
//       ></div>
//     </div>
//   `,
//   styles: [
//     `
//       :host {
//         display: block;
//         min-height: 100vh;
//         background-color: #f9fafb;
//       }

//       .main-content {
//         margin-left: 240px;
//         width: calc(100% - 240px);
//         min-height: 100vh;
//       }

//       .rubrics-container {
//         max-height: calc(100vh - 240px);
//         overflow-y: auto;
//         padding-right: 1rem;
//       }

//       .rubric-card {
//         background: white;
//         border-radius: 12px;
//         box-shadow: 0 4px 6px rgba(0, 0, 0, 0.05), 0 1px 3px rgba(0, 0, 0, 0.1);
//         overflow: hidden;
//       }

//       .rubric-header {
//         padding: 1.5rem;
//         cursor: pointer;
//         display: flex;
//         justify-content: space-between;
//         align-items: center;
//         transition: background-color 0.2s ease;
//       }

//       .rubric-header:hover {
//         background-color: #f9fafb;
//       }

//       .rubric-title {
//         font-size: 1.125rem;
//         font-weight: 600;
//         color: #111827;
//       }

//       .badges {
//         display: flex;
//         flex-wrap: wrap;
//         gap: 0.5rem;
//       }

//       .badge {
//         font-weight: 500;
//         font-size: 0.75rem;
//         padding: 0.35rem 0.65rem;
//         border-radius: 6px;
//       }

//       .subskill-card {
//         background: white;
//         border-radius: 8px;
//         box-shadow: 0 1px 2px rgba(0, 0, 0, 0.05);
//       }

//       .subskill-header {
//         cursor: pointer;
//         transition: background-color 0.2s ease;
//       }

//       .subskill-header:hover {
//         background-color: #f9fafb;
//       }

//       .subskill-title {
//         font-size: 1rem;
//         font-weight: 600;
//         color: #111827;
//       }

//       .rubric-item {
//         border-radius: 8px;
//         overflow: hidden;
//       }

//       .rubric-item-header {
//         cursor: pointer;
//         transition: background-color 0.2s ease;
//       }

//       .rubric-item-header:hover {
//         background-color: #f9fafb !important;
//       }

//       .rubric-item-title {
//         font-size: 0.9rem;
//         font-weight: 600;
//         color: #111827;
//       }

//       .criteria-item {
//         transition: transform 0.2s ease, box-shadow 0.2s ease;
//       }

//       .criteria-item:hover {
//         transform: translateY(-2px);
//         box-shadow: 0 4px 6px rgba(0, 0, 0, 0.05), 0 2px 4px rgba(0, 0, 0, 0.05);
//       }

//       .criteria-title {
//         font-size: 0.875rem;
//         font-weight: 600;
//         margin: 0;
//         color: #111827;
//       }

//       .criteria-description {
//         font-size: 0.8125rem;
//         color: #6b7280;
//         margin: 0;
//         line-height: 1.5;
//       }

//       .criteria-actions,
//       .rubric-actions,
//       .subskill-actions {
//         display: flex;
//         align-items: center;
//       }

//       .empty-state {
//         display: flex;
//         flex-direction: column;
//         align-items: center;
//         padding: 2rem;
//       }

//       @media (max-width: 991.98px) {
//         .main-content {
//           margin-left: 0;
//           width: 100%;
//         }

//         .rubric-header {
//           flex-direction: column;
//           align-items: flex-start;
//           gap: 1rem;
//         }

//         .rubric-actions {
//           align-self: flex-end;
//         }
//       }

//       @media (max-width: 767.98px) {
//         .criteria-item {
//           padding: 1rem;
//         }

//         .criteria-actions {
//           flex-direction: column;
//           gap: 0.5rem;
//         }
//       }

//       .modal {
//         position: fixed;
//         top: 0;
//         left: 0;
//         width: 100%;
//         height: 100%;
//         background: rgba(0, 0, 0, 0.5);
//         display: flex;
//         align-items: center;
//         justify-content: center;
//         z-index: 1000;
//       }

//       .modal-content {
//         background: white;
//         padding: 1.5rem;
//         border-radius: 8px;
//         max-width: 500px;
//         width: 100%;
//       }

//       .modal-backdrop {
//         position: fixed;
//         top: 0;
//         left: 0;
//         width: 100%;
//         height: 100%;
//         background: rgba(0, 0, 0, 0.5);
//         z-index: 999;
//       }
//     `,
//   ],
// })
// export class HiringRubricsComponent implements OnInit, OnDestroy {
//   skills: any = [];
//   filteredSkills: any = [];
//   allSeniorities: any = [];

//   currentPage = 1;
//   pageSize = 10;
//   totalCount = 0;
//   isLoading = false;

//   searchTerm = '';
//   filterSkill = '';
//   filterSeniority = '';

//   // Modal states
//   showAddRubricModal = false;
//   showEditRubricModal = false;
//   showDeleteRubricModal = false;

//   // Form data
//   newRubric: any = {};
//   editingRubric: any = {};
//   deletingRubric: any | null = null;
//   selectedRubric: any | null = null;
//   newCriteria: any = {};
//   editingCriteria: any = {};
//   deletingCriteria: any | null = null;

//   // Selection for adding new rubric
//   selectedSkillId = '';
//   selectedSubskillId = '';
//   selectedSeniorityId = '';
//   availableSubskills: any = [];
//   availableSeniorities: any = [];

//   private subscriptions: Subscription[] = [];

//   constructor(
//     private rubricService: RubricService,
//     private skillService: SkillService
//   ) {}

//   ngOnInit() {
//     this.loadSkills();
//   }

//   ngOnDestroy() {
//     this.subscriptions.forEach((sub) => sub.unsubscribe());
//   }

//   loadSkills() {
//     this.isLoading = true;
//     const sub = this.skillService
//       .skillDetailsData({
//         pageNumber: this.currentPage,
//         pageSize: this.pageSize,
//       })
//       .subscribe({
//         next: (response: any) => {
//           this.skills = response.items.map((skill: any) => ({
//             ...skill,
//             expanded: false,
//             subSkills: skill.subSkills.map((subskill: any) => ({
//               ...subskill,
//               expanded: false,
//               isLoadingRubrics: false,
//             })),
//           }));

//           // Extract all unique seniorities from all skills
//           this.allSeniorities = this.extractAllSeniorities(this.skills);

//           this.filteredSkills = [...this.skills];
//           this.totalCount = response.totalCount;
//           this.isLoading = false;
//         },
//         error: (error: any) => {
//           console.error('Error loading skills:', error);
//           this.isLoading = false;
//         },
//       });
//     this.subscriptions.push(sub);
//   }

//   extractAllSeniorities(skills: any): any {
//     const seniorityMap = new Map<string, any>();

//     skills.forEach((skill: any) => {
//       skill.seniorityLevelJunctions.forEach((junction: any) => {
//         if (!seniorityMap.has(junction.seniority.id)) {
//           seniorityMap.set(junction.seniority.id, junction);
//         }
//       });
//     });

//     return Array.from(seniorityMap.values());
//   }

//   onPageChange(page: number) {
//     this.currentPage = page;
//     this.loadSkills();
//   }

//   toggleSkill(skill: any) {
//     skill.expanded = !skill.expanded;
//   }

//   toggleSubskill(subskill: any, skill: any) {
//     subskill.expanded = !subskill.expanded;

//     // Load rubrics if expanding and no rubrics loaded yet
//     if (
//       subskill.expanded &&
//       (!subskill.rubrics || subskill.rubrics.length === 0)
//     ) {
//       this.loadRubricsForSubskill(subskill, skill);
//     }
//   }

//   loadRubricsForSubskill(subskill: any, skill: any) {
//     subskill.isLoadingRubrics = true;

//     // Initialize pagination properties if not already set
//     if (!subskill.currentRubricPage) {
//       subskill.currentRubricPage = 1;
//     }
//     if (!subskill.rubricPageSize) {
//       subskill.rubricPageSize = 10;
//     }

//     // Call the API to get rubrics for this subskill with pagination
//     this.rubricService
//       .rubricDetailsData({
//         subSkillId: subskill.id,
//         pageNumber: subskill.currentRubricPage,
//         pageSize: subskill.rubricPageSize,
//       })
//       .subscribe({
//         next: (response: any) => {
//           // Map the response to include expanded property
//           const rubrics = response.items.map((rubric: any) => ({
//             ...rubric,
//             expanded: false,
//             // Add seniority name if available
//             seniorityName: rubric.seniorityLevelId
//               ? skill.seniorityLevelJunctions.find(
//                   (j: any) => j.seniority.id === rubric.seniorityLevelId
//                 )?.seniority.name
//               : null,
//           }));

//           subskill.rubrics = rubrics;
//           subskill.totalRubrics = response.totalCount;
//           subskill.totalRubricPages = response.totalPages;
//           subskill.isLoadingRubrics = false;
//         },
//         error: (error: any) => {
//           console.error(
//             `Error loading rubrics for subskill ${subskill.id}:`,
//             error
//           );
//           subskill.rubrics = [];
//           subskill.totalRubrics = 0;
//           subskill.totalRubricPages = 0;
//           subskill.isLoadingRubrics = false;
//         },
//       });
//   }

//   onRubricPageChange(subskill: any, skill: any, page: number) {
//     subskill.currentRubricPage = page;
//     subskill.isLoadingRubrics = true;

//     this.rubricService
//       .rubricDetailsData({
//         subSkillId: subskill.id,
//         pageNumber: subskill.currentRubricPage,
//         pageSize: subskill.rubricPageSize,
//       })
//       .subscribe({
//         next: (response: any) => {
//           const rubrics = response.items.map((rubric: any) => ({
//             ...rubric,
//             expanded: false,
//             seniorityName: rubric.seniorityLevelId
//               ? skill.seniorityLevelJunctions.find(
//                   (j: any) => j.seniority.id === rubric.seniorityLevelId
//                 )?.seniority.name
//               : null,
//           }));

//           subskill.rubrics = rubrics;
//           subskill.totalRubrics = response.totalCount;
//           subskill.totalRubricPages = response.totalPages;
//           subskill.isLoadingRubrics = false;
//         },
//         error: (error: any) => {
//           console.error(
//             `Error loading rubrics for subskill ${subskill.id}:`,
//             error
//           );
//           subskill.isLoadingRubrics = false;
//         },
//       });
//   }

//   toggleRubric(rubric: any) {
//     rubric.expanded = !rubric.expanded;
//   }

//   onSearch() {
//     this.applyFilters();
//   }

//   onFilter() {
//     this.applyFilters();
//   }

//   applyFilters() {
//     // First filter skills
//     const filteredSkills = this.skills.filter((skill: any) => {
//       // Filter by skill name if search term is provided
//       const matchesSearch = this.searchTerm
//         ? skill.name.toLowerCase().includes(this.searchTerm.toLowerCase())
//         : true;

//       // Filter by selected skill if provided
//       const matchesSkill = this.filterSkill
//         ? skill.id === this.filterSkill
//         : true;

//       return matchesSearch && matchesSkill;
//     });

//     // Then filter subskills within each skill
//     this.filteredSkills = filteredSkills
//       .map((skill: any) => {
//         // Create a copy of the skill to avoid modifying the original
//         const filteredSkill = { ...skill };

//         // Filter subskills by search term and seniority
//         filteredSkill.subSkills = skill.subSkills.filter((subskill: any) => {
//           const matchesSearch = this.searchTerm
//             ? subskill.name
//                 .toLowerCase()
//                 .includes(this.searchTerm.toLowerCase())
//             : true;

//           // For seniority filter, we'll need to check if the subskill has rubrics for this seniority
//           // This is a bit complex since rubrics are loaded on demand
//           // For simplicity, we'll just include all subskills when filtering by seniority

//           return matchesSearch;
//         });

//         // Only include skills that have matching subskills
//         return filteredSkill.subSkills.length > 0 ? filteredSkill : null;
//       })
//       .filter((skill: any) => skill !== null);
//   }

//   // Rubric CRUD operations
//   openAddRubricModal() {
//     this.newRubric = {
//       weight: 1, // Default weight
//     };
//     this.selectedSkillId = '';
//     this.selectedSubskillId = '';
//     this.selectedSeniorityId = '';
//     this.availableSubskills = [];
//     this.availableSeniorities = [];
//     this.showAddRubricModal = true;
//   }

//   openAddRubricModalForSubskill(subskill: any, skill: any) {
//     this.newRubric = {
//       weight: 1, // Default weight
//     };
//     this.selectedSkillId = skill.id;
//     this.selectedSubskillId = subskill.id;
//     this.selectedSeniorityId = '';
//     this.availableSubskills = skill.subSkills;
//     this.availableSeniorities = skill.seniorityLevelJunctions;
//     this.showAddRubricModal = true;
//   }

//   closeAddRubricModal() {
//     this.showAddRubricModal = false;
//     this.newRubric = {};
//     this.selectedSkillId = '';
//     this.selectedSubskillId = '';
//     this.selectedSeniorityId = '';
//   }

//   onSkillChange() {
//     if (this.selectedSkillId) {
//       const selectedSkill = this.skills.find(
//         (s: any) => s.id === this.selectedSkillId
//       );
//       if (selectedSkill) {
//         this.availableSubskills = selectedSkill.subSkills;
//         this.availableSeniorities = selectedSkill.seniorityLevelJunctions;
//       } else {
//         this.availableSubskills = [];
//         this.availableSeniorities = [];
//       }
//       this.selectedSubskillId = '';
//       this.selectedSeniorityId = '';
//     }
//   }

//   openEditRubricModal(rubric: any) {
//     this.editingRubric = { ...rubric };
//     this.showEditRubricModal = true;
//   }

//   closeEditRubricModal() {
//     this.showEditRubricModal = false;
//     this.editingRubric = {};
//   }

//   openDeleteRubricModal(rubric: any) {
//     this.deletingRubric = rubric;
//     this.showDeleteRubricModal = true;
//   }

//   closeDeleteRubricModal() {
//     this.showDeleteRubricModal = false;
//     this.deletingRubric = null;
//   }

//   isValidRubric(): boolean {
//     return (
//       !!this.newRubric.title &&
//       !!this.selectedSkillId &&
//       !!this.selectedSubskillId &&
//       !!this.newRubric.weight
//     );
//   }

//   addRubric() {
//     if (this.isValidRubric()) {
//       this.isLoading = true;

//       const rubricData = {
//         title: this.newRubric.title,
//         rubricDescription: this.newRubric.rubricDescription || '',
//         subSkillId: this.selectedSubskillId,
//         seniorityLevelId: this.selectedSeniorityId || null,
//         weight: this.newRubric.weight,
//       };

//       // Call the API to create the rubric
//       this.rubricService.rubricCreatedData(rubricData).subscribe({
//         next: (response: any) => {
//           // Find the subskill and refresh its rubrics
//           this.skills.forEach((skill: any) => {
//             if (skill.id === this.selectedSkillId) {
//               skill.subSkills.forEach((subskill: any) => {
//                 if (
//                   subskill.id === this.selectedSubskillId &&
//                   subskill.expanded
//                 ) {
//                   // Reload rubrics for this subskill to include the new one
//                   this.loadRubricsForSubskill(subskill, skill);
//                 }
//               });
//             }
//           });

//           this.closeAddRubricModal();
//           this.isLoading = false;
//         },
//         error: (error: any) => {
//           console.error('Error creating rubric:', error);
//           this.isLoading = false;

//           // Fallback for demo purposes - simulate success
//           setTimeout(() => {
//             this.skills.forEach((skill: any) => {
//               if (skill.id === this.selectedSkillId) {
//                 skill.subSkills.forEach((subskill: any) => {
//                   if (subskill.id === this.selectedSubskillId) {
//                     if (!subskill.rubrics) {
//                       subskill.rubrics = [];
//                       subskill.totalRubrics = 0;
//                     }

//                     // Generate a mock ID
//                     const newRubric = {
//                       ...rubricData,
//                       id: `mock-${Date.now()}`,
//                       expanded: false,
//                       seniorityName: this.selectedSeniorityId
//                         ? skill.seniorityLevelJunctions.find(
//                             (j: any) =>
//                               j.seniority.id === this.selectedSeniorityId
//                           )?.seniority.name
//                         : null,
//                     };

//                     subskill.rubrics.unshift(newRubric);
//                     subskill.totalRubrics = (subskill.totalRubrics || 0) + 1;
//                   }
//                 });
//               }
//             });

//             this.closeAddRubricModal();
//             this.isLoading = false;
//           }, 500);
//         },
//       });
//     }
//   }

//   updateRubric() {
//     if (this.editingRubric.title) {
//       this.isLoading = true;

//       const updatedRubricData = {
//         id: this.editingRubric.id,
//         title: this.editingRubric.title,
//         rubricDescription: this.editingRubric.rubricDescription,
//         seniorityId: this.editingRubric.seniorityId,
//         subSkillId: this.editingRubric.subSkillId,
//         weight: this.editingRubric.weight
//       };

//       this.rubricService.rubricUpdatedData(updatedRubricData).subscribe({
//         next: (response) => {
//           // Handle success
//           console.log('Rubric updated successfully:', response);
//           this.isLoading = false;
//           this.closeEditRubricModal();
//           this.applyFilters();
//           this.loadSkills();
//         },
//         error: (error) => {
//           // Handle error
//           console.error('Error updating rubric:', error);
//           this.isLoading = false;
//         },
//       });
//     }
//   }

//   confirmDeleteRubric() {
//     if (this.deletingRubric) {
//       this.isLoading = true;
//       this.rubricService.rubricDeletedData(this.deletingRubric.id).subscribe({
//         next: (response) => {
//           // Handle success
//           console.log('Rubric deleted successfully:', response);
//           this.isLoading = false;
//           this.closeEditRubricModal();
//           this.applyFilters();
//           this.loadSkills();
//         },
//         error: (error) => {
//           // Handle error
//           console.error('Error updating rubric:', error);
//           this.isLoading = false;
//         },
//       });
//       // Call your API to delete the rubric
//       // For now, we'll just simulate success
//       setTimeout(() => {
//         // Remove the rubric from the skills array
//         this.skills.forEach((skill: any) => {
//           skill.subSkills.forEach((subskill: any) => {
//             if (subskill.rubrics) {
//               subskill.rubrics = subskill.rubrics.filter(
//                 (r: any) => r.id !== this.deletingRubric!.id
//               );
//             }
//           });
//         });

//         this.applyFilters();
//         this.closeDeleteRubricModal();
//         this.isLoading = false;
//       }, 500);
//     }
//   }

//   closeAllModals() {
//     this.closeAddRubricModal();
//     this.closeEditRubricModal();
//     this.closeDeleteRubricModal();
//   }
// }

import { Component, type OnInit, type OnDestroy } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import type { Subscription } from 'rxjs';
import { SidebarComponent } from '../sidebar/sidebar.component';
import { PaginationComponent } from '../pagination/pagination.component';
import { RubricService } from '../services/rubric.service';
import { SkillService } from '../services/skill.service';

@Component({
  selector: 'app-hiring-rubrics',
  standalone: true,
  imports: [CommonModule, FormsModule, SidebarComponent, PaginationComponent],
  template: `
    <div class="d-flex">
      <app-sidebar></app-sidebar>

      <main class="main-content bg-light">
        <div class="p-4">
          <!-- Header -->
          <div class="d-flex justify-content-between align-items-center mb-4">
            <div>
              <h1 class="h4 mb-1">Hiring Rubrics</h1>
              <p class="text-muted mb-0">
                Manage evaluation criteria for different skills and seniority
                levels
              </p>
            </div>
            <button class="btn btn-dark" (click)="openAddRubricModal()">
              <i class="bi bi-plus-lg me-2"></i>
              Add Rubric
            </button>
          </div>

          <!-- Search and Filter -->
          <div class="row mb-4">
            <div class="col-md-6 mb-3 mb-md-0">
              <div class="input-group">
                <span class="input-group-text bg-white border-end-0">
                  <i class="bi bi-search text-muted"></i>
                </span>
                <input
                  type="text"
                  class="form-control border-start-0"
                  placeholder="Search skills, subskills, or rubrics..."
                  [(ngModel)]="searchTerm"
                  (input)="onSearch()"
                />
              </div>
            </div>
            <div class="col-md-3 mb-3 mb-md-0">
              <select
                class="form-select"
                [(ngModel)]="filterSkill"
                (change)="onFilter()"
              >
                <option value="">All Skills</option>
                <option *ngFor="let skill of skills" [value]="skill.id">
                  {{ skill.name }}
                </option>
              </select>
            </div>
            <div class="col-md-3">
              <select
                class="form-select"
                [(ngModel)]="filterSeniority"
                (change)="onFilter()"
              >
                <option value="">All Seniority Levels</option>
                <option
                  *ngFor="let seniority of allSeniorities"
                  [value]="seniority.seniority.id"
                >
                  {{ seniority.seniority.name }}
                </option>
              </select>
            </div>
          </div>

          <!-- Skills and Subskills List -->
          <div class="rubrics-container">
            <div *ngIf="isLoading" class="text-center py-5">
              <div class="spinner-border text-primary" role="status">
                <span class="visually-hidden">Loading...</span>
              </div>
            </div>

            <div
              *ngIf="!isLoading && filteredSkills.length === 0"
              class="text-center py-5"
            >
              <div class="empty-state">
                <i class="bi bi-clipboard-check fs-1 text-muted mb-3"></i>
                <h5>No skills or rubrics found</h5>
                <p class="text-muted">
                  Try adjusting your search or filters, or add a new rubric.
                </p>
              </div>
            </div>

            <!-- Skills List -->
            <div *ngFor="let skill of filteredSkills" class="rubric-card mb-4">
              <!-- Skill Header -->
              <div class="rubric-header" (click)="toggleSkill(skill)">
                <div
                  class="d-flex flex-column flex-md-row align-items-md-center gap-2"
                >
                  <h3 class="rubric-title mb-0">{{ skill.name }}</h3>
                  <div class="badges">
                    <span class="badge bg-primary me-2"
                      >{{ skill.subSkills.length }} Subskills</span
                    >
                    <span class="badge bg-secondary"
                      >{{ skill.seniorityLevelJunctions.length }} Seniority
                      Levels</span
                    >
                  </div>
                </div>
                <div class="rubric-actions">
                  <i
                    class="bi ms-2"
                    [class.bi-chevron-down]="!skill.expanded"
                    [class.bi-chevron-up]="skill.expanded"
                  ></i>
                </div>
              </div>

              <!-- Subskills List -->
              <div class="subskills-list p-3" *ngIf="skill.expanded">
                <div
                  *ngFor="let subskill of skill.subSkills"
                  class="subskill-card mb-3"
                >
                  <!-- Subskill Header -->
                  <div
                    class="subskill-header p-3"
                    (click)="toggleSubskill(subskill, skill)"
                  >
                    <div
                      class="d-flex justify-content-between align-items-center"
                    >
                      <h4 class="subskill-title mb-0">{{ subskill.name }}</h4>
                      <div class="subskill-actions">
                        <i
                          class="bi"
                          [class.bi-chevron-down]="!subskill.expanded"
                          [class.bi-chevron-up]="subskill.expanded"
                        ></i>
                      </div>
                    </div>
                  </div>

                  <!-- Seniority and Rubrics List -->
                  <div
                    class="seniority-list p-3 bg-light"
                    *ngIf="subskill.expanded"
                  >
                    <div
                      *ngIf="subskill.isLoadingRubrics"
                      class="text-center py-3"
                    >
                      <div
                        class="spinner-border spinner-border-sm text-primary"
                        role="status"
                      >
                        <span class="visually-hidden">Loading rubrics...</span>
                      </div>
                    </div>

                    <div
                      *ngIf="
                        !subskill.isLoadingRubrics &&
                        (!subskill.rubrics || subskill.rubrics.length === 0)
                      "
                      class="text-center py-3"
                    >
                      <p class="text-muted mb-2">
                        No rubrics found for this subskill.
                      </p>
                      <button
                        class="btn btn-sm btn-outline-primary"
                        (click)="openAddRubricModalForSubskill(subskill, skill)"
                      >
                        <i class="bi bi-plus-lg me-1"></i>Add Rubric
                      </button>
                    </div>

                    <div
                      *ngIf="
                        !subskill.isLoadingRubrics &&
                        subskill.rubrics &&
                        subskill.rubrics.length > 0
                      "
                    >
                      <div
                        class="d-flex justify-content-between align-items-center mb-3"
                      >
                        <h5 class="mb-0">Rubrics</h5>
                        <button
                          class="btn btn-sm btn-outline-primary"
                          (click)="
                            openAddRubricModalForSubskill(subskill, skill)
                          "
                        >
                          <i class="bi bi-plus-lg me-1"></i>Add Rubric
                        </button>
                      </div>

                      <div
                        *ngFor="let rubric of subskill.rubrics"
                        class="rubric-item mb-3"
                      >
                        <!-- Rubric Header -->
                        <div
                          class="rubric-item-header p-3 bg-white rounded"
                          (click)="toggleRubric(rubric)"
                        >
                          <div
                            class="d-flex justify-content-between align-items-center"
                          >
                            <div>
                              <h5 class="rubric-item-title mb-1">
                                {{ rubric.title }}
                              </h5>
                              <div class="badges">
                                <span class="badge bg-info me-2">
                                  Weight: {{ rubric.weight }}
                                </span>
                                <span
                                  class="badge bg-secondary"
                                  *ngIf="rubric.seniorityName"
                                >
                                  {{ rubric.seniorityName }}
                                </span>
                              </div>
                            </div>
                            <div class="rubric-item-actions">
                              <button
                                class="btn btn-light btn-sm me-2"
                                (click)="
                                  openEditRubricModal(rubric);
                                  $event.stopPropagation()
                                "
                              >
                                <i class="bi bi-pencil"></i>
                              </button>
                              <button
                                class="btn btn-light btn-sm"
                                (click)="
                                  openDeleteRubricModal(rubric);
                                  $event.stopPropagation()
                                "
                              >
                                <i class="bi bi-trash"></i>
                              </button>
                              <i
                                class="bi ms-2"
                                [class.bi-chevron-down]="!rubric.expanded"
                                [class.bi-chevron-up]="rubric.expanded"
                              ></i>
                            </div>
                          </div>
                        </div>

                        <!-- Rubric Details -->
                        <div
                          class="rubric-details p-3 bg-white rounded ms-3"
                          *ngIf="rubric.expanded"
                        >
                          <div class="mb-3">
                            <h6 class="mb-1">Description</h6>
                            <p class="text-muted mb-0">
                              {{
                                rubric.rubricDescription ||
                                  'No description provided.'
                              }}
                            </p>
                          </div>
                        </div>
                      </div>

                      <!-- Pagination for rubrics -->
                      <app-pagination
                        *ngIf="subskill.totalRubrics > subskill.rubricPageSize"
                        [currentPage]="subskill.currentRubricPage"
                        [pageSize]="subskill.rubricPageSize"
                        [totalItems]="subskill.totalRubrics"
                        (pageChange)="
                          onRubricPageChange(subskill, skill, $event)
                        "
                        class="mt-4"
                      ></app-pagination>
                    </div>
                  </div>
                </div>
              </div>
            </div>

            <!-- Pagination -->
            <app-pagination
              *ngIf="totalCount > 0"
              [currentPage]="currentPage"
              [pageSize]="pageSize"
              [totalItems]="totalCount"
              (pageChange)="onPageChange($event)"
              class="mt-4"
            ></app-pagination>
          </div>
        </div>
      </main>

      <!-- Add Rubric Modal -->
      <div
        class="modal"
        [class.show]="showAddRubricModal"
        [style.display]="showAddRubricModal ? 'block' : 'none'"
      >
        <div class="modal-dialog modal-dialog-centered">
          <div class="modal-content border-0">
            <div class="modal-header border-0">
              <h5 class="modal-title">Add New Rubric</h5>
              <button
                type="button"
                class="btn-close"
                (click)="closeAddRubricModal()"
              ></button>
            </div>
            <div class="modal-body">
              <div class="mb-3">
                <label class="form-label">Title</label>
                <input
                  type="text"
                  class="form-control"
                  [(ngModel)]="newRubric.title"
                  placeholder="Enter rubric title"
                />
              </div>
              <div class="mb-3">
                <label class="form-label">Description</label>
                <textarea
                  class="form-control"
                  rows="3"
                  [(ngModel)]="newRubric.rubricDescription"
                  placeholder="Enter rubric description"
                ></textarea>
              </div>
              <div class="mb-3">
                <label class="form-label">Skill</label>
                <select
                  class="form-select"
                  [(ngModel)]="selectedSkillId"
                  (change)="onSkillChange()"
                >
                  <option value="" disabled selected>Select a skill</option>
                  <option *ngFor="let skill of skills" [value]="skill.id">
                    {{ skill.name }}
                  </option>
                </select>
              </div>
              <div class="mb-3">
                <label class="form-label">Subskill</label>
                <select
                  class="form-select"
                  [(ngModel)]="selectedSubskillId"
                  [disabled]="!selectedSkillId"
                >
                  <option value="" disabled selected>Select a subskill</option>
                  <option
                    *ngFor="let subskill of availableSubskills"
                    [value]="subskill.id"
                  >
                    {{ subskill.name }}
                  </option>
                </select>
              </div>
              <div class="mb-3">
                <label class="form-label">Seniority Level (Optional)</label>
                <select class="form-select" [(ngModel)]="selectedSeniorityId">
                  <option value="">None (Apply to all seniority levels)</option>
                  <option
                    *ngFor="let junction of availableSeniorities"
                    [value]="junction.seniority.id"
                  >
                    {{ junction.seniority.name }}
                  </option>
                </select>
              </div>
              <div class="mb-3">
                <label class="form-label">Weight</label>
                <input
                  type="number"
                  class="form-control"
                  [(ngModel)]="newRubric.weight"
                  placeholder="Enter weight"
                  min="1"
                  max="10"
                />
              </div>
            </div>
            <div class="modal-footer border-0">
              <button
                type="button"
                class="btn btn-link text-dark"
                (click)="closeAddRubricModal()"
              >
                Cancel
              </button>
              <button
                type="button"
                class="btn btn-dark"
                [disabled]="!isValidRubric()"
                (click)="addRubric()"
              >
                Add Rubric
              </button>
            </div>
          </div>
        </div>
      </div>

      <!-- Edit Rubric Modal -->
      <div
        class="modal"
        [class.show]="showEditRubricModal"
        [style.display]="showEditRubricModal ? 'block' : 'none'"
      >
        <div class="modal-dialog modal-dialog-centered">
          <div class="modal-content border-0">
            <div class="modal-header border-0">
              <h5 class="modal-title">Edit Rubric</h5>
              <button
                type="button"
                class="btn-close"
                (click)="closeEditRubricModal()"
              ></button>
            </div>
            <div class="modal-body">
              <div class="mb-3">
                <label class="form-label">Title</label>
                <input
                  type="text"
                  class="form-control"
                  [(ngModel)]="editingRubric.title"
                  placeholder="Enter rubric title"
                />
              </div>
              <div class="mb-3">
                <label class="form-label">Description</label>
                <textarea
                  class="form-control"
                  rows="3"
                  [(ngModel)]="editingRubric.rubricDescription"
                  placeholder="Enter rubric description"
                ></textarea>
              </div>
              <div class="mb-3">
                <label class="form-label">Seniority Level</label>
                <select
                  class="form-select"
                  [(ngModel)]="editingRubric.seniorityId"
                >
                  <option value="">None (Apply to all seniority levels)</option>
                  <option
                    *ngFor="let seniority of allSeniorities"
                    [value]="seniority.seniority.id"
                  >
                    {{ seniority.seniority.name }}
                  </option>
                </select>
              </div>
              <div class="mb-3">
                <label class="form-label">Weight</label>
                <input
                  type="number"
                  class="form-control"
                  [(ngModel)]="editingRubric.weight"
                  placeholder="Enter weight"
                  min="1"
                  max="10"
                />
              </div>
            </div>
            <div class="modal-footer border-0">
              <button
                type="button"
                class="btn btn-link text-dark"
                (click)="closeEditRubricModal()"
              >
                Cancel
              </button>
              <button
                type="button"
                class="btn btn-dark"
                [disabled]="!editingRubric.title"
                (click)="updateRubric()"
              >
                Update Rubric
              </button>
            </div>
          </div>
        </div>
      </div>

      <!-- Delete Rubric Modal -->
      <div
        class="modal"
        [class.show]="showDeleteRubricModal"
        [style.display]="showDeleteRubricModal ? 'block' : 'none'"
      >
        <div class="modal-dialog modal-dialog-centered">
          <div class="modal-content border-0">
            <div class="modal-header border-0">
              <h5 class="modal-title">Delete Rubric</h5>
              <button
                type="button"
                class="btn-close"
                (click)="closeDeleteRubricModal()"
              ></button>
            </div>
            <div class="modal-body">
              <p>
                Are you sure you want to delete the rubric
                <strong>{{ deletingRubric?.title }}</strong
                >?
              </p>
              <p class="text-danger">
                This action cannot be undone and will delete all associated
                criteria.
              </p>
            </div>
            <div class="modal-footer border-0">
              <button
                type="button"
                class="btn btn-link text-dark"
                (click)="closeDeleteRubricModal()"
              >
                Cancel
              </button>
              <button
                type="button"
                class="btn btn-danger"
                (click)="confirmDeleteRubric()"
              >
                Delete Rubric
              </button>
            </div>
          </div>
        </div>
      </div>

      <!-- Modal Backdrop -->
      <div
        class="modal-backdrop fade show"
        *ngIf="
          showAddRubricModal || showEditRubricModal || showDeleteRubricModal
        "
        (click)="closeAllModals()"
      ></div>
    </div>
  `,
  styles: [
    `
      :host {
        display: block;
        min-height: 100vh;
        background-color: #f9fafb;
      }

      .main-content {
        margin-left: 240px;
        width: calc(100% - 240px);
        min-height: 100vh;
      }

      .rubrics-container {
        max-height: calc(100vh - 240px);
        overflow-y: auto;
        padding-right: 1rem;
      }

      .rubric-card {
        background: white;
        border-radius: 12px;
        box-shadow: 0 4px 6px rgba(0, 0, 0, 0.05), 0 1px 3px rgba(0, 0, 0, 0.1);
        overflow: hidden;
      }

      .rubric-header {
        padding: 1.5rem;
        cursor: pointer;
        display: flex;
        justify-content: space-between;
        align-items: center;
        transition: background-color 0.2s ease;
      }

      .rubric-header:hover {
        background-color: #f9fafb;
      }

      .rubric-title {
        font-size: 1.125rem;
        font-weight: 600;
        color: #111827;
      }

      .badges {
        display: flex;
        flex-wrap: wrap;
        gap: 0.5rem;
      }

      .badge {
        font-weight: 500;
        font-size: 0.75rem;
        padding: 0.35rem 0.65rem;
        border-radius: 6px;
      }

      .subskill-card {
        background: white;
        border-radius: 8px;
        box-shadow: 0 1px 2px rgba(0, 0, 0, 0.05);
      }

      .subskill-header {
        cursor: pointer;
        transition: background-color 0.2s ease;
      }

      .subskill-header:hover {
        background-color: #f9fafb;
      }

      .subskill-title {
        font-size: 1rem;
        font-weight: 600;
        color: #111827;
      }

      .rubric-item {
        border-radius: 8px;
        overflow: hidden;
      }

      .rubric-item-header {
        cursor: pointer;
        transition: background-color 0.2s ease;
      }

      .rubric-item-header:hover {
        background-color: #f9fafb !important;
      }

      .rubric-item-title {
        font-size: 0.9rem;
        font-weight: 600;
        color: #111827;
      }

      .criteria-item {
        transition: transform 0.2s ease, box-shadow 0.2s ease;
      }

      .criteria-item:hover {
        transform: translateY(-2px);
        box-shadow: 0 4px 6px rgba(0, 0, 0, 0.05), 0 2px 4px rgba(0, 0, 0, 0.05);
      }

      .criteria-title {
        font-size: 0.875rem;
        font-weight: 600;
        margin: 0;
        color: #111827;
      }

      .criteria-description {
        font-size: 0.8125rem;
        color: #6b7280;
        margin: 0;
        line-height: 1.5;
      }

      .criteria-actions,
      .rubric-actions,
      .subskill-actions {
        display: flex;
        align-items: center;
      }

      .empty-state {
        display: flex;
        flex-direction: column;
        align-items: center;
        padding: 2rem;
      }

      @media (max-width: 991.98px) {
        .main-content {
          margin-left: 0;
          width: 100%;
        }

        .rubric-header {
          flex-direction: column;
          align-items: flex-start;
          gap: 1rem;
        }

        .rubric-actions {
          align-self: flex-end;
        }
      }

      @media (max-width: 767.98px) {
        .criteria-item {
          padding: 1rem;
        }

        .criteria-actions {
          flex-direction: column;
          gap: 0.5rem;
        }
      }

      .modal {
        position: fixed;
        top: 0;
        left: 0;
        width: 100%;
        height: 100%;
        background: rgba(0, 0, 0, 0.5);
        display: flex;
        align-items: center;
        justify-content: center;
        z-index: 1000;
      }

      .modal-content {
        background: white;
        padding: 1.5rem;
        border-radius: 8px;
        max-width: 500px;
        width: 100%;
      }

      .modal-backdrop {
        position: fixed;
        top: 0;
        left: 0;
        width: 100%;
        height: 100%;
        background: rgba(0, 0, 0, 0.5);
        z-index: 999;
      }
    `,
  ],
})
export class HiringRubricsComponent implements OnInit, OnDestroy {
  skills: any = [];
  filteredSkills: any = [];
  allSeniorities: any = [];

  currentPage = 1;
  pageSize = 10;
  totalCount = 0;
  isLoading = false;

  searchTerm = '';
  filterSkill = '';
  filterSeniority = '';

  // Modal states
  showAddRubricModal = false;
  showEditRubricModal = false;
  showDeleteRubricModal = false;

  // Form data
  newRubric: any = {};
  editingRubric: any = {};
  deletingRubric: any | null = null;
  selectedRubric: any | null = null;
  newCriteria: any = {};
  editingCriteria: any = {};
  deletingCriteria: any | null = null;

  // Selection for adding new rubric
  selectedSkillId = '';
  selectedSubskillId = '';
  selectedSeniorityId = '';
  availableSubskills: any = [];
  availableSeniorities: any = [];

  private subscriptions: Subscription[] = [];

  constructor(
    private rubricService: RubricService,
    private skillService: SkillService
  ) {}

  ngOnInit() {
    this.loadSkills();
  }

  ngOnDestroy() {
    this.subscriptions.forEach((sub) => sub.unsubscribe());
  }

  loadSkills() {
    this.isLoading = true;
    const sub = this.skillService
      .skillDetailsData({
        pageNumber: this.currentPage,
        pageSize: this.pageSize,
      })
      .subscribe({
        next: (response: any) => {
          this.skills = response.items.map((skill: any) => ({
            ...skill,
            expanded: false,
            subSkills: skill.subSkills.map((subskill: any) => ({
              ...subskill,
              expanded: false,
              isLoadingRubrics: false,
            })),
          }));

          // Extract all unique seniorities from all skills
          this.allSeniorities = this.extractAllSeniorities(this.skills);

          this.filteredSkills = [...this.skills];
          this.totalCount = response.totalCount;
          this.isLoading = false;
        },
        error: (error: any) => {
          console.error('Error loading skills:', error);
          this.isLoading = false;
        },
      });
    this.subscriptions.push(sub);
  }

  extractAllSeniorities(skills: any): any {
    const seniorityMap = new Map<string, any>();

    skills.forEach((skill: any) => {
      skill.seniorityLevelJunctions.forEach((junction: any) => {
        if (!seniorityMap.has(junction.seniority.id)) {
          seniorityMap.set(junction.seniority.id, junction);
        }
      });
    });

    return Array.from(seniorityMap.values());
  }

  onPageChange(page: number) {
    this.currentPage = page;
    this.loadSkills();
  }

  toggleSkill(skill: any) {
    skill.expanded = !skill.expanded;
  }

  toggleSubskill(subskill: any, skill: any) {
    subskill.expanded = !subskill.expanded;

    // Load rubrics if expanding and no rubrics loaded yet
    if (
      subskill.expanded &&
      (!subskill.rubrics || subskill.rubrics.length === 0)
    ) {
      this.loadRubricsForSubskill(subskill, skill);
    }
  }

  loadRubricsForSubskill(subskill: any, skill: any) {
    subskill.isLoadingRubrics = true;

    // Initialize pagination properties if not already set
    if (!subskill.currentRubricPage) {
      subskill.currentRubricPage = 1;
    }
    if (!subskill.rubricPageSize) {
      subskill.rubricPageSize = 10;
    }

    // Call the API to get rubrics for this subskill with pagination
    this.rubricService
      .rubricDetailsData({
        subSkillId: subskill.id,
        pageNumber: subskill.currentRubricPage,
        pageSize: subskill.rubricPageSize,
      })
      .subscribe({
        next: (response: any) => {
          // Map the response to include expanded property
          const rubrics = response.items.map((rubric: any) => ({
            ...rubric,
            expanded: false,
            // Add seniority name if available
            seniorityName: this.getSeniorityName(skill, rubric.seniorityId),
          }));

          subskill.rubrics = rubrics;
          subskill.totalRubrics = response.totalCount;
          subskill.totalRubricPages = response.totalPages;
          subskill.isLoadingRubrics = false;
        },
        error: (error: any) => {
          console.error(
            `Error loading rubrics for subskill ${subskill.id}:`,
            error
          );
          subskill.rubrics = [];
          subskill.totalRubrics = 0;
          subskill.totalRubricPages = 0;
          subskill.isLoadingRubrics = false;
        },
      });
  }

  onRubricPageChange(subskill: any, skill: any, page: number) {
    subskill.currentRubricPage = page;
    subskill.isLoadingRubrics = true;

    this.rubricService
      .rubricDetailsData({
        subSkillId: subskill.id,
        pageNumber: subskill.currentRubricPage,
        pageSize: subskill.rubricPageSize,
      })
      .subscribe({
        next: (response: any) => {
          const rubrics = response.items.map((rubric: any) => ({
            ...rubric,
            expanded: false,
            seniorityName: this.getSeniorityName(skill, rubric.seniorityId),
          }));

          subskill.rubrics = rubrics;
          subskill.totalRubrics = response.totalCount;
          subskill.totalRubricPages = response.totalPages;
          subskill.isLoadingRubrics = false;
        },
        error: (error: any) => {
          console.error(
            `Error loading rubrics for subskill ${subskill.id}:`,
            error
          );
          subskill.isLoadingRubrics = false;
        },
      });
  }

  toggleRubric(rubric: any) {
    rubric.expanded = !rubric.expanded;
  }

  onSearch() {
    this.applyFilters();
  }

  onFilter() {
    this.applyFilters();
  }

  applyFilters() {
    // First filter skills
    const filteredSkills = this.skills.filter((skill: any) => {
      // Filter by skill name if search term is provided
      const matchesSearch = this.searchTerm
        ? skill.name.toLowerCase().includes(this.searchTerm.toLowerCase())
        : true;

      // Filter by selected skill if provided
      const matchesSkill = this.filterSkill
        ? skill.id === this.filterSkill
        : true;

      return matchesSearch && matchesSkill;
    });

    // Then filter subskills within each skill
    this.filteredSkills = filteredSkills
      .map((skill: any) => {
        // Create a copy of the skill to avoid modifying the original
        const filteredSkill = { ...skill };

        // Filter subskills by search term and seniority
        filteredSkill.subSkills = skill.subSkills.filter((subskill: any) => {
          const matchesSearch = this.searchTerm
            ? subskill.name
                .toLowerCase()
                .includes(this.searchTerm.toLowerCase())
            : true;

          // For seniority filter, we'll need to check if the subskill has rubrics for this seniority
          // This is a bit complex since rubrics are loaded on demand
          // For simplicity, we'll just include all subskills when filtering by seniority

          return matchesSearch;
        });

        // Only include skills that have matching subskills
        return filteredSkill.subSkills.length > 0 ? filteredSkill : null;
      })
      .filter((skill: any) => skill !== null);
  }

  // Rubric CRUD operations
  openAddRubricModal() {
    this.newRubric = {
      weight: 1, // Default weight
    };
    this.selectedSkillId = '';
    this.selectedSubskillId = '';
    this.selectedSeniorityId = '';
    this.availableSubskills = [];
    this.availableSeniorities = [];
    this.showAddRubricModal = true;
  }

  openAddRubricModalForSubskill(subskill: any, skill: any) {
    this.newRubric = {
      weight: 1, // Default weight
    };
    this.selectedSkillId = skill.id;
    this.selectedSubskillId = subskill.id;
    this.selectedSeniorityId = '';
    this.availableSubskills = skill.subSkills;
    this.availableSeniorities = skill.seniorityLevelJunctions;
    this.showAddRubricModal = true;
  }

  closeAddRubricModal() {
    this.showAddRubricModal = false;
    this.newRubric = {};
    this.selectedSkillId = '';
    this.selectedSubskillId = '';
    this.selectedSeniorityId = '';
  }

  onSkillChange() {
    if (this.selectedSkillId) {
      const selectedSkill = this.skills.find(
        (s: any) => s.id === this.selectedSkillId
      );
      if (selectedSkill) {
        this.availableSubskills = selectedSkill.subSkills;
        this.availableSeniorities = selectedSkill.seniorityLevelJunctions;
      } else {
        this.availableSubskills = [];
        this.availableSeniorities = [];
      }
      this.selectedSubskillId = '';
      this.selectedSeniorityId = '';
    }
  }

  openEditRubricModal(rubric: any) {
    this.editingRubric = {
      ...rubric,
      // Ensure we have the seniorityLevelId even if seniorityName is displayed
      seniorityId: rubric.seniorityId || null,
    };
    this.showEditRubricModal = true;
  }

  closeEditRubricModal() {
    this.showEditRubricModal = false;
    this.editingRubric = {};
  }

  openDeleteRubricModal(rubric: any) {
    this.deletingRubric = rubric;
    this.showDeleteRubricModal = true;
  }

  closeDeleteRubricModal() {
    this.showDeleteRubricModal = false;
    this.deletingRubric = null;
  }

  isValidRubric(): boolean {
    return (
      !!this.newRubric.title &&
      !!this.selectedSkillId &&
      !!this.selectedSubskillId &&
      !!this.newRubric.weight
    );
  }

  addRubric() {
    if (this.isValidRubric()) {
      this.isLoading = true;

      const rubricData = {
        title: this.newRubric.title,
        rubricDescription: this.newRubric.rubricDescription || '',
        subSkillId: this.selectedSubskillId,
        seniorityId: this.selectedSeniorityId || null,
        weight: this.newRubric.weight,
      };

      // Call the API to create the rubric
      this.rubricService.rubricCreatedData(rubricData).subscribe({
        next: (response: any) => {
          // Find the subskill and refresh its rubrics
          this.skills.forEach((skill: any) => {
            if (skill.id === this.selectedSkillId) {
              skill.subSkills.forEach((subskill: any) => {
                if (
                  subskill.id === this.selectedSubskillId &&
                  subskill.expanded
                ) {
                  // Reload rubrics for this subskill to include the new one
                  this.loadRubricsForSubskill(subskill, skill);
                }
              });
            }
          });

          this.closeAddRubricModal();
          this.isLoading = false;
        },
        error: (error: any) => {
          console.error('Error creating rubric:', error);
          this.isLoading = false;

          // Fallback for demo purposes - simulate success
          setTimeout(() => {
            this.skills.forEach((skill: any) => {
              if (skill.id === this.selectedSkillId) {
                skill.subSkills.forEach((subskill: any) => {
                  if (subskill.id === this.selectedSubskillId) {
                    if (!subskill.rubrics) {
                      subskill.rubrics = [];
                      subskill.totalRubrics = 0;
                    }

                    // Generate a mock ID
                    const newRubric = {
                      ...rubricData,
                      id: `mock-${Date.now()}`,
                      expanded: false,
                      seniorityName: this.selectedSeniorityId
                        ? skill.seniorityLevelJunctions.find(
                            (j: any) =>
                              j.seniority.id === this.selectedSeniorityId
                          )?.seniority.name
                        : null,
                    };

                    subskill.rubrics.unshift(newRubric);
                    subskill.totalRubrics = (subskill.totalRubrics || 0) + 1;
                  }
                });
              }
            });

            this.closeAddRubricModal();
            this.isLoading = false;
          }, 500);
        },
      });
    }
  }

  updateRubric() {
    if (this.editingRubric.title) {
      this.isLoading = true;

      const updatedRubricData = {
        id: this.editingRubric.id,
        title: this.editingRubric.title,
        rubricDescription: this.editingRubric.rubricDescription,
        seniorityId: this.editingRubric.seniorityId,
        subSkillId: this.editingRubric.subSkillId,
        weight: this.editingRubric.weight,
      };

      this.rubricService.rubricUpdatedData(updatedRubricData).subscribe({
        next: (response: any) => {
          // Handle success
          console.log('Rubric updated successfully:', response);
          this.isLoading = false;
          this.closeEditRubricModal();
          this.applyFilters();
          this.loadSkills();
        },
        error: (error: any) => {
          // Handle error
          console.error('Error updating rubric:', error);
          this.isLoading = false;
        },
      });
    }
  }

  confirmDeleteRubric() {
    if (this.deletingRubric) {
      this.isLoading = true;
      const sub = this.rubricService
        .rubricDeletedData(this.deletingRubric.id)
        .subscribe({
          next: () => {
            this.loadSkills();
            this.closeDeleteRubricModal();
          },
          error: (error: any) => {
            console.error('Error deleting skill:', error);
            this.isLoading = false;
          },
        });
      this.subscriptions.push(sub);
    }
  }

  closeAllModals() {
    this.closeAddRubricModal();
    this.closeEditRubricModal();
    this.closeDeleteRubricModal();
  }

  getSeniorityName(skill: any, seniorityId: string): string | null {
    if (!seniorityId) return null;

    const junction = skill.seniorityLevelJunctions.find(
      (j: any) => j.seniority.id === seniorityId
    );
    return junction ? junction.seniority.name : null;
  }
}
