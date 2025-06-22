using DAL.Models;
using System.ComponentModel.DataAnnotations;

namespace DAL.DTOs.AccountDTOs
{
    public class RegisterAccountDTO
    {
        [Required(ErrorMessage = "Username cannot be empty.")]
        [MinLength(7, ErrorMessage = "Username must be longer than 7 characters.")]
        [RegularExpression(@"^[a-zA-Z0-9]*$", ErrorMessage = "Username cannot contain special characters or spaces.")]
        public string? Username { get; set; }

        [Required(ErrorMessage = "Password cannot be empty.")]
        [MinLength(8, ErrorMessage = "Password must be at least 8 characters long.")]
        [DataType(DataType.Password)] 
        public string? Password { get; set; }

        [Required(ErrorMessage = "Full Name cannot be empty.")]
        [RegularExpression(@"^[a-zA-Z0-9\s]*$", ErrorMessage = "Full Name cannot contain special characters.")]
        public string? Fullname { get; set; }
        public bool? Gender { get; set; }

        [Required(ErrorMessage = "Email cannot be empty.")]
        [EmailAddress(ErrorMessage = "Invalid Email Address format.")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Phone number cannot be empty.")]
        [RegularExpression(@"^0\d{9}$", ErrorMessage = "Phonenumber must be 10 digits and start with '0'.")]
        public string? Phone { get; set; }
        public DateOnly? Birthdate { get; set; }
        public string? Address { get; set; }
        public string? Image { get; set; }
        public string? CurrentPaymentMethod { get; set; }
        public User ToUser()
        {
            return new User {
            UserUsername = Username,
            UserPassword = Password,
            UserFullname = Fullname,
            Gender = Gender,
            UserEmail = Email,
            UserAddress = Address,
            UserBirthdate = Birthdate,
            UserPhone = Phone,
            UserImage = Image,
            UserCurrentPaymentMethod = CurrentPaymentMethod,
            };
                
                
        }
    }
}
