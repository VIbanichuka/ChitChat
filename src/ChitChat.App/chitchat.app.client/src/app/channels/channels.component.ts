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
  searchQuery: string = '';
  filteredChannels: ChannelResponseModel[] | null = null;
  hoveredItemId: number | null = null;
  overlayOpen: boolean = false;
  
  constructor(private matDialog: MatDialog, private channelsService: ChannelsService, private authService: AuthService){}
  ngOnInit(): void {
    this.getChannelsOfUser();
  }
  
  closeOverlay() {
    this.overlayOpen = false;
  }

  getChannelsOfUser() {
    this.authService.getUserIdFromToken().subscribe(userId => {
      if (!userId)
        return;
      this.channelsService.getChannelsUserId(userId).subscribe((channels: ChannelResponseModel[]) => {
        this.channels = channels;
        this.filteredChannels = channels;
        console.log(channels);
      })
    })
  }

  searchChannel(): void {
    const query = this.searchQuery.trim().toLowerCase();
    if (query) {
      this.filteredChannels = this.channels?.filter(channel =>
        channel.channelName.toLowerCase().startsWith(query)
      ) || [];
      this.overlayOpen = this.filteredChannels.length > 0;
    } else {
      this.filteredChannels = []; 
      this.overlayOpen = false; 
    }
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
      this.channelsService.leaveChannel(channelId, userId).subscribe( _ => {
        console.log('success');
      },
        (error) => {
          console.error(error);
        }
      );

    });
  }

}
