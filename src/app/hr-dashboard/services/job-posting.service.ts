import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable, throwError, switchMap } from 'rxjs';
import { catchError, tap, map } from 'rxjs/operators';
import { endpoint } from '../../endpoints/endpoint';
import { JobService, Skill, SubSkill, SeniorityLevel } from './job.service';

export interface JobPostingRequest {
  name: string;
  description: string;
  requirments: string; // Note: API typo in "requirments" instead of "requirements"
  location: string;
  jobType: string;
  experienceLevel: string;
  salary: string;
  postedBy: string;
  numberOfInterviews: number;
  requiredSkillIds: string[];
  requiredSubskillIds: string[];
}

export interface JobPosting {
  id: string;
  name: string;
  description: string;
  requirements: string;
  location: string;
  jobType: string;
  experienceLevel: string;
  salary: string;
  postedBy: string;
  createdAt: string;
  updatedAt: string;
  status: string;
  numberOfInterviews: number;
  requiredSkillIds: string[];
  requiredSubskillIds: string[];
  candidates?: any[];
}

export interface PaginatedJobPostings {
  content: JobPosting[];
  pageNumber: number;
  pageSize: number;
  totalElements: number;
  totalPages: number;
}

export interface JobApplication {
  id: string;
  jobId: string;
  candidateId: string;
  applicationDate: string;
  status: string;
  coverLetter: string;
  createdOn: string;
  jobName: string;
}

export interface UserDetails {
  id: string;
  userName: string;
  email: string;
  isActive: boolean;
  emailConfirmed: boolean;
  imageUrl: string;
  roles: string[];
}

@Injectable({
  providedIn: 'root'
})
export class JobPostingService {
  
  constructor(
    private http: HttpClient,
    private jobService: JobService
  ) { }

  /**
   * Create a new job posting
   */
  createJobPosting(data: {
    name: string;
    description: string;
    requirements: string;
    location: string;
    jobType: string;
    salary: string;
    numberOfInterviews: string | number;
  }): Observable<any> {
    // Build the request payload
    const payload = this.buildJobPostingRequest(data);
    
    // Get authorization token
    const token = sessionStorage.getItem('token');
    const headers: Record<string, string> = {
      'Content-Type': 'application/json'
    };
    
    if (token) {
      headers['Authorization'] = `Bearer ${token}`;
    }

    console.log('[JobPostingService] Sending job posting request:', payload);
    
    return this.http.post(endpoint.jobPostingUrl, payload, { headers })
      .pipe(
        tap(response => console.log('[JobPostingService] Job posting success response:', response)),
        catchError(err => {
          console.error('[JobPostingService] Error creating job posting:', err);
          return throwError(() => err);
        })
      );
  }

  /**
   * Build the job posting request object
   */
  private buildJobPostingRequest(data: {
    name: string;
    description: string;
    requirements: string;
    location: string;
    jobType: string;
    salary: string;
    numberOfInterviews: string | number;
  }): JobPostingRequest {
    // Get the selected skill from the job service (fallback to localStorage if needed)
    const selectedSkill = this.getSelectedSkill();
    const selectedSubSkill = this.getSelectedSubSkill();
    const selectedSeniority = this.getSelectedSeniority();
    
    // Get the user ID from session storage
    const userId = this.getUserId();
    
    // Get experience level from localStorage
    let experienceLevel = '';
    
    // Try to fetch directly from localStorage
    try {
      console.log('[JobPostingService] Trying to get selectedSeniority from localStorage');
      const storedSeniority = localStorage.getItem('selectedSeniority');
      if (storedSeniority) {
        const seniorityData = JSON.parse(storedSeniority);
        console.log('[JobPostingService] Found seniority data in localStorage:', seniorityData);
        
        // Extract title and description
        if (seniorityData) {
          // The object can have either name or title field
          const title = seniorityData.title || seniorityData.name || '';
          const description = seniorityData.description || '';
          
          // Format as "Title: Description"
          if (title && description) {
            experienceLevel = `${title}: ${description}`;
          } else if (title) {
            experienceLevel = title;
          }
          
          console.log('[JobPostingService] Formatted experience level:', experienceLevel);
        }
      }
    } catch (error) {
      console.error('[JobPostingService] Error reading experienceLevel from localStorage:', error);
    }
    
    // If still empty, try from the service
    if (!experienceLevel && selectedSeniority) {
      // SeniorityLevel only has name property, not title
      const name = selectedSeniority.name || '';
      const description = selectedSeniority.description || '';
      
      if (name && description) {
        experienceLevel = `${name}: ${description}`;
      } else if (name) {
        experienceLevel = name;
      }
      
      console.log('[JobPostingService] Using experience level from service:', experienceLevel);
    }
    
    console.log('[JobPostingService] Final user ID used:', userId);
    console.log('[JobPostingService] Final experience level used:', experienceLevel);

    // Build the request
    return {
      name: data.name,
      description: data.description,
      requirments: data.requirements, // Note: API typo in field name
      location: data.location,
      jobType: data.jobType,
      experienceLevel: experienceLevel,
      salary: data.salary,
      postedBy: userId,
      numberOfInterviews: typeof data.numberOfInterviews === 'string' ? 
        parseInt(data.numberOfInterviews, 10) || 0 : 
        data.numberOfInterviews || 0,
      requiredSkillIds: selectedSkill ? [selectedSkill.id] : [],
      requiredSubskillIds: selectedSubSkill ? [selectedSubSkill.id] : []
    };
  }

  /**
   * Get the selected skill from storage
   */
  private getSelectedSkill(): Skill | null {
    // First try to get it from the job service
    const skillFromService = this.jobService.getSelectedSkill();
    if (skillFromService) {
      return skillFromService;
    }

    // Fallback to localStorage
    try {
      const storedSkill = localStorage.getItem('selectedSkill');
      if (storedSkill) {
        return JSON.parse(storedSkill);
      }
    } catch (error) {
      console.error('[JobPostingService] Error reading skill from localStorage:', error);
    }

    return null;
  }

  /**
   * Get the selected subskill from storage
   */
  private getSelectedSubSkill(): SubSkill | null {
    // First try to get it from the job service
    const subSkillFromService = this.jobService.getSelectedSubSkill();
    if (subSkillFromService) {
      return subSkillFromService;
    }

    // Fallback to localStorage
    try {
      const storedSubSkill = localStorage.getItem('selectedSubSkill');
      if (storedSubSkill) {
        return JSON.parse(storedSubSkill);
      }
    } catch (error) {
      console.error('[JobPostingService] Error reading subskill from localStorage:', error);
    }

    return null;
  }

  /**
   * Get the selected seniority level from storage
   */
  private getSelectedSeniority(): SeniorityLevel | null {
    // First try to get it from the job service
    const seniorityFromService = this.jobService.getSelectedSeniority();
    if (seniorityFromService) {
      return seniorityFromService;
    }

    // Fallback to localStorage
    try {
      const storedSeniority = localStorage.getItem('selectedSeniority');
      if (storedSeniority) {
        return JSON.parse(storedSeniority);
      }
    } catch (error) {
      console.error('[JobPostingService] Error reading seniority from localStorage:', error);
    }

    return null;
  }

  /**
   * Get the user ID from session storage
   */
  private getUserId(): string {
    // Try to get user ID from the loggedInUser in sessionStorage
    try {
      const loggedInUserJson = sessionStorage.getItem('loggedInUser');
      if (loggedInUserJson) {
        const loggedInUser = JSON.parse(loggedInUserJson);
        if (loggedInUser && loggedInUser.userId) {
          console.log('[JobPostingService] Found user ID in loggedInUser:', loggedInUser.userId);
          return loggedInUser.userId;
        }
      }
    } catch (error) {
      console.error('[JobPostingService] Error reading loggedInUser from sessionStorage:', error);
    }


    console.warn('[JobPostingService] Could not retrieve user ID from any source');
    return '';
  }

  /**
   * Get job postings with pagination
   */
  getJobPostings(pageNumber: number = 0, pageSize: number = 10): Observable<PaginatedJobPostings> {
    const token = sessionStorage.getItem('token');
    const headers: Record<string, string> = {
      'Content-Type': 'application/json'
    };
    
    if (token) {
      headers['Authorization'] = `Bearer ${token}`;
    }

    // The API expects parameters as pageNumber and pageSize
    const url = `${endpoint.jobPostingUrl}/my-postings?pageNumber=${pageNumber}&pageSize=${pageSize}`;
    
    console.log('[JobPostingService] Fetching job postings with pagination:', { pageNumber, pageSize });
    
    return this.http.get<PaginatedJobPostings>(url, { headers })
      .pipe(
        tap(response => console.log('[JobPostingService] Job postings fetched successfully:', response)),
        catchError(err => {
          console.error('[JobPostingService] Error fetching job postings:', err);
          return throwError(() => err);
        })
      );
  }

  /**
   * Get job applications for the current HR user
   */
  getJobApplications(): Observable<JobApplication[]> {
    const token = sessionStorage.getItem('token');
    const headers: Record<string, string> = {
      'Content-Type': 'application/json'
    };
    
    if (token) {
      headers['Authorization'] = `Bearer ${token}`;
    }

    const url = `${endpoint.jobPostingUrl.replace('/jobs', '')}/job-applications/my-postings`;
    
    console.log('[JobPostingService] Fetching job applications');
    
    return this.http.get<JobApplication[]>(url, { headers })
      .pipe(
        tap(response => console.log('[JobPostingService] Job applications fetched successfully:', response)),
        catchError(err => {
          console.error('[JobPostingService] Error fetching job applications:', err);
          return throwError(() => err);
        })
      );
  }

  /**
   * Get user details by ID
   */
  getUserDetails(userId: string): Observable<UserDetails> {
    const token = sessionStorage.getItem('token');
    const headers: Record<string, string> = {
      'Content-Type': 'application/json'
    };
    
    if (token) {
      headers['Authorization'] = `Bearer ${token}`;
    }

    const url = `${endpoint.userDetailsUrl}/${userId}`;
    
    console.log('[JobPostingService] Fetching user details for ID:', userId);
    
    return this.http.get<UserDetails>(url, { headers })
      .pipe(
        tap(response => console.log('[JobPostingService] User details fetched successfully:', response)),
        catchError(err => {
          console.error('[JobPostingService] Error fetching user details:', err);
          return throwError(() => err);
        })
      );
  }

  /**
   * Update job application status
   * @param applicationId The ID of the job application to update
   * @param application The job application data with updated status
   */
  updateJobApplicationStatus(applicationId: string, application: {
    id: string,
    jobId: string,
    candidateId: string,
    status: string,
    coverLetter: string
  }): Observable<any> {
    const token = sessionStorage.getItem('token');
    const headers: Record<string, string> = {
      'Content-Type': 'application/json'
    };
    
    if (token) {
      headers['Authorization'] = `Bearer ${token}`;
    }

    // Use the correct API endpoint for updating job applications
    // PUT /api/v1/job/jobapplications/{id}
    const url = `${endpoint.jobApplicationUpdateUrl}/${applicationId}`;
    
    console.log('[JobPostingService] Updating job application status:', application);
    
    return this.http.put(url, application, { headers })
      .pipe(
        tap(response => console.log('[JobPostingService] Job application status updated successfully:', response)),
        catchError(err => {
          console.error('[JobPostingService] Error updating job application status:', err);
          return throwError(() => err);
        })
      );
  }

  /**
   * Get all interviewers by fetching users and filtering by role
   * @returns Observable of UserDetails array containing only users with the "Interviewer" role
   */
  getInterviewers(): Observable<UserDetails[]> {
    const token = sessionStorage.getItem('token');
    const headers: Record<string, string> = {
      'Content-Type': 'application/json'
    };
    
    if (token) {
      headers['Authorization'] = `Bearer ${token}`;
    }

    const url = `${endpoint.userDetailsUrl}`;
    
    console.log('[JobPostingService] Fetching interviewers');
    
    return this.http.get<UserDetails[]>(url, { headers })
      .pipe(
        map(users => {
          // Filter only users that have the "Interviewer" role
          const interviewers = users.filter(user => 
            user.roles && user.roles.includes('Interviewer')
          );
          console.log('[JobPostingService] Filtered interviewers:', interviewers);
          return interviewers;
        }),
        catchError(err => {
          console.error('[JobPostingService] Error fetching interviewers:', err);
          return throwError(() => err);
        })
      );
  }

  /**
   * Assign an interviewer to a job application
   * @param applicationId The ID of the job application
   * @param interviewerId The ID of the interviewer to assign
   */
  assignInterviewer(applicationId: string, interviewerId: string): Observable<any> {
    const token = sessionStorage.getItem('token');
    const headers: Record<string, string> = {
      'Content-Type': 'application/json'
    };
    
    if (token) {
      headers['Authorization'] = `Bearer ${token}`;
    }

    // We'll use the same update endpoint but include interviewer information
    const url = `${endpoint.jobApplicationUpdateUrl}/${applicationId}`;
    
    // We need to get the existing job application first to maintain its data
    return this.http.get<JobApplication>(`${endpoint.jobApplicationUpdateUrl}/${applicationId}`, { headers })
      .pipe(
        switchMap(application => {
          // Create the complete payload with all required fields plus the interviewer ID
          const payload = {
            id: applicationId,
            jobId: application.jobId,
            candidateId: application.candidateId,
            status: application.status,
            coverLetter: application.coverLetter || '',
            interviewerId: interviewerId
          };
          
          console.log('[JobPostingService] Assigning interviewer to application:', payload);
          
          return this.http.put(url, payload, { headers });
        }),
        tap(response => console.log('[JobPostingService] Interviewer assigned successfully:', response)),
        catchError(err => {
          console.error('[JobPostingService] Error assigning interviewer:', err);
          return throwError(() => err);
        })
      );
  }
} 