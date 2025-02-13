import { Component, Inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import {
  MatDialogRef,
  MAT_DIALOG_DATA,
  MatDialogModule,
} from '@angular/material/dialog';
import { MatButtonModule } from '@angular/material/button';

@Component({
  selector: 'app-logout-modal',
  standalone: true,
  imports: [CommonModule, MatDialogModule, MatButtonModule],
  template: `
    <h2 mat-dialog-title>Logout Confirmation</h2>
    <mat-dialog-content>
      <p>Are you sure you want to logout?</p>
    </mat-dialog-content>
    <mat-dialog-actions align="end">
      <button mat-button [mat-dialog-close]="false">No</button>
      <button mat-button [mat-dialog-close]="true" cdkFocusInitial>Yes</button>
    </mat-dialog-actions>
  `,
  styles: [
    `
      :host {
        display: block;
        padding: 16px;
      }
    `,
  ],
})
export class LogoutModalComponent {
  constructor(
    public dialogRef: MatDialogRef<LogoutModalComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any
  ) {}
}
