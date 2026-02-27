import {
  HttpInterceptor,
  HttpRequest,
  HttpHandler,
  HttpEvent,
} from '@angular/common/http';
import { inject } from '@angular/core';
import { Observable } from 'rxjs';
import { catchError, switchMap } from 'rxjs/operators';
import { throwError } from 'rxjs';
import { HttpErrorResponse } from '@angular/common/http';
import { Router } from '@angular/router';
import { AuthService } from '../_services/AuthService';

export const jwtInterceptor: HttpInterceptor = {
  intercept: (
    req: HttpRequest<any>,
    next: HttpHandler,
  ): Observable<HttpEvent<any>> => {

    // Retrive token, clone request, and add authorization header
    const auth = inject(AuthService);
    const router = inject(Router);
    const token = auth.getAccessToken();

    const authReq = token
      ? req.clone({
          setHeaders: {
            Authorization: `Bearer ${token}`,
          },
        })
      : req;

    return next.handle(authReq).pipe(

      catchError((err: HttpErrorResponse) => {
        // server return 401 & not authentication request
        if (err.status === 401 && !req.url.includes('/auth/')) {
          return auth.refreshToken().pipe(
            switchMap(() => {

              // get a new token
              const newToken = auth.getAccessToken();

              // clone original request and attact new, retry the request
              const retried = req.clone({
                setHeaders: { Authorization: `Bearer ${newToken}` },
              });

              // send HTTP request with the access token
              return next.handle(retried);
            }),

            // catch the errors from the request
            catchError((refreshErr) => {
              auth.logout();
              return throwError(() => refreshErr);
            }),
          );
        }
        return throwError(() => err);
      }),
    );
  },
};
