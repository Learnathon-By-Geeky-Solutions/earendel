import { ComponentFixture, TestBed } from '@angular/core/testing';
import { FormsModule } from '@angular/forms';
import { PendingRequestComponent } from './pending-request.component';

describe('PendingRequestComponent', () => {
  let component: PendingRequestComponent;
  let fixture: ComponentFixture<PendingRequestComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [PendingRequestComponent],
      imports: [FormsModule]
    }).compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(PendingRequestComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should display the correct number of pending requests', () => {
    const compiled = fixture.nativeElement;
    expect(compiled.querySelector('.header p').textContent).toContain('3 pending requests');
  });

  it('should show the pending tab by default', () => {
    expect(component.activeTab).toBe('pending');
  });

  it('should change tab when setActiveTab is called', () => {
    component.setActiveTab('accepted');
    expect(component.activeTab).toBe('accepted');
    fixture.detectChanges();
    const compiled = fixture.nativeElement;
    expect(compiled.querySelector('.empty-state').textContent).toContain('No accepted interviews yet');
  });

  it('should open dialog when handleAccept is called', () => {
    component.handleAccept(1);
    expect(component.isDialogOpen).toBe(true);
    expect(component.selectedInterview).toBe(1);
  });

  it('should close dialog when closeDialog is called', () => {
    component.isDialogOpen = true;
    component.closeDialog();
    expect(component.isDialogOpen).toBe(false);
  });

  it('should toggle date collapsible', () => {
    const date = '2025-05-02';
    component.openDates = {};
    component.toggleDateCollapsible(date);
    expect(component.openDates[date]).toBe(true);
    component.toggleDateCollapsible(date);
    expect(component.openDates[date]).toBe(false);
  });

  it('should format date correctly', () => {
    const formattedDate = component.formatDate('2023-08-15');
    expect(formattedDate).toBe('Aug 15, 2023');
  });

  it('should format time correctly', () => {
    const formattedTime = component.formatTime('14:30');
    expect(formattedTime).toBe('2:30 PM');
  });
});