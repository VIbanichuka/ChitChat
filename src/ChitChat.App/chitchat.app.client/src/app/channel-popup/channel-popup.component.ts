import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { ChannelRequestModel } from '../api/models/channel-request-model';
import { ChannelsService } from '../api/services/channels.service';
import { Router } from '@angular/router';


@Component({
  selector: 'app-channel-popup',
  templateUrl: './channel-popup.component.html',
  styleUrls: ['./channel-popup.component.css']
})
export class ChannelPopupComponent implements OnInit {
  form: FormGroup;
  channel: ChannelRequestModel | null = null;
  constructor(private channelsService: ChannelsService,
    private formbuilder: FormBuilder,
    private router: Router,
    private matDialog: MatDialog
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
    this.channelsService.createChannel(this.form.value).subscribe(_ => {
      console.log('posted to server');
      this.router.navigate(['./channels']);
    })
  }

}
