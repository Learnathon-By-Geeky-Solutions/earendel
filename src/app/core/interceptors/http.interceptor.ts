import { HttpInterceptorFn } from '@angular/common/http';

export const HttpTokenInterceptor: HttpInterceptorFn = (req, next) => {
  // Default headers
  let headersConfig: any = {
    Accept: 'application/json',
  };

  // Fetch the token from localStorage
  const idToken = localStorage.getItem('token');

  // If token is present, add Authorization header
  if (idToken) {
    headersConfig['Authorization'] = `Bearer ${idToken}`; // Ensure correct format
  }

  // Add tenant header (this could be static or dynamic based on your logic)
  const tenant = 'root'; // You can replace this with dynamic logic if needed (e.g., from sessionStorage or other sources)
  headersConfig['tenant'] = tenant;

  // Clone the request with the new headers
  const clonedRequest = req.clone({
    setHeaders: headersConfig,
  });

  // Proceed with the modified request
  return next(clonedRequest);
};
