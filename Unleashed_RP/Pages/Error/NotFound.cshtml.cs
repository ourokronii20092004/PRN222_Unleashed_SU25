using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Unleashed_RP.Pages.Error
{
    public class NotFoundModel : PageModel
    {
        public IActionResult OnGet()
        {
            return Page();
        }
    }
}
