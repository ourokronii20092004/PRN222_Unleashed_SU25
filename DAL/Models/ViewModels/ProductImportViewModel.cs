using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace DAL.Models.ViewModels
{
    public class ProductImportViewModel
    {
        public int StockId { get; set; }
        public string? StockName { get; set; }

        [Required(ErrorMessage = "Please select a provider.")]
        [Display(Name = "Provider")]
        public int ProviderId { get; set; }
        public List<SelectListItem> Providers { get; set; } = new List<SelectListItem>();

        public List<ProductVariationDetail> VariationsToImport { get; set; } = new List<ProductVariationDetail>();
    }

    public class ProductVariationDetail
    {
        public int VariationId { get; set; }
        public string? ProductName { get; set; }
        public string? BrandName { get; set; }
        public string? SizeName { get; set; }
        public string? ColorName { get; set; }

        [Display(Name = "Current Quantity")]
        public int CurrentQuantity { get; set; }

        [Display(Name = "Import Quantity")]
        [Range(0, int.MaxValue, ErrorMessage = "Quantity cannot be negative.")]
        public int ImportQuantity { get; set; }
    }
}