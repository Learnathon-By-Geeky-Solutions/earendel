import { ComponentFixture, TestBed } from '@angular/core/testing';

import { InterviewersDetailsComponent } from './interviewers-details.component';

describe('InterviewersDetailsComponent', () => {
  let component: InterviewersDetailsComponent;
  let fixture: ComponentFixture<InterviewersDetailsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [InterviewersDetailsComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(InterviewersDetailsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
