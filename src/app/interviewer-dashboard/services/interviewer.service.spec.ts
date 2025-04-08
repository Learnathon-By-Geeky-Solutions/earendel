import { TestBed } from '@angular/core/testing';

import { InterviewerService } from './interviewer.service';

describe('InterviewerService', () => {
  let service: InterviewerService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(InterviewerService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
