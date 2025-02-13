import { ComponentFixture, TestBed } from '@angular/core/testing';

import { InterviewerDashboardComponent } from './interviewer-dashboard.component';

describe('InterviewerDashboardComponent', () => {
  let component: InterviewerDashboardComponent;
  let fixture: ComponentFixture<InterviewerDashboardComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [InterviewerDashboardComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(InterviewerDashboardComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
