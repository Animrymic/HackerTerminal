import { Injectable } from '@angular/core';
import {
  HttpInterceptor,
  HttpRequest,
  HttpHandler
} from '@angular/common/http';

import { AuthService } from '../services/auth.service';

@Injectable()
export class AuthInterceptor implements HttpInterceptor {

  constructor(private authService: AuthService) {
  }

  intercept(
    request: HttpRequest<any>,
    next: HttpHandler
  ) {

    let token: string | null = this.authService.getToken();

    if (token === null) {
      return next.handle(request);
    }

    let authenticatedRequest: HttpRequest<any> = request.clone({
      setHeaders: {
        Authorization: `Bearer ${token}`
      }
    });

    return next.handle(authenticatedRequest);
  }
}
