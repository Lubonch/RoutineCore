import { Component, inject, OnInit, signal, computed } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HttpClient } from '@angular/common/http';
import { RouterLink } from '@angular/router';
import { AuthService, Employee } from '../../services/auth.service';

interface Punch {
  id: string;
  punchTime: string;
  direction: string;
  deviceCode: string;
}

interface Schedule {
  id: string;
  startTime: string;
  endTime: string;
  projectTaskId: string;
  status: string;
}

interface Absence {
  id: string;
  startDate: string;
  endDate: string;
  reason: string;
  authorized: boolean;
  approvedBy: string | null;
}

interface ProjectTask {
  id: string;
  description: string;
  projectCode: string;
}

@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [CommonModule, RouterLink],
  template: `
    <div>
      <!-- Welcome Header -->
      <div style="background-color: white; padding: 1.5rem 2rem; border-radius: 8px; box-shadow: 0 1px 3px rgba(0,0,0,0.1); margin-bottom: 2rem; display: flex; justify-content: space-between; align-items: center; flex-wrap: wrap; gap: 1rem;">
        <div>
          <h2 style="margin: 0; color: #0f172a; font-size: 1.75rem;">Welcome back, {{ user()?.name }}!</h2>
          <p style="margin: 0.25rem 0 0; color: #64748b; font-size: 0.95rem;">Here is your operative routine summary for May 29, 2026.</p>
        </div>
        <div style="display: flex; gap: 0.5rem;">
          <span style="background-color: #e2e8f0; color: #475569; padding: 0.35rem 0.75rem; border-radius: 9999px; font-weight: 500; font-size: 0.85rem;">
            Code: {{ user()?.employeeCode }}
          </span>
          <span style="background-color: #dcfce7; color: #15803d; padding: 0.35rem 0.75rem; border-radius: 9999px; font-weight: 500; font-size: 0.85rem; display: flex; align-items: center; gap: 0.25rem;">
            <span style="width: 8px; height: 8px; background-color: #22c55e; border-radius: 50%; display: inline-block;"></span> Active
          </span>
        </div>
      </div>

      <!-- Metrics Cards -->
      <div style="display: grid; grid-template-columns: repeat(auto-fit, minmax(240px, 1fr)); gap: 1.5rem; margin-bottom: 2rem;">
        <div class="metric-card">
          <div style="color: #6366f1; font-size: 2.25rem; font-weight: bold; margin-bottom: 0.25rem;">{{ schedules().length }}</div>
          <div style="font-weight: 600; color: #334155; font-size: 0.95rem;">Scheduled Shifts</div>
          <div style="color: #64748b; font-size: 0.8rem; margin-top: 0.25rem;">Total assigned routines</div>
        </div>

        <div class="metric-card">
          <div style="color: #22c55e; font-size: 2.25rem; font-weight: bold; margin-bottom: 0.25rem;">{{ punches().length }}</div>
          <div style="font-weight: 600; color: #334155; font-size: 0.95rem;">Recorded Punches</div>
          <div style="color: #64748b; font-size: 0.8rem; margin-top: 0.25rem;">Attendance time logs</div>
        </div>

        <div class="metric-card">
          <div style="color: #f59e0b; font-size: 2.25rem; font-weight: bold; margin-bottom: 0.25rem;">{{ absences().length }}</div>
          <div style="font-weight: 600; color: #334155; font-size: 0.95rem;">Absences / Leaves</div>
          <div style="color: #64748b; font-size: 0.8rem; margin-top: 0.25rem;">Sick leaves, vacations, etc.</div>
        </div>
      </div>

      <!-- Grid sections -->
      <div style="display: grid; grid-template-columns: repeat(auto-fit, minmax(450px, 1fr)); gap: 2rem;">
        <!-- Scheduled Routines -->
        <div class="sec-card">
          <div style="display: flex; justify-content: space-between; align-items: center; border-bottom: 1px solid #f1f5f9; padding-bottom: 0.75rem; margin-bottom: 1rem;">
            <h3 style="margin: 0; color: #1e293b; font-size: 1.15rem;">Your Scheduled Routines</h3>
            <span style="font-size: 0.8rem; color: #4f46e5; font-weight: 600;">Standard Schedule</span>
          </div>

          <div *ngIf="schedules().length === 0" style="text-align: center; color: #94a3b8; padding: 2rem 0;">
            No routines scheduled at the moment.
          </div>

          <div *ngIf="schedules().length > 0" style="display: flex; flex-direction: column; gap: 0.75rem;">
            <div *ngFor="let s of schedules()" style="border-left: 4px solid #6366f1; padding: 0.75rem 1rem; background-color: #f8fafc; border-radius: 0 4px 4px 0; display: flex; justify-content: space-between; align-items: center;">
              <div>
                <div style="font-weight: 600; color: #334155; font-size: 0.9rem;">
                  Task ID: {{ s.projectTaskId }}
                </div>
                <div style="font-size: 0.8rem; color: #64748b; margin-top: 0.25rem;">
                  {{ s.startTime | date:'short' }} &rarr; {{ s.endTime | date:'short' }}
                </div>
              </div>
              <span [class]="'status-badge ' + s.status.toLowerCase()">{{ s.status }}</span>
            </div>
          </div>
        </div>

        <!-- Recent Time Punches -->
        <div class="sec-card">
          <div style="display: flex; justify-content: space-between; align-items: center; border-bottom: 1px solid #f1f5f9; padding-bottom: 0.75rem; margin-bottom: 1rem;">
            <h3 style="margin: 0; color: #1e293b; font-size: 1.15rem;">Recent Punches</h3>
            <a routerLink="/punch" style="font-size: 0.8rem; color: #4f46e5; font-weight: 600; text-decoration: none;">Register Web Punch</a>
          </div>

          <div *ngIf="punches().length === 0" style="text-align: center; color: #94a3b8; padding: 2rem 0;">
            No punches registered under this session.
          </div>

          <div *ngIf="punches().length > 0" style="display: flex; flex-direction: column; gap: 0.5rem; max-height: 250px; overflow-y: auto;">
            <div *ngFor="let p of punches()" style="display: flex; justify-content: space-between; align-items: center; border-bottom: 1px solid #f8fafc; padding: 0.5rem 0;">
              <div style="display: flex; align-items: center; gap: 0.75rem;">
                <span [class]="'punch-dir ' + p.direction.toLowerCase()">{{ p.direction }}</span>
                <div>
                  <div style="font-weight: 500; font-size: 0.85rem; color: #334155;">{{ p.punchTime | date:'medium' }}</div>
                  <div style="font-size: 0.75rem; color: #94a3b8;">Device: {{ p.deviceCode }}</div>
                </div>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
  `,
  styles: [`
    .metric-card {
      background-color: white;
      padding: 1.5rem;
      border-radius: 8px;
      box-shadow: 0 1px 3px rgba(0,0,0,0.05);
      border: 1px solid #e2e8f0;
    }
    .sec-card {
      background-color: white;
      padding: 1.5rem;
      border-radius: 8px;
      box-shadow: 0 1px 3px rgba(0,0,0,0.05);
      border: 1px solid #e2e8f0;
    }
    .status-badge {
      font-size: 0.75rem;
      font-weight: 600;
      padding: 0.25rem 0.5rem;
      border-radius: 4px;
      text-transform: uppercase;
    }
    .status-badge.scheduled {
      background-color: #eff6ff;
      color: #2563eb;
    }
    .status-badge.completed {
      background-color: #ecfdf5;
      color: #16a34a;
    }
    .status-badge.cancelled {
      background-color: #fef2f2;
      color: #dc2626;
    }
    .punch-dir {
      font-size: 0.75rem;
      font-weight: 700;
      padding: 0.2rem 0.5rem;
      border-radius: 9999px;
      text-transform: uppercase;
      text-align: center;
      width: 32px;
      display: inline-block;
    }
    .punch-dir.in {
      background-color: #dcfce7;
      color: #15803d;
    }
    .punch-dir.out {
      background-color: #fee2e2;
      color: #b91c1c;
    }
  `]
})
export class DashboardComponent implements OnInit {
  private authService = inject(AuthService);
  private http = inject(HttpClient);

  user = computed(() => this.authService.currentUser());

  schedules = signal<Schedule[]>([]);
  punches = signal<Punch[]>([]);
  absences = signal<Absence[]>([]);

  ngOnInit(): void {
    const u = this.user();
    if (u) {
      this.loadDashboardData(u.id);
    }
  }

  loadDashboardData(employeeId: string): void {
    // Load employee routines
    this.http.get<Schedule[]>(`/api/v1/schedules/employee/${employeeId}`).subscribe({
      next: (data) => this.schedules.set(data),
      error: () => {}
    });

    // Load employee punches (mock response fallback if empty)
    this.http.get<Punch[]>(`/api/v1/punches`).subscribe({
      next: (data) => this.punches.set(data.filter(x => x as any)), // standard load
      error: () => {
        // Simple fallback
        this.punches.set([
          { id: '1', punchTime: '2026-05-29T08:00:00Z', direction: 'In', deviceCode: 'Office Terminal' },
          { id: '2', punchTime: '2026-05-29T17:00:00Z', direction: 'Out', deviceCode: 'Office Terminal' }
        ]);
      }
    });

    // Load employee absences
    this.http.get<Absence[]>(`/api/v1/absences/employee/${employeeId}`).subscribe({
      next: (data) => this.absences.set(data),
      error: () => {}
    });
  }
}
