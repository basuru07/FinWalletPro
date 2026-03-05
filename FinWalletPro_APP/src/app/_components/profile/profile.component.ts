import { Component, OnInit } from '@angular/core';
import { AccountDetails } from 'src/app/_models';
import { AccountService } from 'src/app/_services/AccountService';
import { AuthService } from 'src/app/_services/AuthService';

@Component({
  selector: 'app-profile',
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.css'],
})
export class ProfileComponent implements OnInit {
  account: AccountDetails | null = null;
  loading: boolean = true;
  profileLoading: boolean = false;
  profileSuccess: string = '';
  profileError: string = '';
  pwLoading: boolean = false;
  pwSuccess: string = '';
  pwError: string = '';

  profileForm: any = {};
  pwForm: any = {};

  get initials(): string {
    const n = this.account?.fullName ?? '';
    return n
      .split(' ')
      .map((w: string) => w[0])
      .join('')
      .toUpperCase()
      .slice(0, 2);
  }

  constructor(
    private acctSvc: AccountService,
    public auth: AuthService,
  ) {}

  ngOnInit(): void {
    this.acctSvc.getMyAccount().subscribe({
      next: (r) => {
        this.account = r.data;
        this.profileForm = {
          fullName: r.data.fullName,
          phoneNumber: r.data.phoneNumber,
        };
        this.loading = false;
      },
      error: () => {
        this.loading = false;
      },
    });
  }

  saveProfile(): void {
    this.profileLoading = true;
    this.profileError = '';
    this.profileSuccess = '';
    this.acctSvc.updateAccount(this.profileForm).subscribe({
      next: (r) => {
        this.account = r.data;
        this.auth.updateUserSignal(r.data);
        this.profileSuccess = 'Profile updated successfully.';
        this.profileLoading = false;
        setTimeout(() => (this.profileSuccess = ''), 3000);
      },
      error: (err) => {
        this.profileLoading = false;
        this.profileError = err?.error?.message ?? 'Update failed.';
      },
    });
  }

  changePassword(): void {
    this.pwError = '';
    this.pwSuccess = '';
    const { currentPassword, newPassword, confirm } = this.pwForm;
    if (!currentPassword || !newPassword) {
      this.pwError = 'All fields are required.';
      return;
    }
    if (newPassword !== confirm) {
      this.pwError = 'Passwords do not match.';
      return;
    }
    if (newPassword.length < 8) {
      this.pwError = 'Min. 8 characters.';
      return;
    }

    this.pwLoading = true;
    this.auth.changePassword(currentPassword, newPassword, confirm).subscribe({
      next: () => {
        this.pwLoading = false;
        this.pwForm = {};
        this.pwSuccess = 'Password changed successfully.';
        setTimeout(() => (this.pwSuccess = ''), 3000);
      },
      error: (err) => {
        this.pwLoading = false;
        this.pwError = err?.error?.message ?? 'Failed to change password.';
      },
    });
  }
}
