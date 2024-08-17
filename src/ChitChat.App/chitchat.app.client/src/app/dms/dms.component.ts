import { AfterViewChecked, ChangeDetectorRef, Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { UserProfileResponseModel } from '../api/models';
import { UserProfileService, UserService} from '../api/services';
import { SignalrService } from '../api/services/signalr.service';
import { ActivatedRoute } from '@angular/router';
import { AuthService } from '../api/services/auth.service';

@Component({
  selector: 'app-dms',
  templateUrl: './dms.component.html',
  styleUrls: ['./dms.component.css']
})
export class DmsComponent implements OnInit, AfterViewChecked{
  currentUser: string = "";
  messages: any[] = [];
  inputMessage = "";
  groupName: string = "";
  friendProfile: UserProfileResponseModel | null = null;
  friendId: string | null = "";
  userId: string = "";
  @ViewChild("scrollBar") private scrollContainer!: ElementRef;
  constructor(private userProfileService: UserProfileService,
    private signalrService: SignalrService,
    private userService: UserService,
    private authService: AuthService,
    private route: ActivatedRoute,
    private changeDetectorRef: ChangeDetectorRef) { }

  ngOnDestroy(): void {
    this.signalrService.hubConnection.off("SendMessageAsync");
  }

  ngOnInit(): void {
    this.route.paramMap.subscribe(p => {
      this.friendId = p.get("userId");
      if (this.friendId) {
        console.log("FriendId", this.friendId);
        this.getFriendProfile(this.friendId);
      }
    });

    this.getCurrentUser();

    this.signalrService.messages$.subscribe(response => {
      console.log('Raw messages:', response);
      this.messages = response.filter((msg: any) => msg.channelName === this.groupName);
      console.log('Filtered messages:', this.messages);
      this.changeDetectorRef.detectChanges();
    });

  }

  ngAfterViewChecked(): void {
    this.scrollContainer.nativeElement.scrollTop = this.scrollContainer.nativeElement.scrollHeight;
  }


  sendPrivateMessage() {
    if (this.friendId) {
      this.signalrService.sendPrivateMessageAsync(this.groupName, this.inputMessage, this.currentUser, this.friendId)
        .then(() => {
          this.inputMessage = '';
        }).catch((error) => {
          console.error(error);
        });
    } else {
      console.error('friendId is null or undefined');
    } 
  }

  private getFriendProfile(userId: string): void {
    this.userProfileService.getUserProfileById(userId).subscribe(
      (friendProfile: UserProfileResponseModel) => {
        this.friendProfile = friendProfile;
      }
    )
  }

  getCurrentUser() {
    this.authService.getUserIdFromToken().subscribe(userId => {
      if (!userId)
        return;
      this.userService.getUserById(userId).subscribe(user => {
        this.currentUser = user.displayName;
        this.userId = user.userId;
        this.getGroupName();
        this.reconnectPrivateChat();
      });
    });
  }

  private getGroupName() {
    const privateChatId = [this.userId, this.friendId].sort().join("-");
    this.groupName = privateChatId;
    this.signalrService.messages$.next(this.signalrService.messages$.value);
  }

  reconnectSelectedPrivateChat(privateChatId: string) {
    this.groupName = privateChatId;
    this.signalrService.reconnectChannelAsync(this.groupName, this.currentUser)
      .then(() => {
      }).catch((error) => {
        console.error(error);
      });
  }

  reconnectPrivateChat() {
    this.signalrService.rejoinGroupAsync(this.groupName, this.currentUser)
      .then(() => {
      }).catch((error) => {
        console.error(error);
      });
  }
}
