import { Component, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { AuthService } from '../../services/auth.service';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [CommonModule, FormsModule],
  template: `
    <div style="display: flex; justify-content: center; align-items: center; min-height: 70vh;">
      <div style="background-color: white; padding: 2.5rem; border-radius: 8px; box-shadow: 0 10px 15px -3px rgba(0,0,0,0.1); width: 100%; max-width: 400px; box-sizing: border-box;">
        <div style="text-align: center; margin-bottom: 2rem;">
          <h1 style="margin: 0; color: #1e293b; font-size: 2rem; font-weight: 800;">RoutineCore</h1>
          <p style="margin: 0.5rem 0 0; color: #64748b; font-size: 0.9rem;">Modern Operative Routine & Attendance</p>
        </div>

        <form (ngSubmit)="onSubmit()" #loginForm="ngForm" style="display: flex; flex-direction: column; gap: 1.25rem;">
          <div>
            <label style="display: block; font-weight: 500; font-size: 0.85rem; color: #475569; margin-bottom: 0.5rem;">Corporate Email</label>
            <input type="email" name="email" [(ngModel)]="email" required placeholder="name@routinecore.com" style="width: 100%; padding: 0.75rem; border: 1px solid #cbd5e1; border-radius: 4px; box-sizing: border-box; font-size: 0.95rem; outline: none; transition: border-color 0.2s;" onfocus="this.style.borderColor='#6366f1'">
          </div>

          <div>
            <label style="display: block; font-weight: 500; font-size: 0.85rem; color: #475569; margin-bottom: 0.5rem;">Employee Code</label>
            <input type="password" name="employeeCode" [(ngModel)]="employeeCode" required placeholder="EMP123" style="width: 100%; padding: 0.75rem; border: 1px solid #cbd5e1; border-radius: 4px; box-sizing: border-box; font-size: 0.95rem; outline: none; transition: border-color 0.2s;" onfocus="this.style.borderColor='#6366f1'">
          </div>

          <div *ngIf="error()" style="background-color: #fef2f2; border: 1px solid #fee2e2; color: #ef4444; padding: 0.75rem; border-radius: 4px; font-size: 0.85rem; line-height: 1.25;">
            {{ error() }}
          </div>

          <button type="submit" [disabled]="loading() || !loginForm.form.valid" style="background-color: #4f46e5; border: none; color: white; padding: 0.75rem; border-radius: 4px; font-weight: 600; font-size: 1rem; cursor: pointer; transition: background-color 0.2s; display: flex; justify-content: center; align-items: center;" onmouseover="this.style.backgroundColor='#4338ca'" onmouseout="this.style.backgroundColor='#4f46e5'">
            <span *ngIf="!loading()">Login</span>
            <span *ngIf="loading()">Loading...</span>
          </button>
        </form>

        <div style="margin-top: 1.5rem; text-align: center; font-size: 0.8rem; color: #94a3b8; border-top: 1px solid #f1f5f9; padding-top: 1rem;">
          Default Admin: admin&#64;routinecore.com (EMP001)<br>
          Default Operator: operator&#64;routinecore.com (EMP002)
        </div>
      </div>
    </div>
  `
})
export class LoginComponent {
  private authService = inject(AuthService);
  private router = inject(Router);

  email = '';
  employeeCode = '';
  
  loading = signal(false);
  error = signal<string | null>(null);

  onSubmit(): void {
    if (!this.email || !this.employeeCode) return;

    this.loading.set(true);
    this.error.set(null);

    this.authService.login(this.email, this.employeeCode).subscribe({
      next: () => {
        this.router.navigate(['/dashboard']);
      },
      error: (err) => {
        this.loading.set(false);
        this.error.set(err.error?.message || 'Failed to authenticate. Check credentials.');
      }
    });
  }
}
