using GolfClub.Data;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace GolfClub.Pages.Bookings;

public class IndexModel(GolfClubContext context) : PageModel
{
    public int Year { get; set; }
    public int Month { get; set; }
    public Dictionary<DateOnly, int> BookingCounts { get; set; } = [];

    public async Task OnGetAsync(int? year, int? month)
    {
        var today = DateOnly.FromDateTime(DateTime.Today);
        Year = year ?? today.Year;
        Month = month ?? today.Month;

        var firstDay = new DateOnly(Year, Month, 1);
        var lastDay = firstDay.AddMonths(1).AddDays(-1);

        BookingCounts = await context.TeeTimeBookings
            .Where(b => b.Date >= firstDay && b.Date <= lastDay)
            .GroupBy(b => b.Date)
            .Select(g => new { Date = g.Key, Count = g.Count() })
            .ToDictionaryAsync(x => x.Date, x => x.Count);
    }
}
