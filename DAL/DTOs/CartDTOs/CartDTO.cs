using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.DTOs.CartDTOs
{
    public class CartDTO
    {
        public Guid UserId { get; set; }

        public int VariationId { get; set; }

        public int? Quantity { get; set; }

        public string ProductName { get; set; } = null!;

        public string? ImageUrl { get; set; }

        public string? ColorName { get; set; }

        public string? SizeName { get; set; }

        public decimal? Price { get; set; }

        public decimal? TotalPrice => Quantity.HasValue && Price.HasValue ? Quantity.Value * Price.Value : null;
    }
}
