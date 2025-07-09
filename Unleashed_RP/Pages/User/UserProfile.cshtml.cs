using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using DAL.DTOs.UserDTOs;
using BLL.Services.Interfaces;

namespace Unleashed_RP.Pages.User
{

    [Filter.Filter(RequiredRoles = new[] { "CUSTOMER" })]
    public class UserProfileModel : PageModel
    {
        private readonly IUserService _userService;
        private readonly ILogger<UserProfileModel> _logger;

        public UserProfileModel(IUserService userService, ILogger<UserProfileModel> logger  )
        {
            _userService = userService;
            _logger = logger;
        }

        [BindProperty]
        public UserDetailDTO User { get; set; } = default!;
        [BindProperty]
        public IFormFile? NewUserImage { get; set; } = null;

        public async Task<IActionResult> OnGetAsync()
        {
            try
            {
            
                string? username = HttpContext.Session.GetString("username");
                ArgumentNullException.ThrowIfNullOrEmpty(username);
                var user = await _userService.GetUserByUsernameAsync(username);
                if (user == null)
                {
                    return NotFound();
                }
                else
                {
                    User = user;
                }
                return Page();
            }
            catch (Exception ex)
            {
                return RedirectToPage("../Index");
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                if (await _userService.EditUserAsync(User, NewUserImage))
                {
                    return await OnGetAsync();
                }

                return NotFound();
            }
            catch (Exception ex)
            {
                return NotFound();
            }
        }
    }
}
