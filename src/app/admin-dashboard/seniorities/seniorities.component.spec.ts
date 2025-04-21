import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SenioritiesComponent } from './seniorities.component';

describe('SenioritiesComponent', () => {
  let component: SenioritiesComponent;
  let fixture: ComponentFixture<SenioritiesComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [SenioritiesComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(SenioritiesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
