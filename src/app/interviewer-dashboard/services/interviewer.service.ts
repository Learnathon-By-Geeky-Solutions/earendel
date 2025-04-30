import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Observable, of, forkJoin, throwError } from 'rxjs';
import { map, catchError, switchMap } from 'rxjs/operators';
import { endpoint } from '../../endpoints/endpoint';
import { environment } from '../../../environments/environment';

export interface InterviewSearchParams {
  pageNumber?: number;
  pageSize?: number;
  interviewerId?: string;
  candidateId?: string;
  status?: 'Pending' | 'Accepted' | 'Declined';
}

export interface InterviewResponse {
  items: Interview[];
  pageNumber: number;
  pageSize: number;
  totalCount: number;
  totalPages: number;
  hasPrevious: boolean;
  hasNext: boolean;
}

export interface Interview {
  id: string;
  applicationId: string;
  interviewerId: string;
  candidateId?: string;
  jobId?: string;
  interviewDate: string;
  status: string;
  notes: string;
  meetingId: string;
  candidate?: string;  // Will be populated from application data if available
  role?: string;       // Will be populated from application data if available
  company?: string;    // Will be populated from application data if available
  jobDetails?: JobDetails; // Will be populated from job data if available
}

export interface JobDetails {
  id: string;
  name: string;
  description: string;
  requirments: string;
  location: string;
  jobType: string;
  experienceLevel: string;
  salary: string;
  postedById: string;
}

export interface InterviewRequest {
  id: number | string;
  candidate: string;
  role: string;
  company: string;
  requestedDate: string;
  status: string;
  description?: string;
  requirements?: string;
  experienceLevel?: string;
  postedBy?: string;
  meetingId?: string;  // Zoom meeting ID
  interviewerId?: string; // Interviewer ID for Zoom meeting URL
}

export interface AvailabilitySlot {
  id?: number | string;
  startTime: string;
  endTime: string;
}

export interface TimeSlot {
  id: number | string;
  start: string;
  end: string;
}

export interface AvailableDate {
  date: string;
  dayOfWeek: string;
  slots: TimeSlot[];
}

export interface AvailabilityResponse {
  items: AvailabilityItem[];
  pageNumber: number;
  pageSize: number;
  totalCount: number;
  totalPages: number;
  hasPrevious: boolean;
  hasNext: boolean;
}

export interface AvailabilityItem {
  id: string;
  interviewerId: string;
  startTime: string;
  endTime: string;
  isAvailable: boolean;
}

// Add new interface for interview feedback
export interface InterviewFeedback {
  interviewId: string;
  interviewQuestionText: string;  
  response: string;
  score: number;
}

@Injectable({
  providedIn: 'root',
})
export class InterviewerService {
  constructor(private http: HttpClient) {}

  /**
   * Search for interviews with the given parameters
   * @param params Search parameters like status, interviewerId, etc.
   */
  searchInterviews(
    params: InterviewSearchParams
  ): Observable<InterviewResponse> {
    const userId = this.getUserId();

    //Set interviewerId to userId if not provided
    if (!params.interviewerId && userId) {
      params.interviewerId = userId;
    }

   

    return this.http
      .post<InterviewResponse>(endpoint.interviewSearchUrl, params)
      .pipe(
        catchError((error) => {
          console.error('Error searching interviews:', error);
          return of({
            items: [],
            pageNumber: 1,
            pageSize: 10,
            totalCount: 0,
            totalPages: 0,
            hasPrevious: false,
            hasNext: false,
          });
        })
      );
  }

  searchCandidateInterviews(
    params: InterviewSearchParams
  ): Observable<InterviewResponse> {
    const userId = this.getUserId();

    if (!params.candidateId && userId) {
      params.candidateId = userId;
    }

    return this.http
      .post<InterviewResponse>(endpoint.interviewSearchUrl, params)
      .pipe(
        catchError((error) => {
          console.error('Error searching interviews:', error);
          return of({
            items: [],
            pageNumber: 1,
            pageSize: 10,
            totalCount: 0,
            totalPages: 0,
            hasPrevious: false,
            hasNext: false,
          });
        })
      );
  }



  /**
   * Get job details by ID
   * @param jobId The ID of the job to fetch
   */
  getJobDetails(jobId: string): Observable<JobDetails | null> {
    if (!jobId) {
      console.error('No job ID provided');
      return of(null);
    }

    const headers = this.getAuthHeaders();

    return this.http
      .get<JobDetails>(`${endpoint.jobDetailsUrl}/${jobId}`, { headers })
      .pipe(
        catchError((error) => {
          console.error(
            `Error fetching job details for job ID ${jobId}:`,
            error
          );
          return of(null);
        })
      );
  }

  /**
   * Get pending interview requests for the current user
   */
  getPendingInterviews(): Observable<InterviewRequest[]> {
    const userId = this.getUserId();

    if (!userId) {
      // Return empty array if no user ID is found
      console.error('No user ID found in session');
      return of([]);
    }

    const params: InterviewSearchParams = {
      interviewerId: userId,
      status: 'Pending',
      pageNumber: 1,
      pageSize: 200,
    };

    return this.searchInterviews(params).pipe(
      switchMap((response) => {
        // Create an array of observables for each interview to fetch job details
        const interviewsWithJobDetails$ = response.items.map((interview) => {
          if (interview.jobId) {
            // If there's a jobId, fetch the job details
            return this.getJobDetails(interview.jobId).pipe(
              map((jobDetails) => ({
                interview,
                jobDetails,
              }))
            );
          } else {
            // If no jobId, just return the interview with null job details
            return of({
              interview,
              jobDetails: null,
            });
          }
        });

        // Wait for all job details to be fetched
        return interviewsWithJobDetails$.length > 0
          ? forkJoin(interviewsWithJobDetails$)
          : of([]);
      }),
      map((interviewsWithDetails) => {
        // Transform the combined data to the format expected by the component
        return interviewsWithDetails.map(({ interview, jobDetails }) => {
          // Calculate requested date as interview date - 1 day
          const interviewDate = new Date(interview.interviewDate);
          interviewDate.setDate(interviewDate.getDate() - 1);

          // Create the interview request object with job details if available
          const interviewRequest: InterviewRequest = {
            id: interview.id,
            candidate:
              interview.candidate ||
              `Candidate (${interview.applicationId.substring(0, 8)})`,
            role:
              jobDetails?.name || interview.role || 'Position not specified',
            company:
              jobDetails?.postedById ||
              interview.company ||
              'Posted by not specified',
            requestedDate: interviewDate.toISOString().split('T')[0],
            status: interview.status.toLowerCase(),
          };

          // Add job details if available
          if (jobDetails) {
            interviewRequest.description = jobDetails.description;
            interviewRequest.requirements = jobDetails.requirments;
            interviewRequest.experienceLevel = jobDetails.experienceLevel;
            interviewRequest.postedBy = jobDetails.postedById;
          }

          return interviewRequest;
        });
      })
    );
  }

  /**
   * Get accepted interview requests for the current user
   */
  getAcceptedInterviews(): Observable<InterviewRequest[]> {
    const userId = this.getUserId();

    if (!userId) {
      // Return empty array if no user ID is found
      console.error('No user ID found in session');
      return of([]);
    }

    const params: InterviewSearchParams = {
      interviewerId: userId,
      status: 'Accepted',
      pageNumber: 1,
      pageSize: 100,
    };

    return this.searchInterviews(params).pipe(
      switchMap((response) => {
        // Create an array of observables for each interview to fetch job details
        const interviewsWithJobDetails$ = response.items.map((interview) => {
          if (interview.jobId) {
            // If there's a jobId, fetch the job details
            return this.getJobDetails(interview.jobId).pipe(
              map((jobDetails) => ({
                interview,
                jobDetails,
              }))
            );
          } else {
            // If no jobId, just return the interview with null job details
            return of({
              interview,
              jobDetails: null,
            });
          }
        });

        // Wait for all job details to be fetched
        return interviewsWithJobDetails$.length > 0
          ? forkJoin(interviewsWithJobDetails$)
          : of([]);
      }),
      map((interviewsWithDetails) => {
        // Transform the combined data to the format expected by the component
        return interviewsWithDetails.map(({ interview, jobDetails }) => {
          // Create the interview request object with job details if available
          const interviewRequest: InterviewRequest = {
            id: interview.id,
            candidate:
              interview.candidate ||
              `Candidate (${interview.applicationId.substring(0, 8)})`,
            role:
              jobDetails?.name || interview.role || 'Position not specified',
            company:
              jobDetails?.postedById ||
              interview.company ||
              'Posted by not specified',
            requestedDate: interview.interviewDate, // Keep the original ISO string for proper parsing
            status: interview.status.toLowerCase(),
            meetingId: interview.meetingId, // Include the meetingId
            interviewerId: interview.interviewerId || userId, // Include the interviewerId
          };

          // Add job details if available
          if (jobDetails) {
            interviewRequest.description = jobDetails.description;
            interviewRequest.requirements = jobDetails.requirments;
            interviewRequest.experienceLevel = jobDetails.experienceLevel;
            interviewRequest.postedBy = jobDetails.postedById;
          }

          return interviewRequest;
        });
      })
    );
  }

  getAcceptedCandidateInterviews(): Observable<InterviewRequest[]> {
    const userId = this.getUserId();
    console.log(userId);

    if (!userId) {
      // Return empty array if no user ID is found
      console.error('No user ID found in session');
      return of([]);
    }

    const params: InterviewSearchParams = {
      candidateId: userId,
      status: 'Accepted',
      pageNumber: 1,
      pageSize: 100,
    };

    return this.searchCandidateInterviews(params).pipe(
      switchMap((response) => {
        console.log(response);
        // Create an array of observables for each interview to fetch job details
        const interviewsWithJobDetails$ = response.items.map((interview) => {
          if (interview.jobId) {
            // If there's a jobId, fetch the job details
            return this.getJobDetails(interview.jobId).pipe(
              map((jobDetails) => ({
                interview,
                jobDetails,
              }))
            );
          } else {
            // If no jobId, just return the interview with null job details
            return of({
              interview,
              jobDetails: null,
            });
          }
        });

        // Wait for all job details to be fetched
        return interviewsWithJobDetails$.length > 0
          ? forkJoin(interviewsWithJobDetails$)
          : of([]);
      }),
      map((interviewsWithDetails) => {
        console.log(interviewsWithDetails);
        // Transform the combined data to the format expected by the component
        return interviewsWithDetails.map(({ interview, jobDetails }) => {
          // Create the interview request object with job details if available
          const interviewRequest: InterviewRequest = {
            id: interview.id,
            candidate:
              `Interviewer (${interview.interviewerId.substring(0, 8)})`,
            role:
              jobDetails?.name || interview.role || 'Position not specified',
            company:
              jobDetails?.postedById ||
              interview.company ||
              'Posted by not specified',
            requestedDate: interview.interviewDate, // Keep the original ISO string for proper parsing
            status: interview.status.toLowerCase(),
            meetingId: interview.meetingId, // Include the meetingId
            interviewerId: interview.interviewerId || userId, // Include the interviewerId
          };

          // Add job details if available
          if (jobDetails) {
            interviewRequest.description = jobDetails.description;
            interviewRequest.requirements = jobDetails.requirments;
            interviewRequest.experienceLevel = jobDetails.experienceLevel;
            interviewRequest.postedBy = jobDetails.postedById;
          }

          return interviewRequest;
        });
      })
    );
  }

  /**
   * Get past interviews for the current user
   * Returns interviews that have already occurred (interview date is in the past)
   */
  getPastInterviews(): Observable<InterviewRequest[]> {
    const userId = this.getUserId();

    if (!userId) {
      // Return empty array if no user ID is found
      console.error('No user ID found in session');
      return of([]);
    }

    const params: InterviewSearchParams = {
      interviewerId: userId,
      status: 'Accepted',
      pageNumber: 1,
      pageSize: 100,
    };

    return this.searchInterviews(params).pipe(
      switchMap((response) => {
        // Create an array of observables for each interview to fetch job details
        const interviewsWithJobDetails$ = response.items.map((interview) => {
          if (interview.jobId) {
            // If there's a jobId, fetch the job details
            return this.getJobDetails(interview.jobId).pipe(
              map((jobDetails) => ({
                interview,
                jobDetails,
              }))
            );
          } else {
            // If no jobId, just return the interview with null job details
            return of({
              interview,
              jobDetails: null,
            });
          }
        });

        // Wait for all job details to be fetched
        return interviewsWithJobDetails$.length > 0
          ? forkJoin(interviewsWithJobDetails$)
          : of([]);
      }),
      map((interviewsWithDetails) => {
        const now = new Date();

        // Filter to only include past interviews and transform the data
        return interviewsWithDetails
          .filter(({ interview }) => {
            const interviewDate = new Date(interview.interviewDate);
            return interviewDate < now;
          })
          .map(({ interview, jobDetails }) => {
            // Create the interview request object with job details if available
            const interviewRequest: InterviewRequest = {
              id: interview.id,
              candidate:
                interview.candidate ||
                `Candidate (${interview.applicationId.substring(0, 8)})`,
              role:
                jobDetails?.name || interview.role || 'Position not specified',
              company:
                jobDetails?.postedById ||
                interview.company ||
                'Posted by not specified',
              requestedDate: interview.interviewDate, // Keep the original ISO string for proper parsing
              status: interview.status.toLowerCase(),
              meetingId: interview.meetingId, // Include the meetingId
              interviewerId: interview.interviewerId || userId, // Include the interviewerId
            };

            // Add job details if available
            if (jobDetails) {
              interviewRequest.description = jobDetails.description;
              interviewRequest.requirements = jobDetails.requirments;
              interviewRequest.experienceLevel = jobDetails.experienceLevel;
              interviewRequest.postedBy = jobDetails.postedById;
            }

            return interviewRequest;
          });
      })
    );
  }

  /**
   * Get available time slots for the interviewer
   * Groups time slots by date and returns them in the format expected by the component
   */
  getAvailableTimeSlots(): Observable<AvailableDate[]> {
    const userId = this.getUserId();

    if (!userId) {
      console.error('No user ID found in session');
      return of([]);
    }

    const request = {
      pageNumber: 1,
      pageSize: 1000,
      interviewerId: userId,
      isAvailable: true,
    };

    const headers = this.getAuthHeaders();

    return this.http
      .post<AvailabilityResponse>(
        endpoint.interviewerAvailabilitySearchUrl,
        request,
        { headers }
      )
      .pipe(
        map((response) => this.transformAvailabilityData(response.items)),
        catchError((error) => {
          console.error('Error fetching availability slots:', error);
          return of([]);
        })
      );
  }

  /**
   * Transform availability data from API to the format expected by the component
   */
  private transformAvailabilityData(
    items: AvailabilityItem[]
  ): AvailableDate[] {
    // Group slots by date
    const slotsByDate: Record<string, AvailabilityItem[]> = {};

    // Process all slots
    items.forEach((slot) => {
      const startDate = new Date(slot.startTime);
      const dateKey = startDate.toISOString().split('T')[0]; // YYYY-MM-DD format

      if (!slotsByDate[dateKey]) {
        slotsByDate[dateKey] = [];
      }

      slotsByDate[dateKey].push(slot);
    });

    // Convert to array format
    return (
      Object.keys(slotsByDate)
        .map((dateKey) => {
          const date = new Date(dateKey);
          const dayNames = [
            'Sunday',
            'Monday',
            'Tuesday',
            'Wednesday',
            'Thursday',
            'Friday',
            'Saturday',
          ];
          const dayOfWeek = dayNames[date.getDay()];

          // Sort slots by start time
          const sortedSlots = slotsByDate[dateKey].sort((a, b) => {
            return (
              new Date(a.startTime).getTime() - new Date(b.startTime).getTime()
            );
          });

          // Transform to TimeSlot format
          const timeSlots = sortedSlots.map((slot) => {
            const startTime = new Date(slot.startTime);
            const endTime = new Date(slot.endTime);

            return {
              id: slot.id,
              start: this.formatTimeForDisplay(startTime),
              end: this.formatTimeForDisplay(endTime),
            };
          });

          return {
            date: dateKey,
            dayOfWeek,
            slots: timeSlots,
          };
        })
        // Sort dates in ascending order
        .sort((a, b) => a.date.localeCompare(b.date))
    );
  }

  /**
   * Format a date to 24-hour time format (HH:MM)
   */
  private formatTimeForDisplay(date: Date): string {
    const hours = date.getHours().toString().padStart(2, '0');
    const minutes = date.getMinutes().toString().padStart(2, '0');
    return `${hours}:${minutes}`;
  }

  /**
   * Get authentication headers for API requests
   */
  private getAuthHeaders(): HttpHeaders {
    let headers = new HttpHeaders({
      'Content-Type': 'application/json',
    });

    try {
      const userData =
        sessionStorage.getItem('userData') ||
        sessionStorage.getItem('loggedInUser');
      if (userData) {
        const parsed = JSON.parse(userData);
        if (parsed && parsed.token) {
          headers = headers.set('Authorization', `Bearer ${parsed.token}`);
        }
      }
    } catch (error) {
      console.error('Error setting auth headers:', error);
    }

    return headers;
  }

  /**
   * Gets the current user ID from session storage
   */
  private getUserId(): string | null {
    try {
      const userData =
        sessionStorage.getItem('userData') ||
        sessionStorage.getItem('loggedInUser');
      if (userData) {
        const parsed = JSON.parse(userData);
        return parsed.userId || parsed.id || null;
      }
      return null;
    } catch (error) {
      console.error('Error retrieving user ID:', error);
      return null;
    }
  }

  /**
   * Update interview with selected time
   * @param interviewId The ID of the interview to update
   * @param interviewDate The selected interview date and time in ISO format (UTC)
   * @returns An Observable that emits the API response
   */
  updateInterview(interviewId: string, interviewDate: string): Observable<any> {
    if (!interviewId) {
      console.error('No interview ID provided');
      return throwError(() => new Error('Interview ID is required'));
    }

    const headers = this.getAuthHeaders();
    const url = `${endpoint.interviewSearchUrl.replace(
      '/search',
      ''
    )}/${interviewId}`;

    // First get the current interview data to ensure we have all required fields
    return this.http.get<any>(`${url}`, { headers }).pipe(
      switchMap((currentInterview) => {
        // Create the payload with updated interview date and status
        // Maintain all existing fields except for the ones we're updating
        const updateData = {
          ...currentInterview,
          interviewDate,
          status: 'Accepted',
        };

        console.log(`Updating interview ID: ${interviewId}`);
        console.log(`New interview date (UTC): ${interviewDate}`);
        console.log('Update payload:', JSON.stringify(updateData));

        // Make PUT request to update the interview
        return this.http
          .put(url, updateData, {
            headers,
            observe: 'response', // Get full response with status
          })
          .pipe(
            map((response) => {
              console.log('Update successful:', response.status);
              return response.body;
            }),
            catchError((error) => {
              console.error('Error updating interview:', error);
              console.error('Request payload:', JSON.stringify(updateData));
              console.error('Response status:', error.status);
              console.error('Response body:', error.error);

              // Rethrow with more context
              return throwError(
                () =>
                  new Error(
                    `Failed to update interview: ${
                      error.message || error.status
                    }`
                  )
              );
            })
          );
      }),
      catchError((error) => {
        console.error(
          `Error fetching interview details for ID ${interviewId}:`,
          error
        );
        return throwError(
          () =>
            new Error(
              `Failed to get interview details: ${
                error.message || error.status
              }`
            )
        );
      })
    );
  }

  /* Placeholder to avoid errors */
  searchAvailabilities(): Observable<any> {
    return this.getAvailableTimeSlots().pipe(map((data) => ({ items: data })));
  }

  /**
   * Get notifications for the current user
   *
   * @param pageNumber Page number to fetch (default 1)
   * @param pageSize Number of notifications per page (default 1000)
   * @returns Observable with notification data
   */
  getNotifications(
    pageNumber: number = 1,
    pageSize: number = 1000
  ): Observable<any> {
    const userId = this.getUserId();

    if (!userId) {
      console.error('Cannot fetch notifications: No user ID found');
      return throwError(() => new Error('User ID not available'));
    }

    console.log('Fetching notifications for user ID:', userId);

    // Correctly format the request according to the API specification
    const request = {
      pageNumber: pageNumber,
      pageSize: pageSize,
      userId: userId,
    };

    const headers = this.getAuthHeaders();

    console.log('Notification search request:', request);
    console.log(
      'Notification search endpoint:',
      endpoint.notificationSearchUrl
    );

    return this.http
      .post(endpoint.notificationSearchUrl, request, { headers })
      .pipe(
        catchError((error) => {
          console.error('API Error searching notifications:', error);
          return throwError(() => error);
        })
      );
  }

  /**
   * Submit feedback for a specific interview question
   * @param feedback The feedback data containing question, answer, and score
   * @returns Observable with the API response
   */
  submitInterviewFeedback(feedback: InterviewFeedback): Observable<any> {
    const headers = this.getAuthHeaders();

    // Log the request payload for debugging
    console.log(
      'Submitting feedback with payload:',
      JSON.stringify(feedback, null, 2)
    );

    const url = endpoint.interviewFeedbackUrl;

    // Use standard Angular HTTP client for submission
    return this.http
      .post(url, feedback, {
        headers,
        observe: 'response', // Get the full response to access status codes
      })
      .pipe(
        map((response) => {
          console.log('Feedback submission successful:', response.status);
          return response.body;
        }),
        catchError((error) => {
          console.error('Error submitting feedback:', error);

          // Add more detailed logging for 400 errors
          if (error.status === 400) {
            console.error('Bad Request (400) details:', {
              error: error.error,
              url: url,
              requestBody: JSON.stringify(feedback),
              headers: Array.from(headers.keys()).reduce(
                (obj: Record<string, string | null>, key) => {
                  obj[key] = headers.get(key);
                  return obj;
                },
                {}
              ),
            });
          }

          return throwError(() => error);
        })
      );
  }

  /**
   * Submit multiple feedback items one by one
   * @param feedbackItems Array of feedback items to submit
   * @returns Observable that completes when all submissions are processed
   */
  submitAllFeedback(feedbackItems: InterviewFeedback[]): Observable<any[]> {
    if (!feedbackItems || feedbackItems.length === 0) {
      return of([]);
    }

    const submissionObservables = feedbackItems.map((feedback) =>
      this.submitInterviewFeedback(feedback)
    );

    return forkJoin(submissionObservables);
  }
}
