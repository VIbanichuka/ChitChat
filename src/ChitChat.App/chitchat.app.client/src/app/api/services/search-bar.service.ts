import { Injectable } from '@angular/core';
import { debounceTime, distinctUntilChanged, Observable, of, Subject, switchMap } from 'rxjs';
import { environment } from '../../../environments/environment.development';
import { UserProfileResponseModel } from '../models';
import { HttpClient } from '@angular/common/http'

@Injectable({
  providedIn: 'root'
})
export class SearchBarService {

  private searchSubject: Subject<string> = new Subject<string>();
  searchUrl = "/UserProfile/query/"
  constructor(private httpClient: HttpClient) { }

  public searchEntries(searchTerm: string | ''): Observable<UserProfileResponseModel[]> {
    const url = `${environment.apiUrl}${this.searchUrl}${searchTerm}`;
    console.log('Request URL:', url);
    return this.httpClient.get<UserProfileResponseModel[]>(url);
  }

  public search(searchTerms: Observable<string>): Observable<any> {
    return searchTerms.pipe(
      debounceTime(300),
      distinctUntilChanged(),
      switchMap(searchTerm => this.searchEntries(searchTerm)));
  }

  public updateSearchTerm(term: string): void {
    this.searchSubject.next(term);
  }

  public getSearchSubject(): Observable<string> {
    return this.searchSubject.asObservable();
  }
}
