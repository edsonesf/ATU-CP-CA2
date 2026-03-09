# Contributing Guide

ATU Links — CA2  
DEVP_IT803 - Cross Platform Development (2025/26)

---

## Quick Start

```bash
# Clone repository
git clone https://github.com/edsonesf/ATU-CP-CA2
cd ATU-CP-CA2

# Run (database is pre-seeded, no migrations needed)
dotnet run --project src/GolfClub
# Open: http://localhost:5276
```

---

## Environment Setup

### Prerequisites
- [.NET 9 SDK](https://dotnet.microsoft.com/download)
- [VSCodium](https://vscodium.com/) or any editor with C# support
- Git

### Verify installation
```bash
dotnet --version   # should print 9.0.x
git --version
```

---

## Git Workflow

### 1. Start from main (always up to date)
```bash
git checkout main
git pull origin main
```

### 2. Create a feature branch
```bash
git checkout -b feat/member-crud
```

### 3. Work in small, atomic commits
```bash
git add .
git commit -m "feat: add Member model with EF Core annotations"
```

### 4. Push branch
```bash
git push origin feat/member-crud
```

### 5. Merge into main when feature is complete and builds cleanly
```bash
git checkout main
git merge feat/member-crud
git push origin main
```

---

## Commit Message Standard

Format: `type: short description`

| Type | When to use |
|------|-------------|
| `feat` | New feature or page |
| `fix` | Bug fix |
| `docs` | Documentation only |
| `style` | UI/CSS changes, no logic |
| `refactor` | Code restructure, no new feature |
| `test` | Adding or updating tests |
| `chore` | Packages, config, migrations |

### Examples
```bash
git commit -m "feat: add Member model with EF Core annotations"
git commit -m "feat: add Members list page with sort and filter"
git commit -m "feat: add BookingService with one-booking-per-day rule"
git commit -m "fix: handicap filter not returning above-20 results"
git commit -m "chore: add EF Core SQLite migration"
git commit -m "test: add unit tests for BookingService capacity rule"
git commit -m "docs: update README with run instructions"
```

**Rules:**
- Lowercase only
- No full stop at the end
- Under 72 characters
- Describe *what* changed, not *how*
- One logical change per commit — don't bundle unrelated changes

---

## Branch Strategy

One branch per phase or feature, merged into `main` when done.

```
main
 ├── feat/project-skeleton
 ├── feat/member-crud
 ├── feat/member-queries
 ├── feat/tee-sheet
 ├── feat/booking-creation
 ├── feat/member-bookings-query
 └── feat/ui-polish
```

**Branch naming:**
- `feat/` — new feature or phase
- `fix/` — bug fix
- `docs/` — documentation
- `refactor/` — code cleanup

**Rules:**
- Never commit directly to `main`
- Each branch = one phase from the development plan
- Merge only when the feature builds and runs cleanly

---

## Releasing / Deploying

### 1. Publish a release build locally

```bash
cd src/GolfClub
dotnet publish -c Release -o ./publish
cp golfclub.db publish/
zip -r atu-links.zip publish/
```

### 2. Copy to server

```bash
scp -i <key.pem> atu-links.zip ec2-user@<server-ip>:~/
```

### 3. On the server — first deploy

```bash
# Install ASP.NET Core runtime (Amazon Linux 2023)
sudo dnf install -y aspnetcore-runtime-9.0

unzip atu-links.zip
mv publish atu-links
```

### 4. Run as a systemd service

Create `/etc/systemd/system/atu-links.service`:

```ini
[Unit]
Description=ATU Links Golf Club App
After=network.target

[Service]
WorkingDirectory=/home/ec2-user/atu-links
ExecStart=/usr/bin/dotnet /home/ec2-user/atu-links/GolfClub.dll --urls http://0.0.0.0:5276
Restart=always
User=ec2-user
Environment=ASPNETCORE_ENVIRONMENT=Production

[Install]
WantedBy=multi-user.target
```

```bash
sudo systemctl daemon-reload
sudo systemctl enable atu-links
sudo systemctl start atu-links
```

### 5. Updating an existing deployment

```bash
# Locally — build and copy new zip
cd src/GolfClub
dotnet publish -c Release -o ./publish
cp golfclub.db publish/
zip -r atu-links.zip publish/
scp -i <key.pem> atu-links.zip ec2-user@<server-ip>:~/

# On the server
sudo systemctl stop atu-links
rm -rf atu-links && unzip atu-links.zip && mv publish atu-links
sudo systemctl start atu-links
```

---

## Development Phases

| Branch | What gets built |
|--------|----------------|
| `feat/project-skeleton` | ASP.NET project, EF Core + SQLite, migrations, DI setup |
| `feat/member-crud` | Member model, CRUD pages, basic validation |
| `feat/member-queries` | Sorting by name/handicap, filtering by gender/handicap |
| `feat/tee-sheet` | Booking + BookingPlayer models, date picker, tee sheet view |
| `feat/booking-creation` | BookingService + unit tests, create/edit/delete booking |
| `feat/member-bookings-query` | View all bookings for a selected member |
| `feat/ui-polish` | Bootstrap layout, navigation, validation messages |

Each phase leaves the app in a working, runnable state.

---

## Code Standards

### C# Naming Conventions
| Element | Convention | Example |
|---------|------------|---------|
| Classes | PascalCase | `BookingService` |
| Methods | PascalCase | `CanMemberBook` |
| Properties | PascalCase | `MembershipNumber` |
| Private fields | camelCase with `_` | `_context` |

### Rules
- Business logic belongs in `Services/` — never in Razor page code-behind
- Keep Razor page models thin: call services, don't re-implement rules
- No commented-out code in commits
- No hardcoded connection strings

### Before Every Commit
```bash
dotnet build    # must pass with 0 errors
```

### Adding NuGet packages

Always pin EF Core packages to `9.0.x` — without a version, `dotnet add package` resolves the latest (currently EF Core 10, which requires .NET 10 and will fail on this project):

```bash
dotnet add package Microsoft.EntityFrameworkCore.Sqlite --version 9.0.3
```

If tests exist:
```bash
dotnet test     # must pass with 0 failures
```

---

## Project Structure

```
ATU-CP-CA2/
├── src/
│   └── GolfClub/
│       ├── Data/           ← DbContext, SeedData
│       ├── Models/         ← Member, TeeTimeBooking, BookingPlayer
│       ├── Services/       ← BookingService (business rules)
│       ├── Pages/
│       │   ├── Members/
│       │   └── Bookings/
│       └── Program.cs
├── tests/
│   └── GolfClub.Tests/     ← xUnit unit tests
├── PLAN.md
├── CONTRIBUTING.md
└── README.md
```
