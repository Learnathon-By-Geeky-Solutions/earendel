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
  templateUrl: './zoomsdk.component.html',
  standalone: true,
  styleUrl: './zoomsdk.component.css',
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
    this.meetingNumber = `${this.route.snapshot.queryParamMap.get('meetingNumber')}`;
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
