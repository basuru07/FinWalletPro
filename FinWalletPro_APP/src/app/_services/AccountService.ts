import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { AccountDetails, AddBankCardRequest, ApiResponse, BalanceResponse, BankCard } from '../_models';

@Injectable({ providedIn: 'root'})

export class AccountService {
  // Backend URL
  private readonly api = `${environment.apiUrl}/account`;

  constructor(private http: HttpClient) { }

  // Get account
  getMyAccount(): Observable<ApiResponse<AccountDetails>>{
    return this.http.get<ApiResponse<AccountDetails>>(`${this.api}/me`)
  }

  // Get balance
  getBalance(): Observable<ApiResponse<BalanceResponse>>{
    return this.http.get<ApiResponse<BalanceResponse>>(`${this.api}/balance`);
  }

  // Update account
  updateAccount(payload: { fullName: string; phoneNmber: string}): Observable<ApiResponse<AccountDetails>>{
    return this.http.put<ApiResponse<AccountDetails>>(`${this.api}/me`, payload);
  }

  // Get the Bank Cards
  getBankCards(): Observable<ApiResponse<BankCard[]>> {
    return this.http.get<ApiResponse<BankCard[]>>(`${this.api}/cards`);
  }

  // Add the Bank Cards
  addBankCard(payload: AddBankCardRequest): Observable<ApiResponse<BankCard>> {
    return this.http.post<ApiResponse<BankCard>>(`${this.api}/cards`, payload);
  }

  // Remove bank cards
  removeBankCard(cardId: number): Observable<ApiResponse<any>> {
    return this.http.delete<ApiResponse<any>>(`${this.api}/cards/${cardId}`);
  }

  // Default Account cards
  setDefaultCard(cardId: number): Observable<ApiResponse<any>> {
    return this.http.patch<ApiResponse<any>>(`${this.api}/cards/${cardId}/set-default`, {});
  }
}
