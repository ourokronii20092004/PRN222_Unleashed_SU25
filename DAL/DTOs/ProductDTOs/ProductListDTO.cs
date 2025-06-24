using DAL.Models;
using System;
using System.Collections.Generic;

namespace DAL.DTOs.ProductDTOs
{
    public class ProductListDTO
    {
        public string? ProductId { get; set; }
        public string? ProductName { get; set; }
        public string? ProductDescription { get; set; }

        public int BrandId { get; set; }
        public string? BrandName { get; set; }

        public List<Category>? CategoryList { get; set; }

        public string? ProductVariationImage { get; set; }
        public decimal ProductPrice { get; set; }

        public Sale? Sale { get; set; }
        public decimal? SaleValue { get; set; }

        public double? AverageRating { get; set; }
        public long? TotalRatings { get; set; }

        public int Quantity { get; set; }

        public List<Variation>? Variations { get; set; }


        public string? ProductCode { get; set; } 
        public DateTimeOffset? ProductCreatedAt { get; set; }  
        public DateTimeOffset? ProductUpdatedAt { get; set; }  
        public int? ProductStatusId { get; set; }  
    }
}
