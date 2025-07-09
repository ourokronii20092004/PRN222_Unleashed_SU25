using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models.ViewModels
{
    public class SelectProductsViewModel
    {
        public int StockId { get; set; }
        public string? StockName { get; set; }
        public List<ProductSelectionItem> Products { get; set; } = new List<ProductSelectionItem>();
    }

    public class ProductSelectionItem
    {
        public Guid ProductId { get; set; }
        public string? ProductName { get; set; }
        public string? BrandName { get; set; }
        public bool IsSelected { get; set; }
        public string? FirstVariationImageUrl { get; set; }
        public string? CategoryNames { get; set; }
        public decimal? FirstVariationPrice { get; set; }
        public int? QuantityInStock { get; set; }

    }
}
