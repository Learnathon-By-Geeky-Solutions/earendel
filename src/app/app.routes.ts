import { Routes } from '@angular/router';
import { SettingsComponent } from './settings/settings.component';
import { DashboardComponent } from './dashboard/dashboard.component';
import { InterviewRequestComponent } from './hr-dashboard/interview-request/interview-request.component';
import { InterviewerDashboardComponent } from './interviewer-dashboard/interviewer-dashboard/interviewer-dashboard.component';
import { AvailabilityComponent } from './interviewer-dashboard/availability/availability.component';
import { UpcomingInterviewsComponent } from './interviewer-dashboard/upcoming-interviews/upcoming-interviews.component';
import { EarningsComponent } from './interviewer-dashboard/earnings/earnings.component';
import { ProfileComponent } from './interviewer-dashboard/profile/profile.component';
import { HrDashboardComponent } from './hr-dashboard/dashboard/dashboard.component';
import { InterviewSetupComponent } from './candidate-dashboard/interview-setup/interview-setup.component';
import { LandingComponent } from './landing-page/landing/landing.component';
import { ProfileSelectionComponent } from './hr-dashboard/job-post/profile-selection/profile-selection.component';
import { TechnologySelectionComponent } from './hr-dashboard/job-post/technology-selection/technology-selection.component';
import { SenioritySelectionComponent } from './hr-dashboard/job-post/seniority-selection/seniority-selection.component';
import { CustomizedInterviewComponent } from './hr-dashboard/job-post/customize-interview/customize-interview.component';
import { AdminDashboardComponent } from './admin-dashboard/dashboard/dashboard.component';
import { CandidatesComponent } from './admin-dashboard/candidates/candidates.component';
import { HrComponent } from './admin-dashboard/hr/hr.component';
import { InterviewersComponent } from './admin-dashboard/interviewers/interviewers.component';
import { PaymentsComponent } from './admin-dashboard/payments/payments.component';
import { VerificationComponent } from './admin-dashboard/verification/verification.component';
import { RequestedInterviewsComponent } from './candidate-dashboard/requested-interviews/requested-interviews.component';
import { CompletedInterviewsComponent } from './candidate-dashboard/completed-interviews/completed-interviews.component';
import { JobPostsComponent } from './candidate-dashboard/job-posts/job-posts.component';
import { CandidateProfileComponent } from './candidate-dashboard/candidate-profile/candidate-profile.component';
import { CandidateLayoutComponent } from './candidate-dashboard/candidate-layout/candidate-layout.component';
import { QuizListComponent } from './candidate-dashboard/quiz-list/quiz-list.component';
import { QuizStartComponent } from './candidate-dashboard/quiz-start/quiz-start.component';
import { QuizInterfaceComponent } from './candidate-dashboard/quiz-interface/quiz-interface.component';
import { QuizResultsComponent } from './candidate-dashboard/quiz-results/quiz-results.component';
import { SkillsComponent } from './admin-dashboard/skills/skills.component';
import { JobsComponent } from './admin-dashboard/jobs/jobs.component';
import { NotificationsComponent } from './admin-dashboard/notifications/notifications.component';
import { RegistrationComponent } from './registration/registration.component';
import { LoginComponent } from './login/login.component';
import { CandidateNotificationsComponent } from './candidate-dashboard/notifications/notifications.component';
import { CandidateComponent } from './hr-dashboard/candidates/candidates.component';
import { HRComponent } from './hr-dashboard/hr/hr.component';
import { InterviewerComponent } from './hr-dashboard/interviewers/interviewers.component';
import { SkillComponent } from './hr-dashboard/skills/skills.component';
import { JobComponent } from './hr-dashboard/jobs/jobs.component';
import { PaymentComponent } from './hr-dashboard/payments/payments.component';
import { NotificationComponent } from './hr-dashboard/notifications/notifications.component';
import { AdminListComponent } from './admin-dashboard/admin-list/admin-list.component';
import { AdminProfileComponent } from './admin-dashboard/profile/profile.component';
import { InterviewerRegistrationComponent } from './candidate-dashboard/interviewer-registration/interviewer-registration.component';
import { AuthGuardService } from './shared/services/auth-guard.service';
import { GithubcallbackComponent } from './githubcallback/githubcallback.component';
import { HiringRubricsComponent } from './admin-dashboard/hiring-rubrics/hiring-rubrics.component';
import { SenioritiesComponent } from './admin-dashboard/seniorities/seniorities.component';
import { QuizzesComponent } from './admin-dashboard/quizzes/quizzes.component';
import { ZoomsdkComponent } from './zoomsdk/zoomsdk.component';
import { CodeComponent } from './code/code.component';
import { InterviewerNotificationComponent } from './interviewer-dashboard/notifications/notifications.component';
import { PendingRequestComponent } from './interviewer-dashboard/pending-request/pending-request.component';
import { PastInterviewsComponent } from './interviewer-dashboard/past-interviews/past-interviews.component';
import { FeedbackComponent } from './interviewer-dashboard/feedback/feedback.component';
import { InterviewReportComponent } from './hr-dashboard/jobs/interview-report/interview-report.component';

export const routes: Routes = [
  { path: '', component: LandingComponent },

  // Candidate dashboard accessible to "Candidate" and "Interviewer"
  {
    path: 'candidate-dashboard',
    component: CandidateLayoutComponent,
    canActivate: [AuthGuardService],
    data: { roles: ['Candidate', 'Interviewer'] },
    children: [
      { path: 'requested', component: RequestedInterviewsComponent },
      { path: 'completed', component: CompletedInterviewsComponent },
      { path: 'jobs', component: JobPostsComponent },
      { path: 'quiz', component: QuizListComponent },
      { path: 'quiz/start', component: QuizStartComponent },
      { path: 'quiz/interface', component: QuizInterfaceComponent },
      { path: 'quiz/results/:attemptId', component: QuizResultsComponent },
      { path: 'profile', component: CandidateProfileComponent },
      { path: 'notifications', component: CandidateNotificationsComponent },
      { path: 'interview-setup', component: InterviewSetupComponent },
      {
        path: 'interviewer-registration',
        component: InterviewerRegistrationComponent,
      },
      { path: '', redirectTo: 'requested', pathMatch: 'full' },
    ],
  },

  { path: 'coding-playground', component: CodeComponent },
  { path: 'meeting', component: ZoomsdkComponent },

  {
    path: 'register',
    component: RegistrationComponent,
    canActivate: [AuthGuardService],
    data: { public: true, redirectIfLoggedIn: true },
  },
  {
    path: 'login',
    component: LoginComponent,
    canActivate: [AuthGuardService],
    data: { public: true, redirectIfLoggedIn: true },
  },
  {
    path: 'github-callback',
    component: GithubcallbackComponent,
  },
  // HR dashboard (only accessible to HR role)
  {
    path: 'hr-dashboard',
    canActivate: [AuthGuardService],
    data: { roles: ['HR'] },
    children: [
      { path: 'overview', component: HrDashboardComponent },
      { path: 'jobs', component: JobComponent },
      { path: 'payments', component: PaymentComponent },
      { path: 'notifications', component: NotificationComponent },
      { path: 'job-post', component: InterviewRequestComponent },
      { path: 'customize/:domain', component: TechnologySelectionComponent },
      {
        path: 'seniority/:domain/:tech',
        component: SenioritySelectionComponent,
      },
      {
        path: 'customized/:domain/:tech/:seniority',
        component: CustomizedInterviewComponent,
      },
      { path: 'job-post/technology-selection',
        component: TechnologySelectionComponent 
      },
      { path: 'job-post/seniority-selection',
        component: SenioritySelectionComponent 
      },
      { path: 'job-post/customize-interview',
        component: CustomizedInterviewComponent 
      },
      { path: 'jobs/interview-report',
        component: InterviewReportComponent 
      },

      { path: '', redirectTo: 'overview', pathMatch: 'full' },
    ],
  },

  // Interviewer dashboard (only accessible to Interviewer role)
  {
    path: 'interviewer-dashboard',
    canActivate: [AuthGuardService],
    data: { roles: ['Interviewer'] },
    children: [
      { path: '', component: InterviewerDashboardComponent },
      { path: 'availability', component: AvailabilityComponent },
      { path: 'pending-request', component: PendingRequestComponent },
      { path: 'upcoming', component: UpcomingInterviewsComponent },
      { path: 'past-interviews', component: PastInterviewsComponent },
      { path: 'earnings', component: EarningsComponent },
      { path: 'profile', component: ProfileComponent },
      { path: 'notifications', component: InterviewerNotificationComponent },
      { path: 'interview-feedback', component: FeedbackComponent },
    ],
  },

  // Admin dashboard (only accessible to Admin role)
  {
    path: 'admin-dashboard',
    canActivate: [AuthGuardService],
    data: { roles: ['Admin'] },
    children: [
      { path: 'overview', component: AdminDashboardComponent },
      { path: 'candidates', component: CandidatesComponent },
      { path: 'hr', component: HrComponent },
      { path: 'interviewers', component: InterviewersComponent },
      { path: 'seniorities', component: SenioritiesComponent },
      { path: 'quizzes', component: QuizzesComponent },
      { path: 'skills', component: SkillsComponent },
      { path: 'rubrics', component: HiringRubricsComponent },
      { path: 'jobs', component: JobsComponent },
      { path: 'payments', component: PaymentsComponent },
      { path: 'notifications', component: NotificationsComponent },
      { path: 'verification', component: VerificationComponent },
      { path: 'admin-list', component: AdminListComponent },
      { path: 'profile', component: AdminProfileComponent },
      { path: '', redirectTo: 'overview', pathMatch: 'full' },
    ],
  },
  // In case of duplicate path definition, you might remove or adjust the following redirect.
  { path: 'admin-dashboard', redirectTo: 'overview', pathMatch: 'full' },
];
