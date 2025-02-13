import { Component, type OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import  { MatDialog } from '@angular/material/dialog';
import  { MatSnackBar } from '@angular/material/snack-bar';
import { SidebarComponent } from '../sidebar/sidebar.component';
import { AddAdminDialogComponent } from '../add-admin-dialog/add-admin-dialog.component';
import { RemoveAdminDialogComponent } from '../remove-admin-dialog/remove-admin-dialog.component';
import { EditAdminDialogComponent } from '../edit-admin-dialog/edit-admin-dialog.component';

interface Admin {
  id: number;
  name: string;
  email: string;
  role: string;
  lastLogin: Date;
}

@Component({
  selector: 'app-admin-list',
  standalone: true,
  imports: [CommonModule, SidebarComponent],
  template: `
    <div class="d-flex">
      <app-sidebar></app-sidebar>

      <main class="main-content bg-light">
        <div class="container-fluid py-4">
          <div class="d-flex justify-content-between align-items-center mb-4">
            <h1 class="h3 mb-0">Admin Management</h1>
            <button class="btn btn-primary" (click)="openAddAdminDialog()">
              <i class="bi bi-plus-lg me-2"></i>Add Admin
            </button>
          </div>

          <div class="card border-0 shadow-sm">
            <div class="card-body">
              <div class="table-responsive">
                <table class="table table-hover align-middle">
                  <thead class="table-light">
                    <tr>
                      <th>ID</th>
                      <th>Name</th>
                      <th>Email</th>
                      <th>Role</th>
                      <th>Last Login</th>
                      <th>Actions</th>
                    </tr>
                  </thead>
                  <tbody>
                    <tr *ngFor="let admin of visibleAdmins">
                      <td>{{ admin.id }}</td>
                      <td>{{ admin.name }}</td>
                      <td>{{ admin.email }}</td>
                      <td>
                        <span
                          class="badge"
                          [ngClass]="{
                            'bg-primary': admin.role === 'Super Admin',
                            'bg-secondary': admin.role === 'Admin'
                          }"
                        >
                          {{ admin.role }}
                        </span>
                      </td>
                      <td>{{ admin.lastLogin | date : 'medium' }}</td>
                      <td>
                        <button
                          class="btn btn-sm btn-outline-primary me-2"
                          (click)="openEditAdminDialog(admin)"
                        >
                          <i class="bi bi-pencil"></i>
                        </button>
                        <button
                          class="btn btn-sm btn-outline-danger"
                          (click)="openRemoveAdminDialog(admin)"
                        >
                          <i class="bi bi-trash"></i>
                        </button>
                      </td>
                    </tr>
                  </tbody>
                </table>
              </div>
              <div class="text-center mt-3">
                <button
                  *ngIf="showViewMore"
                  class="btn btn-primary"
                  (click)="viewMore()"
                >
                  View More
                </button>
                <button
                  *ngIf="!showViewMore && visibleAdmins.length > itemsPerPage"
                  class="btn btn-secondary"
                  (click)="hideItems()"
                >
                  Hide
                </button>
              </div>
            </div>
          </div>
        </div>
      </main>
    </div>
  `,
  styles: [
    `
      .main-content {
        margin-left: 240px;
        width: calc(100% - 240px);
        min-height: 100vh;
      }

      .table th {
        font-weight: 600;
        text-transform: uppercase;
        font-size: 0.75rem;
        letter-spacing: 0.05em;
      }

      .badge {
        font-weight: 500;
      }

      @media (max-width: 991.98px) {
        .main-content {
          margin-left: 0;
          width: 100%;
        }
      }
    `,
  ],
})
export class AdminListComponent implements OnInit {
  admins: Admin[] = [
    {
      id: 1,
      name: 'John Doe',
      email: 'john@example.com',
      role: 'Super Admin',
      lastLogin: new Date('2023-07-20T10:30:00'),
    },
    {
      id: 2,
      name: 'Jane Smith',
      email: 'jane@example.com',
      role: 'Admin',
      lastLogin: new Date('2023-07-19T14:45:00'),
    },
    {
      id: 3,
      name: 'Mike Johnson',
      email: 'mike@example.com',
      role: 'Admin',
      lastLogin: new Date('2023-07-18T09:15:00'),
    },
    {
      id: 4,
      name: 'Emily Brown',
      email: 'emily@example.com',
      role: 'Super Admin',
      lastLogin: new Date('2023-07-17T16:20:00'),
    },
    {
      id: 5,
      name: 'Chris Lee',
      email: 'chris@example.com',
      role: 'Admin',
      lastLogin: new Date('2023-07-16T11:00:00'),
    },
    {
      id: 6,
      name: 'Sarah Wilson',
      email: 'sarah@example.com',
      role: 'Admin',
      lastLogin: new Date('2023-07-15T13:30:00'),
    },
    {
      id: 7,
      name: 'Tom Davis',
      email: 'tom@example.com',
      role: 'Super Admin',
      lastLogin: new Date('2023-07-14T08:45:00'),
    },
  ];
  visibleAdmins: Admin[] = [];
  itemsPerPage = 5;
  showViewMore = true;

  constructor(private dialog: MatDialog, private snackBar: MatSnackBar) {}

  ngOnInit(): void {
    this.visibleAdmins = this.admins.slice(0, this.itemsPerPage);
    this.updateViewMoreButton();
  }

  openAddAdminDialog() {
    const dialogRef = this.dialog.open(AddAdminDialogComponent, {
      width: '400px',
    });

    dialogRef.afterClosed().subscribe((result) => {
      if (result) {
        const newAdmin: Admin = {
          id: Math.max(...this.admins.map((a) => a.id)) + 1,
          name: result.name,
          email: result.email,
          role: result.role,
          lastLogin: new Date(),
        };
        this.admins = [...this.admins, newAdmin];
        this.visibleAdmins = this.admins.slice(
          0,
          this.visibleAdmins.length + 1
        );
        this.updateViewMoreButton();
        this.snackBar.open('New admin added successfully', 'Close', {
          duration: 3000,
        });
      }
    });
  }

  openEditAdminDialog(admin: Admin) {
    const dialogRef = this.dialog.open(EditAdminDialogComponent, {
      width: '400px',
      data: { ...admin },
    });

    dialogRef.afterClosed().subscribe((result) => {
      if (result) {
        const index = this.admins.findIndex((a) => a.id === result.id);
        if (index !== -1) {
          this.admins[index] = { ...this.admins[index], ...result };
          this.admins = [...this.admins];
          this.snackBar.open('Admin updated successfully', 'Close', {
            duration: 3000,
          });
        }
      }
    });
  }

  openRemoveAdminDialog(admin: Admin) {
    const dialogRef = this.dialog.open(RemoveAdminDialogComponent, {
      width: '400px',
      data: { name: admin.name },
    });

    dialogRef.afterClosed().subscribe((result) => {
      if (result) {
        this.admins = this.admins.filter((a) => a.id !== admin.id);
        this.visibleAdmins = this.visibleAdmins.filter(
          (a) => a.id !== admin.id
        );
        this.updateViewMoreButton();
        this.snackBar.open('Admin removed successfully', 'Close', {
          duration: 3000,
        });
      }
    });
  }

  viewMore() {
    const currentLength = this.visibleAdmins.length;
    const newItems = this.admins.slice(
      currentLength,
      currentLength + this.itemsPerPage
    );
    this.visibleAdmins = [...this.visibleAdmins, ...newItems];
    this.updateViewMoreButton();
  }

  hideItems() {
    this.visibleAdmins = this.admins.slice(0, this.itemsPerPage);
    this.updateViewMoreButton();
  }

  updateViewMoreButton() {
    this.showViewMore = this.visibleAdmins.length < this.admins.length;
  }
}
