import { Injectable } from "@angular/core";
import { HttpClient, HttpResponse, HttpContext } from "@angular/common/http";
import { BaseService } from "../base-service";
import { Observable, of } from 'rxjs';
import { ApiConfiguration } from "../api-configuration";
import { StrictHttpResponse } from "../strict-http-response";
import { RequestBuilder } from "../request-builder";
import { map, filter } from "rxjs/operators";
import { UserLoginRequestModel, UserResponseModel } from "src/app/api/models";
import { JwtHelperService, JwtModule } from "@auth0/angular-jwt";

@Injectable({
  providedIn: 'root'
})

export class AuthService extends BaseService {

  jwtHelper = new JwtHelperService();
  constructor(
    config: ApiConfiguration,
    http: HttpClient,
  ) {
    super(config, http);
  }

  static readonly AuthPostPath = '/Auth/Login/'
  authPost$Response(params?: {
    body?: UserLoginRequestModel
  },
    context?: HttpContext

  ): Observable<StrictHttpResponse<string>> {

    const rb = new RequestBuilder(this.rootUrl, AuthService.AuthPostPath, 'post');
    if (params) {
      rb.body(params.body, 'application/*+json');
    }

    return this.http.request(rb.build({
      responseType: 'json',
      accept: 'text/json',
      context: context
    })).pipe(
      filter((r: any) => r instanceof HttpResponse),
      map((r: HttpResponse<any>) => {
        return r as StrictHttpResponse<string>;
      })
    );
  }

  authPost(params?: {
    body?: UserLoginRequestModel
  },
    context?: HttpContext

  ): Observable<string> {

    return this.authPost$Response(params, context).pipe(
      map((r: StrictHttpResponse<string>) => r.body)
    );
  }

  getUserIdFromToken(): Observable<string | null>{
    const token = localStorage.getItem('token');
    if (token && !this.jwtHelper.isTokenExpired(token)) {
      const decodedToken = this.jwtHelper.decodeToken(token);
      return of(decodedToken.id);
    } else {
      return of(null);
    }
  }
}
