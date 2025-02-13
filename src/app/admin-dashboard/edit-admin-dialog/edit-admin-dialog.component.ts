import { Component, Inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import {
  MatDialogModule,
  MatDialogRef,
  MAT_DIALOG_DATA,
} from '@angular/material/dialog';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatSelectModule } from '@angular/material/select';

@Component({
  selector: 'app-edit-admin-dialog',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    MatDialogModule,
    MatFormFieldModule,
    MatInputModule,
    MatButtonModule,
    MatSelectModule,
  ],
  template: `
    <h2 mat-dialog-title>Edit Admin</h2>
    <mat-dialog-content>
      <form #adminForm="ngForm">
        <mat-form-field appearance="fill" class="w-100 mb-3">
          <mat-label>Name</mat-label>
          <input matInput [(ngModel)]="admin.name" name="name" required />
        </mat-form-field>

        <mat-form-field appearance="fill" class="w-100 mb-3">
          <mat-label>Email</mat-label>
          <input
            matInput
            [(ngModel)]="admin.email"
            name="email"
            required
            type="email"
          />
        </mat-form-field>

        <mat-form-field appearance="fill" class="w-100 mb-3">
          <mat-label>Role</mat-label>
          <mat-select [(ngModel)]="admin.role" name="role" required>
            <mat-option value="Admin">Admin</mat-option>
            <mat-option value="Super Admin">Super Admin</mat-option>
          </mat-select>
        </mat-form-field>

        <div class="mb-3">
          <button mat-raised-button color="warn" (click)="resetPassword()">
            Reset Password
          </button>
        </div>
      </form>
    </mat-dialog-content>
    <mat-dialog-actions align="end">
      <button mat-button mat-dialog-close>Cancel</button>
      <button
        mat-raised-button
        color="primary"
        [disabled]="!adminForm.form.valid"
        (click)="onSubmit()"
      >
        Save Changes
      </button>
    </mat-dialog-actions>
  `,
  styles: [
    `
      mat-form-field {
        width: 100%;
      }
    `,
  ],
})
export class EditAdminDialogComponent {
  admin: any;

  constructor(
    public dialogRef: MatDialogRef<EditAdminDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any
  ) {
    this.admin = { ...data };
  }

  onSubmit() {
    this.dialogRef.close(this.admin);
  }

  resetPassword() {
    // In a real application, this would trigger a password reset email
    // For this example, we'll just simulate it
    alert('Password reset email sent to ' + this.admin.email);
  }
}
