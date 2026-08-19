# 🏗️ BuildFlow API

A **multi-tenant construction management** backend built with **ASP.NET Core (.NET 10)** following **Clean Architecture** and **CQRS** principles.

[![.NET](https://img.shields.io/badge/.NET-10.0-512BD4?logo=dotnet)](https://dotnet.microsoft.com/)
[![C#](https://img.shields.io/badge/C%23-14.0-239120?logo=csharp)](https://learn.microsoft.com/en-us/dotnet/csharp/)
[![License](https://img.shields.io/badge/license-Private-red)]()

---

## 📋 Table of Contents

- [Overview](#-overview)
- [Architecture](#-architecture)
- [Project Structure](#-project-structure)
- [Tech Stack](#-tech-stack)
- [Features](#-features)
- [Prerequisites](#-prerequisites)
- [Getting Started](#-getting-started)
- [Configuration](#-configuration)
- [Database Migrations](#-database-migrations)
- [API Documentation](#-api-documentation)
- [Testing](#-testing)

---

## 📖 Overview

BuildFlow API is a multi-tenant SaaS backend for managing construction workflows. Each company registers with a **unique subdomain** and gets an isolated tenant environment. Authentication is handled via **JWT access tokens** paired with **refresh tokens**.

---

## 🏛️ Architecture

The solution follows **Clean Architecture** — dependencies always point inward.

```
BuildFlow.API  ──►  BuildFlow.Application  ──►  BuildFlow.Domain
     │                      │                         │
     ▼                      ▼                         ▼
BuildFlow.Infrastructure  BuildFlow.Persistence  BuildFlow.SharedKernel
```

Every API request is dispatched through **MediatR** as a `Command` or `Query`. Responses are wrapped in a typed `Result<T>` to enforce explicit error handling throughout all layers.

---

## 📁 Project Structure

```
buildflow-api/
│
├── src/
│   ├── BuildFlow.API/              # HTTP layer – Controllers, Middleware, Swagger, Program.cs
│   ├── BuildFlow.Application/      # Use cases – Commands, Queries, Handlers, Validators
│   ├── BuildFlow.Contracts/        # Shared request/response DTOs
│   ├── BuildFlow.Domain/           # Entities, aggregates, domain rules
│   ├── BuildFlow.Infrastructure/   # JWT service, Serilog configuration
│   ├── BuildFlow.Persistence/      # EF Core DbContext, Migrations, Identity setup
│   └── BuildFlow.SharedKernel/     # Result<T>, Error, Exceptions, base types
│
└── tests/
    ├── BuildFlow.UnitTests/         # Unit tests – xUnit, Moq, FluentAssertions
    └── BuildFlow.IntegrationTests/  # Integration tests – xUnit, Testcontainers, WebApplicationFactory
```

---

## 🛠️ Tech Stack

| Category            | Technology                                           |
|---------------------|------------------------------------------------------|
| Framework           | ASP.NET Core (.NET 10)                               |
| Language            | C# 14                                                |
| Architecture        | Clean Architecture, CQRS                             |
| Mediator            | MediatR                                              |
| Validation          | FluentValidation                                     |
| ORM                 | Entity Framework Core                                |
| Database            | SQL Server / LocalDB                                 |
| Authentication      | ASP.NET Core Identity + JWT Bearer                   |
| Logging             | Serilog (Console, File, request logging)             |
| API Documentation   | Swagger / Swashbuckle                                |
| API Versioning      | Asp.Versioning.Mvc                                   |
| Unit Testing        | xUnit, Moq, FluentAssertions, Coverlet               |
| Integration Testing | xUnit, Testcontainers (PostgreSQL), FluentAssertions |

---

## ✅ Features

- 🏢 **Multi-tenant registration** — each company gets a unique subdomain
- 🔐 **JWT authentication** — access token + refresh token flow
- 👥 **Role-based authorization** — `TenantAdmin` role (extensible)
- ⚡ **CQRS + MediatR** — clean separation of reads and writes
- 📦 **Result pattern** — typed `Result<T>` and `Error` for consistent responses
- 🛡️ **Global exception middleware** — centralized error handling
- 📄 **API versioning** — URL-based (`/api/v1/...`)
- 📊 **Structured logging** — Serilog with enriched request logs
- ❤️ **Health checks** — `/health` endpoint
- 📘 **Swagger UI** — interactive API docs (development only)
- 🌐 **CORS** — configurable cross-origin policy

---

## 📦 Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0)
- [SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads) or SQL Server LocalDB
- [Docker Desktop](https://www.docker.com/products/docker-desktop/) *(required for integration tests — Testcontainers)*

---

## 🚀 Getting Started

### 1. Clone the repository

```bash
git clone https://github.com/hysnyasir/buildflow-api.git
cd buildflow-api
```

### 2. Restore dependencies

```bash
dotnet restore
```

### 3. Configure secrets

Use **User Secrets** to avoid committing sensitive values:

```powershell
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Server=(localdb)\mssqllocaldb;Database=buildflow;Trusted_Connection=True;" --project src/BuildFlow.API

dotnet user-secrets set "Jwt:Key" "YOUR_STRONG_SECRET_KEY_MINIMUM_32_CHARACTERS" --project src/BuildFlow.API
```

### 4. Apply database migrations

```powershell
dotnet ef database update `
  --project src/BuildFlow.Persistence `
  --startup-project src/BuildFlow.API
```

### 5. Run the application

```bash
dotnet run --project src/BuildFlow.API
```

The API will be available at:

| URL                              | Description              |
|----------------------------------|--------------------------|
| `https://localhost:7xxx`         | HTTPS                    |
| `http://localhost:5xxx`          | HTTP                     |
| `https://localhost:7xxx/swagger` | Swagger UI (Dev only)    |
| `https://localhost:7xxx/health`  | Health check endpoint    |

---

## ⚙️ Configuration

`src/BuildFlow.API/appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=buildflow;Trusted_Connection=True;MultipleActiveResultSets=true"
  },
  "Jwt": {
    "Key": "REPLACE_WITH_STRONG_SECRET_MIN_32_CHARS_FROM_KEY_VAULT",
    "Issuer": "BuildFlow",
    "Audience": "BuildFlow",
    "ExpiryMinutes": 60,
    "RefreshTokenExpiryDays": 7
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning",
      "Microsoft.EntityFrameworkCore": "Warning"
    }
  }
}
```

> ⚠️ **Never commit real secrets.** Use User Secrets locally and Azure Key Vault / environment variables in production.

---

## 🗄️ Database Migrations

```powershell
# Add a new migration
dotnet ef migrations add <MigrationName> `
  --project src/BuildFlow.Persistence `
  --startup-project src/BuildFlow.API

# Apply pending migrations
dotnet ef database update `
  --project src/BuildFlow.Persistence `
  --startup-project src/BuildFlow.API

# Revert last migration
dotnet ef migrations remove `
  --project src/BuildFlow.Persistence `
  --startup-project src/BuildFlow.API
```

---

## 📘 API Documentation

All endpoints are versioned under `/api/v1/`. Swagger UI is available in Development at `/swagger`.

### Authentication Endpoints

| Method | Endpoint                | Description                        | Auth Required |
|--------|-------------------------|------------------------------------|:-------------:|
| POST   | `/api/v1/auth/register` | Register a new tenant + admin user | ❌            |
| POST   | `/api/v1/auth/login`    | Authenticate and receive tokens    | ❌            |
| POST   | `/api/v1/auth/refresh`  | Refresh access token               | ❌            |

### Register Request Example

```json
{
  "companyName": "Acme Construction",
  "subdomain": "acme",
  "fullName": "John Doe",
  "email": "john@acme.com",
  "password": "P@ssw0rd123!"
}
```

### Auth Response Example

```json
{
  "userId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "tenantId": "7cb0a932-1234-5678-abcd-ef1234567890",
  "fullName": "John Doe",
  "email": "john@acme.com",
  "role": "TenantAdmin",
  "accessToken": "eyJhbGci...",
  "accessTokenExpiry": "2026-08-17T13:00:00Z",
  "refreshToken": "dGhpcyBpcyBh...",
  "refreshTokenExpiry": "2026-08-24T12:00:00Z"
}
```

---

## 🧪 Testing

### Unit Tests

```bash
dotnet test tests/BuildFlow.UnitTests
```

### Integration Tests

> ⚠️ Requires Docker Desktop running — Testcontainers spins up a PostgreSQL container automatically.

```bash
dotnet test tests/BuildFlow.IntegrationTests
```

### All Tests with Coverage

```bash
dotnet test --collect:"XPlat Code Coverage"
```

---

## 📄 License

This project is private. All rights reserved © BuildFlow.
