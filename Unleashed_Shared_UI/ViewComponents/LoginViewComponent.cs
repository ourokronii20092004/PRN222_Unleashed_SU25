using Microsoft.AspNetCore.Mvc;
using Unleashed_Shared_UI.ViewModels;

namespace Unleashed.Shared.UI.ViewComponents
{
    public class LoginViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            var model = new LoginViewModel();
            return View(model);
        }
    }
}