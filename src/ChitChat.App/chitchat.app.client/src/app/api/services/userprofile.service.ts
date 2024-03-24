import { Injectable } from '@angular/core'
import { HttpClient } from '@angular/common/http'
import { Observable } from 'rxjs';
import { UserProfileRequestModel, UserProfileResponseModel } from "../models";
import { environment } from 'src/environments/environment.development'
import { UserProfilePhotoRequest } from '../models/user-profile-photo-request-model';

@Injectable({
  providedIn: 'root'
})

export class UserProfileService {
  userProfileUrl = "/UserProfile/"
  userProfilesUrl = "/UserProfile"
  uploadPhotoUrl = "/UserProfile/upload-photo/"
  deletePhotoUrl = "/UserProfile/remove-photo/"
  searchUrl = "/UserProfile/query/"
  constructor(private httpClient: HttpClient) { }

  public getUserProfileById(id: string): Observable<UserProfileResponseModel> {
    const url = `${environment.apiUrl}${this.userProfileUrl}${id}`;
    console.log('Request URL:', url);
    return this.httpClient.get<UserProfileResponseModel>(url);
  }

  public getAllUserProfiles(): Observable<UserProfileResponseModel[]> {
    const url = `${environment.apiUrl}${this.userProfilesUrl}`;
    console.log('Request URL:', url);
    return this.httpClient.get<UserProfileResponseModel[]>(url);
  }

  public updateUserProfile(id: string | null, userProfileRequest: UserProfileRequestModel): Observable<UserProfileResponseModel> {
    const url = `${environment.apiUrl}${this.userProfileUrl}${id}`;
    console.log('Request URL:', url);
    return this.httpClient.put<UserProfileResponseModel>(url, userProfileRequest);
  }

  public uploadUserProfilePhoto(userProfilePhotoRequest: UserProfilePhotoRequest | any, id: string | null): Observable<UserProfileResponseModel>{
    const url = `${environment.apiUrl}${this.uploadPhotoUrl}${id}`;
    console.log('Request URL:', url);
    return this.httpClient.post<UserProfileResponseModel>(url, userProfilePhotoRequest);
  }

  public removeUserProfilePhoto(id: string | null): Observable<void> {
    const url = `${environment.apiUrl}${this.deletePhotoUrl}${id}`;
    console.log('Request URL:', url);
    return this.httpClient.delete<void>(url);
  }

  public searchUsers(searchTerm: string | ''): Observable<UserProfileResponseModel[]> {
    const url = `${environment.apiUrl}${this.searchUrl}${searchTerm}`;
    console.log('Request URL:', url);
    return this.httpClient.get<UserProfileResponseModel[]>(url);
  }
}
