import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class ConsulService {
  private consulUrl = '/consul/v1/agent/services';

  constructor(private http: HttpClient) {}

  getServices(): Observable<any> {
    return this.http.get<any>(this.consulUrl);
  }

  getService(serviceName: string): Observable<any> {
    return this.http.get<any>(this.consulUrl).pipe(
      map(services => {
        const service = Object.values(services).find((service: any) => service.Service === serviceName);
        if (!service) {
          throw new Error(`Service ${serviceName} not found`);
        }
        return service;
      })
    );
  }
}