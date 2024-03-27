import { Component, OnInit } from '@angular/core';
import { MainUserProfileComponent } from 'src/app/main-user-profile/main-user-profile.component';
import { MatDialog } from '@angular/material/dialog';
import { SearchBarService } from '../api/services/search-bar.service';
import { UserProfileResponseModel } from '../api/models';
import { UserProfileService } from '../api/services';
import { Observable } from 'rxjs';
import { UserHubComponent } from '../user-hub/user-hub.component';

@Component({
  selector: 'app-sidenav',
  templateUrl: './sidenav.component.html',
  styleUrls: ['./sidenav.component.css']
})
export class SidenavComponent implements OnInit {
  results: UserProfileResponseModel[] | null = null;
  searchTerm: string = '';
  searchResults$: Observable<UserProfileResponseModel[]> | null = null;

  constructor(private matDialog: MatDialog, private searchBarService: SearchBarService, private userProfileService: UserProfileService) {
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
      this.closeOverlay();
      const matDialogRef = this.matDialog.open(UserHubComponent, {
        width: '100%',
        maxWidth: '600px',
        data: { user: user }
      });

      matDialogRef.afterOpened().subscribe(result => {
        this.searchTerm = '';
      });
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
}
