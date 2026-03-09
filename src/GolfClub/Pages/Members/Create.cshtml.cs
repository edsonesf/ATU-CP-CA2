using GolfClub.Data;
using GolfClub.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace GolfClub.Pages.Members;

public class CreateModel(GolfClubContext context) : PageModel
{
    [BindProperty]
    public Member Member { get; set; } = new();

    public async Task<IActionResult> OnPostAsync()
    {
        Member.Email = Member.Email.ToLowerInvariant();
        ModelState.Remove("Member.MembershipNumber"); // auto-generated, not submitted

        if (!ModelState.IsValid) return Page();

        if (await context.Members.AnyAsync(m => m.Email == Member.Email))
        {
            ModelState.AddModelError("Member.Email", "A member with this email already exists.");
            return Page();
        }

        var maxNumber = (await context.Members
            .Select(m => m.MembershipNumber)
            .Where(n => n.StartsWith("ATU"))
            .ToListAsync())
            .Select(n => int.TryParse(n[3..], out var num) ? num : 0)
            .DefaultIfEmpty(0)
            .Max();

        Member.MembershipNumber = $"ATU{(maxNumber + 1):D3}";

        context.Members.Add(Member);
        await context.SaveChangesAsync();
        return RedirectToPage("Index");
    }
}
