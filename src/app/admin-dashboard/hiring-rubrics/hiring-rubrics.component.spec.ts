import { ComponentFixture, TestBed } from '@angular/core/testing';

import { HiringRubricsComponent } from './hiring-rubrics.component';

describe('HiringRubricsComponent', () => {
  let component: HiringRubricsComponent;
  let fixture: ComponentFixture<HiringRubricsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [HiringRubricsComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(HiringRubricsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
