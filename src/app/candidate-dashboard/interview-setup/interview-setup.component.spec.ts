import { ComponentFixture, TestBed } from '@angular/core/testing';

import { InterviewSetupComponent } from './interview-setup.component';

describe('InterviewSetupComponent', () => {
  let component: InterviewSetupComponent;
  let fixture: ComponentFixture<InterviewSetupComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [InterviewSetupComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(InterviewSetupComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
