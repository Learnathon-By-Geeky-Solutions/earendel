import { TestBed } from '@angular/core/testing';

import { RubricService } from './rubric.service';

describe('RubricService', () => {
  let service: RubricService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(RubricService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
