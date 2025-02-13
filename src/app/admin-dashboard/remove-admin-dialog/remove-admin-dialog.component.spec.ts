import { ComponentFixture, TestBed } from '@angular/core/testing';

import { RemoveAdminDialogComponent } from './remove-admin-dialog.component';

describe('RemoveAdminDialogComponent', () => {
  let component: RemoveAdminDialogComponent;
  let fixture: ComponentFixture<RemoveAdminDialogComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [RemoveAdminDialogComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(RemoveAdminDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
