import { Injectable } from "@angular/core";
import { HttpClient, HttpResponse, HttpContext, HttpHeaders } from "@angular/common/http";
import { BaseService } from "../base-service";
import { Observable, of } from 'rxjs';
import { ApiConfiguration } from "../api-configuration";
import { StrictHttpResponse } from "../strict-http-response";
import { RequestBuilder } from "../request-builder";
import { map, filter } from "rxjs/operators";
import { UserLoginRequestModel} from "src/app/api/models";
import { JwtHelperService, JwtModule } from "@auth0/angular-jwt";
import * as forge from 'node-forge';
import * as CryptoJS from 'crypto-js';
import { environment } from 'src/environments/environment.development'
import { ActivatedRoute, Router } from "@angular/router";
import { ChangePasswordRequestModel } from "../models/change-password-request-model";
declare const google: any;

@Injectable({
  providedIn: 'root'
})

export class AuthService extends BaseService {
  private googleLoaded: boolean = false;
  publicKeyUrl = "/User/"
  googleAuthUrl = "/Auth/google-login/"
  jwtHelper = new JwtHelperService();
  constructor(
    config: ApiConfiguration,
    http: HttpClient,
    route: ActivatedRoute,
    router: Router
  ) {
    super(config, http, route, router);
    this.loadGoogleAuthScript();
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

  getUserIdFromToken(): Observable<string | null> {
    const token = localStorage.getItem('token');
    if (token && !this.jwtHelper.isTokenExpired(token)) {
      const decodedToken = this.jwtHelper.decodeToken(token);
      return of(decodedToken.id);
    } else {
      return of(null);
    }
  }

  getUsernameFromToken(): Observable<string | null> {
    const token = localStorage.getItem('token');
    if (token && !this.jwtHelper.isTokenExpired(token)) {
      const decodedToken = this.jwtHelper.decodeToken(token);
      return of(decodedToken.display_name);
    } else {
      return of(null);
    }
  }

  private publicKeyCache = new Map<string, string>();
  private privateKeyCache = new Map<string, string>();

  async generateEncryptionKeys(id: string): Promise<void> {
    const keyPair = forge.pki.rsa.generateKeyPair({ bits: 2048, e: 0x10001 });
    const publicKey = forge.pki.publicKeyToPem(keyPair.publicKey);
    const privateKey = forge.pki.privateKeyToPem(keyPair.privateKey);  

    const encryptedPrivateKey = CryptoJS.AES.encrypt(privateKey, id).toString();

    localStorage.setItem('publicKey', publicKey);
    localStorage.setItem('EncryptedKey', encryptedPrivateKey);
    console.log(encryptedPrivateKey);
    const url = `${environment.apiUrl}${this.publicKeyUrl}${id}/keys`;
    await this.http.post(url, { PublicKey: publicKey, PrivateKey: encryptedPrivateKey }).toPromise();
  }

  clearKeys() {
    localStorage.removeItem('publicKey');
    localStorage.removeItem('EncryptedKey');
  }

  async encryptMessage(message: string, id: string): Promise<string> {
    if (!this.publicKeyCache.has(id)) {
      const url = `${environment.apiUrl}${this.publicKeyUrl}${id}/public-key`;
      const response: any = await this.http.get(url).toPromise();
      this.publicKeyCache.set(id, response.publicKey);
    }

    const recipientPublicKey = this.publicKeyCache.get(id)!;
    const publicKey = forge.pki.publicKeyFromPem(recipientPublicKey);
    const encrypted = publicKey.encrypt(message, 'RSA-OAEP', {
      md: forge.md.sha256.create(),
      mgf1: forge.mgf.mgf1.create(forge.md.sha1.create())
    });

    return forge.util.encode64(encrypted);
  }

  async decryptMessage(encryptedMessage: string, id: any): Promise<string> {

    if (!this.privateKeyCache.has(id)) {
      const url = `${environment.apiUrl}${this.publicKeyUrl}${id}/encrypted-key`;
      const response: any = await this.http.get(url).toPromise();
      const encryptedPrivateKey = response.encryptedKey;
      if (!encryptedPrivateKey) {
        throw new Error('Encrypted key not found in the database');
      }
      const privateKeyPem = this.decryptPrivateKey(encryptedPrivateKey, id);
      this.privateKeyCache.set(id, privateKeyPem);
    }

    const privateKeyPem = this.privateKeyCache.get(id)!;
    const privateKey = forge.pki.privateKeyFromPem(privateKeyPem);
    const encryptedBytes = forge.util.decode64(encryptedMessage);
    const decrypted = privateKey.decrypt(encryptedBytes, 'RSA-OAEP', {
      md: forge.md.sha256.create(),
      mgf1: forge.mgf.mgf1.create(forge.md.sha1.create())
    });
    return decrypted;
  }
  
  decryptPrivateKey(encryptedPrivateKey: string, passphrase: string): string {
  const bytes = CryptoJS.AES.decrypt(encryptedPrivateKey, passphrase);
    const decryptedPrivateKey = bytes.toString(CryptoJS.enc.Utf8);
    if (!decryptedPrivateKey)
      throw new Error('Failed to decrypt private key');
    return decryptedPrivateKey;
  }

  initializeGoogleAuth() {
    if (!this.googleLoaded) {
      console.error('Google API not yet loaded. Retrying...');
      setTimeout(() => this.initializeGoogleAuth(), 500);
      return;
    }

    google.accounts.id.initialize({
      client_id: environment.googleClientId,
      callback: (response: any) => this.handleCredentialResponse(response),
      auto_select: false,
      prompt_parent_id: "google-signin-container"
    });

    google.accounts.id.renderButton(
      document.getElementById("google-signin-container"),
      {
        theme: "outline",
        size: "large",
        text: "continue_with",
        shape: "rectangular"
      }
    );

    console.log("Google Sign-In initialized.");
  }

  handleCredentialResponse(response: any) {
    console.log('Google Token: ', response.credential);

    this.http
      .post(`${environment.apiUrl}${this.googleAuthUrl}`, { token: response.credential })
      .subscribe(
        (res: any) => {
          console.log('JWT Token: ', res.token);

          if (res.token) {
            localStorage.setItem('token', res.token);
            this.router.navigate(['/home']);
          } else {
            console.error('Google Auth failed: Token missing');
          }
         
        },
        (err) => {
          console.error('Google Auth failed', err);
        }
      );
  }

  private loadGoogleAuthScript() {
    const script = document.createElement('script');
    script.src = 'https://accounts.google.com/gsi/client';
    script.async = true;
    script.defer = true;
    script.onload = () => {
      this.googleLoaded = true;
    };
    document.head.appendChild(script);
  }

  private authChangePasswordPostPath = '/Auth/change-password/'

  public changePassword(userRequest: ChangePasswordRequestModel): Observable<void> {
    const url = `${environment.apiUrl}${this.authChangePasswordPostPath}`;
    console.log('Request URL:', url);
    return this.http.post<void>(url, userRequest);
  }
}
