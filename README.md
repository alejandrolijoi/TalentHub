# TalentHub - Job Board Platform

A full-stack job board platform connecting companies with software developers. Companies can post jobs, manage applications, and handle payments. Candidates can search, filter, and apply to positions.

## Tech Stack

### Backend

- **.NET 8** - Minimal APIs + Clean Architecture
- **PostgreSQL (Neon)** - Primary database
- **Redis** - Distributed caching
- **EF Core** - ORM
- **JWT** - Authentication
- **Stripe + MercadoPago** - Payments

### Frontend

- **Next.js 14** - App Router
- **TypeScript** - Type safety
- **Tailwind CSS** - Styling
- **shadcn/ui** - UI components

### Infrastructure

- **Docker** - Containerization
- **GitHub Actions** - CI/CD pipeline
- **Render** - Backend hosting
- **Vercel** - Frontend hosting

## Features

### For Companies
- Post and manage job listings
- Review candidate applications
- Subscription plans with Stripe/MercadoPago integration
- Company profile management

### For Candidates
- Search jobs by title, location, and technology
- Apply to positions with one click
- Track application status
- Profile management

## Getting Started

### Prerequisites

- .NET 8 SDK
- Docker & Docker Compose
- Node.js 18+
- npm or yarn

### Local Development

1. Clone the repository

```bash
git clone https://github.com/alejandrolijoi/TalentHub.git
cd TalentHub
```

2. Start infrastructure services (PostgreSQL + Redis)

```bash
docker-compose up -d postgres redis
```

3. Run the backend

```bash
cd backend
dotnet run --project src/TalentHub.Api
```

The API will be available at `http://localhost:5000`

4. Run the frontend

```bash
cd frontend
npm install
npm run dev
```

The frontend will be available at `http://localhost:3000`

## Project Structure

```
TalentHub/
├── backend/                          # .NET 8 API
│   ├── src/
│   │   ├── TalentHub.Api/           # Entry point, controllers, middleware
│   │   ├── TalentHub.Application/   # Business logic, DTOs, interfaces
│   │   ├── TalentHub.Domain/        # Entities, value objects, domain rules
│   │   └── TalentHub.Infrastructure/ # EF Core, repositories, external services
│   └── tests/
│       └── TalentHub.UnitTests/     # Unit tests
├── frontend/                         # Next.js 14 application
│   └── src/
│       ├── app/                      # App Router pages
│       ├── components/               # Reusable UI components
│       └── lib/                      # Utilities and API client
├── docker-compose.yml               # Local development services
├── render.yaml                      # Render deployment config
└── .github/workflows/ci.yml        # CI/CD pipeline
```

## API Documentation

Once the backend is running, visit:

- **Swagger UI:** `http://localhost:5000/swagger`
- **Scalar API Reference:** `http://localhost:5000/scalar/v1`

## CI/CD

The project uses GitHub Actions for continuous integration:

- Build verification on push/PR
- Unit test execution
- Code quality checks

## License

MIT
