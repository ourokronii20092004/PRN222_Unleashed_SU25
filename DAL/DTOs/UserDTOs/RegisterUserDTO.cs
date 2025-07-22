using DAL.Models;
using System.ComponentModel.DataAnnotations;

namespace DAL.DTOs.UserDTOs
{
    public class RegisterUserDTO
    {
        [Required(ErrorMessage = "Username cannot be empty.")]
        [MinLength(7, ErrorMessage = "Username must be longer than 7 characters.")]
        [RegularExpression(@"^[a-zA-Z0-9]*$", ErrorMessage = "Username cannot contain special characters or spaces.")]
        [Display(Name ="Username")]
        public string? UserUsername { get; set; }

        [Required(ErrorMessage = "Password cannot be empty.")]
        [MinLength(8, ErrorMessage = "Password must be at least 8 characters long.")]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string? UserPassword { get; set; }

        [Required(ErrorMessage = "Full Name cannot be empty.")]
        [RegularExpression(@"^[a-zA-Z0-9\s]*$", ErrorMessage = "Full Name cannot contain special characters.")]
        [Display(Name ="Fullname")]
        public string? UserFullname { get; set; }
        public bool? Gender { get; set; }

        [Required(ErrorMessage = "Email cannot be empty.")]
        [EmailAddress(ErrorMessage = "Invalid Email Address format.")]
        [Display(Name ="Email")]
        public string? UserEmail { get; set; }

        [Required(ErrorMessage = "Phone number cannot be empty.")]
        [RegularExpression(@"^0\d{9}$", ErrorMessage = "Phonenumber must be 10 digits and start with '0'.")]
        [Display(Name ="Phone Number")]
        public string? UserPhone { get; set; }
        [Display(Name ="Birthdate")]
        public DateOnly? UserBirthdate { get; set; }
        [Display(Name ="Address")]
        public string? UserAddress { get; set; }
        [Display(Name = "Image")]
        public string? UserImage { get; set; }
        [Display(Name = "Current Payment Method")]
        public string? UserCurrentPaymentMethod { get; set; }
       
    }
}
