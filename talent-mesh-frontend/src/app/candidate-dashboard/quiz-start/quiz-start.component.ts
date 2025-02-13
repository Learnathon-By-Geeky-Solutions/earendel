import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import  { Router } from '@angular/router';

@Component({
  selector: 'app-quiz-start',
  standalone: true,
  imports: [CommonModule],
  template: `
    <div class="quiz-start-container">
      <div class="quiz-notice">
        <h1>Important Notice: AI-Monitored Quiz</h1>

        <div class="warning-box">
          <i class="bi bi-exclamation-triangle"></i>
          <div class="warning-content">
            <strong>Your activity will be tracked by AI</strong>
            <p>
              This quiz uses advanced AI technology to monitor your actions and
              ensure the integrity of the assessment process.
            </p>
          </div>
        </div>

        <div class="rules-section">
          <h2>Please be aware of the following:</h2>
          <ul>
            <li>
              <i class="bi bi-camera-video"></i>
              Your video feed will be monitored throughout the quiz.
            </li>
            <li>
              <i class="bi bi-clipboard"></i>
              Your clipboard activity will be tracked to prevent copy-pasting.
            </li>
            <li>
              <i class="bi bi-window"></i>
              Opening new browser tabs or switching applications is not allowed.
            </li>
            <li>
              <i class="bi bi-clock"></i>
              Each question has a time limit. Once the time is up, you'll
              automatically move to the next question.
            </li>
            <li>
              <i class="bi bi-arrow-left"></i>
              You cannot return to previous questions once answered.
            </li>
          </ul>
        </div>

        <div class="warning-message">
          Any suspicious activity detected by our AI system may result in
          disqualification from the hiring process.
        </div>

        <div class="consent-message">
          By clicking "Start Quiz", you acknowledge that you understand and
          agree to these conditions.
        </div>

        <button class="start-btn" (click)="startQuiz()">Start Quiz</button>
      </div>
    </div>
  `,
  styles: [
    `
      .quiz-start-container {
        min-height: 100vh;
        padding: 24px;
        display: flex;
        align-items: center;
        justify-content: center;
        background: #f8f9fa;
      }

      .quiz-notice {
        background: white;
        border-radius: 12px;
        padding: 32px;
        max-width: 800px;
        width: 100%;
        box-shadow: 0 1px 3px rgba(0, 0, 0, 0.1);
      }

      h1 {
        font-size: 24px;
        font-weight: 600;
        margin: 0 0 24px;
        color: #333;
      }

      .warning-box {
        display: flex;
        gap: 16px;
        padding: 16px;
        background: #fff3e0;
        border-radius: 8px;
        margin-bottom: 24px;

        i {
          font-size: 24px;
          color: #f57c00;
        }

        .warning-content {
          strong {
            display: block;
            margin-bottom: 4px;
            color: #333;
          }

          p {
            margin: 0;
            color: #666;
            font-size: 14px;
          }
        }
      }

      .rules-section {
        margin-bottom: 24px;

        h2 {
          font-size: 18px;
          font-weight: 600;
          margin: 0 0 16px;
          color: #333;
        }

        ul {
          list-style: none;
          padding: 0;
          margin: 0;

          li {
            display: flex;
            align-items: center;
            gap: 12px;
            padding: 12px 0;
            color: #666;
            font-size: 14px;

            i {
              font-size: 18px;
              color: #999;
            }
          }
        }
      }

      .warning-message {
        padding: 16px;
        background: #feeeee;
        color: #dc3545;
        border-radius: 8px;
        margin-bottom: 24px;
        font-size: 14px;
      }

      .consent-message {
        text-align: center;
        color: #666;
        font-size: 14px;
        margin-bottom: 24px;
      }

      .start-btn {
        display: block;
        width: 100%;
        padding: 16px;
        background: #0066ff;
        color: white;
        border: none;
        border-radius: 8px;
        font-size: 16px;
        font-weight: 500;
        cursor: pointer;
        transition: all 0.2s ease;

        &:hover {
          background: #0052cc;
        }
      }

      @media (max-width: 768px) {
        .quiz-start-container {
          padding: 16px;
        }

        .quiz-notice {
          padding: 24px;
        }

        h1 {
          font-size: 20px;
        }
      }
    `,
  ],
})
export class QuizStartComponent {
  constructor(private router: Router) {}

  startQuiz() {
    this.router.navigate(['/candidate-dashboard/quiz/interface'])
  }
}
