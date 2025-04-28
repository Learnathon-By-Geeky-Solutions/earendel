import { Injectable } from '@angular/core';
import { HttpClient, HttpParams, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { endpoint } from '../../endpoints/endpoint';

export interface JobFilter {
  name?: string;
  description?: string;
  requirements?: string;
  location?: string;
  jobType?: string;
  experienceLevel?: string;
  page?: number;
}

export interface Job {
  id: string;
  name: string;
  description: string;
  requirments: string;
  location: string;
  jobType: string;
  experienceLevel: string;
  createdOn: string;
  requiredSkillIds: string[];
  requiredSubskillIds: string[];
}

export interface JobApplication {
  jobId: string;
  candidateId: string;
  coverLetter: string;
}

@Injectable({
  providedIn: 'root'
})
export class JobService {
  constructor(private http: HttpClient) {}

  getJobs(filters: JobFilter): Observable<Job[]> {
    // Add the endpoint to the endpoint.ts file
    const apiUrl = `${endpoint.jobViewUrl}`;
    
    let params = new HttpParams();
    
    if (filters.name) params = params.set('name', filters.name);
    if (filters.description) params = params.set('description', filters.description);
    if (filters.requirements) params = params.set('requirements', filters.requirements);
    if (filters.location) params = params.set('location', filters.location);
    if (filters.jobType) params = params.set('jobType', filters.jobType);
    if (filters.experienceLevel) params = params.set('experienceLevel', filters.experienceLevel);
    if (filters.page) params = params.set('page', filters.page.toString());

    // Get auth token from session storage (following login pattern)
    const userDataStr = sessionStorage.getItem('loggedInUser');
    let headers = new HttpHeaders();
    
    if (userDataStr) {
      try {
        const userData = JSON.parse(userDataStr);
        if (userData.token) {
          headers = headers.set('Authorization', `Bearer ${userData.token}`);
        }
      } catch (error) {
        console.error('Error parsing user data from session storage:', error);
      }
    }

    return this.http.get<Job[]>(apiUrl, { 
      params,
      headers
    });
  }

  applyForJob(jobId: string, coverLetter: string): Observable<any> {
    const apiUrl = `${endpoint.jobViewUrl.split('/JobView')[0]}/job/jobapplications`;
    
    // Get user data from session storage
    const userDataStr = sessionStorage.getItem('loggedInUser');
    let headers = new HttpHeaders();
    let candidateId = '';
    
    if (userDataStr) {
      try {
        const userData = JSON.parse(userDataStr);
        if (userData.token) {
          headers = headers.set('Authorization', `Bearer ${userData.token}`);
        }
        // Use the userId as candidateId
        candidateId = userData.userId || '';
      } catch (error) {
        console.error('Error parsing user data from session storage:', error);
      }
    }

    const applicationData: JobApplication = {
      jobId: jobId,
      candidateId: candidateId,
      coverLetter: coverLetter
    };

    return this.http.post(apiUrl, applicationData, { headers });
  }
} 