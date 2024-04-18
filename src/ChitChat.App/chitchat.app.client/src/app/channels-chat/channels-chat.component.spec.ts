import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ChannelsChatComponent } from './channels-chat.component';

describe('ChannelsChatComponent', () => {
  let component: ChannelsChatComponent;
  let fixture: ComponentFixture<ChannelsChatComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ChannelsChatComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ChannelsChatComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
