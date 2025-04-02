import { TestBed } from '@angular/core/testing';

import { NotificationhubService } from './notificationhub.service';

describe('NotificationhubService', () => {
  let service: NotificationhubService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(NotificationhubService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
