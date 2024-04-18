import { Component } from '@angular/core';
import { UserProfileResponseModel, UserResponseModel } from '../api/models';
import { UserProfileService, UserService } from '../api/services';
import { SignalrService } from '../api/services/signalr.service';

@Component({
  selector: 'app-dms',
  templateUrl: './dms.component.html',
  styleUrls: ['./dms.component.css']
})
export class DmsComponent {
  userInfo: UserResponseModel | null = null;
  userProfileInfo: UserProfileResponseModel | null = null;
  constructor(private userProfileService: UserProfileService, private userService: UserService, private signalrService: SignalrService) { }


}
