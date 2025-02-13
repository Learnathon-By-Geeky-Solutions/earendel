import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-interview-setup',
  standalone: true,
  imports: [CommonModule],
  template: `
    <div class="container-fluid py-4">
      <div class="row">
        <div class="col-lg-8">
          <!-- Video Preview Section -->
          <div class="video-preview-container mb-4">
            <div class="video-preview">
              <div class="preview-placeholder">Camera Preview (Simulated)</div>
              <div class="controls">
                <button class="control-btn active">
                  <i class="bi bi-mic-fill"></i>
                </button>
                <button class="control-btn active">
                  <i class="bi bi-camera-video-fill"></i>
                </button>
              </div>
            </div>
            <div class="text-center mt-2 text-muted">
              Video and Audio Settings
            </div>
          </div>
        </div>

        <div class="col-lg-4">
          <!-- Interview Details -->
          <div class="interview-details">
            <!-- Seniority Card -->
            <div class="detail-card mb-4">
              <div class="row">
                <div class="col-6">
                  <div class="detail-section">
                    <h6 class="text-muted mb-2">Seniority</h6>
                    <p class="mb-0 fw-medium">Mid-senior (3-5 years)</p>
                  </div>
                </div>
                <div class="col-6">
                  <div class="detail-section">
                    <h6 class="text-muted mb-2">Duration</h6>
                    <p class="mb-0 fw-medium">
                      <i class="bi bi-clock me-1"></i>
                      1 hr
                    </p>
                  </div>
                </div>
              </div>
            </div>

            <!-- Difficulty Level Card -->
            <div class="detail-card mb-4">
              <div
                class="d-flex justify-content-between align-items-start mb-3"
              >
                <h6 class="text-muted mb-0">Difficulty level</h6>
                <a href="#" class="text-decoration-none"
                  >view all difficulty levels</a
                >
              </div>
              <div class="difficulty-box">
                <div class="d-flex justify-content-between align-items-center">
                  <span class="fw-medium">Medium - Ask coding questions</span>
                  <a href="#" class="text-decoration-none"
                    >view sample questions</a
                  >
                </div>
              </div>
            </div>

            <!-- Hiring Criteria Card -->
            <div class="detail-card">
              <h6 class="text-muted mb-3">Hiring Criteria</h6>
              <p class="text-muted small mb-3">
                Spend majority of the time testing these rubrics
              </p>

              <div class="criteria-box mb-2">
                <h6 class="mb-1">JavaScript (3)</h6>
                <p class="text-muted mb-0">
                  Understanding of redux fundamentals
                </p>
              </div>

              <div class="criteria-box">
                <h6 class="mb-1">React</h6>
                <p class="text-muted mb-0">Component lifecycle and hooks</p>
              </div>
            </div>
          </div>
        </div>
      </div>

      <!-- Footer -->
      <footer class="interview-footer">
        <div class="d-flex justify-content-end align-items-center gap-3">
          <div class="text-end">
            <span class="me-2">Joining as an Interviewer</span>
            <a href="#" class="text-decoration-none">Not logged in? Sign in</a>
          </div>
          <button class="btn btn-success px-4" (click)="joinInterview()">
            Join Interview
            <i class="bi bi-arrow-right ms-2"></i>
          </button>
        </div>
      </footer>
    </div>
  `,
  styles: [
    `
      .video-preview-container {
        background: white;
        border-radius: 8px;
        padding: 1rem;
      }

      .video-preview {
        position: relative;
        background: #1a1d21;
        border-radius: 8px;
        aspect-ratio: 16/9;
        overflow: hidden;
      }

      .preview-placeholder {
        position: absolute;
        top: 50%;
        left: 50%;
        transform: translate(-50%, -50%);
        color: white;
        font-size: 1.1rem;
      }

      .controls {
        position: absolute;
        bottom: 20px;
        left: 50%;
        transform: translateX(-50%);
        display: flex;
        gap: 1rem;
      }

      .control-btn {
        width: 48px;
        height: 48px;
        border-radius: 50%;
        border: none;
        background: #22c55e;
        color: white;
        font-size: 1.2rem;
        display: flex;
        align-items: center;
        justify-content: center;
        cursor: pointer;
        transition: all 0.2s;
      }

      .control-btn:hover {
        background: #16a34a;
      }

      .detail-card {
        background: white;
        border-radius: 8px;
        padding: 1.5rem;
        box-shadow: 0 1px 3px rgba(0, 0, 0, 0.1);
      }

      .difficulty-box {
        background: #f9fafb;
        border-radius: 6px;
        padding: 1rem;
      }

      .criteria-box {
        background: #f9fafb;
        border-radius: 6px;
        padding: 1rem;
      }

      .interview-footer {
        position: fixed;
        bottom: 0;
        left: 0;
        right: 0;
        background: white;
        padding: 1rem 2rem;
        border-top: 1px solid #e5e7eb;
      }

      .btn-success {
        background-color: #22c55e;
        border-color: #22c55e;
        padding: 0.625rem 1.25rem;
        font-weight: 500;
      }

      .btn-success:hover {
        background-color: #16a34a;
        border-color: #16a34a;
      }

      a {
        color: #22c55e;
      }

      a:hover {
        color: #16a34a;
      }
    `,
  ],
})
export class InterviewSetupComponent {
  constructor() {}

  joinInterview() {
    // Navigate to interview room
    // this.router.navigate(['/interview-room']);
  }
}
