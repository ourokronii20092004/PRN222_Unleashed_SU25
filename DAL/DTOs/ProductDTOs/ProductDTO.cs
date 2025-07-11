using DAL.DTOs.ReviewDTOs;
using DAL.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Drawing;

namespace DAL.DTOs.ProductDTOs
{
    public class ProductDTO
    {
        public Guid ProductId { get; set; }

        [Required(ErrorMessage = "Brand is required.")]
        public int? BrandId { get; set; }
        [Required(ErrorMessage = "Product Status is required.")]
        public int? ProductStatusId { get; set; }

        [Required(ErrorMessage = "Product name cannot be empty.")]
        [StringLength(255, ErrorMessage = "Product name cannot exceed 255 characters.")]
        public string? ProductName { get; set; }

        [Required(ErrorMessage = "Product code cannot be empty.")]
        [StringLength(100, ErrorMessage = "Product code cannot exceed 100 characters.")]
        public string? ProductCode { get; set; }

        [StringLength(1000, ErrorMessage = "Product description cannot exceed 1000 characters.")]
        public string? ProductDescription { get; set; }

        public DateTimeOffset? CreatedAt { get; set; }
        public DateTimeOffset? UpdatedAt { get; set; }
        public List<ProductVariationDTO>? Variations { get; set; }

        public string? SaleType { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Sale value must be a positive number.")]
        public decimal? SaleValue { get; set; }

        public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();
        public virtual ICollection<Category> Categories { get; set; } = new List<Category>();
        public virtual ICollection<Sale> Sales { get; set; } = new List<Sale>();
        public class ProductVariationDTO

        {
            public Guid? ProductVariationId { get; set; }
            [Required(ErrorMessage = "Size is required.")]
            public int SizeId { get; set; }
            [Required(ErrorMessage = "Color is required.")]
            public int ColorId { get; set; }

            [Required(ErrorMessage = "Price is required.")]
            [Range(0.01, 999999999999.99, ErrorMessage = "Price must be between 0.01 and 999,999,999,999.99")]
            [Column(TypeName = "decimal(18,2)")]
            public decimal? ProductPrice { get; set; }

            public IFormFile ProductVariationFile { get; set; }
            public string? ProductVariationImageUrl { get; set; }
        }

        public Product ToProduct()
        {
            return new Product
            {
                BrandId = BrandId,
                ProductStatusId = ProductStatusId,
                ProductName = ProductName,
                ProductCode = ProductCode,
                ProductDescription = ProductDescription,
            };
        }
    }
}
