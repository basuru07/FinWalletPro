import { Component, OnInit } from '@angular/core';
import { Beneficiary, Transaction } from 'src/app/_models';
import { AccountService } from 'src/app/_services/AccountService';
import { BeneficiaryService } from 'src/app/_services/BeneficiaryService';
import { TransactionService } from 'src/app/_services/TransactionService';

@Component({
  selector: 'app-transfer',
  templateUrl: './transfer.component.html',
  styleUrls: ['./transfer.component.css']
})
export class TransferComponent implements OnInit {

  form = { receiverAccountNumber: '', amount: 0, description: '', category: 'Transfer' };
  loading = false;
  error = '';
  success = false;
  successTx: Transaction | null = null;
  beneficiaries: Beneficiary[] = [];
  balance = 0;
  currency = 'USD';
  submitted = false;
  selectedBen: number | null = null;

  constructor(
    private txSvc: TransactionService,
    private benSvc: BeneficiaryService,
    private acctSvc: AccountService
  ) { }

  ngOnInit(): void {
    this.acctSvc.getBalance().subscribe(res => {
      this.balance = res.data.balance;
      this.currency = res.data.currency;
    });

    this.benSvc.getAll().subscribe(res => {
      this.beneficiaries = res.data ?? [];
    });
  }

  totalWithFee(): number {
    return this.form.amount + this.form.amount * 0.005;
  }

  selectBen(b: Beneficiary): void {
    this.selectedBen = b.beneficiaryId;
    this.form.receiverAccountNumber = b.beneficiaryAccountNumber;
  }

  submit(): void {
    this.submitted = true;
    if (!this.form.receiverAccountNumber || !this.form.amount || this.form.amount <= 0) return;

    this.loading = true;
    this.error = '';

    this.txSvc.transfer(this.form).subscribe({
      next: r => {
        this.loading = false;
        this.successTx = r.data;
        this.success = true;
        this.balance = r.data.balanceAfter;
      },
      error: err => {
        this.loading = false;
        this.error = err?.error?.message ?? 'Transfer failed. Please try again.';
      }
    });
  }

  reset(): void {
    this.form = { receiverAccountNumber: '', amount: 0, description: '', category: 'Transfer' };
    this.success = false;
    this.submitted = false;
    this.selectedBen = null;
  }
}
