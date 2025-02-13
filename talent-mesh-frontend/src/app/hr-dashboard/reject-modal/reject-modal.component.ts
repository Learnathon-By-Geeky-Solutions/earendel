import { Component, Inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import {
  MatDialogRef,
  MAT_DIALOG_DATA,
  MatDialogModule,
} from '@angular/material/dialog';

@Component({
  selector: 'app-reject-modal',
  standalone: true,
  imports: [CommonModule, FormsModule, MatDialogModule],
  template: `
    <div class="modal-content">
      <div class="modal-header">
        <h5 class="modal-title">Reject Candidate</h5>
        <button
          type="button"
          class="btn-close"
          (click)="dialogRef.close()"
        ></button>
      </div>
      <div class="modal-body">
        <p>Are you sure you want to reject {{ data.candidate.name }}?</p>
        <div class="mb-3">
          <label for="rejectReason" class="form-label"
            >Reason for rejection</label
          >
          <textarea
            id="rejectReason"
            class="form-control"
            rows="3"
            [(ngModel)]="reason"
            placeholder="Please provide a reason for rejection..."
          ></textarea>
        </div>
      </div>
      <div class="modal-footer">
        <button
          type="button"
          class="btn btn-secondary"
          (click)="dialogRef.close()"
        >
          Cancel
        </button>
        <button
          type="button"
          class="btn btn-danger"
          [disabled]="!reason.trim()"
          (click)="confirm()"
        >
          Confirm Rejection
        </button>
      </div>
    </div>
  `,
})
export class RejectModalComponent {
  reason = '';

  constructor(
    public dialogRef: MatDialogRef<RejectModalComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any
  ) {}

  confirm() {
    this.dialogRef.close({ confirmed: true, reason: this.reason });
  }
}
