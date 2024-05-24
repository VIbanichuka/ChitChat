import { AfterViewChecked, ChangeDetectorRef, Component, ElementRef, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { UserProfileResponseModel, UserResponseModel } from '../api/models';
import { ChannelResponseModel } from '../api/models/channel-response-model';
import { UserProfileService, UserService } from '../api/services';
import { AuthService } from '../api/services/auth.service';
import { ChannelsService } from '../api/services/channels.service';
import { SignalrService } from '../api/services/signalr.service';

@Component({
  selector: 'app-channels-chat',
  templateUrl: './channels-chat.component.html',
  styleUrls: ['./channels-chat.component.css']
})
export class ChannelsChatComponent implements OnInit, AfterViewChecked, OnDestroy {
  showChannelList: boolean = false;
  currentUserChannelList: ChannelResponseModel[] | null = null;
  channelId: string | null = null;
  usersInGroup: UserResponseModel[] | null = null;
  currentUser: string = "";
  userProfileInfo: UserProfileResponseModel | null = null;
  selectedChannel: string = "";
  inputMessage = "";
  messages: any[] = [];
  @ViewChild("scrollBar") private scrollContainer!: ElementRef;
  constructor(
    private route: ActivatedRoute,
    private userProfileService: UserProfileService,
    private authService: AuthService,
    private userService: UserService,
    private channelService: ChannelsService,
    private signalrService: SignalrService,
    private changeDetectorRef: ChangeDetectorRef) { }

  ngOnDestroy(): void {
    this.signalrService.hubConnection.off("SendMessageAsync");
  }

  ngOnInit(): void {
    this.route.paramMap.subscribe(p => {
      this.channelId = p.get("channelId");
      if (this.channelId) {
        console.log("ChannelId:", this.channelId);
        this.getChannelMembers(parseInt(this.channelId));
        this.getCurrentChannelName(parseInt(this.channelId));
      }
    });

    this.getCurrentUser();

    this.signalrService.messages$.subscribe(response => {
      console.log('Raw messages:', response);
      this.messages = response.filter((msg: any) => msg.channelName === this.selectedChannel);
      console.log('Filtered messages:', this.messages);
      this.changeDetectorRef.detectChanges();
    });

    this.getUserChannels();

    
  }

  ngAfterViewChecked(): void {
    this.scrollContainer.nativeElement.scrollTop = this.scrollContainer.nativeElement.scrollHeight;
  }

  sendMessage() {
    this.signalrService.receiveMessage(this.inputMessage)
      .then(() => {
        this.inputMessage = '';
      }).catch((error) => {
        console.error(error);
      })
  }

  sendMessageToGroup() {
    this.signalrService.sendMessageToGroupAsync(this.selectedChannel, this.inputMessage, this.currentUser)
      .then(() => {
        this.inputMessage = '';
      }).catch((error) => {
        console.error(error);
      })
  }

  getChannelMembers(channelId: number) {
    this.channelService.getChannelMembers(channelId).subscribe(users => {
      this.usersInGroup = users;
    });
  }

  getCurrentChannelName(channelId: number) {
    this.channelService.getChannelById(channelId).subscribe(name => {
      this.selectedChannel = name.channelName;
      this.signalrService.messages$.next(this.signalrService.messages$.value);
    })
  }

  getCurrentUser() {
    this.authService.getUserIdFromToken().subscribe(userId => {
      if (!userId)
        return;
      this.userService.getUserById(userId).subscribe(user => {
        this.currentUser = user.displayName;
        this.reconnectGroup();
      });
    });
  }

  reconnectGroup() {
    this.signalrService.rejoinGroupAsync(this.selectedChannel, this.currentUser)
      .then(() => {
      }).catch((error) => {
        console.error(error);
      });
  }

  toggleChannelList() {
    this.showChannelList = !this.showChannelList;
  }

  getUserChannels() {
    this.authService.getUserIdFromToken().subscribe(userId => {
      if (!userId)
        return;
      this.channelService.getChannelsUserId(userId).subscribe((channels: ChannelResponseModel[]) => {
        this.currentUserChannelList = channels;
        console.log(channels);
      })
    })
  }

  reconnectSelectedChannel(channelName: string) {
    this.selectedChannel = channelName;
    this.signalrService.reconnectChannelAsync(this.selectedChannel, this.currentUser)
      .then(() => {
      }).catch((error) => {
        console.error(error);
      });
  }

}