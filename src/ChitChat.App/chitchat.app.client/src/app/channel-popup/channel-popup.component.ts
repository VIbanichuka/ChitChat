import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { ChannelRequestModel } from '../api/models/channel-request-model';
import { ChannelsService } from '../api/services/channels.service';
import { Router } from '@angular/router';
import { AuthService } from '../api/services/auth.service';
import { ChannelResponseModel } from '../api/models/channel-response-model';
import { SignalrService } from '../api/services/signalr.service';
import { UserResponseModel } from '../api/models';
import { UserService } from '../api/services';


@Component({
  selector: 'app-channel-popup',
  templateUrl: './channel-popup.component.html',
  styleUrls: ['./channel-popup.component.css']
})
export class ChannelPopupComponent implements OnInit {
  form: FormGroup;
  channel: ChannelResponseModel | null = null;
  userInfo: UserResponseModel | null = null;

  constructor(private channelsService: ChannelsService,
    private formbuilder: FormBuilder,
    private router: Router,
    private matDialog: MatDialog,
    private authService: AuthService,
    private signalrService: SignalrService,
    private userService: UserService,
  ) {
    this.form = this.formbuilder.group({
      channelName: ['', [Validators.maxLength(50)]]
    })
  }

    ngOnInit(): void {
      
    }

  createChannel() {
    if (this.form.invalid)
      return;
    this.channelsService.createChannel(this.form.value).subscribe(channel => {
      console.log('posted to server');
      const channelId = channel.channelId;
      if (channelId) {
        this.joinChannel(channelId);
        location.reload();
      }
    })
  }

  private joinChannel(channelId: number) {
    this.authService.getUserIdFromToken().subscribe(userId => {
      if (!userId)
        return;
      this.channelsService.joinChannel(channelId, userId).subscribe(_ => {
        console.log('success');
      });

      const channelName: string = this.form.value.channelName.toString();
      this.userService.getUserById(userId).subscribe(user => {
        const displayName = user.displayName;
        this.signalrService.joinGroupAsync(channelName, displayName)
          .then(() => {
            this.matDialog.closeAll();
            this.router.navigate(['home']);
          })
          .catch((error) => { console.error(error); });
      });
    });
  }
}
