import { Component, OnInit } from '@angular/core';
import { NotificationService } from 'src/app/_services/NotificationService';
import { Notification as AppNotification } from 'src/app/_models';

@Component({
  selector: 'app-notification',
  templateUrl: './notification.component.html',
  styleUrls: ['./notification.component.css']
})
export class NotificationComponent implements OnInit {

  notifications: AppNotification[] = [];
  loading:        boolean           = true;
  unreadOnly:     boolean           = false;
  unreadCount:    number            = 0;

  constructor(private svc: NotificationService) {}

  ngOnInit(): void { this.load(); }

  load(): void {
    this.loading = true;
    this.svc.getAll(this.unreadOnly).subscribe({
      next: r => {
        this.notifications = (r.data ?? []) as unknown as AppNotification[];
        this.unreadCount   = this.notifications.filter(n => !n.isRead).length;
        this.loading       = false;
      },
      error: () => { this.loading = false; }
    });
  }

  toggleUnreadOnly(): void {
    this.unreadOnly = !this.unreadOnly;
    this.load();
  }

  read(n: AppNotification): void {
    if (n.isRead) return;
    this.svc.markRead(n.notificationId).subscribe({
      next: () => {
        this.notifications = this.notifications.map(x =>
          x.notificationId === n.notificationId ? { ...x, isRead: true } : x
        );
        this.unreadCount = Math.max(0, this.unreadCount - 1);
      }
    });
  }

  markAllRead(): void {
    this.svc.markAllRead().subscribe({ next: () => this.load() });
  }

  remove(n: AppNotification, e: Event): void {
    e.stopPropagation();
    this.svc.delete(n.notificationId).subscribe({
      next: () => {
        this.notifications = this.notifications.filter(
          x => x.notificationId !== n.notificationId
        );
      }
    });
  }

  icon(n: AppNotification): string {
    const m: Record<string, string> = {
      Transaction: '↔',
      Security:    '🔒',
      System:      '⚙',
      Promotional: '🎁'
    };
    return m[n.notificationType] ?? '🔔';
  }

  iconClass(n: AppNotification): string {
    return `notif-icon ${n.notificationType?.toLowerCase() ?? 'system'}`;
  }
}
