using Microsoft.AspNetCore.Mvc;
using Unleashed_Shared_UI.ViewModels;

namespace Unleashed_Shared_UI.ViewComponents.Account
{
    public class ForgotPasswordViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            var model = new ForgotPasswordViewModel();
            return View(model);
        }
    }
}