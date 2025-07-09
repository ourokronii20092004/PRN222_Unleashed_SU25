using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.IdentityModel.Tokens;

namespace Unleashed_RP.Filter
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
                    context.Result = new RedirectToPageResult("../Index");
                    return;
                }
                string? username = context.HttpContext.Session.GetString("username");
                ArgumentException.ThrowIfNullOrEmpty(username, nameof(username));
                string? currentrole = context.HttpContext.Session.GetString("role");
                ArgumentException.ThrowIfNullOrEmpty(currentrole, nameof(currentrole));
                if(!RequiredRoles.IsNullOrEmpty() && !RequiredRoles.Contains(currentrole))
                {
                    context.Result = new RedirectToPageResult("./Authentication/Login");
                    return;
                }
            }
            catch (Exception)
            {
                context.Result = new RedirectToPageResult("../Index");
                return;
            }
           
        }
    }
}
