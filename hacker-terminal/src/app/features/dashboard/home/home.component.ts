import { Component } from '@angular/core';
import { Router } from '@angular/router';

import { AuthService } from '../../../services/auth.service';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrl: './home.component.css'
})
export class HomeComponent {

  username: string = '';

  constructor(
    private authService: AuthService,
    private router: Router
  ) {

    let user = this.authService.getUser();

    if (user !== null) {
      this.username = user.username;
    }
  }

  logout(): void {

    this.authService.logout();

    this.router.navigate(['/login']);
  }

  openMissions(): void {
    this.router.navigate(['/missions']);
  }

  openProfile(): void {
    this.router.navigate(['/profile']);
  }

  openLeaderboard(): void {
    this.router.navigate(['/leaderboard']);
  }
}
