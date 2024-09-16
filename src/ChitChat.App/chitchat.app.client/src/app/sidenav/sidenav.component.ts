import { Component, OnInit } from '@angular/core';
import { MainUserProfileComponent } from 'src/app/main-user-profile/main-user-profile.component';
import { MatDialog } from '@angular/material/dialog';
import { SearchBarService } from '../api/services/search-bar.service';
import { UserProfileResponseModel } from '../api/models';
import { UserProfileService } from '../api/services';
import { Observable } from 'rxjs';
import { UserHubComponent } from '../user-hub/user-hub.component';
import { Router } from '@angular/router';
import { AuthService } from '../api/services/auth.service';
import { FriendshipService } from '../api/services/friendship.service';

@Component({
  selector: 'app-sidenav',
  templateUrl: './sidenav.component.html',
  styleUrls: ['./sidenav.component.css']
})
export class SidenavComponent implements OnInit {
  results: UserProfileResponseModel[] | null = null;
  searchTerm: string = '';
  searchResults$: Observable<UserProfileResponseModel[]> | null = null;

  constructor(private router: Router, 
    private authService: AuthService, 
    private matDialog: MatDialog, 
    private friendshipService: FriendshipService,
    private searchBarService: SearchBarService, 
    private userProfileService: UserProfileService) {
    this.searchResults$ = this.searchBarService.search(this.searchBarService.getSearchSubject());
  }

  updateSearchTerm() {
    if (this.searchTerm.trim() === '') {
      this.searchResults$ = null;
      this.results = null;
      this.closeOverlay();
    } else {
      this.searchBarService.updateSearchTerm(this.searchTerm);
    }
  }

  ngOnInit(): void {
  }

  opened = true;
  overlayOpen = false;

  openProfileDialog() {
    this.matDialog.open(MainUserProfileComponent, {
      maxWidth: '400px',
      width: '100%'
    })
  }

  openProfileHub(userId: string | any) {
    this.userProfileService.getUserProfileById(userId).subscribe(user => {
      this.checkIfAFriend(userId).then(isFriend => {
        this.closeOverlay();
        const matDialogRef = this.matDialog.open(UserHubComponent, {
          width: '100%',
          maxWidth: '600px',
          data: { user: user, showInviteButton: !isFriend }
        });
        matDialogRef.afterOpened().subscribe(result => {
          this.searchTerm = '';
        });
      })     
    });  
  }

  closeOverlay() {
    this.overlayOpen = false;
  }

  search() {
    this.userProfileService.searchUsers(this.searchTerm).subscribe((searchResults: UserProfileResponseModel[]) => {
      this.results = searchResults;
      console.log(searchResults);
    })

  }

  signOut() {
    localStorage.removeItem('token');
    this.authService.clearKeys();
    this.router.navigate(['/signin-user']);
  }

  checkIfAFriend(userId: string): Promise<boolean> {
    return new Promise((resolve, reject) => {
      this.authService.getUserIdFromToken().subscribe(currentUserId => {
        if (!currentUserId) {
          resolve(false);
          return;
        }
        this.friendshipService.getFriends(currentUserId).subscribe(friends => {
          const isFriend = friends.some(friend => friend.userId === userId);
          resolve(isFriend);
        }, error => {
          reject(error);
        });
      }, error => {
        reject(error);
      });
    });
  }
  
}
