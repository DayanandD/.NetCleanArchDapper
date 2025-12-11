# .NetCleanArchDapper
NET Clean Architecture with Dapper  A modular, maintainable, and high-performance backend template built using .NET, following Clean Architecture principles, and powered by the lightweight and super-fast Dapper ORM. This project is designed for developers who want

# ================================
# CREATE SOLUTION
# ================================
dotnet new sln -n NETCleanArch

# ================================
# CREATE API PROJECT (Nested)
# ================================
mkdir NETCleanArch.Api
cd NETCleanArch.Api
dotnet new webapi -n NETCleanArch.Api
cd ..

# ================================
# CREATE APPLICATION PROJECT (Nested)
# ================================
mkdir NETCleanArch.Application
cd NETCleanArch.Application
dotnet new classlib -n NETCleanArch.Application
cd ..

# ================================
# CREATE DOMAIN PROJECT (Nested)
# ================================
mkdir NETCleanArch.Domain
cd NETCleanArch.Domain
dotnet new classlib -n NETCleanArch.Domain
cd ..

# ================================
# CREATE INFRASTRUCTURE PROJECT (Nested)
# ================================
mkdir NETCleanArch.Infrastructure
cd NETCleanArch.Infrastructure
dotnet new classlib -n NETCleanArch.Infrastructure
cd ..

# ================================
# CREATE WORKER PROJECT (Nested)
# ================================
mkdir NETCleanArch.Worker
cd NETCleanArch.Worker
dotnet new worker -n NETCleanArch.Worker
cd ..

# ================================
# ADD PROJECTS TO SOLUTION
# ================================
dotnet sln add NETCleanArch.Api/NETCleanArch.Api/NETCleanArch.Api.csproj
dotnet sln add NETCleanArch.Application/NETCleanArch.Application/NETCleanArch.Application.csproj
dotnet sln add NETCleanArch.Domain/NETCleanArch.Domain/NETCleanArch.Domain.csproj
dotnet sln add NETCleanArch.Infrastructure/NETCleanArch.Infrastructure/NETCleanArch.Infrastructure.csproj
dotnet sln add NETCleanArch.Worker/NETCleanArch.Worker/NETCleanArch.Worker.csproj

# ================================
# ADD CLEAN ARCHITECTURE REFERENCES
# ================================

# API → Application + Infrastructure
dotnet add NETCleanArch.Api/NETCleanArch.Api/NETCleanArch.Api.csproj reference \
  NETCleanArch.Application/NETCleanArch.Application/NETCleanArch.Application.csproj

dotnet add NETCleanArch.Api/NETCleanArch.Api/NETCleanArch.Api.csproj reference \
  NETCleanArch.Infrastructure/NETCleanArch.Infrastructure/NETCleanArch.Infrastructure.csproj

# Application → Domain
dotnet add NETCleanArch.Application/NETCleanArch.Application/NETCleanArch.Application.csproj reference \
  NETCleanArch.Domain/NETCleanArch.Domain/NETCleanArch.Domain.csproj

# Infrastructure → Application + Domain
dotnet add NETCleanArch.Infrastructure/NETCleanArch.Infrastructure/NETCleanArch.Infrastructure.csproj reference \
  NETCleanArch.Application/NETCleanArch.Application/NETCleanArch.Application.csproj

dotnet add NETCleanArch.Infrastructure/NETCleanArch.Infrastructure/NETCleanArch.Infrastructure.csproj reference \
  NETCleanArch.Domain/NETCleanArch.Domain/NETCleanArch.Domain.csproj

# Worker → Application + Infrastructure
dotnet add NETCleanArch.Worker/NETCleanArch.Worker/NETCleanArch.Worker.csproj reference \
  NETCleanArch.Application/NETCleanArch.Application/NETCleanArch.Application.csproj

dotnet add NETCleanArch.Worker/NETCleanArch.Worker/NETCleanArch.Worker.csproj reference \
  NETCleanArch.Infrastructure/NETCleanArch.Infrastructure/NETCleanArch.Infrastructure.csproj

# ================================
# INSTALL DEPENDENCIES
# ================================

# ---- PostgreSQL (Npgsql) ----
dotnet add NETCleanArch.Infrastructure/NETCleanArch.Infrastructure/NETCleanArch.Infrastructure.csproj package Npgsql
dotnet add NETCleanArch.Infrastructure/NETCleanArch.Infrastructure/NETCleanArch.Infrastructure.csproj package Npgsql.EntityFrameworkCore.PostgreSQL

# ---- Dapper ----
dotnet add NETCleanArch.Infrastructure/NETCleanArch.Infrastructure/NETCleanArch.Infrastructure.csproj package Dapper

# ---- AutoMapper ----
dotnet add NETCleanArch.Application/NETCleanArch.Application/NETCleanArch.Application.csproj package AutoMapper
dotnet add NETCleanArch.Api/NETCleanArch.Api/NETCleanArch.Api.csproj package AutoMapper.Extensions.Microsoft.DependencyInjection

# ---- JWT Authentication ----
dotnet add NETCleanArch.Api/NETCleanArch.Api/NETCleanArch.Api.csproj package Microsoft.AspNetCore.Authentication.JwtBearer
dotnet add NETCleanArch.Api/NETCleanArch.Api/NETCleanArch.Api.csproj package System.IdentityModel.Tokens.Jwt

# ---- Serilog ----
dotnet add NETCleanArch.Api/NETCleanArch.Api/NETCleanArch.Api.csproj package Serilog
dotnet add NETCleanArch.Api/NETCleanArch.Api/NETCleanArch.Api.csproj package Serilog.AspNetCore
dotnet add NETCleanArch.Api/NETCleanArch.Api/NETCleanArch.Api.csproj package Serilog.Sinks.Console
dotnet add NETCleanArch.Api/NETCleanArch.Api/NETCleanArch.Api.csproj package Serilog.Sinks.File
dotnet add NETCleanArch.Api/NETCleanArch.Api/NETCleanArch.Api.csproj package Serilog.Sinks.PostgreSQL

# ---- Serilog for Worker ----
dotnet add NETCleanArch.Worker/NETCleanArch.Worker/NETCleanArch.Worker.csproj package Serilog
dotnet add NETCleanArch.Worker/NETCleanArch.Worker/NETCleanArch.Worker.csproj package Serilog.AspNetCore
dotnet add NETCleanArch.Worker/NETCleanArch.Worker/NETCleanArch.Worker.csproj package Serilog.Sinks.Console

# ================================
# RESTORE + BUILD
# ================================
dotnet restore
dotnet build


NETCleanArch/
├── NETCleanArch.sln
├── NETCleanArch.Api/
│   └── NETCleanArch.Api/
│       ├── Controllers/
│       │   ├── AuthController.cs
│       │   ├── VisitorsController.cs
│       │   ├── VisitsController.cs
│       │   ├── VendorsController.cs
│       │   ├── ReportsController.cs
│       │   └── OcrController.cs
│       ├── Filters/
│       │   ├── CustomAuthorizationFilter.cs
│       │   └── GlobalExceptionFilter.cs
│       ├── Middleware/
│       │   ├── JwtMiddleware.cs
│       │   └── CorrelationIdMiddleware.cs
│       ├── Program.cs
│       └── appsettings.json
├── NETCleanArch.Application/
│   └── NETCleanArch.Application/
│       ├── DTOs/
│       ├── Interfaces/
│       ├── Services/
│       ├── Events/
│       ├── Mappings/
│       ├── Validators/
│       └── NETCleanArch.Application.csproj
├── NETCleanArch.Domain/
│   └── NETCleanArch.Domain/
│       ├── Entities/
│       ├── ValueObjects/
│       ├── Enums/
│       ├── Exceptions/
│       └── NETCleanArch.Domain.csproj
├── NETCleanArch.Infrastructure/
│   └── NETCleanArch.Infrastructure/
│       ├── Data/
│       ├── External/
│       ├── Events/
│       ├── Security/
│       └── NETCleanArch.Infrastructure.csproj
├── NETCleanArch.Worker/
│   └── NETCleanArch.Worker/
│       ├── Jobs/
│       └── Program.cs
└── NETCleanArch-frontend/
    └── src/
        ├── components/
        ├── pages/
        ├── store/
        ├── services/
        ├── types/
        ├── utils/
        └── App.tsx
