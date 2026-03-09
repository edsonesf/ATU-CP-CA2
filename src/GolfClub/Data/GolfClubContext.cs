using Microsoft.EntityFrameworkCore;

namespace GolfClub.Data;

public class GolfClubContext(DbContextOptions<GolfClubContext> options) : DbContext(options)
{
}
