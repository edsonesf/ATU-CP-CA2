using GolfClub.Data;
using GolfClub.Models;
using GolfClub.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace GolfClub.Pages.Bookings;

public class CreateModel(GolfClubContext context, BookingService bookingService) : PageModel
{
    [BindProperty(SupportsGet = true)] public DateOnly Date { get; set; }
    [BindProperty(SupportsGet = true)] public TimeOnly TimeSlot { get; set; }
    [BindProperty] public int SelectedMemberId { get; set; }

    public SelectList Members { get; set; } = null!;
    public string? ErrorMessage { get; set; }

    public async Task<IActionResult> OnGetAsync()
    {
        await LoadMembersAsync();
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!bookingService.IsTeeTimeAvailable(Date, TimeSlot))
            ErrorMessage = "That slot is already booked.";
        else if (!bookingService.CanMemberBook(SelectedMemberId, Date))
            ErrorMessage = "That member already has a booking on this date.";

        if (ErrorMessage is not null)
        {
            await LoadMembersAsync();
            return Page();
        }

        var member = await context.Members.FindAsync(SelectedMemberId);
        var booking = new TeeTimeBooking
        {
            Date = Date,
            TimeSlot = TimeSlot,
            BookedByMemberId = SelectedMemberId,
            Players = [new BookingPlayer { MemberId = SelectedMemberId, HandicapAtBooking = member!.Handicap }]
        };

        context.TeeTimeBookings.Add(booking);
        await context.SaveChangesAsync();

        return RedirectToPage("TeeSheet", new { date = Date.ToString("yyyy-MM-dd") });
    }

    private async Task LoadMembersAsync() =>
        Members = new SelectList(await context.Members.OrderBy(m => m.Name).ToListAsync(), "MemberId", "Name");
}
