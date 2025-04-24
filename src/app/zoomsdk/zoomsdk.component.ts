import { Component, OnInit, Inject, NgZone } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { CommonModule, DOCUMENT } from '@angular/common';

import { ZoomMtg } from '@zoom/meetingsdk';
import { CodeComponent } from '../code/code.component';
import { HomeService } from '../shared/services/home.service';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { environment } from '../../environments/environment';

ZoomMtg.preLoadWasm();
// ZoomMtg.prepareWebSDK();

console.log('Zoom SDK prepared.');

@Component({
  selector: 'app-root',
  // templateUrl: './zoomsdk.component.html',
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
        <div class="editor-container">
          <app-monaco-editor></app-monaco-editor>
        </div>
      </div>
    </main>
  `,
  standalone: true,
  // styleUrl: './zoomsdk.component.css',
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
        min-height: 600px; /* Add minimum height */
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

  authEndpoint = '';
  sdkKey = `${environment.zoomSdkKey}`;
  // meetingNumber = '75964314386';
  meetingNumber: any;
  passWord = `${environment.zoomSdkPassword}`;
  role: any;
  userName = 'Angular';
  // userEmail = 'mdnafiulhasanhamim12345@gmail.com';
  userEmail = 'mdnafiulhasanhamim12345@gmail.com';
  registrantToken = '';
  zakToken = '';
  leaveUrl = 'http://localhost:4200';

  constructor(
    public httpClient: HttpClient,
    @Inject(DOCUMENT) document: any,
    private ngZone: NgZone,
    private homeService: HomeService,
    private route: ActivatedRoute,
    private router: Router
  ) {}

  ngOnInit() {
    this.meetingNumber = `${this.route.snapshot.queryParamMap.get(
      'meetingNumber'
    )}`;
    const interviewerId =
      this.route.snapshot.queryParamMap.get('interviewerId');
    const user = JSON.parse(sessionStorage.getItem('loggedInUser') || '{}');
    if (interviewerId === user.userId) {
      console.log('matched with interviewer id');
      this.role = 1;
    } else {
      this.role = 0;
    }
  }

  getSignature() {
    this.homeService
      .getZoomSignature({
        meetingNumber: this.meetingNumber,
        role: this.role,
      })
      .subscribe(
        (data) => {
          console.log(data);
          this.startMeeting(data.signature);
        },
        (error: any) => {
          console.log('Somthing went wrong ', error);
        }
      );
  }

  // zoomsdk.component.ts
  startMeeting(signature: string) {
    ZoomMtg.init({
      leaveUrl: this.leaveUrl,
      patchJsMedia: true,
      disableCallOut: true,
      success: () => {
        // Clean up Zoom's default styles
        const zoomRoot = document.getElementById('zmmtg-root')!;
        Object.assign(zoomRoot.style, {
          position: 'relative',
          width: '100%',
          height: '100%',
          minHeight: '600px',
          overflow: 'hidden',
        });

        // Remove Zoom's default footer
        ZoomMtg.inMeetingServiceListener('onMeetingStatus', (data: any) => {
          if (data.status === 'MEETING_STATUS_INMEETING') {
            document
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

            // Additional UI cleanup after join
            setTimeout(() => {
              document.querySelectorAll('.react-draggable').forEach((el) => {
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
