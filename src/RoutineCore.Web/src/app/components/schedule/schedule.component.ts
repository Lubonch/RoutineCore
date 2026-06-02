import { Component, inject, OnInit, signal, computed } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { HttpClient } from '@angular/common/http';
import { AuthService } from '../../services/auth.service';

interface Schedule {
  id: string;
  employeeId: string;
  startTime: string;
  endTime: string;
  projectTaskId: string;
  status: string;
}

@Component({
  selector: 'app-schedule',
  standalone: true,
  imports: [CommonModule, FormsModule],
  template: `
    <div style="display: flex; flex-direction: column; gap: 2rem;">
      <div style="background-color: white; padding: 2rem; border-radius: 8px; box-shadow: 0 1px 3px rgba(0,0,0,0.1); border: 1px solid #e2e8f0;">
        <h2 style="margin: 0 0 0.5rem; color: #1e293b;">Routine Scheduling Plan</h2>
        <p style="margin: 0 0 1.5rem; color: #64748b; font-size: 0.95rem;">
          Assign structured timeslots, program show rosters, and operating shifts for your teams.
        </p>

        <!-- Access Guard Warning if non-admin -->
        <div *ngIf="!canManage()" style="background-color: #fef3c7; border: 1px solid #fde68a; color: #b45309; padding: 1rem; border-radius: 6px; font-size: 0.88rem;">
          ⚠️ <strong>Access View Only:</strong> Your account does not hold routing/planner privileges. Only designated administrators or operator planners can draft active employee rosters.
        </div>

        <!-- Roster Assignment Form (Authenticated Admins / Planners) -->
        <form *ngIf="canManage()" (ngSubmit)="onCreateSchedule()" #scheduleForm="ngForm" style="display: flex; flex-direction: column; gap: 1rem; max-width: 500px;">
          <div>
            <label style="display: block; font-size: 0.85rem; font-weight: 500; color: #475569; margin-bottom: 0.25rem;">Employee Target ID</label>
            <input type="text" name="employeeId" [(ngModel)]="employeeId" required placeholder="Paste Employee GUID (e.g. 5d5a7bca-... or login name)" style="width: 100%; padding: 0.5rem; border: 1px solid #cbd5e1; border-radius: 4px; box-sizing: border-box;">
          </div>

          <div style="display: flex; gap: 1rem;">
            <div style="flex: 1;">
              <label style="display: block; font-size: 0.85rem; font-weight: 500; color: #475569; margin-bottom: 0.25rem;">Shift Start Time</label>
              <input type="datetime-local" name="startTime" [(ngModel)]="startTime" required style="width: 100%; padding: 0.5rem; border: 1px solid #cbd5e1; border-radius: 4px;">
            </div>

            <div style="flex: 1;">
              <label style="display: block; font-size: 0.85rem; font-weight: 500; color: #475569; margin-bottom: 0.25rem;">Shift End Time</label>
              <input type="datetime-local" name="endTime" [(ngModel)]="endTime" required style="width: 100%; padding: 0.5rem; border: 1px solid #cbd5e1; border-radius: 4px;">
            </div>
          </div>

          <div>
            <label style="display: block; font-size: 0.85rem; font-weight: 500; color: #475569; margin-bottom: 0.25rem;">Project Task / Activity Code</label>
            <input type="text" name="projectTaskId" [(ngModel)]="projectTaskId" required placeholder="e.g. CONTROL_ROOM_T1 or NEWS_ROSTER_AM" style="width: 100%; padding: 0.5rem; border: 1px solid #cbd5e1; border-radius: 4px; box-sizing: border-box;">
          </div>

          <button type="submit" [disabled]="!scheduleForm.form.valid" style="align-self: flex-start; background-color: #4f46e5; color: white; border: none; padding: 0.6rem 1.25rem; border-radius: 4px; font-weight: 600; cursor: pointer; transition: background-color 0.2s;" onmouseover="this.style.backgroundColor='#4338ca'" onmouseout="this.style.backgroundColor='#4f46e5'">
            Assign Team Shift
          </button>
        </form>
      </div>

      <!-- Schedule List Roster -->
      <div style="background-color: white; padding: 2rem; border-radius: 8px; box-shadow: 0 1px 3px rgba(0,0,0,0.1); border: 1px solid #e2e8f0;">
        <h3 style="margin: 0 0 1.25rem; color: #0f172a;">Active Operational Shifts & Schedules</h3>
        
        <div *ngIf="schedules().length === 0" style="text-align: center; color: #94a3b8; padding: 2rem 0;">
          No operational schedules posted in this system cycle.
        </div>

        <div *ngIf="schedules().length > 0" style="overflow-x: auto;">
          <table style="width: 100%; border-collapse: collapse; text-align: left; font-size: 0.88rem;">
            <thead>
              <tr style="background-color: #f8fafc; border-bottom: 1px solid #e2e8f0; color: #475569; font-weight: 600;">
                <th style="padding: 0.75rem 1rem;">Employee ID</th>
                <th style="padding: 0.75rem 1rem;">Start Time</th>
                <th style="padding: 0.75rem 1rem;">End Time</th>
                <th style="padding: 0.75rem 1rem;">Task Details</th>
                <th style="padding: 0.75rem 1rem;">Roster Status</th>
              </tr>
            </thead>
            <tbody>
              <tr *ngFor="let s of schedules()" style="border-bottom: 1px solid #f1f5f9;">
                <td style="padding: 0.75rem 1rem; font-family: monospace;">{{ s.employeeId }}</td>
                <td style="padding: 0.75rem 1rem;">{{ s.startTime | date:'medium' }}</td>
                <td style="padding: 0.75rem 1rem;">{{ s.endTime | date:'medium' }}</td>
                <td style="padding: 0.75rem 1rem; font-weight: 500;">{{ s.projectTaskId }}</td>
                <td style="padding: 0.75rem 1rem;">
                  <span [class]="'badge-status ' + s.status.toLowerCase()">{{ s.status }}</span>
                </td>
              </tr>
            </tbody>
          </table>
        </div>
      </div>
    </div>
  `,
  styles: [`
    .badge-status {
      font-size: 0.75rem;
      font-weight: 700;
      padding: 0.2rem 0.5rem;
      border-radius: 4px;
      text-transform: uppercase;
    }
    .badge-status.scheduled {
      background-color: #e0f2fe;
      color: #0369a1;
    }
    .badge-status.completed {
      background-color: #dcfce7;
      color: #15803d;
    }
  `]
})
export class ScheduleComponent implements OnInit {
  private authService = inject(AuthService);
  private http = inject(HttpClient);

  currentUser = computed(() => this.authService.currentUser());
  canManage = computed(() => this.authService.hasRole(['Admin', 'Operator']));

  schedules = signal<Schedule[]>([]);

  employeeId = '';
  startTime = '';
  endTime = '';
  projectTaskId = '';

  ngOnInit(): void {
    this.loadAllSchedules();
  }

  loadAllSchedules(): void {
    this.http.get<Schedule[]>('/api/v1/schedules').subscribe({
      next: (data) => this.schedules.set(data),
      error: () => {}
    });
  }

  onCreateSchedule(): void {
    if (!this.employeeId || !this.startTime || !this.endTime || !this.projectTaskId) return;

    this.http.post<Schedule>('/api/v1/schedules', {
      employeeId: this.employeeId,
      startTime: this.startTime,
      endTime: this.endTime,
      projectTaskId: this.projectTaskId,
      status: 'Scheduled'
    }).subscribe({
      next: () => {
        alert('Routine schedule created successfully.');
        this.employeeId = '';
        this.startTime = '';
        this.endTime = '';
        this.projectTaskId = '';
        this.loadAllSchedules();
      },
      error: (err) => {
        alert(err.error?.message || 'Error occurred while programming shift schedule.');
      }
    });
  }
}
//
