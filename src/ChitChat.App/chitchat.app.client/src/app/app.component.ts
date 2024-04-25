import { Component, OnDestroy, OnInit } from '@angular/core';
import { SignalrService } from './api/services/signalr.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit, OnDestroy {

  constructor(private signalrService: SignalrService) { }

  ngOnDestroy(): void {
    this.signalrService.hubConnection.off("SendMessageAsync");
  }

  ngOnInit(): void {
    this.signalrService.startConnection();

  }
  title = 'chitchat.app.client';
}
