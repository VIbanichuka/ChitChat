import { Component, OnInit } from '@angular/core';
import { UserProfileService } from 'src/app/api/services/userprofile.service'
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router} from '@angular/router';
import { UserProfileResponseModel } from '../api/models';

@Component({
  selector: 'app-user-profile',
  templateUrl: './user-profile.component.html',
  styleUrls: ['./user-profile.component.css']
})
export class UserProfileComponent implements OnInit {
  form: FormGroup;

  constructor(private userProfileService: UserProfileService,
    private formbuilder: FormBuilder,
    private router: Router,
  ) {
    this.form = this.formbuilder.group({
      firstName: ['', [Validators.maxLength(50)]],
      lastName: ['', [Validators.maxLength(50)]],
      Bio: ['', [Validators.maxLength(300)]],
    })

  }

  profileId: string = ''
  userProfiles: UserProfileResponseModel[] = [];

 
  ngOnInit(): void {
    this.getAllUserProfiles();
  }

  editUserProfile(): void {

  }

  getAllUserProfiles() {
    this.userProfileService.getAllUserProfiles().subscribe(
      (userProfiles: UserProfileResponseModel[]) => {
        this.userProfiles = userProfiles;
        console.log(userProfiles)
      },
      (error) => {
        console.error('Error fetching user profiles:', error);
      }
    );
  }
}
