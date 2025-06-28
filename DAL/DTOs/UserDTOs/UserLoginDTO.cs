using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.DTOs.UserDTOs
{
    public class UserLoginDTO
    {
        [Display(Name ="Username")]
        [Required(ErrorMessage = "Username cannot be empty.")]
        [MinLength(7, ErrorMessage = "Username must be longer than 7 characters.")]
        [RegularExpression(@"^[a-zA-Z0-9]*$", ErrorMessage = "Username cannot contain special characters or spaces.")]
        public string? UserUsername { get; set; }
        [Display(Name = "Password")]
        [Required(ErrorMessage = "Password cannot be empty.")]
        [MinLength(8, ErrorMessage = "Password must be at least 8 characters long.")]
        [DataType(DataType.Password)]
        public string? UserPassword { get; set; }
    }
}
