import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TechnologySelectionComponent } from './technology-selection.component';

describe('TechnologySelectionComponent', () => {
  let component: TechnologySelectionComponent;
  let fixture: ComponentFixture<TechnologySelectionComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [TechnologySelectionComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(TechnologySelectionComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
