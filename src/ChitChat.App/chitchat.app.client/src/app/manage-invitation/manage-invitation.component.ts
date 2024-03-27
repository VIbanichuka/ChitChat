import { Component, OnInit } from '@angular/core';
import { FriendshipResponseModel } from '../api/models/friendship-response-model';
import { AuthService } from '../api/services/auth.service';
import { FriendshipService } from '../api/services/friendship.service';

@Component({
  selector: 'app-manage-invitation',
  templateUrl: './manage-invitation.component.html',
  styleUrls: ['./manage-invitation.component.css']
})
export class ManageInvitationComponent implements OnInit {
  pendingInvites: FriendshipResponseModel[] | null = null;

  constructor(private authService: AuthService, private friendshipService: FriendshipService) { }

    ngOnInit(): void {
      this.getPendingInvitations();
    }

  getPendingInvitations() {
    this.authService.getUserIdFromToken().subscribe(userId => {
      if (!userId)
        return;
      this.friendshipService.getPendingInvites(userId).subscribe((pendingInvites: FriendshipResponseModel[]) => {
        this.pendingInvites = pendingInvites;
      })
    })
  }

  acceptInvite(friendshipId: number) {
    this.friendshipService.acceptInvite(friendshipId).subscribe(() => {
    });
  }

  rejectInvite(friendshipId: number) {
    this.friendshipService.rejectInvite(friendshipId).subscribe(() => {
    });
  }
}
