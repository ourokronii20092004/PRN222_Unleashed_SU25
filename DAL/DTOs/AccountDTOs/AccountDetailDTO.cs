using DAL.Models;
using System.ComponentModel;


namespace DAL.DTOs.AccountDTOs
{
    public class AccountDetailDTO
    {
        public string? Username { get; set; }
        public string? Fullname { get; set; }
        public virtual Role? Role { get; set; }
        public string? Gender { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public DateOnly? BirthDate { get; set; }
        public string? Address { get; set; }
        [DisplayName("Image")]
        public string? ImageLink { get; set; }
        public string? CurrentPaymentMethod { get; set; }
        [DisplayName("Status")]
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
                Gender = user.Gender switch
                {
                    false => "Male" ,
                    true  => "Female",
                    _ => null 
                },
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
                Gender = Gender?.ToLower() switch
                {
                    "male" => false,
                    "female" => true,
                    _ => null
                },
                Role = Role,
                UserEmail = Email,
                UserPhone = Phone,
                UserAddress = Address,
                UserBirthdate = BirthDate,
                UserImage = ImageLink,
                UserCurrentPaymentMethod = CurrentPaymentMethod,
            };
        }

        
    }
}
