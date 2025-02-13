import { ComponentFixture, TestBed } from '@angular/core/testing';

import { BackendSkillsComponent } from './backend-skills.component';

describe('BackendSkillsComponent', () => {
  let component: BackendSkillsComponent;
  let fixture: ComponentFixture<BackendSkillsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [BackendSkillsComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(BackendSkillsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
