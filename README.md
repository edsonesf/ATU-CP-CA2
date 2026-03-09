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

## Live Demo

https://golf.edson.rocks/

---

## Features

- **Member management** — create, view, edit, delete members; auto-generated membership numbers
- **Tee sheet** — calendar view + day view of all 15-minute slots with colour-coded availability
- **Bookings** — book tee times for up to 4 members per slot; view all bookings per member
- **Queries** — filter members by gender, handicap range, name, or membership number
- **Sorting** — sort members by name, handicap, membership number, or booking count
- **Validation** — client-side and server-side validation on all forms

---

## Quick Start

### Prerequisites
- [.NET 9 SDK](https://dotnet.microsoft.com/download)

### Run

```bash
git clone https://github.com/edsonesf/ATU-CP-CA2
cd ATU-CP-CA2
dotnet run --project src/GolfClub
```

Open: `http://localhost:5276`

The database (`golfclub.db`) is committed to the repo and seeded with sample data — no setup required.

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
└── tests/GolfClub.Tests/   ← xUnit unit tests for BookingService
```

---

## Testing

Unit tests cover the core business rules in `BookingService`:
- One booking per member per day
- Maximum 4 players per tee time slot
- Valid handicap range (0–54)

```bash
dotnet test tests/GolfClub.Tests/GolfClub.Tests.csproj
```

---

## NuGet Package Versions

EF Core packages are pinned to `9.0.x` in the `.csproj`:

```xml
<PackageReference Include="Microsoft.EntityFrameworkCore.Sqlite" Version="9.0.3" />
<PackageReference Include="Microsoft.EntityFrameworkCore.Design" Version="9.0.3" />
```

Without a version, `dotnet add package` resolves the latest — currently EF Core 10, which requires `.NET 10`. Pinning ensures the packages match the project's `net9.0` target framework.

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
