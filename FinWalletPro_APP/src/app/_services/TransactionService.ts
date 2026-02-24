import { Transaction } from './../_models/index';
// src/app/custom-name.service.ts
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root' // Makes the service a singleton available throughout the application
})
export class TransactionService {

  constructor() {
    // Initialization logic can go here
  }

  // Example method for the service's functionality
  public getData(): string[] {
    return ['data item 1', 'data item 2'];
  }
}
