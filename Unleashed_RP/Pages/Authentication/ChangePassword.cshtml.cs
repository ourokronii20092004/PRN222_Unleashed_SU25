using BLL.Services.Interfaces;
using DAL.Data;
using DAL.DTOs.UserDTOs;
using DAL.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Unleashed_RP.Pages.Authentication
{
    [Filter.Filter(RequiredRoles = new[] { "CUSTOMER" })]
    public class ChangePasswordModel : PageModel
    {
        private readonly IAuthenticationService _authenticationService ;

        public ChangePasswordModel(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService ;
        }

        [BindProperty]
        public ChangePassswordUserDTO ChangePassswordUserDTO { get; set; } = default!;

        public IActionResult OnGet()
        {
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            try
            {
                ArgumentNullException.ThrowIfNull(nameof(ChangePassswordUserDTO));
                if (!ChangePassswordUserDTO.CurrentPassword.Equals(ChangePassswordUserDTO.ConfirmPassword))
                {
                    ModelState.AddModelError("Error", "Invalid! current password and confirm password mismatch");
                    return Page();
                }
                string? username = HttpContext.Session.GetString("username");
                ArgumentNullException.ThrowIfNullOrEmpty(username);
                if (await _authenticationService.ChangePassword(username, ChangePassswordUserDTO))
                {
                    return RedirectToPage("../Index");
                }
                else {
                    ModelState.AddModelError("Error", "Invalid! current password is incorrect!");
                    return Page();
                };
            }
            catch (DbUpdateConcurrencyException)
            {
                ModelState.AddModelError("Error", "Can't update password!");
            }
            catch (Exception)
            {
                return NotFound();
            }

            return RedirectToPage("./Index");
            }

    }
}

