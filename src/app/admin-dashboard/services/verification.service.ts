import { Injectable } from '@angular/core';
import { endpoint } from '../../endpoints/endpoint';
import { HttpClient, HttpParams } from '@angular/common/http';
import { ApiService } from '../../core/service/api.service';
import { Observable } from 'rxjs';

export interface InterviewerVerification {
  id: string;
  userId: string;
  name?: string;
  personalEmail?: string;
  workEmail?: string;
  submittedDate?: string;
  cv: string | null;
  idCard?: string | null;
  workPermit?: string | null;
  additionalInfo: string;
  status: 'pending' | 'approved' | 'rejected';
}

export interface VerificationApiResponse {
  items: InterviewerVerification[];
  pageNumber: number;
  pageSize: number;
  totalCount: number;
  totalPages: number;
  hasPrevious: boolean;
  hasNext: boolean;
}

@Injectable({
  providedIn: 'root',
})
export class VerificationService {
  // Verification endpoints
  getInterviewerEntryFormList = `${endpoint.getInterviewerEntryFormList}`;
  approveInterviewerEntryForm = `${endpoint.approveInterviewerEntryForm}`;
  rejectInterviewerEntryForm = `${endpoint.rejectInterviewerEntryForm}`;

  requestInterviewerEntryFormData = `${endpoint.requestInterviewerEntryFormData}`;

  downloadInterviewerEntryFormPdf = `${endpoint.downloadInterviewerEntryFormPdf}`;
  uploadInterviewerEntryFormPdf = `${endpoint.uploadInterviewerEntryFormPdf}`;

  userProfile = `${endpoint.userProfile}`;

  constructor(
    private readonly http: HttpClient,
    private readonly apiService: ApiService
  ) {}

  getVerificationList(params: any): Observable<any> {
    return this.http.post(this.getInterviewerEntryFormList.trim(), params);
  }

  getUserProfile(id: string): Observable<any> {
    // Use the correct endpoint for downloading documents
    return this.http.get(`${this.userProfile.trim()}/${id}`);
  }

  approveVerification(id: string): Observable<any> {
    return this.http.post<any>(
      `${this.approveInterviewerEntryForm.trim()}/${id}/approve`,
      {}
    );
  }

  rejectVerification(id: string): Observable<any> {
    return this.http.post<any>(
      `${this.rejectInterviewerEntryForm.trim()}/${id}/reject`,
      {}
    );
  }

  getVerificationDocument(id: string, documentType = 'cv'): Observable<Blob> {
    // Use the correct endpoint for downloading documents
    return this.http.get(
      `${this.downloadInterviewerEntryFormPdf.trim()}/${id}/download-${documentType}`,
      {
        responseType: 'blob',
      }
    );
  }

  getDocumentTypeFromPath(path: string | null | undefined): string {
    if (!path) return 'cv';

    if (path.includes('id-card')) return 'id';
    if (path.includes('work-permit')) return 'permit';

    // Default to CV
    return 'cv';
  }

  submitVerificationApplication(formData: any): Observable<any> {
    return this.http.post<any>(
      `${this.requestInterviewerEntryFormData}`,
      formData
    );
  }

  uploadFile(
    applicationId: string,
    documentType: string,
    formData: FormData
  ): Observable<any> {
    console.log(applicationId)
    return this.http.post(
      `${this.uploadInterviewerEntryFormPdf}/${applicationId}/upload-cv`,
      formData,
      {
        reportProgress: true,
        observe: 'events',
      }
    );
  }
}
