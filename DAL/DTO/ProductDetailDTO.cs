using DAL.Models;
using System;
using System.Collections.Generic;

namespace DAL.DTOs
{
    public class ProductDetailDTO
    {
        public string? ProductId { get; set; }
        public string? ProductName { get; set; }
        public string? ProductCode { get; set; }
        public string? ProductDescription { get; set; }

        public DateTimeOffset? ProductCreatedAt { get; set; }
        public DateTimeOffset? ProductUpdatedAt { get; set; }

        public Brand? Brand { get; set; }
        public ProductStatus? ProductStatusId { get; set; }

        public List<Category>? Categories { get; set; }
        public List<Variation>? ProductVariations { get; set; }
    }
}
