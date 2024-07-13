import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';
import { User } from '../models/user.models';

@Injectable({
  providedIn: 'root'
})
export class UserService {
    private baseUrl = environment.apiUserBaseUrl;
    private userEndpoint = `${this.baseUrl}/api/user`;
  
  constructor(private http: HttpClient) {}

  sendUserData(userData: User): Observable<any> {
    return this.http.post<any>(this.userEndpoint, userData);
  }
}