using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BLL.Services.Interfaces;
using DAL.DTOs.UserDTOs;

namespace Unleashed_RP.Pages.Authentication
{
    public class RegisterModel : PageModel
    {
        private readonly IUserService _userService;

        public RegisterModel(IUserService userService)
        {
            _userService = userService;
        }

        public IActionResult OnGet()
        {
            try
            {
                if (HttpContext.Session.GetString("username") != null) return RedirectToPage("../Index"); ;
                return Page();
            }
            catch (Exception) {
                return RedirectToPage("../Index");
            }
        }

        [BindProperty]
        public RegisterUserDTO RegisterUserDTO { get; set; } = default!;

        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return Page();
                }
                ArgumentNullException.ThrowIfNullOrEmpty(RegisterUserDTO.UserUsername, nameof(RegisterUserDTO));
                var validate = await _userService.ValidationUserAsync(RegisterUserDTO.UserUsername);
                if (validate)
                {
                    ModelState.AddModelError(string.Empty, "Username has been existed, Please use a new username.");
                    return Page();
                }
                if (await _userService.AddUserAsync(RegisterUserDTO, 2, null))
                {
                    return RedirectToPage("/Index");
                }
                return Page();
            }
            catch (Exception)
            {
                ModelState.AddModelError(string.Empty, "Error while creating account.");
                return Page();
            }
        }
    }
}
