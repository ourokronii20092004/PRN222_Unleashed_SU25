using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.DTOs.ProductDTOs
{
    /// <summary>
    /// Represents the data needed for a simplified product import transaction
    /// into a specific stock location from a specific provider.
    /// </summary>
    public class ProductImportDTO
    {
        [Required]
        public int StockId { get; set; }

        [Required]
        public int ProviderId { get; set; }

        [Required(ErrorMessage = "At least one product variation must be specified.")]
        [MinLength(1, ErrorMessage = "At least one variation must be specified.")]
        public List<VariationQuantityDTO> Variations { get; set; } = new List<VariationQuantityDTO>();
    }

    /// <summary>
    /// A simple pairing of a variation ID and the quantity to import.
    /// </summary>
    public class VariationQuantityDTO
    {
        [Required]
        public int VariationId { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Import quantity must be at least 1.")]
        public int Quantity { get; set; }
    }
}
