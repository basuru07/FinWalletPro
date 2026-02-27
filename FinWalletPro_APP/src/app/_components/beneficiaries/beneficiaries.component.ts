import { Component, OnInit } from '@angular/core';
import { BeneficiaryService } from 'src/app/_services/BeneficiaryService';

interface Beneficiary {
  beneficiaryId: number;
  beneficiaryName: string;
  nickName?: string;
  beneficiaryAccountNumber: string;
  bankName?: string;
  beneficiaryEmail?: string;
  beneficiaryPhone?: string;
}

@Component({
  selector: 'app-beneficiaries',
  templateUrl: './beneficiaries.component.html',
  styleUrls: ['./beneficiaries.component.css']
})
export class BeneficiariesComponent implements OnInit {

  beneficiaries: Beneficiary[] = [];
  loading = true;

  modal = false;
  editingId: number | null = null;
  modalLoading = false;
  modalError = '';

  deleteTarget: Beneficiary | null = null;
  deleteLoading = false;

  form: any = {};

  constructor(private benSvc: BeneficiaryService) {}

  ngOnInit(): void { this.load(); }

  load(): void {
    this.loading = true;
    this.benSvc.getAll().subscribe({
      next: r => { this.beneficiaries = r.data ?? []; this.loading = false; },
      error: () => this.loading = false
    });
  }

  openAdd(): void {
    this.form = {};
    this.editingId = null;
    this.modalError = '';
    this.modal = true;
  }

  editBen(b: Beneficiary): void {
    this.form = { ...b };
    this.editingId = b.beneficiaryId;
    this.modalError = '';
    this.modal = true;
  }

  closeModal(): void { this.modal = false; }

  save(): void {
    if (!this.form.beneficiaryName || !this.form.beneficiaryAccountNumber) {
      this.modalError = 'Name and account number are required.';
      return;
    }
    this.modalLoading = true;
    const obs = this.editingId
      ? this.benSvc.update(this.editingId, this.form)
      : this.benSvc.add(this.form);

    obs.subscribe({
      next: () => { this.modalLoading = false; this.closeModal(); this.load(); },
      error: err => { this.modalLoading = false; this.modalError = err?.error?.message || 'Failed to save beneficiary.'; }
    });
  }

  confirmDelete(b: Beneficiary): void { this.deleteTarget = b; }

  doDelete(): void {
    if (!this.deleteTarget) return;
    this.deleteLoading = true;
    this.benSvc.remove(this.deleteTarget.beneficiaryId).subscribe({
      next: () => { this.deleteLoading = false; this.deleteTarget = null; this.load(); },
      error: () => this.deleteLoading = false
    });
  }
}
