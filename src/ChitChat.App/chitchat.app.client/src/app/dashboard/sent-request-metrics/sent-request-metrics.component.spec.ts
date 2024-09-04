import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SentRequestMetricsComponent } from './sent-request-metrics.component';

describe('SentRequestMetricsComponent', () => {
  let component: SentRequestMetricsComponent;
  let fixture: ComponentFixture<SentRequestMetricsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ SentRequestMetricsComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(SentRequestMetricsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
