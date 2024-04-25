import { Injectable } from '@angular/core';
import * as signalR from "@microsoft/signalr";
import { BehaviorSubject, Observable, Subject } from 'rxjs';
import { environment } from '../../../environments/environment.development';

@Injectable({
  providedIn: 'root'
})
export class SignalrService {
  chatUrl = "/chitChatHub"
  messages$ = new BehaviorSubject<any>([]);
  messages: any[] = [];
 hubConnection!: signalR.HubConnection;
  constructor() {
  }

 startConnection = () => {
    this.createConnection();

    this.hubConnection
      .start()
      .then(() => {
        console.log('Connection started...');
        this.receiveMessageListener();
      })
      .catch(error => console.log('Error while starting connection: ' + error));
 }

  createConnection() {
    this.hubConnection = new signalR.HubConnectionBuilder()
      .withUrl(`${environment.apiUrl}${this.chatUrl}`, {
        skipNegotiation: true,
        transport: signalR.HttpTransportType.WebSockets
      })
      .withAutomaticReconnect()
      .configureLogging(signalR.LogLevel.Information)
      .build();
  }

  receiveMessageListener() {
    this.hubConnection.on("SendMessageAsync", (message: string, sender: string, channelName: string) => {
      this.messages = [...this.messages, { message, sender, channelName }];
      console.log("Received raw message:", message, sender);
      this.messages$.next(this.messages);
      console.log("Received message:", message);
    });
  }

  async receiveMessage(message: string) {
    await this.hubConnection.invoke("ReceiveMessageAsync", message)
      .catch(error => console.error(error));
  }

  async sendMessageToGroupAsync(channelName: string, message: string, sender: string) {
    return await this.hubConnection.invoke("SendMessageToGroupAsync", channelName, message, sender)
      .then(() => console.log("successful"))
      .catch(error => console.error(error));
  }

  async JoinGroupAsync(channelName: string, displayName: string) {
    return await this.hubConnection.invoke("JoinGroupAsync", { channelName, displayName })
      .then(() => console.log('Joined group'))
      .catch(err => console.error('Error while joining group: ' + err));
  }

  async rejoinGroupAsync(channelName: string, displayName: string) {
    return await this.hubConnection.invoke("RejoinGroupAsync", { channelName, displayName })
      .then(() => console.log('rejoined group'))
      .catch(err => console.error('Error while joining group: ' + err));
  }

}
