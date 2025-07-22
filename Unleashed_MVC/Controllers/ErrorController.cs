using Microsoft.AspNetCore.Mvc;

namespace Unleashed_MVC.Controllers
{
    public class ErrorController : Controller
    {
        [Route("/Error/Forbidden")]
        public IActionResult Forbidden()
        {
            return View();
        }

        [Route("/Error/NotFound")]
        public IActionResult NotFound()
        {
            return View();
        }
    }
}
