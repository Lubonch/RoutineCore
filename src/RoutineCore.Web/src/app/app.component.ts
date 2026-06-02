import { Component, inject, computed } from '@angular/core';
import { Router, RouterLink, RouterLinkActive, RouterOutlet } from '@angular/router';
import { CommonModule } from '@angular/common';
import { AuthService } from './services/auth.service';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [CommonModule, RouterOutlet, RouterLink, RouterLinkActive],
  template: `
    <div style="font-family: 'Segoe UI', Roboto, sans-serif; background-color: #f4f6f9; min-height: 100vh; display: flex; flex-direction: column; margin: 0;">
      <!-- Header / Navbar -->
      <header *ngIf="user()" style="background-color: #1e293b; color: white; padding: 1rem 2rem; display: flex; justify-content: space-between; align-items: center; box-shadow: 0 4px 6px -1px rgba(0, 0, 0, 0.1);">
        <div style="font-size: 1.5rem; font-weight: bold; letter-spacing: 0.05em; display: flex; align-items: center; gap: 0.5rem;">
          <span style="color: #6366f1;">⚡</span> RoutineCore
        </div>
        
        <nav style="display: flex; gap: 1.5rem; align-items: center;">
          <a routerLink="/dashboard" routerLinkActive="active" [routerLinkActiveOptions]="{exact: true}" class="nav-btn">Dashboard</a>
          <a routerLink="/punch" routerLinkActive="active" class="nav-btn">Punches</a>
          <a routerLink="/absence" routerLinkActive="active" class="nav-btn">Absences</a>
          <a *ngIf="isAdmin()" routerLink="/schedule" routerLinkActive="active" class="nav-btn">Scheduling</a>
        </nav>

        <div style="display: flex; align-items: center; gap: 1rem;">
          <div style="text-align: right;">
            <div style="font-weight: 600; font-size: 0.95rem;">{{ user()?.name }}</div>
            <div style="font-size: 0.75rem; color: #94a3b8; text-transform: uppercase;">{{ user()?.role }}</div>
          </div>
          <button (click)="onLogout()" style="background-color: #ef4444; border: none; color: white; padding: 0.5rem 1rem; border-radius: 4px; font-weight: 600; cursor: pointer; transition: background-color 0.2s;" onmouseover="this.style.backgroundColor='#dc2626'" onmouseout="this.style.backgroundColor='#ef4444'">
            Logout
          </button>
        </div>
      </header>

      <!-- Main Content Container -->
      <main style="flex: 1; padding: 2rem; max-width: 1200px; width: 100%; margin: 0 auto; box-sizing: border-box;">
        <router-outlet></router-outlet>
      </main>

      <!-- Footer -->
      <footer style="text-align: center; padding: 1.5rem; color: #64748b; font-size: 0.85rem; border-top: 1px solid #e2e8f0; background-color: #fff;">
        RoutineCore Pulse System &copy; 2026 - Modern Decoupled Solution
      </footer>
    </div>
  `,
  styles: [`
    .nav-btn {
      color: #94a3b8;
      text-decoration: none;
      font-weight: 500;
      font-size: 0.95rem;
      padding: 0.5rem 0.75rem;
      border-radius: 4px;
      transition: all 0.2s;
    }
    .nav-btn:hover {
      color: #f1f5f9;
      background-color: #334155;
    }
    .nav-btn.active {
      color: white;
      background-color: #4f46e5;
    }
  `]
})
export class AppComponent {
  private authService = inject(AuthService);
  private router = inject(Router);

  user = computed(() => this.authService.currentUser());
  isAdmin = computed(() => this.authService.hasRole(['Admin', 'Manager']));

  onLogout(): void {
    this.authService.logout();
    this.router.navigate(['/login']);
  }
}
