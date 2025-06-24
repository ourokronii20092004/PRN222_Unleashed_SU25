using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.DTOs.ProductDTOs
{
    public class ProductSearchResultDTO
    {
        public Guid ProductId { get; set; }
        public string? ProductName { get; set; }
        public string? ProductCode { get; set; }
        public string? ProductDescription { get; set; }
        public int? BrandId { get; set; }
        public string? BrandName { get; set; }
        public int? ProductStatusId { get; set; }
        public string? ProductStatusName { get; set; }
        public int? FirstVariationId { get; set; }
        public string? FirstVariationImage { get; set; }
        public decimal? FirstVariationPrice { get; set; }
        public string? FirstVariationSizeName { get; set; }
        public string? FirstVariationColorName { get; set; }
        public double AverageRating { get; set; }
        public int TotalRatings { get; set; }
    }
}