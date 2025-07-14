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
        public Variation Variation { get; set; }
        public int Quantity { get; set; }
        public int StockQuantity { get; set; }
    }
}
