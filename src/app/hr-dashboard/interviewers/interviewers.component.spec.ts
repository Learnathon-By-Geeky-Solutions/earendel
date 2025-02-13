import { ComponentFixture, TestBed } from '@angular/core/testing';

import { InterviewersComponent } from './interviewers.component';

describe('InterviewersComponent', () => {
  let component: InterviewersComponent;
  let fixture: ComponentFixture<InterviewersComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [InterviewersComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(InterviewersComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
