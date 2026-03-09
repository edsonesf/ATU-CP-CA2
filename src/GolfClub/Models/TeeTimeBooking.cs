namespace GolfClub.Models;

public class TeeTimeBooking
{
    public int BookingId { get; set; }
    public DateOnly Date { get; set; }
    public TimeOnly TimeSlot { get; set; }

    public int BookedByMemberId { get; set; }
    public Member BookedBy { get; set; } = null!;

    public ICollection<BookingPlayer> Players { get; set; } = [];
}
