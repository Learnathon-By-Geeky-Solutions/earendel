import { ComponentFixture, TestBed } from '@angular/core/testing';

import { InterviewTrendsComponent } from './interview-trends.component';

describe('InterviewTrendsComponent', () => {
  let component: InterviewTrendsComponent;
  let fixture: ComponentFixture<InterviewTrendsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [InterviewTrendsComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(InterviewTrendsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
