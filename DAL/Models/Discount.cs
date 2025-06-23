using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Models
{
    [Table("discount")]
    public class Discount
    {
        [Key]
        [Column("discount_id")]
        public int DiscountId { get; set; }

        [Display(Name = "Status")]
        [Column("discount_status_id")]
        public int DiscountStatusId { get; set; }

        [Display(Name = "Type")]
        [Column("discount_type_id")]
        public int DiscountTypeId { get; set; }

        public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
        public virtual ICollection<UserDiscount> UserDiscounts { get; set; } = new List<UserDiscount>();

        [Required]
        [Display(Name = "Discount Code")]
        [StringLength(50, MinimumLength = 6, ErrorMessage = "Discount code must be between 6 and 50 characters.")]
        [RegularExpression(@"^[a-zA-Z0-9]+$", ErrorMessage = "Discount code can only contain letters and numbers.")]
        [Column("discount_code")]
        public string DiscountCode { get; set; }

        [Required]
        [Display(Name = "Value")]
        [Range(0, double.MaxValue, ErrorMessage = "Value must be a positive number.")]
        [DisplayFormat(DataFormatString = "{0:F0}", ApplyFormatInEditMode = true)] // Thêm dòng này
        [Column("discount_value")]
        public decimal DiscountValue { get; set; }

        [Display(Name = "Description")]
        [Column("discount_description")]
        public string? DiscountDescription { get; set; }

        [Display(Name = "Minimum Order Value")]
        [Range(0, double.MaxValue, ErrorMessage = "Minimum order value must be a positive number.")]
        [DisplayFormat(DataFormatString = "{0:F0}", ApplyFormatInEditMode = true)] // Thêm dòng này
        [Column("discount_minimum_order_value")]
        public decimal? DiscountMinimumOrderValue { get; set; }

        [Display(Name = "Maximum Value")]
        [Range(0, double.MaxValue, ErrorMessage = "Maximum value must be a positive number.")]
        [DisplayFormat(DataFormatString = "{0:F0}", ApplyFormatInEditMode = true)] // Thêm dòng này
        [Column("discount_maximum_value")]
        public decimal? DiscountMaximumValue { get; set; }

        [Display(Name = "Usage Limit")]
        [Range(1, int.MaxValue, ErrorMessage = "Usage limit must be greater than 0.")]
        [Column("discount_usage_limit")]
        public int? DiscountUsageLimit { get; set; }

        [Required]
        [Display(Name = "Start Date")]
        [DataType(DataType.DateTime)]
        [Column("discount_start_date")]
        public DateTimeOffset DiscountStartDate { get; set; }

        [Required]
        [Display(Name = "End Date")]
        [DataType(DataType.DateTime)]
        [Column("discount_end_date")]
        public DateTimeOffset DiscountEndDate { get; set; }

        [Column("discount_created_at")]
        public DateTimeOffset DiscountCreatedAt { get; set; }

        [Column("discount_updated_at")]
        public DateTimeOffset? DiscountUpdatedAt { get; set; }

        [Display(Name = "Usage Count")]
        [Range(0, int.MaxValue, ErrorMessage = "Usage count cannot be negative.")]
        [Column("discount_usage_count")]
        public int DiscountUsageCount { get; set; }

        // Navigation properties
        [ForeignKey("DiscountStatusId")]
        public virtual DiscountStatus DiscountStatus { get; set; }

        [ForeignKey("DiscountTypeId")]
        public virtual DiscountType DiscountType { get; set; }
    }
}