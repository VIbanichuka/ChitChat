import { Component, Inject, OnInit } from '@angular/core';
import { MAT_DIALOG_DATA } from '@angular/material/dialog';
import { UserResponseModel } from '../api/models';
import { FriendshipRequest } from '../api/models/friendship-request-model';
import { UserService } from '../api/services';
import { AuthService } from '../api/services/auth.service';
import { FriendshipService } from '../api/services/friendship.service';


@Component({
  selector: 'app-user-hub',
  templateUrl: './user-hub.component.html',
  styleUrls: ['./user-hub.component.css']
})
export class UserHubComponent implements OnInit {
  user: any;
  userInfo: UserResponseModel | null = null;

  constructor(@Inject(MAT_DIALOG_DATA) public data: { user: any },
    private userService: UserService, private friendshipService: FriendshipService, private authService: AuthService) {
    this.user = data.user;
  }

  ngOnInit(): void {
    this.getUserInfo();
  }

  getUserInfo() {
    this.userService.getUserById(this.user.userId).subscribe((userInfo: UserResponseModel) => {
      this.userInfo = userInfo;
    }
    );
  }

  sendInvite() {
    this.authService.getUserIdFromToken().subscribe(userId => {
      if (!userId)
        return;

      const friendshipRequest: FriendshipRequest = { inviterId: userId, inviteeId: this.user.userId };
      this.friendshipService.sendFriendRequest(friendshipRequest).subscribe(response => {
        console.log('success');
      }, error => {
        console.error("failed sending invite");
      })
    })
  }

}
