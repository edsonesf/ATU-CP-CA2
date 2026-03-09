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
    [BindProperty] public List<int> AdditionalMemberIds { get; set; } = [];

    public SelectList Members { get; set; } = null!;
    public string? ErrorMessage { get; set; }

    public async Task<IActionResult> OnGetAsync()
    {
        await LoadMembersAsync();
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        var extraIds = AdditionalMemberIds.Where(id => id > 0).Distinct().ToList();
        var allIds = new[] { SelectedMemberId }.Concat(extraIds).ToList();

        if (Date < DateOnly.FromDateTime(DateTime.Today))
            ErrorMessage = "Cannot book a tee time in the past.";
        else if (!bookingService.IsTeeTimeAvailable(Date, TimeSlot))
            ErrorMessage = "That slot is already booked.";
        else if (allIds.Distinct().Count() != allIds.Count)
            ErrorMessage = "The same member cannot be added twice.";
        else if (!bookingService.CanMemberBook(SelectedMemberId, Date))
            ErrorMessage = "That member already has a booking on this date.";
        else
        {
            foreach (var id in extraIds)
            {
                if (!bookingService.CanMemberBook(id, Date))
                {
                    ErrorMessage = "One of the selected members already has a booking on this date.";
                    break;
                }
            }
        }

        if (ErrorMessage is not null)
        {
            await LoadMembersAsync();
            return Page();
        }

        var players = new List<BookingPlayer>();
        foreach (var id in allIds)
        {
            var member = await context.Members.FindAsync(id);
            players.Add(new BookingPlayer { MemberId = id, HandicapAtBooking = member!.Handicap });
        }

        context.TeeTimeBookings.Add(new TeeTimeBooking
        {
            Date = Date,
            TimeSlot = TimeSlot,
            BookedByMemberId = SelectedMemberId,
            Players = players
        });
        await context.SaveChangesAsync();

        return RedirectToPage("TeeSheet", new { date = Date.ToString("yyyy-MM-dd") });
    }

    private async Task LoadMembersAsync() =>
        Members = new SelectList(await context.Members.OrderBy(m => m.Name).ToListAsync(), "MemberId", "Name");
}
