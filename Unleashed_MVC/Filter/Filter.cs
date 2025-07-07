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
                if (context.HttpContext.Session == null)
                {
                    context.Result = new RedirectToActionResult("Login", "Authentication", null);
                    return;
                }
                string? username = context.HttpContext.Session.GetString("username");
                ArgumentException.ThrowIfNullOrEmpty(username, nameof(username));
                string? currentrole = context.HttpContext.Session.GetString("role");
                ArgumentException.ThrowIfNullOrEmpty(currentrole, nameof(currentrole));
                if(!RequiredRoles.IsNullOrEmpty() && !RequiredRoles.Contains(currentrole))
                {
                    context.Result = new RedirectToActionResult("Index", "Home", null);
                    return;
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
