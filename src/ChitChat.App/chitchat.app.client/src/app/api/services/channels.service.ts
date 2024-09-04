import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment.development'
import { ChannelRequestModel } from '../models/channel-request-model';
import { ChannelResponseModel } from '../models/channel-response-model';
import { MemberResponseModel } from '../models/member-response-model';
import { ChannelStatsDto } from '../models/channel-stats-dto';

@Injectable({
  providedIn: 'root'
})
export class ChannelsService {
  channelsUrl = "/Channel/"
  channelsbyNameUrl = "/Channel/name/"
  createChannelUrl = "/Channel/create/"
  channelsbyUserId = "/Channel/user-channel/"
  topChannelsUrl = "/Channel/channels"
  constructor(private httpClient: HttpClient) { }

  public getChannels(): Observable<ChannelResponseModel[]> {
    const url = `${environment.apiUrl}${this.channelsUrl}`;
    console.log('Request URL:', url);
    return this.httpClient.get<ChannelResponseModel[]>(url);
  }

  public getTopChannels(): Observable<ChannelStatsDto[]> {
    const url = `${environment.apiUrl}${this.topChannelsUrl}`;
    console.log('Request URL:', url);
    return this.httpClient.get<ChannelStatsDto[]>(url);
  }

  public getChannelMembers(id: number): Observable<MemberResponseModel[]> {
    const url = `${environment.apiUrl}${this.channelsUrl}members/${id}`;
    console.log('Request URL:', url);
    return this.httpClient.get<MemberResponseModel[]>(url);
  }

  public getChannelById(id: number): Observable<ChannelResponseModel> {
    const url = `${environment.apiUrl}${this.channelsUrl}${id}`;
    console.log('Request URL:', url);
    return this.httpClient.get<ChannelResponseModel>(url);
  }

  public getChannelByName(name: string): Observable<ChannelResponseModel> {
    const url = `${environment.apiUrl}${this.channelsbyNameUrl}${name}`;
    console.log('Request URL:', url);
    return this.httpClient.get<ChannelResponseModel>(url);
  }

  public getChannelsUserId(id: string): Observable<ChannelResponseModel[]> {
    const url = `${environment.apiUrl}${this.channelsbyUserId}${id}`;
    console.log('Request URL:', url);
    return this.httpClient.get<ChannelResponseModel[]>(url);
  }

  public createChannel(request: ChannelRequestModel): Observable<ChannelResponseModel> {
    const url = `${environment.apiUrl}${this.createChannelUrl}`;
    console.log('Request URL:', url);
    return this.httpClient.post<ChannelResponseModel>(url, request);
  }

  public joinChannel(channelId: number, userId: string): Observable<ChannelResponseModel> {
    const url = `${environment.apiUrl}${this.channelsUrl}${channelId}/user/${userId}/join`;
    const body = {};
    console.log('Request URL:', url);
    return this.httpClient.post<ChannelResponseModel>(url,body);
  }

  public leaveChannel(channelId: number, userId: string): Observable<ChannelResponseModel> {
    const url = `${environment.apiUrl}${this.channelsUrl}${channelId}/user/${userId}/leave`;
    const body = {};
    console.log('Request URL:', url);
    return this.httpClient.post<ChannelResponseModel>(url, body);
  }

  public updateChannel(id: number, request: ChannelRequestModel): Observable<ChannelResponseModel> {
    const url = `${environment.apiUrl}${this.channelsUrl}${id}`;
    console.log('Request URL:', url);
    return this.httpClient.put<ChannelResponseModel>(url, request);
  }

  public deleteChannel(id: number): Observable<ChannelResponseModel> {
    const url = `${environment.apiUrl}${this.channelsUrl}${id}`;
    console.log('Request URL:', url);
    return this.httpClient.delete<ChannelResponseModel>(url);
  }
  
}
