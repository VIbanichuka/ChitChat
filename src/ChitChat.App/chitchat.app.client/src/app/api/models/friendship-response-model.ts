export interface FriendshipResponseModel {
  friendshipId: number;
  displayName?: string | null;
  profilePicture?: string | null;
  inviteTime: Date;
}
