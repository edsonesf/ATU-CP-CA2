namespace GolfClub.Models;

public class BookingPlayer
{
    public int BookingPlayerId { get; set; }

    public int BookingId { get; set; }
    public TeeTimeBooking Booking { get; set; } = null!;

    public int MemberId { get; set; }
    public Member Member { get; set; } = null!;

    public int HandicapAtBooking { get; set; }
}
