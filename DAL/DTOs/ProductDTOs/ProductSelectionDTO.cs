using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.DTOs.ProductDTOs
{
    /// <summary>
    /// A lightweight DTO representing a product for selection lists.
    /// </summary>
    public class ProductSelectionDTO
    {
        public Guid ProductId { get; set; }
        public string? ProductName { get; set; }
        public string? BrandName { get; set; }
    }
}