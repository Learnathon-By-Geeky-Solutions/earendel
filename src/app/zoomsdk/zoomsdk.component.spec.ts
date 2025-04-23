import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ZoomsdkComponent } from './zoomsdk.component';

describe('ZoomsdkComponent', () => {
  let component: ZoomsdkComponent;
  let fixture: ComponentFixture<ZoomsdkComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ZoomsdkComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(ZoomsdkComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
