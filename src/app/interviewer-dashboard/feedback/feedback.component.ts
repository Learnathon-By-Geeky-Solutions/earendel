import { Component, ElementRef, QueryList, ViewChildren, AfterViewInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, Router } from '@angular/router';
import { finalize } from 'rxjs/operators';
import { InterviewerService, InterviewFeedback } from '../services/interviewer.service';

interface QuestionAnswerPair {
  question: string;
  answer: string;
  score: number;
}

@Component({
  selector: 'app-feedback',
  standalone: true,
  imports: [FormsModule, CommonModule],
  templateUrl: './feedback.component.html',
  styleUrl: './feedback.component.css'
})
export class FeedbackComponent implements AfterViewInit {
  @ViewChildren('sliderContainer') sliderContainers!: QueryList<ElementRef>;
  
  feedbackPairs: QuestionAnswerPair[] = [{ question: '', answer: '', score: 5 }];
  interviewId: string = '';
  isSubmitting: boolean = false;
  successMessage: string = '';
  errorMessage: string = '';
  
  // Dialog state
  showConfirmDialog: boolean = false;
  showPartialSuccessDialog: boolean = false;
  submittedCount: number = 0;
  totalCount: number = 0;

  constructor(
    private interviewerService: InterviewerService,
    private route: ActivatedRoute,
    private router: Router
  ) { }

  ngOnInit() {
    // Directly extract from query parameters, which is where the ID is in the interviewer-dashboard route
    this.route.queryParamMap.subscribe(queryParams => {
      const queryId = queryParams.get('interviewId');
      if (queryId) {
        this.interviewId = queryId;
        console.log('Interview ID from query params:', this.interviewId);
        
        // Clear any existing error message if we found the ID
        this.errorMessage = '';
      } else {
        console.warn('No interview ID found in URL query parameters');
        this.errorMessage = 'No interview ID found. Please make sure the URL includes the interviewId parameter.';
      }
    });
    
    // If the URL changes, we should also grab the interviewId from current window.location
    // This is a fallback in case Angular route handling doesn't catch it
    if (!this.interviewId) {
      const urlParams = new URLSearchParams(window.location.search);
      const urlInterviewId = urlParams.get('interviewId');
      
      if (urlInterviewId) {
        this.interviewId = urlInterviewId;
        console.log('Interview ID from window.location:', this.interviewId);
        this.errorMessage = '';
      }
    }
  }

  ngAfterViewInit() {
    this.updateAllSliders();
  }

  // Open the confirmation dialog before submitting feedback
  openConfirmDialog(): void {
    // First validate if we can proceed
    if (!this.validateForm()) {
      return;
    }
    
    this.showConfirmDialog = true;
  }
  
  // Close the confirmation dialog
  cancelConfirmation(): void {
    this.showConfirmDialog = false;
  }
  
  // Proceed with submission after confirmation
  confirmSubmission(): void {
    this.showConfirmDialog = false;
    this.submitFeedback();
  }
  
  // Show dialog for partial success
  showPartialSuccessConfirmation(submitted: number, total: number): void {
    this.submittedCount = submitted;
    this.totalCount = total;
    this.showPartialSuccessDialog = true;
  }
  
  // Close the partial success dialog
  cancelPartialConfirmation(): void {
    this.showPartialSuccessDialog = false;
  }
  
  // Navigate to dashboard after partial success
  confirmPartialSuccess(): void {
    this.showPartialSuccessDialog = false;
    this.router.navigate(['/interviewer-dashboard']);
  }

  updateAllSliders() {
    setTimeout(() => {
      this.sliderContainers.forEach((container) => {
        const slider = container.nativeElement;
        const score = parseInt(slider.getAttribute('data-value'), 10);
        this.setSliderThumbPosition(slider, score);
      });
    });
  }

  getScoreLabel(score: number): string {
    const labels: Record<number, string> = {
      1: 'Very Bad',
      2: 'Bad',
      3: 'Average',
      4: 'Good',
      5: 'Very Good'
    };
    return labels[score] || '';
  }

  getSliderWidth(score: number): number {
    // Calculate percentage width based on score (0-100%)
    return (score - 1) * 25;
  }

  updateSliderPosition(event: Event, pair: QuestionAnswerPair) {
    const input = event.target as HTMLInputElement;
    const value = parseInt(input.value, 10);
    const slider = input.closest('.custom-slider') as HTMLElement;
    
    pair.score = value;
    
    if (slider) {
      this.setSliderThumbPosition(slider, value);
    }
  }

  setSliderThumbPosition(slider: HTMLElement, value: number) {
    // Update the data attribute
    slider.setAttribute('data-value', value.toString());
    
    // Calculate the position percentage (0%, 25%, 50%, 75%, 100%)
    const position = (value - 1) * 25;
    
    // Update the range bar width
    const range = slider.querySelector('.slider-range') as HTMLElement;
    if (range) {
      range.style.width = `${position}%`;
    }
  }

  addMore(): void {
    this.feedbackPairs.push({ question: '', answer: '', score: 5 });
    
    // Update sliders after view has been updated
    setTimeout(() => this.updateAllSliders());
  }

  submitFeedback(): void {
    // Clear previous messages
    this.errorMessage = '';
    this.successMessage = '';

    // Show a specific warning about potential server issues
    console.warn("Note: The server has been returning 500 errors. This submission may fail due to server-side issues.");
    
    // Display initial status to user
    this.errorMessage = "Attempting to submit feedback... (Note: The server may be experiencing issues)";

    // Log the interview ID for debugging
    console.log('Using interview ID for submission:', this.interviewId);

    // Start submission process
    this.isSubmitting = true;

    // Convert feedbackPairs to InterviewFeedback array
    const feedbackItems: InterviewFeedback[] = this.feedbackPairs.map(pair => ({
      interviewId: this.interviewId,
      interviewQuestionText: pair.question,
      response: pair.answer,
      score: pair.score
    }));

    // Log the first feedback item for debugging
    if (feedbackItems.length > 0) {
      console.log('Sample feedback item:', JSON.stringify(feedbackItems[0], null, 2));
    }

    // Try to submit just the first item to avoid overwhelming the server
    this.interviewerService.submitInterviewFeedback(feedbackItems[0])
      .pipe(
        finalize(() => {
          this.isSubmitting = false;
        })
      )
      .subscribe({
        next: (response) => {
          console.log('First feedback submitted successfully:', response);
          this.successMessage = 'First feedback item submitted successfully!';
          
          // If first submission worked, try submitting the rest sequentially
          if (feedbackItems.length > 1) {
            this.errorMessage = "First submission succeeded. Attempting to submit remaining items...";
            this.submitRemainingFeedback(feedbackItems.slice(1));
          } else {
            this.errorMessage = '';
            // Redirect on successful submission when there's only one item
            setTimeout(() => {
              this.router.navigate(['/interviewer-dashboard']);
            }, 1500); // Short delay to show the success message
          }
        },
        error: (error) => {
          console.error('Error submitting feedback:', error);
          
          // Clear the previous warning message
          this.errorMessage = '';
          
          // Update with specific error based on the error we're seeing
          if (error.message && error.message.includes('500')) {
            this.errorMessage = 'The server is currently experiencing technical difficulties (500 Internal Server Error). Please try again later or contact support with the exact feedback details.';
          } else {
            this.errorMessage = error.message || 'Failed to submit feedback. Please try again.';
          }
          
          // Try to provide more specific error messages based on common issues
          if (error.status === 500) {
            this.errorMessage = 'Server Error (500): The server encountered an error while processing your feedback. Please try again later or contact support.';
          } else if (error.status === 401 || error.status === 403) {
            this.errorMessage = 'You are not authorized to submit feedback. Please check your login status.';
          } else if (error.status === 400) {
            this.errorMessage = 'The feedback data format was incorrect. Please check your inputs.';
          } else if (error.status === 404) {
            this.errorMessage = 'The interview ID may be invalid or not found.';
          }
        }
      });
  }

  // Helper method to submit remaining feedback items one by one
  submitRemainingFeedback(remainingItems: InterviewFeedback[]): void {
    if (remainingItems.length === 0) {
      this.successMessage = 'All feedback submitted successfully!';
      this.errorMessage = '';
      
      // Redirect to interviewer dashboard after successful submission
      setTimeout(() => {
        this.router.navigate(['/interviewer-dashboard']);
      }, 1500); // Short delay to show the success message
      
      return;
    }

    // Submit the first item from the remaining items
    this.interviewerService.submitInterviewFeedback(remainingItems[0])
      .subscribe({
        next: (response) => {
          console.log('Additional feedback submitted successfully:', response);
          // Continue with the rest
          this.submitRemainingFeedback(remainingItems.slice(1));
        },
        error: (error) => {
          console.error('Error submitting additional feedback:', error);
          this.errorMessage = `Submitted ${this.feedbackPairs.length - remainingItems.length} of ${this.feedbackPairs.length} items. Some items failed to submit.`;
          
          // Partial success - open the partial success dialog
          if (this.feedbackPairs.length - remainingItems.length > 0) {
            this.showPartialSuccessConfirmation(
              this.feedbackPairs.length - remainingItems.length,
              this.feedbackPairs.length
            );
          }
        }
      });
  }

  // Add a method to test if the ID is in UUID format
  isValidUUID(id: string): boolean {
    const uuidRegex = /^[0-9a-f]{8}-[0-9a-f]{4}-[0-9a-f]{4}-[0-9a-f]{4}-[0-9a-f]{12}$/i;
    return uuidRegex.test(id);
  }

  validateForm(): boolean {
    // Check if interview ID is available
    if (!this.interviewId) {
      this.errorMessage = 'Interview ID is missing. Cannot submit feedback.';
      return false;
    }

    // Check if interview ID is in the expected format (UUID)
    if (!this.isValidUUID(this.interviewId)) {
      this.errorMessage = `Invalid interview ID format: ${this.interviewId}. Expected a UUID.`;
      console.error('Invalid interview ID format:', this.interviewId);
      return false;
    }

    // Check if at least one question-answer pair exists
    if (this.feedbackPairs.length === 0) {
      this.errorMessage = 'At least one question and answer is required.';
      return false;
    }

    // Check if all questions and answers are filled
    const isValid = this.feedbackPairs.every(pair => 
      pair.question.trim() !== '' && pair.answer.trim() !== ''
    );

    if (!isValid) {
      this.errorMessage = 'Please fill in all questions and answers.';
    }

    return isValid;
  }
}
