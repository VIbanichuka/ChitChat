import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ManageInvitationComponent } from './manage-invitation.component';

describe('ManageInvitationComponent', () => {
  let component: ManageInvitationComponent;
  let fixture: ComponentFixture<ManageInvitationComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ManageInvitationComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ManageInvitationComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
