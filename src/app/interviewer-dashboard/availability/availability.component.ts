import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { SidebarComponent } from '../sidebar/sidebar.component';

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
export class AvailabilityComponent {
  daysOfWeek = ['Su', 'Mo', 'Tu', 'We', 'Th', 'Fr', 'Sa'];
  currentDate = new Date();
  currentMonth = this.currentDate.getMonth();
  currentYear = this.currentDate.getFullYear();
  selectedDate: Date | null = null;
  timeRanges: { startTime: string; endTime: string }[] = [
    { startTime: '09:00', endTime: '17:00' },
  ];

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
    {
      name: 'John Doe',
      date: new Date('2025-01-26'),
      timeSlots: [
        { id: 1, startTime: '09:00', endTime: '11:00' },
        { id: 2, startTime: '14:00', endTime: '16:00' },
      ],
      isAvailable: true,
      expanded: false,
    },
    {
      name: 'Jane Smith',
      date: new Date('2025-01-27'),
      timeSlots: [
        { id: 3, startTime: '10:00', endTime: '12:00' },
        { id: 4, startTime: '15:00', endTime: '17:00' },
      ],
      isAvailable: true,
      expanded: false,
    },
  ];

  showDeleteConfirmation = false;
  selectedInterviewer: Interviewer | null = null;
  selectedSlotId: number | null = null;
  editingSlot: { interviewer: Interviewer; slotIndex: number } | null = null;

  constructor() {
    this.generateCalendarDays();
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
