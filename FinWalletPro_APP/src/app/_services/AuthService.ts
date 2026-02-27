import {
  AccountSummary,
  AuthResponse,
  LoginRequest,
  RegisterRequest,
} from './../_models/index';
import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { catchError, Observable, tap, throwError, BehaviorSubject } from 'rxjs';
import { environment } from 'src/environments/environment';
import { AccountDetails, ApiResponse } from '../_models';
import { Router } from '@angular/router';

const TOKEN_KEY   = 'fwp_access';
const REFRESH_KEY = 'fwp_refresh';
const USER_KEY    = 'fwp_user';

@Injectable({ providedIn: 'root' })
export class AuthService {
  private readonly api = environment.apiUrl;

  private _user = new BehaviorSubject<AccountSummary | null>(this.loadUser());
  readonly user   = this._user.asObservable();
  readonly isAuth = this._user.asObservable();

  constructor(private http: HttpClient, private router: Router) {}

  // ─── Public Methods ─────────────────────────────────────────────────────────
  login(payload: LoginRequest): Observable<ApiResponse<AuthResponse>> {
    return this.http.post<ApiResponse<AuthResponse>>(`${this.api}/auth/login`, payload).pipe(
      tap(res => this.storeSession(res.data)),
      catchError(err => throwError(() => err))
    );
  }

  register(payload: RegisterRequest): Observable<ApiResponse<AccountSummary>> {
    return this.http.post<ApiResponse<AccountSummary>>(`${this.api}/auth/register`, payload);
  }

  logout(): void {
    this.http.post(`${this.api}/auth/logout`, {}).subscribe();
    this.clearSession();
    this.router.navigate(['/auth/login']);
  }

  refreshToken(): Observable<ApiResponse<{ accessToken: string; refreshToken: string }>> {
    const token = this.getRefreshToken();
    return this.http.post<ApiResponse<any>>(`${this.api}/auth/refresh-token`, { refreshToken: token }).pipe(
      tap(res => {
        localStorage.setItem(TOKEN_KEY, res.data.accessToken);
        localStorage.setItem(REFRESH_KEY, res.data.refreshToken);
      })
    );
  }

  changePassword(currentPassword: string, newPassword: string, confirmNewPassword: string): Observable<ApiResponse<any>> {
    return this.http.post<ApiResponse<any>>(`${this.api}/auth/change-password`, {
      currentPassword, newPassword, confirmNewPassword
    });
  }

  updateUserSignal(account: AccountSummary): void {
    this._user.next(account);
    localStorage.setItem(USER_KEY, JSON.stringify(account));
  }

  // ─── Token helpers ──────────────────────────────────────────────────────────
  getAccessToken(): string | null { return localStorage.getItem(TOKEN_KEY); }
  getRefreshToken(): string | null { return localStorage.getItem(REFRESH_KEY); }

  // ─── Private ────────────────────────────────────────────────────────────────
  private storeSession(data: AuthResponse): void {
    localStorage.setItem(TOKEN_KEY, data.accessToken);
    localStorage.setItem(REFRESH_KEY, data.refreshToken);
    localStorage.setItem(USER_KEY, JSON.stringify(data.account));
    this._user.next(data.account);
  }

  private clearSession(): void {
    localStorage.removeItem(TOKEN_KEY);
    localStorage.removeItem(REFRESH_KEY);
    localStorage.removeItem(USER_KEY);
    this._user.next(null);
  }

  private loadUser(): AccountSummary | null {
    try {
      const stored = localStorage.getItem(USER_KEY);
      return stored ? JSON.parse(stored) : null;
    } catch { return null; }
  }
}
