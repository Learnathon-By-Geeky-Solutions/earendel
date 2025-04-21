import { Injectable } from '@angular/core';
import { endpoint } from '../../endpoints/endpoint';
import { HttpClient } from '@angular/common/http';
import { ApiService } from '../../core/service/api.service';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class HrService {
  hrDetailsUrl = `${endpoint.hrDetailsUrl}`;

  constructor(
    private readonly http: HttpClient,
    private readonly apiService: ApiService
  ) {}

  hrDetailsData(
    pageNumber = 1,
    pageSize = 10,
    search = '',
    sortBy = null,
    sortDirection = null
  ): Observable<any> {
    const fetchUrl = `${this.hrDetailsUrl}?search=${search}&sortBy=${sortBy}&sortDirection=${sortDirection}&pageNumber=${pageNumber}&pageSize=${pageSize}`;
    return this.apiService.get(fetchUrl);
  }
}
