import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SenioritySelectionComponent } from './seniority-selection.component';

describe('SenioritySelectionComponent', () => {
  let component: SenioritySelectionComponent;
  let fixture: ComponentFixture<SenioritySelectionComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [SenioritySelectionComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(SenioritySelectionComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
