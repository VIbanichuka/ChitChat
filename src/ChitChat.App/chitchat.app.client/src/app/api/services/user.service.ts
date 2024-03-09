import { Injectable } from '@angular/core'
import { HttpClient } from '@angular/common/http'
import { Observable } from 'rxjs';
import { UserRequestModel, UserResponseModel } from "../models";
import { environment } from 'src/environments/environment.development'

@Injectable({
  providedIn: 'root'
})

export class UserService {
  userByEmailUrl = "/User/find-email/"
  userUrl = "/User/"
  userEmailUrl = "/User/email/"
  userDisplayNameUrl = "/User/display-name/"
  constructor(private httpClient: HttpClient) { }

  public findUserByEmail(email: string): Observable<UserResponseModel> {
    const url = `${environment.apiUrl}${this.userByEmailUrl}${email}`;
    console.log('Request URL:', url);
    return this.httpClient.get<UserResponseModel>(url);
  }

  public getUserById(id: string): Observable<UserResponseModel> {
    const url = `${environment.apiUrl}${this.userUrl}${id}`;
    console.log('Request URL:', url);
    return this.httpClient.get<UserResponseModel>(url);
  }

  public deleteUser(id: string): Observable<UserResponseModel> {
    const url = `${environment.apiUrl}${this.userUrl}${id}`;
    console.log('Request URL:', url);
    return this.httpClient.delete<UserResponseModel>(url);
  }

  public updateUser(id: string, userRequest: UserRequestModel): Observable<UserResponseModel> {
    const url = `${environment.apiUrl}${this.userUrl}${id}`;
    console.log('Request URL:', url);
    return this.httpClient.put<UserResponseModel>(url, userRequest);
  }

  public getAllUsers(): Observable<UserResponseModel> {
    const url = `${environment.apiUrl}${this.userUrl}`;
    console.log('Request URL:', url);
    return this.httpClient.get<UserResponseModel>(url);
  }

  public registerUser(userRequest: UserRequestModel): Observable<UserResponseModel> {
    const url = `${environment.apiUrl}${this.userUrl}`;
    console.log('Request URL:', url);
    return this.httpClient.post<UserResponseModel>(url, userRequest);
  }

  public getEmail(email: string): Observable<boolean> {
    const url = `${environment.apiUrl}${this.userEmailUrl}${email}`;
    console.log('Request URL:', url);
    return this.httpClient.get<boolean>(url);
  }

  public getDisplayName(displayName: string): Observable<boolean> {
    const url = `${environment.apiUrl}${this.userDisplayNameUrl}${displayName}`;
    console.log('Request URL:', url);
    return this.httpClient.get<boolean>(url);
  }
}
