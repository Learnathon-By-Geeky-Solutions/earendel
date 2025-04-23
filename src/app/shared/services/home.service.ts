import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { ApiService } from '../../core/service/api.service';
import { Observable } from 'rxjs';
import { endpoint } from '../../endpoints/endpoint';

@Injectable({
  providedIn: 'root',
})
export class HomeService {
  public weather_url = 'http://localhost:5214/weatherforecast';

  public getSignatureApi = `${endpoint.getZoomSignatureUrl}`;

  public judgeapi_url =
    'https://judge0-ce.p.rapidapi.com/submissions?wait=true';

  constructor(
    private readonly http: HttpClient,
    private readonly apiService: ApiService
  ) {}

  weatherData(): Observable<any> {
    return this.apiService.get(this.weather_url);
  }

  codePost(body: any): Observable<any> {
    const headers = new HttpHeaders({
      'Content-Type': 'application/json',
      'X-RapidAPI-Host': 'judge0-ce.p.rapidapi.com',
      'X-RapidAPI-Key': 'd68459c4aemshfc0db4f351d492bp187015jsn5839c751ecf2',
    });

    return this.apiService.post(this.judgeapi_url, body, { headers });
  }

  getZoomSignature(signatureData: any): Observable<any> {
    console.log(signatureData);
    return this.http.post(this.getSignatureApi.trim(), signatureData);
  }
}
