import { Component, inject, OnInit, signal, computed } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { HttpClient } from '@angular/common/http';
import { AuthService } from '../../services/auth.service';

interface Absence {
  id: string;
  employeeId: string;
  startDate: string;
  endDate: string;
  reason: string;
  authorized: boolean;
  approvedBy: string | null;
}

@Component({
  selector: 'app-absence-request',
  standalone: true,
  imports: [CommonModule, FormsModule],
  template: `
    <div style="display: flex; flex-direction: column; gap: 2rem;">
      <div style="background-color: white; padding: 2rem; border-radius: 8px; box-shadow: 0 1px 3px rgba(0,0,0,0.1); border: 1px solid #e2e8f0;">
        <h2 style="margin: 0 0 0.5rem; color: #1e293b;">Time-off & Absence Requests</h2>
        <p style="margin: 0 0 1.5rem; color: #64748b; font-size: 0.95rem;">
          Request sick leaves, vacations, or general absences. Admins and Managers will evaluate your requests.
        </p>

        <!-- New Request Form -->
        <form (ngSubmit)="onRequestSubmit()" #requestForm="ngForm" style="display: flex; flex-direction: column; gap: 1rem; max-width: 500px;">
          <div style="display: flex; gap: 1rem;">
            <div style="flex: 1;">
              <label style="display: block; font-size: 0.85rem; font-weight: 500; color: #475569; margin-bottom: 0.25rem;">Start Date</label>
              <input type="date" name="startDate" [(ngModel)]="startDate" required style="width: 100%; padding: 0.5rem; border: 1px solid #cbd5e1; border-radius: 4px;">
            </div>
            
            <div style="flex: 1;">
              <label style="display: block; font-size: 0.85rem; font-weight: 500; color: #475569; margin-bottom: 0.25rem;">End Date</label>
              <input type="date" name="endDate" [(ngModel)]="endDate" required style="width: 100%; padding: 0.5rem; border: 1px solid #cbd5e1; border-radius: 4px;">
            </div>
          </div>

          <div>
            <label style="display: block; font-size: 0.85rem; font-weight: 500; color: #475569; margin-bottom: 0.25rem;">Reason / Description</label>
            <textarea name="reason" [(ngModel)]="reason" required placeholder="Describe the reason for the absence" rows="3" style="width: 100%; padding: 0.5rem; border: 1px solid #cbd5e1; border-radius: 4px; box-sizing: border-box; resize: none;"></textarea>
          </div>

          <button type="submit" [disabled]="!requestForm.form.valid" style="align-self: flex-start; background-color: #6366f1; color: white; border: none; padding: 0.6rem 1.25rem; border-radius: 4px; font-weight: 600; cursor: pointer; transition: background-color 0.2s;" onmouseover="this.style.backgroundColor='#4f46e5'" onmouseout="this.style.backgroundColor='#6366f1'">
            Submit Request
          </button>
        </form>
      </div>

      <!-- Admin / Approvals View -->
      <div *ngIf="isAdmin()" style="background-color: white; padding: 2rem; border-radius: 8px; box-shadow: 0 1px 3px rgba(0,0,0,0.1); border: 1px solid #e2e8f0;">
        <h3 style="margin: 0 0 0.5rem; color: #0f172a;">Pending Absence Approvals (Admin Controls)</h3>
        <p style="margin: 0 0 1.5rem; color: #64748b; font-size: 0.88rem;">
          Evaluating requests triggers custom RabbitMQ dispatch notifications via <strong>PulseDispatcher</strong> (ActionCenter replica).
        </p>

        <div *ngIf="pendingAbsences().length === 0" style="text-align: center; color: #94a3b8; padding: 1.5rem 0;">
          No requests awaiting authorization.
        </div>

        <div *ngIf="pendingAbsences().length > 0" style="overflow-x: auto;">
          <table style="width: 100%; border-collapse: collapse; text-align: left; font-size: 0.88rem;">
            <thead>
              <tr style="background-color: #f8fafc; border-bottom: 1px solid #e2e8f0; color: #475569; font-weight: 600;">
                <th style="padding: 0.75rem 1rem;">Employee ID</th>
                <th style="padding: 0.75rem 1rem;">Duration</th>
                <th style="padding: 0.75rem 1rem;">Reason</th>
                <th style="padding: 0.75rem 1rem;">Action</th>
              </tr>
            </thead>
            <tbody>
              <tr *ngFor="let a of pendingAbsences()" style="border-bottom: 1px solid #f1f5f9;">
                <td style="padding: 0.75rem 1rem; font-family: monospace;">{{ a.employeeId }}</td>
                <td style="padding: 0.75rem 1rem;">{{ a.startDate | date:'shortDate' }} &rarr; {{ a.endDate | date:'shortDate' }}</td>
                <td style="padding: 0.75rem 1rem; color: #334155;">{{ a.reason }}</td>
                <td style="padding: 0.75rem 1rem;">
                  <button (click)="onApprove(a.id)" style="background-color: #10b981; color: white; border: none; padding: 0.35rem 0.75rem; border-radius: 4px; font-weight: 600; cursor: pointer;" onmouseover="this.style.backgroundColor='#059669'" onmouseout="this.style.backgroundColor='#10b981'">
                    Authorize
                  </button>
                </td>
              </tr>
            </tbody>
          </table>
        </div>
      </div>
    </div>
  `,
  styles: []
})
export class AbsenceRequestComponent implements OnInit {
  private authService = inject(AuthService);
  private http = inject(HttpClient);

  currentUser = computed(() => this.authService.currentUser());
  isAdmin = computed(() => this.authService.hasRole(['Admin', 'Manager']));

  startDate = '';
  endDate = '';
  reason = '';

  pendingAbsences = signal<Absence[]>([]);

  ngOnInit(): void {
    if (this.isAdmin()) {
      this.loadPendingApprovals();
    }
  }

  loadPendingApprovals(): void {
    this.http.get<Absence[]>('/api/v1/absences/pending').subscribe({
      next: (data) => this.pendingAbsences.set(data),
      error: () => {}
    });
  }

  onRequestSubmit(): void {
    const user = this.currentUser();
    if (!user) return;

    this.http.post<Absence>('/api/v1/absences', {
      employeeId: user.id,
      startDate: this.startDate,
      endDate: this.endDate,
      reason: this.reason,
      authorized: false
    }).subscribe({
      next: () => {
        alert('Abence request created successfully.');
        this.startDate = '';
        this.endDate = '';
        this.reason = '';
        if (this.isAdmin()) this.loadPendingApprovals();
      }
    });
  }

  onApprove(id: string): void {
    this.http.post(`/api/v1/absences/${id}/approve`, {}).subscribe({
      next: () => {
        alert('Absence authorized! Event triggered down the queue.');
        this.loadPendingApprovals();
      }
    });
  }
}
//
