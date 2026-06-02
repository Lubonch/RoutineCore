import { Component, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HttpClient } from '@angular/common/http';
import { AuthService } from '../../services/auth.service';

interface PunchResult {
  id: string;
  employeeId: string;
  punchTime: string;
  direction: string;
  deviceCode: string;
  processed: boolean;
}

@Component({
  selector: 'app-punch',
  standalone: true,
  imports: [CommonModule],
  template: `
    <div style="max-width: 600px; margin: 0 auto;">
      <div style="background-color: white; padding: 2rem; border-radius: 8px; box-shadow: 0 4px 6px -1px rgba(0,0,0,0.1); border: 1px solid #e2e8f0;">
        <h2 style="margin: 0 0 0.5rem; color: #1e293b;">Register Attendance Punch</h2>
        <p style="margin: 0 0 2rem; color: #64748b; font-size: 0.95rem;">
          Punching records your exact check-in or check-out timestamp in the **RoutineCore** database.
        </p>

        <!-- Dynamic Feedback Message of Asynchronous Integration -->
        <div style="background-color: #f0fdf4; border: 1px solid #bbf7d0; padding: 1rem; border-radius: 6px; margin-bottom: 2rem; display: flex; gap: 0.75rem; align-items: flex-start;">
          <span style="font-size: 1.25rem;">ℹ️</span>
          <div>
            <strong style="color: #166534; font-size: 0.85rem; display: block; margin-bottom: 0.25rem;">Integration Pipeline Active:</strong>
            <p style="margin: 0; color: #14532d; font-size: 0.82rem; line-height: 1.4;">
              When you punch, the API fires a <code>PunchRegisteredEvent</code>. Our RabbitMQ messaging system forwards this down the line to <strong>PulseDispatcher</strong> (Worker Service), executing live modern dispatcher workflows in real-time.
            </p>
          </div>
        </div>

        <div style="display: flex; gap: 1.5rem; justify-content: center; margin-bottom: 2rem;">
          <button (click)="onPunch('In')" [disabled]="loading()" class="punch-btn in">
            Check-In (Entrada)
          </button>
          
          <button (click)="onPunch('Out')" [disabled]="loading()" class="punch-btn out">
            Check-Out (Salida)
          </button>
        </div>

        <div *ngIf="lastPunch()" style="background-color: #f8fafc; border: 1px solid #cbd5e1; padding: 1.25rem; border-radius: 6px;">
          <h4 style="margin: 0 0 0.75rem; color: #334155; font-size: 0.95rem;">Last Registered Punch Confirmation</h4>
          <div style="display: grid; grid-template-columns: 1fr 1fr; gap: 0.5rem; font-size: 0.85rem; color: #475569;">
            <div><strong>Punch ID:</strong> {{ lastPunch()?.id }}</div>
            <div><strong>Direction:</strong> <span [class]="'badge ' + lastPunch()?.direction?.toLowerCase()">{{ lastPunch()?.direction }}</span></div>
            <div><strong>Timestamp:</strong> {{ lastPunch()?.punchTime | date:'medium' }}</div>
            <div><strong>Device Registered:</strong> {{ lastPunch()?.deviceCode }}</div>
          </div>
        </div>
      </div>
    </div>
  `,
  styles: [`
    .punch-btn {
      flex: 1;
      padding: 1.25rem;
      font-size: 1.1rem;
      font-weight: 700;
      border: none;
      border-radius: 6px;
      cursor: pointer;
      color: white;
      transition: opacity 0.2s, transform 0.1s;
    }
    .punch-btn:hover {
      opacity: 0.9;
    }
    .punch-btn:active {
      transform: scale(0.98);
    }
    .punch-btn.in {
      background-color: #10b981;
    }
    .punch-btn.out {
      background-color: #ef4444;
    }
    .badge {
      font-size: 0.75rem;
      font-weight: 700;
      padding: 0.15rem 0.4rem;
      border-radius: 4px;
      text-transform: uppercase;
    }
    .badge.in {
      background-color: #dcfce7;
      color: #15803d;
    }
    .badge.out {
      background-color: #fee2e2;
      color: #b91c1c;
    }
  `]
})
export class PunchComponent {
  private http = inject(HttpClient);
  private authService = inject(AuthService);

  loading = signal(false);
  lastPunch = signal<PunchResult | null>(null);

  onPunch(direction: string): void {
    const user = this.authService.currentUser();
    if (!user) return;

    this.loading.set(true);

    this.http.post<PunchResult>('/api/v1/punches/register', {
      employeeId: user.id,
      direction: direction,
      deviceCode: 'Web browser'
    }).subscribe({
      next: (res) => {
        this.loading.set(false);
        this.lastPunch.set(res);
      },
      error: () => this.loading.set(false)
    });
  }
}
//
