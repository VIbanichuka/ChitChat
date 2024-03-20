import { Component, OnInit } from '@angular/core';
import { UserProfileComponent } from 'src/app/user-profile/user-profile.component'
import { MatDialog } from '@angular/material/dialog';
import { UserProfileService, UserService } from '../api/services';
import { UserProfileResponseModel, UserResponseModel } from '../api/models';
import { AuthService } from '../api/services/auth.service';

@Component({
  selector: 'app-main-user-profile',
  templateUrl: './main-user-profile.component.html',
  styleUrls: ['./main-user-profile.component.css']
})
export class MainUserProfileComponent implements OnInit {
  userProfile: UserProfileResponseModel | null = null;
  initials: string = '';
  user: UserResponseModel | null = null;
  circleColor: string = '';
  showInitials: boolean = false;

  private colors = [
    '#EB7181',
    '#468547',
    '#FFD558',
    '#3670B2',
  ];

  constructor(private matDialog: MatDialog,
    private userProfileService: UserProfileService,
    private authService: AuthService,
    private userService: UserService) { }

  ngOnInit(): void {
    this.getUserProfile();
    this.getUser();
  }

  openEditUserProfileDialog() {
    this.matDialog.open(UserProfileComponent, {
      width: '600px',
    })
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

  getUser(): void {
    this.authService.getUserIdFromToken().subscribe(userId => {
      if (!userId)
        return;
      this.userService.getUserById(userId).subscribe(
        (user: UserResponseModel) => {
          this.user = user;
          console.log(user.userId);
          this.createAlternativeProfilePic();
        }
      )
    })
  }

  createInitials(): void {
    if (!this.user?.displayName)
      return;

    let displayName = this.user.displayName.trim();
    console.log(displayName);

    if (displayName.length === 0) {
      this.initials = "";
    }

    let firstCharacter = displayName.charAt(0);

    if (firstCharacter === firstCharacter.toLowerCase()) {
      firstCharacter = firstCharacter.toUpperCase();
    }
    console.log(firstCharacter);
    this.initials = firstCharacter;
  }

  createAlternativeProfilePic() {
    if (this.userProfile?.profilePicture === '') {
      this.showInitials = true;
      this.createInitials();

      const randomIndex = Math.floor(Math.random() * Math.floor(this.colors.length));
      this.circleColor = this.colors[randomIndex];
      console.log(this.circleColor);
    }
  }
}
