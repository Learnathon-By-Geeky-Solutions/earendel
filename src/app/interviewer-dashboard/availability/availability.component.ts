import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { SidebarComponent } from '../sidebar/sidebar.component';
import { InterviewerService, AvailabilitySlot } from '../../shared/services/interviewer.service';

interface TimeSlot {
  id: number;
  startTime: string;
  endTime: string;
}

interface Interviewer {
  name: string;
  date: Date;
  timeSlots: TimeSlot[];
  isAvailable: boolean;
  expanded: boolean;
}

@Component({
  selector: 'app-availability',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterModule, SidebarComponent],
  templateUrl: './availability.component.html',
  styleUrls: ['./availability.component.css'],
})
export class AvailabilityComponent implements OnInit {
  daysOfWeek = ['Su', 'Mo', 'Tu', 'We', 'Th', 'Fr', 'Sa'];
  currentDate = new Date();
  currentMonth = this.currentDate.getMonth();
  currentYear = this.currentDate.getFullYear();
  selectedDate: Date | null = null;
  timeRanges: { startTime: string; endTime: string }[] = [
    { startTime: '09:00', endTime: '17:00' },
  ];

  // API integration states
  isLoading = false;
  apiError = "";

  monthNames = [
    'January',
    'February',
    'March',
    'April',
    'May',
    'June',
    'July',
    'August',
    'September',
    'October',
    'November',
    'December',
  ];

  calendarDays: {
    date: Date;
    dayNumber: number;
    isCurrentMonth: boolean;
    isDisabled: boolean;
  }[][] = [];

  interviewerAvailability: Interviewer[] = [
  
  ];

  showDeleteConfirmation = false;
  selectedInterviewer: Interviewer | null = null;
  selectedSlotId: number | null = null;
  editingSlot: { interviewer: Interviewer; slotIndex: number } | null = null;

  constructor(private interviewerService: InterviewerService) {
    this.generateCalendarDays();
  }

  ngOnInit(): void {
    // Debug session storage data
    console.log('===== SESSION STORAGE DEBUG =====');
    console.log('loggedInUser:', sessionStorage.getItem('loggedInUser'));
    console.log('userData:', sessionStorage.getItem('userData'));
    
    const loggedInUser = sessionStorage.getItem('loggedInUser');
    const userData = sessionStorage.getItem('userData');
    
    if (loggedInUser) {
      try {
        const parsedUser = JSON.parse(loggedInUser);
        console.log('Parsed loggedInUser userId:', parsedUser.userId);
        console.log('Parsed loggedInUser token exists:', !!parsedUser.token);
      } catch (e) {
        console.error('Error parsing loggedInUser:', e);
      }
    }
    
    if (userData) {
      try {
        const parsedData = JSON.parse(userData);
        console.log('Parsed userData userId:', parsedData.userId);
        console.log('Parsed userData token exists:', !!parsedData.token);
      } catch (e) {
        console.error('Error parsing userData:', e);
      }
    }
    console.log('================================');
    
    this.loadAvailabilities();
  }

  // Load availabilities from API
  loadAvailabilities(): void {
    this.isLoading = true;
    this.apiError = "";

    console.log('Loading interviewer availabilities from API...');

    this.interviewerService.searchAvailabilities().subscribe({
      next: (response) => {
        console.log('API response received:', response);
        this.isLoading = false;
        
        // Process API response - handle both potential response formats
        if (response && response.items && Array.isArray(response.items)) {
          // The API is returning items instead of data
          console.log('Found', response.items.length, 'availability records in items');
          
          if (response.items.length > 0) {
            this.processAvailabilityData(response.items);
          } else {
            console.log('No availability data returned from API');
          }
        } else if (response && response.data && Array.isArray(response.data)) {
          // In case API format changes back to data
          console.log('Found', response.data.length, 'availability records in data');
          
          if (response.data.length > 0) {
            this.processAvailabilityData(response.data);
          } else {
            console.log('No availability data returned from API');
          }
        } else if (Array.isArray(response)) {
          // Direct array response
          console.log('Found', response.length, 'availability records in array');
          
          if (response.length > 0) {
            this.processAvailabilityData(response);
          } else {
            console.log('No availability data returned from API');
          }
        } else {
          console.warn('Unexpected API response format:', response);
          this.apiError = "Could not load availabilities from server";
        }
      },
      error: (error) => {
        console.error('API error loading availabilities:', error);
        this.isLoading = false;
        this.apiError = "Error loading availabilities from server";
      }
    });
  }

  // Process availability data from API
  processAvailabilityData(data: any[]): void {
    console.log('Processing availability data...');
    
    if (!data || data.length === 0) {
      console.log('No data to process');
      return;
    }

    try {
      // Get current user info
      let userName = 'Current User'; 
      try {
        const userData = JSON.parse(sessionStorage.getItem('loggedInUser') || sessionStorage.getItem('userData') || '{}');
        userName = userData.name || userData.userName || 'Current User';
        console.log('Current user name:', userName);
      } catch (e) {
        console.error('Could not parse user data', e);
      }

      // Group API data by date
      const groupedByDate: {[key: string]: any[]} = {};
      
      data.forEach((item, index) => {
        console.log(`Processing item ${index}:`, item);
        
        if (!item) {
          console.log('Skipping null/undefined item');
          return;
        }
        
        // Handle various possible response formats
        let startTime: Date | undefined;
        let endTime: Date | undefined;
        let id: number = Math.floor(Math.random() * 10000); // Default ID
        
        if (item.startTime && item.endTime) {
          // Direct format
          startTime = new Date(item.startTime);
          endTime = new Date(item.endTime);
          id = item.id || Math.floor(Math.random() * 10000);
          console.log(`Direct format - Start: ${startTime}, End: ${endTime}, ID: ${id}`);
        } else if (item.availabilitySlot) {
          // Nested format
          startTime = new Date(item.availabilitySlot.startTime);
          endTime = new Date(item.availabilitySlot.endTime);
          id = item.id || Math.floor(Math.random() * 10000);
          console.log(`Nested format - Start: ${startTime}, End: ${endTime}, ID: ${id}`);
        } else if (item.availability && item.availability.startTime && item.availability.endTime) {
          // Another possible nested format
          startTime = new Date(item.availability.startTime);
          endTime = new Date(item.availability.endTime);
          id = item.id || Math.floor(Math.random() * 10000);
          console.log(`Availability nested format - Start: ${startTime}, End: ${endTime}, ID: ${id}`);
        } else if (item.interviewerAvailabilityId) {
          // Format from screenshots
          id = item.interviewerAvailabilityId || Math.floor(Math.random() * 10000);
          
          // Try different possible paths for time data
          if (item.startTime && item.endTime) {
            startTime = new Date(item.startTime);
            endTime = new Date(item.endTime);
          } else if (item.availabilitySlot) {
            startTime = new Date(item.availabilitySlot.startTime);
            endTime = new Date(item.availabilitySlot.endTime);
          }
          
          if (startTime && endTime) {
            console.log(`ID format - Start: ${startTime}, End: ${endTime}, ID: ${id}`);
          } else {
            console.warn('Missing time data for item with ID:', id);
            return;
          }
        } else {
          // Look for any properties that contain startTime/endTime
          const keys = Object.keys(item);
          for (const key of keys) {
            if (typeof item[key] === 'object' && item[key] !== null) {
              if (item[key].startTime && item[key].endTime) {
                startTime = new Date(item[key].startTime);
                endTime = new Date(item[key].endTime);
                id = item.id || item[key].id || Math.floor(Math.random() * 10000);
                console.log(`Found in ${key} - Start: ${startTime}, End: ${endTime}, ID: ${id}`);
                break;
              }
            }
          }
          
          if (!startTime || !endTime) {
            console.warn('Unknown item format:', item);
            return;
          }
        }
        
        // Ensure we have valid dates before proceeding
        if (!startTime || !endTime || isNaN(startTime.getTime()) || isNaN(endTime.getTime())) {
          console.warn('Invalid date format in item:', item);
          return;
        }
        
        const dateKey = startTime.toDateString();
        console.log('Date key:', dateKey);
        
        if (!groupedByDate[dateKey]) {
          groupedByDate[dateKey] = [];
        }
        
        groupedByDate[dateKey].push({
          id: id,
          startTime: this.formatTime(startTime),
          endTime: this.formatTime(endTime)
        });
      });
      
      console.log('Grouped data by date:', groupedByDate);
      
      // Create interviewer objects
      const newAvailability = Object.keys(groupedByDate).map(dateKey => ({
        name: userName,
        date: new Date(dateKey),
        timeSlots: groupedByDate[dateKey],
        isAvailable: true,
        expanded: false
      }));
      
      console.log('New availability data:', newAvailability);
      
      // Replace the existing data with new data from API
      if (newAvailability.length > 0) {
        this.interviewerAvailability = newAvailability;
      }
    } catch (error) {
      console.error('Error processing availability data:', error);
    }
  }

  // Format time from date to HH:MM
  formatTime(date: Date): string {
    return date.toTimeString().substring(0, 5);
  }

  generateCalendarDays() {
    const firstDayOfMonth = new Date(this.currentYear, this.currentMonth, 1);
    const lastDayOfMonth = new Date(this.currentYear, this.currentMonth + 1, 0);
    const daysInMonth = lastDayOfMonth.getDate();
    const startDayOfWeek = firstDayOfMonth.getDay();

    const today = new Date();
    today.setHours(0, 0, 0, 0);

    this.calendarDays = [];
    let week: {
      date: Date;
      dayNumber: number;
      isCurrentMonth: boolean;
      isDisabled: boolean;
    }[] = [];

    for (let i = 1; i <= daysInMonth; i++) {
      const date = new Date(this.currentYear, this.currentMonth, i);
      const isDisabled = date < today;
      week.push({
        date,
        dayNumber: i,
        isCurrentMonth: true,
        isDisabled,
      });

      if (week.length === 7 || i === daysInMonth) {
        this.calendarDays.push(week.filter((day) => day !== null));
        week = [];
      }
    }
  }

  previousMonth() {
    if (this.currentMonth === 0) {
      this.currentMonth = 11;
      this.currentYear--;
    } else {
      this.currentMonth--;
    }
    this.generateCalendarDays();
  }

  nextMonth() {
    if (this.currentMonth === 11) {
      this.currentMonth = 0;
      this.currentYear++;
    } else {
      this.currentMonth++;
    }
    this.generateCalendarDays();
  }

  isDateSelected(date: Date): boolean {
    return this.selectedDate?.getTime() === date.getTime();
  }

  toggleDate(date: Date): void {
    if (this.selectedDate?.getTime() === date.getTime()) {
      this.selectedDate = null;
    } else {
      this.selectedDate = date;
    }
  }

  addMoreTimeRange() {
    this.timeRanges.push({ startTime: '09:00', endTime: '17:00' });
  }

  removeTimeRange(index: number) {
    this.timeRanges.splice(index, 1);
  }

  getMinTime() {
    const now = new Date();
    const hours = now.getHours();
    const minutes = now.getMinutes();
    const minTime = new Date(now);
    minTime.setMinutes(minutes + 30);
    return minTime.toISOString().substring(11, 16);
  }

  editTimeSlot(interviewer: Interviewer, slotId: number) {
    const slotIndex = interviewer.timeSlots.findIndex((s) => s.id === slotId);
    if (slotIndex !== -1) {
      this.selectedDate = new Date(interviewer.date);
      this.timeRanges = [
        {
          startTime: interviewer.timeSlots[slotIndex].startTime,
          endTime: interviewer.timeSlots[slotIndex].endTime,
        },
      ];
      this.editingSlot = { interviewer, slotIndex };
    }
  }

  deleteTimeSlot(interviewer: Interviewer, slotId: number) {
    interviewer.timeSlots = interviewer.timeSlots.filter(
      (s) => s.id !== slotId
    );
    if (interviewer.timeSlots.length === 0) {
      interviewer.isAvailable = false;
    }
  }

  updateAvailabilityList() {
    if (this.selectedDate) {
      const existingInterviewer = this.interviewerAvailability.find(
        (i) => i.date.getTime() === this.selectedDate!.getTime()
      );

      if (existingInterviewer) {
        // Update existing time slots or add new ones
        this.timeRanges.forEach((range) => {
          const existingSlot = existingInterviewer.timeSlots.find(
            (slot) =>
              slot.startTime === range.startTime &&
              slot.endTime === range.endTime
          );
          if (!existingSlot) {
            existingInterviewer.timeSlots.push({
              id: Date.now() + existingInterviewer.timeSlots.length,
              startTime: range.startTime,
              endTime: range.endTime,
            });
          }
        });
        existingInterviewer.isAvailable = true;
      } else {
        // Add new interviewer availability
        this.interviewerAvailability.push({
          name: 'Current User', // Replace with actual user name
          date: new Date(this.selectedDate),
          timeSlots: this.timeRanges.map((range, index) => ({
            id: Date.now() + index,
            startTime: range.startTime,
            endTime: range.endTime,
          })),
          isAvailable: true,
          expanded: false,
        });
      }
    }
  }

  saveAvailability() {
    const currentDateTime = new Date();
    const selectedDateTime = this.selectedDate;

    if (!selectedDateTime) {
      alert('Please select a date before saving availability.');
      return;
    }

    if (this.timeRanges.length === 0) {
      alert('Please add at least one time slot.');
      return;
    }

    for (const range of this.timeRanges) {
      const startTimeParts = range.startTime.split(':');
      const endTimeParts = range.endTime.split(':');

      const startDateTime: any = new Date(selectedDateTime.getTime());
      startDateTime.setHours(
        Number.parseInt(startTimeParts[0], 10),
        Number.parseInt(startTimeParts[1], 10),
        0
      );

      const endDateTime: any = new Date(selectedDateTime.getTime());
      endDateTime.setHours(
        Number.parseInt(endTimeParts[0], 10),
        Number.parseInt(endTimeParts[1], 10),
        0
      );

      // Calculate the difference in milliseconds
      const diffInMilliseconds = endDateTime - startDateTime;

      // Convert milliseconds to hours
      const diffInHours = diffInMilliseconds / (1000 * 60 * 60);

      // Check if the difference is at least 1 hour
      if (diffInHours < 1) {
        alert('You have to set a time range atleast 1 hr.');
        return;
      }

      if (startDateTime < currentDateTime) {
        alert('You cannot set a time range that starts in the past.');
        return;
      }

      if (endDateTime <= startDateTime) {
        alert('End time must be later than start time.');
        return;
      }
    }

    let slotsToCheck: TimeSlot[] = [];
    if (this.editingSlot) {
      // When editing, check against all slots except the one being edited
      slotsToCheck = this.editingSlot.interviewer.timeSlots.filter(
        (_, index) => index !== this.editingSlot!.slotIndex
      );
    } else if (this.selectedDate) {
      // When adding new, check against all existing slots for that date
      const existingInterviewer = this.interviewerAvailability.find(
        (i) => i.date.getTime() === this.selectedDate!.getTime()
      );
      if (existingInterviewer) {
        slotsToCheck = existingInterviewer.timeSlots;
      }
    }

    for (const range of this.timeRanges) {
      if (this.checkOverlap(range.startTime, range.endTime, slotsToCheck)) {
        alert(
          'This time slot overlaps with an existing slot. Please adjust your availability.'
        );
        return;
      }
    }

    const timeIntervals = this.timeRanges.map((range) => ({
      start: new Date(selectedDateTime.getTime()).setHours(
        Number.parseInt(range.startTime.split(':')[0], 10),
        Number.parseInt(range.startTime.split(':')[1], 10),
        0
      ),
      end: new Date(selectedDateTime.getTime()).setHours(
        Number.parseInt(range.endTime.split(':')[0], 10),
        Number.parseInt(range.endTime.split(':')[1], 10),
        0
      ),
    }));

    for (let i = 0; i < timeIntervals.length; i++) {
      for (let j = i + 1; j < timeIntervals.length; j++) {
        if (
          timeIntervals[i].start < timeIntervals[j].end &&
          timeIntervals[i].end > timeIntervals[j].start
        ) {
          alert('Time ranges cannot overlap. Please adjust your availability.');
          return;
        }
      }
    }

    // After all validations, prepare API request for saving availability
    this.isLoading = true;
    this.apiError = "";

    console.log('Preparing availability data for saving...');
    console.log('Selected date:', selectedDateTime);
    console.log('Time ranges to save:', this.timeRanges);

    const formattedDate = selectedDateTime.toISOString().split('T')[0];
    const availabilitySlots: AvailabilitySlot[] = this.timeRanges.map(range => {
      // Create full ISO date strings for the API
      const startDateTime = new Date(`${formattedDate}T${range.startTime}:00`);
      const endDateTime = new Date(`${formattedDate}T${range.endTime}:00`);
      
      return {
        startTime: startDateTime.toISOString(),
        endTime: endDateTime.toISOString()
      };
    });

    console.log('Formatted availability slots for API:', availabilitySlots);
    console.log('Sending availability data to API...');

    this.interviewerService.createAvailability(availabilitySlots).subscribe({
      next: (response) => {
        console.log('API success response:', response);
        this.isLoading = false;
        
        // Update UI
        if (this.editingSlot) {
          // Update existing slot
          const { interviewer, slotIndex } = this.editingSlot;
          interviewer.timeSlots[slotIndex] = {
            id: interviewer.timeSlots[slotIndex].id,
            startTime: this.timeRanges[0].startTime,
            endTime: this.timeRanges[0].endTime,
          };
          this.editingSlot = null;
        } else {
          // Add new availability
          this.updateAvailabilityList();
        }

        this.selectedDate = null;
        this.timeRanges = [{ startTime: '09:00', endTime: '17:00' }];
        alert('Availability saved successfully.');
        
        // Reload availabilities from API to ensure we have the latest data
        console.log('Reloading availabilities from API...');
        setTimeout(() => {
          this.loadAvailabilities();
        }, 500);
      },
      error: (error) => {
        console.error('API error saving availability:', error);
        this.isLoading = false;
        this.apiError = "Failed to save availability";
        
        const errorMessage = error.error?.message || 'Server error occurred. Please try again.';
        console.log('Error message:', errorMessage);
        alert('Error saving availability: ' + errorMessage);
        
        // Still update UI locally for user experience
        if (this.editingSlot) {
          const { interviewer, slotIndex } = this.editingSlot;
          interviewer.timeSlots[slotIndex] = {
            id: interviewer.timeSlots[slotIndex].id,
            startTime: this.timeRanges[0].startTime,
            endTime: this.timeRanges[0].endTime,
          };
          this.editingSlot = null;
        } else {
          this.updateAvailabilityList();
        }

        this.selectedDate = null;
        this.timeRanges = [{ startTime: '09:00', endTime: '17:00' }];
      }
    });
  }

  toggleExpand(interviewer: Interviewer) {
    interviewer.expanded = !interviewer.expanded;
  }

  addMoreSlot(interviewer: Interviewer) {
    this.selectedDate = new Date(interviewer.date);
    this.timeRanges = [{ startTime: '09:00', endTime: '17:00' }];
  }

  showDeleteModal(interviewer: Interviewer, slotId: number) {
    this.selectedInterviewer = interviewer;
    this.selectedSlotId = slotId;
    this.showDeleteConfirmation = true;
  }

  confirmDelete() {
    if (this.selectedInterviewer && this.selectedSlotId !== null) {
      this.deleteTimeSlot(this.selectedInterviewer, this.selectedSlotId);
      this.closeDeleteModal();
    }
  }

  closeDeleteModal() {
    this.showDeleteConfirmation = false;
    this.selectedInterviewer = null;
    this.selectedSlotId = null;
  }

  private checkOverlap(
    newStart: string,
    newEnd: string,
    existingSlots: TimeSlot[]
  ): boolean {
    const newStartTime = new Date(`1970-01-01T${newStart}`);
    const newEndTime = new Date(`1970-01-01T${newEnd}`);

    return existingSlots.some((slot) => {
      const slotStart = new Date(`1970-01-01T${slot.startTime}`);
      const slotEnd = new Date(`1970-01-01T${slot.endTime}`);
      return newStartTime < slotEnd && newEndTime > slotStart;
    });
  }
}
