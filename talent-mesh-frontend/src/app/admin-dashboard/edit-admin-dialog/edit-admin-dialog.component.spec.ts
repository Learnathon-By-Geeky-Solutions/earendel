import { ComponentFixture, TestBed } from '@angular/core/testing';

import { EditAdminDialogComponent } from './edit-admin-dialog.component';

describe('EditAdminDialogComponent', () => {
  let component: EditAdminDialogComponent;
  let fixture: ComponentFixture<EditAdminDialogComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [EditAdminDialogComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(EditAdminDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
