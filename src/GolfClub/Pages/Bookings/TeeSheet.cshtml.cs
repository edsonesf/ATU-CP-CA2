using GolfClub.Data;
using GolfClub.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace GolfClub.Pages.Bookings;

public class TeeSheetModel(GolfClubContext context) : PageModel
{
    public DateOnly Date { get; set; }
    public List<TimeOnly> Slots { get; set; } = [];
    public Dictionary<TimeOnly, TeeTimeBooking?> Bookings { get; set; } = [];

    public async Task OnGetAsync(string date)
    {
        Date = DateOnly.TryParse(date, out var d) ? d : DateOnly.FromDateTime(DateTime.Today);

        // Generate all 15-min slots from 07:00 to 19:45
        for (var t = new TimeOnly(7, 0); t <= new TimeOnly(19, 45); t = t.AddMinutes(15))
            Slots.Add(t);

        var bookings = await context.TeeTimeBookings
            .Include(b => b.Players)
            .Where(b => b.Date == Date)
            .ToListAsync();

        foreach (var slot in Slots)
            Bookings[slot] = bookings.FirstOrDefault(b => b.TimeSlot == slot);
    }
}
