using GolfClub.Models;
using Microsoft.EntityFrameworkCore;

namespace GolfClub.Data;

public class GolfClubContext(DbContextOptions<GolfClubContext> options) : DbContext(options)
{
    public DbSet<Member> Members => Set<Member>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Member>(e =>
        {
            e.HasIndex(m => m.MembershipNumber).IsUnique();
            e.HasIndex(m => m.Email).IsUnique();
            e.Property(m => m.Gender).HasConversion<string>();
        });
    }
}
