import {
  HttpInterceptorFn,
  HttpErrorResponse,
  HttpClient,
} from '@angular/common/http';
import { inject } from '@angular/core';
import { Router } from '@angular/router';
import { catchError, switchMap, throwError } from 'rxjs';
import { endpoint } from '../../endpoints/endpoint';

export const HttpTokenInterceptor: HttpInterceptorFn = (req, next) => {
  const http = inject(HttpClient);
  const router = inject(Router);

  // 1) Don’t intercept refresh‑token calls or login endpoints
  if (
    req.url.includes(endpoint.refreshTokenUrl) ||
    req.url.includes('/login')
  ) {
    return next(req);
  }

  // 2) Build headers with the current access token
  const sessionData = sessionStorage.getItem('loggedInUser');
  const user = sessionData ? JSON.parse(sessionData) : null;
  const accessToken = user?.token;
  const refreshToken = user?.refreshToken;
  let headersConfig: any = { Accept: 'application/json' };

  if (accessToken) {
    headersConfig['Authorization'] = `Bearer ${accessToken}`;
  }
  headersConfig['tenant'] = 'root';

  const authReq = req.clone({ setHeaders: headersConfig });

  return next(authReq).pipe(
    catchError((err: HttpErrorResponse) => {
      // 3) If 401, try refreshing
      if (err.status === 401 && refreshToken) {
        return http
          .post<any>(endpoint.refreshTokenUrl, {
            token: accessToken,
            refreshToken,
          })
          .pipe(
            switchMap((newTokens) => {
              // 4) Store new tokens
              sessionStorage.setItem('loggedInUser', JSON.stringify(newTokens));

              // 5) Retry original request with updated access token
              const retryHeaders = {
                ...headersConfig,
                Authorization: `Bearer ${newTokens.token}`,
              };
              const retryReq = req.clone({ setHeaders: retryHeaders });
              return next(retryReq);
            }),
            catchError((refreshErr) => {
              // 6) Refresh failed: clear session and force login
              sessionStorage.removeItem('loggedInUser');
              router.navigate(['/login']);
              return throwError(() => refreshErr);
            })
          );
      }

      // If not a 401 or no refreshToken, just rethrow
      return throwError(() => err);
    })
  );
};
