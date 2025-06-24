using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.DTOs.StockDTOs
{
    public class StockTransactionDTO
    {
        [Required(ErrorMessage = "Stock ID is required.")]
        public int? StockId { get; set; }

        // This 'VariationId' at the top level of StockTransactionDTO in Java seems odd
        // given there's a List of variations below.
        // If it's a general context VariationId, keep it.
        // If it was a mistake and should be e.g. ProductId, adjust.
        // For now, I'll keep it as it was in Java DTO, but question its purpose.
        public int? VariationId { get; set; } // Potentially review its necessity/meaning

        [Required(ErrorMessage = "Provider ID is required.")]
        public int? ProviderId { get; set; }

        [Required(ErrorMessage = "Transaction type is required.")]
        public string? TransactionType { get; set; } // e.g., "IN", "OUT". Service will map this to TransactionType entity.

        [Required(ErrorMessage = "At least one variation must be specified.")]
        [MinLength(1, ErrorMessage = "At least one variation must be specified.")]
        public List<ProductVariationQuantityForTransactionDTO>? Variations { get; set; }

        [Required(ErrorMessage = "Username is required.")]
        public string? Username { get; set; } // For InchargeEmployee

        // Renamed inner class for clarity and to avoid name collision
        public class ProductVariationQuantityForTransactionDTO
        {
            [Required(ErrorMessage = "Product Variation ID is required.")]
            public int? ProductVariationId { get; set; } // This is the Variation.VariationId

            [Required(ErrorMessage = "Quantity is required.")]
            [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1.")]
            public int? Quantity { get; set; }
        }
    }
}