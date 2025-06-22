using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.DTO
{
    public class StockDetailDTO
    {
        public int StockId { get; set; }
        public string? StockName { get; set; }
        public string? StockAddress { get; set; }
        public int VariationId { get; set; } // Assuming int, matches Variation.VariationId
        public decimal? ProductPrice { get; set; } // Matches Variation.VariationPrice
        public string? ProductVariationImage { get; set; } // Matches Variation.VariationImage
        public string? ProductName { get; set; } // Matches Product.ProductName
        public Guid ProductId { get; set; } // Matches Product.ProductId (which is Guid)
        public int? BrandId { get; set; } // Matches Product.BrandId
        public string? BrandName { get; set; } // From related Brand
        public int? CategoryId { get; set; } // Assuming from related Category (via Product)
        public string? CategoryName { get; set; } // From related Category
        public string? SizeName { get; set; } // From related Size (via Variation)
        public string? ColorName { get; set; } // From related Color (via Variation)
        public string? HexCode { get; set; } // From related Color
        public int? Quantity { get; set; } // Matches StockVariation.StockQuantity
    }
}