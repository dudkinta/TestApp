import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, from, of } from 'rxjs';
import { switchMap, catchError, map } from 'rxjs/operators';
import { Country, Province } from '../models/location.models';
import { ConsulService } from './consul.service';

@Injectable({
  providedIn: 'root'
})
export class LocationService {
  private baseUrl: string = '';
  private countryUrl: string = '';
  private provinceUrl: string = '';
  private isInitialized: boolean = false;

  constructor(private http: HttpClient, private consulService: ConsulService) {
    this.initializeUrls().subscribe();
  }

  private initializeUrls(): Observable<boolean> {
    return this.consulService.getService('LocationService').pipe(
      map(service => {
        this.baseUrl = `http://${service.Address}:${service.Port}`;
        this.countryUrl = `${this.baseUrl}/api/country`;
        this.provinceUrl = `${this.baseUrl}/api/province`;
        this.isInitialized = true;
        return true;
      }),
      catchError(error => {
        console.error('Failed to initialize URLs', error);
        return of(false);
      })
    );
  }

  private ensureInitialized(): Observable<void> {
    if (this.isInitialized) {
      return of(void 0);
    } else {
      return this.initializeUrls().pipe(map(() => void 0));
    }
  }

  getCountries(): Observable<Country[]> {
    return this.ensureInitialized().pipe(
      switchMap(() => this.http.get<Country[]>(this.countryUrl))
    );
  }

  getProvinces(countryId: string): Observable<Province[]> {
    return this.ensureInitialized().pipe(
      switchMap(() => this.http.get<Province[]>(`${this.provinceUrl}/${countryId}`))
    );
  }
}