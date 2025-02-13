import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AddAdminDialogComponent } from './add-admin-dialog.component';

describe('AddAdminDialogComponent', () => {
  let component: AddAdminDialogComponent;
  let fixture: ComponentFixture<AddAdminDialogComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [AddAdminDialogComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(AddAdminDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
