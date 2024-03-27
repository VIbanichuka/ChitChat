import { Component, OnInit } from '@angular/core';
import { FriendModel } from '../api/models/friend-model';
import { AuthService } from '../api/services/auth.service';
import { FriendshipService } from '../api/services/friendship.service';

@Component({
  selector: 'app-friends',
  templateUrl: './friends.component.html',
  styleUrls: ['./friends.component.css']
})
export class FriendsComponent implements OnInit {
  friends: FriendModel[] | null = null;
  constructor(private friendshipService: FriendshipService, private authService: AuthService) { }
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
}
