using System.ComponentModel.DataAnnotations;

namespace Unleashed.Shared.UI.ViewModels
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Username is required")]
        [MinLength(7, ErrorMessage = "Username must be at least 7 characters")]
        [RegularExpression("^[a-zA-Z0-9]*$", ErrorMessage = "Username cannot contain special characters or spaces")]
        [Display(Name = "Username")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Full name is required")]
        [RegularExpression(@"^[\p{L} .'-]+$", ErrorMessage = "Full name can only contain letters and basic punctuation.")]
        [Display(Name = "Full Name")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Phone number is required")]
        [RegularExpression(@"^(?:\+84|0)\d{9,10}$", ErrorMessage = "Phone number must be a valid Vietnamese mobile number.")]
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [MinLength(8, ErrorMessage = "Password must be at least 8 characters")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{8,}$", ErrorMessage = "Password must contain at least one uppercase letter, one lowercase letter, and one number.")]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Please confirm your password")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        [Display(Name = "Confirm Password")]
        public string ConfirmPassword { get; set; }

        [Range(typeof(bool), "true", "true", ErrorMessage = "You must agree to the Terms of Use and Privacy Policy.")]
        [Display(Name = "I agree to the Terms of Use and Privacy Policy")]
        public bool AcceptTerms { get; set; }
    }
}