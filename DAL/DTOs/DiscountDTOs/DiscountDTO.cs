using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.DTOs.DiscountDTOs
{
    public class DiscountDTO
    {
        public int DiscountId { get; set; }
        public string DiscountCode { get; set; }
        public decimal DiscountValue { get; set; }
        public string? DiscountDescription { get; set; }
        public decimal? DiscountMinimumOrderValue { get; set; }
        public decimal? DiscountMaximumValue { get; set; }
        public int? DiscountUsageLimit { get; set; }
        public int DiscountUsageCount { get; set; }
        public DateTimeOffset DiscountStartDate { get; set; }
        public DateTimeOffset DiscountEndDate { get; set; }
        public DateTimeOffset DiscountCreatedAt { get; set; }
        public DateTimeOffset? DiscountUpdatedAt { get; set; }

        // Dữ liệu từ bảng liên quan
        public string DiscountStatusName { get; set; }
        public string DiscountTypeName { get; set; }
    }
}
