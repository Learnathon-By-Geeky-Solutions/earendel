import { Component, Input } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';

interface NavItem {
  title: string;
  href: string;
}

@Component({
  selector: 'app-sidebar',
  standalone: true,
  imports: [CommonModule, RouterModule],
  template: `
    <nav class="nav flex-column">
      @for (item of items; track item.href) {
      <a [routerLink]="item.href" routerLinkActive="active" class="nav-link">
        {{ item.title }}
      </a>
      }
    </nav>
  `,
  styles: [
    `
      .nav-link {
        color: #333;
      }
      .nav-link.active {
        font-weight: bold;
        color: #007bff;
      }
    `,
  ],
})
export class SidebarComponent {
  @Input() items: NavItem[] = [];
}
