using Microsoft.AspNetCore.Mvc;
using Unleashed_Shared_UI.ViewModels;

namespace Unleashed_Shared_UI.ViewComponents
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