import { HttpInterceptorFn } from '@angular/common/http';

export const HttpTokenInterceptor: HttpInterceptorFn = (req, next) => {
  const headersConfig: any = {
    Accept: 'application/json',
  };

  const idToken = localStorage.getItem('token');

  if (idToken) {
    headersConfig['Authorization'] = idToken;
    const clonedRequest = req.clone({
      setHeaders: headersConfig,
    });
    return next(clonedRequest);
  }

  return next(req);
};
