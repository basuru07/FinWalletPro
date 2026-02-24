import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { AccountDetails, ApiResponse } from '../_models';

@Injectable({
  providedIn: 'root' // Makes the service a singleton available throughout the application
})
export class AccountService {
  // Backend URL
  private readonly api = `${environment.apiUrl}/account`;

  constructor(private http: HttpClient) { }

  getMyAccount(): Observable<ApiResponse<AccountDetails>>{
    return this.http.get<ApiResponse<AccountDetails>>(`${this.api}/me`)
  }
}
