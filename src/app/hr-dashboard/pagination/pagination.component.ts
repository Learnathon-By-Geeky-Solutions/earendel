import { Component, EventEmitter, Input, Output } from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-pagination',
  standalone: true,
  imports: [CommonModule],
  template: `
    <div
      class="d-flex justify-content-between align-items-center px-4 py-3 border-top"
    >
      <div class="text-muted small">
        Showing {{ startIndex + 1 }} to {{ endIndex }} of
        {{ totalItems }} entries
      </div>
      <div class="d-flex gap-2">
        <button
          class="btn btn-outline-secondary btn-sm"
          [disabled]="currentPage === 1"
          (click)="onPageChange(currentPage - 1)"
        >
          Previous
        </button>

        <div class="d-flex gap-1">
          <button
            *ngFor="let page of visiblePages"
            class="btn btn-sm"
            [class.btn-dark]="page === currentPage"
            [class.btn-outline-secondary]="page !== currentPage"
            (click)="onPageChange(page)"
          >
            {{ page }}
          </button>
        </div>

        <button
          class="btn btn-outline-secondary btn-sm"
          [disabled]="currentPage === totalPages"
          (click)="onPageChange(currentPage + 1)"
        >
          Next
        </button>
      </div>
    </div>
  `,
  styles: [
    `
      .btn-sm {
        min-width: 32px;
      }
    `,
  ],
})
export class PaginationComponent {
  @Input() currentPage = 1;
  @Input() pageSize = 10;
  @Input() totalItems = 0;
  @Output() pageChange = new EventEmitter<number>();

  get totalPages(): number {
    return Math.ceil(this.totalItems / this.pageSize);
  }

  get startIndex(): number {
    return (this.currentPage - 1) * this.pageSize;
  }

  get endIndex(): number {
    return Math.min(this.startIndex + this.pageSize, this.totalItems);
  }

  get visiblePages(): number[] {
    const pages: number[] = [];
    let start = Math.max(1, this.currentPage - 2);
    const end = Math.min(this.totalPages, start + 4);

    if (end - start < 4) {
      start = Math.max(1, end - 4);
    }

    for (let i = start; i <= end; i++) {
      pages.push(i);
    }

    return pages;
  }

  onPageChange(page: number): void {
    this.pageChange.emit(page);
  }
}
