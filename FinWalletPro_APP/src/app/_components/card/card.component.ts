import { Component, OnInit } from '@angular/core';
import { AccountService } from 'src/app/_services/AccountService';
import { BankCard } from 'src/app/_models'; // ← use the real model, not a local copy

@Component({
  selector: 'app-card',
  templateUrl: './card.component.html',
  styleUrls: ['./card.component.css'],
})
export class CardComponent implements OnInit {
  cards: BankCard[] = [];
  loading: boolean = true;
  modal: boolean = false;
  modalLoading: boolean = false;
  modalError: string = '';
  deleteTarget: BankCard | null = null;
  deleteLoading: boolean = false;

  // expiryMonth and expiryYear are numbers to match the BankCard model
  form: any = { cardType: 'Visa', cardCategory: 'Debit', expiryMonth: 1 };
  months = [1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12];
  years = Array.from({ length: 15 }, (_, i) => new Date().getFullYear() + i);

  constructor(private acctSvc: AccountService) {}

  ngOnInit(): void {
    this.acctSvc.getBankCards().subscribe({
      next: (r) => {
        this.cards = r.data ?? [];
        this.loading = false;
      },
      error: () => {
        this.loading = false;
      },
    });
    this.form.expiryYear = this.years[0];
  }

  openAdd(): void {
    this.form = {
      cardType: 'Visa',
      cardCategory: 'Debit',
      expiryMonth: 1,
      expiryYear: this.years[0],
    };
    this.modalError = '';
    this.modal = true;
  }

  cardTheme(card: BankCard): string {
    const t = card.cardType?.toLowerCase();
    if (t === 'visa') return 'card-visual visa';
    if (t === 'mastercard') return 'card-visual mastercard';
    if (t === 'amex') return 'card-visual amex';
    return 'card-visual other';
  }

  addCard(): void {
    if (!this.form.cardHolderName || !this.form.cardNumber) {
      this.modalError = 'Card holder name and number are required.';
      return;
    }
    this.modalLoading = true;
    this.acctSvc.addBankCard(this.form).subscribe({
      next: (r) => {
        this.cards = [...this.cards, r.data];
        this.modalLoading = false;
        this.modal = false;
      },
      error: (err) => {
        this.modalLoading = false;
        this.modalError = err?.error?.message ?? 'Failed to add card.';
      },
    });
  }

  setDefault(cardId: number): void {
    this.acctSvc.setDefaultCard(cardId).subscribe({
      next: () => {
        this.cards = this.cards.map((c) => ({
          ...c,
          isDefault: c.cardId === cardId,
        }));
      },
    });
  }

  removeCard(): void {
    const c = this.deleteTarget;
    if (!c) return;
    this.deleteLoading = true;
    this.acctSvc.removeBankCard(c.cardId).subscribe({
      next: () => {
        this.cards = this.cards.filter((x) => x.cardId !== c.cardId);
        this.deleteLoading = false;
        this.deleteTarget = null;
      },
      error: () => {
        this.deleteLoading = false;
      },
    });
  }
}
