import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError, tap } from 'rxjs/operators';
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
} 