using DAL.Models;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;


namespace DAL.DTOs.UserDTOs
{
    public class UserDetailDTO
    {
        [Required(ErrorMessage = "Username cannot be empty.")]
        [MinLength(7, ErrorMessage = "Username must be longer than 7 characters.")]
        [RegularExpression(@"^[a-zA-Z0-9]*$", ErrorMessage = "Username cannot contain special characters or spaces.")]
        [Display(Name = "Username")]
        public string? UserUsername { get; set; }

        [Required(ErrorMessage = "Full Name cannot be empty.")]
        [RegularExpression(@"^[a-zA-Z0-9\s]*$", ErrorMessage = "Full Name cannot contain special characters.")]
        [Display(Name = "Fullname")]
        public string? UserFullname { get; set; }
        public bool? Gender { get; set; }
        public Role Role { get; set; }

        [Required(ErrorMessage = "Email cannot be empty.")]
        [EmailAddress(ErrorMessage = "Invalid Email Address format.")]
        [Display(Name = "Email")]
        public string? UserEmail { get; set; }

        [Required(ErrorMessage = "Phone number cannot be empty.")]
        [RegularExpression(@"^0\d{9}$", ErrorMessage = "Phonenumber must be 10 digits and start with '0'.")]
        [Display(Name = "Phonenumber")]
        public string? UserPhone { get; set; }
        [Display(Name = "Birthday")]
        public DateOnly? UserBirthdate { get; set; }
        [Display(Name = "Address")]
        public string? UserAddress { get; set; }
        [Display(Name = "Image")]
        public string? UserImage { get; set; }
        [Display(Name = "Current Paymennt Method")]
        public string? UserCurrentPaymentMethod { get; set; }
        [Display(Name = "Status")]
        public bool? IsUserEnabled { get; set; }
        [DisplayName("Created at")]
        public DateTimeOffset? UserCreatedAt { get; set; }
        [DisplayName("Updated At")]
        public DateTimeOffset? UserUpdatedAt { get; set; }
        
    }
}
