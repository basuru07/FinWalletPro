# Password Hashing with SALT & JWT Refresh Token API

A secure authentication system built with ASP.NET Core that implements password hashing with SALT and JWT-based authentication with refresh tokens, using Oracle database.

## 🔐 Security Features

- **Password Hashing with SALT**: Secure password storage using industry-standard hashing algorithms
- **JWT Access Tokens**: Short-lived tokens for API authentication
- **Refresh Tokens**: Long-lived tokens for obtaining new access tokens without re-authentication
- **Oracle Database**: Enterprise-grade data persistence

## 🏗️ Architecture

This project follows **Clean Architecture** principles with clear separation of concerns:

```
┌─────────────────────────────────────────┐
│     Presentation Layer (API)            │
│  ├── Controllers                        │
│  └── DTOs                               │
├─────────────────────────────────────────┤
│     Business Logic Layer (Core)         │
│  ├── Services                           │
│  ├── Interfaces                         │
│  └── Models/Entities                    │
├─────────────────────────────────────────┤
│     Data Access Layer (Infrastructure)  │
│  ├── Repositories                       │
│  └── Database Connection                │
└─────────────────────────────────────────┘
```

## 📁 Project Structure - BACKEND

```
Refresh_Token_APK/
├── Refreshtoken.API/              # Presentation Layer
│   ├── Controllers/
│   │   └── AuthController.cs      # Authentication endpoints
│   ├── DTOs/
│   │   ├── LoginRequest.cs
│   │   ├── SignupRequest.cs
│   │   ├── AuthResponse.cs
│   │   └── RefreshTokenRequest.cs
│   ├── Program.cs                 # Application configuration & DI
│   └── appsettings.json           # Configuration settings
│
├── Refreshtoken.Core/             # Business Logic Layer
│   ├── Models/
│   │   └── User.cs
│   ├── Entities/
│   │   └── RefreshToken.cs
│   ├── Services/
│   │   ├── IAuthService.cs
│   │   ├── AuthService.cs
│   │   ├── ITokenService.cs
│   │   ├── TokenService.cs
│   │   ├── IPasswordHasher.cs
│   │   └── PasswordHasher.cs
│   ├── Interfaces/
│   │   ├── IUserRepository.cs
│   │   └── IRefreshTokenRepository.cs
│
└── Refreshtoken.Infrastructure/   # Data Access Layer
    ├── OracleConnectionFactory.cs
    └── Repositories/
        ├── UserRepository.cs
        └── RefreshTokenRepository.cs
```
# Folder Structure - FRONTEND

```
src/
├── app/
│   ├── _components/
│   │   ├── login/
│   │   │   ├── login.component.ts
│   │   │   ├── login.component.html
│   │   │   ├── login.component.css
│   │   │   └── login.component.spec.ts
│   │   ├── signup/
│   │   │   ├── signup.component.ts
│   │   │   ├── signup.component.html
│   │   │   ├── signup.component.css
│   │   │   └── signup.component.spec.ts
│   │   ├── dashboard/
│   │   │   ├── dashboard.component.ts
│   │   │   ├── dashboard.component.html
│   │   │   ├── dashboard.component.css
│   │   │   └── dashboard.component.spec.ts
│   │   └── nav/
│   │       ├── nav.component.ts
│   │       ├── nav.component.html
│   │       └── nav.component.css
│   ├── _models/
│   │   ├── signup-request.model.ts
│   │   ├── login-request.model.ts
│   │   ├── refresh-request.model.ts
│   │   ├── auth-response.model.ts
│   │   └── user.model.ts
│   ├── _services/
│   │   ├── auth.service.ts
│   │   └── token.service.ts
│   ├── _guards/
│   │   └── auth.guard.ts
│   ├── _interceptors/
│   │   └── auth.interceptor.ts
│   ├── app.component.ts
│   ├── app.component.html
│   ├── app.component.css
│   ├── app.routes.ts
│   └── app.config.ts
├── environments/
│   ├── environment.ts
│   └── environment.development.ts
└── main.ts
```




## 🔄 Password Hashing Flow

### Registration Flow

```
1️⃣ User enters password → "Admin@123"
2️⃣ Generate random SALT → "X8f9K2LmPq12..."
3️⃣ Combine Password + SALT → "Admin@123" + SALT
4️⃣ Apply Hash Algorithm → BCrypt/PBKDF2/SHA256
5️⃣ Store in Database → SALT.HASH (NOT the real password)
```

### Login Verification Flow

```
6️⃣ User enters password
7️⃣ System retrieves stored SALT + HASH
8️⃣ System hashes entered password using SAME SALT
9️⃣ Compare: New Hash == Stored Hash ?
   ✔️ Yes → Generate JWT tokens
   ❌ No → Return invalid credentials
```

## 🚀 Getting Started

### Prerequisites

- .NET 8.0 SDK or later
- Oracle Database (19c or later)
- Visual Studio 2022 or VS Code

