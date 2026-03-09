using GolfClub.Data;
using GolfClub.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace GolfClub.Pages.Members;

public class BookingsModel(GolfClubContext context) : PageModel
{
    public Member Member { get; set; } = null!;
    public List<TeeTimeBooking> Bookings { get; set; } = [];

    public async Task<IActionResult> OnGetAsync(int id)
    {
        var member = await context.Members.FindAsync(id);
        if (member is null) return NotFound();
        Member = member;

        Bookings = await context.TeeTimeBookings
            .Include(b => b.Players).ThenInclude(p => p.Member)
            .Where(b => b.Players.Any(p => p.MemberId == id))
            .OrderByDescending(b => b.Date).ThenBy(b => b.TimeSlot)
            .ToListAsync();

        return Page();
    }
}
