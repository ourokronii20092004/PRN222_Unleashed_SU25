using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace Unleashed_MVC.Filter
{
    public class Filter : Attribute, IAuthorizationFilter
    {

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            if (context.HttpContext.Session == null)
            {
                context.Result = new RedirectToActionResult("Login", "Authentication", null);
                return;
            }
            var username = context.HttpContext.Session.GetString("username");
            if (string.IsNullOrEmpty(username))
            {
                context.Result = new RedirectToActionResult("Login", "Authentication", null);
                return;
            }
        }
    }
}
