import { TestBed } from '@angular/core/testing';

import { UserprofileserviceService } from './userprofile.service';

describe('UserprofileserviceService', () => {
  let service: UserprofileserviceService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(UserprofileserviceService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
