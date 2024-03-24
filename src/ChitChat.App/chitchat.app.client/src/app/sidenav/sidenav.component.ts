import { Component, OnInit } from '@angular/core';
import { MainUserProfileComponent } from 'src/app/main-user-profile/main-user-profile.component';
import { MatDialog } from '@angular/material/dialog';
import { SearchBarService } from '../api/services/search-bar.service';
import { UserProfileResponseModel } from '../api/models';
import { UserProfileService } from '../api/services';
import { Observable } from 'rxjs';

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

  openDialog() {
    this.matDialog.open(MainUserProfileComponent, {
      width: '400px',
    })
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
