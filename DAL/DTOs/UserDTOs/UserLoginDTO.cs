using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.DTOs.UserDTOs
{
    internal class UserLoginDTO
    {
        [Required(ErrorMessage = "Username cannot be empty.")]
        [MinLength(7, ErrorMessage = "Username must be longer than 7 characters.")]
        [RegularExpression(@"^[a-zA-Z0-9]*$", ErrorMessage = "Username cannot contain special characters or spaces.")]
        public string? Username { get; set; }

        [Required(ErrorMessage = "Password cannot be empty.")]
        [MinLength(8, ErrorMessage = "Password must be at least 8 characters long.")]
        [DataType(DataType.Password)]
        public string? Password { get; set; }
    }
}
