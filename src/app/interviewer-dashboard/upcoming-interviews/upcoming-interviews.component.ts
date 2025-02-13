import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { SidebarComponent } from '../sidebar/sidebar.component';
import { PaginationComponent } from '../pagination/pagination.component';

interface Interview {
  candidate: string;
  role: string;
  company: string;
  date: string;
  time: string;
}

@Component({
  selector: 'app-upcoming-interviews',
  standalone: true,
  imports: [CommonModule, RouterModule, SidebarComponent, PaginationComponent],
  template: `
    <div class="d-flex">
      <app-sidebar> </app-sidebar>
      <div class="container-fluid py-4">
        <h1 class="h3 mb-2">Upcoming Interviews</h1>
        <p class="text-muted mb-4">
          You have {{ totalItems }} interviews scheduled
        </p>

        <div class="card border-2 shadow-md">
          <div class="card-body p-0">
            <div class="table-responsive">
              <table class="table table-hover mb-0">
                <thead>
                  <tr>
                    <th class="ps-4">Candidate</th>
                    <th>Role</th>
                    <th>Company</th>
                    <th>Date</th>
                    <th>Time</th>
                    <th class="pe-4">Actions</th>
                  </tr>
                </thead>
                <tbody>
                  <tr *ngFor="let interview of paginatedInterviews">
                    <td class="ps-4">{{ interview.candidate }}</td>
                    <td>{{ interview.role }}</td>
                    <td>{{ interview.company }}</td>
                    <td>{{ interview.date }}</td>
                    <td>{{ interview.time }}</td>
                    <td class="pe-4">
                      <button class="btn btn-outline-dark btn-sm">
                        View Details
                      </button>
                    </td>
                  </tr>
                </tbody>
              </table>
            </div>
            <div class="border-top">
              <app-pagination
                [currentPage]="currentPage"
                [pageSize]="pageSize"
                [totalItems]="totalItems"
                (pageChange)="onPageChange($event)"
              ></app-pagination>
            </div>
          </div>
        </div>
      </div>
    </div>
  `,
  styles: [
    `
      .nav-link {
        color: #666;
        transition: all 0.3s;
      }
      .nav-link:hover,
      .nav-link.active {
        background-color: #f8f9fa;
        color: #000;
      }
      .card {
        transition: transform 0.2s;
      }
      .card:hover {
        transform: translateY(-2px);
      }
      .activity-icon {
        width: 40px;
        height: 40px;
        display: flex;
        align-items: center;
        justify-content: center;
      }
      .upcoming-interviews {
        max-height: 300px;
        overflow-y: auto;
      }
      .table {
        margin-bottom: 0;
      }
      .table th {
        border-top: none;
        border-bottom-width: 1px;
        font-weight: 500;
        text-transform: uppercase;
        font-size: 0.75rem;
        color: #6c757d;
        padding: 1rem;
      }
      .table td {
        padding: 1rem;
        vertical-align: middle;
      }
      .btn-outline-dark {
        border-color: #dee2e6;
      }
      .btn-outline-dark:hover {
        background-color: #000;
        border-color: #000;
      }
    `,
  ],
})
export class UpcomingInterviewsComponent {
  interviews: Interview[] = [
    {
      candidate: 'John Doe',
      role: 'Frontend Developer',
      company: 'TechCorp',
      date: '2023-07-20',
      time: '14:00',
    },
    {
      candidate: 'Jane Smith',
      role: 'Backend Developer',
      company: 'InnoSoft',
      date: '2023-07-21',
      time: '10:30',
    },
    {
      candidate: 'Mike Johnson',
      role: 'Full Stack Developer',
      company: 'WebWizards',
      date: '2023-07-22',
      time: '15:45',
    },
    {
      candidate: 'Emily Brown',
      role: 'DevOps Engineer',
      company: 'CloudTech',
      date: '2023-07-23',
      time: '11:00',
    },
    {
      candidate: 'Chris Lee',
      role: 'Mobile Developer',
      company: 'AppMakers',
      date: '2023-07-24',
      time: '13:30',
    },
    {
      candidate: 'Anna Taylor',
      role: 'Data Scientist',
      company: 'DataGenix',
      date: '2023-07-25',
      time: '10:00',
    },
    {
      candidate: 'David Brown',
      role: 'AI Engineer',
      company: 'AI Solutions',
      date: '2023-07-26',
      time: '12:00',
    },
    {
      candidate: 'Sophia Wilson',
      role: 'Product Manager',
      company: 'Innovators Inc.',
      date: '2023-07-27',
      time: '16:00',
    },
    {
      candidate: 'James Miller',
      role: 'QA Engineer',
      company: 'QualityFirst',
      date: '2023-07-28',
      time: '09:30',
    },
    {
      candidate: 'Mia Davis',
      role: 'UX Designer',
      company: 'DesignHub',
      date: '2023-07-29',
      time: '14:45',
    },
  ];

  currentPage = 1;
  pageSize = 5;
  totalItems = this.interviews.length;

  get paginatedInterviews(): Interview[] {
    const startIndex = (this.currentPage - 1) * this.pageSize;
    const endIndex = startIndex + this.pageSize;
    return this.interviews.slice(startIndex, endIndex);
  }

  onPageChange(page: number): void {
    this.currentPage = page;
  }
}
