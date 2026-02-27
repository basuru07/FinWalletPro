import { environment } from 'src/environments/environment';
import {
  ApiResponse,
  DepositRequest,
  Transaction,
  TransactionFilter,
  TransferRequest,
  WithdrawRequest,
  PageResult,
} from './../_models/index';

import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({ providedIn: 'root' })
export class TransactionService {
  getTransactionById(id: string) {
    throw new Error('Method not implemented.');
  }
  reverseTransaction(id: any, reverseReason: string) {
    throw new Error('Method not implemented.');
  }
  // Backend URL
  private readonly api = `${environment.apiUrl}/transaction`;

  constructor(private http: HttpClient) {}

  // Transfer transactions
  transfer(payload: TransferRequest): Observable<ApiResponse<Transaction>> {
    return this.http.post<ApiResponse<Transaction>>(
      `${this.api}/transfer`,
      payload,
    );
  }

  // Deposit transaction
  deposit(payload: DepositRequest): Observable<ApiResponse<Transaction>> {
    return this.http.post<ApiResponse<Transaction>>(
      `${this.api}/deposit`,
      payload,
    );
  }

  // Withdraw transaction
  withdraw(payload: WithdrawRequest): Observable<ApiResponse<Transaction>> {
    return this.http.post<ApiResponse<Transaction>>(
      `${this.api}/withdraw`,
      payload,
    );
  }

  // Get history
  getHistory(
    filter: TransactionFilter = {
      fromDate: '',
      toDate: '',
      transactionType: '',
      status: '',
      minAmount: 0,
      maxAmount: 0,
      category: '',
      pageNumber: 0,
      pageSize: 0,
    },
  ): Observable<ApiResponse<PageResult<Transaction>>> {
    let params = new HttpParams();
    Object.entries(filter).forEach(([k, v]) => {
      if (v !== undefined && v !== null && v !== '')
        params = params.set(k, String(v));
    });
    return this.http.get<ApiResponse<PageResult<Transaction>>>(
      `${this.api}/history`,
      { params },
    );
  }

  // Get transaction statement
  getStatement(from: string, to: string): Observable<ApiResponse<any>> {
    return this.http.get<ApiResponse<any>>(
      `${this.api}/statement?fromDate=${from}&toDate=${to}`,
    );
  }

  // Get transaction by ID
  getById(id: number): Observable<ApiResponse<Transaction>> {
    return this.http.get<ApiResponse<Transaction>>(`${this.api}/${id}`);
  }

  // Reverse transaction
  reverse(id: number, reason: string): Observable<ApiResponse<any>> {
    return this.http.post<ApiResponse<any>>(`${this.api}/${id}/reverse`, {
      reason,
    });
  }
}
