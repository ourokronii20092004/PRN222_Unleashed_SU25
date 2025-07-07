using Microsoft.AspNetCore.Mvc;
using Unleashed.Shared.UI.ViewModels;

namespace Unleashed.Shared.UI.ViewComponents
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