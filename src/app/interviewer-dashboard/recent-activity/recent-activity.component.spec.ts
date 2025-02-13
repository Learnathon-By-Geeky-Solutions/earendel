import { ComponentFixture, TestBed } from '@angular/core/testing';

import { RecentActivityComponent } from './recent-activity.component';

describe('RecentActivityComponent', () => {
  let component: RecentActivityComponent;
  let fixture: ComponentFixture<RecentActivityComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [RecentActivityComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(RecentActivityComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
