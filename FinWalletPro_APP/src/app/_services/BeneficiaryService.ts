import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

import { environment } from 'src/environments/environment';
import { AddBeneficiaryRequest, ApiResponse, Beneficiary } from '../_models';

@Injectable({ providedIn: 'root' })
export class BeneficiaryService {
  // Backend URL
  private readonly api = `${environment.apiUrl}/beneficiary`;

  constructor(private http: HttpClient) {}

  // Get the all Beneficiary records
  getAll(): Observable<ApiResponse<Beneficiary[]>> {
    return this.http.get<ApiResponse<Beneficiary[]>>(this.api);
  }

  // Get the Beneficiary record by Id
  getById(id: number): Observable<ApiResponse<Beneficiary>> {
    return this.http.get<ApiResponse<Beneficiary>>(`${this.api}/${id}`);
  }

  // Add Beneficiary record
  add(payload: AddBeneficiaryRequest): Observable<ApiResponse<Beneficiary>> {
    return this.http.post<ApiResponse<Beneficiary>>(this.api, payload);
  }

  // Update Beneficiary record
  update(
    id: number,
    payload: Partial<AddBeneficiaryRequest>,
  ): Observable<ApiResponse<Beneficiary>> {
    return this.http.put<ApiResponse<Beneficiary>>(
      `${this.api}/${id}`,
      payload,
    );
  }

  // Remove the Beneficiary card
  remove(id: number): Observable<ApiResponse<any>> {
    return this.http.delete<ApiResponse<any>>(`${this.api}/${id}`);
  }
}
