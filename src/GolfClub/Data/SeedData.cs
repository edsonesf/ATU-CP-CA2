using GolfClub.Models;

namespace GolfClub.Data;

public static class SeedData
{
    public static void Seed(GolfClubContext context)
    {
        if (context.Members.Any()) return;

        context.Members.AddRange(
            new Member { MembershipNumber = "ATU001", Name = "Alice Murphy",   Email = "alice@atu.ie",  Gender = Gender.Female, Handicap = 5  },
            new Member { MembershipNumber = "ATU002", Name = "Brian Kelly",    Email = "brian@atu.ie",  Gender = Gender.Male,   Handicap = 8  },
            new Member { MembershipNumber = "ATU003", Name = "Carol Dunne",    Email = "carol@atu.ie",  Gender = Gender.Female, Handicap = 15 },
            new Member { MembershipNumber = "ATU004", Name = "David Walsh",    Email = "david@atu.ie",  Gender = Gender.Male,   Handicap = 18 },
            new Member { MembershipNumber = "ATU005", Name = "Emma Byrne",     Email = "emma@atu.ie",   Gender = Gender.Female, Handicap = 25 },
            new Member { MembershipNumber = "ATU006", Name = "Frank O'Brien",  Email = "frank@atu.ie",  Gender = Gender.Male,   Handicap = 32 },
            new Member { MembershipNumber = "ATU007", Name = "Grace Nolan",    Email = "grace@atu.ie",  Gender = Gender.Female, Handicap = 12 },
            new Member { MembershipNumber = "ATU008", Name = "Hugh Connell",   Email = "hugh@atu.ie",   Gender = Gender.Male,   Handicap = 7  }
        );

        context.SaveChanges();
    }
}
