import { Component, ElementRef, QueryList, ViewChildren, AfterViewInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';

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
  selectedScore: number = 5;

  ngAfterViewInit() {
    this.updateAllSliders();
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

  getScoreLabel(): string {
    const labels: Record<number, string> = {
      1: 'Very Bad',
      2: 'Bad',
      3: 'Average',
      4: 'Good',
      5: 'Very Good'
    };
    return labels[this.selectedScore] || '';
  }

  getSliderWidth(score: number): number {
    // Calculate percentage width based on score (0-100%)
    return (score - 1) * 25;
  }

  updateSliderPosition(event: Event) {
    const input = event.target as HTMLInputElement;
    const value = parseInt(input.value, 10);
    const slider = input.closest('.custom-slider') as HTMLElement;
    
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
    console.log('Feedback submitted:', this.feedbackPairs);
    // Implement actual submission logic here
  }
}
