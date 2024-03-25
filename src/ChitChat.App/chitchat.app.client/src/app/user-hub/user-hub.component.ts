import { Component, Inject, OnInit } from '@angular/core';
import { MAT_DIALOG_DATA } from '@angular/material/dialog';
import { UserResponseModel } from '../api/models';
import { UserService } from '../api/services';


@Component({
  selector: 'app-user-hub',
  templateUrl: './user-hub.component.html',
  styleUrls: ['./user-hub.component.css']
})
export class UserHubComponent implements OnInit {
  user: any;
  userInfo: UserResponseModel | null = null;
  constructor(@Inject(MAT_DIALOG_DATA) public data: { user: any },
    private userService: UserService) {
    this.user = data.user;
  }

  ngOnInit(): void {
    this.getUserInfo();
  }

  getUserInfo() {
    this.userService.getUserById(this.user.userId).subscribe((userInfo: UserResponseModel) => {
      this.userInfo = userInfo;
    }
    );
  }
}
