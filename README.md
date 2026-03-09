# ATU Links

Atlantic Technological University, 2025/26

CA2 — Cross Platform Development (DEVP_IT803)

Professor Gerard McCloskey

Continuous Assessment 2 (CA2) – Golf Club Management System

A web application for managing golf club members and tee time bookings, built with ASP.NET 9 Razor Pages and Entity Framework Core.

## CA2 – Individual Project

| Student ID | Name | GitHub |
|---|---|---|
| L00196839 | Edson Ferreira | edsonesf |

---

## Features

- **Member management** — create, view, edit, delete members
- **Tee sheet** — visual day view of all 15-minute slots with availability
- **Bookings** — book tee times for up to 4 members per slot
- **Queries** — filter members by gender and handicap range
- **Sorting** — sort members by name or handicap (ascending/descending)
- **Validation** — client-side and server-side validation on all forms

---

## Quick Start

### Prerequisites
- [.NET 9 SDK](https://dotnet.microsoft.com/download)

### Run

```bash
git clone https://github.com/edsonesf/ATU-CP-CA2
cd ATU-CP-CA2

dotnet restore src/GolfClub/GolfClub.csproj
dotnet ef database update --project src/GolfClub
dotnet run --project src/GolfClub
```

Open: `https://localhost:5001`

The database is created automatically on first run and seeded with sample data.

---

## Database

SQLite is used for zero-configuration deployment — no database engine required.

The assignment specifies SQL Server. To switch:

1. Replace in `Program.cs`:
   ```csharp
   // from
   options.UseSqlite("Data Source=golfclub.db");
   // to
   options.UseSqlServer("your-connection-string");
   ```
2. Update connection string in `appsettings.json`
3. Delete the `Migrations/` folder
4. Run `dotnet ef migrations add InitialCreate`
5. Run `dotnet ef database update`

Models, queries, and business logic remain unchanged.

---

## Project Structure

```
src/GolfClub/
├── Data/           ← DbContext, seed data
├── Models/         ← Member, TeeTimeBooking, BookingPlayer
├── Services/       ← BookingService (business rules)
├── Pages/
│   ├── Members/    ← CRUD + sorting + filtering
│   └── Bookings/   ← Tee sheet + booking management
└── Program.cs

tests/GolfClub.Tests/   ← xUnit unit tests for BookingService
```

---

## Testing

Unit tests cover the core business rules in `BookingService`:
- One booking per member per day
- Maximum 4 players per tee time slot
- Valid handicap range (0–54)

Tests are included as a professional practice, not a requirement of the assignment.

```bash
dotnet test tests/GolfClub.Tests/GolfClub.Tests.csproj
```

---

## Tech Stack

| Layer | Technology |
|-------|------------|
| Framework | ASP.NET 9 — Razor Pages |
| ORM | Entity Framework Core |
| Database | SQLite |
| Frontend | Bootstrap 5 |
| Tests | xUnit |

---

## Contributing

See [CONTRIBUTING.md](CONTRIBUTING.md) for git workflow, commit standards, and branch strategy.
