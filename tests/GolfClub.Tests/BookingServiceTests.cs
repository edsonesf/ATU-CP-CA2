using GolfClub.Data;
using GolfClub.Models;
using GolfClub.Services;
using Microsoft.EntityFrameworkCore;

namespace GolfClub.Tests;

public class BookingServiceTests
{
    private static GolfClubContext CreateContext()
    {
        var options = new DbContextOptionsBuilder<GolfClubContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        return new GolfClubContext(options);
    }

    private static Member MakeMember(int id, int handicap = 10) => new()
    {
        MemberId = id,
        MembershipNumber = $"M{id:000}",
        Name = $"Member {id}",
        Email = $"member{id}@example.com",
        Gender = Gender.Male,
        Handicap = handicap
    };

    // --- CanMemberBook ---

    [Fact]
    public void CanMemberBook_NoExistingBooking_ReturnsTrue()
    {
        using var ctx = CreateContext();
        ctx.Members.Add(MakeMember(1));
        ctx.SaveChanges();

        var svc = new BookingService(ctx);
        Assert.True(svc.CanMemberBook(1, DateOnly.FromDateTime(DateTime.Today)));
    }

    [Fact]
    public void CanMemberBook_AlreadyBookedToday_ReturnsFalse()
    {
        using var ctx = CreateContext();
        var today = DateOnly.FromDateTime(DateTime.Today);
        ctx.Members.Add(MakeMember(1));
        ctx.TeeTimeBookings.Add(new TeeTimeBooking
        {
            Date = today, TimeSlot = new TimeOnly(9, 0), BookedByMemberId = 1,
            Players = [new BookingPlayer { MemberId = 1, HandicapAtBooking = 10 }]
        });
        ctx.SaveChanges();

        var svc = new BookingService(ctx);
        Assert.False(svc.CanMemberBook(1, today));
    }

    [Fact]
    public void CanMemberBook_BookedOnDifferentDay_ReturnsTrue()
    {
        using var ctx = CreateContext();
        var today = DateOnly.FromDateTime(DateTime.Today);
        ctx.Members.Add(MakeMember(1));
        ctx.TeeTimeBookings.Add(new TeeTimeBooking
        {
            Date = today.AddDays(-1), TimeSlot = new TimeOnly(9, 0), BookedByMemberId = 1,
            Players = [new BookingPlayer { MemberId = 1, HandicapAtBooking = 10 }]
        });
        ctx.SaveChanges();

        var svc = new BookingService(ctx);
        Assert.True(svc.CanMemberBook(1, today));
    }

    // --- CanAddPlayerToBooking ---

    [Fact]
    public void CanAddPlayerToBooking_SlotHasRoom_ReturnsTrue()
    {
        using var ctx = CreateContext();
        ctx.Members.Add(MakeMember(1));
        var booking = new TeeTimeBooking
        {
            Date = DateOnly.FromDateTime(DateTime.Today), TimeSlot = new TimeOnly(9, 0), BookedByMemberId = 1,
            Players = [new BookingPlayer { MemberId = 1, HandicapAtBooking = 10 }]
        };
        ctx.TeeTimeBookings.Add(booking);
        ctx.SaveChanges();

        var svc = new BookingService(ctx);
        Assert.True(svc.CanAddPlayerToBooking(booking.BookingId));
    }

    [Fact]
    public void CanAddPlayerToBooking_SlotFull_ReturnsFalse()
    {
        using var ctx = CreateContext();
        for (int i = 1; i <= 4; i++) ctx.Members.Add(MakeMember(i));
        var booking = new TeeTimeBooking
        {
            Date = DateOnly.FromDateTime(DateTime.Today), TimeSlot = new TimeOnly(9, 0), BookedByMemberId = 1,
            Players = Enumerable.Range(1, 4)
                .Select(i => new BookingPlayer { MemberId = i, HandicapAtBooking = 10 })
                .ToList()
        };
        ctx.TeeTimeBookings.Add(booking);
        ctx.SaveChanges();

        var svc = new BookingService(ctx);
        Assert.False(svc.CanAddPlayerToBooking(booking.BookingId));
    }

    // --- IsTeeTimeAvailable ---

    [Fact]
    public void IsTeeTimeAvailable_NoBookingExists_ReturnsTrue()
    {
        using var ctx = CreateContext();
        var svc = new BookingService(ctx);
        Assert.True(svc.IsTeeTimeAvailable(DateOnly.FromDateTime(DateTime.Today), new TimeOnly(9, 0)));
    }

    [Fact]
    public void IsTeeTimeAvailable_SlotAlreadyBooked_ReturnsFalse()
    {
        using var ctx = CreateContext();
        var today = DateOnly.FromDateTime(DateTime.Today);
        ctx.Members.Add(MakeMember(1));
        ctx.TeeTimeBookings.Add(new TeeTimeBooking
        {
            Date = today, TimeSlot = new TimeOnly(9, 0), BookedByMemberId = 1,
            Players = [new BookingPlayer { MemberId = 1, HandicapAtBooking = 10 }]
        });
        ctx.SaveChanges();

        var svc = new BookingService(ctx);
        Assert.False(svc.IsTeeTimeAvailable(today, new TimeOnly(9, 0)));
    }
}
