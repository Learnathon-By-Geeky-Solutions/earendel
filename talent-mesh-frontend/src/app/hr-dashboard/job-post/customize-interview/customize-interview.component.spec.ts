import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CustomizeInterviewComponent } from './customize-interview.component';

describe('CustomizeInterviewComponent', () => {
  let component: CustomizeInterviewComponent;
  let fixture: ComponentFixture<CustomizeInterviewComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [CustomizeInterviewComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(CustomizeInterviewComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
