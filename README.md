<div align="center">

# 🚀 TalentMesh

<h3>Next-Generation Interview as a Service Platform</h3>

[![.NET 8](https://img.shields.io/badge/.NET-8.0-512BD4?style=for-the-badge&logo=.net&logoColor=white)](https://dotnet.microsoft.com/download)
[![ASP.NET Core](https://img.shields.io/badge/ASP.NET-Core-purple?style=for-the-badge&logo=.net&logoColor=white)](https://dotnet.microsoft.com/apps/aspnet)
[![PostgreSQL](https://img.shields.io/badge/PostgreSQL-15-316192?style=for-the-badge&logo=postgresql&logoColor=white)](https://www.postgresql.org/)
[![Angular](https://img.shields.io/badge/Angular-17-DD0031?style=for-the-badge&logo=angular&logoColor=white)](https://angular.io)

</div>

---

<div align="center">

### 🔥 Meet Team - Earendel

  <ul>
  <li><b>Rakibul Hasan</b> <i>(Team Leader)</i></li>
  <li><b>Nafiul Hasan Hamim</b> </li>
  <li><b>MFR Siam</b> </li>
  <li><b>Mahmudul Hasan</b> <i>(Mentor)</i></li>
  </ul>

</div>

---

<div align="center">

# 🌟 Talent Mesh Frontend

  <p align="center">
    <a href="https://angular.io">
      <img src="https://img.shields.io/badge/Angular-17.3.11-red.svg" alt="Angular">
    </a>
    <a href="https://www.typescriptlang.org/">
      <img src="https://img.shields.io/badge/TypeScript-5.2-blue.svg" alt="TypeScript">
    </a>
  </p>

</div>

<div align="center">

While this repository only hosts our backend services. But for convenience, we have included all the dashboard link here to provide a quick glimpse of all dashboards:

|          Portal           |              Features               |                                  Quick Access                                  |
| :-----------------------: | :---------------------------------: | :----------------------------------------------------------------------------: |
| 👨‍💼 **Interviewer Portal** | AI-assisted interviews & scheduling |  [🔗 Launch](https://talent-mesh-frontend.netlify.app/interviewer-dashboard)   |
|  👨‍💻 **Candidate Portal**  | Interactive assessments & tracking  |   [🔗 Launch](https://talent-mesh-frontend.netlify.app/candidate-dashboard)    |
|    👑 **Admin Portal**    |     System control & analytics      | [🔗 Launch](https://talent-mesh-frontend.netlify.app/admin-dashboard/overview) |
|     🏢 **HR Portal**      |    Recruitment & talent pipeline    |  [🔗 Launch](https://talent-mesh-frontend.netlify.app/hr-dashboard/overview)   |

</div>

---

## 🌟 Overview

Earendel is a revolutionary **Interview as a Service (IaaS)** platform that leverages cutting-edge .NET 8 microservices architecture to deliver instant, affordable, and standardized technical assessments. Our platform seamlessly connects talent with opportunities through an innovative approach to technical recruitment.

### 🎯 Core Features

- **🤖 AI-Powered Interview Engine**

  - Real-time code analysis and evaluation
  - Automated skill assessment and scoring
  - Behavioral analysis through ML algorithms

- **💻 Advanced Technical Assessment**

  - Live coding environments with 20+ languages
  - System design interview tools
  - Automated test case generation

- **📈 Enterprise Analytics**
  - Comprehensive recruitment metrics
  - Candidate performance analytics
  - ROI and efficiency tracking

## 🛠 Technical Architecture

### Architecture Overview

```yaml
Core Architecture:
  - Pattern: Modular Monolithic
  - Design: Clean Architecture
  - Principles: SOLID, DDD

Modules:
  Interview Engine:
    - Real-time Code Execution
    - Assessment Management
    - Scoring Engine
  User Management:
    - Identity & Access Control
    - Profile Management
  Recruitment:
    - Job Management
    - Candidate Tracking
  Analytics:
    - Reporting Engine
    - Performance Metrics

Technology Stack:
  Framework: ASP.NET Core 8.0
  Database: PostgreSQL 15
  Caching: Redis
  Real-time: SignalR

Development Practices:
  - Domain-Driven Design
  - CQRS Pattern
  - Vertical Slice Architecture
  - Event-Driven Communication
```

### Infrastructure & Deployment

- **🏛️ Application Architecture**

  - Modular Monolithic Design
  - Containerized with Docker
  - Azure App Service Deployment
  - GitHub Actions CI/CD Pipeline

- **🔐 Security & Authentication**

  - ASP.NET Core Identity
  - JWT Authentication
  - Role-based Access Control (RBAC)
  - API Key Management

- **📈 Monitoring & Logging**
  - Application Insights Integration
  - Structured Logging with Serilog
  - Health Checks & Diagnostics
  - Performance Monitoring

<!-- ## 📂 Project Structure -->

## 🚀 Quick Start

### Prerequisites

```bash
# Install .NET 8 SDK
dotnet --version  # Should be 8.0.x

# Install PostgreSQL
brew install postgresql@15

# Install Redis
brew install redis
```

### Development Setup

```bash
# Clone the repository
git clone https://github.com/Learnathon-By-Geeky-Solutions/earendel.git

# Navigate to backend
cd earendel/backend

# Restore dependencies
dotnet restore

# Update database
dotnet ef database update

# Run the application
dotnet run --project Api/Earendel.Api.csproj
```

## 📐 Documentation

- [📚 API Documentation](https://api.earendel.dev/swagger)
- [🛠 Architecture Guide](docs/architecture.md)
- [👨‍💻 Developer Guide](docs/development.md)
- [🔧 Configuration Guide](docs/configuration.md)

---
