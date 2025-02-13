import { ComponentFixture, TestBed } from '@angular/core/testing';

import { JobPostsComponent } from './job-posts.component';

describe('JobPostsComponent', () => {
  let component: JobPostsComponent;
  let fixture: ComponentFixture<JobPostsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [JobPostsComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(JobPostsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
