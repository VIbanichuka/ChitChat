import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { ChannelResponseModel } from '../api/models/channel-response-model';
import { AuthService } from '../api/services/auth.service';
import { ChannelsService } from '../api/services/channels.service';
import { ChannelPopupComponent } from '../channel-popup/channel-popup.component';

@Component({
  selector: 'app-channels',
  templateUrl: './channels.component.html',
  styleUrls: ['./channels.component.css']
})
export class ChannelsComponent implements OnInit{
  channels: ChannelResponseModel[] | null = null;
  hoveredItemId: number | null = null;
  constructor(private matDialog: MatDialog, private channelsService: ChannelsService, private authService: AuthService){}
  ngOnInit(): void {
    this.getChannelsOfUser();
  }

  getChannelsOfUser() {
    this.authService.getUserIdFromToken().subscribe(userId => {
      if (!userId)
        return;
      this.channelsService.getChannelsUserId(userId).subscribe((channels: ChannelResponseModel[]) => {
        this.channels = channels;
        console.log(channels);
      })
    })
  }

  openChannelDialog() {   
    this.matDialog.open(ChannelPopupComponent, {
      maxWidth: '400px',
      width: '100%'
    })
  }

  toggleHoverButton(channelId: number, hovered: boolean) {
    if (hovered) {
      this.hoveredItemId = channelId;
    } else {
      this.hoveredItemId = null;
    }
  }

  leaveChannel(channelId: number) {
    this.authService.getUserIdFromToken().subscribe(userId => {
      if (!userId)
        return;
      this.channelsService.leaveChannel(channelId, userId).subscribe((response: ChannelResponseModel) => {
        console.log('success');
      },
        (error) => {
          console.error(error);
        }
      );

    });
  }

}
