import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class BearImagesService {
  watchBearImages: string[] = [];
  hideBearImages: string[] = [];
  peakBearImages: string[] = [];

  constructor() {
    this.loadImages();
  }

  private loadImages(): void {
    // Generate image paths for each type
    const generatePaths = (prefix: string, count: number): string[] => {
      return Array.from({ length: count }, (_, i) => `/assets/img/${prefix}_${i}.png`);
    };

    // Watch bear has 21 images (0-20)
    this.watchBearImages = generatePaths('watch_bear', 21);
    
    // Hide bear has 6 images (0-5)
    this.hideBearImages = generatePaths('hide_bear', 6);
    
    // Peak bear has 4 images (0-3)
    this.peakBearImages = generatePaths('peak_bear', 4);
  }
} 