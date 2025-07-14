using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.DTOs.ProviderDTOs
{
    public class ProviderCreateDTO
    {
        [Required(ErrorMessage = "Supplier name is required.")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Supplier name must be between 6 and 100 characters.")]
        [RegularExpression(@"^[a-zA-Z0-9 ]+$", ErrorMessage = "Supplier name cannot contain special characters.")]
        public string ProviderName { get; set; }

        public string? ProviderImageUrl { get; set; }

        [EmailAddress(ErrorMessage = "Invalid email format.")]
        public string? ProviderEmail { get; set; }

        [Required(ErrorMessage = "Phone number is required.")]
        [RegularExpression(@"^\d{10,11}$", ErrorMessage = "Phone number must be 10 or 11 digits and contain no other characters.")]
        public string? ProviderPhone { get; set; }

        public string? ProviderAddress { get; set; }
    }
}
