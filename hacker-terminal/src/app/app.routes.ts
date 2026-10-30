import { Routes } from '@angular/router';

import { LoginComponent } from './features/auth/login/login.component';
import { RegisterComponent } from './features/auth/register/register.component';
import { HomeComponent } from './features/dashboard/home/home.component';
import { MissionListComponent } from './features/missions/mission-list/mission-list.component';
import { ProfileComponent } from './features/profile/profile.component';
import { LeaderboardComponent } from './features/leaderboard/leaderboard.component';
import { AuthGuard } from './guards/auth.guard';

export const routes: Routes = [

  {
    path: 'login',
    component: LoginComponent
  },

  {
    path: 'register',
    component: RegisterComponent
  },

  {
    path: 'home',
    component: HomeComponent,
    canActivate: [AuthGuard]
  },

  {
    path: '',
    redirectTo: 'login',
    pathMatch: 'full'
  },

  {
    path: 'missions',
    component: MissionListComponent,
    canActivate: [AuthGuard]
  },

  {
    path: 'profile',
    component: ProfileComponent,
    canActivate: [AuthGuard]
  },

  {
    path: 'leaderboard',
    component: LeaderboardComponent,
    canActivate: [AuthGuard]
  }
];
