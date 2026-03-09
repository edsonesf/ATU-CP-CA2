using GolfClub.Data;
using GolfClub.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace GolfClub.Pages.Members;

public class DeleteModel(GolfClubContext context) : PageModel
{
    public Member Member { get; set; } = new();

    public async Task<IActionResult> OnGetAsync(int id)
    {
        var member = await context.Members.FindAsync(id);
        if (member is null) return NotFound();
        Member = member;
        return Page();
    }

    public async Task<IActionResult> OnPostAsync(int id)
    {
        var member = await context.Members.FindAsync(id);
        if (member is null) return RedirectToPage("Index");

        var hasBookings = await context.BookingPlayers.AnyAsync(bp => bp.MemberId == id);
        if (hasBookings)
        {
            Member = member;
            ModelState.AddModelError(string.Empty, "Cannot delete this member — they have existing bookings. Remove them from all bookings first.");
            return Page();
        }

        context.Members.Remove(member);
        await context.SaveChangesAsync();
        return RedirectToPage("Index");
    }
}
