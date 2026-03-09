using GolfClub.Data;
using GolfClub.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace GolfClub.Pages.Members;

public class IndexModel(GolfClubContext context) : PageModel
{
    public IList<Member> Members { get; set; } = [];
    public string CurrentSort { get; set; } = "name";
    public string CurrentOrder { get; set; } = "asc";
    public string? CurrentGender { get; set; }
    public string? CurrentHandicap { get; set; }
    public Dictionary<int, int> BookingCounts { get; set; } = [];
    public HashSet<int> BookedToday { get; set; } = [];

    public string? CurrentSearch { get; set; }

    public async Task OnGetAsync(string sort = "name", string order = "asc", string? gender = null, string? handicap = null, string? search = null)
    {
        CurrentSort = sort;
        CurrentOrder = order;
        CurrentGender = gender;
        CurrentHandicap = handicap;
        CurrentSearch = search;

        var query = context.Members.AsQueryable();

        // Search by name or membership number
        if (!string.IsNullOrWhiteSpace(search))
            query = query.Where(m => m.Name.Contains(search) || m.MembershipNumber.Contains(search));

        // Filter by gender
        if (!string.IsNullOrEmpty(gender) && Enum.TryParse<Gender>(gender, out var genderEnum))
            query = query.Where(m => m.Gender == genderEnum);

        // Filter by handicap range
        query = handicap switch
        {
            "below10"  => query.Where(m => m.Handicap <= 10),
            "11to20"   => query.Where(m => m.Handicap >= 11 && m.Handicap <= 20),
            "above20"  => query.Where(m => m.Handicap > 20),
            _          => query
        };

        // Sort
        query = (sort, order) switch
        {
            ("handicap",   "desc") => query.OrderByDescending(m => m.Handicap),
            ("handicap",   _)      => query.OrderBy(m => m.Handicap),
            ("membership", "desc") => query.OrderByDescending(m => m.MembershipNumber),
            ("membership", _)      => query.OrderBy(m => m.MembershipNumber),
            (_,            "desc") => query.OrderByDescending(m => m.Name),
            _                      => query.OrderBy(m => m.Name)
        };

        Members = await query.ToListAsync();

        BookingCounts = await context.BookingPlayers
            .GroupBy(bp => bp.MemberId)
            .Select(g => new { MemberId = g.Key, Count = g.Count() })
            .ToDictionaryAsync(x => x.MemberId, x => x.Count);

        if (sort == "bookings")
            Members = [.. (order == "desc"
                ? Members.OrderByDescending(m => BookingCounts.GetValueOrDefault(m.MemberId, 0))
                : Members.OrderBy(m => BookingCounts.GetValueOrDefault(m.MemberId, 0)))];

        var today = DateOnly.FromDateTime(DateTime.Today);
        BookedToday = (await context.TeeTimeBookings
            .Where(b => b.Date == today)
            .SelectMany(b => b.Players.Select(p => p.MemberId))
            .ToListAsync()).ToHashSet();
    }
}
