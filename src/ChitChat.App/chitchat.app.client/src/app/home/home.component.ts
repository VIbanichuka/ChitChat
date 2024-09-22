import { Component, OnInit} from '@angular/core';
import { FriendshipService } from '../api/services/friendship.service';
import { FriendModel } from '../api/models/friend-model';
import { AuthService } from '../api/services/auth.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css'],
})
export class HomeComponent implements OnInit{ 
  username: string | null = '';
  friends: FriendModel[] = [];
  displayFriends: FriendModel[] = [];
  constructor(private friendshipService: FriendshipService, private authService: AuthService, private router: Router) {}
  
  ngOnInit(): void {
    this.authService.getUsernameFromToken().subscribe(displayName => {
      this.username = displayName;
    });
    this.getSortedFriendsList();
  }

  getSortedFriendsList() {
    this.authService.getUserIdFromToken().subscribe(userId => {
      if (!userId)
        return;
      
      this.friendshipService.getFriends(userId).subscribe((friends: FriendModel[]) => {
        this.friends = friends.sort((a, b) => a.displayName.localeCompare(b.displayName));
        this.displayFriends = this.friends.slice(0, 6);
        console.log(this.displayFriends);
      });
    });
  }

  openUserDm(friend: FriendModel) {
    this.router.navigate(['/home/dms', friend.userId]);
  }
}
