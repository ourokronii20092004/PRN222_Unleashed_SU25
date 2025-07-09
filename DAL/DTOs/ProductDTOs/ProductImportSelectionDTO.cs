using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.DTOs.ProductDTOs
{
    /// <summary>
    /// A rich DTO containing detailed information for the product selection step of the import workflow.
    /// </summary>
    public class ProductImportSelectionDTO
    {
        public Guid ProductId { get; set; }
        public string? ProductName { get; set; }
        public string? BrandName { get; set; }
        public string? FirstVariationImageUrl { get; set; }
        public string? CategoryNames { get; set; }
        public decimal? FirstVariationPrice { get; set; }
        public int? QuantityInStock { get; set; }
    }
}
