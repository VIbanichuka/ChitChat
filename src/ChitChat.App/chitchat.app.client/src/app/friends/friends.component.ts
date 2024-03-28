import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { FriendModel } from '../api/models/friend-model';
import { AuthService } from '../api/services/auth.service';
import { FriendshipService } from '../api/services/friendship.service';
import { UserHubComponent } from '../user-hub/user-hub.component';

@Component({
  selector: 'app-friends',
  templateUrl: './friends.component.html',
  styleUrls: ['./friends.component.css']
})
export class FriendsComponent implements OnInit {
  friends: FriendModel[] | null = null;
  constructor(private matDialog: MatDialog, private friendshipService: FriendshipService, private authService: AuthService) { }
    ngOnInit(): void {
      this.getFriends();
    }

  getFriends() {
    this.authService.getUserIdFromToken().subscribe(userId => {
      if (!userId)
        return;
      this.friendshipService.getFriends(userId).subscribe((friends: FriendModel[]) => {
        this.friends = friends;
        console.log(friends);
      })
    })
  }

  openUserHubDialog(friend: FriendModel): void {
    this.matDialog.open(UserHubComponent, {
      width: '100%',
      maxWidth: '600px',
      data: { user: friend, showInviteButton: false }
    });
  }
}
