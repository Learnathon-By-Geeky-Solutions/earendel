import { ComponentFixture, TestBed } from '@angular/core/testing';

import { InterviewPerformanceComponent } from './interview-performance.component';

describe('InterviewPerformanceComponent', () => {
  let component: InterviewPerformanceComponent;
  let fixture: ComponentFixture<InterviewPerformanceComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [InterviewPerformanceComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(InterviewPerformanceComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
