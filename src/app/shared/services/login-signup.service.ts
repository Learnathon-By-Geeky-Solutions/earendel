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
  userRegistrationUrl = endpoint.userRegistrationUrl;
  userLoginUrl = endpoint.userLoginUrl;

  constructor(private http: HttpClient, private apiService: ApiService) {}

  googleLogin(token: string): Observable<any> {
    return this.http.post(this.googleLoginUrl.trim(), { token: token });
  }
  userRegistration(registrationData: any): Observable<any> {
    return this.http.post(this.userRegistrationUrl.trim(), registrationData);
  }

  userLogin(loginData: any): Observable<any> {
    return this.http.post(this.userLoginUrl.trim(), loginData);
  }
}
