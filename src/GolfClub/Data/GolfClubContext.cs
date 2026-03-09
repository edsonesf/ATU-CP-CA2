using GolfClub.Models;
using Microsoft.EntityFrameworkCore;

namespace GolfClub.Data;

public class GolfClubContext(DbContextOptions<GolfClubContext> options) : DbContext(options)
{
    public DbSet<Member> Members => Set<Member>();
    public DbSet<TeeTimeBooking> TeeTimeBookings => Set<TeeTimeBooking>();
    public DbSet<BookingPlayer> BookingPlayers => Set<BookingPlayer>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Member>(e =>
        {
            e.HasIndex(m => m.MembershipNumber).IsUnique();
            e.HasIndex(m => m.Email).IsUnique();
            e.Property(m => m.Gender).HasConversion<string>();
        });

        modelBuilder.Entity<TeeTimeBooking>(e =>
        {
            e.HasKey(b => b.BookingId);
            e.HasIndex(b => new { b.Date, b.TimeSlot }).IsUnique();
            e.Property(b => b.Date).HasConversion(
                d => d.ToDateTime(TimeOnly.MinValue),
                d => DateOnly.FromDateTime(d));
            e.Property(b => b.TimeSlot).HasConversion(
                t => t.ToTimeSpan(),
                t => TimeOnly.FromTimeSpan(t));
        });

        modelBuilder.Entity<BookingPlayer>(e =>
        {
            e.HasIndex(bp => new { bp.BookingId, bp.MemberId }).IsUnique();
        });
    }
}
