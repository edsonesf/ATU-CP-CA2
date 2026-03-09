using GolfClub.Data;
using GolfClub.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace GolfClub.Pages.Members;

public class IndexModel(GolfClubContext context) : PageModel
{
    public IList<Member> Members { get; set; } = [];

    public async Task OnGetAsync()
    {
        Members = await context.Members.OrderBy(m => m.Name).ToListAsync();
    }
}
