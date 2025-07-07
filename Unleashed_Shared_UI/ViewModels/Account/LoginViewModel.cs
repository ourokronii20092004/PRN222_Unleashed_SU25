using System.ComponentModel.DataAnnotations;

namespace Unleashed_Shared_UI.ViewModels.Account
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Username can't be blank")]
        [Display(Name = "Username")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Display(Name = "Remember me")]
        public bool RememberMe { get; set; }
    }
}