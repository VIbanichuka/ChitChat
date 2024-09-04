import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import {environment } from 'src/environments/environment.development'
import { FriendshipRequest } from '../models/friendship-request-model';
import { FriendModel } from '../models/friend-model';
import { FriendshipResponseModel } from '../models/friendship-response-model';
import { FriendshipRequestStatsDto } from '../models/friendship-request-stats-dto';

@Injectable({
  providedIn: 'root'
})
export class FriendshipService {
  sendrequestUrl = "/Friendship/send-request"
  friendsUrl = "/Friendship/friends/"
  pendingInviteUrl = "/Friendship/pending-requests/"
  acceptInviteUrl = "/Friendship/accept-request/"
  rejectInviteUrl = "/Friendship/reject-request/"
  friendshipRequestStatsUrl = "/Friendship/sent-request-stats/"
  constructor(private httpClient: HttpClient) { }

  public sendFriendRequest(frienshipRequest: FriendshipRequest): Observable<FriendshipRequest> {
    const url = `${environment.apiUrl}${this.sendrequestUrl}`;
    console.log('Request URL:', url);
    return this.httpClient.post<FriendshipRequest>(url, frienshipRequest);
  }

  public getFriends(id: string): Observable<FriendModel[]> {
    const url = `${environment.apiUrl}${this.friendsUrl}${id}`;
    console.log('Request URL:', url);
    return this.httpClient.get<FriendModel[]>(url);
  }

  public getSentFriendRequestStats(id: string): Observable<FriendshipRequestStatsDto[]> {
    const url = `${environment.apiUrl}${this.friendshipRequestStatsUrl}${id}`;
    console.log('Request URL:', url);
    return this.httpClient.get<FriendshipRequestStatsDto[]>(url);
  }

  public getPendingInvites(id: string): Observable<FriendshipResponseModel[]> {
    const url = `${environment.apiUrl}${this.pendingInviteUrl}${id}`;
    console.log('Request URL:', url);
    return this.httpClient.get<FriendshipResponseModel[]>(url);
  }

  public acceptInvite(id: number): Observable<void> {
    const url = `${environment.apiUrl}${this.acceptInviteUrl}${id}`
    console.log('Request URL:', url);
    return this.httpClient.put<void>(url, null);
  }

  public rejectInvite(id: number): Observable<void> {
    const url = `${environment.apiUrl}${this.rejectInviteUrl}${id}`
    console.log('Request URL:', url);
    return this.httpClient.put<void>(url, null);
  }
}
