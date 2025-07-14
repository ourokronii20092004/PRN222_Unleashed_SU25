using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.DTOs.DiscountDTOs
{
    public class DiscountCreateDTO
    {
        [Required(ErrorMessage = "Discount code is required.")]
        [StringLength(50, MinimumLength = 6, ErrorMessage = "Discount code must be between 6 and 50 characters.")]
        [RegularExpression(@"^[a-zA-Z0-9]+$", ErrorMessage = "Discount code can only contain letters and numbers.")]
        public string DiscountCode { get; set; }

        [Required(ErrorMessage = "Discount value is required.")]
        [Range(0, double.MaxValue, ErrorMessage = "Value must be a positive number.")]
        public decimal DiscountValue { get; set; }

        [Required(ErrorMessage = "Discount type is required.")]
        public int DiscountTypeId { get; set; }

        [Required(ErrorMessage = "Discount status is required.")]
        public int DiscountStatusId { get; set; }

        public string? DiscountDescription { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Minimum order value must be a positive number.")]
        public decimal? DiscountMinimumOrderValue { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Maximum value must be a positive number.")]
        public decimal? DiscountMaximumValue { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Usage limit must be at least 1.")]
        public int? DiscountUsageLimit { get; set; }

        [Required(ErrorMessage = "Start date is required.")]
        public DateTimeOffset DiscountStartDate { get; set; }

        [Required(ErrorMessage = "End date is required.")]
        public DateTimeOffset DiscountEndDate { get; set; }
    }
}
