using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.DTO
{
    public class BrandUpdateDTO
    {
        [Required(ErrorMessage = "Brand name cannot be empty.")]
        [StringLength(255)]
        public string BrandName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Brand description cannot be empty.")]
        public string BrandDescription { get; set; } = string.Empty;

        [Required(ErrorMessage = "Brand image URL cannot be empty.")]
        [Url(ErrorMessage = "Please enter a valid image URL.")]
        public string BrandImageUrl { get; set; } = string.Empty;

        [Required(ErrorMessage = "Brand website URL cannot be empty.")]
        [Url(ErrorMessage = "Please enter a valid website URL.")]
        public string BrandWebsiteUrl { get; set; } = string.Empty;
    }
}
