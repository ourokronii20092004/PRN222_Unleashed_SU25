using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.DTOs.BrandDTOs
{
    public class BrandDTO
    {
        public int BrandId { get; set; }
        public string? BrandName { get; set; }
        public string? BrandDescription { get; set; }
        public string? BrandImageUrl { get; set; }
        public string? BrandWebsiteUrl { get; set; }
        public DateTimeOffset? BrandCreatedAt { get; set; }
        public DateTimeOffset? BrandUpdatedAt { get; set; }
        public int TotalQuantity { get; set; }
    }
}