import { Component, OnInit } from '@angular/core';
import { Transaction } from 'src/app/_models';
import { AccountService } from 'src/app/_services/AccountService';
import { TransactionService } from 'src/app/_services/TransactionService';

@Component({
  selector: 'app-history',
  templateUrl: './history.component.html',
  styleUrls: ['./history.component.css'],
})
export class HistoryComponent implements OnInit {

  transactions: Transaction[] = [];
  loading = true;
  page = 1;
  pageSize = 20;
  accountCurrency = 'USD';

  filter: any = {
    transactionType: '',
    status: '',
    fromDate: '',
    toDate: ''
  };

  constructor(
    private txSvc: TransactionService,
    private acctSvc: AccountService
  ) {}

  ngOnInit(): void {
    this.acctSvc.getBalance().subscribe({
      next: (r) => this.accountCurrency = r.data.currency
    });

    this.load();
  }

  load(): void {
    this.loading = true;

    this.txSvc.getHistory({
      ...this.filter,
      pageNumber: this.page,
      pageSize: this.pageSize,
    }).subscribe({
      next: (r) => {
        this.transactions = r.data.transactions ?? [];
        this.loading = false;
      },
      error: () => this.loading = false,
    });
  }

  applyFilter(): void {
    this.page = 1;
    this.load();
  }

  clearFilters(): void {
    this.filter = {
      transactionType: '',
      status: '',
      fromDate: '',
      toDate: ''
    };
    this.applyFilter();
  }

  prevPage(): void {
    this.page--;
    this.load();
  }

  nextPage(): void {
    this.page++;
    this.load();
  }

  isCredited(tx: Transaction): boolean {
    if (tx.transactionType === 'Deposit') return true;
    if (tx.transactionType === 'Withdrawal') return false;
    return !tx.senderAccountNumber;
  }

  peer(tx: Transaction): string {
    if (tx.transactionType === 'Deposit') return 'Deposit';
    if (tx.transactionType === 'Withdrawal') return 'Withdrawal';
    return tx.senderName || tx.receiverName || tx.receiverAccountNumber || '—';
  }

  icon(tx: Transaction): string {
    if (tx.transactionType === 'Deposit') return '↓';
    if (tx.transactionType === 'Withdrawal') return '↑';
    return this.isCredited(tx) ? '↓' : '↑';
  }

  iconClass(tx: Transaction): string {
    if (tx.transactionType === 'Deposit') return 'dep';
    if (tx.transactionType === 'Withdrawal') return 'with';
    return this.isCredited(tx) ? 'recv' : 'send';
  }

  badgeClass(status: string): string {
    const map: any = {
      Completed: 'badge-success',
      Pending: 'badge-warning',
      Failed: 'badge-danger',
      Reversed: 'badge-muted'
    };
    return map[status] || 'badge-muted';
  }
}
