import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SenioritySelectComponent } from './seniority-select.component';

describe('SenioritySelectComponent', () => {
  let component: SenioritySelectComponent;
  let fixture: ComponentFixture<SenioritySelectComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [SenioritySelectComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(SenioritySelectComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
