using Microsoft.AspNetCore.Mvc;
using Unleashed_Shared_UI.ViewModels.Account;

namespace Unleashed_Shared_UI.ViewComponents.Account
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