using GolfClub.Data;
using GolfClub.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace GolfClub.Pages.Bookings;

public class DeleteModel(GolfClubContext context) : PageModel
{
    public TeeTimeBooking Booking { get; set; } = null!;

    public async Task<IActionResult> OnGetAsync(int id)
    {
        var booking = await context.TeeTimeBookings
            .Include(b => b.Players).ThenInclude(p => p.Member)
            .FirstOrDefaultAsync(b => b.BookingId == id);

        if (booking is null) return NotFound();
        Booking = booking;
        return Page();
    }

    public async Task<IActionResult> OnPostAsync(int id)
    {
        var booking = await context.TeeTimeBookings
            .Include(b => b.Players)
            .FirstOrDefaultAsync(b => b.BookingId == id);

        if (booking is not null)
        {
            context.BookingPlayers.RemoveRange(booking.Players);
            context.TeeTimeBookings.Remove(booking);
            await context.SaveChangesAsync();
        }

        return RedirectToPage("TeeSheet", new { date = booking?.Date.ToString("yyyy-MM-dd") });
    }
}
