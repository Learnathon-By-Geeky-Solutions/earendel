import { Component, OnInit } from '@angular/core';
import { formatDate } from '@angular/common';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { SidebarComponent } from '../sidebar/sidebar.component';
import { InterviewerService, InterviewRequest, AvailableDate, TimeSlot } from '../services/interviewer.service';

interface Tab {
  value: string;
  label: string;
}

@Component({
  selector: 'app-pending-request',
  templateUrl: './pending-request.component.html',
  styleUrls: ['./pending-request.component.css'],
  standalone: true,
  imports: [CommonModule, FormsModule, RouterModule, SidebarComponent]
})
export class PendingRequestComponent implements OnInit {
  // Tab management
  tabs: Tab[] = [
    { value: 'pending', label: 'Pending' },
    { value: 'accepted', label: 'Accepted' },
    { value: 'declined', label: 'Declined' }
  ];
  activeTab: string = 'pending';

  // Interview data
  requestedInterviews: InterviewRequest[] = [];
  isLoading: boolean = false;
  error: string | null = null;

  // Available time slots
  availableDates: AvailableDate[] = [];
  isLoadingTimeSlots: boolean = false;
  timeSlotError: string | null = null;

  // Dialog state
  isDialogOpen: boolean = false;
  selectedInterview: string | null = null;
  selectedDateSlot: string | null = null;
  openDates: Record<string, boolean> = {};

  constructor(private interviewerService: InterviewerService) { }

  ngOnInit(): void {
    // Load interview data from API
    this.loadInterviews();
  }
  
  loadInterviews(): void {
    this.isLoading = true;
    this.error = null;
    
    this.interviewerService.getPendingInterviews()
      .subscribe({
        next: (data: InterviewRequest[]) => {
          this.requestedInterviews = data;
          this.isLoading = false;
        },
        error: (err: any) => {
          console.error('Error fetching interview requests:', err);
          this.error = 'Failed to load interview requests. Please try again later.';
          this.isLoading = false;
        }
      });
  }

  setActiveTab(tab: string): void {
    this.activeTab = tab;
    
    // In a real implementation, you would load different interviews based on the active tab
    // This is a placeholder for future implementation
    if (tab === 'pending') {
      this.loadInterviews();
    } else {
      // For now, we'll just show empty states for accepted and declined
      this.requestedInterviews = [];
    }
  }

  handleAccept(interviewId: string): void {
    this.selectedInterview = interviewId;
    this.isDialogOpen = true;
    this.selectedDateSlot = null;
    
    // Fetch available time slots from API
    this.loadAvailableTimeSlots();
  }
  
  loadAvailableTimeSlots(): void {
    this.isLoadingTimeSlots = true;
    this.timeSlotError = null;
    
    this.interviewerService.getAvailableTimeSlots()
      .subscribe({
        next: (data: AvailableDate[]) => {
          this.availableDates = data;
          this.isLoadingTimeSlots = false;
          
          // If no available dates, show error
          if (this.availableDates.length === 0) {
            this.timeSlotError = 'No available time slots found. Please set your availability first.';
          }
          
          // Initialize all dates as open for better UX
          this.availableDates.forEach(date => {
            this.openDates[date.date] = true;
          });
        },
        error: (err: any) => {
          console.error('Error fetching available time slots:', err);
          this.timeSlotError = 'Failed to load available time slots. Please try again later.';
          this.isLoadingTimeSlots = false;
        }
      });
  }

  closeDialog(): void {
    this.isDialogOpen = false;
    this.selectedInterview = null;
    this.selectedDateSlot = null;
  }

  toggleDateCollapsible(date: string): void {
    this.openDates[date] = !this.openDates[date];
  }

  isDateOpen(date: string): boolean {
    return !!this.openDates[date];
  }

  handleConfirm(): void {
    if (!this.selectedDateSlot) return;

    // Parse the selected value to get date and slot ID
    const [date, slotId] = this.selectedDateSlot.split('|');

    // Find the selected date and slot
    const selectedDate = this.availableDates.find(d => d.date === date);
    const selectedSlot = selectedDate?.slots.find(s => s.id.toString() === slotId);

    if (selectedDate && selectedSlot) {
      // Here you would handle the API call to accept the interview
      console.log(
        `Accepting interview ${this.selectedInterview} on ${this.formatFullDate(selectedDate.date)} from ${this.formatTime(selectedSlot.start)} to ${this.formatTime(selectedSlot.end)}`
      );
      
      // TODO: Implement the API call to update the interview status to Accepted
      // this.interviewerService.acceptInterview(this.selectedInterview, { date, slotId })
      //   .subscribe(...)
    }

    this.closeDialog();
  }

  formatDate(dateString: string): string {
    const date = new Date(dateString);
    return formatDate(date, 'MMM d, yyyy', 'en-US');
  }

  formatFullDate(dateString: string): string {
    const date = new Date(dateString);
    return formatDate(date, 'EEEE, MMMM d, yyyy', 'en-US');
  }

  formatTime(time: string): string {
    const [hours, minutes] = time.split(':');
    const hour = parseInt(hours);
    const ampm = hour >= 12 ? 'PM' : 'AM';
    const formattedHour = hour % 12 || 12;
    return `${formattedHour}:${minutes} ${ampm}`;
  }
}