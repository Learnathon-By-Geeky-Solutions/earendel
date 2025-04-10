import { ComponentFixture, TestBed } from '@angular/core/testing';

import { GithubcallbackComponent } from './githubcallback.component';

describe('GithubcallbackComponent', () => {
  let component: GithubcallbackComponent;
  let fixture: ComponentFixture<GithubcallbackComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [GithubcallbackComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(GithubcallbackComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
