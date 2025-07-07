using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Unleashed_Shared_UI.ViewModels.Menu;

namespace Unleashed_Shared_UI.ViewComponents.Menu
{
    public class LoggedMenuViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            // If the user isn't authenticated, render nothing.
            if (User?.Identity?.IsAuthenticated != true)
            {
                return Content(string.Empty);
            }

            // Find the user's profile picture URL from their claims.
            // "picture" is a standard claim name used by Google, etc.
            var pictureUrl = ((ClaimsPrincipal)User).FindFirst("picture")?.Value;

            // The default image to use if the user has no picture.
            string defaultImage = "/images/userdefault.webp";

            if (string.IsNullOrEmpty(pictureUrl))
            {
                pictureUrl = defaultImage;
            }
            // This is the C# equivalent of the cache-busting logic.
            // We append a timestamp to non-Google URLs to prevent stale images.
            else if (!pictureUrl.StartsWith("https://lh3.googleusercontent.com/"))
            {
                pictureUrl += $"?t={DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()}";
            }

            var model = new LoggedMenuViewModel
            {
                DisplayName = User.Identity.Name,
                ProfilePictureUrl = pictureUrl
            };

            return View(model);
        }
    }
}