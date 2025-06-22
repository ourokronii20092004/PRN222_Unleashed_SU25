using DAL.Models;
using System;
using System.Collections.Generic;

namespace DAL.DTOs
{
    public class ProductDTO
    {
        public Guid ProductId { get; set; }
        public int? BrandId { get; set; }
        public int? ProductStatusId { get; set; }
        public string? ProductName { get; set; }
        public string? ProductCode { get; set; }
        public string? ProductDescription { get; set; }
        public DateTimeOffset? CreatedAt { get; set; }
        public DateTimeOffset? UpdatedAt { get; set; }
        public List<ProductVariationDTO>? Variations { get; set; }
        public string? SaleType { get; set; }   
        public decimal? SaleValue { get; set; }
        public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();
        public virtual ICollection<Category> Categories { get; set; } = new List<Category>();
        public virtual ICollection<Sale> Sales { get; set; } = new List<Sale>();
        public class ProductVariationDTO
        {
            public int? SizeId { get; set; }
            public int? ColorId { get; set; }
            public decimal? ProductPrice { get; set; }
            public string? ProductVariationImage { get; set; }
        }

        public Product ToProduct()
        {
            return new Product
            {
                ProductId = ProductId,
                BrandId = BrandId,
                ProductStatusId = ProductStatusId,
                ProductName = ProductName,
                ProductCode = ProductCode,
                ProductDescription = ProductDescription,

            };

        }
    }
}
