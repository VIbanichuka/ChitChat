import { Component, OnInit } from '@angular/core';
import { UserProfileService } from 'src/app/api/services/userprofile.service'
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { AuthService } from '../api/services/auth.service';
import { UserProfileResponseModel, UserResponseModel } from '../api/models';
import { MatDialog } from '@angular/material/dialog';
import { UserService } from '../api/services';

@Component({
  selector: 'app-user-profile',
  templateUrl: './user-profile.component.html',
  styleUrls: ['./user-profile.component.css']
})
export class UserProfileComponent implements OnInit {
  form: FormGroup;
  userProfile: UserProfileResponseModel | null = null;
  initials: string = '';
  user: UserResponseModel | null = null;
  circleColor: string = '';
  showInitials: boolean = false;
  selectedFile: File | null = null

  private colors = [
    '#EB7181',
    '#468547',
    '#FFD558',
    '#3670B2',
  ];

  constructor(private userProfileService: UserProfileService,
    private formbuilder: FormBuilder,
    private authService: AuthService,
    private userService: UserService,
    private matDialog: MatDialog) {

    this.form = this.formbuilder.group({
      firstName: ['', [Validators.maxLength(50)]],
      lastName: ['', [Validators.maxLength(50)]],
      bio: ['', [Validators.maxLength(300)]],
    })
  }

  ngOnInit(): void {
    this.prefillProfile();
    this.showAlternativeProfilePhoto();
  }

  closeUserProfileDialogs() {
    this.matDialog.closeAll()
  }

  editUserProfile() {
    this.authService.getUserIdFromToken().subscribe(userId => {
      if (!userId) {
        console.log('no user found')
        return;
      }

      if (this.form.invalid)
        return;

      this.userProfileService.updateUserProfile(userId, this.form.value).subscribe(_ => {
        console.log('updated')
      });
      this.closeUserProfileDialogs();
    });
  }

  removePhoto(): void {
    this.authService.getUserIdFromToken().subscribe(userId => {
      if (!userId)
        return;
      this.showAlternativeProfilePhoto();
      this.userProfileService.removeUserProfilePhoto(userId).subscribe(_ => {
        this.showInitials = true;
        console.log("success");
      })

    })
  }

  onFileSelected(event: any) {
    this.selectedFile = event.target.files[0];
    this.uploadPhoto();
  }

  uploadPhoto(): void {
    if (!this.selectedFile)
      return;

    const formData = new FormData();
    formData.append('imageFile', this.selectedFile, this.selectedFile.name);

    this.authService.getUserIdFromToken().subscribe(userId => {
      if (!userId)
        return;
      this.userProfileService.uploadUserProfilePhoto(formData, userId).subscribe((userProfile: UserProfileResponseModel) => {
        this.userProfile = userProfile;
        this.showInitials = !userProfile.profilePicture;
        console.log("success");
      })

    })
  }

  private prefillProfile(): void {
    this.authService.getUserIdFromToken().subscribe(userId => {
      if (!userId) {
        return;
      }

      this.userProfileService.getUserProfileById(userId).subscribe(
        (userProfile: UserProfileResponseModel) => {
          this.userProfile = userProfile;
          this.prefillForm();
        }
      )
    })
  }

  private prefillForm(): void {
    if (this.userProfile) {
      this.form.patchValue({
        firstName: this.userProfile.firstName,
        lastName: this.userProfile.lastName,
        bio: this.userProfile.bio,
        profilePicture: this.userProfile.profilePicture,
      })
    }
  }

  showAlternativeProfilePhoto(): void {
    this.authService.getUserIdFromToken().subscribe(userId => {
      if (!userId)
        return;
      this.userService.getUserById(userId).subscribe(
        (user: UserResponseModel) => {
          this.user = user;
          console.log(user);
          this.createAlternativeProfilePic();
        }
      )
    })
  }

  createAlternativeProfilePic() {
    if (this.userProfile?.profilePicture === '' || this.userProfile?.profilePicture === null) {
      this.showInitials = true;
      this.createInitials();

      const randomIndex = Math.floor(Math.random() * Math.floor(this.colors.length));
      this.circleColor = this.colors[randomIndex];
      console.log(this.circleColor);
    }
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
}
