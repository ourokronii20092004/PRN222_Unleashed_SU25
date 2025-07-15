using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.DTOs.OderDTOs
{
    public class OrderDetailDTO
    {
        public Guid OrderId { get; set; }
        public int VariationSingleId { get; set; }
        public decimal VariationPriceAtPurchase { get; set; }
        public int Quantity { get; set; } // Cần thêm trường này

        // Navigation properties
        public Order Order { get; set; }
        public Variation Variation { get; set; }
    }
}
