# 💳 FinWalletPro — Digital Wallet Platform

> A full-stack digital wallet solution built with ASP.NET Core (API) and Angular 17 (Frontend).

---

## 📋 Features

| # | Feature |
|---|---------|
| 01 | Register and manage accounts |
| 02 | Add/link bank cards and payment methods |
| 03 | Transfer money between wallets |
| 04 | View transaction history with filtering |
| 05 | Check account balance and statements |
| 06 | Manage beneficiaries |
| 07 | Receive real-time notifications |

---

## 🗂️ Solution Structure

### Backend — `FinWalletPro.sln`

```
FinWalletPro.sln
│
├── FinWalletPro_API/
│   ├── Controllers/
│   │   ├── AuthController.cs
│   │   ├── AccountController.cs
│   │   ├── TransactionController.cs
│   │   ├── BeneficiaryController.cs
│   │   └── NotificationController.cs
│   │
│   ├── DTOs/
│   │   ├── AuthDTOs.cs
│   │   ├── AccountDTOs.cs
│   │   ├── TransactionDTOs.cs
│   │   ├── BeneficiaryDTOs.cs
│   │   └── NotificationDTOs.cs
│   │
│   ├── Middleware/
│   │   └── ExceptionMiddleware.cs
│   │
│   ├── appsettings.json
│   └── Program.cs
│
├── FinWalletPro_Core/
│   ├── Interface/
│   │   ├── IAccountService.cs
│   │   ├── ITransactionService.cs
│   │   ├── IBeneficiaryService.cs
│   │   └── INotificationService.cs
│   │
│   ├── Models/
│   │   ├── Account.cs
│   │   ├── Transaction.cs
│   │   ├── Beneficiary.cs
│   │   ├── Notification.cs
│   │   └── BankCard.cs
│   │
│   └── Services/
│       ├── AccountService.cs
│       ├── TransactionService.cs
│       ├── BeneficiaryService.cs
│       └── NotificationService.cs
│
└── FinWalletPro_Infrastructure/
    ├── Data/
    │   └── WalletDbContext.cs
    │
    └── Repositories/
        ├── AccountRepository.cs
        ├── TransactionRepository.cs
        ├── BeneficiaryRepository.cs
        └── NotificationRepository.cs
```

#### Backend Layer Responsibilities

| Layer | Responsibility |
|-------|----------------|
| `FinWalletPro_API` | HTTP entry points — controllers, DTOs, middleware, startup config |
| `FinWalletPro_Core` | Business logic — interfaces, domain models, service implementations |
| `FinWalletPro_Infrastructure` | Data access — EF Core DbContext and repository implementations |

---

### Frontend — `finwallet-frontend/` (Angular 17)

```
finwallet-frontend/
├── angular.json                  ← Workspace config
├── package.json                  ← Angular 17 dependencies
├── proxy.conf.json               ← Dev proxy → localhost:5000
├── tsconfig.json
│
└── src/
    ├── index.html
    ├── main.ts
    ├── styles.css                 ← Global design system (Syne + DM Sans fonts)
    ├── environments/
    │   └── environment.ts
    │
    └── app/
        ├── app.component.ts       ← Root component (router-outlet)
        ├── app-shell.component.ts ← Sidebar + <router-outlet> layout
        ├── app.module.ts          ← Module wiring
        ├── app-routing.module.ts
        │
        ├── core/
        │   ├── guards/            auth.guard + guest.guard
        │   ├── interceptors/      jwt.interceptor (Bearer token + 401 handler)
        │   └── services/          auth.service + api.services (wallet, txn, card, bene)
        │
        ├── shared/
        │   ├── models/            models.ts (interfaces matching backend DTOs)
        │   └── components/        sidebar (html + css + ts)
        │
        └── features/
            ├── auth/              Login + Register (split-panel luxury design)
            ├── dashboard/         Balance card + quick actions + recent transactions
            ├── wallet/            Full wallet detail + activity table
            ├── transactions/
            │   ├── transfer/      Send money form + success state
            │   ├── deposit/       Fund from card
            │   ├── withdraw/      To card with fee preview
            │   └── history/       Filterable paginated table
            ├── cards/             Visual card display + add/remove
            ├── beneficiaries/     Saved recipients + quick-send
            └── profile/           Edit name/phone, view wallet info
```

#### Frontend Module Breakdown

| Module | Description |
|--------|-------------|
| `core/guards` | Route protection — `auth.guard` (requires login), `guest.guard` (redirects if logged in) |
| `core/interceptors` | JWT injection on every API request; handles 401 auto-logout |
| `core/services` | Centralised API service layer for all backend communication |
| `shared/models` | TypeScript interfaces aligned with backend DTOs |
| `features/auth` | Login & registration with split-panel luxury UI |
| `features/dashboard` | Home view: balance snapshot, quick actions, recent activity |
| `features/transactions` | Full transaction flows: transfer, deposit, withdraw, history |
| `features/cards` | Visual bank card management — add, view, remove |
| `features/beneficiaries` | Manage saved recipients with quick-send shortcut |
| `features/profile` | User profile editing and wallet information |

---

## 🏗️ Architecture Overview

```
Angular 17 Frontend
        │
        │  HTTP + JWT Bearer Token
        ▼
ASP.NET Core Web API  (Controllers → Services → Repositories)
        │
        │  Entity Framework Core
        ▼
    SQL Database  (WalletDbContext)
```

---

## 🛠️ Tech Stack

| Layer | Technology |
|-------|-----------|
| Frontend | Angular 17, TypeScript, Syne + DM Sans (fonts) |
| Backend API | ASP.NET Core, C# |
| ORM | Entity Framework Core |
| Auth | JWT Bearer Tokens |
| Architecture | Clean Architecture (API / Core / Infrastructure) |
| Pattern | Repository Pattern + Service Layer |
