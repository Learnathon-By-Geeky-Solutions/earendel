import { Component, type OnInit } from "@angular/core"
import { CommonModule } from "@angular/common"
import { RouterModule } from "@angular/router"
import { FormsModule } from "@angular/forms"
import { HeaderComponent } from "../header/header.component"

@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [CommonModule, RouterModule, FormsModule, HeaderComponent],
  template: `
    <div class="dashboard-wrapper">
      <app-header> </app-header>
      <!-- Main Content -->
      <main class="main-content">
        <div class="content-wrapper">
          <!-- Welcome Section -->
          <section class="welcome-section">
            <div class="welcome-left">
              <img
                src="https://placehold.jp/3d4070/ffffff/120x40.png?text=Talent%20Mesh"
                alt="User Avatar"
                class="welcome-avatar"
              />
              <div class="welcome-text">
                <h1>Hi, Md.</h1>
                <p>
                  Welcome to your mock interviews dashboard, here you will find
                  the scheduled interviews along with detailed history of the
                  ones you have completed
                </p>
              </div>
            </div>
          </section>

          <!-- Tabs Section -->
          <nav class="tabs-nav">
            <ul class="tabs-list">
              <li>
                <a
                  routerLink="candidate-dashboard"
                  style="text-decoration: none;"
                  [class.active]="currentTab === 'requested'"
                  (click)="changeTab('requested')"
                  >Requested</a
                >
              </li>
              <li>
                <a
                  routerLink="candidate-dashboard"
                  style="text-decoration: none;"
                  [class.active]="currentTab === 'completed'"
                  (click)="changeTab('completed')"
                  >Completed</a
                >
              </li>
              <li>
                <a
                  routerLink="candidate-dashboard"
                  style="text-decoration: none;"
                  [class.active]="currentTab === 'jobs'"
                  (click)="changeTab('jobs')"
                  >Job Posts</a
                >
              </li>
            </ul>
          </nav>

          <!-- Job Posts Modal -->
          <div class="modal" [class.show]="showJobPosts" *ngIf="showJobPosts">
            <div class="modal-content">
              <div class="modal-header">
                <h3>Active Job Posts</h3>
                <button class="close-btn" (click)="toggleJobPosts()">
                  <i class="bi bi-x-lg"></i>
                </button>
              </div>
              <div class="modal-body">
                <!-- Filters -->
                <div class="filters">
                  <input
                    type="text"
                    [(ngModel)]="searchTerm"
                    placeholder="Search jobs..."
                    class="search-input"
                  />
                  <select [(ngModel)]="filterType" class="filter-select">
                    <option value="all">All Types</option>
                    <option value="full-time">Full Time</option>
                    <option value="part-time">Part Time</option>
                    <option value="contract">Contract</option>
                  </select>
                  <select [(ngModel)]="filterLocation" class="filter-select">
                    <option value="all">All Locations</option>
                    <option value="remote">Remote</option>
                    <option value="onsite">On-site</option>
                    <option value="hybrid">Hybrid</option>
                  </select>
                </div>

                <!-- Job Cards -->
                <div class="job-cards">
                  <div class="job-card" *ngFor="let job of filteredJobs">
                    <div class="job-card-header">
                      <h4>{{ job.title }}</h4>
                      <span class="company">{{ job.company }}</span>
                    </div>
                    <div class="job-card-body">
                      <p><i class="bi bi-geo-alt"></i> {{ job.location }}</p>
                      <p><i class="bi bi-briefcase"></i> {{ job.type }}</p>
                      <p><i class="bi bi-cash"></i> {{ job.salary }}</p>
                      <p>
                        <i class="bi bi-clock"></i>
                        {{ job.experience }} experience
                      </p>
                    </div>
                    <div class="job-card-footer">
                      <button class="apply-btn">Apply Now</button>
                      <span class="deadline">Deadline: {{ job.deadline }}</span>
                    </div>
                  </div>
                </div>

                <!-- Pagination -->
                <div class="pagination">
                  <button
                    [disabled]="currentPage === 1"
                    (click)="changePage(currentPage - 1)"
                    class="page-btn"
                  >
                    Previous
                  </button>
                  <span class="page-info"
                    >Page {{ currentPage }} of {{ totalPages }}</span
                  >
                  <button
                    [disabled]="currentPage === totalPages"
                    (click)="changePage(currentPage + 1)"
                    class="page-btn"
                  >
                    Next
                  </button>
                </div>
              </div>
            </div>
          </div>

          <!-- Tab Content -->
          <div [ngSwitch]="currentTab">
            <!-- Requested Interviews -->
            <div *ngSwitchCase="'requested'" class="tab-content">
              <div *ngIf="requestedInterviews.length === 0" class="empty-state">
                <img
                  src="${'https://hebbkx1anhila5yf.public.blob.vercel-storage.com/C-1.PNG-cQZGogC3aRbDbZOUHCQQjVz97F3v6k.png'}"
                  alt="No interviews"
                  class="empty-icon"
                />
                <h2>You haven't scheduled any interviews</h2>
                <button class="request-btn">
                  Request Interview
                  <i class="bi bi-arrow-right"></i>
                </button>
              </div>
              <div
                *ngIf="requestedInterviews.length > 0"
                class="interviews-list"
              >
                <div
                  class="interview-card"
                  *ngFor="let interview of requestedInterviews"
                >
                  <div class="interview-header">
                    <h4>{{ interview.title }}</h4>
                    <span class="status scheduled">Scheduled</span>
                  </div>
                  <div class="interview-details">
                    <p>
                      <i class="bi bi-building"></i> {{ interview.company }}
                    </p>
                    <p><i class="bi bi-calendar"></i> {{ interview.date }}</p>
                    <p>
                      <i class="bi bi-person"></i> {{ interview.interviewer }}
                    </p>
                    <p><i class="bi bi-clock"></i> {{ interview.duration }}</p>
                  </div>
                </div>
              </div>
            </div>

            <!-- Completed Interviews -->
            <div *ngSwitchCase="'completed'" class="tab-content">
              <div *ngIf="completedInterviews.length === 0" class="empty-state">
                <img
                  src="${'https://hebbkx1anhila5yf.public.blob.vercel-storage.com/C-1.PNG-cQZGogC3aRbDbZOUHCQQjVz97F3v6k.png'}"
                  alt="No completed interviews"
                  class="empty-icon"
                />
                <h2>No completed interviews yet</h2>
                <button class="request-btn">
                  Request Interview
                  <i class="bi bi-arrow-right"></i>
                </button>
              </div>
              <div
                *ngIf="completedInterviews.length > 0"
                class="interviews-list"
              >
                <div
                  class="interview-card"
                  *ngFor="let interview of completedInterviews"
                >
                  <div class="interview-header">
                    <h4>{{ interview.title }}</h4>
                    <span class="status completed">Completed</span>
                  </div>
                  <div class="interview-details">
                    <p>
                      <i class="bi bi-building"></i> {{ interview.company }}
                    </p>
                    <p><i class="bi bi-calendar"></i> {{ interview.date }}</p>
                    <p>
                      <i class="bi bi-star-fill"></i> Score:
                      {{ interview.score }}/10
                    </p>
                  </div>
                  <div class="interview-feedback">
                    <h5>Feedback:</h5>
                    <p>{{ interview.feedback }}</p>
                  </div>
                </div>
              </div>
            </div>

            <!-- Job Posts -->
            <div *ngSwitchCase="'jobs'" class="tab-content">
              <div *ngIf="jobPosts.length === 0" class="empty-state">
                <img
                  src="${'https://hebbkx1anhila5yf.public.blob.vercel-storage.com/C-1.PNG-cQZGogC3aRbDbZOUHCQQjVz97F3v6k.png'}"
                  alt="No jobs"
                  class="empty-icon"
                />
                <h2>No active job posts</h2>
              </div>
              <div *ngIf="jobPosts.length > 0" class="job-cards">
                <div class="job-card" *ngFor="let job of filteredJobs">
                  <div class="job-card-header">
                    <h4>{{ job.title }}</h4>
                    <span class="company">{{ job.company }}</span>
                  </div>
                  <div class="job-card-body">
                    <p><i class="bi bi-geo-alt"></i> {{ job.location }}</p>
                    <p><i class="bi bi-briefcase"></i> {{ job.type }}</p>
                    <p><i class="bi bi-cash"></i> {{ job.salary }}</p>
                    <p>
                      <i class="bi bi-clock"></i>
                      {{ job.experience }} experience
                    </p>
                  </div>
                  <div class="job-card-footer">
                    <button class="apply-btn">Apply Now</button>
                    <span class="deadline">Deadline: {{ job.deadline }}</span>
                  </div>
                </div>
              </div>
            </div>

            <!-- Badges -->
            <div *ngSwitchCase="'badges'" class="tab-content">
              <!-- Badges content here -->
            </div>
          </div>
        </div>

        <!-- Sidebar -->
        <aside class="sidebar">
          <div class="resources-section">
            <h2>
              <i class="bi bi-lightbulb-fill"></i>
              Discover helpful resources
            </h2>

            <div class="section-content">
              <h3>Explore our blog</h3>
              <div class="blog-cards">
                <div class="blog-card">
                  <img
                    src="https://via.placeholder.com/300x200"
                    alt="Blog post image"
                  />
                  <div class="blog-content">
                    <h4>What is full-stack Development</h4>
                    <p>
                      Explore Full Stack Development: Covering Design, Creation,
                      Testing, and Deployment of Web Applications, Including
                      Databases, Front-End & Back-End Development.
                    </p>
                    <a href="#" class="read-more">Read more</a>
                  </div>
                </div>
              </div>
              <div class="pagination-dots">
                <span class="dot active"></span>
                <span class="dot"></span>
                <span class="dot"></span>
              </div>
            </div>

            <div class="section-content">
              <h3>Interview Questions</h3>
              <div class="question-card">
                <img
                  src="https://via.placeholder.com/300x200"
                  alt="Swift programming"
                />
                <div class="card-content">
                  <h4>Advanced Swift Interview Questions</h4>
                  <p>
                    Essential Swift interview questions to effectively assess
                    experienced candidates.
                  </p>
                  <a href="#" class="read-more">Read more</a>
                </div>
              </div>
            </div>

            <div class="section-content">
              <h3>Sample Resumes</h3>
              <div class="resume-card">
                <img
                  src="https://via.placeholder.com/300x200"
                  alt="Resume template"
                />
                <div class="card-content">
                  <h4>Angular Developer Resume</h4>
                  <p>Professional resume template for Angular developers.</p>
                  <a href="#" class="read-more">Read more</a>
                </div>
              </div>
            </div>
          </div>
        </aside>
      </main>
    </div>
  `,
  styles: [
    `
      :host {
        display: block;
        min-height: 100vh;
        background-color: #ffffff;
      }

      .dashboard-wrapper {
        min-height: 100vh;
        display: flex;
        flex-direction: column;
      }

      .header {
        display: flex;
        justify-content: space-between;
        align-items: center;
        padding: 1rem 2rem;
        background-color: #000000;
        color: white;
      }

      .header-right {
        display: flex;
        align-items: center;
        gap: 1.5rem;
      }

      .contact-btn {
        padding: 0.5rem 1rem;
        background: transparent;
        border: 1px solid rgba(255, 255, 255, 0.2);
        color: white;
        border-radius: 4px;
      }

      .user-profile {
        display: flex;
        align-items: center;
        gap: 0.5rem;
      }

      .avatar {
        width: 32px;
        height: 32px;
        background: #666;
        border-radius: 50%;
        display: flex;
        align-items: center;
        justify-content: center;
        font-size: 0.875rem;
      }

      .dropdown-toggle {
        background: transparent;
        border: none;
        color: white;
        display: flex;
        align-items: center;
        gap: 0.5rem;
      }

      .main-content {
        display: flex;
        flex: 1;
        gap: 2rem;
        padding: 2rem 2rem 2rem 4rem; /* Added more left padding */
      }

      .content-wrapper {
        flex: 1;
      }

      .welcome-section {
        display: flex;
        justify-content: space-between;
        align-items: flex-start;
        margin-bottom: 2rem;
      }

      .welcome-left {
        display: flex;
        gap: 1rem;
      }

      .welcome-avatar {
        width: 60px;
        height: 60px;
        border-radius: 50%;
      }

      .welcome-text h1 {
        font-size: 1.5rem;
        margin: 0;
      }

      .welcome-text p {
        color: #666;
        margin: 0.5rem 0 0;
      }

      .request-btn {
        display: flex;
        align-items: center;
        gap: 0.5rem;
        padding: 0.75rem 1.5rem;
        background-color: #000;
        color: white;
        border: none;
        border-radius: 6px;
        font-weight: 500;
        transition: all 0.2s ease;
        cursor: pointer;
      }

      .request-btn:hover {
        background-color: #333;
        transform: translateY(-1px);
      }

      .tabs-nav {
        margin-bottom: 2rem;
      }

      .tabs-list {
        list-style: none;
        padding: 0;
        margin: 0;
        display: flex;
        gap: 3rem; /* Increased gap between tabs */
        border-bottom: none; /* Remove bottom border */
      }

      .tabs-list a {
        padding: 1rem 0;
        color: #0066ff; /* Blue color */
        text-decoration: underline; /* Default underline */
        position: relative;
        font-weight: 500;
      }

      .tabs-list a.active {
        color: #0066ff;
        text-decoration: none; /* Remove default underline for active tab */
      }

      .tabs-list a.active::after {
        content: '';
        position: absolute;
        bottom: 0;
        left: 0;
        right: 0;
        height: 3px; /* Thicker underline for active tab */
        background-color: #0066ff;
        border-radius: 3px;
      }

      .empty-state {
        text-align: center;
        padding: 4rem 0;
      }

      .empty-icon {
        width: 120px;
        height: 120px;
        margin-bottom: 1.5rem;
      }

      .sidebar {
        width: 400px;
        background-color: #f8f9fa;
        padding: 2rem;
        border-radius: 8px;
      }

      .resources-section h2 {
        display: flex;
        align-items: center;
        gap: 0.5rem;
        font-size: 1.25rem;
        margin-bottom: 2rem;
      }

      .section-content {
        margin-bottom: 2rem;
      }

      .section-content h3 {
        font-size: 1rem;
        margin-bottom: 1rem;
      }

      .blog-cards,
      .question-card,
      .resume-card {
        background: white;
        border-radius: 8px;
        overflow: hidden;
        box-shadow: 0 2px 4px rgba(0, 0, 0, 0.05);
      }

      .blog-card img,
      .question-card img,
      .resume-card img {
        width: 100%;
        height: 200px;
        object-fit: cover;
      }

      .blog-content,
      .card-content {
        padding: 1rem;
      }

      .blog-content h4,
      .card-content h4 {
        font-size: 1rem;
        margin: 0 0 0.5rem;
      }

      .blog-content p,
      .card-content p {
        font-size: 0.875rem;
        color: #666;
        margin: 0 0 1rem;
      }

      .read-more {
        color: #0066cc;
        text-decoration: none;
        font-size: 0.875rem;
      }

      .pagination-dots {
        display: flex;
        justify-content: center;
        gap: 0.5rem;
        margin-top: 1rem;
      }

      .dot {
        width: 8px;
        height: 8px;
        border-radius: 50%;
        background-color: #ddd;
      }

      .dot.active {
        background-color: #666;
      }

      /* Dropdown Styles */
      .dropdown {
        position: relative;
      }

      .dropdown-menu {
        display: none;
        position: absolute;
        top: 100%;
        right: 0;
        background: white;
        border-radius: 8px;
        box-shadow: 0 2px 10px rgba(0, 0, 0, 0.1);
        min-width: 200px;
        z-index: 1000;
      }

      .dropdown-menu.show {
        display: block;
      }

      .dropdown-item {
        display: flex;
        align-items: center;
        gap: 0.5rem;
        padding: 0.75rem 1rem;
        color: #333;
        text-decoration: none;
        transition: background-color 0.2s ease;
      }

      .dropdown-item:hover {
        background-color: #f8f9fa;
      }

      /* Job Posts Modal */
      .modal {
        display: none;
        position: fixed;
        top: 0;
        left: 0;
        right: 0;
        bottom: 0;
        background-color: rgba(0, 0, 0, 0.5);
        z-index: 1000;
      }

      .modal.show {
        display: flex;
        align-items: center;
        justify-content: center;
      }

      .modal-content {
        background: white;
        border-radius: 12px;
        width: 90%;
        max-width: 900px;
        max-height: 90vh;
        overflow-y: auto;
      }

      .modal-header {
        display: flex;
        justify-content: space-between;
        align-items: center;
        padding: 1.5rem;
        border-bottom: 1px solid #eee;
      }

      .modal-body {
        padding: 1.5rem;
      }

      .close-btn {
        background: none;
        border: none;
        font-size: 1.5rem;
        cursor: pointer;
        color: #666;
      }

      /* Filters */
      .filters {
        display: flex;
        gap: 1rem;
        margin-bottom: 1.5rem;
      }

      .search-input,
      .filter-select {
        padding: 0.5rem 1rem;
        border: 1px solid #ddd;
        border-radius: 6px;
        font-size: 0.875rem;
      }

      .search-input {
        flex: 1;
      }

      /* Job Cards */
      .job-cards {
        display: grid;
        grid-template-columns: repeat(auto-fill, minmax(300px, 1fr));
        gap: 1.5rem;
        margin-bottom: 1.5rem;
      }

      .job-card {
        border: 1px solid #eee;
        border-radius: 8px;
        padding: 1.5rem;
        transition: all 0.2s ease;
      }

      .job-card:hover {
        transform: translateY(-2px);
        box-shadow: 0 4px 12px rgba(0, 0, 0, 0.1);
      }

      .job-card-header h4 {
        margin: 0;
        color: #333;
      }

      .job-card-body {
        margin: 1rem 0;
      }

      .job-card-body p {
        margin: 0.5rem 0;
        color: #666;
        display: flex;
        align-items: center;
        gap: 0.5rem;
      }

      .job-card-footer {
        display: flex;
        justify-content: space-between;
        align-items: center;
      }

      .apply-btn {
        background-color: #000;
        color: white;
        border: none;
        padding: 0.5rem 1rem;
        border-radius: 6px;
        cursor: pointer;
        transition: background-color 0.2s ease;
      }

      .apply-btn:hover {
        background-color: #333;
      }

      /* Interview Cards */
      .interviews-list {
        display: grid;
        gap: 1.5rem;
      }

      .interview-card {
        background: white;
        border-radius: 8px;
        padding: 1.5rem;
        box-shadow: 0 2px 4px rgba(0, 0, 0, 0.05);
      }

      .interview-header {
        display: flex;
        justify-content: space-between;
        align-items: center;
        margin-bottom: 1rem;
      }

      .status {
        padding: 0.25rem 0.75rem;
        border-radius: 999px;
        font-size: 0.875rem;
      }

      .status.scheduled {
        background-color: #e3f2fd;
        color: #1976d2;
      }

      .status.completed {
        background-color: #e8f5e9;
        color: #2e7d32;
      }

      /* Responsive Design */
      @media (max-width: 768px) {
        .main-content {
          flex-direction: column;
        }

        .sidebar {
          width: 100%;
          margin-top: 2rem;
        }

        .filters {
          flex-direction: column;
        }

        .job-cards {
          grid-template-columns: 1fr;
        }

        .welcome-section {
          flex-direction: column;
          gap: 1rem;
        }

        .welcome-left {
          flex-direction: column;
          text-align: center;
        }

        .welcome-avatar {
          margin: 0 auto;
        }
      }

      .tab-with-cursor {
        position: relative;
        padding-right: 1.5rem; /* Space for cursor icon */
      }

      .job-posts-btn {
        position: absolute;
        top: 50%;
        right: -20px;
        transform: translateY(-50%);
        color: #0066ff;
      }

      .pagination {
        display: flex;
        justify-content: center;
        gap: 1rem;
        margin-top: 1rem;
      }

      .page-btn {
        padding: 0.5rem 1rem;
        background-color: #000;
        color: white;
        border: none;
        border-radius: 6px;
        cursor: pointer;
      }

      .page-btn:disabled {
        opacity: 0.5;
        cursor: default;
      }

      .page-info {
        margin: 0 1rem;
      }
    `,
  ],
})
export class DashboardComponent implements OnInit {
  showDropdown = false;
  currentTab = 'requested'; // Updated default tab
  showJobPosts = false;
  searchTerm = '';
  filterType = 'all';
  filterLocation = 'all';
  currentPage = 1;
  itemsPerPage = 6;

  // Sample data
  jobPosts: any[] = [
    {
      id: 1,
      title: 'Full Stack Developer',
      company: 'Google',
      location: 'Remote',
      type: 'full-time',
      salary: '$120,000',
      experience: '3+',
      deadline: '2024-03-15',
    },
    {
      id: 2,
      title: 'Frontend Developer',
      company: 'Amazon',
      location: 'On-site',
      type: 'full-time',
      salary: '$100,000',
      experience: '2+',
      deadline: '2024-04-15',
    },
    {
      id: 3,
      title: 'Backend Developer',
      company: 'Microsoft',
      location: 'Hybrid',
      type: 'full-time',
      salary: '$110,000',
      experience: '2+',
      deadline: '2024-05-15',
    },
    {
      id: 4,
      title: 'React Developer',
      company: 'Facebook',
      location: 'Remote',
      type: 'full-time',
      salary: '$105,000',
      experience: '1+',
      deadline: '2024-06-15',
    },
    {
      id: 5,
      title: 'Angular Developer',
      company: 'Netflix',
      location: 'On-site',
      type: 'full-time',
      salary: '$115,000',
      experience: '3+',
      deadline: '2024-07-15',
    },
    {
      id: 6,
      title: 'Software Engineer',
      company: 'Apple',
      location: 'Hybrid',
      type: 'full-time',
      salary: '$130,000',
      experience: '4+',
      deadline: '2024-08-15',
    },
    {
      id: 7,
      title: 'Data Scientist',
      company: 'Uber',
      location: 'Remote',
      type: 'full-time',
      salary: '$140,000',
      experience: '5+',
      deadline: '2024-09-15',
    },
    {
      id: 8,
      title: 'Machine Learning Engineer',
      company: 'Tesla',
      location: 'On-site',
      type: 'full-time',
      salary: '$150,000',
      experience: '6+',
      deadline: '2024-10-15',
    },
  ];

  requestedInterviews: any[] = [
    {
      id: 1,
      title: 'Software Engineer Interview',
      company: 'Google',
      date: '2024-03-08',
      interviewer: 'John Doe',
      duration: '60 minutes',
    },
    {
      id: 2,
      title: 'Frontend Developer Interview',
      company: 'Amazon',
      date: '2024-03-15',
      interviewer: 'Jane Smith',
      duration: '45 minutes',
    },
  ];

  completedInterviews: any[] = [
    {
      id: 1,
      title: 'Full Stack Developer Interview',
      company: 'Google',
      date: '2024-02-28',
      score: 8,
      feedback:
        'Good performance, but needs improvement in problem-solving skills.',
    },
    {
      id: 2,
      title: 'Backend Developer Interview',
      company: 'Amazon',
      date: '2024-03-01',
      score: 9,
      feedback: 'Excellent performance, strong technical skills.',
    },
  ];

  get filteredJobs() {
    return this.jobPosts
      .filter((job) => {
        const matchesSearch =
          job.title.toLowerCase().includes(this.searchTerm.toLowerCase()) ||
          job.company.toLowerCase().includes(this.searchTerm.toLowerCase());
        const matchesType =
          this.filterType === 'all' ||
          job.type.toLowerCase() === this.filterType;
        const matchesLocation =
          this.filterLocation === 'all' ||
          job.location.toLowerCase().includes(this.filterLocation);
        return matchesSearch && matchesType && matchesLocation;
      })
      .slice(
        (this.currentPage - 1) * this.itemsPerPage,
        this.currentPage * this.itemsPerPage
      );
  }

  get totalPages() {
    return Math.ceil(this.jobPosts.length / this.itemsPerPage);
  }

  ngOnInit() {
    // Initialize with sample data
  }

  toggleDropdown() {
    this.showDropdown = !this.showDropdown;
  }

  closeDropdown() {
    this.showDropdown = false;
  }

  changeTab(tab: string) {
    this.currentTab = tab;
    if (tab === 'jobs') {
      this.toggleJobPosts();
    }
  }

  toggleJobPosts() {
    this.showJobPosts = !this.showJobPosts;
  }

  changePage(page: number) {
    if (page >= 1 && page <= this.totalPages) {
      this.currentPage = page;
    }
  }
}