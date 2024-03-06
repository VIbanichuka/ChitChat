import { Injectable } from "@angular/core";
import { HttpClient, HttpResponse, HttpContext } from "@angular/common/http";
import { BaseService } from "../base-service";
import { Observable } from 'rxjs';
import { ApiConfiguration } from "../api-configuration";
import { StrictHttpResponse } from "../strict-http-response";
import { RequestBuilder } from "../request-builder";
import { map, filter } from "rxjs/operators";
import { UserProfileRequestModel, UserProfileResponseModel } from "../models";

@Injectable({
    providedIn: 'root'
})

export class UserProfileService extends BaseService {
    constructor(
        config: ApiConfiguration,
        http: HttpClient
    ) {
        super(config, http);
    }

    static readonly UserProfileIdGetPath = '/UserProfile/{id}';
    userProfileIdGet$Response(params: {
        id: string;
    },
        context?: HttpContext

    ): Observable<StrictHttpResponse<UserProfileResponseModel>> {

        const rb = new RequestBuilder(this.rootUrl, UserProfileService.UserProfileIdGetPath, 'get');
        if (params) {
            rb.path('id', params.id, {});
        }

        return this.http.request(rb.build({
            responseType: 'json',
            accept: 'text/json',
            context: context
        })).pipe(
            filter((r: any) => r instanceof HttpResponse),
            map((r: HttpResponse<any>) => {
                return r as StrictHttpResponse<UserProfileResponseModel>;
            })
        );
    }

    userProfileIdGet(params: {
        id: string;
    },
        context?: HttpContext

    ): Observable<UserProfileResponseModel> {

        return this.userProfileIdGet$Response(params, context).pipe(
            map((r: StrictHttpResponse<UserProfileResponseModel>) => r.body as UserProfileResponseModel)
        );
    }

    static readonly UserProfileGetPath = '/UserProfile'
    userProfileGet$Response(params?: {
    },
        context?: HttpContext

    ): Observable<StrictHttpResponse<Array<UserProfileResponseModel>>> {

        const rb = new RequestBuilder(this.rootUrl, UserProfileService.UserProfileGetPath, 'get');
        if (params) {
        }

        return this.http.request(rb.build({
            responseType: 'json',
            accept: 'text/json',
            context: context
        })).pipe(
            filter((r: any) => r instanceof HttpResponse),
            map((r: HttpResponse<any>) => {
                return r as StrictHttpResponse<Array<UserProfileResponseModel>>;
            })
        );
    }

    userProfileGet(params?: {
    },
        context?: HttpContext

    ): Observable<Array<UserProfileResponseModel>> {

        return this.userProfileGet$Response(params, context).pipe(
            map((r: StrictHttpResponse<Array<UserProfileResponseModel>>) => r.body as Array<UserProfileResponseModel>)
        );
    }

    static readonly UserProfilePutPath = '/UserProfile/{id}';
    userProfilePut$Response(params: {
        id: string,
        body?: UserProfileRequestModel
    },
    context: HttpContext
    ): Observable<StrictHttpResponse<UserProfileResponseModel>>{
        const rb = new RequestBuilder(this.rootUrl, UserProfileService.UserProfilePutPath, 'put');
        if (params) {
            rb.path('id', params.id, {});
            rb.body(params.body, 'application/*+json');
        }
        return this.http.request(rb.build({
            responseType: 'json',
            accept: 'text/json',
            context: context
        })).pipe(
            filter((r: any) => r instanceof HttpResponse),
            map((r: HttpResponse<any>) => {
                return r as StrictHttpResponse<UserProfileResponseModel>;
            })
        );
    }

    userProfilePut(params: {
        id: string,
        body?: UserProfileRequestModel
    },
        context: HttpContext
    ): Observable<UserProfileResponseModel>{
        return this.userProfilePut$Response(params, context).pipe(
            map((r: StrictHttpResponse<UserProfileResponseModel>) => r.body as UserProfileResponseModel)
        );
    }
}
