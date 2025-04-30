import { Component, Input } from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-bear-avatar',
  standalone: true,
  imports: [CommonModule],
  template: `
    <img 
      [src]="currentImage" 
      class="rounded-full transition-all duration-200 ease-in-out"
      [width]="size"
      [height]="size"
      style="object-fit: contain; transform: translate3d(0,0,0);"
      tabindex="-1"
      alt="Animated bear avatar"
    />
  `,
  styles: [`
    :host {
      display: block;
    }
  `]
})
export class BearAvatarComponent {
  @Input() currentImage: string = '';
  @Input() size: number = 130;
} 