import { ComponentFixture, TestBed } from '@angular/core/testing';

import { StatsCardsComponent } from './stats-cards.component';

describe('StatsCardsComponent', () => {
  let component: StatsCardsComponent;
  let fixture: ComponentFixture<StatsCardsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [StatsCardsComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(StatsCardsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
