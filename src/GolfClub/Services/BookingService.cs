using GolfClub.Data;
using Microsoft.EntityFrameworkCore;

namespace GolfClub.Services;

public class BookingService(GolfClubContext context)
{
    private const int MaxPlayersPerSlot = 4;

    // True if the member has no booking on the given date
    public bool CanMemberBook(int memberId, DateOnly date) =>
        !context.TeeTimeBookings.Any(b =>
            b.Date == date &&
            b.Players.Any(p => p.MemberId == memberId));

    // True if the slot exists and has fewer than 4 players
    public bool CanAddPlayerToBooking(int bookingId) =>
        context.TeeTimeBookings
            .Include(b => b.Players)
            .Where(b => b.BookingId == bookingId)
            .Select(b => b.Players.Count)
            .FirstOrDefault() < MaxPlayersPerSlot;

    // True if no booking exists for that date+time (slot is free)
    public bool IsTeeTimeAvailable(DateOnly date, TimeOnly timeSlot) =>
        !context.TeeTimeBookings.Any(b => b.Date == date && b.TimeSlot == timeSlot);
}
