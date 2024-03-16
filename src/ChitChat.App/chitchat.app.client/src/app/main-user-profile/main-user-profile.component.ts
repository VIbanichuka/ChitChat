import { Component, OnInit } from '@angular/core';
import { UserProfileComponent } from 'src/app/user-profile/user-profile.component'
import { MatDialog } from '@angular/material/dialog';
import { UserProfileService } from '../api/services';
import { UserProfileResponseModel } from '../api/models';
import { AuthService } from '../api/services/auth.service';

@Component({
  selector: 'app-main-user-profile',
  templateUrl: './main-user-profile.component.html',
  styleUrls: ['./main-user-profile.component.css']
})
export class MainUserProfileComponent implements OnInit {
  userProfile: UserProfileResponseModel | null = null;

  constructor(private matDialog: MatDialog,
    private userProfileService: UserProfileService,
    private authService: AuthService) { }

    ngOnInit(): void {
      this.getUserProfile();
    }
  openEditUserProfileDialog() {
    this.matDialog.open(UserProfileComponent, {
      width: '600px',
    } )
  }

  getUserProfile(): void {
    this.authService.getUserIdFromToken().subscribe(userId => {
      if (!userId) {
        return;
      }

      this.userProfileService.getUserProfileById(userId).subscribe(
        (userProfile: UserProfileResponseModel) => {
          this.userProfile = userProfile;
        }
      )
    })
  }

}
