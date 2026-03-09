using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace GolfClub.Pages.Bookings;

public class IndexModel : PageModel
{
    [BindProperty(SupportsGet = true)]
    public string? Date { get; set; }

    public IActionResult OnGet()
    {
        if (!string.IsNullOrEmpty(Date))
            return RedirectToPage("TeeSheet", new { date = Date });

        return Page();
    }
}
