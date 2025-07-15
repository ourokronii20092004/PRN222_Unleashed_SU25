using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.DTOs.OderDTOs
{
    public class OrderDTO
    {
        public Guid OrderId { get; set; }
        public Guid UserId { get; set; }
        public int? OrderStatusId { get; set; }
        public decimal? OrderTotalAmount { get; set; }
        public DateTimeOffset? OrderDate { get; set; }
        // Navigation properties
        public User User { get; set; }
        public ICollection<OrderDetailDTO> OrderDetails { get; set; } = new List<OrderDetailDTO>();
        public List<OrderVariationSingleDTO> OrderItems { get; set; } = new();
    }
}
