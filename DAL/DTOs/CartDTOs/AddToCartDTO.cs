using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.DTOs.CartDTOs
{
    public class AddToCartDTO
    {
        public Guid UserId { get; set; }

        public int VariationId { get; set; }

        public int Quantity { get; set; }
    }
}
