import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';

import { AuthService } from '../../../services/auth.service';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [FormsModule],
  templateUrl: './login.component.html',
  styleUrl: './login.component.css'
})
export class LoginComponent {
  username = '';
  password = '';

  errorMessage = '';
  isLoading = false;

  constructor(
    private authService: AuthService,
    private router: Router
  ) { }

  login(): void {
    this.errorMessage = '';

    const username = this.username.trim();

    if (!username || username.length > 100) {
      this.errorMessage = 'Enter a valid username.';
      return;
    }

    if (!this.password || this.password.length > 128) {
      this.errorMessage = 'Enter your password.';
      return;
    }

    if (this.isLoading) {
      return;
    }

    this.isLoading = true;

    this.authService.login(username, this.password).subscribe({
      next: () => {
        this.isLoading = false;
        this.router.navigate(['/home']);
      },
      error: () => {
        this.isLoading = false;
        this.errorMessage = 'Invalid username or password.';
      }
    });
  }
}
