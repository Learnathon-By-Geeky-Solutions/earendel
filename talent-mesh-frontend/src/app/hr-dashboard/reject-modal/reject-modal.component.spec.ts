import { ComponentFixture, TestBed } from '@angular/core/testing';

import { RejectModalComponent } from './reject-modal.component';

describe('RejectModalComponent', () => {
  let component: RejectModalComponent;
  let fixture: ComponentFixture<RejectModalComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [RejectModalComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(RejectModalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
