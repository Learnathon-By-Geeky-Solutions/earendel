import {
  Component,
  HostListener,
  ChangeDetectorRef,
  OnInit,
  OnDestroy,
} from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';

@Component({
  selector: 'app-quiz-interface',
  standalone: true,
  imports: [CommonModule, FormsModule],
  template: `
    <div class="quiz-container">
      <div class="quiz-header">
        <h1>Candidate Quiz</h1>
        <div class="quiz-progress">
          <span
            >Question {{ currentQuestionIndex + 1 }} of
            {{ questions.length }}</span
          >
          <div class="timer">{{ formatTime(timeLeft) }}</div>
        </div>
        <div class="progress-bar">
          <div
            class="progress-fill"
            [style.width]="
              ((currentQuestionIndex + 1) / questions.length) * 100 + '%'
            "
          ></div>
        </div>
      </div>

      <div class="quiz-content">
        <div class="question">
          <h2>{{ currentQuestion.text }}</h2>
          <div class="options">
            <label
              *ngFor="let option of currentQuestion.options"
              class="option"
              [class.selected]="selectedAnswer === option"
            >
              <input
                type="radio"
                [value]="option"
                [(ngModel)]="selectedAnswer"
                name="answer"
              />
              <span class="radio-custom"></span>
              <span class="option-text">{{ option }}</span>
            </label>
          </div>
        </div>
      </div>

      <div class="quiz-footer">
        <button
          class="next-btn"
          (click)="nextQuestion()"
        >
          {{ isLastQuestion ? 'Finish Quiz' : 'Next Question' }}
        </button>
      </div>
    </div>
  `,
  styles: [
    `
      .quiz-container {
        max-width: 800px;
        margin: 0 auto;
        padding: 24px;
        user-select: none;
      }

      .quiz-header {
        margin-bottom: 32px;

        h1 {
          font-size: 24px;
          font-weight: 600;
          margin: 0 0 16px;
        }
      }

      .quiz-progress {
        display: flex;
        justify-content: space-between;
        align-items: center;
        margin-bottom: 8px;
        font-size: 14px;
        color: #666;
      }

      .timer {
        font-weight: 600;
        color: #333;
      }

      .progress-bar {
        height: 4px;
        background: #eee;
        border-radius: 2px;
        overflow: hidden;
      }

      .progress-fill {
        height: 100%;
        background: #0066ff;
        transition: width 0.3s ease;
      }

      .quiz-content {
        background: white;
        border-radius: 12px;
        padding: 24px;
        margin-bottom: 24px;
        box-shadow: 0 1px 3px rgba(0, 0, 0, 0.1);
      }

      .question {
        h2 {
          font-size: 18px;
          font-weight: 500;
          margin: 0 0 24px;
          color: #333;
        }
      }

      .options {
        display: flex;
        flex-direction: column;
        gap: 16px;
      }

      .option {
        display: flex;
        align-items: center;
        gap: 12px;
        padding: 16px;
        border: 1px solid #dee2e6;
        border-radius: 8px;
        cursor: pointer;
        transition: all 0.2s ease;

        &:hover {
          background: #f8f9fa;
        }

        &.selected {
          background: #f0f7ff;
          border-color: #0066ff;
        }

        input[type='radio'] {
          display: none;
        }
      }

      .radio-custom {
        width: 20px;
        height: 20px;
        border: 2px solid #dee2e6;
        border-radius: 50%;
        position: relative;

        &:after {
          content: '';
          position: absolute;
          top: 50%;
          left: 50%;
          transform: translate(-50%, -50%);
          width: 10px;
          height: 10px;
          background: #0066ff;
          border-radius: 50%;
          opacity: 0;
          transition: opacity 0.2s ease;
        }
      }

      input[type='radio']:checked + .radio-custom:after {
        opacity: 1;
      }

      .option-text {
        font-size: 16px;
        color: #333;
      }

      .quiz-footer {
        display: flex;
        justify-content: flex-end;
      }

      .next-btn {
        padding: 12px 24px;
        background: #0066ff;
        color: white;
        border: none;
        border-radius: 8px;
        font-size: 16px;
        cursor: pointer;
        transition: all 0.2s ease;

        &:hover:not(:disabled) {
          background: #0052cc;
        }

        &:disabled {
          opacity: 0.5;
          cursor: not-allowed;
        }
      }

      @media (max-width: 768px) {
        .quiz-container {
          padding: 16px;
        }

        .quiz-content {
          padding: 16px;
        }

        .option {
          padding: 12px;
        }
      }
    `,
  ],
})
export class QuizInterfaceComponent implements OnInit, OnDestroy {
  questions: any[] = [
    {
      id: 1,
      text: 'What is the capital of France?',
      options: ['London', 'Berlin', 'Paris', 'Madrid'],
      correctAnswer: 'Paris',
      timeLimit: 30,
    },
    {
      id: 2,
      text: 'What is the capital of Germany?',
      options: ['London', 'Berlin', 'Paris', 'Madrid'],
      correctAnswer: 'Berlin',
      timeLimit: 30,
    },
    {
      id: 3,
      text: 'What is the capital of Spain?',
      options: ['London', 'Berlin', 'Paris', 'Madrid'],
      correctAnswer: 'Madrid',
      timeLimit: 30,
    },
    {
      id: 4,
      text: 'What is the capital of Italy?',
      options: ['London', 'Berlin', 'Paris', 'Rome'],
      correctAnswer: 'Rome',
      timeLimit: 30,
    },
  ];

  currentQuestionIndex = 0;
  selectedAnswer = '';
  timeLeft = 30;
  timer: any;
  answers: string[] = [];
  isLastQuestion = false;
  quizStartTime: Date | null = null;
  quizEndTime: Date | null = null;

  constructor(private router: Router, private cdr: ChangeDetectorRef) {}
  @HostListener('window:blur')
  onWindowBlur() {
    alert(
      'Warning: Switching tabs or applications is not allowed during the quiz!'
    );
  }

  @HostListener('copy')
  @HostListener('cut')
  @HostListener('paste')
  onCopy(e: Event) {
    e.preventDefault();
    return false;
  }

  ngOnInit() {
    // Start timer after the first question is shown
    setTimeout(() => {
      this.quizStartTime = new Date();
      this.startTimer();
    }, 0); // delay to ensure the first question is rendered
  }

  ngOnDestroy() {
    this.clearTimer();
  }

  get currentQuestion() {
    return this.questions[this.currentQuestionIndex];
  }

  startTimer() {
    this.timeLeft = this.currentQuestion.timeLimit; // Ensure time is set to the correct limit for the current question
    this.timer = setInterval(() => {
      if (this.timeLeft > 0) {
        this.timeLeft--;
      } else {
        this.handleTimeUp();
      }
    }, 1000);
  }

  clearTimer() {
    if (this.timer) {
      clearInterval(this.timer);
    }
  }

  handleTimeUp() {
    this.answers.push('');
    this.moveToNextQuestion();
  }

  formatTime(seconds: number): string {
    return `00:${seconds.toString().padStart(2, '0')}`;
  }

  nextQuestion() {
    this.answers.push(this.selectedAnswer);
    this.moveToNextQuestion();
  }

  moveToNextQuestion() {
    this.clearTimer(); // Clear the timer before moving to the next question

    if (this.currentQuestionIndex < this.questions.length - 1) {
      this.currentQuestionIndex++;
      this.selectedAnswer = '';
      this.startTimer(); // Start timer for the next question
      this.isLastQuestion =
        this.currentQuestionIndex === this.questions.length - 1;
    } else {
      this.finishQuiz();
    }
  }

  calculateTimeTaken(): any {
    if (!this.quizStartTime || !this.quizEndTime) {
      return 'Time not recorded';
    }

    const timeTakenMs =
      this.quizEndTime.getTime() - this.quizStartTime.getTime();
    const totalSeconds = Math.floor(timeTakenMs / 1000);
    const minutes = Math.floor(totalSeconds / 60);
    const seconds = totalSeconds % 60;
    return { minutes, seconds };
    // return `${minutes} minute(s) and ${seconds} second(s)`;
  }

  finishQuiz() {
    this.quizEndTime = new Date(); // Record the end time
    const totalTimeTaken = this.calculateTimeTaken();
    const score = this.answers.reduce(
      (total, answer, index) =>
        total + (answer === this.questions[index].correctAnswer ? 1 : 0),
      0
    );

    // Navigate to results with score
    this.router.navigate(['/candidate-dashboard/quiz/results'], {
      state: {
        score,
        totalQuestions: this.questions.length,
        timeTaken: `${totalTimeTaken.minutes} minutes ${totalTimeTaken.seconds} seconds`, // Calculate actual time
        position: 'Software Engineer',
      },
    });
  }
}
