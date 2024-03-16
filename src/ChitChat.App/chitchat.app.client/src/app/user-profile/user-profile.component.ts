import { Component, OnInit } from '@angular/core';
import { UserProfileService } from 'src/app/api/services/userprofile.service'
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { AuthService } from '../api/services/auth.service';
import { UserProfileResponseModel } from '../api/models';
import { MatDialog } from '@angular/material/dialog';

@Component({
  selector: 'app-user-profile',
  templateUrl: './user-profile.component.html',
  styleUrls: ['./user-profile.component.css']
})
export class UserProfileComponent implements OnInit {
  form: FormGroup;
  userProfile: UserProfileResponseModel | null = null;

  constructor(private userProfileService: UserProfileService,
    private formbuilder: FormBuilder,
    private authService: AuthService,
    private matDialog: MatDialog) {

    this.form = this.formbuilder.group({
      firstName: ['', [Validators.maxLength(50)]],
      lastName: ['', [Validators.maxLength(50)]],
      bio: ['', [Validators.maxLength(300)]],
    })
  }

  ngOnInit(): void {
    this.prefillProfile();
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

  prefillProfile(): void {
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
        bio: this.userProfile.bio
      })
    }
  }

}
