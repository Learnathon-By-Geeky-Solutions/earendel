import { ComponentFixture, TestBed } from '@angular/core/testing';

import { RubricsConfigComponent } from './rubrics-config.component';

describe('RubricsConfigComponent', () => {
  let component: RubricsConfigComponent;
  let fixture: ComponentFixture<RubricsConfigComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [RubricsConfigComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(RubricsConfigComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
