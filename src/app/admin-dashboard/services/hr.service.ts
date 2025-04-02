import { Injectable } from '@angular/core';
import { endpoint } from '../../endpoints/endpoint';
import { HttpClient } from '@angular/common/http';
import { ApiService } from '../../core/service/api.service';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class HrService {
  hrDetailsUrl = `${endpoint.hrDetailsUrl}?pageNumber=1&pageSize=10`;

  constructor(
    private readonly http: HttpClient,
    private readonly apiService: ApiService
  ) {}

  hrDetailsData(): Observable<any> {
    return this.apiService.get(this.hrDetailsUrl);
  }
}
