using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Unleashed_RP.Pages.Authentication
{
    public class LogoutModel : PageModel
    {
        public void OnGet()
        {
        }
        public ActionResult OnPost()
        {
            HttpContext.Session.Clear();
            return RedirectToPage("../Index");
        }
    }
}
