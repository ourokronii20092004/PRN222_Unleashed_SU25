using DAL.Models;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;


namespace DAL.DTOs.AccountDTOs
{
    public class AccountDetailDTO
    {
        [Required(ErrorMessage = "Username cannot be empty.")]
        [MinLength(7, ErrorMessage = "Username must be longer than 7 characters.")]
        [RegularExpression(@"^[a-zA-Z0-9]*$", ErrorMessage = "Username cannot contain special characters or spaces.")]
        public string? Username { get; set; }
        [Required(ErrorMessage = "Full Name cannot be empty.")]
        [RegularExpression(@"^[a-zA-Z0-9\s]*$", ErrorMessage = "Full Name cannot contain special characters.")]
        public string? Fullname { get; set; }
        public virtual Role? Role { get; set; }
        public bool? Gender { get; set; }
        [Required(ErrorMessage = "Email cannot be empty.")]
        [EmailAddress(ErrorMessage = "Invalid Email Address format.")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Phonenumber cannot be empty.")]
        [RegularExpression(@"^0\d{9}$", ErrorMessage = "Phonenumber must be 10 digits and start with '0'.")]
        public string? Phone { get; set; }
        [Required]
        public DateOnly? BirthDate { get; set; }
        [Required]
        public string? Address { get; set; }
        [DisplayName("Image")]
        public string? ImageLink { get; set; }
        public string? CurrentPaymentMethod { get; set; }
        [DisplayName("Status")]
        [Required]
        public bool? IsEnabled { get; set; }
        [DisplayName("Created at")]
        public DateTimeOffset? CreatedDate { get; set; }
        [DisplayName("Updated At")]
        public DateTimeOffset? UpdatedDate { get; set; }

        public AccountDetailDTO() { }
        public static AccountDetailDTO FromUser(User user)
        {
            return new AccountDetailDTO
            {
                Username = user.UserUsername,
                Fullname = user.UserFullname,
                Gender = user.Gender,
                Role = user.Role,
                Email = user.UserEmail,
                Phone = user.UserPhone,
                BirthDate = user.UserBirthdate,
                Address = user.UserAddress,
                ImageLink = user.UserImage,
                CurrentPaymentMethod = user.UserCurrentPaymentMethod,
                IsEnabled = user.IsUserEnabled,
                CreatedDate = user.UserCreatedAt,
                UpdatedDate = user.UserUpdatedAt
            };
        }

        public User ToUser()
        {
            return new User
            {
                UserUsername = Username,
                UserFullname = Fullname,
                Gender = Gender,
                Role = Role,
                UserEmail = Email,
                UserPhone = Phone,
                UserAddress = Address,
                UserBirthdate = BirthDate,
                UserImage = ImageLink,
                UserCurrentPaymentMethod = CurrentPaymentMethod,
                IsUserEnabled = IsEnabled,
            };
        }

        
    }
}
