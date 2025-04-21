import { Component, OnInit, OnDestroy } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { SidebarComponent } from '../sidebar/sidebar.component';
import { PaginationComponent } from '../pagination/pagination.component';
import { SkillService } from '../services/skill.service';
import { Subscription } from 'rxjs';

// Interfaces
interface Seniority {
  id: string;
  name: string;
  description: string;
}

interface SeniorityApiResponse {
  items: Seniority[];
  pageNumber: number;
  pageSize: number;
  totalCount: number;
  totalPages: number;
  hasPrevious: boolean;
  hasNext: boolean;
}

interface SeniorityLevelJunction {
  id: string;
  seniorityLevelId: string;
  skillId: string;
  seniority: Seniority;
}

interface SubSkill {
  id: string;
  name: string;
  description: string;
  skillId: string;
}

interface Skill {
  id: string;
  name: string;
  description: string;
  subSkills: SubSkill[];
  seniorityLevelJunctions: SeniorityLevelJunction[];
  expanded?: boolean;
  selectedSeniorityIds?: string[];
}

interface SkillApiResponse {
  items: Skill[];
  pageNumber: number;
  pageSize: number;
  totalCount: number;
  totalPages: number;
  hasPrevious: boolean;
  hasNext: boolean;
}

@Component({
  selector: 'app-skills',
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
              <h1 class="h4 mb-1">Skills Management</h1>
              <p class="text-muted mb-0">
                Manage interview skills and subcategories
              </p>
            </div>
            <button class="btn btn-dark" (click)="openAddSkillModal()">
              <i class="bi bi-plus-lg me-2"></i>
              Add Skill
            </button>
          </div>

          <!-- Skills List -->
          <div class="skills-container">
            <div *ngFor="let skill of skills" class="skill-card">
              <!-- Main Skill -->
              <div class="skill-header" (click)="toggleSkill(skill)">
                <div class="d-flex align-items-center gap-3">
                  <div>
                    <h3 class="skill-title">{{ skill.name }}</h3>
                    <p class="skill-description">{{ skill.description }}</p>
                    <span
                      *ngFor="let junction of skill.seniorityLevelJunctions"
                      class="badge me-1"
                      [ngClass]="
                        getSeniorityBadgeClass(junction.seniority.name)
                      "
                    >
                      {{ junction.seniority.name }}
                    </span>
                  </div>
                </div>
                <div class="skill-actions">
                  <button
                    class="btn btn-light btn-sm me-2"
                    (click)="
                      openAddSubSkillModal(skill); $event.stopPropagation()
                    "
                  >
                    <i class="bi bi-plus-lg"></i>
                  </button>
                  <button
                    class="btn btn-light btn-sm me-2"
                    (click)="
                      openEditSkillModal(skill); $event.stopPropagation()
                    "
                  >
                    <i class="bi bi-pencil"></i>
                  </button>
                  <button
                    class="btn btn-light btn-sm"
                    (click)="
                      openDeleteSkillModal(skill); $event.stopPropagation()
                    "
                  >
                    <i class="bi bi-trash"></i>
                  </button>
                  <i
                    class="bi"
                    [class.bi-chevron-down]="!skill.expanded"
                    [class.bi-chevron-up]="skill.expanded"
                  ></i>
                </div>
              </div>

              <!-- Sub Skills -->
              <div class="sub-skills" *ngIf="skill.expanded">
                <div *ngIf="skill.subSkills.length > 0; else noSubSkills">
                  <div
                    *ngFor="let subSkill of skill.subSkills"
                    class="sub-skill-item"
                  >
                    <div
                      class="d-flex justify-content-between align-items-center"
                    >
                      <div>
                        <h4 class="sub-skill-title">{{ subSkill.name }}</h4>
                        <p class="sub-skill-description">
                          {{ subSkill.description }}
                        </p>
                      </div>
                      <div class="sub-skill-actions">
                        <button
                          class="btn btn-light btn-sm me-2"
                          (click)="
                            openEditSubSkillModal(skill, subSkill);
                            $event.stopPropagation()
                          "
                        >
                          <i class="bi bi-pencil"></i>
                        </button>
                        <button
                          class="btn btn-light btn-sm"
                          (click)="
                            openDeleteSubSkillModal(skill, subSkill);
                            $event.stopPropagation()
                          "
                        >
                          <i class="bi bi-trash"></i>
                        </button>
                      </div>
                    </div>
                  </div>
                </div>
                <ng-template #noSubSkills>
                  <div class="text-center py-3 text-muted">
                    No Sub-position exists
                  </div>
                </ng-template>
              </div>
            </div>

            <!-- Loading Indicator -->
            <div *ngIf="isLoading" class="text-center py-4">
              <div class="spinner-border text-primary" role="status">
                <span class="visually-hidden">Loading...</span>
              </div>
            </div>

            <!-- Pagination -->
            <app-pagination
              [currentPage]="currentPage"
              [pageSize]="pageSize"
              [totalItems]="totalCount"
              (pageChange)="onPageChange($event)"
            ></app-pagination>
          </div>
        </div>
      </main>

      <!-- Add Skill Modal -->
      <div
        class="modal"
        [class.show]="showAddSkillModal"
        [style.display]="showAddSkillModal ? 'block' : 'none'"
      >
        <div class="modal-dialog modal-dialog-centered">
          <div class="modal-content border-0">
            <div class="modal-header border-0">
              <h5 class="modal-title">Add New Skill</h5>
              <button
                type="button"
                class="btn-close"
                (click)="closeAddSkillModal()"
              ></button>
            </div>
            <div class="modal-body">
              <div class="mb-3">
                <label class="form-label">Name</label>
                <input
                  type="text"
                  class="form-control"
                  [(ngModel)]="newSkill.name"
                />
              </div>
              <div class="mb-3">
                <label class="form-label">Description</label>
                <textarea
                  class="form-control"
                  rows="3"
                  [(ngModel)]="newSkill.description"
                ></textarea>
              </div>
              <div class="mb-3">
                <label class="form-label">Seniority Levels</label>
                <select
                  class="form-select"
                  multiple
                  [(ngModel)]="newSkill.selectedSeniorityIds"
                >
                  <option
                    *ngFor="let seniority of seniorities"
                    [value]="seniority.id"
                  >
                    {{ seniority.name }} ({{ seniority.description }})
                  </option>
                </select>
              </div>
            </div>
            <div class="modal-footer border-0">
              <button
                type="button"
                class="btn btn-link text-dark"
                (click)="closeAddSkillModal()"
              >
                Cancel
              </button>
              <button
                type="button"
                class="btn btn-dark"
                [disabled]="!isValidSkill(newSkill)"
                (click)="addSkill()"
              >
                Add Skill
              </button>
            </div>
          </div>
        </div>
      </div>

      <!-- Edit Skill Modal -->
      <div
        class="modal"
        [class.show]="showEditSkillModal"
        [style.display]="showEditSkillModal ? 'block' : 'none'"
      >
        <div class="modal-dialog modal-dialog-centered">
          <div class="modal-content border-0">
            <div class="modal-header border-0">
              <h5 class="modal-title">Edit Skill</h5>
              <button
                type="button"
                class="btn-close"
                (click)="closeEditSkillModal()"
              ></button>
            </div>
            <div class="modal-body">
              <div class="mb-3">
                <label class="form-label">Name</label>
                <input
                  type="text"
                  class="form-control"
                  [(ngModel)]="editingSkill.name"
                />
              </div>
              <div class="mb-3">
                <label class="form-label">Description</label>
                <textarea
                  class="form-control"
                  rows="3"
                  [(ngModel)]="editingSkill.description"
                ></textarea>
              </div>
              <div class="mb-3">
                <label class="form-label">Seniority Levels</label>
                <select
                  class="form-select"
                  multiple
                  [(ngModel)]="editingSkill.selectedSeniorityIds"
                >
                  <option
                    *ngFor="let seniority of seniorities"
                    [value]="seniority.id"
                    [selected]="
                      editingSkill.selectedSeniorityIds?.includes(seniority.id)
                    "
                  >
                    {{ seniority.name }} ({{ seniority.description }})
                  </option>
                </select>
              </div>
            </div>
            <div class="modal-footer border-0">
              <button
                type="button"
                class="btn btn-link text-dark"
                (click)="closeEditSkillModal()"
              >
                Cancel
              </button>
              <button
                type="button"
                class="btn btn-dark"
                [disabled]="!isValidSkill(editingSkill)"
                (click)="updateSkill()"
              >
                Update Skill
              </button>
            </div>
          </div>
        </div>
      </div>

      <!-- Delete Skill Modal -->
      <div
        class="modal"
        [class.show]="showDeleteSkillModal"
        [style.display]="showDeleteSkillModal ? 'block' : 'none'"
      >
        <div class="modal-dialog modal-dialog-centered">
          <div class="modal-content border-0">
            <div class="modal-header border-0">
              <h5 class="modal-title">Delete Skill</h5>
              <button
                type="button"
                class="btn-close"
                (click)="closeDeleteSkillModal()"
              ></button>
            </div>
            <div class="modal-body">
              <p>
                Are you sure you want to delete
                <strong>{{ deletingSkill?.name }}</strong
                >? This action cannot be undone.
              </p>
            </div>
            <div class="modal-footer border-0">
              <button
                type="button"
                class="btn btn-link text-dark"
                (click)="closeDeleteSkillModal()"
              >
                Cancel
              </button>
              <button
                type="button"
                class="btn btn-danger"
                (click)="confirmDeleteSkill()"
              >
                Delete Skill
              </button>
            </div>
          </div>
        </div>
      </div>

      <!-- Add Sub Skill Modal -->
      <div
        class="modal"
        [class.show]="showAddSubSkillModal"
        [style.display]="showAddSubSkillModal ? 'block' : 'none'"
      >
        <div class="modal-dialog modal-dialog-centered">
          <div class="modal-content border-0">
            <div class="modal-header border-0">
              <h5 class="modal-title">
                Add Sub-Skill to {{ selectedSkill?.name }}
              </h5>
              <button
                type="button"
                class="btn-close"
                (click)="closeAddSubSkillModal()"
              ></button>
            </div>
            <div class="modal-body">
              <div class="mb-3">
                <label class="form-label">Name</label>
                <input
                  type="text"
                  class="form-control"
                  [(ngModel)]="newSubSkill.name"
                />
              </div>
              <div class="mb-3">
                <label class="form-label">Description</label>
                <textarea
                  class="form-control"
                  rows="3"
                  [(ngModel)]="newSubSkill.description"
                ></textarea>
              </div>
            </div>
            <div class="modal-footer border-0">
              <button
                type="button"
                class="btn btn-link text-dark"
                (click)="closeAddSubSkillModal()"
              >
                Cancel
              </button>
              <button
                type="button"
                class="btn btn-dark"
                [disabled]="!isValidSubSkill(newSubSkill)"
                (click)="addSubSkill()"
              >
                Add Sub-Skill
              </button>
            </div>
          </div>
        </div>
      </div>

      <!-- Edit Sub Skill Modal -->
      <div
        class="modal"
        [class.show]="showEditSubSkillModal"
        [style.display]="showEditSubSkillModal ? 'block' : 'none'"
      >
        <div class="modal-dialog modal-dialog-centered">
          <div class="modal-content border-0">
            <div class="modal-header border-0">
              <h5 class="modal-title">Edit Sub-Skill</h5>
              <button
                type="button"
                class="btn-close"
                (click)="closeEditSubSkillModal()"
              ></button>
            </div>
            <div class="modal-body">
              <div class="mb-3">
                <label class="form-label">Name</label>
                <input
                  type="text"
                  class="form-control"
                  [(ngModel)]="editingSubSkill.name"
                />
              </div>
              <div class="mb-3">
                <label class="form-label">Description</label>
                <textarea
                  class="form-control"
                  rows="3"
                  [(ngModel)]="editingSubSkill.description"
                ></textarea>
              </div>
            </div>
            <div class="modal-footer border-0">
              <button
                type="button"
                class="btn btn-link text-dark"
                (click)="closeEditSubSkillModal()"
              >
                Cancel
              </button>
              <button
                type="button"
                class="btn btn-dark"
                [disabled]="!isValidSubSkill(editingSubSkill)"
                (click)="updateSubSkill()"
              >
                Update Sub-Skill
              </button>
            </div>
          </div>
        </div>
      </div>

      <!-- Delete Sub Skill Modal -->
      <div
        class="modal"
        [class.show]="showDeleteSubSkillModal"
        [style.display]="showDeleteSubSkillModal ? 'block' : 'none'"
      >
        <div class="modal-dialog modal-dialog-centered">
          <div class="modal-content border-0">
            <div class="modal-header border-0">
              <h5 class="modal-title">Delete Sub-Skill</h5>
              <button
                type="button"
                class="btn-close"
                (click)="closeDeleteSubSkillModal()"
              ></button>
            </div>
            <div class="modal-body">
              <p>
                Are you sure you want to delete
                <strong>{{ deletingSubSkill?.name }}</strong
                >? This action cannot be undone.
              </p>
            </div>
            <div class="modal-footer border-0">
              <button
                type="button"
                class="btn btn-link text-dark"
                (click)="closeDeleteSubSkillModal()"
              >
                Cancel
              </button>
              <button
                type="button"
                class="btn btn-danger"
                (click)="confirmDeleteSubSkill()"
              >
                Delete Sub-Skill
              </button>
            </div>
          </div>
        </div>
      </div>

      <!-- Modal Backdrop -->
      <div
        class="modal-backdrop fade show"
        *ngIf="
          showAddSkillModal ||
          showEditSkillModal ||
          showDeleteSkillModal ||
          showAddSubSkillModal ||
          showEditSubSkillModal ||
          showDeleteSubSkillModal
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
      .skills-container {
        max-height: calc(100vh - 240px);
        overflow-y: auto;
        padding-right: 1rem;
      }
      .skill-card {
        background: white;
        border-radius: 12px;
        box-shadow: 0 4px 6px rgba(0, 0, 0, 0.05), 0 1px 3px rgba(0, 0, 0, 0.1);
        margin-bottom: 1.5rem;
      }
      .skill-header {
        padding: 1.5rem;
        cursor: pointer;
        display: flex;
        justify-content: space-between;
        align-items: center;
      }
      .skill-title {
        font-size: 1.125rem;
        font-weight: 600;
        margin: 0;
      }
      .skill-description {
        color: #6b7280;
        font-size: 0.875rem;
        margin: 0.25rem 0 0;
      }
      .badge {
        font-size: 0.75rem;
        padding: 0.35rem 0.65rem;
        border-radius: 6px;
        margin-right: 0.5rem;
      }
      .badge-beginner {
        background-color: #fef3c7;
        color: #d97706;
      }
      .badge-intermediate {
        background-color: #e0e7ff;
        color: #4f46e5;
      }
      .badge-advanced {
        background-color: #ecfdf5;
        color: #059669;
      }
      .sub-skills {
        padding: 1.5rem;
        background: #f9fafb;
        border-bottom-left-radius: 12px;
        border-bottom-right-radius: 12px;
      }

      .sub-skill-item {
        padding: 1rem;
        background: white;
        border-radius: 8px;
        margin-bottom: 0.75rem;
        box-shadow: 0 1px 2px rgba(0, 0, 0, 0.05);
      }

      /* Empty state message styling */
      .text-muted {
        color: #6b7280;
      }
      .sub-skill-item {
        padding: 1rem;
        background: white;
        border-radius: 8px;
        margin-bottom: 0.75rem;
      }
      @media (max-width: 991.98px) {
        .main-content {
          margin-left: 0;
          width: 100%;
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
export class SkillsComponent implements OnInit, OnDestroy {
  skills: Skill[] = [];
  seniorities: Seniority[] = [];
  currentPage = 1;
  pageSize = 10;
  totalCount = 0;
  isLoading = false;

  // Modal states
  showAddSkillModal = false;
  showEditSkillModal = false;
  showDeleteSkillModal = false;
  showAddSubSkillModal = false;
  showEditSubSkillModal = false;
  showDeleteSubSkillModal = false;

  // Form data
  newSkill: Partial<Skill> = { selectedSeniorityIds: [] };
  editingSkill: Partial<Skill> = { selectedSeniorityIds: [] };
  deletingSkill: Skill | null = null;
  selectedSkill: Skill | null = null;
  newSubSkill: Partial<SubSkill> = {};
  editingSubSkill: Partial<SubSkill> = {};
  deletingSubSkill: SubSkill | null = null;

  private subscriptions: Subscription[] = [];

  constructor(private skillService: SkillService) {}

  ngOnInit() {
    this.loadSkills();
    this.loadSeniorities();
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
        next: (response: SkillApiResponse) => {
          this.skills = response.items.map((skill) => ({
            ...skill,
            expanded: false,
            selectedSeniorityIds: skill.seniorityLevelJunctions.map(
              (j) => j.seniorityLevelId
            ),
          }));
          this.totalCount = response.totalCount;
          this.isLoading = false;
        },
        error: (error) => {
          console.error('Error loading skills:', error);
          this.isLoading = false;
        },
      });
    this.subscriptions.push(sub);
  }
  // Add this method to the SkillsComponent class
  onPageChange(page: number): void {
    this.currentPage = page;
    this.loadSkills();
  }

  getSeniorityBadgeClass(seniorityName: string): string {
    switch (seniorityName) {
      case 'Internship':
        return 'badge-beginner';
      case '0-1 yr experience':
        return 'badge-intermediate';
      case '2 yrs of experience':
        return 'badge-advanced';
      default:
        return 'badge-beginner';
    }
  }

  // Sub-skill modal handlers
  openAddSubSkillModal(skill: Skill): void {
    this.selectedSkill = skill;
    this.newSubSkill = {};
    this.showAddSubSkillModal = true;
  }

  // Skill modal handlers
  openDeleteSkillModal(skill: Skill): void {
    this.deletingSkill = skill;
    this.showDeleteSkillModal = true;
  }

  toggleSkill(skill: Skill) {
    skill.expanded = !skill.expanded;
  }

  loadSeniorities() {
    const sub = this.skillService
      .seniorityDetailsData({
        pageNumber: 1,
        pageSize: 100, // Adjust based on expected maximum seniority levels
      })
      .subscribe({
        next: (response: SeniorityApiResponse) => {
          this.seniorities = response.items;
        },
        error: (error: any) => {
          console.error('Error loading seniorities:', error);
        },
      });
    this.subscriptions.push(sub);
  }

  // Updated addSkill method
  addSkill() {
    if (this.isValidSkill(this.newSkill)) {
      this.isLoading = true;

      const skillData = {
        name: this.newSkill.name!,
        description: this.newSkill.description!,
        seniorityLevels: this.newSkill.selectedSeniorityIds || [],
      };

      const sub = this.skillService.skillCreatedData(skillData).subscribe({
        next: () => {
          this.loadSkills();
          this.closeAddSkillModal();
          this.isLoading = false;
        },
        error: (error) => {
          console.error('Error creating skill:', error);
          this.isLoading = false;
        },
      });
      this.subscriptions.push(sub);
    }
  }

  // Updated updateSkill method
  updateSkill(): void {
    if (this.isValidSkill(this.editingSkill)) {
      this.isLoading = true;

      const skillData = {
        id: this.editingSkill.id!,
        name: this.editingSkill.name!,
        description: this.editingSkill.description!,
        seniorityLevelIds: this.editingSkill.selectedSeniorityIds || [],
      };

      const sub = this.skillService.skillUpdatedData(skillData).subscribe({
        next: () => {
          this.loadSkills(); // Refresh the skills list
          this.closeEditSkillModal();
          this.isLoading = false;
        },
        error: (error) => {
          console.error('Error updating skill:', error);
          this.isLoading = false;
          // Optionally show error message to user
        },
      });
      this.subscriptions.push(sub);
    }
  }
  closeAddSkillModal() {
    this.showAddSkillModal = false;
    this.newSkill = {};
  }

  openEditSubSkillModal(skill: Skill, subSkill: SubSkill) {
    this.selectedSkill = skill;
    this.editingSubSkill = { ...subSkill };
    this.showEditSubSkillModal = true;
  }

  // Updated validation
  isValidSkill(skill: Partial<Skill>): boolean {
    return (
      !!skill.name &&
      !!skill.description &&
      (skill.selectedSeniorityIds?.length || 0) > 0
    );
  }

  // Updated modal open methods
  openAddSkillModal() {
    this.newSkill = { selectedSeniorityIds: [] };
    this.showAddSkillModal = true;
  }

  openEditSkillModal(skill: Skill) {
    this.editingSkill = {
      ...skill,
      selectedSeniorityIds: skill.seniorityLevelJunctions.map(
        (j) => j.seniorityLevelId
      ),
    };
    this.showEditSkillModal = true;
  }
  openDeleteSubSkillModal(skill: Skill, subSkill: SubSkill) {
    this.selectedSkill = skill;
    this.deletingSubSkill = subSkill;
    this.showDeleteSubSkillModal = true;
  }

  closeAddSubSkillModal() {
    this.showAddSubSkillModal = false;
    this.newSubSkill = {};
    this.selectedSkill = null;
  }

  closeEditSubSkillModal() {
    this.showEditSubSkillModal = false;
    this.editingSubSkill = {};
    this.selectedSkill = null;
  }

  closeDeleteSubSkillModal() {
    this.showDeleteSubSkillModal = false;
    this.deletingSubSkill = null;
    this.selectedSkill = null;
  }

  closeAllModals() {
    this.closeAddSkillModal();
    this.closeEditSkillModal();
    this.closeDeleteSkillModal();
    this.closeAddSubSkillModal();
    this.closeEditSubSkillModal();
    this.closeDeleteSubSkillModal();
  }

  closeDeleteSkillModal() {
    this.showDeleteSkillModal = false;
    this.deletingSkill = null;
  }

  closeEditSkillModal() {
    this.showEditSkillModal = false;
    this.editingSkill = {};
  }

  confirmDeleteSkill() {
    if (this.deletingSkill) {
      this.isLoading = true;
      const sub = this.skillService
        .skillDeletedData(this.deletingSkill.id)
        .subscribe({
          next: () => {
            this.loadSkills();
            this.closeDeleteSkillModal();
          },
          error: (error: any) => {
            console.error('Error deleting skill:', error);
            this.isLoading = false;
          },
        });
      this.subscriptions.push(sub);
    }
  }

  addSubSkill() {
    if (this.selectedSkill && this.isValidSubSkill(this.newSubSkill)) {
      this.isLoading = true;
      const sub = this.skillService
        .subskillCreatedData(this.selectedSkill.id, this.newSubSkill)
        .subscribe({
          next: () => {
            this.loadSkills();
            this.closeAddSubSkillModal();
          },
          error: (error: any) => {
            console.error('Error creating sub-skill:', error);
            this.isLoading = false;
          },
        });
      this.subscriptions.push(sub);
    }
  }

  updateSubSkill() {
    if (this.selectedSkill && this.isValidSubSkill(this.editingSubSkill)) {
      this.isLoading = true;
      const sub = this.skillService
        .subskillUpdatedData(this.editingSubSkill.id, this.editingSubSkill)
        .subscribe({
          next: () => {
            this.loadSkills();
            this.closeEditSubSkillModal();
          },
          error: (error) => {
            console.error('Error updating sub-skill:', error);
            this.isLoading = false;
          },
        });
      this.subscriptions.push(sub);
    }
  }

  confirmDeleteSubSkill() {
    if (this.selectedSkill && this.deletingSubSkill) {
      this.isLoading = true;
      const sub = this.skillService
        .subskillDeletedData(this.deletingSubSkill.id)
        .subscribe({
          next: () => {
            this.loadSkills();
            this.closeDeleteSubSkillModal();
          },
          error: (error) => {
            console.error('Error deleting sub-skill:', error);
            this.isLoading = false;
          },
        });
      this.subscriptions.push(sub);
    }
  }

  isValidSubSkill(subSkill: Partial<SubSkill>): boolean {
    return !!subSkill.name && !!subSkill.description;
  }
}
