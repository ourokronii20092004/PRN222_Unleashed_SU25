using AspNetCoreGeneratedDocument;
using BLL.Services.Interfaces;
using DAL.DTOs.UserDTOs;
using Microsoft.AspNetCore.Mvc;


namespace Unleashed_MVC.Controllers
{
    public class AuthenticationController : Controller
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly ILogger<AuthenticationController> _logger;

        public AuthenticationController(IAuthenticationService authenticationService, ILogger<AuthenticationController> logger)
        {
            _authenticationService = authenticationService;
            _logger = logger;
        }
        // GET: AuthenticationController
        public ActionResult Login()
        {
            return View();
        }

       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(UserLoginDTO loginInfor)
        {
            
            {
                if (ModelState.IsValid)
                {
                    var user = await _authenticationService.Login(loginInfor);
                    _logger.LogInformation("user: " + (user == null? null : user.Role.RoleName));
                    if (user != null)
                    {
                        HttpContext.Session.SetString("username", user.UserUsername);
                        HttpContext.Session.SetString("role", user.Role.RoleName);
                        return RedirectToAction("Index", "Home");
                    }
                }
                ModelState.AddModelError(string.Empty, "Invalid Username or Password");
                loginInfor.UserPassword = null;
                return View(loginInfor);
            }
           
        }

    }
}
