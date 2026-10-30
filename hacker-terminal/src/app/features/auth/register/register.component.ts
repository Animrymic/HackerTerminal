import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';

import { AuthService } from '../../../services/auth.service';

@Component({
  selector: 'app-register',
  standalone: true,
  imports: [FormsModule],
  templateUrl: './register.component.html',
  styleUrl: './register.component.css'
})
export class RegisterComponent {
  username = '';
  email = '';
  password = '';
  confirmPassword = '';

  errorMessage = '';
  successMessage = '';
  isLoading = false;

  constructor(
    private authService: AuthService,
    private router: Router
  ) { }

  register(): void {
    this.errorMessage = '';
    this.successMessage = '';

    const username = this.username.trim();
    const email = this.email.trim();
    const usernamePattern = /^[a-zA-Z0-9_-]{3,30}$/;
    const emailPattern = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;

    if (!usernamePattern.test(username)) {
      this.errorMessage =
        'Username must be 3–30 characters and use only letters, numbers, underscores, or hyphens.';
      return;
    }

    if (email.length > 254 || !emailPattern.test(email)) {
      this.errorMessage = 'Enter a valid email address.';
      return;
    }

    if (this.password.length < 8 || this.password.length > 128) {
      this.errorMessage = 'Password must be between 8 and 128 characters.';
      return;
    }

    if (this.password !== this.confirmPassword) {
      this.errorMessage = 'Passwords do not match.';
      return;
    }

    this.isLoading = true;

    this.authService.register(username, email, this.password).subscribe({
      next: () => {
        this.isLoading = false;
        this.successMessage =
          'Registration successful! Redirecting to login...';

        setTimeout(() => {
          this.router.navigate(['/login']);
        }, 1500);
      },
      error: (error: { status?: number }) => {
        this.isLoading = false;

        if (error.status === 409) {
          this.errorMessage = 'Username or email already exists.';
        } else if (error.status === 400) {
          this.errorMessage =
            'Check the username, email, and password requirements.';
        } else {
          this.errorMessage = 'Registration failed. Please try again.';
        }
      }
    });
  }
}
