import { Component, OnInit } from '@angular/core';
import { AccountDetails, Transaction } from 'src/app/_models';
import { AccountService } from 'src/app/_services/AccountService';
import { TransactionService } from 'src/app/_services/TransactionService';
import { AuthService } from 'src/app/_services/AuthService';

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.css']
})
export class DashboardComponent implements OnInit {

  account: AccountDetails | null = null;
  recentTx: Transaction[] = [];
  txLoading = true;

  totalSent = 0;
  totalReceived = 0;

  depositModal = false;
  depositLoading = false;
  depositAmount = 0;
  depositNote = '';

  quickActions = [
    { label: 'Send Money', route: '/transfer', icon: '↑' },
    { label: 'History', route: '/transactions', icon: '↔' },
    { label: 'Beneficiaries', route: '/beneficiaries', icon: '👥' },
    { label: 'Bank Cards', route: '/cards', icon: '💳' },
    { label: 'Profile', route: '/profile', icon: '👤' },
  ];

  constructor(
    private accountSvc: AccountService,
    private txSvc: TransactionService,
    private auth: AuthService
  ) {}

  ngOnInit(): void {
    this.loadDashboard();
  }

  loadDashboard(): void {

    this.accountSvc.getMyAccount().subscribe({
      next: r => {
        this.account = r.data;
      }
    });

    const now = new Date();
    const from = new Date(now.getFullYear(), now.getMonth(), 1)
      .toISOString().split('T')[0];
    const to = now.toISOString().split('T')[0];

    this.txSvc.getHistory({
      pageNumber: 1, pageSize: 8,
      fromDate: '',
      toDate: '',
      transactionType: '',
      status: '',
      minAmount: 0,
      maxAmount: 0,
      category: ''
    }).subscribe({
      next: r => {
        this.recentTx = r.data.transactions ?? [];
        this.txLoading = false;
      },
      error: () => this.txLoading = false
    });

    this.txSvc.getStatement(from, to).subscribe({
      next: r => {
        this.totalSent = r.data.summary.totalSent ?? 0;
        this.totalReceived = r.data.summary.totalReceived ?? 0;
      }
    });
  }

  timeOfDay(): string {
    const h = new Date().getHours();
    if (h < 12) return 'morning';
    if (h < 17) return 'afternoon';
    return 'evening';
  }

  firstName(): string {
    return this.account?.fullName?.split(' ')[0] ?? '';
  }

  isCredited(tx: Transaction): boolean {
    if (tx.transactionType === 'Deposit') return true;
    if (tx.transactionType === 'Withdrawal') return false;
    return tx.receiverAccountNumber === this.account?.accountNumber;
  }

  txCounterparty(tx: Transaction): string {
    const me = this.account?.accountNumber;

    if (tx.transactionType === 'Deposit' || tx.transactionType === 'Withdrawal')
      return tx.transactionType;

    return tx.senderAccountNumber === me
      ? (tx.receiverName || tx.receiverAccountNumber)
      : (tx.senderName || tx.senderAccountNumber);
  }

  showDeposit(): void {
    this.depositModal = true;
  }

  doDeposit(): void {
    if (!this.depositAmount || this.depositAmount <= 0) return;

    this.depositLoading = true;

    this.txSvc.deposit({
      amount: this.depositAmount,
      description: this.depositNote
    }).subscribe({
      next: () => {
        this.depositLoading = false;
        this.depositModal = false;
        this.depositAmount = 0;
        this.depositNote = '';
        this.loadDashboard();
      },
      error: () => this.depositLoading = false
    });
  }
}
