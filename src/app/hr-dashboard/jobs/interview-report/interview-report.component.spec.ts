import { ComponentFixture, TestBed } from '@angular/core/testing';

import { InterviewReportComponent } from './interview-report.component';

describe('InterviewReportComponent', () => {
  let component: InterviewReportComponent;
  let fixture: ComponentFixture<InterviewReportComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [InterviewReportComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(InterviewReportComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
