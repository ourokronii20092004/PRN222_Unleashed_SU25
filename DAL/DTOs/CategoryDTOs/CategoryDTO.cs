using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.DTOs.CategoryDTOs
{
    public class CategoryDTO
    {
        public int CategoryId { get; set; }

        public string? CategoryName { get; set; }

        public string? CategoryDescription { get; set; }

        public string? CategoryImageUrl { get; set; }

        public DateTimeOffset? CategoryCreatedAt { get; set; }

        public DateTimeOffset? CategoryUpdatedAt { get; set; }

    }
}
