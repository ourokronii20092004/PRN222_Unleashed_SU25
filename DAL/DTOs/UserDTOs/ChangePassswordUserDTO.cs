using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.DTOs.UserDTOs
{
    public class ChangePassswordUserDTO
    {
        [Required(ErrorMessage = "Current password must not be empty!")]
        [Display(Name = "Current password") ]
        public string? CurrentPassword { get; set; }
        [Required(ErrorMessage = "Confirm password must not be empty!")]
        [Display(Name = "Confirm password")]
        public string? ConfirmPassword { get; set; }
        [Required(ErrorMessage = "New password must not be empty!")]
        [Display(Name = "New password")]
        public string? NewPassword { get; set; }
    }
}
