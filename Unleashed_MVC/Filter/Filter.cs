using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;

namespace Unleashed_MVC.Filter
{
    public class Filter : Attribute, IAuthorizationFilter
    {
        public string[] RequiredRoles { get; set; } = [];
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            try
            {
                string? username = context.HttpContext.Session.GetString("username");
                if (string.IsNullOrEmpty(username))
                {
                    context.Result = new RedirectToActionResult("Login", "Authentication", null);
                    return;
                }

                if (!RequiredRoles.IsNullOrEmpty())
                {
                    string? currentRole = context.HttpContext.Session.GetString("role");

                    if (string.IsNullOrEmpty(currentRole) || !RequiredRoles.Contains(currentRole))
                    {
                        context.Result = new RedirectToActionResult("Forbidden", "Error", null);
                        return;
                    }
                }
            }
            catch (Exception)
            {
                context.Result = new RedirectToActionResult("Login", "Authentication", null);
                return;
            }
        }
    }
}
