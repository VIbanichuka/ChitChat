import { Component, Inject } from '@angular/core';
import { UserProfileComponent } from 'src/app/user-profile/user-profile.component'
import { MatDialog } from '@angular/material/dialog';

@Component({
  selector: 'app-main-user-profile',
  templateUrl: './main-user-profile.component.html',
  styleUrls: ['./main-user-profile.component.css']
})
export class MainUserProfileComponent {
  constructor(private matDialog: MatDialog) { }
  openEditUserProfileDialog() {
    this.matDialog.open(UserProfileComponent, {
      width: '600px',
    } )
  }
}
