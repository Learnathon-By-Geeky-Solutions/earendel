import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SidebarComponent } from '../sidebar/sidebar.component';
import { InterviewerService } from '../services/interviewer.service';

interface Notification {
  id: string;
  message: string;
  title: string;
  isRead: boolean;
  createdOn: string;
  userId: string;
}

@Component({
  selector: 'app-notification',
  standalone: true,
  imports: [CommonModule, SidebarComponent],
  templateUrl: './notification.component.html',
  styleUrl: './notification.component.css'
})
export class NotificationComponent implements OnInit {
  notifications: Notification[] = [];
  isLoading: boolean = false;
  error: string | null = null;
  
  constructor(private interviewerService: InterviewerService) {}
  
  ngOnInit(): void {
    this.loadNotifications();
  }
  
  loadNotifications(): void {
    this.isLoading = true;
    this.error = null;
    
    this.interviewerService.getNotifications().subscribe({
      next: (response) => {
        this.isLoading = false;
        console.log('Notifications API response:', response);
        
        // Process the response based on API structure
        if (response && response.items && Array.isArray(response.items)) {
          this.notifications = response.items;
        } else if (response && response.data && Array.isArray(response.data)) {
          this.notifications = response.data;
        } else if (Array.isArray(response)) {
          this.notifications = response;
        } else {
          console.warn('Unexpected API response format:', response);
          this.error = 'Could not load notifications: unexpected response format';
          this.notifications = [];
        }
        
        if (this.notifications.length === 0) {
          console.log('No notifications found');
        }
      },
      error: (err) => {
        this.isLoading = false;
        console.error('Error fetching notifications:', err);
        this.error = 'Failed to load notifications. Please try again later.';
        this.notifications = [];
      }
    });
  }
  
  // Format the timestamp for display
  formatDate(dateString: string): string {
    try {
      const date = new Date(dateString);
      if (isNaN(date.getTime())) return 'Invalid date';
      
      // Check if it's today
      const today = new Date();
      if (date.toDateString() === today.toDateString()) {
        return `Today at ${date.toLocaleTimeString([], { hour: '2-digit', minute: '2-digit' })}`;
      }
      
      // Check if it's yesterday
      const yesterday = new Date();
      yesterday.setDate(yesterday.getDate() - 1);
      if (date.toDateString() === yesterday.toDateString()) {
        return `Yesterday at ${date.toLocaleTimeString([], { hour: '2-digit', minute: '2-digit' })}`;
      }
      
      // Otherwise, show full date
      return date.toLocaleDateString([], { 
        year: 'numeric', 
        month: 'short', 
        day: 'numeric',
        hour: '2-digit',
        minute: '2-digit'
      });
    } catch (e) {
      console.error('Error formatting date:', e);
      return dateString || 'Unknown date';
    }
  }
}
