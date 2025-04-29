import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { SidebarComponent } from '../sidebar/sidebar.component';
import { InterviewerService, AvailabilitySlot } from '../../shared/services/interviewer.service';

interface TimeSlot {
  id: number; // Unique ID for UI tracking
  apiId?: number | string; // Optional: Store the original API ID
  startTime: string; // Formatted time for display (e.g., 9:00 AM BST)
  endTime: string; // Formatted time for display (e.g., 5:00 PM BST)
  startTimeUTC: string; // Store original UTC string from API
  endTimeUTC: string; // Store original UTC string from API
}

interface Interviewer {
  name: string;
  date: Date; // Represents the specific date (UTC midnight)
  dateString: string; // Store YYYY-MM-DD date string derived from UTC
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
  selectedDate: Date | null = null; // Stores selected date as midnight UTC
  timeRanges: { startTime: string; endTime: string }[] = [ // Input model uses HH:mm
    { startTime: '09:00', endTime: '17:00' },
  ];

  isLoading = false;
  apiError = "";

  // Toast notification properties
  showToast = false;
  toastMessage = '';
  toastType = 'success'; // 'success', 'error', 'info'
  toastTimeout: any = null;

  monthNames = [
    'January', 'February', 'March', 'April', 'May', 'June', 'July',
    'August', 'September', 'October', 'November', 'December',
  ];

  calendarDays: {
    date: Date; // Represents UTC midnight for the calendar day
    dayNumber: number;
    isCurrentMonth: boolean;
    isDisabled: boolean;
  }[][] = [];

  // Holds the processed availability data grouped by date for display
  interviewerAvailability: Interviewer[] = [];

  showDeleteConfirmation = false;
  selectedInterviewer: Interviewer | null = null;
  selectedSlotId: number | null = null; // Uses the internal unique 'id'
  editingSlot: { interviewer: Interviewer; slotIndex: number } | null = null; // Tracks which slot is being edited

  constructor(private interviewerService: InterviewerService) {
    this.generateCalendarDays();
  }

  ngOnInit(): void {
    // Initial logging for debugging session/user data
    const loggedInUser = sessionStorage.getItem('loggedInUser');
    const userData = sessionStorage.getItem('userData');
    if (loggedInUser) { try { const p = JSON.parse(loggedInUser); console.log('Init: loggedInUser OK', p?.userId); } catch(e) { console.error('Init: Error parsing loggedInUser'); } }
    if (userData) { try { const p = JSON.parse(userData); console.log('Init: userData OK', p?.userId); } catch(e) { console.error('Init: Error parsing userData'); } }
    console.log('================================');
    this.loadAvailabilities(); // Load data when component initializes
  }

  // Fetches availability data from the backend
  loadAvailabilities(): void {
    this.isLoading = true;
    this.apiError = "";
    console.log('Loading interviewer availabilities from API...');
    this.interviewerService.searchAvailabilities().subscribe({
      next: (response) => {
        console.log('API response received:', response);
        this.isLoading = false;
        let itemsToProcess: any[] = [];

        // Handle different possible response structures robustly
        if (response && response.items && Array.isArray(response.items)) {
          itemsToProcess = response.items;
          console.log('Found', itemsToProcess.length, 'availability records in "items"');
        } else if (response && response.data && Array.isArray(response.data)) {
          itemsToProcess = response.data;
          console.log('Found', itemsToProcess.length, 'availability records in "data"');
        } else if (Array.isArray(response)) {
          itemsToProcess = response;
          console.log('Found', itemsToProcess.length, 'availability records in direct array');
        } else {
          console.warn('Unexpected API response format or empty response:', response);
          this.apiError = "Could not load availabilities: unexpected server response.";
          this.interviewerAvailability = []; // Clear data on error/bad format
          return;
        }

        if (itemsToProcess.length > 0) {
          this.processAvailabilityData(itemsToProcess);
        } else {
          console.log('No availability data returned from API');
          this.interviewerAvailability = []; // Clear data if API returns empty list
        }
      },
      error: (error) => {
        console.error('API error loading availabilities:', error);
        this.isLoading = false;
        this.apiError = "Error loading availabilities from server. Please try again later.";
        this.interviewerAvailability = []; // Clear data on error
      }
    });
  }

  // Processes the raw data from the API into the structured format for display
  processAvailabilityData(data: any[]): void {
    console.log('Processing availability data...');
    if (!data || data.length === 0) {
      console.log('No data received to process');
      this.interviewerAvailability = [];
      return;
    }

    let userName = 'Current User';
    try {
      const userData = JSON.parse(sessionStorage.getItem('loggedInUser') || sessionStorage.getItem('userData') || '{}');
      userName = userData.name || userData.userName || 'Current User';
    } catch (e) { console.error('Could not parse user data during processing', e); }

    // Group by UTC Date String (YYYY-MM-DD) to avoid timezone issues
    const groupedByUTCDate: { [key: string]: TimeSlot[] } = {};

    data.forEach((item, index) => {
      console.log(`Processing item ${index}:`, item);
      if (!item) { console.log('Skipping null/undefined item'); return; }

      let startTimeISO: string | undefined;
      let endTimeISO: string | undefined;
      let apiId: number | string | undefined = item.id || item.interviewerAvailabilityId || undefined; // Adapt based on actual API ID field name

      // Improved API ID extraction - check all possible locations based on API response structure
      if (!apiId) {
        // Check for nested ID if the primary ID wasn't found
        apiId = item.availabilitySlot?.id || item.availabilitySlot?.interviewerAvailabilityId || undefined;
        
        if (!apiId) {
          console.warn('Could not extract valid ID from availability item:', item);
          // Continue anyway, as we might still want to display the time slot even if we can't delete it
        } else {
          console.log(`Found API ID (${apiId}) in nested availabilitySlot object`);
        }
      } else {
        console.log(`Found API ID (${apiId}) in root availability item`);
      }

      // Extract ISO UTC time strings - Make robust based on confirmed API response structure
      if (item.startTime && item.endTime && typeof item.startTime === 'string' && typeof item.endTime === 'string') {
        startTimeISO = item.startTime; endTimeISO = item.endTime;
      } else if (item.availabilitySlot?.startTime && item.availabilitySlot?.endTime) {
        startTimeISO = item.availabilitySlot.startTime; endTimeISO = item.availabilitySlot.endTime;
      }
      // Add more 'else if' blocks if other structures exist

      if (!startTimeISO || !endTimeISO) {
        console.warn('Could not extract valid startTime/endTime ISO strings from item:', item);
        return; // Skip item if essential data is missing
      }

      // --- Date Handling ---
      let startDate: Date;
      let endDate: Date;
      try {
        startDate = new Date(startTimeISO);
        endDate = new Date(endTimeISO);
        if (isNaN(startDate.getTime()) || isNaN(endDate.getTime())) throw new Error("Invalid date parsed");
      } catch (e) {
        console.warn('Invalid date format in API item, cannot parse:', item, e);
        return; // Skip item if dates are invalid
      }

      // --- Grouping Key: Use UTC Date (YYYY-MM-DD) ---
      const dateKey = startDate.toISOString().split('T')[0];
      console.log('Grouping Date key (UTC):', dateKey);

      if (!groupedByUTCDate[dateKey]) {
        groupedByUTCDate[dateKey] = [];
      }

      // Add to group, storing both formatted and original times
      groupedByUTCDate[dateKey].push({
        id: Math.random(), // Generate unique ID for internal UI list keying
        apiId: apiId,
        startTime: this.formatTime(startDate), // Format for BST display
        endTime: this.formatTime(endDate),     // Format for BST display
        startTimeUTC: startTimeISO,           // Keep original UTC string
        endTimeUTC: endTimeISO,             // Keep original UTC string
      });
    });

    console.log('Grouped data by UTC date:', groupedByUTCDate);

    // Create Interviewer objects for the UI
    const newAvailability: Interviewer[] = Object.keys(groupedByUTCDate).map(dateKey => {
      const dateObj = new Date(dateKey + 'T00:00:00Z'); // Represents midnight UTC for that date
      return {
        name: userName,
        date: dateObj,
        dateString: dateKey,
        timeSlots: groupedByUTCDate[dateKey], // Add the grouped slots
        isAvailable: true,
        expanded: false
      };
    }).sort((a, b) => a.date.getTime() - b.date.getTime()); // Sort entries by date

    console.log('Processed availability for display:', newAvailability);
    this.interviewerAvailability = newAvailability; // Update the component's data source
  }

  // Formats a Date object into Bangladesh Time (BST), 12-hour AM/PM format
  formatTime(date: Date): string {
    if (!(date instanceof Date) || isNaN(date.getTime())) {
      return "Invalid Time";
    }
    try {
      return date.toLocaleTimeString('en-US', { // Use en-US locale for structure, override TZ
        timeZone: 'Asia/Dhaka', // Explicitly Bangladesh Time
        hour: 'numeric',        // e.g., 9
        minute: '2-digit',      // e.g., 05
        hour12: true            // Use AM/PM
      });
    } catch (e) {
      console.error("Error formatting time to BST, falling back.", e);
      // Fallback to local 24h HH:MM if Intl fails
      const hours = date.getHours().toString().padStart(2, '0');
      const minutes = date.getMinutes().toString().padStart(2, '0');
      return `${hours}:${minutes}`;
    }
  }

  // Generates the calendar grid structure for the current month/year
  generateCalendarDays() {
    // Use UTC methods for consistency in date calculations
    const firstDayOfMonth = new Date(Date.UTC(this.currentYear, this.currentMonth, 1));
    const daysInMonth = new Date(Date.UTC(this.currentYear, this.currentMonth + 1, 0)).getUTCDate();
    const startDayOfWeek = firstDayOfMonth.getUTCDay(); // 0 = Sunday, ..., 6 = Saturday

    const today = new Date(); // Use local 'today' for disabling past dates comparison
    today.setHours(0, 0, 0, 0);

    this.calendarDays = [];
    let dayCounter = 1;
    for (let weekIndex = 0; weekIndex < 6; weekIndex++) { // Display up to 6 weeks
      let week: { date: Date; dayNumber: number; isCurrentMonth: boolean; isDisabled: boolean; }[] = [];
      for (let dayIndex = 0; dayIndex < 7; dayIndex++) {
        if (weekIndex === 0 && dayIndex < startDayOfWeek) {
          // Days from the previous month (padding)
           const prevMonthDay = new Date(Date.UTC(this.currentYear, this.currentMonth, 1 - (startDayOfWeek - dayIndex)));
           week.push({ date: prevMonthDay, dayNumber: prevMonthDay.getUTCDate(), isCurrentMonth: false, isDisabled: true });
        } else if (dayCounter <= daysInMonth) {
          // Days of the current month
          const dateUTC = new Date(Date.UTC(this.currentYear, this.currentMonth, dayCounter));
          // Disable past dates comparing UTC date with local 'today' start
           const isDisabled = dateUTC < today && dateUTC.toDateString() !== today.toDateString();
          week.push({ date: dateUTC, dayNumber: dayCounter, isCurrentMonth: true, isDisabled });
          dayCounter++;
        } else {
          // Days from the next month (padding)
           const nextMonthDay = new Date(Date.UTC(this.currentYear, this.currentMonth, dayCounter));
           week.push({ date: nextMonthDay, dayNumber: nextMonthDay.getUTCDate(), isCurrentMonth: false, isDisabled: true });
           dayCounter++;
        }
      }
      this.calendarDays.push(week);
      // Optimization: Stop generating weeks if we've passed the end of the month and displayed enough rows
      if (dayCounter > daysInMonth && weekIndex >= 4) break;
    }
  }

  // Navigate to the previous month in the calendar
  previousMonth() {
    this.currentMonth--;
    if (this.currentMonth < 0) {
      this.currentMonth = 11;
      this.currentYear--;
    }
    this.generateCalendarDays();
  }

  // Navigate to the next month in the calendar
  nextMonth() {
    this.currentMonth++;
    if (this.currentMonth > 11) {
      this.currentMonth = 0;
      this.currentYear++;
    }
    this.generateCalendarDays();
  }

  // Checks if a given calendar date is the currently selected date
  isDateSelected(calendarDate: Date): boolean {
    if (!this.selectedDate || !calendarDate) return false;
    // Compare based on the UTC date string (YYYY-MM-DD)
    return calendarDate.toISOString().split('T')[0] === this.selectedDate.toISOString().split('T')[0];
  }

  // Toggles the selection state of a calendar date
  toggleDate(calendarDate: Date): void {
     // Create a new Date object representing midnight UTC of the clicked date
     const clickedDateUTC = new Date(Date.UTC(calendarDate.getUTCFullYear(), calendarDate.getUTCMonth(), calendarDate.getUTCDate()));

     if (this.selectedDate && this.selectedDate.getTime() === clickedDateUTC.getTime()) {
       this.selectedDate = null; // Deselect if clicking the same date
     } else {
       this.selectedDate = clickedDateUTC; // Select the date (store as midnight UTC)
       this.editingSlot = null; // Clear editing state when selecting a new date
       this.timeRanges = [{ startTime: '09:00', endTime: '17:00' }]; // Reset time ranges
     }
   }

  // Adds a new empty time range input row
  addMoreTimeRange() {
    // Only allow adding more if not currently editing a specific slot
    if (this.editingSlot) {
        alert("You can only edit one time slot at a time. Save or cancel the current edit to add new ranges.");
        return;
    }
    this.timeRanges.push({ startTime: '09:00', endTime: '17:00' });
  }

  // Removes a time range input row
  removeTimeRange(index: number) {
    if (this.editingSlot) {
        alert("You cannot remove time ranges while editing a specific slot.");
        return;
    }
    if (this.timeRanges.length > 1) {
      this.timeRanges.splice(index, 1);
    } else {
      alert("You must have at least one time range specified.");
    }
  }

  // Gets the current local time as HH:MM for potential use in <input type="time"> min attribute
  getMinTime() {
    const now = new Date();
    const hours = now.getHours().toString().padStart(2, '0');
    const minutes = now.getMinutes().toString().padStart(2, '0');
    return `${hours}:${minutes}`;
  }

  // Prepares the form to edit an existing time slot
  editTimeSlot(interviewer: Interviewer, slotId: number) {
    const slotIndex = interviewer.timeSlots.findIndex((s) => s.id === slotId);

    if (slotIndex !== -1) {
      const slotToEdit = interviewer.timeSlots[slotIndex];

      // Set the selected date to the interviewer's date (which is stored as UTC midnight)
      this.selectedDate = new Date(interviewer.date);

      try {
        // Parse the stored UTC strings back to Date objects
        const startDate = new Date(slotToEdit.startTimeUTC);
        const endDate = new Date(slotToEdit.endTimeUTC);

        // Format back to local HH:MM for the input fields
        // This uses the browser's local timezone interpretation of the UTC time
        const startHHMM = startDate.getHours().toString().padStart(2, '0') + ':' + startDate.getMinutes().toString().padStart(2, '0');
        const endHHMM = endDate.getHours().toString().padStart(2, '0') + ':' + endDate.getMinutes().toString().padStart(2, '0');

        // Populate the timeRanges array (only one when editing)
        this.timeRanges = [{
          startTime: startHHMM,
          endTime: endHHMM,
        }];

        // Set the editing state, referencing the interviewer and the specific slot index
        this.editingSlot = { interviewer, slotIndex };

        console.log(`Editing slot UI ID ${slotId} (API ID: ${slotToEdit.apiId}). Start: ${startHHMM}, End: ${endHHMM}. Original UTC: ${slotToEdit.startTimeUTC}`);
        window.scrollTo({ top: 0, behavior: 'smooth' }); // Scroll to top form

      } catch (e) {
        console.error("Error parsing date for editing:", e);
        alert("Could not prepare the slot for editing due to a date parsing error.");
        this.editingSlot = null;
        this.selectedDate = null;
        this.timeRanges = [{ startTime: '09:00', endTime: '17:00' }]; // Reset form on error
      }
    } else {
      console.error(`Slot with UI ID ${slotId} not found for editing.`);
      alert("Could not find the selected slot for editing.");
    }
  }


  // Validates and saves the new or edited availability slots via API
  saveAvailability() {
    if (!this.selectedDate) {
      alert('Please select a date from the calendar first.');
      return;
    }
    if (this.timeRanges.length === 0 || !this.timeRanges[0].startTime || !this.timeRanges[0].endTime) {
      alert('Please ensure at least one valid time range (HH:MM format) is entered.');
      return;
    }

    // --- Validation Stage ---
    const now = new Date(); // Current local time
    // Create a local date object corresponding to the start of the selected UTC day
    const selectedDateStartOfDayLocal = new Date(this.selectedDate.getUTCFullYear(), this.selectedDate.getUTCMonth(), this.selectedDate.getUTCDate());

    for (const range of this.timeRanges) {
        // Validate HH:MM format
        if (!range.startTime || !range.endTime || !/^\d{2}:\d{2}$/.test(range.startTime) || !/^\d{2}:\d{2}$/.test(range.endTime)) {
            alert(`Invalid time format: ${range.startTime}-${range.endTime}. Please use HH:MM (24-hour) format.`);
            return;
        }

        const startParts = range.startTime.split(':');
        const endParts = range.endTime.split(':');
        const startHour = parseInt(startParts[0], 10), startMinute = parseInt(startParts[1], 10);
        const endHour = parseInt(endParts[0], 10), endMinute = parseInt(endParts[1], 10);

        // Validate numeric values and range (00:00 to 23:59)
        if (isNaN(startHour) || isNaN(startMinute) || isNaN(endHour) || isNaN(endMinute) ||
            startHour < 0 || startHour > 23 || startMinute < 0 || startMinute > 59 ||
            endHour < 0 || endHour > 23 || endMinute < 0 || endMinute > 59) {
            alert(`Invalid time value: ${range.startTime}-${range.endTime}. Hours 0-23, Minutes 0-59.`);
            return;
        }

        // Create Date objects in LOCAL time for comparison checks
        const startDateTimeLocal = new Date(selectedDateStartOfDayLocal);
        startDateTimeLocal.setHours(startHour, startMinute, 0, 0);
        const endDateTimeLocal = new Date(selectedDateStartOfDayLocal);
        endDateTimeLocal.setHours(endHour, endMinute, 0, 0);

        // Check: End time must be after Start time
        if (endDateTimeLocal <= startDateTimeLocal) {
            alert(`End time (${range.endTime}) must be strictly later than start time (${range.startTime}).`);
            return;
        }

        // Check: Start time not in the past (relative to current local time)
        if (startDateTimeLocal < now && startDateTimeLocal.toDateString() === now.toDateString()) {
             // If it's today, check if the time itself is in the past/present
             if (startDateTimeLocal.getHours() < now.getHours() || (startDateTimeLocal.getHours() === now.getHours() && startDateTimeLocal.getMinutes() <= now.getMinutes())) {
                 alert(`Cannot set start time (${range.startTime}) in the past for today.`);
                 return;
             }
        } else if (startDateTimeLocal < now) {
            // If it's a date before today (should be prevented by calendar, but double-check)
             alert(`Cannot set availability for a past date.`);
            return;
        }

        // Check: Minimum duration (e.g., 1 hour)
        const diffInMinutes = (endDateTimeLocal.getTime() - startDateTimeLocal.getTime()) / (1000 * 60);
        if (diffInMinutes < 60) { // Example: Require at least 60 minutes
            alert(`Time range ${range.startTime} - ${range.endTime} must be at least 1 hour long.`);
            return;
        }
    }

    // --- Overlap Check 1: Within the new ranges being added/edited ---
    if (this.timeRanges.length > 1) { // No need to check overlap if only one range
      const timeIntervals = this.timeRanges.map(range => {
          const start = parseInt(range.startTime.split(':')[0], 10) * 60 + parseInt(range.startTime.split(':')[1], 10);
          const end = parseInt(range.endTime.split(':')[0], 10) * 60 + parseInt(range.endTime.split(':')[1], 10);
          return { start, end }; // Minutes from midnight
      }).sort((a, b) => a.start - b.start); // Sort by start time

      for (let i = 0; i < timeIntervals.length - 1; i++) {
          if (timeIntervals[i].end > timeIntervals[i + 1].start) { // Check if end overlaps next start
              alert('Time ranges within your new selection cannot overlap. Please adjust.');
              return;
          }
      }
    }

    // --- Overlap Check 2: Against existing slots for the same date ---
    const selectedDateString = this.selectedDate.toISOString().split('T')[0];
    const existingInterviewerData = this.interviewerAvailability.find(i => i.dateString === selectedDateString);
    let existingSlotsOnDate: TimeSlot[] = existingInterviewerData ? existingInterviewerData.timeSlots : [];

    // If editing, exclude the slot being edited from the overlap check list
    if (this.editingSlot) {
        const slotBeingEditedId = this.editingSlot.interviewer.timeSlots[this.editingSlot.slotIndex].id;
        existingSlotsOnDate = existingSlotsOnDate.filter(slot => slot.id !== slotBeingEditedId);
        console.log("Excluding slot being edited (UI ID:", slotBeingEditedId, ") from overlap check.");
    }

    // Check each new/edited time range against the relevant existing slots
    for (const newTimeRange of this.timeRanges) {
        if (this.checkOverlap(newTimeRange.startTime, newTimeRange.endTime, existingSlotsOnDate)) {
            alert(`Time slot ${newTimeRange.startTime}-${newTimeRange.endTime} overlaps with another existing availability for this date. Please adjust.`);
            return; // Overlap found
        }
    }

    // --- All Validations Passed: Prepare API Payload ---
    this.isLoading = true;
    this.apiError = "";
    console.log('Validation passed. Preparing availability data for saving...');

    const availabilitySlots: AvailabilitySlot[] = this.timeRanges.map(range => {

             // Get year, month, day from the selected UTC date
             const year = this.selectedDate!.getUTCFullYear();
             const month = this.selectedDate!.getUTCMonth(); // 0-indexed (May is 4)
             const day = this.selectedDate!.getDate();
            
             console.log(year, month, day)
               // Get hours and minutes from the HH:MM input
               const startHour = parseInt(range.startTime.split(':')[0], 10);
               const startMinute = parseInt(range.startTime.split(':')[1], 10);
               const endHour = parseInt(range.endTime.split(':')[0], 10);
               const endMinute = parseInt(range.endTime.split(':')[1], 10);

                       // Create Date objects interpreting the Y/M/D + H:M in the BROWSER'S LOCAL TIMEZONE
        const startDateTimeLocal = new Date(year, month, day, startHour, startMinute, 0, 0);
        const endDateTimeLocal = new Date(year, month, day, endHour, endMinute, 0, 0);

  
         // Now convert these specific moments in local time to their UTC ISO string representation
         return {
          startTime: startDateTimeLocal.toISOString(),
          endTime: endDateTimeLocal.toISOString()
      };
    });

    console.log('Formatted availability slots for API:', availabilitySlots);

    // Determine if this is a create or update operation (if your API supports update)
    // For now, assuming the create API handles potential overwrites or adds new slots.
    // If you have a separate update API, you'd call it here when `this.editingSlot` is set.

    console.log('Calling createAvailability API...');
    this.interviewerService.createAvailability(availabilitySlots).subscribe({
        next: (response) => {
            console.log('API save success:', response);
            this.isLoading = false;
            this.showToastNotification('Availability saved successfully!', 'success');

            // Reset form state
            this.selectedDate = null;
            this.timeRanges = [{ startTime: '09:00', endTime: '17:00' }];
            this.editingSlot = null;

            // Reload from API to get the definitive state including any new IDs
            console.log('Reloading availabilities from API after save...');
            setTimeout(() => { this.loadAvailabilities(); }, 300); // Small delay if needed
        },
        error: (error) => {
            console.error('API error saving availability:', error);
            this.isLoading = false;
            const errorMessage = error.error?.message || error.message || 'Server error occurred.';
            this.apiError = `Failed to save availability: ${errorMessage}`;
            this.showToastNotification(`Error saving availability: ${errorMessage}`, 'error');
            // Optionally, keep the form state on error for retry?
            // this.editingSlot = null; // Maybe keep editing state if save fails? Depends on UX desired.
        }
    });
  }

  // Toggles the expanded/collapsed state of an interviewer's daily schedule
  toggleExpand(interviewer: Interviewer) {
    interviewer.expanded = !interviewer.expanded;
  }

  // Sets the selected date and resets the form to add a new slot for an existing date
  addMoreSlot(interviewer: Interviewer) {
    this.selectedDate = new Date(interviewer.date); // Use the stored UTC date
    this.timeRanges = [{ startTime: '09:00', endTime: '17:00' }]; // Reset input form
    this.editingSlot = null; // Ensure we are not in edit mode
    window.scrollTo({ top: 0, behavior: 'smooth' }); // Scroll to form
  }


  // --- Delete Functionality ---
  // Shows the confirmation modal for deleting a slot
  showDeleteModal(interviewer: Interviewer, slotId: number) {
    const slot = interviewer.timeSlots.find(s => s.id === slotId);
    if (slot) {
        console.log(`Attempting to delete slot UI ID: ${slot.id}, API ID: ${slot.apiId}`);
        this.selectedInterviewer = interviewer;
        this.selectedSlotId = slotId; // Use internal UI ID for modal state
        this.showDeleteConfirmation = true;
    } else {
        console.error("Slot not found for deletion:", slotId);
    }
  }

  // Handles the confirmation of the delete action
  confirmDelete() {
    if (this.selectedInterviewer && this.selectedSlotId !== null) {
        const slotToDelete = this.selectedInterviewer.timeSlots.find(s => s.id === this.selectedSlotId);
        
        if (!slotToDelete) {
            console.error("Cannot delete: Slot not found");
            this.showToastNotification("Cannot delete this slot: Slot not found in local data.", "error");
            this.closeDeleteModal();
            return;
        }
        
        if (!slotToDelete.apiId) {
            console.error("Cannot delete: Slot has no API ID", slotToDelete);
            this.showToastNotification("Cannot delete this slot: No server ID available.", "error");
            this.closeDeleteModal();
            return;
        }

        this.isLoading = true;
        console.log(`Deleting availability slot with:
        - UI ID: ${this.selectedSlotId}
        - API ID: ${slotToDelete.apiId}
        - Time Range: ${slotToDelete.startTime} - ${slotToDelete.endTime}
        - UTC Time: ${slotToDelete.startTimeUTC} - ${slotToDelete.endTimeUTC}
        `);
        
        this.interviewerService.deleteAvailability(slotToDelete.apiId).subscribe({
            next: (response) => {
                console.log('Slot deleted successfully via API.', response);
                this.isLoading = false;
                this.showToastNotification("Availability time slot deleted successfully!", "success");
                
                // Update UI after successful API deletion
                this.performLocalDelete(this.selectedInterviewer!, this.selectedSlotId!);
                this.closeDeleteModal();
                
                // Optionally reload from API to ensure UI matches server state
                // setTimeout(() => { this.loadAvailabilities(); }, 300);
            },
            error: (err) => {
                console.error('API error deleting slot:', err);
                this.isLoading = false;
                const errorMessage = err.error?.message || err.message || 'Server error occurred.';
                this.showToastNotification(`Failed to delete time slot: ${errorMessage}`, "error");
                this.closeDeleteModal();
            }
        });
    }
  }

  // Helper function to remove slot from local data (call this after successful API delete)
  private performLocalDelete(interviewer: Interviewer, slotId: number): void {
      interviewer.timeSlots = interviewer.timeSlots.filter(s => s.id !== slotId);
      if (interviewer.timeSlots.length === 0) {
          // If last slot for that day is removed, remove the whole day entry from UI
          this.interviewerAvailability = this.interviewerAvailability.filter(
              i => i.dateString !== interviewer.dateString
          );
      }
  }


  // Closes the delete confirmation modal
  closeDeleteModal() {
    this.showDeleteConfirmation = false;
    this.selectedInterviewer = null;
    this.selectedSlotId = null;
  }

  // Checks if a new time range (HH:MM) overlaps with any existing slots (using their UTC times)
  private checkOverlap(newStartHHMM: string, newEndHHMM: string, existingSlots: TimeSlot[]): boolean {
    try {
        // Convert the new HH:MM range into minutes since midnight (interpreted on the selected UTC day)
        const newStartMinutes = parseInt(newStartHHMM.split(':')[0], 10) * 60 + parseInt(newStartHHMM.split(':')[1], 10);
        const newEndMinutes = parseInt(newEndHHMM.split(':')[0], 10) * 60 + parseInt(newEndHHMM.split(':')[1], 10);

        if (isNaN(newStartMinutes) || isNaN(newEndMinutes) || newStartMinutes >= newEndMinutes) {
            console.error("Invalid new time range format passed to overlap check:", newStartHHMM, newEndHHMM);
            return true; // Treat invalid input as potential overlap to prevent issues
        }

        // Check against each existing slot
        return existingSlots.some(slot => {
            let existingStartDate: Date, existingEndDate: Date;
            try {
                // Parse the existing slot's stored UTC start and end times
                existingStartDate = new Date(slot.startTimeUTC);
                existingEndDate = new Date(slot.endTimeUTC);
                if (isNaN(existingStartDate.getTime()) || isNaN(existingEndDate.getTime())) {
                    throw new Error("Invalid date parsed from existing slot UTC string");
                }
            } catch (parseError) {
                 console.warn("Could not parse date from existing slot during overlap check:", slot, parseError);
                 return false; // Cannot compare, assume no overlap with this invalid slot
            }

            // Get minutes since midnight *UTC* for the existing slot's boundaries
            const slotStartMinutesUTC = existingStartDate.getUTCHours() * 60 + existingStartDate.getUTCMinutes();
            const slotEndMinutesUTC = existingEndDate.getUTCHours() * 60 + existingEndDate.getUTCMinutes();

            // Core overlap condition: (StartA < EndB) and (EndA > StartB)
            const overlaps = newStartMinutes < slotEndMinutesUTC && newEndMinutes > slotStartMinutesUTC;

            if (overlaps) {
                console.log(`Overlap detected: New ${newStartHHMM}-${newEndHHMM} (mins ${newStartMinutes}-${newEndMinutes}) vs Existing UTC ${slot.startTimeUTC}-${slot.endTimeUTC} (UTC mins ${slotStartMinutesUTC}-${slotEndMinutesUTC})`);
            }
            return overlaps;
        });
    } catch (e) {
        console.error("Error during overlap check execution:", e);
        // Fail safe: Assume overlap if any unexpected error occurs during the check
        return true;
    }
  }

  // Display a toast notification
  showToastNotification(message: string, type: 'success' | 'error' | 'info' = 'success') {
    this.toastMessage = message;
    this.toastType = type;
    this.showToast = true;
    
    // Auto hide after 3 seconds
    if (this.toastTimeout) {
      clearTimeout(this.toastTimeout);
    }
    
    this.toastTimeout = setTimeout(() => {
      this.showToast = false;
    }, 3000);
  }
  
  // Hide toast manually
  hideToast() {
    this.showToast = false;
    if (this.toastTimeout) {
      clearTimeout(this.toastTimeout);
    }
  }

} // End of AvailabilityComponent class