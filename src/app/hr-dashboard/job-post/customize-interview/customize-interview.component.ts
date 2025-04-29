import { Component, type OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router, ActivatedRoute } from '@angular/router';
import { SidebarComponent } from '../../sidebar/sidebar.component';
import { JobService, Skill, SubSkill, SeniorityLevel, Rubric } from '../../services/job.service';
import { JobPostingService } from '../../services/job-posting.service';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-customized-interview',
  standalone: true,
  imports: [CommonModule, FormsModule],
  template: `
    <div class="container">
      <div class="main-content">
        <header class="header">
          <button class="back-btn" (click)="goBack()">
            <i class="bi bi-arrow-left"></i>
            Back to Seniority Selection
          </button>
        </header>

        <div class="content">
          <div class="skill-section">
            <h2>{{ getSelectedSkill()?.name}}</h2>
            <p class="subtitle">Choose to have skills</p>

            <div class="experience-level">
              <div class="level-info">
                <h3>{{ getSelectedSeniority()?.name || 'Mid Level Seniority' }}</h3>
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

            <!-- Job Posting Form -->
            <div class="job-posting-form">
              <h3 class="form-title">Finish Job Posting</h3>
              <p class="form-subtitle">Fill in the details for this job posting</p>
              
              <div class="card">
                <div class="card-content">
                  <div class="form-grid">
                    <!-- Job Title -->
                    <div class="form-group">
                      <label for="name" class="form-label">Job Title</label>
                      <input 
                        id="name" 
                        type="text" 
                        class="form-input" 
                        placeholder="e.g. Senior Frontend Developer" 
                        [(ngModel)]="formData.name">
                    </div>
                    
                    <!-- Location -->
                    <div class="form-group">
                      <label for="location" class="form-label">Location</label>
                      <input 
                        id="location" 
                        type="text" 
                        class="form-input" 
                        placeholder="e.g. New York, Remote" 
                        [(ngModel)]="formData.location">
                    </div>
                    
                    <!-- Job Type -->
                    <div class="form-group">
                      <label for="jobType" class="form-label">Job Type</label>
                      <div class="select-wrapper position-relative">
                        <select id="jobType" class="form-select" [(ngModel)]="formData.jobType">
                          <option value="" disabled selected>Select job type</option>
                          <option value="full-time">Full-time</option>
                          <option value="part-time">Part-time</option>
                          <option value="contract">Contract</option>
                          <option value="freelance">Freelance</option>
                          <option value="internship">Internship</option>
                        </select>
                        <i class="bi bi-chevron-down position-absolute" style="right: 10px; top: 50%; transform: translateY(-50%);"></i>
                      </div>
                    </div>
                    
                    <!-- Salary -->
                    <div class="form-group">
                      <label for="salary" class="form-label">Salary</label>
                      <input 
                        id="salary" 
                        type="text" 
                        class="form-input" 
                        placeholder="e.g. $80,000 - $120,000" 
                        [(ngModel)]="formData.salary">
                    </div>
                    
                    <!-- Number of Interviews -->
                    <div class="form-group number-interviews-group">
                      <label for="numberOfInterviews" class="form-label">Number of Interviews</label>
                      <div class="input-with-icon">
                        <i class="bi bi-briefcase icon-left"></i>
                        <input 
                          id="numberOfInterviews" 
                          type="number" 
                          class="form-input with-icon" 
                          placeholder="Enter number" 
                          [(ngModel)]="formData.numberOfInterviews">
                      </div>
                    </div>
                    
                    <!-- Description -->
                    <div class="form-group full-width">
                      <label for="description" class="form-label">Description</label>
                      <textarea 
                        id="description" 
                        class="form-textarea" 
                        placeholder="Enter job description" 
                        [(ngModel)]="formData.description"
                        rows="4"></textarea>
                    </div>
                    
                    <!-- Requirements -->
                    <div class="form-group full-width">
                      <label for="requirements" class="form-label">Requirements</label>
                      <textarea 
                        id="requirements" 
                        class="form-textarea" 
                        placeholder="Enter job requirements" 
                        [(ngModel)]="formData.requirements"
                        rows="4"></textarea>
                    </div>
                  </div>
                </div>
              </div>
            </div>

            <!-- Form submission status -->
            <div *ngIf="submissionStatus" class="submission-status" [ngClass]="submissionStatus">
              <i 
                class="bi" 
                [ngClass]="{'bi-check-circle-fill': submissionStatus === 'success', 'bi-x-circle-fill': submissionStatus === 'error', 'bi-hourglass-split': submissionStatus === 'loading'}"
              ></i>
              <span>{{ getStatusMessage() }}</span>
            </div>

            <div class="submit-section">
              <button 
                class="submit-btn" 
                (click)="submitJob()"
                [disabled]="isSubmitting || !isFormValid()">
                <div class="avatars">
                </div>
                <span *ngIf="!isSubmitting">Submit & Make Payment</span>
                <span *ngIf="isSubmitting">Submitting...</span>
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
        min-height: 100vh;
        background: #fff;
        display: flex;
        justify-content: center;
      }

      .main-content {
        width: 100%;
        max-width: 1000px;
        padding: 30px 20px;
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

      .skill-section {
        h2 {
          font-size: 28px;
          margin: 0 0 8px;
          font-weight: 600;
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
            font-size: 18px;
            margin: 0;
            font-weight: 500;
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
        background: #f8f9fa;

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

      .job-posting-form {
        margin-top: 40px;
        
        .form-title {
          font-size: 20px;
          margin: 0 0 8px;
          font-weight: 600;
        }
        
        .form-subtitle {
          color: #666;
          margin: 0 0 24px;
          font-size: 14px;
        }
        
        .card {
          border: 1px solid #e0e0e0;
          border-radius: 12px;
          overflow: hidden;
          margin-bottom: 24px;
          background: white;
        }
        
        .card-content {
          padding: 24px;
        }
        
        .form-grid {
          display: grid;
          grid-template-columns: 1fr 1fr;
          gap: 24px;
        }
        
        .form-group {
          display: flex;
          flex-direction: column;
          
          &.full-width {
            grid-column: 1 / -1;
          }
          
          &.number-interviews-group {
            grid-column: 1 / -1;
            max-width: 100%;
          }
        }
        
        .form-label {
          font-size: 14px;
          font-weight: 500;
          color: #444;
          margin-bottom: 8px;
        }
        
        .form-input,
        .form-select,
        .form-textarea {
          padding: 12px;
          border: 1px solid #e0e0e0;
          border-radius: 6px;
          font-size: 16px;
          background: #fff;
          width: 100%;
          
          &:focus {
            border-color: #0066ff;
            outline: none;
            box-shadow: 0 0 0 2px rgba(0,102,255,0.1);
          }
          
          &.with-icon {
            padding-left: 40px;
          }
        }
        
        .form-textarea {
          min-height: 120px;
          resize: vertical;
        }
        
        .input-with-icon {
          position: relative;
          width: 100%;
          
          .icon-left {
            position: absolute;
            left: 12px;
            top: 50%;
            transform: translateY(-50%);
            color: #0066ff;
            font-size: 18px;
          }
        }
      }

      .submission-status {
        display: flex;
        align-items: center;
        gap: 10px;
        padding: 16px;
        border-radius: 8px;
        margin: 16px 0;
        font-size: 14px;
        
        &.loading {
          background-color: #e3f2fd;
          color: #0277bd;
        }
        
        &.success {
          background-color: #e8f5e9;
          color: #2e7d32;
        }
        
        &.error {
          background-color: #ffebee;
          color: #c62828;
        }
        
        i {
          font-size: 18px;
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

        &:hover:not(:disabled) {
          background: #000;
        }
        
        &:disabled {
          background: #999;
          cursor: not-allowed;
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

      @media (max-width: 768px) {
        .form-grid {
          grid-template-columns: 1fr !important;
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
  
  // Job posting form data
  formData = {
    name: "",
    description: "",
    requirements: "",
    location: "",
    jobType: "",
    salary: "",
    numberOfInterviews: ""
  };
  
  // Submission status
  isSubmitting = false;
  submissionStatus: 'loading' | 'success' | 'error' | null = null;
  errorMessage = '';

  constructor(
    private router: Router, 
    private route: ActivatedRoute,
    private jobService: JobService,
    private jobPostingService: JobPostingService
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
  
  isFormValid(): boolean {
    // Check if required fields are filled
    return !!(
      this.formData.name &&
      this.formData.description &&
      this.formData.location &&
      this.formData.jobType &&
      this.formData.salary &&
      this.formData.numberOfInterviews
    );
  }
  
  getStatusMessage(): string {
    switch(this.submissionStatus) {
      case 'loading':
        return 'Submitting job posting...';
      case 'success':
        return 'Job posting created successfully!';
      case 'error':
        return this.errorMessage || 'An error occurred while creating the job posting.';
      default:
        return '';
    }
  }
  
  submitJob(): void {
    if (!this.isFormValid()) {
      this.submissionStatus = 'error';
      this.errorMessage = 'Please fill in all required fields.';
      return;
    }
    
    // Check localStorage directly
    try {
      const storedSeniority = localStorage.getItem('selectedSeniority');
      console.log('LocalStorage selectedSeniority:', storedSeniority ? JSON.parse(storedSeniority) : 'Not found');
      
      // If seniority is not in localStorage, let's store it
      if (!storedSeniority && this.getSelectedSeniority()) {
        const seniority = this.getSelectedSeniority();
        console.log('Storing seniority in localStorage:', seniority);
        localStorage.setItem('selectedSeniority', JSON.stringify(seniority));
      }
    } catch (e) {
      console.error('Error handling localStorage:', e);
    }
    
    this.isSubmitting = true;
    this.submissionStatus = 'loading';
    
    this.jobPostingService.createJobPosting(this.formData).subscribe({
      next: (response) => {
        console.log('Job posting created:', response);
        this.isSubmitting = false;
        this.submissionStatus = 'success';
        
        // Check if there's a payment URL in the response
        if (response && response.payment) {
          // Store job ID in localStorage for reference (optional)
          if (response.jobId) {
            localStorage.setItem('lastCreatedJobId', response.jobId);
          }
          
          // Show success message for a short time before redirecting
          this.errorMessage = `Job created successfully. Redirecting to payment...`;
          
          // Redirect to payment page after a short delay
          setTimeout(() => {
            window.location.href = response.payment;
          }, 1500);
        } else {
          // If no payment URL, just navigate to dashboard
          setTimeout(() => {
            this.router.navigate(['/hr-dashboard']);
          }, 2000);
        }
      },
      error: (error) => {
        console.error('============== API ERROR DETAILS ==============');
        console.error('Status:', error?.status);
        console.error('Status Text:', error?.statusText);
        console.error('Error:', error?.error);
        console.error('Message:', error?.message);
        
        this.isSubmitting = false;
        this.submissionStatus = 'error';
        
        if (error?.error?.message) {
          this.errorMessage = error.error.message;
        } else if (error?.message) {
          this.errorMessage = error.message;
        } else {
          this.errorMessage = 'Failed to create job posting. Please try again.';
        }
      }
    });
  }
}
