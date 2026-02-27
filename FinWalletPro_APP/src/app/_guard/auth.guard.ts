import { AuthService } from './../_services/AuthService';
import { Injectable } from '@angular/core';
import {
  ActivatedRouteSnapshot,
  CanActivate,
  Router,
  RouterStateSnapshot,
  UrlTree,
} from '@angular/router';
import { map, Observable } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class AuthGuard implements CanActivate {
  // Constructor
  constructor(
    private router: Router,
    private authService: AuthService,
  ) {}

  canActivate(
    route: ActivatedRouteSnapshot,
    state: RouterStateSnapshot,
  ):
    | Observable<boolean | UrlTree>
    | Promise<boolean | UrlTree>
    | boolean
    | UrlTree {

      // condition - if user exists -> allow | if null -> redirect to login
    return this.authService.isAuth.pipe(
      map((user) => {
        if (user) {
          return true;
        } else {
          return this.router.createUrlTree(['/auth/login']);
        }
      }),
    );
    
  }
}
