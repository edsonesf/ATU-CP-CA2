using GolfClub.Data;
using GolfClub.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace GolfClub.Pages.Members;

public class IndexModel(GolfClubContext context) : PageModel
{
    public IList<Member> Members { get; set; } = [];
    public string CurrentSort { get; set; } = "name";
    public string CurrentOrder { get; set; } = "asc";
    public string? CurrentGender { get; set; }
    public string? CurrentHandicap { get; set; }

    public async Task OnGetAsync(string sort = "name", string order = "asc", string? gender = null, string? handicap = null)
    {
        CurrentSort = sort;
        CurrentOrder = order;
        CurrentGender = gender;
        CurrentHandicap = handicap;

        var query = context.Members.AsQueryable();

        // Filter by gender
        if (!string.IsNullOrEmpty(gender) && Enum.TryParse<Gender>(gender, out var genderEnum))
            query = query.Where(m => m.Gender == genderEnum);

        // Filter by handicap range
        query = handicap switch
        {
            "below10"  => query.Where(m => m.Handicap <= 10),
            "11to20"   => query.Where(m => m.Handicap >= 11 && m.Handicap <= 20),
            "above20"  => query.Where(m => m.Handicap > 20),
            _          => query
        };

        // Sort
        query = (sort, order) switch
        {
            ("handicap", "desc") => query.OrderByDescending(m => m.Handicap),
            ("handicap", _)      => query.OrderBy(m => m.Handicap),
            (_, "desc")          => query.OrderByDescending(m => m.Name),
            _                    => query.OrderBy(m => m.Name)
        };

        Members = await query.ToListAsync();
    }
}
