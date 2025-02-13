import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TopSkillsComponent } from './top-skills.component';

describe('TopSkillsComponent', () => {
  let component: TopSkillsComponent;
  let fixture: ComponentFixture<TopSkillsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [TopSkillsComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(TopSkillsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
