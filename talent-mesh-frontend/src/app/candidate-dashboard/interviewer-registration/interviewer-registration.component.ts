import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { CandidateSidebarComponent } from '../candidate-sidebar/candidate-sidebar.component';

@Component({
  selector: 'app-interviewer-registration',
  standalone: true,
  imports: [CommonModule, FormsModule, CandidateSidebarComponent],
  template: `
    <div class="dashboard-container">
      <main class="main-content">
        <div class="registration-container">
          <h1>Become an Interviewer</h1>
          <p class="subtitle">
            Please provide the following documents to complete your
            registration.
          </p>

          <form
            (ngSubmit)="onSubmit()"
            #registrationForm="ngForm"
            class="registration-form"
          >
            <div class="form-group">
              <label for="workPermit">Work Permit Document</label>
              <div class="file-input-container">
                <input
                  type="file"
                  id="workPermit"
                  (change)="onFileSelected($event, 'workPermit')"
                  accept=".pdf,.jpg,.png"
                  required
                />
                <label for="workPermit" class="file-input-label">
                  <i class="bi bi-cloud-upload"></i>
                  <span>{{ files['workPermit']?.name || 'Choose file' }}</span>
                </label>
              </div>
            </div>

            <div class="form-group">
              <label for="idFront">ID Card (Front)</label>
              <div class="file-input-container">
                <input
                  type="file"
                  id="idFront"
                  (change)="onFileSelected($event, 'idFront')"
                  accept=".jpg,.png"
                  required
                />
                <label for="idFront" class="file-input-label">
                  <i class="bi bi-cloud-upload"></i>
                  <span>{{ files['idFront']?.name || 'Choose file' }}</span>
                </label>
              </div>
            </div>

            <div class="form-group">
              <label for="idBack">ID Card (Back)</label>
              <div class="file-input-container">
                <input
                  type="file"
                  id="idBack"
                  (change)="onFileSelected($event, 'idBack')"
                  accept=".jpg,.png"
                  required
                />
                <label for="idBack" class="file-input-label">
                  <i class="bi bi-cloud-upload"></i>
                  <span>{{ files['idBack']?.name || 'Choose file' }}</span>
                </label>
              </div>
            </div>

            <div class="form-group">
              <label for="resume">Resume/CV</label>
              <div class="file-input-container">
                <input
                  type="file"
                  id="resume"
                  (change)="onFileSelected($event, 'resume')"
                  accept=".pdf,.doc,.docx"
                  required
                />
                <label for="resume" class="file-input-label">
                  <i class="bi bi-cloud-upload"></i>
                  <span>{{ files['resume']?.name || 'Choose file' }}</span>
                </label>
              </div>
            </div>

            <button
              type="submit"
              class="submit-btn bg-dark"
              [disabled]="!registrationForm.form.valid"
            >
              Submit Application
            </button>
          </form>
        </div>
      </main>
    </div>
  `,
  styles: [
    `
      .dashboard-container {
        display: flex;
        min-height: 100vh;
        background-color: #f8f9fa;
      }

      .main-content {
        flex: 1;
        padding: 2rem;
        transition: margin-left 0.3s ease-in-out;
      }

      .registration-container {
        max-width: 600px;
        margin: 0 auto;
        background-color: #ffffff;
        border-radius: 8px;
        padding: 2rem;
        box-shadow: 0 4px 6px rgba(0, 0, 0, 0.1);
      }

      h1 {
        font-size: 24px;
        font-weight: 600;
        color: #333;
        margin-bottom: 0.5rem;
      }

      .subtitle {
        font-size: 14px;
        color: #666;
        margin-bottom: 2rem;
      }

      .registration-form {
        display: flex;
        flex-direction: column;
        gap: 1.5rem;
      }

      .form-group {
        display: flex;
        flex-direction: column;
        gap: 0.5rem;
      }

      label {
        font-size: 14px;
        font-weight: 500;
        color: #333;
      }

      .file-input-container {
        position: relative;
      }

      input[type='file'] {
        position: absolute;
        width: 1px;
        height: 1px;
        padding: 0;
        margin: -1px;
        overflow: hidden;
        clip: rect(0, 0, 0, 0);
        white-space: nowrap;
        border-width: 0;
      }

      .file-input-label {
        display: flex;
        align-items: center;
        gap: 0.5rem;
        padding: 0.75rem 1rem;
        background-color: #f0f0f0;
        border: 1px solid #ddd;
        border-radius: 4px;
        cursor: pointer;
        transition: background-color 0.2s ease;
      }

      .file-input-label:hover {
        background-color: #e8e8e8;
      }

      .file-input-label i {
        font-size: 1.25rem;
        color: #666;
      }

      .file-input-label span {
        font-size: 14px;
        color: #333;
        white-space: nowrap;
        overflow: hidden;
        text-overflow: ellipsis;
      }

      .submit-btn {
        background-color: #0066ff;
        color: #ffffff;
        border: none;
        border-radius: 4px;
        padding: 0.75rem 1rem;
        font-size: 16px;
        font-weight: 500;
        cursor: pointer;
        transition: background-color 0.2s ease;
      }

      .submit-btn:hover {
        background-color: #0052cc;
      }

      .submit-btn:disabled {
        background-color: #ccc;
        cursor: not-allowed;
      }

      @media (max-width: 768px) {
        .main-content {
          margin-left: 0;
          padding: 1rem;
        }

        .registration-container {
          padding: 1.5rem;
        }

        h1 {
          font-size: 20px;
        }

        .subtitle {
          font-size: 12px;
        }
      }
    `,
  ],
})
export class InterviewerRegistrationComponent {
  files: { [key: string]: File | null } = {
    workPermit: null,
    idFront: null,
    idBack: null,
    resume: null,
  };

  onFileSelected(event: Event, fileType: string) {
    const input = event.target as HTMLInputElement;
    if (input.files && input.files.length > 0) {
      this.files[fileType as keyof typeof this.files] = input.files[0];
    }
  }

  onSubmit() {
    console.log('Submitting files:', this.files);
    // Implement your file upload logic here
  }
}
