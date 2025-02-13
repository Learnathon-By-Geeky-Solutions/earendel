import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-availability',
  standalone: true,
  imports: [CommonModule, FormsModule],
  template: `
    <div class="container-fluid">
      <h1 class="h2 mb-4">Set Your Availability</h1>
      <p class="lead mb-4">
        Select the dates and times when you're available to conduct interviews.
        This helps us match you with potential candidates.
      </p>
      <div class="row g-4">
        <div class="col-md-6">
          <div class="card">
            <div class="card-body">
              <h5 class="card-title">Select Dates</h5>
              <p class="card-text">
                Choose the dates you're available for interviews
              </p>
              <div class="bg-light p-5 text-center">Calendar Placeholder</div>
            </div>
          </div>
        </div>
        <div class="col-md-6">
          <div class="card">
            <div class="card-body">
              <h5 class="card-title">Set Availability</h5>
              <p class="card-text">
                Set your availability for the selected date
              </p>
              <div class="mb-3">
                <select class="form-select">
                  <option>Morning (9AM - 12PM)</option>
                  <option>Afternoon (1PM - 5PM)</option>
                  <option>Evening (6PM - 9PM)</option>
                </select>
              </div>
              <div class="form-check mb-3">
                <input
                  class="form-check-input"
                  type="checkbox"
                  id="availableCheck"
                />
                <label class="form-check-label" for="availableCheck">
                  Available
                </label>
              </div>
              <div class="mb-3">
                <h6>Current Availability</h6>
                <ul class="list-group">
                  <li
                    class="list-group-item d-flex justify-content-between align-items-center"
                  >
                    Morning
                    <span class="badge bg-primary rounded-pill">Available</span>
                  </li>
                  <li
                    class="list-group-item d-flex justify-content-between align-items-center"
                  >
                    Afternoon
                    <span class="badge bg-secondary rounded-pill"
                      >Unavailable</span
                    >
                  </li>
                  <li
                    class="list-group-item d-flex justify-content-between align-items-center"
                  >
                    Evening
                    <span class="badge bg-primary rounded-pill">Available</span>
                  </li>
                </ul>
              </div>
              <button class="btn btn-primary">Save Availability</button>
            </div>
          </div>
        </div>
      </div>
    </div>
  `,
  styles: [],
})
export class AvailabilityComponent {}
