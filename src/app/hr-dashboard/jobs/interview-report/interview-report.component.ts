import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute } from '@angular/router';
import { HttpClientModule, HttpClient } from '@angular/common/http';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatDividerModule } from '@angular/material/divider';
import { MatIconModule } from '@angular/material/icon';
import { jsPDF } from 'jspdf';
import { switchMap, catchError, finalize } from 'rxjs/operators';
import { of } from 'rxjs';
import { endpoint } from '../../../endpoints/endpoint';
import { SidebarComponent } from '../../sidebar/sidebar.component';

interface Interview {
  id: string;
  applicationId: string;
  interviewerId: string;
  candidateId: string;
  jobId: string;
  interviewDate: string;
  status: string;
  notes: string;
  meetingId: string;
}

interface InterviewFeedback {
  id: string;
  interviewId: string;
  interviewQuestionText: string;
  response: string;
  score: number;
}

interface SearchResponse<T> {
  items: T[];
  pageNumber: number;
  pageSize: number;
  totalCount: number;
  totalPages: number;
  hasPrevious: boolean;
  hasNext: boolean;
}

@Component({
  selector: 'app-interview-report',
  standalone: true,
  imports: [
    CommonModule,
    HttpClientModule,
    MatProgressSpinnerModule,
    MatSnackBarModule,
    MatButtonModule,
    MatCardModule,
    MatDividerModule,
    MatIconModule,
    SidebarComponent,
  ],
  templateUrl: './interview-report.component.html',
  styleUrl: './interview-report.component.css'
})
export class InterviewReportComponent implements OnInit {
  candidateId: string | null = null;
  jobId: string | null = null;
  interviewId: string | null = null;
  
  loading = false;
  error: string | null = null;
  
  interview: Interview | null = null;
  feedbackItems: InterviewFeedback[] = [];
  
  totalScore = 0;
  maxPossibleScore = 0;
  
  candidateName = 'Candidate';
  jobTitle = 'Job Position';
  interviewDate = '';

  constructor(
    private route: ActivatedRoute,
    private http: HttpClient,
    private snackBar: MatSnackBar
  ) {}

  ngOnInit(): void {
    this.route.queryParamMap.subscribe(params => {
      this.candidateId = params.get('candidateId');
      this.jobId = params.get('jobId');
      
      if (this.candidateId && this.jobId) {
        this.fetchInterviewData();
      } else {
        this.error = 'Missing candidate ID or job ID parameters';
      }
    });
  }

  fetchInterviewData(): void {
    this.loading = true;
    this.error = null;
    
    // Step 1: Search for interviews
    const searchParams = {
      pageNumber: 1,
      pageSize: 100,
      candidateId: this.candidateId,
      jobId: this.jobId
    };
    
    this.http.post<SearchResponse<Interview>>(endpoint.interviewSearchUrl, searchParams, { headers: this.getAuthHeaders() })
      .pipe(
        switchMap(response => {
          if (response.items && response.items.length > 0) {
            this.interview = response.items[0];
            this.interviewId = this.interview.id;
            
            if (this.interview.interviewDate) {
              this.interviewDate = new Date(this.interview.interviewDate).toLocaleDateString();
            }
            
            // Get candidate and job details if possible
            this.fetchCandidateDetails();
            this.fetchJobDetails();
            
            // Step 2: Search for interview feedback
            return this.fetchInterviewFeedback(this.interviewId);
          } else {
            return of({ items: [], pageNumber: 1, pageSize: 0, totalCount: 0, totalPages: 0, hasPrevious: false, hasNext: false } as SearchResponse<InterviewFeedback>);
          }
        }),
        catchError(error => {
          console.error('Error fetching interview data:', error);
          this.error = 'Failed to fetch interview data. Please try again later.';
          return of({ items: [], pageNumber: 1, pageSize: 0, totalCount: 0, totalPages: 0, hasPrevious: false, hasNext: false } as SearchResponse<InterviewFeedback>);
        }),
        finalize(() => {
          this.loading = false;
        })
      )
      .subscribe((feedbackResponse) => {
        this.feedbackItems = feedbackResponse.items || [];
        
        // Calculate total score
        this.calculateScores();
      });
  }

  fetchInterviewFeedback(interviewId: string) {
    const feedbackSearchParams = {
      pageNumber: 1,
      pageSize: 100,
      interviewId: interviewId
    };
    
    return this.http.post<SearchResponse<InterviewFeedback>>(
      `${endpoint.interviewFeedbackUrl}/search`, 
      feedbackSearchParams, 
      { headers: this.getAuthHeaders() }
    );
  }

  fetchCandidateDetails(): void {
    if (this.candidateId) {
      this.http.get(`${endpoint.userDetailsUrl}/${this.candidateId}`, { headers: this.getAuthHeaders() })
        .pipe(
          catchError(error => {
            console.error('Error fetching candidate details:', error);
            return of(null);
          })
        )
        .subscribe((response: any) => {
          if (response && response.userName) {
            this.candidateName = response.userName;
          }
        });
    }
  }

  fetchJobDetails(): void {
    if (this.jobId) {
      this.http.get(`${endpoint.jobDetailsUrl}/${this.jobId}`, { headers: this.getAuthHeaders() })
        .pipe(
          catchError(error => {
            console.error('Error fetching job details:', error);
            return of(null);
          })
        )
        .subscribe((response: any) => {
          if (response && response.name) {
            this.jobTitle = response.name;
          }
        });
    }
  }

  calculateScores(): void {
    this.totalScore = 0;
    this.maxPossibleScore = 0;
    
    this.feedbackItems.forEach(feedback => {
      this.totalScore += feedback.score;
      this.maxPossibleScore += 5; // Assuming max score per question is 5
    });
  }

  getScoreColor(score: number): string {
    if (score >= 4) {
      return 'green';
    } else if (score >= 3) {
      return 'orange';
    } else {
      return 'red';
    }
  }

  getScorePercentage(): number {
    if (this.maxPossibleScore === 0) {
      return 0;
    }
    return Math.round((this.totalScore / this.maxPossibleScore) * 100);
  }

  getOverallRating(): string {
    const percentage = this.getScorePercentage();
    
    if (percentage >= 80) {
      return 'Excellent';
    } else if (percentage >= 70) {
      return 'Very Good';
    } else if (percentage >= 60) {
      return 'Good';
    } else if (percentage >= 50) {
      return 'Average';
    } else {
      return 'Below Average';
    }
  }

  downloadPDF(): void {
    const doc = new jsPDF();
    const pageWidth = doc.internal.pageSize.getWidth();
    let y = 20;
    
    // Set title
    doc.setFontSize(18);
    doc.text('Interview Feedback Report', pageWidth / 2, y, { align: 'center' });
    y += 15;
    
    // Candidate and job info
    doc.setFontSize(12);
    doc.text(`Candidate: ${this.candidateName}`, 20, y);
    y += 8;
    
    doc.text(`Position: ${this.jobTitle}`, 20, y);
    y += 8;
    
    doc.text(`Interview Date: ${this.interviewDate}`, 20, y);
    y += 8;
    
    doc.text(`Overall Score: ${this.totalScore}/${this.maxPossibleScore} (${this.getScorePercentage()}%)`, 20, y);
    y += 8;
    
    doc.text(`Overall Rating: ${this.getOverallRating()}`, 20, y);
    y += 15;
    
    // Table headers
    doc.setFillColor(240, 240, 240);
    doc.rect(20, y, pageWidth - 40, 10, 'F');
    doc.setFontSize(11);
    doc.text('Question', 25, y + 7);
    doc.text('Response', 110, y + 7);
    doc.text('Score', pageWidth - 30, y + 7);
    y += 15;
    
    // Table rows
    doc.setFontSize(10);
    this.feedbackItems.forEach((feedback, index) => {
      // Check if we need a new page
      if (y > doc.internal.pageSize.getHeight() - 20) {
        doc.addPage();
        y = 20;
      }
      
      const questionText = feedback.interviewQuestionText || 'N/A';
      const responseText = feedback.response || 'N/A';
      
      // Split long text to fit within columns
      const questionLines = doc.splitTextToSize(questionText, 80);
      const responseLines = doc.splitTextToSize(responseText, 80);
      
      const lineHeight = 6;
      const maxLines = Math.max(questionLines.length, responseLines.length);
      const rowHeight = maxLines * lineHeight;
      
      // Question
      for (let i = 0; i < questionLines.length; i++) {
        doc.text(questionLines[i], 25, y + (i * lineHeight));
      }
      
      // Response
      for (let i = 0; i < responseLines.length; i++) {
        doc.text(responseLines[i], 110, y + (i * lineHeight));
      }
      
      // Score
      doc.text(feedback.score.toString(), pageWidth - 30, y);
      
      y += rowHeight + 5;
      
      // Draw line after each item except the last one
      if (index < this.feedbackItems.length - 1) {
        doc.setDrawColor(200, 200, 200);
        doc.line(20, y - 3, pageWidth - 20, y - 3);
      }
    });
    
    // Save the PDF
    doc.save(`Interview_Report_${this.candidateName.replace(/\s+/g, '_')}.pdf`);
    
    this.snackBar.open('Report downloaded successfully', 'Close', { duration: 3000 });
  }

  downloadCSV(): void {
    const headers = ['Question', 'Response', 'Score'];
    
    // Create CSV content
    let csvContent = headers.join(',') + '\n';
    
    this.feedbackItems.forEach(feedback => {
      // Escape any commas in the text
      const question = `"${feedback.interviewQuestionText.replace(/"/g, '""')}"`;
      const response = `"${feedback.response.replace(/"/g, '""')}"`;
      const score = feedback.score;
      
      csvContent += `${question},${response},${score}\n`;
    });
    
    // Add summary row
    csvContent += `\n"Total Score","${this.totalScore}/${this.maxPossibleScore}",${this.getScorePercentage()}%\n`;
    csvContent += `"Overall Rating","${this.getOverallRating()}",\n`;
    
    // Create a blob and trigger download
    const blob = new Blob([csvContent], { type: 'text/csv;charset=utf-8' });
    const url = URL.createObjectURL(blob);
    const link = document.createElement('a');
    link.setAttribute('href', url);
    link.setAttribute('download', `Interview_Report_${this.candidateName.replace(/\s+/g, '_')}.csv`);
    link.style.visibility = 'hidden';
    document.body.appendChild(link);
    link.click();
    document.body.removeChild(link);
    
    this.snackBar.open('CSV downloaded successfully', 'Close', { duration: 3000 });
  }

  private getAuthHeaders(): any {
    let headers: any = {
      'Content-Type': 'application/json'
    };
    
    try {
      const userData = sessionStorage.getItem('userData') || sessionStorage.getItem('loggedInUser');
      if (userData) {
        const parsed = JSON.parse(userData);
        if (parsed && parsed.token) {
          headers['Authorization'] = `Bearer ${parsed.token}`;
        }
      }
    } catch (error) {
      console.error('Error setting auth headers:', error);
    }
    
    return headers;
  }
}
