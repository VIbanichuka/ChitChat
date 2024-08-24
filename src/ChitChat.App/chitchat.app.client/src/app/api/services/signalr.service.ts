import { Injectable } from '@angular/core';
import * as signalR from "@microsoft/signalr";
import { BehaviorSubject } from 'rxjs';
import { environment } from '../../../environments/environment.development';
import { AuthService } from './auth.service';

@Injectable({
  providedIn: 'root'
})
export class SignalrService {
  chatUrl = "/chitChatHub"
  messages$ = new BehaviorSubject<any>([]);
  messages: any[] = [];
  hubConnection!: signalR.HubConnection;
  private sentMessagesCache: Map<string, string> = new Map();
  private currentUser: string | null = null;
  constructor(private authService: AuthService) {
    this.authService.getUsernameFromToken().subscribe(displayName => {
      this.currentUser = displayName;
    });
  }

  startConnection = () => {
    this.createConnection();

    this.hubConnection
      .start()
      .then(() => {
        console.log('Connection started...');
        this.receiveMessageListener();
        this.receiveGroupMessageListener();
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
    this.hubConnection.on("SendMessageAsync", async (encryptedMessage: string, sender: string, channelName: string, timestamp: string) => {
      let messageToDisplay;

      if (this.currentUser === sender) {
        messageToDisplay = this.sentMessagesCache.get(encryptedMessage);
      } else {
        try {
          messageToDisplay = this.authService.decryptMessage(encryptedMessage);
        } catch (error) {
          console.error('Error decrypting message:', error);
          return;
        }
      }

      this.messages = [...this.messages, { message: messageToDisplay, sender, channelName, timestamp }];
      this.messages$.next(this.messages);
      console.log("Received message:", messageToDisplay);
    });
  }


  receiveGroupMessageListener() {
    this.hubConnection.on("SendGroupMessageAsync", async (message: string, sender: string, channelName: string, timestamp: string) => {
      try {
        this.messages = [...this.messages, { message, sender, channelName, timestamp }];
        console.log("Received raw message:", message, sender, timestamp);
        this.messages$.next(this.messages);
        console.log("Received message:", message);
      } catch (error) {
        console.error('Error decrypting message:', error)
      }
    });
  }

  async receiveMessage(message: string) {
    await this.hubConnection.invoke("ReceiveMessageAsync", message)
      .catch(error => console.error(error));
  }

  async sendMessageToGroupAsync(channelName: string, message: string, sender: string) {
    return await this.hubConnection.invoke("SendGroupMessageAsync", channelName, message, sender)
      .then(() => console.log("successful"))
      .catch(error => console.error(error));
  }

  async sendPrivateMessageAsync(channelName: string, message: string, sender: string, friendId: string) {
    try {
      const encryptedMessage = await this.authService.encryptMessage(message, friendId);

      this.sentMessagesCache.set(encryptedMessage, message);

      return await this.hubConnection.invoke("SendMessageToGroupAsync", channelName, encryptedMessage, sender)
        .then(() => console.log('Message sent successfully', encryptedMessage))
        .catch(error => console.error('Error sending message:', error));

    } catch (error) {
      console.error('Error encrypting message:', error);
    }
  }

  async joinGroupAsync(channelName: string, displayName: string) {
    return await this.hubConnection.invoke("JoinGroupAsync", { channelName, displayName })
      .then(() => console.log('Joined group'))
      .catch(err => console.error('Error while joining group: ' + err));
  }

  async rejoinPrivateChatAsync(channelName: string, displayName: string) {
    const hubModel = { ChannelName: channelName, Sender: displayName }
    return await this.hubConnection.invoke("RejoinGroupAsync", hubModel)
      .then(() => console.log('rejoined group'))
      .catch(err => console.error('Error while joining group: ' + err));
  }


  async rejoinGroupChatAsync(channelName: string, displayName: string) {
    const hubModel = { ChannelName: channelName, Sender: displayName }
    return await this.hubConnection.invoke("RejoinGroupMessageAsync", hubModel)
      .then(() => console.log('rejoined group'))
      .catch(err => console.error('Error while joining group: ' + err));
  }

  async reconnectChannelAsync(channelName: string, displayName: string) {
    const hubModel = { ChannelName: channelName, Sender: displayName }
    return await this.hubConnection.invoke("ReconnectChannelAsync", hubModel)
      .then(() => console.log('reconnected channel'))
      .catch(err => console.error('Error while reconnecting channel: ' + err));
  }
}
