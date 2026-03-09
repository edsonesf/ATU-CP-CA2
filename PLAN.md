# ATU Links — Project Plan

## Domain Understanding

A golf club manages **members** and controls access to the **course** via tee time bookings.
The course itself doesn't need to be modelled — only who plays and when.

### The Golfer's Journey
```
Join club → Get membership → View tee sheet → Book a slot → Show up → Play
```

### What is a Tee Time?
Like a restaurant reservation for the golf course. Groups are spaced 15 minutes apart so they don't interfere with each other on the course.

---

## Database Design

### Entity Relationship
```
Member ──────< BookingPlayer >────── TeeTimeBooking
(1 member)   (join table)           (1 booking slot)
```

- One member can have many `BookingPlayer` records (one per booking they join)
- One `TeeTimeBooking` can have up to 4 `BookingPlayer` records
- `BookingPlayer` is the join table linking members to bookings

### Member
| Field | Type | Notes |
|-------|------|-------|
| MemberId | int (PK) | Auto-generated |
| MembershipNumber | string | Unique, club-assigned |
| Name | string | Full name |
| Email | string | Unique |
| Gender | enum | Male / Female / Other |
| Handicap | int | 0–54, lower = better |

### TeeTimeBooking
| Field | Type | Notes |
|-------|------|-------|
| BookingId | int (PK) | Auto-generated |
| Date | DateOnly | Day of play |
| TimeSlot | TimeOnly | Fixed 15-min intervals |
| BookedByMemberId | int (FK) | → Member who created the booking |

### BookingPlayer (join table)
| Field | Type | Notes |
|-------|------|-------|
| BookingPlayerId | int (PK) | Auto-generated |
| BookingId | int (FK) | → TeeTimeBooking |
| MemberId | int (FK) | → Member (always required — guests not allowed) |
| HandicapAtBooking | int | Snapshot of handicap at time of booking (optional if time is tight) |

> **Why snapshot the handicap?** A member's handicap can change over time.
> Storing it at booking time preserves an accurate record of who played at what level.

### Constraints
- Unique index on `(Date, TimeSlot)` in `TeeTimeBooking` — no duplicate slots
- Unique constraint on `(MemberId, BookingId)` in `BookingPlayer` — no duplicate players per slot
- One booking per member per day enforced in `BookingService` (not DB-level, but documented)

### Data Types
- Use `DateOnly` for dates (no time component)
- Use `TimeOnly` for time slots (no date component)

---

## Business Rules

- A member can book **at most one tee time per day**
- Each tee time slot holds **up to 4 players**
- Time slots are fixed: every **15 minutes** from the hour
- Handicap is a number — lower means better player

---

## Required Features (from assignment)

### CRUD
- Create, Read, Update, Delete for **Members**
- Create, Read, Update, Delete for **Bookings**

### Queries
- View all members
- Filter members by **Gender**
- Filter by **Handicap**: below 10 / 11–20 / above 20
- View all bookings for a **selected member**

### Sorting
- Members by **Name** (asc / desc)
- Members by **Handicap** (asc / desc)

### Validation
- Member form: all fields required, email format, handicap range
- Booking form: valid date, valid time slot, max 4 players, one booking per day per member

---

## Out of Scope
- Scoring / scorecards
- Course or hole management
- Payments or membership fees
- Equipment rental

---

## Tech Stack
- **Framework:** ASP.NET 9 — Razor Pages
- **Database:** SQLite — see note below
- **ORM:** Entity Framework Core
- **Frontend:** Razor + Bootstrap (basic styling)

> **Database decision:** SQLite was chosen for portability and zero-configuration deployment.
> Anyone can clone and run with `dotnet run` — no database engine, no setup, no credentials.
> The assignment specifies SQL Server, but EF Core fully abstracts the provider.
> Switching to SQL Server requires one line in `Program.cs` and a connection string in `appsettings.json`.
> **Important:** EF Core migrations are provider-specific. When switching to SQL Server,
> delete the `Migrations/` folder and run `dotnet ef migrations add InitialCreate` to regenerate them.
> Models, queries, and business logic remain unchanged.
> This is a deliberate, professional tradeoff — documented here and in the README.

---

## Marking Priority

| Marks | Area |
|-------|------|
| 20% | GUI |
| 20% | Sorting |
| 10% | Individual bookings query |
| 10% | Member validation |
| 5% each | CRUD + remaining queries |

---

## Project Structure

```
/GolfClub
├── Data/
│   ├── GolfClubContext.cs
│   └── SeedData.cs
├── Models/
│   ├── Member.cs
│   ├── TeeTimeBooking.cs
│   └── BookingPlayer.cs
├── Services/
│   └── BookingService.cs       ← business rules live here, not in pages
├── Pages/
│   ├── Members/
│   └── Bookings/
└── Program.cs
```

## Service Layer (Business Rules)

Business rules must NOT live in Razor pages. They belong in `BookingService`:
- `CanMemberBook(memberId, date)` — enforces one booking per member per day
- `IsTeeTimeAvailable(bookingId)` — enforces max 4 players per slot
- `CanAddPlayerToBooking(bookingId)` — checks slot capacity before adding a player

## Page Flow

```
Home
├── Members
│   ├── List (sort + filter here)
│   ├── Create / Edit / Delete
│   └── View Member Bookings
└── Bookings
    └── Select Date → Tee Sheet (all 15-min slots for the day)
                          ├── Free slot  → Create Booking
                          └── Booked slot → Edit / Delete
```

User journey: **choose date → see full tee sheet → click a slot → book**

- Time slots are never typed manually — always selected from the tee sheet
- This enforces the 15-min interval rule visually, not just in code
- Each slot shows player count (e.g. "2 / 4") and disables booking when full

## Testing Strategy

Testing is not required by the assignment but is included as a professional practice.

### What to test
Unit tests for `BookingService` business rules only — these are the logic most likely to break:

| Test | Rule being verified |
|------|---------------------|
| Member can book when no booking exists today | One booking per day |
| Member cannot book when already booked today | One booking per day |
| Slot accepts up to 4 players | Max players per slot |
| Slot rejects a 5th player | Max players per slot |
| Member can join a booking as 2nd/3rd/4th player | Join table logic |
| Valid handicap range accepted (0–54) | Member validation |
| Invalid handicap rejected (-1, 55) | Member validation |

### What NOT to test
- UI / Razor pages — manual testing is sufficient
- Database queries — covered by EF Core itself
- Integration / browser tests — too much setup for the time available

### Tools
- **xUnit** — standard .NET test framework
- **EF Core InMemory provider** — fake database for fast isolated tests

> **Note:** EF Core InMemory is not fully relational (no constraints, no transactions).
> A more accurate alternative is SQLite in-memory mode (same provider as production).
> For this assignment, InMemory is acceptable — the tradeoff is documented here.

> Note in README: unit tests are included despite not being a requirement,
> as a demonstration of professional development practices.

---

## Seed Data

Minimum data to cover all filter, sort, and booking scenarios from day one.

### Members (8)

| Name | Gender | Handicap | Covers |
|------|--------|----------|--------|
| Alice Murphy | Female | 5 | Below 10 |
| Brian Kelly | Male | 8 | Below 10 |
| Carol Dunne | Female | 15 | 11–20 |
| David Walsh | Male | 18 | 11–20 |
| Emma Byrne | Female | 25 | Above 20 |
| Frank O'Brien | Male | 32 | Above 20 |
| Grace Nolan | Female | 12 | 11–20 |
| Hugh Connell | Male | 7 | Below 10 |

4 male / 4 female — all handicap bands covered by both genders.

### TeeTimeBookings (tomorrow's date, 4 slots)

| Time | Players | State |
|------|---------|-------|
| 09:00 | Alice + Brian + Carol + David | Full (4/4) — tests slot rejection |
| 09:15 | Emma + Frank | Partial (2/4) — tests joining |
| 09:30 | Grace | Single (1/4) — tests partial slot |
| 09:45 | *(empty)* | Free — tests available slot |

All four tee sheet states visible on first run.
Alice already has a booking — tests "one booking per day" rule.

---

## Development Phases (Vertical Slices)

Build one complete feature at a time — each phase leaves the app in a working state.

1. **Project skeleton** — ASP.NET project, EF Core + SQLite, empty migration, `Program.cs` DI setup
2. **Members CRUD** — Member model, create/edit/delete/list pages, basic validation
3. **Members queries** — sorting by name/handicap, filtering by gender/handicap range
4. **Tee sheet (read-only)** — Booking + BookingPlayer models, date picker, tee sheet view
5. **Booking creation + Business rules** — BookingService + unit tests, create/edit/delete booking, enforce constraints
6. **Queries** — View bookings per member, all remaining filter/sort requirements
7. **UI Polish** — Bootstrap layout, validation messages, navigation


