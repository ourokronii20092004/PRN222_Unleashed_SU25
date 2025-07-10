using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.DTOs.CartDTOs
{
    public class CheckOutDTO
    {
        public Guid UserId { get; set; }

        public List<CartDTO> Items { get; set; } = new List<CartDTO>();

        public decimal? TotalAmount => Items.Sum(i => i.TotalPrice ?? 0);
    }
}
