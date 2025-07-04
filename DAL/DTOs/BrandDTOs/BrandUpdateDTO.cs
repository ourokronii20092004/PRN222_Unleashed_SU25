using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.DTOs.BrandDTOs
{
    public class BrandUpdateDTO
    {
        public string BrandName { get; set; } = string.Empty;
        public string BrandDescription { get; set; } = string.Empty;
        public string BrandImageUrl { get; set; } = string.Empty;
        public string BrandWebsiteUrl { get; set; } = string.Empty;
        public IFormFile? BrandImageFile { get; set; }
    }
}
