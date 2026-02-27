import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Transaction } from 'src/app/_models';
import { TransactionService } from 'src/app/_services/TransactionService';

@Component({
  selector: 'app-detail',
  templateUrl: './detail.component.html',
  styleUrls: ['./detail.component.css']
})
export class DetailComponent implements OnInit {

  tx: Transaction | null = null;
  loading: boolean = true;

  reverseModal: boolean = false;
  reverseLoading: boolean = false;
  reverseReason: string = '';

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private txSvc: TransactionService
  ) {}

  ngOnInit(): void {
    const id = Number(this.route.snapshot.paramMap.get('id'));
    if (!id) {
      this.router.navigate(['/transactions']);
      return;
    }

    this.txSvc.getById(id).subscribe({
      next: res => {
        this.tx = res.data;
        this.loading = false;
      },
      error: () => {
        this.loading = false;
        alert('Failed to load transaction.');
      }
    });
  }

  heroClass(): string {
    if (!this.tx) return 'hero-card';

    const t = this.tx.transactionType?.toLowerCase();
    if (t === 'deposit') return 'hero-card deposit';
    if (t === 'withdrawal') return 'hero-card withdraw';
    return 'hero-card transfer';
  }

  heroIcon(): string {
    if (!this.tx) return '';
    if (this.tx.transactionType === 'Deposit') return '↓';
    if (this.tx.transactionType === 'Withdrawal') return '↑';
    return '↔';
  }

  badgeClass(status: string): string {
    const map: Record<string, string> = {
      Completed: 'badge-success',
      Pending: 'badge-warning',
      Failed: 'badge-danger',
      Reversed: 'badge-muted'
    };
    return map[status] ?? 'badge-muted';
  }

  openReverse(): void {
    this.reverseReason = '';
    this.reverseModal = true;
  }

  closeReverse(): void {
    this.reverseModal = false;
  }

  doReverse(): void {
    if (!this.tx || !this.reverseReason) return;

    this.reverseLoading = true;
    this.txSvc.reverse(this.tx.transactionId, this.reverseReason).subscribe({
      next: () => {
        this.reverseLoading = false;
        this.reverseModal = false;
        // Update status locally
        if (this.tx) this.tx.status = 'Pending Reversal';
      },
      error: () => {
        this.reverseLoading = false;
        alert('Reversal failed.');
      }
    });
  }
}
