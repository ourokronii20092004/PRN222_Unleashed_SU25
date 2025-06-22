using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.DTO
{
    public class StockProductDTO
    {
        public int StockId { get; set; }
        public List<ProductVariationQuantityDTO>? Variations { get; set; }
        public class ProductVariationQuantityDTO
        {
            public int VariationId { get; set; }
            public int Quantity { get; set; }
        }
    }
}