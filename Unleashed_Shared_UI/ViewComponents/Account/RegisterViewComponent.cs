using Microsoft.AspNetCore.Mvc;
using Unleashed_Shared_UI.ViewModels;

namespace Unleashed_Shared_UI.ViewComponents.Account
{
    public class RegisterViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            var model = new RegisterViewModel();
            return View(model);
        }
    }
}