import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CandidateSidebarComponent } from './candidate-sidebar.component';

describe('CandidateSidebarComponent', () => {
  let component: CandidateSidebarComponent;
  let fixture: ComponentFixture<CandidateSidebarComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [CandidateSidebarComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(CandidateSidebarComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
