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
    public class UserSideMenuViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            // Get the current controller and action from the ViewContext
            var currentController = ViewContext.RouteData.Values["controller"]?.ToString();
            var currentAction = ViewContext.RouteData.Values["action"]?.ToString();

            // Define the menu structure
            var menuItems = new List<SideMenuItemViewModel>
            {
                new() { Label = "User Information", Controller = "User", Action = "Information" },
                new() { Label = "My Orders", Controller = "User", Action = "Orders" },
                new() { Label = "Discounts", Controller = "User", Action = "Discounts" },
                new() { Label = "Membership", Controller = "User", Action = "Membership" },
                new() { Label = "History Reviews", Controller = "User", Action = "HistoryReviews" },
                new() { Label = "Wishlist", Controller = "User", Action = "WishList" },
            };

            // Loop through and set the active item
            foreach (var item in menuItems)
            {
                if (string.Equals(item.Controller, currentController, System.StringComparison.OrdinalIgnoreCase) &&
                    string.Equals(item.Action, currentAction, System.StringComparison.OrdinalIgnoreCase))
                {
                    item.IsActive = true;
                }
            }

            return View(menuItems);
        }
    }
}
