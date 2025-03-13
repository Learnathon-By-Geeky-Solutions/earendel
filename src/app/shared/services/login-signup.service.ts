import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { ApiService } from '../../core/service/api.service';
import { Observable } from 'rxjs';
import { endpoint } from '../../endpoints/endpoint';

@Injectable({
  providedIn: 'root',
})
export class LoginSignupService {
  googleLoginUrl = endpoint.googleLoginUrl;

  constructor(private http: HttpClient, private apiService: ApiService) {}

  googleLogin(token: string): Observable<any> {
    console.log(token);
    return this.http.post(
      this.googleLoginUrl.trim(),
      { token: token },
      {
        headers: new HttpHeaders({
          'Content-Type': 'application/json',
          tenant: 'root',
        }),
      }
    );
  }
}
