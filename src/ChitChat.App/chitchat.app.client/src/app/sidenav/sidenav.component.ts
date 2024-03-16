import { Component } from '@angular/core';
import { MainUserProfileComponent } from 'src/app/main-user-profile/main-user-profile.component';
import { MatDialog } from '@angular/material/dialog';

@Component({
  selector: 'app-sidenav',
  templateUrl: './sidenav.component.html',
  styleUrls: ['./sidenav.component.css']
})
export class SidenavComponent {
  constructor(private matDialog: MatDialog) { }
  opened = true;
  openDialog() {
    this.matDialog.open(MainUserProfileComponent, {
      width: '400px',
    })
  }
  
}
