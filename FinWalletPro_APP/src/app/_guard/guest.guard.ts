import { AuthService } from './../_services/AuthService';
import { Injectable } from '@angular/core';
import { CanActivate, Router } from '@angular/router';
import { map, Observable, take } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class GuestGuard implements CanActivate {
  // Constructor
  constructor(
    private router: Router,
    private authService: AuthService,
  ) {}

  canActivate() {
    // condition - if user exists -> allow | if null -> redirect to login
    return this.authService.user.pipe(
      take(1),
      map((user) => {
        if (!user) return true;
        this.router.navigate(['/dashboard']);
        return false;
      }),
    );
  }
}
