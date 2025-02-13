import { Component, HostListener } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { SidebarComponent } from '../sidebar/sidebar.component';

interface Skill {
  id: number;
  name: string;
  description?: string;
  icon: string;
  subSkills: SubSkill[];
  expanded?: boolean;
}

interface SubSkill {
  id: number;
  name: string;
  description?: string;
  level?: 'Beginner' | 'Intermediate' | 'Advanced';
}

@Component({
  selector: 'app-skills',
  standalone: true,
  imports: [CommonModule, FormsModule, SidebarComponent],
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

          <!-- Search and Filter -->
          <div class="card border-0 shadow-sm mb-4">
            <div class="card-body">
              <div class="row g-3">
                <div class="col-md-8">
                  <input
                    type="text"
                    class="form-control"
                    placeholder="Search skills..."
                    [(ngModel)]="searchQuery"
                  />
                </div>
                <div class="col-md-4">
                  <select class="form-select" [(ngModel)]="filterCategory">
                    <option value="">All Categories</option>
                    <option *ngFor="let skill of skills" [value]="skill.name">
                      {{ skill.name }}
                    </option>
                  </select>
                </div>
              </div>
            </div>
          </div>

          <!-- Skills List -->
          <div class="skills-container" #skillsContainer>
            <div *ngFor="let skill of filteredSkills" class="skill-card">
              <!-- Main Skill -->
              <div class="skill-header" (click)="toggleSkill(skill)">
                <div class="d-flex align-items-center gap-3">
                  <div class="skill-icon">
                    <i [class]="'bi ' + skill.icon"></i>
                  </div>
                  <div>
                    <h3 class="skill-title">{{ skill.name }}</h3>
                    <p class="skill-description" *ngIf="skill.description">
                      {{ skill.description }}
                    </p>
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
                <div
                  *ngFor="let subSkill of skill.subSkills"
                  class="sub-skill-item"
                >
                  <div
                    class="d-flex justify-content-between align-items-center"
                  >
                    <div>
                      <h4 class="sub-skill-title">{{ subSkill.name }}</h4>
                      <p
                        class="sub-skill-description"
                        *ngIf="subSkill.description"
                      >
                        {{ subSkill.description }}
                      </p>
                      <span
                        class="badge"
                        [ngClass]="getLevelBadgeClass(subSkill.level)"
                      >
                        {{ subSkill.level }}
                      </span>
                    </div>
                    <div class="sub-skill-actions">
                      <button
                        class="btn btn-light btn-sm me-2"
                        (click)="openEditSubSkillModal(skill, subSkill)"
                      >
                        <i class="bi bi-pencil"></i>
                      </button>
                      <button
                        class="btn btn-light btn-sm"
                        (click)="openDeleteSubSkillModal(skill, subSkill)"
                      >
                        <i class="bi bi-trash"></i>
                      </button>
                    </div>
                  </div>
                </div>
              </div>
            </div>

            <!-- Loading Indicator -->
            <div *ngIf="isLoading" class="text-center py-4">
              <div class="spinner-border text-primary" role="status">
                <span class="visually-hidden">Loading...</span>
              </div>
            </div>
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
                <label class="form-label">Icon</label>
                <select class="form-select" [(ngModel)]="newSkill.icon">
                  <option value="bi-code-square">Code</option>
                  <option value="bi-database">Database</option>
                  <option value="bi-cpu">System</option>
                  <option value="bi-braces">Frontend</option>
                  <option value="bi-server">Backend</option>
                  <option value="bi-diagram-3">Architecture</option>
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

      <!-- Add Sub-Skill Modal -->
      <div
        class="modal"
        [class.show]="showAddSubSkillModal"
        [style.display]="showAddSubSkillModal ? 'block' : 'none'"
      >
        <div class="modal-dialog modal-dialog-centered">
          <div class="modal-content border-0">
            <div class="modal-header border-0">
              <h5 class="modal-title">Add New Sub-Skill</h5>
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

              <div class="mb-3">
                <label class="form-label">Level</label>
                <select class="form-select" [(ngModel)]="newSubSkill.level">
                  <option value="Beginner">Beginner</option>
                  <option value="Intermediate">Intermediate</option>
                  <option value="Advanced">Advanced</option>
                </select>
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
                <label class="form-label">Icon</label>
                <select class="form-select" [(ngModel)]="editingSkill.icon">
                  <option value="bi-code-square">Code</option>
                  <option value="bi-database">Database</option>
                  <option value="bi-cpu">System</option>
                  <option value="bi-braces">Frontend</option>
                  <option value="bi-server">Backend</option>
                  <option value="bi-diagram-3">Architecture</option>
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

      <!-- Edit Sub-Skill Modal -->
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

              <div class="mb-3">
                <label class="form-label">Level</label>
                <select class="form-select" [(ngModel)]="editingSubSkill.level">
                  <option value="Beginner">Beginner</option>
                  <option value="Intermediate">Intermediate</option>
                  <option value="Advanced">Advanced</option>
                </select>
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
                Are you sure you want to delete the skill "{{
                  deletingSkill?.name
                }}"?
              </p>
              <p class="text-muted">This action cannot be undone.</p>
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
                (click)="deleteSkill()"
              >
                Delete Skill
              </button>
            </div>
          </div>
        </div>
      </div>

      <!-- Delete Sub-Skill Modal -->
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
                Are you sure you want to delete the sub-skill "{{
                  deletingSubSkill?.name
                }}"?
              </p>
              <p class="text-muted">This action cannot be undone.</p>
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
                (click)="deleteSubSkill()"
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
          showAddSubSkillModal ||
          showEditSkillModal ||
          showEditSubSkillModal ||
          showDeleteSkillModal ||
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
        border-radius: 8px;
        box-shadow: 0 1px 3px rgba(0, 0, 0, 0.1);
        margin-bottom: 1rem;
      }

      .skill-header {
        padding: 1.5rem;
        cursor: pointer;
        display: flex;
        justify-content: space-between;
        align-items: center;
        transition: background-color 0.2s;
      }

      .skill-header:hover {
        background-color: #f9fafb;
      }

      .skill-icon {
        width: 48px;
        height: 48px;
        background: #f3f4f6;
        border-radius: 8px;
        display: flex;
        align-items: center;
        justify-content: center;
        font-size: 1.5rem;
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

      .skill-actions {
        display: flex;
        align-items: center;
        gap: 0.5rem;
      }

      .sub-skills {
        padding: 0 1.5rem 1.5rem;
        background: #f9fafb;
        border-bottom-left-radius: 8px;
        border-bottom-right-radius: 8px;
      }

      .sub-skill-item {
        padding: 1rem;
        background: white;
        border-radius: 6px;
        margin-bottom: 0.5rem;
      }

      .sub-skill-item:last-child {
        margin-bottom: 0;
      }

      .sub-skill-title {
        font-size: 1rem;
        font-weight: 500;
        margin: 0;
      }

      .sub-skill-description {
        color: #6b7280;
        font-size: 0.875rem;
        margin: 0.25rem 0;
      }

      .badge {
        font-size: 0.75rem;
        padding: 0.25rem 0.5rem;
        border-radius: 4px;
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

      .sub-skill-actions {
        display: flex;
        gap: 0.5rem;
      }

      @media (max-width: 991.98px) {
        .main-content {
          margin-left: 0;
          width: 100%;
        }
      }
    `,
  ],
})
export class SkillComponent {
  searchQuery = '';
  filterCategory = '';
  isLoading = false;
  page = 1;

  // Modal states
  showAddSkillModal = false;
  showAddSubSkillModal = false;
  showEditSkillModal = false;
  showEditSubSkillModal = false;
  showDeleteSkillModal = false;
  showDeleteSubSkillModal = false;

  // Form data
  newSkill: Partial<Skill> = {};
  newSubSkill: Partial<SubSkill> = {};
  editingSkill: Partial<Skill> = {};
  editingSubSkill: Partial<SubSkill> = {};
  deletingSkill: Skill | null = null;
  deletingSubSkill: SubSkill | null = null;
  selectedSkill: Skill | null = null;

  skills: Skill[] = [
    {
      id: 1,
      name: 'Backend Development',
      description: 'Server-side programming and architecture',
      icon: 'bi-server',
      expanded: false,
      subSkills: [
        {
          id: 1,
          name: 'Python',
          description: 'Python programming with Django/Flask',
          level: 'Advanced',
        },
        {
          id: 2,
          name: 'Java',
          description: 'Java with Spring Boot',
          level: 'Intermediate',
        },
        {
          id: 3,
          name: 'Node.js',
          description: 'Server-side JavaScript',
          level: 'Advanced',
        },
      ],
    },
    {
      id: 2,
      name: 'Frontend Development',
      description: 'Client-side programming and UI/UX',
      icon: 'bi-braces',
      expanded: false,
      subSkills: [
        {
          id: 4,
          name: 'React',
          description: 'Modern React with Hooks',
          level: 'Advanced',
        },
        {
          id: 5,
          name: 'Angular',
          description: 'Enterprise Angular applications',
          level: 'Intermediate',
        },
      ],
    },
    {
      id: 3,
      name: 'System Design',
      description: 'Architecture and scalability',
      icon: 'bi-diagram-3',
      expanded: false,
      subSkills: [
        {
          id: 6,
          name: 'Distributed Systems',
          description: 'Scalable distributed architecture',
          level: 'Advanced',
        },
        {
          id: 7,
          name: 'Microservices',
          description: 'Microservices architecture patterns',
          level: 'Intermediate',
        },
      ],
    },
  ];

  get filteredSkills(): Skill[] {
    return this.skills.filter((skill) => {
      const matchesSearch =
        skill.name.toLowerCase().includes(this.searchQuery.toLowerCase()) ||
        skill.description
          ?.toLowerCase()
          .includes(this.searchQuery.toLowerCase()) ||
        skill.subSkills.some(
          (sub) =>
            sub.name.toLowerCase().includes(this.searchQuery.toLowerCase()) ||
            sub.description
              ?.toLowerCase()
              .includes(this.searchQuery.toLowerCase())
        );

      const matchesCategory =
        !this.filterCategory || skill.name === this.filterCategory;

      return matchesSearch && matchesCategory;
    });
  }

  @HostListener('scroll', ['$event'])
  onScroll(event: any) {
    const element = event.target;
    if (element.scrollHeight - element.scrollTop === element.clientHeight) {
      this.loadMore();
    }
  }

  loadMore() {
    if (!this.isLoading) {
      this.isLoading = true;
      // Simulate API call
      setTimeout(() => {
        this.page++;
        this.isLoading = false;
      }, 1000);
    }
  }

  toggleSkill(skill: Skill) {
    skill.expanded = !skill.expanded;
  }

  getLevelBadgeClass(level?: string): string {
    if (!level) return '';
    return `badge-${level.toLowerCase()}`;
  }

  // Modal handlers
  openAddSkillModal() {
    this.newSkill = {};
    this.showAddSkillModal = true;
  }

  openAddSubSkillModal(skill: Skill) {
    this.selectedSkill = skill;
    this.newSubSkill = {};
    this.showAddSubSkillModal = true;
  }

  openEditSkillModal(skill: Skill) {
    this.editingSkill = { ...skill };
    this.showEditSkillModal = true;
  }

  openEditSubSkillModal(skill: Skill, subSkill: SubSkill) {
    this.selectedSkill = skill;
    this.editingSubSkill = { ...subSkill };
    this.showEditSubSkillModal = true;
  }

  openDeleteSkillModal(skill: Skill) {
    this.deletingSkill = skill;
    this.showDeleteSkillModal = true;
  }

  openDeleteSubSkillModal(skill: Skill, subSkill: SubSkill) {
    this.selectedSkill = skill;
    this.deletingSubSkill = subSkill;
    this.showDeleteSubSkillModal = true;
  }

  closeAddSkillModal() {
    this.showAddSkillModal = false;
    this.newSkill = {};
  }

  closeAddSubSkillModal() {
    this.showAddSubSkillModal = false;
    this.newSubSkill = {};
    this.selectedSkill = null;
  }

  closeEditSkillModal() {
    this.showEditSkillModal = false;
    this.editingSkill = {};
  }

  closeEditSubSkillModal() {
    this.showEditSubSkillModal = false;
    this.editingSubSkill = {};
    this.selectedSkill = null;
  }

  closeDeleteSkillModal() {
    this.showDeleteSkillModal = false;
    this.deletingSkill = null;
  }

  closeDeleteSubSkillModal() {
    this.showDeleteSubSkillModal = false;
    this.deletingSubSkill = null;
    this.selectedSkill = null;
  }

  closeAllModals() {
    this.closeAddSkillModal();
    this.closeAddSubSkillModal();
    this.closeEditSkillModal();
    this.closeEditSubSkillModal();
    this.closeDeleteSkillModal();
    this.closeDeleteSubSkillModal();
  }

  // CRUD operations
  addSkill() {
    if (this.isValidSkill(this.newSkill)) {
      const newId = Math.max(...this.skills.map((s) => s.id)) + 1;
      const skill: Skill = {
        id: newId,
        name: this.newSkill.name!,
        description: this.newSkill.description,
        icon: this.newSkill.icon!,
        subSkills: [],
        expanded: false,
      };
      this.skills.push(skill);
      this.closeAddSkillModal();
    }
  }

  addSubSkill() {
    if (this.selectedSkill && this.isValidSubSkill(this.newSubSkill)) {
      const newId =
        Math.max(...this.selectedSkill.subSkills.map((s) => s.id), 0) + 1;
      const subSkill: SubSkill = {
        id: newId,
        name: this.newSubSkill.name!,
        description: this.newSubSkill.description,
        level: this.newSubSkill.level,
      };
      this.selectedSkill.subSkills.push(subSkill);
      this.closeAddSubSkillModal();
    }
  }

  updateSkill() {
    if (this.isValidSkill(this.editingSkill)) {
      const index = this.skills.findIndex((s) => s.id === this.editingSkill.id);
      if (index !== -1) {
        this.skills[index] = { ...this.skills[index], ...this.editingSkill };
        this.closeEditSkillModal();
      }
    }
  }

  updateSubSkill() {
    if (this.selectedSkill && this.isValidSubSkill(this.editingSubSkill)) {
      const skillIndex = this.skills.findIndex(
        (s) => s.id === this.selectedSkill!.id
      );
      if (skillIndex !== -1) {
        const subSkillIndex = this.skills[skillIndex].subSkills.findIndex(
          (s) => s.id === this.editingSubSkill.id
        );
        if (subSkillIndex !== -1) {
          this.skills[skillIndex].subSkills[subSkillIndex] = {
            ...(this.editingSubSkill as SubSkill),
          };
          this.closeEditSubSkillModal();
        }
      }
    }
  }

  deleteSkill() {
    if (this.deletingSkill) {
      this.skills = this.skills.filter((s) => s.id !== this.deletingSkill!.id);
      this.closeDeleteSkillModal();
    }
  }

  deleteSubSkill() {
    if (this.selectedSkill && this.deletingSubSkill) {
      const skillIndex = this.skills.findIndex(
        (s) => s.id === this.selectedSkill!.id
      );
      if (skillIndex !== -1) {
        this.skills[skillIndex].subSkills = this.skills[
          skillIndex
        ].subSkills.filter((s) => s.id !== this.deletingSubSkill!.id);
        this.closeDeleteSubSkillModal();
      }
    }
  }

  public isValidSkill(skill: Partial<Skill>): boolean {
    return !!(skill.name && skill.icon);
  }

  public isValidSubSkill(subSkill: Partial<SubSkill>): boolean {
    return !!(subSkill.name && subSkill.level);
  }
}
