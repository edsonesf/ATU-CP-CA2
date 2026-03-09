using GolfClub.Data;
using GolfClub.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace GolfClub.Pages.Members;

public class CreateModel(GolfClubContext context) : PageModel
{
    [BindProperty]
    public Member Member { get; set; } = new();

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid) return Page();

        context.Members.Add(Member);
        await context.SaveChangesAsync();
        return RedirectToPage("Index");
    }
}
