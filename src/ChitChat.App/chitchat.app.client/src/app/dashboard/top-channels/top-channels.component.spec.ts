import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TopChannelsComponent } from './top-channels.component';

describe('TopChannelsComponent', () => {
  let component: TopChannelsComponent;
  let fixture: ComponentFixture<TopChannelsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ TopChannelsComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(TopChannelsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
