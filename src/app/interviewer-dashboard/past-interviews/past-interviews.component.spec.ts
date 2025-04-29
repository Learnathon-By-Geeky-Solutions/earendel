import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PastInterviewsComponent } from './past-interviews.component';

describe('PastInterviewsComponent', () => {
  let component: PastInterviewsComponent;
  let fixture: ComponentFixture<PastInterviewsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [PastInterviewsComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(PastInterviewsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
