using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models.ViewModels
{
    /// <summary>
    /// The primary ViewModel for the Stock Details page.
    /// Holds the stock's information and a grouped list of its products.
    /// </summary>
    public class StockDetailsViewModel
    {
        public int StockId { get; set; }
        public string? StockName { get; set; }
        public string? StockAddress { get; set; }
        public List<ProductInventoryGroup> Products { get; set; } = new List<ProductInventoryGroup>();
    }

    /// <summary>
    /// Represents a group of variations belonging to a single product.
    /// </summary>
    public class ProductInventoryGroup
    {
        public Guid ProductId { get; set; }
        public string? ProductName { get; set; }
        public string? BrandName { get; set; }
        public List<VariationInventoryDetail> Variations { get; set; } = new List<VariationInventoryDetail>();
    }

    /// <summary>
    /// Represents a single product variation with its quantity in the current stock.
    /// </summary>
    public class VariationInventoryDetail
    {
        public int VariationId { get; set; }
        public string? SizeName { get; set; }
        public string? ColorName { get; set; }
        public int Quantity { get; set; }
    }
}
