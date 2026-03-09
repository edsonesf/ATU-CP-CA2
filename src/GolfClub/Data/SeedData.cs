using GolfClub.Models;

namespace GolfClub.Data;

public static class SeedData
{
    public static void Seed(GolfClubContext context)
    {
        if (context.Members.Any()) return;

        context.Members.AddRange(
            new Member { MembershipNumber = "ATU001", Name = "Alice Murphy",   Email = "alice@example.com",  Gender = Gender.Female, Handicap = 5  },
            new Member { MembershipNumber = "ATU002", Name = "Brian Kelly",    Email = "brian@example.com",  Gender = Gender.Male,   Handicap = 8  },
            new Member { MembershipNumber = "ATU003", Name = "Carol Dunne",    Email = "carol@example.com",  Gender = Gender.Female, Handicap = 15 },
            new Member { MembershipNumber = "ATU004", Name = "David Walsh",    Email = "david@example.com",  Gender = Gender.Male,   Handicap = 18 },
            new Member { MembershipNumber = "ATU005", Name = "Emma Byrne",     Email = "emma@example.com",   Gender = Gender.Female, Handicap = 25 },
            new Member { MembershipNumber = "ATU006", Name = "Frank O'Brien",  Email = "frank@example.com",  Gender = Gender.Male,   Handicap = 32 },
            new Member { MembershipNumber = "ATU007", Name = "Grace Nolan",    Email = "grace@example.com",  Gender = Gender.Female, Handicap = 12 },
            new Member { MembershipNumber = "ATU008", Name = "Hugh Connell",   Email = "hugh@example.com",   Gender = Gender.Male,   Handicap = 7  }
        );
        context.SaveChanges();

        if (context.TeeTimeBookings.Any()) return;

        var tomorrow = DateOnly.FromDateTime(DateTime.Today.AddDays(1));

        // 09:00 — full (4/4): Alice + Brian + Carol + David
        var b1 = new TeeTimeBooking { Date = tomorrow, TimeSlot = new TimeOnly(9, 0), BookedByMemberId = 1 };
        context.TeeTimeBookings.Add(b1);
        context.SaveChanges();
        context.BookingPlayers.AddRange(
            new BookingPlayer { BookingId = b1.BookingId, MemberId = 1, HandicapAtBooking = 5  },
            new BookingPlayer { BookingId = b1.BookingId, MemberId = 2, HandicapAtBooking = 8  },
            new BookingPlayer { BookingId = b1.BookingId, MemberId = 3, HandicapAtBooking = 15 },
            new BookingPlayer { BookingId = b1.BookingId, MemberId = 4, HandicapAtBooking = 18 }
        );

        // 09:15 — partial (2/4): Emma + Frank
        var b2 = new TeeTimeBooking { Date = tomorrow, TimeSlot = new TimeOnly(9, 15), BookedByMemberId = 5 };
        context.TeeTimeBookings.Add(b2);
        context.SaveChanges();
        context.BookingPlayers.AddRange(
            new BookingPlayer { BookingId = b2.BookingId, MemberId = 5, HandicapAtBooking = 25 },
            new BookingPlayer { BookingId = b2.BookingId, MemberId = 6, HandicapAtBooking = 32 }
        );

        // 09:30 — single (1/4): Grace
        var b3 = new TeeTimeBooking { Date = tomorrow, TimeSlot = new TimeOnly(9, 30), BookedByMemberId = 7 };
        context.TeeTimeBookings.Add(b3);
        context.SaveChanges();
        context.BookingPlayers.Add(
            new BookingPlayer { BookingId = b3.BookingId, MemberId = 7, HandicapAtBooking = 12 }
        );

        // 09:45 — empty (0/4): booking exists but no players
        var b4 = new TeeTimeBooking { Date = tomorrow, TimeSlot = new TimeOnly(9, 45), BookedByMemberId = 8 };
        context.TeeTimeBookings.Add(b4);

        context.SaveChanges();
    }
}
