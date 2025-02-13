import { Component, Inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import {
  MatDialogModule,
   MatDialogRef,
  MAT_DIALOG_DATA,
} from '@angular/material/dialog';
import { MatButtonModule } from '@angular/material/button';

@Component({
  selector: 'app-remove-admin-dialog',
  standalone: true,
  imports: [CommonModule, MatDialogModule, MatButtonModule],
  template: `
    <h2 mat-dialog-title>Remove Admin</h2>
    <mat-dialog-content>
      Are you sure you want to remove the admin: <strong>{{ data.name }}</strong
      >?
    </mat-dialog-content>
    <mat-dialog-actions align="end">
      <button mat-button mat-dialog-close>Cancel</button>
      <button mat-raised-button color="warn" [mat-dialog-close]="true">
        Remove
      </button>
    </mat-dialog-actions>
  `,
})
export class RemoveAdminDialogComponent {
  constructor(
    public dialogRef: MatDialogRef<RemoveAdminDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: { name: string }
  ) {}
}
