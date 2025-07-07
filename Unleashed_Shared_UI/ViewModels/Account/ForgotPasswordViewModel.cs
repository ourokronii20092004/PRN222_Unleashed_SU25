using System.ComponentModel.DataAnnotations;

namespace Unleashed_Shared_UI.ViewModels.Account
{
    public class ForgotPasswordViewModel
    {
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Please enter a valid email address")]
        [Display(Name = "Your email")]
        public string Email { get; set; }
    }
}