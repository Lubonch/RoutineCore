import { Routes } from '@angular/router';
import { inject } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from './services/auth.service';

const authGuard = () => {
  const authService = inject(AuthService);
  const router = inject(Router);
  if (authService.isAuthenticated()) {
    return true;
  }
  return router.createUrlTree(['/login']);
};

export const routes: Routes = [
  {
    path: '',
    redirectTo: 'dashboard',
    pathMatch: 'full'
  },
  {
    path: 'login',
    loadComponent: () => import('./components/login/login.component').then(m => m.LoginComponent)
  },
  {
    path: 'dashboard',
    loadComponent: () => import('./components/dashboard/dashboard.component').then(m => m.DashboardComponent),
    canActivate: [authGuard]
  },
  {
    path: 'punch',
    loadComponent: () => import('./components/punch/punch.component').then(m => m.PunchComponent),
    canActivate: [authGuard]
  },
  {
    path: 'absence',
    loadComponent: () => import('./components/absence-request/absence-request.component').then(m => m.AbsenceRequestComponent),
    canActivate: [authGuard]
  },
  {
    path: 'schedule',
    loadComponent: () => import('./components/schedule/schedule.component').then(m => m.ScheduleComponent),
    canActivate: [authGuard]
  },
  {
    path: '**',
    redirectTo: 'dashboard'
  }
];
