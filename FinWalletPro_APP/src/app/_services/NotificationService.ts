import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { ApiResponse } from '../_models';
import { Injectable } from '@angular/core';

@Injectable({ providedIn: 'root' })
export class NotificationService {

  // Backend URL
  private readonly api = `${environment.apiUrl}/notification`;

  constructor(private http: HttpClient) {}

  // Get notification
  getAll(unreadOnly = false): Observable<ApiResponse<Notification[]>> {
    return this.http.get<ApiResponse<Notification[]>>(
      `${this.api}?unreadOnly=${unreadOnly}`,
    );
  }

  // Get unread count
  getUnreadCount(): Observable<ApiResponse<{ unreadCount: number }>> {
    return this.http.get<ApiResponse<{ unreadCount: number }>>(
      `${this.api}/unread-count`,
    );
  }

  // Get notification read by Id
  markRead(id: number): Observable<ApiResponse<any>> {
    return this.http.patch<ApiResponse<any>>(`${this.api}/${id}/read`, {});
  }

  // Read all notification
  markAllRead(): Observable<ApiResponse<any>> {
    return this.http.patch<ApiResponse<any>>(`${this.api}/read-all`, {});
  }

  // Delete notification
  delete(id: number): Observable<ApiResponse<any>> {
    return this.http.delete<ApiResponse<any>>(`${this.api}/${id}`);
  }
}
