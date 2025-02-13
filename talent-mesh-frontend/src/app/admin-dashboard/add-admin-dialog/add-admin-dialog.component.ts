import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { MatDialogModule, MatDialogRef } from '@angular/material/dialog';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatSelectModule } from '@angular/material/select';

@Component({
  selector: 'app-add-admin-dialog',
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
    <h2 mat-dialog-title>Add New Admin</h2>
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
        Add Admin
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
export class AddAdminDialogComponent {
  admin: any = {
    name: '',
    email: '',
    role: '',
  };

  constructor(public dialogRef: MatDialogRef<AddAdminDialogComponent>) {}

  onSubmit() {
    this.dialogRef.close(this.admin);
  }
}
