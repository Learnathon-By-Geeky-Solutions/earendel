import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';
import { BearImagesService } from './bear-images.service';

export type InputFocus = 'EMAIL' | 'PASSWORD';

@Injectable({
  providedIn: 'root'
})
export class BearAnimationService {
  private currentFocusSubject = new BehaviorSubject<InputFocus>('EMAIL');
  private currentBearImageSubject = new BehaviorSubject<string | null>(null);
  private isAnimatingSubject = new BehaviorSubject<boolean>(false);
  
  private timeouts: any[] = [];
  private prevFocus: InputFocus = 'EMAIL';
  private prevShowPassword = false;

  get currentFocus$(): Observable<InputFocus> {
    return this.currentFocusSubject.asObservable();
  }

  get currentBearImage$(): Observable<string | null> {
    return this.currentBearImageSubject.asObservable();
  }

  get isAnimating$(): Observable<boolean> {
    return this.isAnimatingSubject.asObservable();
  }

  get currentFocus(): InputFocus {
    return this.currentFocusSubject.value;
  }

  get currentBearImage(): string | null {
    return this.currentBearImageSubject.value || 
      (this.bearImagesService.watchBearImages.length > 0 ? 
        this.bearImagesService.watchBearImages[0] : null);
  }

  get isAnimating(): boolean {
    return this.isAnimatingSubject.value;
  }

  constructor(private bearImagesService: BearImagesService) {
    // Set initial image
    if (this.bearImagesService.watchBearImages.length > 0) {
      this.currentBearImageSubject.next(this.bearImagesService.watchBearImages[0]);
    }
  }

  setCurrentFocus(focus: InputFocus): void {
    this.currentFocusSubject.next(focus);
  }

  updateAnimation(emailLength: number, showPassword: boolean): void {
    // Clear existing timeouts
    this.timeouts.forEach(timeout => clearTimeout(timeout));
    this.timeouts = [];

    const animateImages = (
      images: string[],
      interval: number,
      reverse = false,
      onComplete?: () => void,
    ) => {
      if (images.length === 0) {
        onComplete?.();
        return;
      }

      this.isAnimatingSubject.next(true);
      const imageSequence = reverse ? [...images].reverse() : images;

      imageSequence.forEach((img, index) => {
        const timeoutId = setTimeout(() => {
          this.currentBearImageSubject.next(img);
          if (index === imageSequence.length - 1) {
            this.isAnimatingSubject.next(false);
            onComplete?.();
          }
        }, index * interval);
        this.timeouts.push(timeoutId);
      });
    };

    // For email input, calculate which bear image to show based on email length
    const animateWatchingBearImages = () => {
      // Ensure we're selecting the right bear image based on email length
      const progress = Math.min(emailLength / 30, 1);
      const index = Math.min(
        Math.floor(progress * (this.bearImagesService.watchBearImages.length - 1)),
        this.bearImagesService.watchBearImages.length - 1,
      );
      
      // Select the appropriate image and log for debugging
      const selectedImage = this.bearImagesService.watchBearImages[Math.max(0, index)];
      console.log(`Email length: ${emailLength}, Selected image index: ${index}`);
      
      this.currentBearImageSubject.next(selectedImage);
      this.isAnimatingSubject.next(false);
    };

    // Animation Logic based on Focus and ShowPassword
    const currentFocus = this.currentFocus;
    if (currentFocus === 'EMAIL') {
      if (this.prevFocus === 'PASSWORD') {
        // Reverse hideBearImages when moving from PASSWORD to EMAIL
        animateImages(this.bearImagesService.hideBearImages, 60, true, animateWatchingBearImages);
      } else {
        animateWatchingBearImages();
      }
    } else if (currentFocus === 'PASSWORD') {
      if (this.prevFocus !== 'PASSWORD') {
        // First time entering password field
        animateImages(this.bearImagesService.hideBearImages, 40, false, () => {
          if (showPassword) {
            animateImages(this.bearImagesService.peakBearImages, 50);
          }
        });
      } else if (showPassword && this.prevShowPassword === false) {
        // Show password selected
        animateImages(this.bearImagesService.peakBearImages, 50);
      } else if (!showPassword && this.prevShowPassword === true) {
        // Hide password selected
        animateImages(this.bearImagesService.peakBearImages, 50, true);
      }
    }

    this.prevFocus = currentFocus;
    this.prevShowPassword = showPassword;
  }

  ngOnDestroy(): void {
    this.timeouts.forEach(timeout => clearTimeout(timeout));
  }
} 