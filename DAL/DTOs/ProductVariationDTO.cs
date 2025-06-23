using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.DTOs
{
    internal class ProductVariationDTO
    {
        public int VariationId { get; set; }
        public decimal? ProductPrice { get; set; }
        public string? ProductVariationImage { get; set; }
        public long Quantity { get; set; }
    }
}
