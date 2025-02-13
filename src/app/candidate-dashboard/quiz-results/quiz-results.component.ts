import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';

@Component({
  selector: 'app-quiz-results',
  standalone: true,
  imports: [CommonModule],
  template: `
    <div class="results-container">
      <div class="results-card">
        <h1>Exam Results</h1>

        <div class="candidate-info">
          <h2>Hello, {{ result.candidateName }}</h2>
          <p>Here are your results for the {{ result.position }} position.</p>
        </div>

        <div class="score-section">
          <div class="score-header">
            <span>Your Score:</span>
            <span>{{ result.score }} / {{ result.totalQuestions }}</span>
          </div>
          <div class="score-bar">
            <div
              class="score-fill"
              [style.width]="(result.score / result.totalQuestions) * 100 + '%'"
            ></div>
          </div>
          <div class="score-percentage">
            {{ ((result.score / result.totalQuestions) * 100).toFixed(1) }}%
          </div>
        </div>

        <div class="details">
          <div class="detail-row">
            <span>Time Taken:</span>
            <span>{{ result.timeTaken }}</span>
          </div>
          <div class="detail-row">
            <span>Job Applied For:</span>
            <span>{{ result.position }}</span>
          </div>
        </div>

        <div class="monitoring-notice">
          <i class="bi bi-shield-check"></i>
          <div>
            <strong>AI Monitoring Reminder</strong>
            <p>
              Please note that this exam was monitored by our AI system. Any
              detected irregularities or attempts at cheating may result in
              disqualification. We appreciate your honest participation.
            </p>
          </div>
        </div>

        <div class="next-steps">
          <h3>Next Steps:</h3>
          <p>
            Our team will review your results and get back to you shortly
            regarding the next steps in the interview process. Thank you for
            your participation!
          </p>
        </div>

        <div class="action-buttons">
          <button class="primary-btn" (click)="retakeQuiz()">
            Retake Quiz
          </button>
          <button class="secondary-btn" (click)="goToDashboard()">
            Go to Dashboard
          </button>
        </div>
      </div>
    </div>
  `,
  styles: [
    `
      .results-container {
        min-height: 100vh;
        padding: 24px;
        display: flex;
        align-items: center;
        justify-content: center;
        background: #f8f9fa;
      }

      .results-card {
        background: white;
        border-radius: 12px;
        padding: 32px;
        max-width: 600px;
        width: 100%;
        box-shadow: 0 1px 3px rgba(0, 0, 0, 0.1);
      }

      h1 {
        font-size: 24px;
        font-weight: 600;
        margin: 0 0 24px;
        color: #333;
      }

      .candidate-info {
        margin-bottom: 32px;

        h2 {
          font-size: 20px;
          font-weight: 500;
          margin: 0 0 8px;
          color: #333;
        }

        p {
          margin: 0;
          color: #666;
        }
      }

      .score-section {
        margin-bottom: 32px;
      }

      .score-header {
        display: flex;
        justify-content: space-between;
        align-items: center;
        margin-bottom: 8px;
        font-size: 16px;
        color: #333;
        font-weight: 500;
      }

      .score-bar {
        height: 8px;
        background: #eee;
        border-radius: 4px;
        overflow: hidden;
        margin-bottom: 8px;
      }

      .score-fill {
        height: 100%;
        background: #0066ff;
        transition: width 0.3s ease;
      }

      .score-percentage {
        text-align: right;
        font-size: 14px;
        color: #666;
      }

      .details {
        margin-bottom: 32px;
      }

      .detail-row {
        display: flex;
        justify-content: space-between;
        padding: 12px 0;
        border-bottom: 1px solid #eee;
        font-size: 14px;

        &:last-child {
          border-bottom: none;
        }

        span:first-child {
          color: #666;
        }

        span:last-child {
          color: #333;
          font-weight: 500;
        }
      }

      .monitoring-notice {
        display: flex;
        gap: 16px;
        padding: 16px;
        background: #fff3e0;
        border-radius: 8px;
        margin-bottom: 32px;

        i {
          font-size: 24px;
          color: #f57c00;
        }

        strong {
          display: block;
          margin-bottom: 4px;
          color: #333;
        }

        p {
          margin: 0;
          font-size: 14px;
          color: #666;
        }
      }

      .next-steps {
        margin-bottom: 32px;

        h3 {
          font-size: 16px;
          font-weight: 500;
          margin: 0 0 8px;
          color: #333;
        }

        p {
          margin: 0;
          font-size: 14px;
          color: #666;
        }
      }

      .action-buttons {
        display: flex;
        gap: 16px;
      }

      .primary-btn,
      .secondary-btn {
        flex: 1;
        padding: 12px 24px;
        border: none;
        border-radius: 8px;
        font-size: 14px;
        cursor: pointer;
        transition: all 0.2s ease;
      }

      .primary-btn {
        background: #0066ff;
        color: white;

        &:hover {
          background: #0052cc;
        }
      }

      .secondary-btn {
        background: #f8f9fa;
        color: #666;
        border: 1px solid #dee2e6;

        &:hover {
          background: #e9ecef;
        }
      }

      @media (max-width: 768px) {
        .results-container {
          padding: 16px;
        }

        .results-card {
          padding: 24px;
        }

        .action-buttons {
          flex-direction: column;
        }
      }
    `,
  ],
})
export class QuizResultsComponent {
  result: any = {
    candidateName: 'John Doe',
    position: 'Software Engineer',
    score: 7,
    totalQuestions: 10,
    timeTaken: '8 minutes 30 seconds',
    monitoringStatus: 'clean',
  };

  constructor(private router: Router) {
    const navigation = this.router.getCurrentNavigation();
    if (navigation?.extras.state) {
      const state = navigation.extras.state as any;
      this.result = {
        ...this.result,
        score: state.score,
        totalQuestions: state.totalQuestions,
        timeTaken: state.timeTaken,
        position: state.position,
      };
    }
  }

  retakeQuiz() {
    this.router.navigate(['/candidate-dashboard/quiz/start']);
  }

  goToDashboard() {
    this.router.navigate(['/candidate-dashboard/quiz']);
  }
}
