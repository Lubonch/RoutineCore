import { Injectable, inject, signal } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, tap } from 'rxjs';

export interface Employee {
  id: string;
  name: string;
  email: string;
  role: string;
  employeeCode: string;
  isActive: boolean;
}

export interface AuthResponse {
  token: string;
  employee: Employee;
}

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private http = inject(HttpClient);
  private apiUrl = '/api/v1/auth';

  // Standalone Signals for modern Angular state management
  currentUser = signal<Employee | null>(null);

  constructor() {
    this.restoreSession();
  }

  login(email: string, employeeCode: string): Observable<AuthResponse> {
    return this.http.post<AuthResponse>(`${this.apiUrl}/login`, { email, employeeCode }).pipe(
      tap((res) => {
        localStorage.setItem('routine_token', res.token);
        localStorage.setItem('routine_user', JSON.stringify(res.employee));
        this.currentUser.set(res.employee);
      })
    );
  }

  logout(): void {
    localStorage.removeItem('routine_token');
    localStorage.removeItem('routine_user');
    this.currentUser.set(null);
  }

  restoreSession(): void {
    const token = localStorage.getItem('routine_token');
    const userJson = localStorage.getItem('routine_user');
    if (token && userJson) {
      try {
        this.currentUser.set(JSON.parse(userJson));
      } catch {
        this.logout();
      }
    }
  }

  isAuthenticated(): boolean {
    return this.currentUser() !== null;
  }

  hasRole(roles: string[]): boolean {
    const user = this.currentUser();
    if (!user) return false;
    return roles.includes(user.role);
  }
}
//
