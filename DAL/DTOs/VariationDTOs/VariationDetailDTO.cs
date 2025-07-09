using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.DTOs.VariationDTOs
{
    /// <summary>
    /// A DTO containing detailed information about a single product variation.
    /// </summary>
    public class VariationDetailDTO
    {
        public int VariationId { get; set; }
        public string? ProductName { get; set; }
        public string? BrandName { get; set; }
        public string? SizeName { get; set; }
        public string? ColorName { get; set; }
    }
}