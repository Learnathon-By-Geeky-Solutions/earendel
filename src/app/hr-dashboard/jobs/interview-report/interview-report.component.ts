import {
  Component,
  OnInit,
  OnDestroy,
  ViewChild,
  ElementRef,
  Inject,
  NgZone,
} from '@angular/core';
import { CommonModule, DOCUMENT } from '@angular/common';
import { HttpClientModule, HttpClient } from '@angular/common/http';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatDividerModule } from '@angular/material/divider';
import { MatIconModule } from '@angular/material/icon';
import html2canvas from 'html2canvas';
import { jsPDF } from 'jspdf';
import { switchMap, catchError, finalize } from 'rxjs/operators';
import { of, Subscription } from 'rxjs';
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
    RouterModule,
    SidebarComponent,
  ],
  templateUrl: './interview-report.component.html',
  styleUrls: ['./interview-report.component.css'],
})
export class InterviewReportComponent implements OnInit, OnDestroy {
  @ViewChild('reportContainer', { static: false })
  reportContainer!: ElementRef<HTMLElement>;

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
  candidateEmail = '';
  jobTitle = 'Job Position';
  interviewDate = '';

  private subs: Subscription[] = [];

  constructor(
    private route: ActivatedRoute,
    private http: HttpClient,
    private snackBar: MatSnackBar,
    @Inject(DOCUMENT) private document: Document,
    private ngZone: NgZone
  ) {}

  ngOnInit(): void {
    this.subs.push(
      this.route.queryParamMap.subscribe((params) => {
        this.candidateId = params.get('candidateId');
        this.jobId = params.get('jobId');
        if (this.candidateId && this.jobId) {
          this.fetchInterviewData();
        } else {
          this.error = 'Missing candidate ID or job ID parameters';
        }
      })
    );
  }

  ngOnDestroy(): void {
    this.subs.forEach((s) => s.unsubscribe());
  }

  private fetchInterviewData(): void {
    this.loading = true;
    this.error = null;

    const searchParams = {
      pageNumber: 1,
      pageSize: 100,
      candidateId: this.candidateId!,
      jobId: this.jobId!,
    };

    this.subs.push(
      this.http
        .post<SearchResponse<Interview>>(
          endpoint.interviewSearchUrl,
          searchParams,
          {
            headers: this.getAuthHeaders(),
          }
        )
        .pipe(
          switchMap((response) => {
            if (response.items.length > 0) {
              this.interview = response.items[0];
              this.interviewId = this.interview.id;
              this.interviewDate = new Date(
                this.interview.interviewDate
              ).toLocaleDateString();
              this.fetchCandidateDetails();
              this.fetchJobDetails();
              return this.fetchInterviewFeedback(this.interviewId);
            } else {
              return of({
                items: [],
                pageNumber: 1,
                pageSize: 0,
                totalCount: 0,
                totalPages: 0,
                hasPrevious: false,
                hasNext: false,
              } as SearchResponse<InterviewFeedback>);
            }
          }),
          catchError((err) => {
            console.error(err);
            this.error = 'Failed to fetch interview data.';
            return of({
              items: [],
              pageNumber: 1,
              pageSize: 0,
              totalCount: 0,
              totalPages: 0,
              hasPrevious: false,
              hasNext: false,
            } as SearchResponse<InterviewFeedback>);
          }),
          finalize(() => (this.loading = false))
        )
        .subscribe((feedbackResp) => {
          this.feedbackItems = feedbackResp.items;
          this.calculateScores();
        })
    );
  }

  private fetchInterviewFeedback(interviewId: string) {
    const params = { pageNumber: 1, pageSize: 100, interviewId };
    return this.http.post<SearchResponse<InterviewFeedback>>(
      `${endpoint.interviewFeedbackUrl}/search`,
      params,
      { headers: this.getAuthHeaders() }
    );
  }

  private fetchCandidateDetails(): void {
    this.subs.push(
      this.http
        .get<any>(`${endpoint.userDetailsUrl}/${this.candidateId}`, {
          headers: this.getAuthHeaders(),
        })
        .pipe(catchError(() => of(null)))
        .subscribe((res) => {
          if (res?.userName) this.candidateName = res.userName;
           if (res.email) {
             this.candidateEmail = res.email;
           }
        })
    );
  }

  private fetchJobDetails(): void {
    this.subs.push(
      this.http
        .get<any>(`${endpoint.jobDetailsUrl}/${this.jobId}`, {
          headers: this.getAuthHeaders(),
        })
        .pipe(catchError(() => of(null)))
        .subscribe((res) => {
          if (res?.name) this.jobTitle = res.name;
        })
    );
  }

  private calculateScores(): void {
    this.totalScore = 0;
    this.maxPossibleScore = 0;
    this.feedbackItems.forEach((f) => {
      this.totalScore += f.score;
      this.maxPossibleScore += 5;
    });
  }

  getScoreColor(score: number): string {
    if (score >= 4) return 'green';
    if (score >= 3) return 'orange';
    return 'red';
  }

  getScorePercentage(): number {
    return this.maxPossibleScore
      ? Math.round((this.totalScore / this.maxPossibleScore) * 100)
      : 0;
  }

  getOverallRating(): string {
    const pct = this.getScorePercentage();
    if (pct >= 80) return 'Excellent';
    if (pct >= 70) return 'Very Good';
    if (pct >= 60) return 'Good';
    if (pct >= 50) return 'Average';
    return 'Below Average';
  }

  /** Capture the report container DOM and download as PNG */
  downloadReportAsImage(): void {
    if (!this.reportContainer) {
      console.error('reportContainer not found');
      return;
    }

    const container = this.reportContainer.nativeElement;
    // Select the button bar elements to hide
    const actions = container.querySelectorAll('.btn-group, .text-end.mt-3');

    // Hide actions
    actions.forEach((el) => ((el as HTMLElement).style.display = 'none'));

    html2canvas(container, { scale: 2 })
      .then((canvas) => {
        // Restore actions immediately after render
        actions.forEach((el) => ((el as HTMLElement).style.display = ''));

        canvas.toBlob((blob) => {
          if (!blob) return;
          const url = URL.createObjectURL(blob);
          const a = document.createElement('a');
          a.href = url;
          a.download = `Interview_Report_${this.interview?.id}.png`;
          a.click();
          URL.revokeObjectURL(url);
        });
      })
      .catch((err) => {
        // Restore on error
        actions.forEach((el) => ((el as HTMLElement).style.display = ''));

        console.error('Error capturing report as image:', err);
        this.snackBar.open('Failed to capture image', 'Close', {
          duration: 3000,
        });
      });
  }

  downloadPDF(): void {
    if (!this.reportContainer) return;

    this.snackBar.open('Generating PDF...', '', { duration: 2000 });

    const container = this.reportContainer.nativeElement;
    const actions = container.querySelectorAll('.btn-group, .text-end.mt-3');

    // Hide action buttons
    actions.forEach((el) => ((el as HTMLElement).style.display = 'none'));

    // Temporarily force white background
    const originalBackground = container.style.background;
    container.style.background = 'white';

    const options = {
      scale: 1.5,
      useCORS: true,
      allowTaint: true,
      backgroundColor: '#ffffff',
    };

    html2canvas(container, options)
      .then((canvas) => {
        // Restore UI
        container.style.background = originalBackground;
        actions.forEach((el) => ((el as HTMLElement).style.display = ''));

        const imgWidth = 210; // A4 width in mm
        const pageHeight = 297; // A4 height in mm
        const imgHeight = (canvas.height * imgWidth) / canvas.width;

        const pdf = new jsPDF('p', 'mm', 'a4');
        let position = 0;

        const imgData = canvas.toDataURL('image/png');
        pdf.addImage(imgData, 'PNG', 0, position, imgWidth, imgHeight);

        let heightRemaining = imgHeight - pageHeight;
        while (heightRemaining > 0) {
          position -= pageHeight;
          pdf.addPage();
          pdf.addImage(imgData, 'PNG', 0, position, imgWidth, imgHeight);
          heightRemaining -= pageHeight;
        }

        pdf.save(`Interview_Report_${this.interview?.id || 'download'}.pdf`);

        this.ngZone.run(() => {
          this.snackBar.open('PDF downloaded successfully', 'Close', {
            duration: 3000,
          });
        });
      })
      .catch((error) => {
        // Restore on error
        container.style.background = originalBackground;
        actions.forEach((el) => ((el as HTMLElement).style.display = ''));

        console.error('Error generating PDF:', error);
        this.ngZone.run(() => {
          this.snackBar.open(
            'Failed to generate PDF. Please try again.',
            'Close',
            { duration: 5000 }
          );
        });
      });
  }

  downloadCSV(): void {
    const headers = ['Question', 'Response', 'Score'];
    let csv = headers.join(',') + '\n';
    this.feedbackItems.forEach((f) => {
      const q = `"${(f.interviewQuestionText || '').replace(/"/g, '""')}"`;
      const r = `"${(f.response || '').replace(/"/g, '""')}"`;
      csv += `${q},${r},${f.score}\n`;
    });
    csv += `\n"Total Score","${this.totalScore}/${
      this.maxPossibleScore
    }",${this.getScorePercentage()}%\n`;
    csv += `"Overall Rating","${this.getOverallRating()}",\n`;

    const blob = new Blob([csv], { type: 'text/csv' });
    const url = URL.createObjectURL(blob);
    const a = document.createElement('a');
    a.href = url;
    a.download = `Interview_Report_${this.interview?.id}.csv`;
    a.click();
    URL.revokeObjectURL(url);
    this.snackBar.open('CSV downloaded', 'Close', { duration: 3000 });
  }

  private getAuthHeaders(): any {
    const headers: any = { 'Content-Type': 'application/json' };
    try {
      const stored =
        sessionStorage.getItem('userData') ||
        sessionStorage.getItem('loggedInUser');
      if (stored) {
        const p = JSON.parse(stored);
        if (p?.token) headers['Authorization'] = `Bearer ${p.token}`;
      }
    } catch {}
    return headers;
  }
}
