using GolfClub.Data;
using GolfClub.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace GolfClub.Pages.Members;

public class EditModel(GolfClubContext context) : PageModel
{
    [BindProperty]
    public Member Member { get; set; } = new();

    public async Task<IActionResult> OnGetAsync(int id)
    {
        var member = await context.Members.FindAsync(id);
        if (member is null) return NotFound();
        Member = member;
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid) return Page();

        context.Members.Update(Member);
        await context.SaveChangesAsync();
        return RedirectToPage("Index");
    }
}
