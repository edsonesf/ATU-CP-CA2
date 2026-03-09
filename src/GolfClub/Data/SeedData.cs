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
    }
}
