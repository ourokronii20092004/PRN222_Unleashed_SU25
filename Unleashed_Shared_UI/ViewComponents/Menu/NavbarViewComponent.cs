using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unleashed_Shared_UI.ViewModels.Menu;

namespace Unleashed_Shared_UI.ViewComponents.Menu
{
    public class NavbarViewComponent : ViewComponent
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        // call cart service when cart is done
        // private readonly ICartService _cartService; 

        public NavbarViewComponent(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public IViewComponentResult Invoke()
        {
            var currentController = ViewContext.RouteData.Values["controller"]?.ToString();
            var currentAction = ViewContext.RouteData.Values["action"]?.ToString();
            var user = _httpContextAccessor.HttpContext?.User;

            var links = new List<NavigationLinkViewModel>
            {
                new() { Label = "Home", Controller = "Home", Action = "Index" },
                new() { Label = "Shop", Controller = "Shop", Action = "Index" },
                new() { Label = "About", Controller = "Home", Action = "About" },
                new() { Label = "Contact", Controller = "Home", Action = "Contact" },
            };

            foreach (var link in links)
            {
                if (string.Equals(link.Controller, currentController, System.StringComparison.OrdinalIgnoreCase) &&
                    string.Equals(link.Action, currentAction, System.StringComparison.OrdinalIgnoreCase))
                {
                    link.IsActive = true;
                }
            }

            var model = new NavbarViewModel
            {
                IsAuthenticated = user?.Identity?.IsAuthenticated ?? false,
                Links = links,
                CartItemCount = 0 // replace with _cartService.GetCartCount() or smth when cart is done pls
            };

            return View(model);
        }
    }
}