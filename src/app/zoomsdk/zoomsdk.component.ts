import { Component, OnInit, Inject, NgZone } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { CommonModule, DOCUMENT } from '@angular/common';
import { ZoomMtg } from '@zoom/meetingsdk';
import { CodeComponent } from '../code/code.component';
import { HomeService } from '../shared/services/home.service';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { environment } from '../../environments/environment';

ZoomMtg.preLoadWasm();
console.log('Zoom SDK prepared.');

@Component({
  selector: 'app-root',
  template: `
    <main class="main-container">
      <div class="video-editor-wrapper">
        <!-- Zoom Meeting Container -->
        <div class="zoom-container">
          <h1>Zoom Meeting</h1>
          <div id="zmmtg-root"></div>

          <button (click)="getSignature()" *ngIf="!isMeetingJoined">
            Join Meeting
          </button>
        </div>

        <!-- Code Editor Container -->
        <div class="editor-container" *ngIf="role !== 1">
          <app-monaco-editor></app-monaco-editor>
        </div>
      </div>
    </main>
  `,
  standalone: true,
  styles: [
    `
      .main-container {
        width: 100%;
        height: 100vh;
        padding: 20px;
        background-color: #1e1e1e;
      }

      .video-editor-wrapper {
        display: flex;
        gap: 20px;
        height: calc(100vh - 40px);
        min-height: 600px;
      }

      .zoom-container {
        flex: 2;
        display: flex;
        flex-direction: column;
        background: #252526;
        border-radius: 8px;
        padding: 20px;
      }

      #zmmtg-root {
        flex: 1;
        min-height: 400px;
        margin: 20px 0;
        background: #1c1c1c;
        border-radius: 8px;
        overflow: hidden;
      }

      .editor-container {
        flex: 1;
        background: #252526;
        border-radius: 8px;
        overflow: hidden;
      }

      button {
        margin-top: auto;
        padding: 12px 24px;
        background: #007acc;
        color: white;
        border: none;
        border-radius: 4px;
        cursor: pointer;
      }

      @media (max-width: 768px) {
        .video-editor-wrapper {
          flex-direction: column;
          height: auto;
        }

        #zmmtg-root {
          min-height: 300px;
        }

        .editor-container {
          min-height: 500px;
        }
      }
    `,
  ],
  imports: [CommonModule, CodeComponent, RouterLink],
})
export class ZoomsdkComponent implements OnInit {
  isMeetingJoined = false;
  sdkKey = environment.zoomSdkKey;
  meetingNumber!: string;
  passWord = environment.zoomSdkPassword;
  role!: number;
  userName = 'Angular';
  userEmail = 'mdnafiulhasanhamim12345@gmail.com';
  leaveUrl = '';

  constructor(
    private httpClient: HttpClient,
    @Inject(DOCUMENT) private document: Document,
    private ngZone: NgZone,
    private homeService: HomeService,
    private route: ActivatedRoute,
    private router: Router
  ) {}

  ngOnInit() {
    this.meetingNumber =
      this.route.snapshot.queryParamMap.get('meetingNumber') || '';
    const interviewerId =
      this.route.snapshot.queryParamMap.get('interviewerId');
    const interviewId = this.route.snapshot.queryParamMap.get('interviewId');
    const user = JSON.parse(sessionStorage.getItem('loggedInUser') || '{}');
    this.role = interviewerId === user.userId ? 1 : 0;

    // Set dynamic leaveUrl based on role
    if (this.role === 1 && interviewId) {
      this.leaveUrl = `https://talent-mesh-frontend.netlify.app/interviewer-dashboard/interview-feedback?interviewId=${interviewId}`;
    } else {
      this.leaveUrl =
        'https://talent-mesh-frontend.netlify.app/candidate-dashboard';
    }
  }

  getSignature() {
    this.homeService
      .getZoomSignature({ meetingNumber: this.meetingNumber, role: this.role })
      .subscribe(
        (data: any) => this.startMeeting(data.signature),
        (error: any) => console.error('Something went wrong', error)
      );
  }

  startMeeting(signature: string) {
    ZoomMtg.init({
      leaveUrl: this.leaveUrl,
      patchJsMedia: true,
      disableCallOut: true,
      success: () => {
        // style adjustments
        const zoomRoot = this.document.getElementById('zmmtg-root')!;
        Object.assign(zoomRoot.style, {
          position: 'relative',
          width: '100%',
          height: '100%',
          minHeight: '600px',
          overflow: 'hidden',
        });

        ZoomMtg.inMeetingServiceListener('onMeetingStatus', (data: any) => {
          if (data.status === 'MEETING_STATUS_INMEETING') {
            this.document
              .querySelectorAll('.footer-button__list')
              .forEach((el) => el.remove());
          }
        });

        ZoomMtg.join({
          signature,
          sdkKey: this.sdkKey,
          meetingNumber: this.meetingNumber,
          passWord: this.passWord,
          userName: this.userName,
          userEmail: this.userEmail,
          success: () => {
            this.isMeetingJoined = true;
            setTimeout(() => {
              this.document
                .querySelectorAll('.react-draggable')
                .forEach((el) => {
                  (el as HTMLElement).style.transform = 'none !important';
                });
            }, 1000);
          },
          error: console.error,
        });
      },
      error: (error: any) => {
        console.error(error);
        this.isMeetingJoined = false;
      },
    });
  }
}
