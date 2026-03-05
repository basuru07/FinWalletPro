import { Component, OnInit } from '@angular/core';
import { AccountSummary } from 'src/app/_models';
import { AuthService } from 'src/app/_services/AuthService';
import { NotificationService } from 'src/app/_services/NotificationService';

interface NavItem {
  label: string;
  route: string;
  icon: string;
}

@Component({
  selector: 'app-shell',
  templateUrl: './shell.component.html',
  styleUrls: ['./shell.component.css'],
})
export class ShellComponent implements OnInit {
  // Keep as Observable — unwrapped in template via async pipe
  user = this.auth.user;
  sidebarOpen = false;
  unreadCount = 0;

  navMain: NavItem[] = [
    { label: 'Dashboard',     route: '/dashboard',     icon: '⬡' },
    { label: 'Notifications', route: '/notifications', icon: '🔔' },
  ];

  navFinance: NavItem[] = [
    { label: 'Transactions',  route: '/transactions',  icon: '↔' },
    { label: 'Send Money',    route: '/transfer',      icon: '↑' },
    { label: 'Beneficiaries', route: '/beneficiaries', icon: '👥' },
    { label: 'Bank Cards',    route: '/cards',         icon: '💳' },
  ];
item: any;

  constructor(
    private auth: AuthService,
    private notifSvc: NotificationService,
  ) {}

  ngOnInit(): void {
    this.loadUnread();
  }

  // Accepts the already-resolved user from the template
  initials(user: AccountSummary | null): string {
    const name = user?.fullName ?? '';
    return name
      .split(' ')
      .map((n: string) => n[0])
      .join('')
      .toUpperCase()
      .slice(0, 2);
  }

  loadUnread(): void {
    this.notifSvc.getUnreadCount().subscribe({
      next: (res) => (this.unreadCount = res.data.unreadCount),
    });
  }

  logout(): void {
    this.auth.logout();
  }
}
