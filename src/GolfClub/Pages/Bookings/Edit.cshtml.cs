using GolfClub.Data;
using GolfClub.Models;
using GolfClub.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace GolfClub.Pages.Bookings;

public class EditModel(GolfClubContext context, BookingService bookingService) : PageModel
{
    public TeeTimeBooking Booking { get; set; } = null!;
    [BindProperty] public int SelectedMemberId { get; set; }
    public SelectList AvailableMembers { get; set; } = null!;
    public string? ErrorMessage { get; set; }

    public async Task<IActionResult> OnGetAsync(int id)
    {
        var booking = await LoadBookingAsync(id);
        if (booking is null) return NotFound();
        Booking = booking;
        await LoadAvailableMembersAsync();
        return Page();
    }

    public async Task<IActionResult> OnPostAddAsync(int id)
    {
        var booking = await LoadBookingAsync(id);
        if (booking is null) return NotFound();
        Booking = booking;

        if (!bookingService.CanAddPlayerToBooking(id))
            ErrorMessage = "This slot is already full (4 players).";
        else if (!bookingService.CanMemberBook(SelectedMemberId, booking.Date))
            ErrorMessage = "That member already has a booking on this date.";

        if (ErrorMessage is not null)
        {
            await LoadAvailableMembersAsync();
            return Page();
        }

        var member = await context.Members.FindAsync(SelectedMemberId);
        context.BookingPlayers.Add(new BookingPlayer
        {
            BookingId = id,
            MemberId = SelectedMemberId,
            HandicapAtBooking = member!.Handicap
        });
        await context.SaveChangesAsync();

        return RedirectToPage(new { id });
    }

    public async Task<IActionResult> OnPostRemoveAsync(int id, int playerId)
    {
        var booking = await LoadBookingAsync(id);
        if (booking is null) return NotFound();

        if (booking.Players.Count <= 1)
        {
            Booking = booking;
            ErrorMessage = "Cannot remove the last player. Delete the booking instead.";
            await LoadAvailableMembersAsync();
            return Page();
        }

        var player = booking.Players.FirstOrDefault(p => p.BookingPlayerId == playerId);
        if (player is not null)
        {
            context.BookingPlayers.Remove(player);
            await context.SaveChangesAsync();
        }

        return RedirectToPage(new { id });
    }

    private async Task<TeeTimeBooking?> LoadBookingAsync(int id) =>
        await context.TeeTimeBookings
            .Include(b => b.Players).ThenInclude(p => p.Member)
            .FirstOrDefaultAsync(b => b.BookingId == id);

    private async Task LoadAvailableMembersAsync()
    {
        var takenIds = Booking.Players.Select(p => p.MemberId).ToHashSet();
        var members = await context.Members
            .Where(m => !takenIds.Contains(m.MemberId))
            .OrderBy(m => m.Name)
            .ToListAsync();
        AvailableMembers = new SelectList(members, "MemberId", "Name");
    }
}
