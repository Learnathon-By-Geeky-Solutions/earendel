import { Routes } from '@angular/router';
import { SettingsComponent } from './settings/settings.component';
import { DashboardComponent } from './dashboard/dashboard.component';
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

export const routes: Routes = [
  { path: '', component: LandingComponent },

  // { path: 'settings', component: SettingsComponent },
  // { path: 'candidate-dashboard', component: DashboardComponent },
  // { path: 'interview-setup', component: InterviewSetupComponent },

  {
    path: 'candidate-dashboard',
    component: CandidateLayoutComponent,
    children: [
      { path: 'requested', component: RequestedInterviewsComponent },
      { path: 'completed', component: CompletedInterviewsComponent },
      { path: 'jobs', component: JobPostsComponent },
      { path: 'quiz', component: QuizListComponent },
      { path: 'quiz/start', component: QuizStartComponent },
      { path: 'quiz/interface', component: QuizInterfaceComponent },
      { path: 'quiz/results', component: QuizResultsComponent },
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

  { path: 'register', component: RegistrationComponent },
  { path: 'login', component: LoginComponent },

  {
    path: 'hr-dashboard',
    children: [
      { path: 'overview', component: HrDashboardComponent },
      { path: 'jobs', component: JobComponent },
      { path: 'payments', component: PaymentComponent },
      { path: 'notifications', component: NotificationComponent },
      { path: 'job-post', component: ProfileSelectionComponent },
      { path: 'customize/:domain', component: TechnologySelectionComponent },
      {
        path: 'seniority/:domain/:tech',
        component: SenioritySelectionComponent,
      },
      {
        path: 'customized/:domain/:tech/:seniority',
        component: CustomizedInterviewComponent,
      },
      { path: '', redirectTo: 'overview', pathMatch: 'full' },
    ],
  },

  // { path: 'hr-dashboard', component: HrDashboardComponent },
  // { path: 'job-post', component: ProfileSelectionComponent },
  // { path: 'customize/:domain', component: TechnologySelectionComponent },
  // { path: 'seniority/:domain/:tech', component: SenioritySelectionComponent },
  // {
  //   path: 'customized/:domain/:tech/:seniority',
  //   component: CustomizedInterviewComponent,
  // },
  // { path: 'backend-skills', component: BackendSkillsComponent },
  // { path: 'interview-request', component: InterviewRequestComponent },

  // { path: 'seniority-select', component: SenioritySelectComponent },
  // { path: 'customize-interview', component: CustomizeInterviewComponent },

  // { path: 'availability', component: AvailabilityComponent },
  // { path: 'upcoming', component: UpcomingInterviewsComponent },
  // { path: 'earnings', component: EarningsComponent },
  // { path: 'profile', component: ProfileComponent },

  {
    path: 'interviewer-dashboard',
    children: [
      { path: '', component: InterviewerDashboardComponent },
      { path: 'availability', component: AvailabilityComponent },
      { path: 'upcoming', component: UpcomingInterviewsComponent },
      { path: 'earnings', component: EarningsComponent },
      { path: 'profile', component: ProfileComponent },
    ],
  },

  {
    path: 'admin-dashboard',
    children: [
      { path: 'overview', component: AdminDashboardComponent },
      { path: 'candidates', component: CandidatesComponent },
      { path: 'hr', component: HrComponent },
      { path: 'interviewers', component: InterviewersComponent },
      { path: 'skills', component: SkillsComponent },
      { path: 'jobs', component: JobsComponent },
      { path: 'payments', component: PaymentsComponent },
      { path: 'notifications', component: NotificationsComponent },
      { path: 'verification', component: VerificationComponent },
      { path: 'admin-list', component: AdminListComponent },
      { path: 'profile', component: AdminProfileComponent },

      { path: '', redirectTo: 'overview', pathMatch: 'full' },
    ],
  },
  { path: 'admin-dashboard', redirectTo: 'overview', pathMatch: 'full' },
  { path: '**', redirectTo: '' }
];
