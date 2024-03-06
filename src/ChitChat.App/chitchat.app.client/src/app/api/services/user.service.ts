import { Injectable } from "@angular/core";
import { HttpClient, HttpResponse, HttpContext } from "@angular/common/http";
import { BaseService } from "../base-service";
import { Observable } from 'rxjs';
import { ApiConfiguration } from "../api-configuration";
import { StrictHttpResponse } from "../strict-http-response";
import { RequestBuilder } from "../request-builder";
import { map, filter } from "rxjs/operators";
import { UserRequestModel, UserResponseModel } from "../models";

@Injectable({
    providedIn: 'root'
})

export class UserService extends BaseService {
    constructor(
        config: ApiConfiguration,
        http: HttpClient
    ) {
        super(config, http);
    }

    static readonly UserIdGetPath = '/User/{id}';
    userIdGet$Plain$Response(params: {
        id: string;
    },
        context?: HttpContext

    ): Observable<StrictHttpResponse<UserResponseModel>> {

        const rb = new RequestBuilder(this.rootUrl, UserService.UserIdGetPath, 'get');
        if (params) {
            rb.path('id', params.id, {});
        }

        return this.http.request(rb.build({
            responseType: 'text',
            accept: 'text/plain',
            context: context
        })).pipe(
            filter((r: any) => r instanceof HttpResponse),
            map((r: HttpResponse<any>) => {
                return r as StrictHttpResponse<UserResponseModel>;
            })
        );
    }

    userIdGet$Plain(params: {
        id: string;
    },
        context?: HttpContext

    ): Observable<UserResponseModel> {

        return this.userIdGet$Plain$Response(params, context).pipe(
            map((r: StrictHttpResponse<UserResponseModel>) => r.body as UserResponseModel)
        );
    }

    userIdGet$Response(params: {
        id: string;
    },
        context?: HttpContext

    ): Observable<StrictHttpResponse<UserResponseModel>> {

        const rb = new RequestBuilder(this.rootUrl, UserService.UserIdGetPath, 'get');
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
                return r as StrictHttpResponse<UserResponseModel>;
            })
        );
    }

    userIdGet(params: {
        id: string;
    },
        context?: HttpContext

    ): Observable<UserResponseModel> {

        return this.userIdGet$Response(params, context).pipe(
            map((r: StrictHttpResponse<UserResponseModel>) => r.body as UserResponseModel)
        );
    }

    static readonly UserGetPath = '/User';
    userGet$Plain$Response(params?: {
    },
        context?: HttpContext

    ): Observable<StrictHttpResponse<Array<UserResponseModel>>> {

        const rb = new RequestBuilder(this.rootUrl, UserService.UserGetPath, 'get');
        if (params) {
        }

        return this.http.request(rb.build({
            responseType: 'text',
            accept: 'text/plain',
            context: context
        })).pipe(
            filter((r: any) => r instanceof HttpResponse),
            map((r: HttpResponse<any>) => {
                return r as StrictHttpResponse<Array<UserResponseModel>>;
            })
        );
    }

    userGet$Plain(params?: {
    },
        context?: HttpContext

    ): Observable<Array<UserResponseModel>> {

        return this.userGet$Plain$Response(params, context).pipe(
            map((r: StrictHttpResponse<Array<UserResponseModel>>) => r.body as Array<UserResponseModel>)
        );
    }

    userGet$Response(params?: {
    },
        context?: HttpContext

    ): Observable<StrictHttpResponse<Array<UserResponseModel>>> {

        const rb = new RequestBuilder(this.rootUrl, UserService.UserGetPath, 'get');
        if (params) {
        }

        return this.http.request(rb.build({
            responseType: 'json',
            accept: 'text/json',
            context: context
        })).pipe(
            filter((r: any) => r instanceof HttpResponse),
            map((r: HttpResponse<any>) => {
                return r as StrictHttpResponse<Array<UserResponseModel>>;
            })
        );
    }

    userGet(params?: {
    },
        context?: HttpContext

    ): Observable<Array<UserResponseModel>> {

        return this.userGet$Response(params, context).pipe(
            map((r: StrictHttpResponse<Array<UserResponseModel>>) => r.body as Array<UserResponseModel>)
        );
    }

    static readonly UserPostPath = '/User';
    userPost$Response(params?: {
        body?: UserRequestModel
    },
        context?: HttpContext

    ): Observable<StrictHttpResponse<void>> {

        const rb = new RequestBuilder(this.rootUrl, UserService.UserPostPath, 'post');
        if (params) {
            rb.body(params.body, 'application/*+json');
        }

        return this.http.request(rb.build({
            responseType: 'text',
            accept: '*/*',
            context: context
        })).pipe(
            filter((r: any) => r instanceof HttpResponse),
            map((r: HttpResponse<any>) => {
                return (r as HttpResponse<any>).clone({ body: undefined }) as StrictHttpResponse<void>;
            })
        );
    }

    userPost(params?: {
        body?: UserRequestModel
    },
        context?: HttpContext

    ): Observable<void> {

        return this.userPost$Response(params, context).pipe(
            map((r: StrictHttpResponse<void>) => r.body as void)
        );
    }

    static readonly UserDeletePath = '/User/{id}';
    userDelete$Response(params: {
        id: string;
    },
        context?: HttpContext

    ): Observable<StrictHttpResponse<void>> {

        const rb = new RequestBuilder(this.rootUrl, UserService.UserDeletePath, 'delete');
        if (params) {
            rb.body(params.id, 'application/*+json');
        }

        return this.http.request(rb.build({
            responseType: 'text',
            accept: '*/*',
            context: context
        })).pipe(
            filter((r: any) => r instanceof HttpResponse),
            map((r: HttpResponse<any>) => {
                return (r as HttpResponse<any>).clone({ body: undefined }) as StrictHttpResponse<void>;
            })
        );
    }

    userDelete(params: {
        id: string
    },
        context?: HttpContext

    ): Observable<void> {

        return this.userDelete$Response(params, context).pipe(
            map((r: StrictHttpResponse<void>) => r.body as void)
        );
    }

    static readonly UserPutPath = '/User/{id}';
    userPut$Response(params: {
        id: string,
        body?: UserRequestModel
    },
        context?: HttpContext

    ): Observable<StrictHttpResponse<UserResponseModel>> {
        const rb = new RequestBuilder(this.rootUrl, UserService.UserPutPath, 'put');
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
                return r as StrictHttpResponse<UserResponseModel>;
            })
        );
    }

    userPut(params: {
        id: string,
        body?: UserRequestModel
    },
        context: HttpContext
    ): Observable<UserResponseModel> {

        return this.userPut$Response(params, context).pipe(
            map((r: StrictHttpResponse<UserResponseModel>) => r.body as UserResponseModel)
        );
    }
}
