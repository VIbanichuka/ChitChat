import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatCardModule } from '@angular/material/card';
import { ChannelsService } from 'src/app/api/services/channels.service';
import { ChannelStatsDto } from 'src/app/api/models/channel-stats-dto';

@Component({
  selector: 'app-top-channels',
  standalone: true,
  imports: [CommonModule, MatCardModule],
  templateUrl: './top-channels.component.html',
  styleUrls: ['./top-channels.component.css']
})
export class TopChannelsComponent implements OnInit {
  channels: ChannelStatsDto[] = [];
  constructor(private channelsService: ChannelsService) {}
  ngOnInit(): void {
    this.getTopChannels();
  }

  getTopChannels(){
    this.channelsService.getTopChannels().subscribe((channels: ChannelStatsDto[])=>{
      this.channels = channels;
    })
  }


}
