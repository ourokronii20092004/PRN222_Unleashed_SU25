using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unleashed_Shared_UI.ViewComponents.Menu
{
    public class MinimalNavViewComponent : ViewComponent
    {
        // A returnUrl can be passed to make the back button smarter
        public IViewComponentResult Invoke(string returnUrl = "/")
        {
            return View((object)returnUrl);
        }
    }
}