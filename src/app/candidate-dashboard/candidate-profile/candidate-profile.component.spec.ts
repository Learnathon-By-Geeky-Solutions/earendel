import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CandidateProfileComponent } from './candidate-profile.component';

describe('CandidateProfileComponent', () => {
  let component: CandidateProfileComponent;
  let fixture: ComponentFixture<CandidateProfileComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [CandidateProfileComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(CandidateProfileComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
