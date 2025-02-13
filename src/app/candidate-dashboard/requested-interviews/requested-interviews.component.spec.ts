import { ComponentFixture, TestBed } from '@angular/core/testing';

import { RequestedInterviewsComponent } from './requested-interviews.component';

describe('RequestedInterviewsComponent', () => {
  let component: RequestedInterviewsComponent;
  let fixture: ComponentFixture<RequestedInterviewsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [RequestedInterviewsComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(RequestedInterviewsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
