// ------------------------- Auth -------------------------
export interface LoginRequest {
  email: string;
  password: string;
}

export interface RegisterRequest {
  fullName: string;
  email: string;
  password: string;
  confirmPassword: string;
  phoneNumber?: string;
}

export interface AuthResponse {
  accessToken: string;
  refreshToken: string;
  tokenType: string;
  expiresIn: number;
  account: AccountSummary;
}

export interface AccountSummary {
  accountId: number;
  fullName: string;
  email: string;
  accountNumber: string;
  phoneNumber: string;
  balance: number;
  currency: string;
  accountStatus: string;
}

// ------------------------- Account -------------------------
export interface AccountDetails extends AccountSummary {
  createdAt: string;
  bankCards: BankCard[];
}
export interface BalanceResponse {
  accountId: number;
  accountNumber: string;
  balance: number;
  currency: string;
  asOf: string;
}
export interface BankCard {
  cardId: number;
  cardHolderName: string;
  cardNumberMasked: string;
  cardNumber: string;
  expiryMonth: number;
  expiryYear: 5437;
  cardType: string;
  cardCategory: string;
  bankName: string;
  isDefault: boolean;
  linkedAt: string;
}
export interface AddBankCardRequest {
  cardHolderName: string;
  cardNumber: string;
  expiryMonth: string;
  expiryYear: string;
  cardType: string;
  cardCategory: string;
  bankName?: string;
}

// ------------------------- Transaction -------------------------
export interface Transaction {
  id(id: any, reverseReason: string): unknown;
  transactionId: number;
  transactionReference: string;
  senderName: string;
  senderAccountNumber: string;
  receiverName: string;
  receiverAccountNumber: string;
  amount: number;
  fee: number;
  totalAmount: number;
  currency: string;
  transactionType: string;
  status: string;
  description: string;
  category: string;
  balanceAfter: number;
  transactionDate: string;
}

export interface TransferRequest {
  receiverAccountNumber: string;
  amount: number;
  description: string;
  category: string;
}
export interface DepositRequest {
  amount: number;
  description: string;
}

export interface WithdrawRequest {
  amount: number;
  description: string;
}

export interface TransactionFilter {
  fromDate: string;
  toDate: string;
  transactionType: string;
  status: string;
  minAmount: number;
  maxAmount: number;
  category: string;
  pageNumber: number;
  pageSize: number;
}

export interface StatementSummary {
  totalTransactions: number;
  totalSent: number;
  totalReceived: number;
  fromDate: string;
  toDate: string;
}
// ------------------------- Beneficiary -------------------------
export interface Beneficiary {
  beneficiaryId: number;
  beneficiaryName: string;
  beneficiaryAccountNumber: string;
  nickName: string;
  beneficiaryEmail: string;
  beneficiaryPhone: string;
  bankName: string;
  createdAt: string;
}

export interface AddBeneficiaryRequest {
  beneficiaryName: string;
  beneficiaryAccountNumber: string;
  nickName: string;
  beneficiaryEmail: string;
  beneficiaryPhone: string;
  bankName: string;
}

// ------------------------- Beneficiary -------------------------
export interface Notification{
  notificationId: number;
  title: string;
  message: string;
  notificationType: string;
  isRead: boolean;
  referenceId: string;
  referenceType: string;
  createdAt: string;
  readAt?: string;
}

// ------------------------- API -------------------------
export interface ApiResponse<T>{
  success: boolean;
  message: string;
  data: T;
  errors?: string[];
}

// ------------------------- UI Helpers -------------------------
export interface PageResult<T>{
  page: number;
  pageSize: number;
  transactions: T[];
}


