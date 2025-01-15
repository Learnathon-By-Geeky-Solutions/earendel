import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { ApiService } from '../../core/service/api.service';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class HomeService {
  public weather_url = 'http://localhost:5214/weatherforecast';

  constructor(private readonly httpClient: HttpClient, private readonly apiService: ApiService) {}
  weatherData(): Observable<any> {
    return this.apiService.get(this.weather_url);
  }
}
