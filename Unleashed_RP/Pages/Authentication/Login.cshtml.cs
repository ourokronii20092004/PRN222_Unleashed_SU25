using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using DAL.Data;
using DAL.Models;
using DAL.DTOs.UserDTOs;
using BLL.Services.Interfaces;

namespace Unleashed_RP.Pages.Authentication
{
    public class LoginModel : PageModel
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly ILogger<LoginModel> _logger;

        public LoginModel(IAuthenticationService authenticationService, ILogger<LoginModel> logger)
        {
            _authenticationService = authenticationService;
            _logger = logger;
        }

        [BindProperty]
        public UserLoginDTO UserLoginDTO { get; set; } = default!;

        public  IActionResult OnGet()
        {
            return Page();
        }

        public async Task<IActionResult> OnPostAsync() {
            if (ModelState.IsValid)
            {
                var user = await _authenticationService.Login(UserLoginDTO);
                if (user != null) {
                    HttpContext.Session.SetString("username", user.UserUsername);
                    HttpContext.Session.SetString("role", user.Role.RoleName);
                    HttpContext.Session.SetString("fullname", user.UserFullname);
                    return RedirectToPage("/Index");
                }
                else
                {
                    ModelState.AddModelError("Invalid Login", "The username or password is invalid!");
                }                
            }
            return Page(); 
        }
    }
}
