import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, tap } from 'rxjs';

import { environment } from '../../environments/environment';

interface LoginResponse {
  token: string;
  user: {
    id: number;
    username: string;
    email: string;
    createdAt: string;
    isActive: boolean;
  };
}

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  private apiUrl: string = `${environment.apiUrl}/Users`;

  constructor(private http: HttpClient) {
  }

  login(username: string, password: string): Observable<LoginResponse> {

    return this.http.post<LoginResponse>(
      `${this.apiUrl}/login`,
      {
        username: username,
        password: password
      }
    ).pipe(
      tap((response: LoginResponse) => {
        localStorage.setItem('token', response.token);
        localStorage.setItem(
          'user',
          JSON.stringify(response.user)
        );
      })
    );
  }

  register(
    username: string,
    email: string,
    password: string
  ): Observable<any> {

    return this.http.post<any>(
      `${this.apiUrl}/register`,
      {
        username: username,
        email: email,
        password: password
      }
    );
  }

  logout(): void {
    localStorage.removeItem('token');
    localStorage.removeItem('user');
  }

  getToken(): string | null {
    return localStorage.getItem('token');
  }

  getUser(): LoginResponse['user'] | null {

    let userJson: string | null = localStorage.getItem('user');

    if (userJson === null) {
      return null;
    }

    return JSON.parse(userJson);
  }

  isLoggedIn(): boolean {
    return this.getToken() !== null;
  }
}
