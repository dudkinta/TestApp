import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Country, Province } from '../models/location.models';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class LocationService {
  private baseUrl = environment.apiLocationBaseUrl;
  private countryUrl = `${this.baseUrl}/api/country`;
  private provinceUrl = `${this.baseUrl}/api/province`;

  constructor(private http: HttpClient) {}

  getCountries(): Observable<Country[]> {
    return this.http.get<Country[]>(this.countryUrl);
  }

  getProvinces(countryId: string): Observable<Province[]> {
    return this.http.get<Province[]>(`${this.provinceUrl}/${countryId}`);
  }
}