# TalentHub - Job Board Platform

A modern job board platform built with .NET 8 Clean Architecture, Next.js 14, and PostgreSQL.

## Tech Stack

### Backend
- **.NET 8** - Minimal APIs + Clean Architecture
- **PostgreSQL** - Primary database
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
- **GitHub Actions** - CI/CD
- **Render** - Backend hosting
- **Vercel** - Frontend hosting

## Getting Started

### Prerequisites
- .NET 8 SDK
- Docker & Docker Compose
- Node.js 18+

### Local Development

1. Clone the repository
```bash
git clone https://github.com/yourusername/talenthub.git
cd talenthub
```

2. Start services with Docker
```bash
docker-compose up -d
```

3. Run the backend
```bash
cd backend
dotnet run --project src/TalentHub.Api
```

4. Run the frontend
```bash
cd frontend
npm install
npm run dev
```

## Project Structure

```
talenthub/
├── backend/                     # .NET 8 API
│   ├── src/
│   │   ├── TalentHub.Api/       # Entry point
│   │   ├── TalentHub.Application/ # Business logic
│   │   ├── TalentHub.Domain/    # Entities
│   │   └── TalentHub.Infrastructure/ # EF Core
│   └── tests/
├── frontend/                    # Next.js 14
├── docker-compose.yml
└── .github/workflows/ci.yml
```

## API Documentation

Once running, visit:
- Swagger UI: `http://localhost:5000/swagger`
- Scalar API: `http://localhost:5000/scalar/v1`

## License

MIT
