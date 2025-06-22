using System;
using System.Collections.Generic;

namespace DAL.DTOs
{
    public class ProductDTO
    {
        public string ProductId { get; set; } = null!;
        public int? BrandId { get; set; }
        public int? ProductStatusId { get; set; }
        public string? ProductName { get; set; }
        public string? ProductCode { get; set; }
        public string? ProductDescription { get; set; }
        public DateTimeOffset? CreatedAt { get; set; }
        public DateTimeOffset? UpdatedAt { get; set; }
        public List<int>? CategoryIdList { get; set; }
        public decimal? ProductPrice { get; set; }
        public List<ProductVariationDTO>? Variations { get; set; }
        public string? SaleType { get; set; }   
        public decimal? SaleValue { get; set; }

        public class ProductVariationDTO
        {
            public int? SizeId { get; set; }
            public int? ColorId { get; set; }
            public decimal? ProductPrice { get; set; }
            public string? ProductVariationImage { get; set; }
        }
    }
}
