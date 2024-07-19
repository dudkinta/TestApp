import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, from, of } from 'rxjs';
import { switchMap, catchError, map } from 'rxjs/operators';
import { User } from '../models/user.models';
import { ConsulService } from './consul.service';

@Injectable({
  providedIn: 'root'
})
export class UserService {
  private baseUrl: string = '';
  private userEndpoint: string = '';
  private isInitialized: boolean = false;

  constructor(private http: HttpClient, private consulService: ConsulService) {
    this.initializeUrls().subscribe();
  }
  private initializeUrls(): Observable<boolean> {
    return this.consulService.getService('RegistrationService').pipe(
      map(service => {
        this.baseUrl = `http://${service.Address}:${service.Port}`;
        this.userEndpoint = `${this.baseUrl}/api/user`;
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

  sendUserData(userData: User): Observable<any> {
    return this.ensureInitialized().pipe(
      switchMap(() => this.http.post<any>(this.userEndpoint, userData))
    );
  }
}