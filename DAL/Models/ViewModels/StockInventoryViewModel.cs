using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models.ViewModels
{
    public class StockInventoryViewModel
    {
        public int StockId { get; set; }
        public string? StockName { get; set; }
        public string? StockAddress { get; set; }
        public List<ProductInStockViewModel> ProductsInStock { get; set; } = new List<ProductInStockViewModel>();
    }

    public class ProductInStockViewModel
    {
        public Guid ProductId { get; set; }
        public string? ProductName { get; set; }
        public string? ProductCode { get; set; }
        public string? BrandName { get; set; }
        public List<VariationInStockViewModel> VariationsInStock { get; set; } = new List<VariationInStockViewModel>();
    }

    public class VariationInStockViewModel
    {
        public int VariationId { get; set; }
        public string? SizeName { get; set; }
        public string? ColorName { get; set; }
        public string? ColorHexCode { get; set; }
        public string? VariationImage { get; set; }
        public decimal? VariationPrice { get; set; }
        public int QuantityInStock { get; set; }
    }
}