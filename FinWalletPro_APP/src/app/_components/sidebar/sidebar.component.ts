import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from 'src/app/_services/AuthService';
import { NotificationService } from 'src/app/_services/NotificationService';
import { AccountSummary } from 'src/app/_models';

interface NavItem {
  label: string;
  route: string;
  icon: string;
}

@Component({
  selector: 'app-sidebar',
  templateUrl: './sidebar.component.html',
  styleUrls: ['./sidebar.component.css'],
})
export class SidebarComponent implements OnInit {
  user: AccountSummary | null = null; // plain object, not Observable
  sidebarOpen = false;
  unreadCount = 0;

  navMain: NavItem[] = [
    { label: 'Dashboard', route: '/dashboard', icon: '⬡' },
    { label: 'Notifications', route: '/notifications', icon: '🔔' },
  ];

  navFinance: NavItem[] = [
    { label: 'Transactions', route: '/transactions', icon: '↔' },
    { label: 'Send Money', route: '/transfer', icon: '↑' },
    { label: 'Beneficiaries', route: '/beneficiaries', icon: '👥' },
    { label: 'Bank Cards', route: '/cards', icon: '💳' },
  ];

  constructor(
    private auth: AuthService,
    private notifSvc: NotificationService,
    private router: Router
  ) {}

  ngOnInit(): void {
    // Subscribe to user observable
    this.auth.user.subscribe(u => this.user = u);

    this.loadUnread();
  }

  // Getter for initials
  get initials(): string {
    const name = this.user?.fullName ?? '';
    return name
      .split(' ')
      .map(n => n[0])
      .join('')
      .toUpperCase()
      .slice(0, 2);
  }

  loadUnread(): void {
    this.notifSvc.getUnreadCount().subscribe({
      next: res => (this.unreadCount = res.data.unreadCount),
    });
  }

  toggleSidebar(): void {
    this.sidebarOpen = !this.sidebarOpen;
  }

  closeSidebar(): void {
    this.sidebarOpen = false;
  }

  logout(): void {
    this.auth.logout();
  }
}
