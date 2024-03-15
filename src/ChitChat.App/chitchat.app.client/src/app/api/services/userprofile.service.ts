import { Injectable } from '@angular/core'
import { HttpClient } from '@angular/common/http'
import { Observable } from 'rxjs';
import { UserProfileRequestModel, UserProfileResponseModel } from "../models";
import { environment } from 'src/environments/environment.development'

@Injectable({
  providedIn: 'root'
})

export class UserProfileService {
  userProfileUrl = "/UserProfile/"
  userProfilesUrl = "/UserProfile"
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

  public updateUserProfile(id: string, userProfileRequest: UserProfileRequestModel): Observable<UserProfileResponseModel> {
    const url = `${environment.apiUrl}${this.userProfileUrl}${id}`;
    console.log('Request URL:', url);
    return this.httpClient.put<UserProfileResponseModel>(url, userProfileRequest);
  }
}
