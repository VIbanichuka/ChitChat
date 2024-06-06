import { Injectable } from '@angular/core';
import {HttpRequest,HttpHandler,HttpEvent,HttpInterceptor,HttpErrorResponse} from '@angular/common/http';
import { catchError, Observable, throwError } from "rxjs";
import { Router } from '@angular/router';

@Injectable()
export class ErrorInterceptor implements HttpInterceptor {

  constructor(private router: Router) {}

  intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    return next.handle(request).pipe(
      catchError((error: HttpErrorResponse) => {
        switch (error.status) {
          case 401:
            console.error('Unauthorized:', error.message);
            break;
          case 403:
            console.error('Forbidden:', error.message);
            break;
          case 404:
            console.error('Not Found:', error.message);
            break;
          case 500:
            this.router.navigate(['/500']);
            console.error('Internal Server Error:', error.message);
            break;
          case 502:
            console.error('Bad Gateway:', error.message);
            break;
          case 503:
            console.error('Service Unavailable:', error.message);
            break;
          default:
            console.error('Internal Server Error:', error.message);
            break;
        }
        return throwError(error);
      })
    );
  }
}
