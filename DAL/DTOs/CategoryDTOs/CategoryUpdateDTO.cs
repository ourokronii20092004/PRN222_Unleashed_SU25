using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.DTOs.CategoryDTOs
{
    public class CategoryUpdateDTO
    {
        public string? CategoryName { get; set; } = string.Empty;

        public string? CategoryDescription { get; set; } = string.Empty ;

        public string? CategoryImageUrl { get; set; } = string.Empty;

        public IFormFile? CategoryImageFile { get; set; } 

        public DateTimeOffset? CategoryUpdatedAt { get; set; }
    }
}
